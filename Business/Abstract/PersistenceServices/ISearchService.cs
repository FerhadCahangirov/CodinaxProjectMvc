using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.DataAccess.Models.Common;
using CodinaxProjectMvc.ViewModel.SearchVm;

namespace CodinaxProjectMvc.Business.Abstract.PersistenceServices
{
    public interface ISearchService
    {
        Task<SearchListVm> SearchAsync(string query);
        IEnumerable<TEntity>? Search<TEntity>(string query) where TEntity : BaseEntity;
    }
}
