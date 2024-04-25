using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.Context;
using CodinaxProjectMvc.ViewModel.InstructorVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Policy = PolicyConstants.InstructorPolicy)]
    public class AccountController : Controller
    {
        private readonly CodinaxDbContext _db;

        public AccountController(CodinaxDbContext db)
        {
            _db = db;
        }

        private async Task<DataAccess.Models.Instructor> GetActiveInstructorAsync()
            => await _db.Instructors.FirstAsync(x => x.UserName == HttpContext.User.Identity.Name);

        public async Task<IActionResult> Settings()
        {
            var instructor = await GetActiveInstructorAsync();
            InstructorAccountVm instructorVm = instructor.FromInstructor_ToInstructorAccountVm();
            return View(instructorVm);
        }

        [HttpPost]
        public async Task<IActionResult> Settings(InstructorAccountVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var instructor = await GetActiveInstructorAsync();

            instructor.FirstName = vm.FirstName;
            instructor.LastName = vm.LastName;
            instructor.PhoneNumber = vm.PhoneNumber;

            _db.Instructors.Update(instructor);
            await _db.SaveChangesAsync();

            TempData["UpdatedProfile"] = true;
            return Redirect("/Instructor/Account/Settings");
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage()
        {
            return View();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateImage()
        {
            return View();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteImage()
        {
            return View();
        }
    }
}
