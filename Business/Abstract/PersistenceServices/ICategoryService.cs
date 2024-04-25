using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel;
using CodinaxProjectMvc.ViewModel.CategoryVm;

namespace CodinaxProjectMvc.Business.Abstract.PersistenceServices
{
    /// <summary>
    /// Interface for managing categories.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Retrieves a paginated list of categories asynchronously, optionally filtered by search and status.
        /// </summary>
        /// <param name="searchFilter">Optional search filter for filtering categories.</param>
        /// <param name="statusFilter">Optional status filter for filtering categories by status.</param>
        /// <returns>A task representing the asynchronous operation, returning the pagination view model containing categories.</returns>
        public Task<PaginationVm<IEnumerable<Category>>> GetCategoriesPaginationAsync(string? searchFilter = null, string? statusFilter = null);

        /// <summary>
        /// Creates a new category asynchronously.
        /// </summary>
        /// <param name="categoryCreateVm">The view model containing information for creating the category.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the creation is successful, otherwise false.</returns>
        Task<bool> CreateAsync(CategoryCreateVm categoryCreateVm);

        /// <summary>
        /// Retrieves update data for a category asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the category.</param>
        /// <returns>A task representing the asynchronous operation, returning the view model containing category update data.</returns>
        Task<CategoryUpdateVm> GetCategoryUpdateDataAsync(Guid id);

        /// <summary>
        /// Updates a category asynchronously.
        /// </summary>
        /// <param name="categoryUpdateVm">The view model containing updated category information.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the update is successful, otherwise false.</returns>
        Task<bool> UpdateAsync(CategoryUpdateVm categoryUpdateVm);

        /// <summary>
        /// Archives a category asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the category.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the archiving is successful, otherwise false.</returns>
        Task<bool> ArchiveAsync(Guid id);

        /// <summary>
        /// Unarchives a previously archived category asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the category.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the unarchiving is successful, otherwise false.</returns>
        Task<bool> UnArchiveAsync(Guid id);

        /// <summary>
        /// Deletes a category asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the category.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the deletion is successful, otherwise false.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Restores a previously deleted category asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the category.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the restoration is successful, otherwise false.</returns>
        Task<bool> RestoreAsync(Guid id);

        /// <summary>
        /// Checks if a category with the given name exists asynchronously.
        /// </summary>
        /// <param name="name">The name of the category to check.</param>
        /// <returns>A task representing the asynchronous operation, returning true if a category with the given name exists, otherwise false.</returns>
        Task<bool> CheckIsExistsAsync(string name);
    }

}
