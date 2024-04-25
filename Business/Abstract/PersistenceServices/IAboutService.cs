using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel;
using CodinaxProjectMvc.ViewModel.AboutVm;

namespace CodinaxProjectMvc.Business.Abstract.PersistenceServices
{
    /// <summary>
    /// Interface for managing 'About' information.
    /// </summary>
    public interface IAboutService
    {
        /// <summary>
        /// Retrieves a paginated list of 'About' entries asynchronously, optionally filtered by search and status.
        /// </summary>
        /// <param name="searchFilter">Optional search filter for filtering 'About' entries.</param>
        /// <param name="statusFilter">Optional status filter for filtering 'About' entries by status.</param>
        /// <returns>A task representing the asynchronous operation, returning the pagination view model containing 'About' entries.</returns>
        public Task<PaginationVm<IEnumerable<About>>> GetAboutsPaginationAsync(string? searchFilter = null, string? statusFilter = null);

        /// <summary>
        /// Creates a new 'About' entry asynchronously.
        /// </summary>
        /// <param name="aboutCreateVm">The view model containing information for creating the 'About' entry.</param>
        /// <returns>A task representing the asynchronous operation, returning the action return type indicating the outcome of the creation.</returns>
        Task<AboutActionReturnType> CreateAsync(AboutCreateVm aboutCreateVm);

        /// <summary>
        /// Retrieves update data for a 'About' entry asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the 'About' entry.</param>
        /// <returns>A task representing the asynchronous operation, returning the view model containing 'About' update data.</returns>
        Task<AboutUpdateVm> GetAboutUpdateDataAsync(Guid id);

        /// <summary>
        /// Updates a 'About' entry asynchronously.
        /// </summary>
        /// <param name="aboutUpdateVm">The view model containing updated 'About' information.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the update is successful, otherwise false.</returns>
        Task<bool> UpdateAsync(AboutUpdateVm aboutUpdateVm);

        /// <summary>
        /// Archives a 'About' entry asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the 'About' entry.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the archiving is successful, otherwise false.</returns>
        Task<bool> ArchiveAsync(Guid id);

        /// <summary>
        /// Unarchives a previously archived 'About' entry asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the 'About' entry.</param>
        /// <returns>A task representing the asynchronous operation, returning the action return type indicating the outcome of the operation.</returns>
        Task<AboutActionReturnType> UnArchiveAsync(Guid id);

        /// <summary>
        /// Deletes a 'About' entry asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the 'About' entry.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the deletion is successful, otherwise false.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Restores a previously deleted 'About' entry asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the 'About' entry.</param>
        /// <returns>A task representing the asynchronous operation, returning the action return type indicating the outcome of the restoration.</returns>
        Task<AboutActionReturnType> RestoreAsync(Guid id);
    }

    /// <summary>
    /// Enumeration indicating the outcome of an action related to 'About' entries.
    /// </summary>
    public enum AboutActionReturnType
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
