using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel;
using CodinaxProjectMvc.ViewModel.FeatureVm;

namespace CodinaxProjectMvc.Business.Abstract.PersistenceServices
{
    /// <summary>
    /// Interface for managing features.
    /// </summary>
    public interface IFeatureService
    {
        /// <summary>
        /// Retrieves a paginated list of features asynchronously, optionally filtered by search.
        /// </summary>
        /// <param name="searchFilter">Optional search filter for filtering features.</param>
        /// <returns>A task representing the asynchronous operation, returning the pagination view model containing features.</returns>
        Task<PaginationVm<IEnumerable<Feature>>> GetFeaturesAsync(string? searchFilter = null);

        /// <summary>
        /// Creates a new feature asynchronously.
        /// </summary>
        /// <param name="featureCreateVm">The view model containing information for creating the feature.</param>
        /// <returns>A task representing the asynchronous operation, returning the action return type indicating the outcome of the creation.</returns>
        Task<FeatureActionReturnType> CreateFeatureAsync(FeatureCreateVm featureCreateVm);

        /// <summary>
        /// Retrieves update data for a feature asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the feature.</param>
        /// <returns>A task representing the asynchronous operation, returning the view model containing feature update data.</returns>
        Task<FeatureUpdateVm> GetFeatureUpdateDataAsync(Guid id);

        /// <summary>
        /// Updates a feature asynchronously.
        /// </summary>
        /// <param name="featureUpdateVm">The view model containing updated feature information.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the update is successful, otherwise false.</returns>
        Task<bool> UpdateFeatureAsync(FeatureUpdateVm featureUpdateVm);

        /// <summary>
        /// Archives a feature asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the feature.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the archiving is successful, otherwise false.</returns>
        Task<bool> ArchiveAsync(Guid id);

        /// <summary>
        /// Deletes a feature asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the feature.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the deletion is successful, otherwise false.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Restores a previously deleted feature asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the feature.</param>
        /// <returns>A task representing the asynchronous operation, returning the action return type indicating the outcome of the restoration.</returns>
        Task<FeatureActionReturnType> RestoreAsync(Guid id);

        /// <summary>
        /// Unarchives a previously archived feature asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the feature.</param>
        /// <returns>A task representing the asynchronous operation, returning the action return type indicating the outcome of the operation.</returns>
        Task<FeatureActionReturnType> UnarchiveAsync(Guid id);
    }

    /// <summary>
    /// Enumeration indicating the outcome of an action related to features.
    /// </summary>
    public enum FeatureActionReturnType
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
