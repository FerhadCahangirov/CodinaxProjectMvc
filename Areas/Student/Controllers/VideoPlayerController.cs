using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Areas.Student.Controllers
{
    [Area("Student")]
    [Route("[area]/video-player")]
    public class VideoPlayerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
