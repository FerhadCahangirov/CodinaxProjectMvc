using CodinaxProjectMvc.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Areas.Instructor.Controllers
{
    public class StudentsController : Controller
    {
        [Area("Instructor")]
        [Authorize(Policy = PolicyConstants.InstructorPolicy)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
