using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel;
using CodinaxProjectMvc.ViewModel.SubscribeVm;

namespace CodinaxProjectMvc.Business.Abstract.PersistenceServices
{
    public interface ISubscriberService
    {
        Task<PaginationVm<IEnumerable<Subscriber>>> ListSubscribersAsync(string? searchFilter = null, string? statusFilter = null);

        Task<bool> SubscribeAsync(string email);
        Task<bool> UnSubscribeAsync(string email);

        Task<bool> ConfirmAsync(string email, string token);

        Task<bool> SendAsync(SubscribeSendVm subscribeSendVm);
    }
}
