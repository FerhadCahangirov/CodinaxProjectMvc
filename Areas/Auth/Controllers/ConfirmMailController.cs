using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.DataAccess.Models.Identity;
using CodinaxProjectMvc.Managers.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Areas.Auth.Controllers
{
    [Area("Auth")]
    public class ConfirmMailController : Controller
    {
        private readonly IAuthService _authService;

        public ConfirmMailController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string token, string email)
        {
            var result = await _authService.ConfirmMailAsync(token, email);
            if(!result)
            {
                Redirect("/Home/Error");
            }

            TempData["MailConfirmed"] = true;
            return Redirect("/Home/Index");
        }
    }
}
