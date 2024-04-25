using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Abstract.Storages;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.DataAccess.Models.Identity;
using CodinaxProjectMvc.Managers.Abstract;
using CodinaxProjectMvc.ViewModel;
using CodinaxProjectMvc.ViewModel.InstructorVm;
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

        public async Task<PaginationVm<IEnumerable<Student>>> GetStudentsPaginationAsync(string? searchFilter = null, string? statusFilter = null, int page = 0, int size = 10)
        {
            var query = _studentReadRepository.GetAll().Include(x => x.Courses).AsQueryable();

            int take = size;
            int skip = (page - 1) * take;

            if (!string.IsNullOrEmpty(searchFilter))
            {
                searchFilter = searchFilter.ToLower();
                query = query.Where(x => x.UserName.ToLower().Contains(searchFilter) ||
                    x.FirstName.ToLower().Contains(searchFilter) ||
                    x.LastName.ToLower().Contains(searchFilter) ||
                    x.Email.ToLower().Contains(searchFilter));
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

            var paginatedData = students.Skip(skip).Take(take).ToList();

            PaginationVm<IEnumerable<Student>> pagination = new PaginationVm<IEnumerable<Student>>(total, page, (int)Math.Ceiling((decimal)total / take), paginatedData, take);

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

        public async Task<bool> SendConfirmationMailAsync(Guid id)
        {
            Student? student = await _studentReadRepository.GetSingleAsync(x => x.Id == id);
            if (student == null) return false;

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(student);

            await _mailManager.SendConfirmationMailAsync(token, student);

            return true;
        }
    }
}
