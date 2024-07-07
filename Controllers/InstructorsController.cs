using CodinaxProjectMvc.Context;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.Filters;
using CodinaxProjectMvc.ViewModel.InstructorVm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Controllers
{
    [CurrentLangFilterFactory]
    public class InstructorsController : Controller
    {
        private readonly CodinaxDbContext _db;
        private readonly IConfiguration _configuration;

        public InstructorsController(CodinaxDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            IEnumerable<Instructor> instructors = _db.Instructors
                .Where(UserQueryFilters<Instructor>.GeneralFilter).ToList();

            InstructorsVm instructorsVm = new InstructorsVm() 
            { 
                Instructors = instructors,
                BaseUrl = _configuration["BaseUrl:Azure"],
            };

            return View(instructorsVm);
        }
    }
}
