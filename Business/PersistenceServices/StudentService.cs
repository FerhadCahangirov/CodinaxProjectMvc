using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Abstract.Storages;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.DataAccess.Models.Identity;
using CodinaxProjectMvc.Managers.Abstract;
using CodinaxProjectMvc.ViewModel;
using CodinaxProjectMvc.ViewModel.ApplicationVm;
using CodinaxProjectMvc.ViewModel.CourseVm;
using CodinaxProjectMvc.ViewModel.StudentVm;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Business.PersistenceServices
{
    public class StudentService : IStudentService
    {
        private readonly IReadRepository<Student> _studentReadRepository;
        private readonly IWriteRepository<Student> _studentWriteRepository;
        private readonly IConfiguration _configuration;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IAzureStorage _storage;
        private readonly IMailManager _mailManager;
        private readonly UserManager<AppUser> _userManager;

        public StudentService(IReadRepository<Student> studentReadRepository, IWriteRepository<Student> studentWriteRepository, IConfiguration configuration, IActionContextAccessor actionContextAccessor, IAzureStorage storage, IMailManager mailManager, UserManager<AppUser> userManager)
        {
            _studentReadRepository = studentReadRepository;
            _studentWriteRepository = studentWriteRepository;
            _configuration = configuration;
            _actionContextAccessor = actionContextAccessor;
            _storage = storage;
            _mailManager = mailManager;
            _userManager = userManager;
        }

        public async Task<bool> ApproveAsync(Guid id)
        {
            Student? student = await _studentReadRepository.GetSingleAsync(x => x.Id == id);

            if (student == null) return false;

            student.IsApproved = true;

            _studentWriteRepository.Update(student);
            await _studentWriteRepository.SaveAsync();

            string token = await _userManager.GeneratePasswordResetTokenAsync(student);

            await _mailManager.SendPasswordSetupMailAsync(token, student);

            return true;
        }

        public async Task<bool> BanAsync(Guid id)
        {
            Student? student = await _studentReadRepository.GetSingleAsync(x => x.Id == id);

            if (student == null) return false;

            student.IsBanned = true;

            _studentWriteRepository.Update(student);
            await _studentWriteRepository.SaveAsync();
            return true;
        }

        public async Task<bool> ChangeProfileImageAsync(Guid id, IFormFile file)
        {

            Student? student = await _studentReadRepository.GetSingleAsync(x => x.Id == id);
            if (student == null) return false;

            if (!string.IsNullOrEmpty(student.ProfileImagePathOrContainer) && !string.IsNullOrEmpty(student.ProfileImageName))
            {
                await _storage.DeleteAsync(student.ProfileImagePathOrContainer, student.ProfileImageName);
            }

            IFormFileCollection files = new FormFileCollection
            {
                file
            };

            List<(string fileName, string pathOrContainerName)> uploadedFiles = await _storage.UploadAsync(AzureContainerNames.StudentProfileImages, files);

            (string fileName, string pathOrContainerName) uploadedFile = uploadedFiles[0];

            student.ProfileImageName = uploadedFile.fileName;
            student.ProfileImagePathOrContainer = uploadedFile.pathOrContainerName;

            _studentWriteRepository.Update(student);
            await _studentWriteRepository.SaveAsync();

            return true;

        }

        public async Task<bool> DeleteProfileImageAsync(Guid id)
        {
            Student? student = await _studentReadRepository.GetSingleAsync(x => x.Id == id);
            if (student == null) return false;

            if (!string.IsNullOrEmpty(student.ProfileImagePathOrContainer) && !string.IsNullOrEmpty(student.ProfileImageName))
            {
                await _storage.DeleteAsync(student.ProfileImagePathOrContainer, student.ProfileImageName);

                student.ProfileImageName = null;
                student.ProfileImagePathOrContainer = null;

                _studentWriteRepository.Update(student);
                await _studentWriteRepository.SaveAsync();
            }
            return true;
        }

        public async Task<StudentAccountVm> GetProfileDataAsync(Guid id)
        {
            Student? student = await _studentReadRepository.GetSingleAsync(x => x.Id == id);
            if (student == null) return new StudentAccountVm();

            StudentAccountVm studentAccountVm = student.FromStudent_ToStudentAccountVm();
            studentAccountVm.BaseUrl = _configuration["BaseUrl:Azure"];

            return studentAccountVm;
        }

        public async Task<StudentAccountVm> GetProfileDataAsync()
        {
            Student? student = await _studentReadRepository.GetSingleAsync(x => x.Email == _actionContextAccessor.ActionContext.HttpContext.User.Identity.Name);
            if (student == null) return new StudentAccountVm();

            StudentAccountVm studentAccountVm = student.FromStudent_ToStudentAccountVm();
            studentAccountVm.BaseUrl = _configuration["BaseUrl:Azure"];

            return studentAccountVm;
        }


        public async Task<PaginationVm<IEnumerable<Student>>> GetStudentsPartialAsync(string? searchFilter = null, string? statusFilter = null)
        {
            var query = _studentReadRepository.GetWhere(x => !x.IsDeleted).Include(x => x.Courses).AsQueryable();

            if (!string.IsNullOrEmpty(searchFilter))
            {
                searchFilter = searchFilter.ToLower().Replace(" ", "");
                query = query.Where(x => x.UserName.ToLower().Contains(searchFilter) ||
                    x.FirstName.ToLower().Replace(" ", "").Contains(searchFilter) ||
                    x.LastName.ToLower().Replace(" ", "").Contains(searchFilter) ||
                    x.Email.ToLower().Replace(" ", "").Contains(searchFilter) ||
                    (x.FirstName.ToLower() + x.LastName.ToLower()).Contains(searchFilter));
            }

            if (!string.IsNullOrEmpty(statusFilter))
            {
                statusFilter = statusFilter.ToLower();
                if (statusFilter == "banned")
                    query = query.Where(x => x.IsBanned == true);
                else if (statusFilter == "approved")
                    query = query.Where(x => x.IsApproved == true && x.IsBanned == false);
                else if (statusFilter == "not approved")
                    query = query.Where(x => !x.IsApproved);
            }

            var students = await query.ToListAsync();

            int total = await query.CountAsync();

            var paginatedData = students.ToList();

            PaginationVm<IEnumerable<Student>> pagination = new PaginationVm<IEnumerable<Student>>(paginatedData);

            pagination.BaseUrl = _configuration[ConfigurationStrings.AzureBaseUrl];

            return pagination;
        }

        public async Task<bool> UnbanAsync(Guid id)
        {
            Student? student = await _studentReadRepository.GetSingleAsync(x => x.Id == id);

            if (student == null) return false;

            student.IsBanned = false;

            _studentWriteRepository.Update(student);
            await _studentWriteRepository.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            Student? student = await _studentReadRepository.GetSingleAsync(x => x.Id == id);

            if (student == null) return false;

            student.IsDeleted = true;

            _studentWriteRepository.Update(student);
            await _studentWriteRepository.SaveAsync();

            return true;
        }

        public async Task<bool> UpdateProfileAsync(StudentAccountVm studentAccountVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
                return false;

            Student? student = await _studentReadRepository.GetSingleAsync(x => x.Id == studentAccountVm.Id);
            if (student == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(StudentAccountVm.Id), "Student Not Found");
                return false;
            }

            student.FirstName = studentAccountVm.FirstName;
            student.LastName = studentAccountVm.LastName;
            student.PhoneNumber = studentAccountVm.PhoneNumber;
            student.AdditionalNotes = studentAccountVm.AdditionalNotes;
            student.CountryOfBirth = studentAccountVm.CountryOfBirth;
            student.CountryOfResidence = studentAccountVm.CountryOfResidence;

            if (!studentAccountVm.IsEmailConfirmed)
            {
                AppUser? user = await _userManager.FindByEmailAsync(studentAccountVm.EmailAddress);
                if (user != null)
                {
                    _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(StudentAccountVm.EmailAddress), "Email Already Exists!");
                    return false;
                }

                student.Email = studentAccountVm.EmailAddress;
                student.NormalizedEmail = studentAccountVm.EmailAddress.Normalize();
            }

            _studentWriteRepository.Update(student);
            await _studentWriteRepository.SaveAsync();

            return true;
        }

        public async Task<bool> UploadProfileImageAsync(Guid id, IFormFile file)
        {

            Student? student = await _studentReadRepository.GetSingleAsync(x => x.Id == id);
            if (student == null) return false;

            FormFileCollection files = new FormFileCollection
            {
                file
            };

            List<(string fileName, string pathOrContainerName)> uploadedFiles = await _storage.UploadAsync(AzureContainerNames.StudentProfileImages, files);

            (string fileName, string pathOrContainerName) uploadedFile = uploadedFiles[0];

            student.ProfileImageName = uploadedFile.fileName;
            student.ProfileImagePathOrContainer = uploadedFile.pathOrContainerName;

            _studentWriteRepository.Update(student);
            await _studentWriteRepository.SaveAsync();
            return true;
        }
        #region Mail Services
        public async Task<bool> SendConfirmationMailAsync(Guid id)
        {
            Student? student = await _studentReadRepository.GetSingleAsync(x => x.Id == id);
            if (student == null) return false;

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(student);

            await _mailManager.SendConfirmationMailAsync(token, student);

            return true;
        }

        public async Task<bool> SendPasswordGenerateMailAsync(Guid id)
        {
            Student? student = await _studentReadRepository.GetSingleAsync(x => x.Id == id);
            if (student == null) return false;

            if (string.IsNullOrEmpty(student.PasswordHash))
            {
                string token = await _userManager.GeneratePasswordResetTokenAsync(student);

                await _mailManager.SendPasswordSetupMailAsync(token, student);
            }

            return true;
        }

        #endregion

        public async Task<bool> ChangePasswordAsync(StudentResetPasswordVm studentResetPasswordVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
            {
                return false;
            }

            AppUser? user = await _userManager.FindByIdAsync(studentResetPasswordVm.Id.ToString());
            if (user == null) return false;

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            await _userManager.ResetPasswordAsync(user, token, studentResetPasswordVm.Password);

            return true;
        }

        public Task<PaginationVm<IEnumerable<Student>>> GetAssignableStudentPaginationAsync(string? searchFilter = null)
        {
            var query = _studentReadRepository.Table.Where(UserQueryFilters<Student>.GeneralFilter);

            if (!string.IsNullOrEmpty(searchFilter))
            {
                searchFilter = searchFilter.ToLower();
                query = query.Where(x => x.UserName.ToLower().Contains(searchFilter) ||
                    x.FirstName.ToLower().Contains(searchFilter) ||
                    x.LastName.ToLower().Contains(searchFilter) ||
                    x.Email.ToLower().Contains(searchFilter));
            }

            var paginatedData = query.ToList();

            PaginationVm<IEnumerable<Student>> pagination = new PaginationVm<IEnumerable<Student>>(paginatedData);

            pagination.BaseUrl = _configuration[ConfigurationStrings.AzureBaseUrl];

            return Task.FromResult(pagination);
        }

        #region Student Panel Services

        public async Task<IEnumerable<Course>> GetStudentCoursesAsync(string email)
        {
            Student? student = await _studentReadRepository.Table
                .Include(x => x.Courses)
                .ThenInclude(x => x.Modules)
                .FirstOrDefaultAsync(x => x.Email == email);

            if (student == null) return Enumerable.Empty<Course>();

            return student.Courses;
        }

        public async Task<CourseSingleVm> GetStudentCourseSingleAsync(string email, Guid id)
        {
            Student? student = await _studentReadRepository.Table
                .Include(x => x.Courses).ThenInclude(x => x.Template)
                .Include(x => x.Courses).ThenInclude(x => x.Modules).ThenInclude(x => x.Bookmarks)
                .Include(x => x.Courses).ThenInclude(x => x.Modules).ThenInclude(x => x.Lectures)
                .FirstOrDefaultAsync(x => x.Email == email);

            Course? course = student?.Courses.FirstOrDefault(x => x.Id == id);

            if (course == null) return new CourseSingleVm();

            CourseSingleVm courseSingleVm = new CourseSingleVm()
            {
                BaseUrl = _configuration[ConfigurationStrings.AzureBaseUrl],
                Course = course,
                Template = course.Template,
            };

            return courseSingleVm;
        }

        public async Task<ApplicationListVm> ListApplicationsAsync()
        {
            Student student = await _studentReadRepository.Table
                    .Include(x => x.Courses)
                    .ThenInclude(x => x.Applications)
                    .FirstAsync(x => x.Email == _actionContextAccessor.ActionContext.HttpContext.User.Identity.Name);

            return new()
            {
                Applications = student.Courses
                .Where(EntityQueryFilters<Course>.LayoutFilter)
                .SelectMany(x => x.Applications).Where(EntityQueryFilters<Application>.LayoutFilter),
                BaseUrl = _configuration[ConfigurationStrings.AzureBaseUrl],
            };
        }

        #endregion
    }
}
