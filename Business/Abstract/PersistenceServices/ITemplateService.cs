using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel;
using CodinaxProjectMvc.ViewModel.TemplateVm;

namespace CodinaxProjectMvc.Business.Abstract.PersistenceServices
{
    /// <summary>
    /// Interface representing the service for managing templates.
    /// </summary>
    public interface ITemplateService
    {
        /// <summary>
        /// Retrieves a collection of templates asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, returning the collection of templates.</returns>
        Task<TemplatesVm> GetTemplatesAsync();

        /// <summary>
        /// Creates a new template asynchronously.
        /// </summary>
        /// <param name="templateCreateVm">The view model containing information for creating the template.</param> 
        /// <returns>A task representing the asynchronous operation, returning true if the template creation is successful, otherwise false.</returns>
        Task<bool> CreateTemplateAsync(TemplateCreateVm templateCreateVm);

        /// <summary>
        /// Retrieves update data for a template asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the template.</param>
        /// <returns>A task representing the asynchronous operation, returning the view model containing template update data.</returns>
        Task<TemplateUpdateVm> GetTemplateUpdateDataAsync(Guid id);

        /// <summary>
        /// Updates a template asynchronously.
        /// </summary>
        /// <param name="templateUpdateVm">The view model containing updated template information.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the template update is successful, otherwise false.</returns>
        Task<bool> UpdateTemplateAsync(TemplateUpdateVm templateUpdateVm);

        /// <summary>
        /// Removes a course fragment video asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the course fragment video.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the removal is successful, otherwise false.</returns>
        Task<bool> RemoveCourseFragmentVideoAsync(Guid id);

        /// <summary>
        /// Deletes a template asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the template.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the deletion is successful, otherwise false.</returns>
        Task<bool> DeleteTemplateAsync(Guid id);

        /// <summary>
        /// Archives a tool asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the tool.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the archiving is successful, otherwise false.</returns>
        Task<bool> ArchiveToolAsync(Guid id);

        /// <summary>
        /// Updates a tool asynchronously.
        /// </summary>
        /// <param name="courseToolUpdateVm">The view model containing updated tool information.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the update is successful, otherwise false.</returns>
        Task<bool> UpdateToolAsync(CourseToolUpdateVm courseToolUpdateVm);

        /// <summary>
        /// Creates a tool asynchronously.
        /// </summary>
        /// <param name="courseToolCreateVm">The view model containing information for creating the tool.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the creation is successful, otherwise false.</returns>
        Task<bool> CreateToolAsync(CourseToolCreateVm courseToolCreateVm);

        /// <summary>
        /// Deletes a tool asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the tool.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the deletion is successful, otherwise false.</returns>
        Task<bool> DeleteToolAsync(Guid id);

        /// <summary>
        /// Unarchives a tool asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the tool.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the unarchiving is successful, otherwise false.</returns>
        Task<bool> UnArchiveToolAsync(Guid id);

        /// <summary>
        /// Restores a tool asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the tool.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the restoration is successful, otherwise false.</returns>
        Task<bool> RestoreToolAsync(Guid id);

        /// <summary>
        /// Retrieves update data for a tool asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the tool.</param>
        /// <returns>A task representing the asynchronous operation, returning the view model containing tool update data.</returns>
        Task<CourseToolUpdateVm> GetUpdateToolDataAsync(Guid id);

        /// <summary>
        /// Retrieves a collection of tools asynchronously associated with a template.
        /// </summary>
        /// <param name="id">The unique identifier of the template.</param>
        /// <returns>A task representing the asynchronous operation, returning the collection of tools.</returns>
        Task<CourseToolsVm> GetToolsAsync(Guid id);

        /// <summary>
        /// Adds a course price asynchronously.
        /// </summary>
        /// <param name="coursePriceCreateVm">The view model containing information for adding the course price.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the addition is successful, otherwise false.</returns>
        Task<bool> AddCoursePriceAsync(CoursePriceCreateVm coursePriceCreateVm);

        /// <summary>
        /// Retrieves a paginated list of course prices asynchronously associated with a template.
        /// </summary>
        /// <param name="id">The unique identifier of the template.</param>
        /// <param name="searchFilter">Optional search filter for course prices.</param>
        /// <returns>A task representing the asynchronous operation, returning the paginated list of course prices.</returns>
        Task<PaginationVm<IEnumerable<Price>>> GetCoursePricesAsync(Guid id, string? searchFilter = null);

        /// <summary>
        /// Retrieves update data for a course price asynchronously.
        /// </summary>
        /// <param name="courseId">The unique identifier of the template.</param>
        /// <param name="priceId">The unique identifier of the course price.</param>
        /// <returns>A task representing the asynchronous operation, returning the view model containing course price update data.</returns>
        Task<CoursePriceUpdateVm> GetCoursePriceUpdateDataAsync(Guid courseId, Guid priceId);

        /// <summary>
        /// Updates a course price asynchronously.
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
        /// Unarchives a course price asynchronously.
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
        /// Restores a course price asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the course price.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the restoration is successful, otherwise false.</returns>
        Task<bool> RestoreCoursePriceAsync(Guid id);

        /// <summary>
        /// Retrieves a view model containing the collection of course prices asynchronously associated with a template.
        /// </summary>
        /// <param name="id">The unique identifier of the template.</param>
        /// <returns>A task representing the asynchronous operation, returning the view model containing the collection of course prices.</returns>
        Task<CoursePricesVm> GetPricesAsync(Guid id);
    }

}
