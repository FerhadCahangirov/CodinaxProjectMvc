using CodinaxProjectMvc.DataAccess.Models;

namespace CodinaxProjectMvc.Business.Abstract.PersistenceServices
{
    public interface ISubscriberService
    {
        Task<IEnumerable<Subscriber>> ListSubscribersAsync();

        Task<bool> SubscribeAsync(string email);

        Task<bool> ConfirmAsync(string email, string token);
    }
}
