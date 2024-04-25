using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Areas.Student.Controllers
{
    public class CoursesController : Controller
    {
        [Area("Student")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
