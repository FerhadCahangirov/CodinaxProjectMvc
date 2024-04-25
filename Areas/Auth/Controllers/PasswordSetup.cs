using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.DataAccess.Models.Identity;
using CodinaxProjectMvc.ViewModel.AccountVm;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Areas.Auth.Controllers
{
    [Area("Auth")]
    public class PasswordSetup : Controller
    {
        private readonly IAuthService _authService;
        private readonly UserManager<AppUser> _userManager;
        public PasswordSetup(IAuthService authService, UserManager<AppUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string token, string email)
        {
            AppUser? user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                Redirect("/home/error");
            }

            PasswordSetupVm vm = new PasswordSetupVm()
            {
                EmailAddress = email,
                Token = token,
                Password = string.Empty,
                ConfirmPassword = string.Empty
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Setup(PasswordSetupVm passwordSetupVm)
        {
            bool result = await _authService.SetupPasswordAsync(passwordSetupVm);
            if (!result)
            {
                return View(viewName: nameof(Index), passwordSetupVm);
            }

            return Redirect("/");
        }
    }
}
