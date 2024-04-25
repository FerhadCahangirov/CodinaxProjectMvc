using CodinaxProjectMvc.ViewModel.LayoutVm;

namespace CodinaxProjectMvc.Business.Abstract.InfrastructureServices
{
    public interface IContactService
    {
        Task<bool> SendAsync(ContactVm contactVm);
    }
}
