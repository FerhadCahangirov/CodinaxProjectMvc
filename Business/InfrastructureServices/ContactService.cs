using CodinaxProjectMvc.Business.Abstract.InfrastructureServices;
using CodinaxProjectMvc.Managers.Abstract;
using CodinaxProjectMvc.ViewModel.LayoutVm;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace CodinaxProjectMvc.Business.InfrastructureServices
{
    public class ContactService : IContactService
    {
        private readonly IMailManager _mailManager;
        private readonly IActionContextAccessor _actionContextAccessor;

        public ContactService(IMailManager mailManager, IActionContextAccessor actionContextAccessor)
        {
            _mailManager = mailManager;
            _actionContextAccessor = actionContextAccessor;
        }

        public async Task<bool> SendAsync(ContactVm contactVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
                return false;
            await _mailManager.SendContactMailAsync(contactVm);
            return true;
        }
    }
}
