using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.ViewModel.InstructorVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = PolicyConstants.AdminPolicy)]
    public class InstructorsController : Controller
    {
        private readonly IInstructorService _instructorService;
        private const string _instructorsPartialConstant = "InstructorsPartial";

        public InstructorsController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        [HttpPost]
        public async Task<IActionResult> InstructorsPartial(string? searchFilter, string? statusFilter)
        {
            var pagination = await _instructorService.GetInstructorPaginationAsync(searchFilter, statusFilter);

            if (pagination.Items?.ToList().Count == 0)
            {
                ViewBag.Message = "Nothing Found";
            }

            return PartialView(viewName: _instructorsPartialConstant, pagination);
        }

        public async Task<IActionResult> Index()
        {
            var pagination = await _instructorService.GetInstructorPaginationAsync();

            if (pagination.Items?.ToList().Count == 0)
            {
                ViewBag.Message = "Nothing Found";
            }

            return View(pagination);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveInstructor(Guid id)
        {
            bool result = await _instructorService.ApproveAsync(id);

            return new JsonResult(new { success = result });
        }

        [HttpPost]
        public async Task<IActionResult> BanInstructor(Guid id)
        {
            bool result = await _instructorService.BanAsync(id);
            return new JsonResult(new { success = result });
        }

        [HttpPost]
        public async Task<IActionResult> UnBanInstructor(Guid id)
        {
            bool result = await _instructorService.UnbanAsync(id);
            return new JsonResult(new { success = result });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteInstructor(Guid id)
            => new JsonResult(new { success = await _instructorService.DeleteAsync(id) });

        public async Task<IActionResult> Profile(Guid id)
        {
            InstructorAccountVm instructorAccountVm = await _instructorService.GetProfileDataAsync(id);
            return View(instructorAccountVm);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(InstructorAccountVm instructorAccountVm)
        {
            bool result = await _instructorService.UpdateProfileAsync(instructorAccountVm);

            if (!result)
                return View(instructorAccountVm);

            TempData["ProfileUpdated"] = true;
            return Redirect($"/Admin/Instructors/Profile/{instructorAccountVm.Id}");
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
        public async Task<JsonResult> SendConfirmationMail(Guid id)
            => new JsonResult(new { success = await _instructorService.SendConfirmationMailAsync(id) });

        [HttpPost]
        public async Task<JsonResult> SendPasswordGenerateMail(Guid id)
            => new JsonResult(new { success = await _instructorService.SendPasswordGenerateMailAsync(id) });


    }
}
