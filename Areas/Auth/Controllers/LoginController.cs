using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.Enums;
using CodinaxProjectMvc.ViewModel.AuthVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Areas.Auth.Controllers
{
    [Area("Auth")]
    [Authorize(Policy = PolicyConstants.NotAuthRequiredPolicy)]
    public class LoginController : Controller
    {

        private readonly IAuthService _authService;

        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginVm loginVm)
        {
            IEnumerable<string>? roles = await _authService.LoginAsync(loginVm);

            if(roles == null) 
                return View(loginVm);

            if (roles.Any(role => role == Roles.Instructor.ToString()))
                return Redirect("/Instructor/Courses");
            else if (roles.Any(role => role == Roles.Student.ToString()))
                return Redirect("/Student/Applications");
            else
                return Redirect("/Admin/Instructors");
        }
    }
}
