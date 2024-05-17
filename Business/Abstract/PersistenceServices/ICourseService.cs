
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel;
using CodinaxProjectMvc.ViewModel.CourseVm;
using System.Runtime.CompilerServices;

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
        /// Deletes a course asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the course to delete.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the deletion is successful, otherwise false.</returns>
        Task<bool> DeleteCourseAsync(Guid id);

        /// <summary>
        /// Changes the showcase status of a course asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the course.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the change is successful, otherwise false.</returns>
        Task<bool> ChangeShowcaseAsync(Guid id);

        Task<PrimaryCourseActionReturnType> SetCoursePrimaryAsync(Guid id);


        #region Managing Course Instructors

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

        Task<CourseInstructorsAssignVm> GetAssignableInstructorsAsync(Guid id, string? searchFilter = null);

        #endregion

        #region Managing Course Students

        Task<bool> AssignStudentAsync(Guid courseId, Guid studentId);

        Task<bool> ReassignStudentAsync(Guid courseId, Guid studentId);

        Task<CourseStudentsVm> GetCourseStudentsAsync(Guid id, string? searchFilter);

        Task<CourseStudentsAssignVm> GetAssignableStudentsAsync(Guid id, string? searchFilter = null);

        #endregion

    }

    /// <summary>
    /// Enumeration indicating the outcome of an action related to primary course.
    /// </summary>
    public enum PrimaryCourseActionReturnType
    {
        /// <summary>
        /// The action was successful.
        /// </summary>
        Success,

        /// <summary>
        /// The action failed.
        /// </summary>
        Failure,

        /// <summary>
        /// The action failed due to an oversized entry.
        /// </summary>
        Oversized
    }
}
