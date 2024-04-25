using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel;
using CodinaxProjectMvc.ViewModel.StudentVm;

namespace CodinaxProjectMvc.Business.Abstract.PersistenceServices
{
    /// <summary>
    /// Interface representing the service for managing students.
    /// </summary>
    public interface IStudentService
    {
        /// <summary>
        /// Retrieves a paginated list of students based on optional search and status filters.
        /// </summary>
        /// <param name="searchFilter">Optional search filter for student names or other criteria.</param>
        /// <param name="statusFilter">Optional status filter for students (e.g., Active, Inactive).</param>
        /// <param name="page">Page number for pagination (default is 0).</param>
        /// <param name="size">Number of items per page (default is 10).</param>
        /// <returns>A task representing the asynchronous operation, returning a pagination view model of students.</returns>
        Task<PaginationVm<IEnumerable<Student>>> GetStudentsPaginationAsync(string? searchFilter = null, string? statusFilter = null, int page = 0, int size = 10);

        /// <summary>
        /// Approves a student with the specified ID.
        /// </summary>
        /// <param name="id">The unique identifier of the student to approve.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the approval succeeds, otherwise false.</returns>
        Task<bool> ApproveAsync(Guid id);

        /// <summary>
        /// Bans a student with the specified ID.
        /// </summary>
        /// <param name="id">The unique identifier of the student to ban.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the ban succeeds, otherwise false.</returns>
        Task<bool> BanAsync(Guid id);

        /// <summary>
        /// Unbans a previously banned student with the specified ID.
        /// </summary>
        /// <param name="id">The unique identifier of the student to unban.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the unban succeeds, otherwise false.</returns>
        Task<bool> UnbanAsync(Guid id);

        /// <summary>
        /// Retrieves the profile data of a student with the specified ID.
        /// </summary>
        /// <param name="id">The unique identifier of the student.</param>
        /// <returns>A task representing the asynchronous operation, returning the student's account view model.</returns>
        Task<StudentAccountVm> GetProfileDataAsync(Guid id);

        /// <summary>
        /// Updates the profile data of a student.
        /// </summary>
        /// <param name="studentAccountVm">The updated student account view model.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the update succeeds, otherwise false.</returns>
        Task<bool> UpdateProfileAsync(StudentAccountVm studentAccountVm);

        /// <summary>
        /// Uploads a profile image for the student with the specified ID.
        /// </summary>
        /// <param name="id">The unique identifier of the student.</param>
        /// <param name="file">The form file containing the profile image.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the upload succeeds, otherwise false.</returns>
        Task<bool> UploadProfileImageAsync(Guid id, IFormFile file);

        /// <summary>
        /// Changes the profile image for the student with the specified ID.
        /// </summary>
        /// <param name="id">The unique identifier of the student.</param>
        /// <param name="file">The form file containing the new profile image.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the change succeeds, otherwise false.</returns>
        Task<bool> ChangeProfileImageAsync(Guid id, IFormFile file);

        /// <summary>
        /// Deletes the profile image of the student with the specified ID.
        /// </summary>
        /// <param name="id">The unique identifier of the student.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the deletion succeeds, otherwise false.</returns>
        Task<bool> DeleteProfileImageAsync(Guid id);

        /// <summary>
        /// Sends a confirmation email asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier associated with the confirmation email.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the email sending is successful, otherwise false.</returns>
        public Task<bool> SendConfirmationMailAsync(Guid id);
    }

}
