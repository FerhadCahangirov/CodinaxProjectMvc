using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Controllers
{
    public class CorporateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
