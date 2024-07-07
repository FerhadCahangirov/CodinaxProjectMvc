using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.DataAccess.Models.Identity;
using CodinaxProjectMvc.ViewModel.AdminVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = PolicyConstants.AdminPolicy)]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Profile()
        {
            AppUser user = _userManager.GetUsersInRoleAsync("Admin").Result[0];

            AdminAccountVm vm = new AdminAccountVm()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.Email,
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(AdminAccountVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            AppUser user = _userManager.GetUsersInRoleAsync("Admin").Result[0];

            user.FirstName = vm.FirstName;
            user.LastName = vm.LastName;

            await _userManager.UpdateAsync(user);

            TempData["ProfileUpdated"] = true;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(AdminResetPasswordVm passwordVm)
        {

            AppUser user = _userManager.GetUsersInRoleAsync("Admin").Result[0];

            AdminAccountVm vm = new AdminAccountVm()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.Email,
            };

            if (!ModelState.IsValid)
            {
                vm.Password = passwordVm.Password;
                vm.ConfirmPassword = passwordVm.ConfirmPassword;
                return View(viewName: nameof(Profile), vm);
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            IdentityResult result = await _userManager.ResetPasswordAsync(user, token, passwordVm.Password);

            if (result.Succeeded)
            {
                TempData["PasswordUpdated"] = true;
            }

            return View(viewName: nameof(Profile), vm);
        }
    }
}
