using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.ViewModel.StudentVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Policy = PolicyConstants.StudentPolicy)]
    public class AccountController : Controller
    {

        private readonly IStudentService _studentService;

        public AccountController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task<IActionResult> Profile()
        {
            StudentAccountVm studentAccountVm = await _studentService.GetProfileDataAsync();
            return View(studentAccountVm);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(StudentAccountVm studentAccountVm)
        {
            bool result = await _studentService.UpdateProfileAsync(studentAccountVm);

            if (!result)
                return View(studentAccountVm);

            TempData["ProfileUpdated"] = true;
            return Redirect($"/Student/Account/Profile");
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfileImage([FromRoute] Guid id, [FromForm] IFormFile file)
        {
            bool result = await _studentService.UploadProfileImageAsync(id, file);
            return new JsonResult(new { success = result });
        }

        [HttpPut]
        public async Task<IActionResult> ChangeProfileImage(Guid id, IFormFile file)
        {
            bool result = await _studentService.ChangeProfileImageAsync(id, file);
            return new JsonResult(new { success = result });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProfileImage(Guid id)
        {
            bool result = await _studentService.DeleteProfileImageAsync(id);
            return new JsonResult(new { success = result });
        }

        [HttpPost]

        public async Task<IActionResult> ChangePassword(StudentResetPasswordVm studentResetPasswordVm)
        {
            bool result = await _studentService.ChangePasswordAsync(studentResetPasswordVm);
            var studentAccountVm = await _studentService.GetProfileDataAsync();

            if (!result)
            {
                return View(viewName: nameof(Profile), studentAccountVm);
            }

            TempData["PasswordUpdated"] = true;
            return View(viewName: nameof(Profile), studentAccountVm);
        }

    }
}
