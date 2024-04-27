using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel;
using CodinaxProjectMvc.ViewModel.TemplateVm;

namespace CodinaxProjectMvc.Business.Abstract.PersistenceServices
{
    public interface ITemplateService
    {
        /// <summary>
        /// Retrieves a collection of courses asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, returning the collection of courses.</returns>
        Task<TemplatesVm> GetTemplatesAsync();

        /// <summary>
        /// Creates a new course asynchronously.
        /// </summary>
        /// <param name="courseCreateVm">The view model containing information for creating the course.</param> 
        /// <returns>A task representing the asynchronous operation, returning true if the course creation is successful, otherwise false.</returns>
        Task<bool> CreateTemplateAsync(TemplateCreateVm courseCreateVm);

        /// <summary>
        /// Retrieves update data for a course asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the course.</param>
        /// <returns>A task representing the asynchronous operation, returning the view model containing course update data.</returns>
        Task<TemplateUpdateVm> GetTemplateUpdateDataAsync(Guid id);

        /// <summary>
        /// Updates a course asynchronously.
        /// </summary>
        /// <param name="courseUpdateVm">The view model containing updated course information.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the course update is successful, otherwise false.</returns>
        Task<bool> UpdateTemplateAsync(TemplateUpdateVm courseUpdateVm);

        Task<bool> RemoveCourseFragmentVideoAsync(Guid id);

        Task<bool> DeleteTemplateAsync(Guid id);


        Task<bool> ArchiveToolAsync(Guid id);

        Task<bool> UpdateToolAsync(CourseToolUpdateVm courseToolUpdateVm);

        Task<bool> CreateToolAsync(CourseToolCreateVm courseToolCreateVm);

        Task<bool> DeleteToolAsync(Guid id);

        Task<bool> UnArchiveTool(Guid id);

        Task<bool> RestoreToolAsync(Guid id);

        Task<CourseToolUpdateVm> GetUpdateToolDataAsync(Guid id);

        Task<CourseToolsVm> GetToolsAsync(Guid id);

        Task<bool> AddCoursePriceAsync(CoursePriceCreateVm coursePriceCreateVm);

        Task<PaginationVm<IEnumerable<Price>>> GetCoursePricesAsync(Guid id, string? searchFilter = null);

        Task<CoursePriceUpdateVm> GetCoursePriceUpdateDataAsync(Guid courseId, Guid priceId);

        Task<bool> UpdateCoursePriceAsync(CoursePriceUpdateVm coursePriceUpdateVm);

        Task<bool> ArchiveCoursePriceAsync(Guid id);

        Task<bool> UnArchiveCoursePriceAsync(Guid id);

        Task<bool> DeleteCoursePriceAsync(Guid id);

        Task<bool> RestoreCoursePriceAsync(Guid id);

        Task<CoursePricesVm> GetPricesAsync(Guid id);
    }
}
