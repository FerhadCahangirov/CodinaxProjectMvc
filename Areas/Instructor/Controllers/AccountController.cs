using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Business.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.ViewModel.InstructorVm;
using CodinaxProjectMvc.ViewModel.StudentVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Policy = PolicyConstants.InstructorPolicy)]
    public class AccountController : Controller
    {
        private readonly IInstructorService _instructorService;

        public AccountController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        public async Task<IActionResult> Profile()
        {
            InstructorAccountVm instructorAccountVm = await _instructorService.GetProfileDataAsync();
            return View(instructorAccountVm);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(InstructorAccountVm instructorAccountVm)
        {
            bool result = await _instructorService.UpdateProfileAsync(instructorAccountVm);

            if (!result)
                return View(instructorAccountVm);

            TempData["ProfileUpdated"] = true;
            return Redirect($"/Instructor/Account/Profile");
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfileImage([FromRoute] Guid id, [FromForm] IFormFile file)
        {
            bool result = await _instructorService.UploadProfileImageAsync(id, file);
            return new JsonResult(new { success = result });
        }

        [HttpPut]
        public async Task<IActionResult> ChangeProfileImage(Guid id, IFormFile file)
        {
            bool result = await _instructorService.ChangeProfileImageAsync(id, file);
            return new JsonResult(new { success = result });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProfileImage(Guid id)
        {
            bool result = await _instructorService.DeleteProfileImageAsync(id);
            return new JsonResult(new { success = result });
        }

        [HttpPost]

        public async Task<IActionResult> ChangePassword(InstructorResetPasswordVm instructorResetPasswordVm)
        {
            bool result = await _instructorService.ChangePasswordAsync(instructorResetPasswordVm);
            var instructorAccountVm = await _instructorService.GetProfileDataAsync();

            if (!result)
            {
                instructorAccountVm.Password = instructorResetPasswordVm.Password;
                instructorAccountVm.ConfirmPassword = instructorResetPasswordVm.ConfirmPassword;
                return View(viewName: nameof(Profile), instructorAccountVm);
            }

            TempData["PasswordUpdated"] = true;
            return View(viewName: nameof(Profile), instructorAccountVm);
        }

    }
}
