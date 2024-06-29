using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel;
using CodinaxProjectMvc.ViewModel.CourseVm;
using CodinaxProjectMvc.ViewModel.InstructorVm;

namespace CodinaxProjectMvc.Business.Abstract.PersistenceServices
{
    /// <summary>
    /// Interface representing the service for managing instructors.
    /// </summary>
    public interface IInstructorService
    {
        /// <summary>
        /// Retrieves a paginated list of instructors based on optional search and status filters.
        /// </summary>
        /// <param name="searchFilter">Optional search filter for instructor names.</param>
        /// <param name="statusFilter">Optional status filter for instructors (e.g., Active, Inactive).</param>
        /// <param name="page">Page number for pagination (default is 0).</param>
        /// <param name="size">Number of items per page (default is 10).</param>
        /// <returns>A task representing the asynchronous operation, returning a pagination view model of instructors.</returns>
        public Task<PaginationVm<IEnumerable<Instructor>>> GetInstructorPaginationAsync(string? searchFilter = null, string? statusFilter = null);

        /// <summary>
        /// Approves an instructor with the specified ID.
        /// </summary>
        /// <param name="id">The unique identifier of the instructor to approve.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the approval succeeds, otherwise false.</returns>
        public Task<bool> ApproveAsync(Guid id);

        /// <summary>
        /// Bans an instructor with the specified ID.
        /// </summary>
        /// <param name="id">The unique identifier of the instructor to ban.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the ban succeeds, otherwise false.</returns>
        public Task<bool> BanAsync(Guid id);

        /// <summary>
        /// Unbans a previously banned instructor with the specified ID.
        /// </summary>
        /// <param name="id">The unique identifier of the instructor to unban.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the unban succeeds, otherwise false.</returns>
        public Task<bool> UnbanAsync(Guid id);

        /// <summary>
        /// Retrieves the profile data of an instructor with the specified ID.
        /// </summary>
        /// <param name="id">The unique identifier of the instructor.</param>
        /// <returns>A task representing the asynchronous operation, returning the instructor's account view model.</returns>
        public Task<InstructorAccountVm> GetProfileDataAsync(Guid id);

        public Task<InstructorAccountVm> GetProfileDataAsync();

        /// <summary>
        /// Updates the profile data of an instructor.
        /// </summary>
        /// <param name="instructorAccountVm">The updated instructor account view model.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the update succeeds, otherwise false.</returns>
        public Task<bool> UpdateProfileAsync(InstructorAccountVm instructorAccountVm);

        /// <summary>
        /// Uploads a profile image for the instructor with the specified ID.
        /// </summary>
        /// <param name="id">The unique identifier of the instructor.</param>
        /// <param name="file">The form file containing the profile image.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the upload succeeds, otherwise false.</returns>
        public Task<bool> UploadProfileImageAsync(Guid id, IFormFile file);

        /// <summary>
        /// Changes the profile image for the instructor with the specified ID.
        /// </summary>
        /// <param name="id">The unique identifier of the instructor.</param>
        /// <param name="file">The form file containing the new profile image.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the change succeeds, otherwise false.</returns>
        public Task<bool> ChangeProfileImageAsync(Guid id, IFormFile file);

        /// <summary>
        /// Deletes the profile image of the instructor with the specified ID.
        /// </summary>
        /// <param name="id">The unique identifier of the instructor.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the deletion succeeds, otherwise false.</returns>
        public Task<bool> DeleteProfileImageAsync(Guid id);

        /// <summary>
        /// Sends a confirmation email asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier associated with the confirmation email.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the email sending is successful, otherwise false.</returns>
        public Task<bool> SendConfirmationMailAsync(Guid id);

        Task<PaginationVm<IEnumerable<Instructor>>> GetAssignableInstructorPaginationAsync(string? searchFilter = null);

        Task<bool> DeleteAsync(Guid id);

        Task<IEnumerable<Course>> GetInstructorCoursesAsync(string email);

        Task<CourseSingleVm> GetInstructorCourseSingleAsync(string email, Guid id);

        Task<bool> SendPasswordGenerateMailAsync(Guid id);

        Task<bool> ChangePasswordAsync(InstructorResetPasswordVm instructorResetPasswordVm);
    }
}
