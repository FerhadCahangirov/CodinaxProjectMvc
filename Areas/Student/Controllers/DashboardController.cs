using CodinaxProjectMvc.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Areas.Student.Controllers
{
    [Area("Student")]       
    [Authorize(Policy = PolicyConstants.StudentPolicy)]

    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
