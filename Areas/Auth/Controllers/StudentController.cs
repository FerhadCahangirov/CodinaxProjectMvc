using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.Context;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.DataAccess.Models.Identity;
using CodinaxProjectMvc.ViewModel.StudentVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CodinaxProjectMvc.Areas.Auth.Controllers
{
    [Area("Auth")]
    [Authorize(Policy = PolicyConstants.NotAuthRequiredPolicy)]
    public class StudentController : Controller
    {
        private readonly IReadRepository<Course> _courseReadRepository;
        private readonly IAuthService _authService;

        public StudentController(IReadRepository<Course> courseReadRepository, IAuthService authService)
        {
            _courseReadRepository = courseReadRepository;
            _authService = authService;
        }

        public IActionResult Apply()
        {
            ViewBag.Courses = new SelectList(
                 _courseReadRepository
                 .GetWhere(x => !x.IsDeleted && !x.IsArchived)
                 .Select(c => new { c.Id, Text = c.Title }),
                     "Id",
                     "Text"
                   );

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Apply(StudentApplyVm studentApplyVm)
        {
            bool result = await _authService.StudentApplyAsync(studentApplyVm);

            if (!result)
            {
                ViewBag.Courses = new SelectList(
                     _courseReadRepository
                        .GetWhere(x => !x.IsDeleted && !x.IsArchived)
                        .Select(c => new { c.Id, Text = c.Title }),
                             "Id",
                             "Text"
                    );
                return View(studentApplyVm);
            }

            TempData["StudentApplySuccess"] = true;
            return Redirect("/Home/Index");
        }
    }
}
