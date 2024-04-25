using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Business.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.Context;
using CodinaxProjectMvc.DataAccess.Abstract.Storages;
using CodinaxProjectMvc.ViewModel;
using CodinaxProjectMvc.ViewModel.StudentVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = PolicyConstants.AdminPolicy)]
    public class StudentsController : Controller
    {
        private readonly CodinaxDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly IAzureStorage _storage;

        private readonly IStudentService _studentService;

        private const string _studentsPartialConstant = "StudentsPartial";

        public StudentsController(CodinaxDbContext db, IAzureStorage storage, IConfiguration configuration, IStudentService studentService)
        {
            _db = db;
            _storage = storage;
            _configuration = configuration;
            _studentService = studentService;
        }

        public async Task<IActionResult> StudentsPartial(string? searchFilter,string? statusFilter, int page = 1, int size = 4)
        {
            var pagination = await _studentService.GetStudentsPaginationAsync(searchFilter, statusFilter, page, size);

            if (pagination.Items?.ToList().Count == 0)
            {
                ViewBag.Message = "Nothing Found";
            }

            return PartialView(viewName: _studentsPartialConstant, pagination);
        }

        public async Task<IActionResult> Index()
        {
            var pagination = await _studentService.GetStudentsPaginationAsync();

            if (pagination.Items?.ToList().Count == 0)
            {
                ViewBag.Message = "Nothing Found";
            }

            return View(pagination);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveStudent(Guid id)
        {
            bool result = await _studentService.ApproveAsync(id);
            return new JsonResult(new { success = result });
        }

        [HttpPost]
        public async Task<IActionResult> BanStudent(Guid id)
        {
            bool result = await _studentService.BanAsync(id);
            return new JsonResult(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> UnBanStudent(Guid id)
        {
            bool result = await _studentService.UnbanAsync(id);
            return new JsonResult(new { success = result });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            DataAccess.Models.Student? student = await _db.Students.FirstOrDefaultAsync(x => x.Id == id);

            if (student == null) return NotFound();

            _db.Students.Remove(student);
            await _db.SaveChangesAsync();

            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> Profile(Guid id)
        {
            StudentAccountVm studentAccountVm = await _studentService.GetProfileDataAsync(id);
            return View(studentAccountVm);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(StudentAccountVm studentAccountVm)
        {
            bool result = await _studentService.UpdateProfileAsync(studentAccountVm);

            if (!result)
                return View(studentAccountVm);
                    
            TempData["ProfileUpdated"] = true;
            return Redirect($"/Admin/Students/Profile/{studentAccountVm.Id}");
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfileImage([FromRoute] Guid id, [FromForm] IFormFile file)
        {
            bool result = await _studentService.UploadProfileImageAsync(id, file);
            return new JsonResult(new { success = result});
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
        public async Task<IActionResult> SendConfirmationMail(Guid id)
            => new JsonResult(new { success = await _studentService.SendConfirmationMailAsync(id) });
    }
}
