
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel;
using CodinaxProjectMvc.ViewModel.CourseVm;

namespace CodinaxProjectMvc.Business.Abstract.PersistenceServices
{
    /// <summary>
    /// Interface for managing courses and associated tools and instructors.
    /// </summary>
    public interface ICourseService
    {
        /// <summary>
        /// Retrieves a collection of courses asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, returning the collection of courses.</returns>
        Task<IEnumerable<Course>> GetCoursesAsync();

        /// <summary>
        /// Creates a new course asynchronously.
        /// </summary>
        /// <param name="courseCreateVm">The view model containing information for creating the course.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the course creation is successful, otherwise false.</returns>
        Task<bool> CreateCourseAsync(CourseCreateVm courseCreateVm);

        /// <summary>
        /// Retrieves update data for a course asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the course.</param>
        /// <returns>A task representing the asynchronous operation, returning the view model containing course update data.</returns>
        Task<CourseUpdateVm> GetCourseUpdateDataAsync(Guid id);

        /// <summary>
        /// Updates a course asynchronously.
        /// </summary>
        /// <param name="courseUpdateVm">The view model containing updated course information.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the course update is successful, otherwise false.</returns>
        Task<bool> UpdateCourseAsync(CourseUpdateVm courseUpdateVm);

        /// <summary>
        /// Retrieves details of a single course asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the course.</param>
        /// <returns>A task representing the asynchronous operation, returning the view model containing details of the course.</returns>
        Task<CourseSingleVm> GetCourseSingleAsync(Guid id);

        /// <summary>
        /// Assigns an instructor to a course asynchronously.
        /// </summary>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <param name="instructorId">The unique identifier of the instructor.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the assignment is successful, otherwise false.</returns>
        Task<bool> AssignInstructorAsync(Guid courseId, Guid instructorId);

        /// <summary>
        /// Reassigns an instructor for a course asynchronously.
        /// </summary>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <param name="instructorId">The unique identifier of the new instructor.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the reassignment is successful, otherwise false.</returns>
        Task<bool> ReassignInstructorAsync(Guid courseId, Guid instructorId);

        /// <summary>
        /// Retrieves instructors associated with a course asynchronously, optionally filtered by name.
        /// </summary>
        /// <param name="id">The unique identifier of the course.</param>
        /// <param name="searchFilter">Optional search filter for filtering instructors by name.</param>
        /// <returns>A task representing the asynchronous operation, returning the view model containing course instructors.</returns>
        Task<CourseInstructorsVm> GetCourseInstructorsAsync(Guid id, string? searchFilter);

        /// <summary>
        /// Creates a new tool for a course asynchronously.
        /// </summary>
        /// <param name="courseToolCreateVm">The view model containing information for creating the tool.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the tool creation is successful, otherwise false.</returns>
        Task<bool> CreateToolAsync(CourseToolCreateVm courseToolCreateVm);

        /// <summary>
        /// Retrieves update data for a tool asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the tool.</param>
        /// <returns>A task representing the asynchronous operation, returning the view model containing tool update data.</returns>
        Task<CourseToolUpdateVm> GetUpdateToolDataAsync(Guid id);

        /// <summary>
        /// Updates a tool asynchronously.
        /// </summary>
        /// <param name="courseToolUpdateVm">The view model containing updated tool information.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the tool update is successful, otherwise false.</returns>
        Task<bool> UpdateToolAsync(CourseToolUpdateVm courseToolUpdateVm);

        /// <summary>
        /// Archives a tool asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the tool.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the archiving is successful, otherwise false.</returns>
        Task<bool> ArchiveToolAsync(Guid id);

        /// <summary>
        /// Unarchives a previously archived tool asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the tool.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the unarchiving is successful, otherwise false.</returns>
        Task<bool> UnArchiveTool(Guid id);

        /// <summary>
        /// Deletes a tool asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the tool.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the deletion is successful, otherwise false.</returns>
        Task<bool> DeleteToolAsync(Guid id);

        /// <summary>
        /// Restores a previously deleted tool asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the tool.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the restoration is successful, otherwise false.</returns>
        Task<bool> RestoreToolAsync(Guid id);
        /// <summary>
        /// Adds a price to a course asynchronously.
        /// </summary>
        /// <param name="coursePriceCreateVm">The view model containing information for adding the price to the course.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the addition is successful, otherwise false.</returns>
        Task<bool> AddCoursePriceAsync(CoursePriceCreateVm coursePriceCreateVm);

        /// <summary>
        /// Retrieves course prices asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the course.</param>
        /// <param name="searchFilter">Optional search filter for filtering prices by title.</param>
        /// <returns>A task representing the asynchronous operation, returning the view model containing course prices.</returns>
        Task<PaginationVm<IEnumerable<Price>>> GetCoursePricesAsync(Guid id, string? searchFilter = null);

        /// <summary>
        /// Retrieves update data for a course price asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the course price.</param>
        /// <returns>A task representing the asynchronous operation, returning the view model containing course price update data.</returns>
        Task<CoursePriceUpdateVm> GetCoursePriceUpdateDataAsync(Guid courseId, Guid priceId);

        /// <summary>
        /// Updates course prices asynchronously.
        /// </summary>
        /// <param name="coursePriceUpdateVm">The view model containing updated course price information.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the update is successful, otherwise false.</returns>
        Task<bool> UpdateCoursePriceAsync(CoursePriceUpdateVm coursePriceUpdateVm);

        /// <summary>
        /// Archives a course price asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the course price.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the archiving is successful, otherwise false.</returns>
        Task<bool> ArchiveCoursePriceAsync(Guid id);

        /// <summary>
        /// Unarchives a previously archived course price asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the course price.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the unarchiving is successful, otherwise false.</returns>
        Task<bool> UnArchiveCoursePriceAsync(Guid id);

        /// <summary>
        /// Deletes a course price asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the course price.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the deletion is successful, otherwise false.</returns>
        Task<bool> DeleteCoursePriceAsync(Guid id);

        /// <summary>
        /// Restores a previously deleted course price asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the course price.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the restoration is successful, otherwise false.</returns>
        Task<bool> RestoreCoursePriceAsync(Guid id);

        /// <summary>
        /// Removes a course fragment video asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the course fragment video to remove.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the removal is successful, otherwise false.</returns>
        Task<bool> RemoveCourseFragmentVideoAsync(Guid id);

        /// <summary>
        /// Deletes a course asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the course to delete.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the deletion is successful, otherwise false.</returns>
        Task<bool> DeleteCourseAsync(Guid id);
    }

}
