using CodinaxProjectMvc.DataAccess.Models.Identity;
using CodinaxProjectMvc.ViewModel.LayoutVm;

namespace CodinaxProjectMvc.Managers.Abstract
{
    public interface IMailManager
    {
        Task SendConfirmationMailAsync<TEntity>(string token, TEntity user) where TEntity : AppUser;

        Task SendPasswordSetupMailAsync<TEntity>(string token, TEntity user) where TEntity : AppUser;

        Task SendContactMailAsync(ContactVm contactVm);
    }
}
