using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Policy = PolicyConstants.InstructorPolicy)]
    public class CoursesController : Controller
    {
        private readonly IInstructorService _instructorService;

        public CoursesController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        public async Task<IActionResult> Index()
            => View(await _instructorService.GetInstructorCoursesAsync(HttpContext.User.Identity.Name));


        public async Task<IActionResult> Single(Guid id)
            => View(await _instructorService.GetInstructorCourseSingleAsync(HttpContext.User.Identity.Name, id));
    }
}
