using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.DataAccess.Models.Identity;
using CodinaxProjectMvc.ViewModel.LayoutVm;

namespace CodinaxProjectMvc.Managers.Abstract
{
    public interface IMailManager
    {
        Task SendConfirmationMailAsync<TEntity>(string token, TEntity user) where TEntity : AppUser;

        Task SendPasswordSetupMailAsync<TEntity>(string token, TEntity user) where TEntity : AppUser;

        Task SendContactMailAsync(ContactVm contactVm);

        Task SendSubscribeConfirmationMailAsync(string token, string email);

        Task SendMailToAllSubscribersAsync(string subject, string content, List<Subscriber> subscribers);
    }
}
