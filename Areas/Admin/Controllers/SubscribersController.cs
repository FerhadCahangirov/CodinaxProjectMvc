using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.ViewModel.SubscribeVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = PolicyConstants.AdminPolicy)]
    public class SubscribersController : Controller
    {
        private readonly ISubscriberService _subscriberService;

        public SubscribersController(ISubscriberService subscriberService)
        {
            _subscriberService = subscriberService;
        }

        public async Task<IActionResult> SubscribersPartial(string? searchFilter, string? statusFilter)
            => PartialView(viewName: nameof(SubscribersPartial) ,await _subscriberService.ListSubscribersAsync(searchFilter, statusFilter));


        public async Task<IActionResult> Index()
            => View(await _subscriberService.ListSubscribersAsync());

        public IActionResult Send()
            => View();

        [HttpPost]
        public async Task<IActionResult> Send(SubscribeSendVm subscribeSendVm)
        {
            bool result = await _subscriberService.SendAsync(subscribeSendVm);

            if (!result)
            {
                return View(subscribeSendVm);
            }   

            TempData["SubscribeMailSentSuccess"] = true;
            return Redirect($"/Admin/Subscribers/{nameof(Index)}");   
        }
    }
}
