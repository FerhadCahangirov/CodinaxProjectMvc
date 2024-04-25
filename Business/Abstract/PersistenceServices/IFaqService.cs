using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel.FaqVm;
using CodinaxProjectMvc.ViewModel;

namespace CodinaxProjectMvc.Business.Abstract.PersistenceServices
{
    /// <summary>
    /// Interface for managing FAQs (Frequently Asked Questions).
    /// </summary>
    public interface IFaqService
    {
        /// <summary>
        /// Retrieves a paginated list of FAQs asynchronously, optionally filtered by question and/or answer.
        /// </summary>
        /// <param name="questionSearchFilter">Optional search filter for filtering FAQs by question.</param>
        /// <param name="answerSearchFilter">Optional search filter for filtering FAQs by answer.</param>
        /// <returns>A task representing the asynchronous operation, returning the pagination view model containing FAQs.</returns>
        public Task<PaginationVm<IEnumerable<Faq>>> GetFaqsPaginationAsync(string? questionSearchFilter = null, string? answerSearchFilter = null);

        /// <summary>
        /// Creates a new FAQ asynchronously.
        /// </summary>
        /// <param name="faqCreateVm">The view model containing information for creating the FAQ.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the creation is successful, otherwise false.</returns>
        Task<bool> CreateAsync(FaqCreateVm faqCreateVm);

        /// <summary>
        /// Retrieves update data for an FAQ asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the FAQ.</param>
        /// <returns>A task representing the asynchronous operation, returning the view model containing FAQ update data.</returns>
        Task<FaqUpdateVm> GetFaqUpdateDataAsync(Guid id);

        /// <summary>
        /// Updates an FAQ asynchronously.
        /// </summary>
        /// <param name="faqUpdateVm">The view model containing updated FAQ information.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the update is successful, otherwise false.</returns>
        Task<bool> UpdateAsync(FaqUpdateVm faqUpdateVm);

        /// <summary>
        /// Archives an FAQ asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the FAQ.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the archiving is successful, otherwise false.</returns>
        Task<bool> ArchiveAsync(Guid id);

        /// <summary>
        /// Unarchives a previously archived FAQ asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the FAQ.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the unarchiving is successful, otherwise false.</returns>
        Task<bool> UnArchiveAsync(Guid id);

        /// <summary>
        /// Deletes an FAQ asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the FAQ.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the deletion is successful, otherwise false.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Restores a previously deleted FAQ asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the FAQ.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the restoration is successful, otherwise false.</returns>
        Task<bool> RestoreAsync(Guid id);
    }
}
