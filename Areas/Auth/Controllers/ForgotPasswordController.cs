using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.Managers.Abstract;
using CodinaxProjectMvc.ViewModel.AuthVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Areas.Auth.Controllers
{
    [Area("Auth")]
    [Authorize(Policy = PolicyConstants.NotAuthRequiredPolicy)]
    public class ForgotPasswordController : Controller
    {
        private readonly IAuthService _authService;

        public ForgotPasswordController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Send(ForgotPasswordVm forgotPasswordVm)
        {
            await _authService.SendForgotPasswordAsync(forgotPasswordVm);
            TempData["ForgotPasswordSent"] = true;
            return View(viewName: nameof(Index), forgotPasswordVm);
        }

        public IActionResult Reset(string email, string token)
        {
            return View(new ResetPasswordVm(email, token));
        }

        [HttpPost]
        public async Task<IActionResult> Reset(ResetPasswordVm resetPasswordVm)
        {
            bool result = await _authService.ResetPasswordAsync(resetPasswordVm);

            if (!result)
            {
                return View(resetPasswordVm);
            }

            TempData["PasswordReset"] = true;
            return Redirect("/Auth/Login");
        }
    }
}
