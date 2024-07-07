using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Abstract.Storages;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.DataAccess.Models.Identity;
using CodinaxProjectMvc.Managers.Abstract;
using CodinaxProjectMvc.ViewModel;
using CodinaxProjectMvc.ViewModel.CourseVm;
using CodinaxProjectMvc.ViewModel.InstructorVm;
using CodinaxProjectMvc.ViewModel.StudentVm;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Business.PersistenceServices
{
    public class InstructorService : IInstructorService
    {
        private readonly IReadRepository<Instructor> _instructorReadRepository;
        private readonly IWriteRepository<Instructor> _instructorWriteRepository;
        private readonly IConfiguration _configuration;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IAzureStorage _storage;
        private readonly IMailManager _mailManager;
        private readonly UserManager<AppUser> _userManager;

        public InstructorService(IReadRepository<Instructor> instructorReadRepository, IConfiguration configuration, IWriteRepository<Instructor> instructorWriteRepository, IActionContextAccessor actionContextAccessor, IAzureStorage storage, IMailManager mailManager, UserManager<AppUser> userManager)
        {
            _instructorReadRepository = instructorReadRepository;
            _configuration = configuration;
            _instructorWriteRepository = instructorWriteRepository;
            _actionContextAccessor = actionContextAccessor;
            _storage = storage;
            _mailManager = mailManager;
            _userManager = userManager;
        }

        public async Task<PaginationVm<IEnumerable<Instructor>>> GetInstructorsPartialAsync(string? searchFilter = null, string? statusFilter = null)
        {
            var query = _instructorReadRepository.GetWhere(x => !x.IsDeleted).Include(x => x.Courses).AsQueryable();

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

            var instructors = await query.OrderByDescending(x => x.Showcase).ToListAsync();

            var paginatedData = instructors.ToList();

            PaginationVm<IEnumerable<Instructor>> pagination = new PaginationVm<IEnumerable<Instructor>>(paginatedData);

            pagination.BaseUrl = _configuration[ConfigurationStrings.AzureBaseUrl];

            return pagination;
        }

        public Task<PaginationVm<IEnumerable<Instructor>>> GetAssignableInstructorsPartialAsync(string? searchFilter = null)
        {
            var query = _instructorReadRepository.Table.Where(UserQueryFilters<Instructor>.GeneralFilter);

            if (!string.IsNullOrEmpty(searchFilter))
            {
                searchFilter = searchFilter.ToLower();
                query = query.Where(x => x.UserName.ToLower().Contains(searchFilter) ||
                    x.FirstName.ToLower().Contains(searchFilter) ||
                    x.LastName.ToLower().Contains(searchFilter) ||
                    (x.FirstName.ToLower() + x.LastName.ToLower()).Trim().Replace(" ", "").Contains(searchFilter.Trim().Replace(" ", "")) ||
                    x.Email.ToLower().Contains(searchFilter));
            }

            var paginatedData = query.ToList();

            PaginationVm<IEnumerable<Instructor>> pagination = new PaginationVm<IEnumerable<Instructor>>(paginatedData);

            pagination.BaseUrl = _configuration[ConfigurationStrings.AzureBaseUrl];

            return Task.FromResult(pagination);
        }

        public async Task<bool> ApproveAsync(Guid id)
        {
            Instructor? instructor = await _instructorReadRepository.GetSingleAsync(x => x.Id == id);

            if (instructor == null) return false;

            instructor.IsApproved = true;

            _instructorWriteRepository.Update(instructor);
            await _instructorWriteRepository.SaveAsync();

            string token = await _userManager.GeneratePasswordResetTokenAsync(instructor);

            await _mailManager.SendPasswordSetupMailAsync(token, instructor);

            return true;
        }

        public async Task<bool> BanAsync(Guid id)
        {
            Instructor? instructor = await _instructorReadRepository.GetSingleAsync(x => x.Id == id);

            if (instructor == null) return false;

            instructor.IsBanned = true;
            instructor.Showcase = false;

            _instructorWriteRepository.Update(instructor);
            await _instructorWriteRepository.SaveAsync();

            return true;
        }

        public async Task<bool> UnbanAsync(Guid id)
        {

            Instructor? instructor = await _instructorReadRepository.GetSingleAsync(x => x.Id == id);

            if (instructor == null) return false;

            instructor.IsBanned = false;

            _instructorWriteRepository.Update(instructor);
            await _instructorWriteRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            Instructor? instructor = await _instructorReadRepository.GetSingleAsync(x => x.Id == id);

            if (instructor == null) return false;

            instructor.IsDeleted = true;
            instructor.Showcase = false;

            _instructorWriteRepository.Update(instructor);
            await _instructorWriteRepository.SaveAsync();

            return true;
        }

        public async Task<InstructorAccountVm> GetProfileDataAsync(Guid id)
        {
            Instructor? instructor = await _instructorReadRepository.GetSingleAsync(x => x.Id == id);
            if (instructor == null) return new InstructorAccountVm();

            InstructorAccountVm instructorAccountVm = instructor.FromInstructor_ToInstructorAccountVm();
            instructorAccountVm.BaseUrl = _configuration["BaseUrl:Azure"];
            return instructorAccountVm;
        }

        public async Task<InstructorAccountVm> GetProfileDataAsync()
        {
            Instructor? instructor = await _instructorReadRepository.GetSingleAsync(x => x.Email == _actionContextAccessor.ActionContext.HttpContext.User.Identity.Name);
            if (instructor == null) return new InstructorAccountVm();

            InstructorAccountVm instructorAccountVm = instructor.FromInstructor_ToInstructorAccountVm();
            instructorAccountVm.BaseUrl = _configuration["BaseUrl:Azure"];
            return instructorAccountVm;
        }

        public async Task<bool> UpdateProfileAsync(InstructorAccountVm instructorAccountVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
                return false;

            Instructor? instructor = await _instructorReadRepository.GetSingleAsync(x => x.Id == instructorAccountVm.Id);
            if (instructor == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(InstructorAccountVm.Id), "Instructor Not Found");
                return false;
            };

            instructor.FirstName = instructorAccountVm.FirstName;
            instructor.LastName = instructorAccountVm.LastName;
            instructor.PhoneNumber = instructorAccountVm.PhoneNumber;
            instructor.Description = instructorAccountVm.Description;
            instructor.Profession = instructorAccountVm.Profession;
            instructor.AdditionalNotes = instructorAccountVm.AdditionalNotes;
            instructor.CountryOfBirth = instructorAccountVm.CountryOfBirth;
            instructor.CountryOfResidence = instructorAccountVm.CountryOfResidence;

            if (!instructorAccountVm.IsEmailConfirmed)
            {
                AppUser? user = await _userManager.FindByEmailAsync(instructorAccountVm.EmailAddress);
                if (user != null)
                {
                    _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(InstructorAccountVm.EmailAddress), "Email Already Exists!");
                    return false;
                }

                instructor.Email = instructorAccountVm.EmailAddress;
                instructor.NormalizedEmail = instructorAccountVm.EmailAddress.Normalize();
            }

            _instructorWriteRepository.Update(instructor);
            await _instructorWriteRepository.SaveAsync();
            return true;
        }

        public async Task<bool> UploadProfileImageAsync(Guid id, IFormFile file)
        {
            Instructor? instructor = await _instructorReadRepository.GetSingleAsync(x => x.Id == id);
            if (instructor == null) return false;

            FormFileCollection files = new FormFileCollection
                {
                file
            };

            List<(string fileName, string pathOrContainerName)> uploadedFiles = await _storage.UploadAsync(AzureContainerNames.InstructorProfileImages, files);

            (string fileName, string pathOrContainerName) uploadedFile = uploadedFiles[0];

            instructor.ProfileImageName = uploadedFile.fileName;
            instructor.ProfileImagePathOrContainer = uploadedFile.pathOrContainerName;

            _instructorWriteRepository.Update(instructor);
            await _instructorWriteRepository.SaveAsync();
            return true;
        }

        public async Task<bool> ChangeProfileImageAsync(Guid id, IFormFile file)
        {
            Instructor? instructor = await _instructorReadRepository.GetSingleAsync(x => x.Id == id);
            if (instructor == null) return false;

            if (!string.IsNullOrEmpty(instructor.ProfileImagePathOrContainer) && !string.IsNullOrEmpty(instructor.ProfileImageName))
            {
                await _storage.DeleteAsync(instructor.ProfileImagePathOrContainer, instructor.ProfileImageName);
            }

            IFormFileCollection files = new FormFileCollection
            {
                file
            };

            List<(string fileName, string pathOrContainerName)> uploadedFiles = await _storage.UploadAsync(AzureContainerNames.InstructorProfileImages, files);

            (string fileName, string pathOrContainerName) uploadedFile = uploadedFiles[0];

            instructor.ProfileImageName = uploadedFile.fileName;
            instructor.ProfileImagePathOrContainer = uploadedFile.pathOrContainerName;

            _instructorWriteRepository.Update(instructor);
            await _instructorWriteRepository.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteProfileImageAsync(Guid id)
        {
            Instructor? instructor = await _instructorReadRepository.GetSingleAsync(x => x.Id == id);
            if (instructor == null) return false;

            if (!string.IsNullOrEmpty(instructor.ProfileImagePathOrContainer) && !string.IsNullOrEmpty(instructor.ProfileImageName))
            {
                await _storage.DeleteAsync(instructor.ProfileImagePathOrContainer, instructor.ProfileImageName);

                instructor.ProfileImageName = null;
                instructor.ProfileImagePathOrContainer = null;

                _instructorWriteRepository.Update(instructor);
                await _instructorWriteRepository.SaveAsync();

            }

            return true;
        }

        public async Task<bool> ChangePasswordAsync(InstructorResetPasswordVm instructorResetPasswordVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
            {
                return false;
            }

            AppUser? user = await _userManager.FindByIdAsync(instructorResetPasswordVm.Id.ToString());
            if (user == null) return false;

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            await _userManager.ResetPasswordAsync(user, token, instructorResetPasswordVm.Password);

            return true;
        }



        #region Mail Services
        public async Task<bool> SendConfirmationMailAsync(Guid id)
        {
            Instructor? instructor = await _instructorReadRepository.GetSingleAsync(x => x.Id == id);
            if (instructor == null) return false;

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(instructor);

            await _mailManager.SendConfirmationMailAsync(token, instructor);

            return true;
        }

        public async Task<bool> SendPasswordGenerateMailAsync(Guid id)
        {
            Instructor? instructor = await _instructorReadRepository.GetSingleAsync(x => x.Id == id);
            if (instructor == null) return false;

            if (string.IsNullOrEmpty(instructor.PasswordHash))
            {
                string token = await _userManager.GeneratePasswordResetTokenAsync(instructor);

                await _mailManager.SendPasswordSetupMailAsync(token, instructor);
            }

            return true;
        }

        #endregion

        #region Instructor Panel Services

        public async Task<IEnumerable<Course>> GetInstructorCoursesAsync(string email)
        {
            Instructor? instructor = await _instructorReadRepository.Table
                .Include(x => x.Courses)
                .ThenInclude(x => x.Modules)
                .FirstOrDefaultAsync(x => x.Email == email);

            if (instructor == null) return Enumerable.Empty<Course>();

            return instructor.Courses;
        }

        public async Task<CourseSingleVm> GetInstructorCourseSingleAsync(string email, Guid id)
        {
            Instructor? instructor = await _instructorReadRepository.Table
                .Include(x => x.Courses)
                .ThenInclude(x => x.Template)
                .Include(x => x.Courses)
                .ThenInclude(x => x.Modules)
                .ThenInclude(x => x.Lectures)
                .FirstOrDefaultAsync(x => x.Email == email);

            Course? course = instructor?.Courses.FirstOrDefault(x => x.Id == id);

            if (course == null) return new CourseSingleVm();

            CourseSingleVm courseSingleVm = new CourseSingleVm()
            {
                BaseUrl = _configuration[ConfigurationStrings.AzureBaseUrl],
                Course = course,
                Template = course.Template,
            };

            return courseSingleVm;
        }

        public async Task<(bool success, string message)> ChangeInstructorShowcaseAsync(Guid id)
        {
            Instructor? instructor = await _instructorReadRepository.GetSingleAsync(x => x.Id == id);
            if (instructor == null || !instructor.IsApproved)
            {
                return (false, "Failed To Change Showcase.");
            }

            if(!instructor.Showcase && _instructorReadRepository.GetWhere(x => x.IsApproved && !x.IsDeleted && !x.IsBanned && x.Showcase).Count() >= 4)
            {
                return (false, "Only 4 Instructor Showcase Allowed.");
            }

            instructor.Showcase = !instructor.Showcase;

            _instructorWriteRepository.Update(instructor);
            await _instructorWriteRepository.SaveAsync();

            return (
                true,
                instructor.Showcase ? "Instructor Showcase Enabled." : "Instructor Showcase Disabled."
                );
        }

        #endregion
    }
}
