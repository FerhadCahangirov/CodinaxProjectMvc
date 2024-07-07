using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Areas.Auth.Controllers
{
    [Area("Auth")]
    public class SubscribeController : Controller
    {
        private readonly ISubscriberService _subscriberService;

        public SubscribeController(ISubscriberService subscriberService)
        {
            _subscriberService = subscriberService;
        }

        public async Task<IActionResult> Index(string email, string token)
        {
            bool result = await _subscriberService.ConfirmAsync(email, token);
            TempData["SubscriptionSuccess"] = true;
            return Redirect("/Home");
        }

        [HttpPost]
        public async Task<IActionResult> Send(string email)
            => new JsonResult(new { success = await _subscriberService.SubscribeAsync(email) });

        public async Task<IActionResult> UnSubscribe(string email)
        {
            await _subscriberService.UnSubscribeAsync(email);
            return Redirect("/Home/Index");
        }

    }
}
