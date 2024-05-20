using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Business.PersistenceServices;
using CodinaxProjectMvc.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Policy = PolicyConstants.StudentPolicy)]
    public class CoursesController : Controller
    {
        private readonly IStudentService _studentService;

        public CoursesController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task<IActionResult> Index()
            => View(await _studentService.GetStudentCoursesAsync(HttpContext.User.Identity.Name));

        public async Task<IActionResult> Single(Guid id)
           => View(await _studentService.GetStudentCourseSingleAsync(HttpContext.User.Identity.Name, id));
    }
}
