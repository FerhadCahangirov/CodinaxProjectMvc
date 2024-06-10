using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel.HistoryVm;

namespace CodinaxProjectMvc.Business.Abstract.PersistenceServices
{
    public interface IHistoryService
    {
        Task AddOrUpdateHistoryAsync(Guid videoId);
        Task<List<History>> ListHistoriesAsync();
    }
}
