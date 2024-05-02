using CodinaxProjectMvc.Context;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel.LayoutVm;
using CodinaxProjectMvc.Views.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CodinaxDbContext _db;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, CodinaxDbContext db, IConfiguration configuration)
        {
            _logger = logger;
            _db = db;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            List<About> abouts = await _db.Abouts
               .Where(x => !x.IsDeleted && !x.IsArchived)
               .ToListAsync();

            List<Faq> faqs = await _db.Faqs
               .Where(x => !x.IsDeleted && !x.IsArchived)
               .ToListAsync();

            List<Course> courses = _db.Courses
                .Include(x => x.Students)
                .Include(x => x.Template)
                .Where(QueryFilters.CoursesLayoutFilter())
                .OrderBy(x => x.Students.Count())
                .Take(3)
                .ToList();

            List<Instructor> instructors = await _db.Instructors
                .Include(x => x.Courses)
                .ThenInclude(x => x.Students)
                .Where(x => x.IsApproved && !x.IsBanned)
                .Select(x => new
                {
                    Instructor = x,
                    TotalStudents = x.Courses.SelectMany(c => c.Students).Count()
                })
                .OrderBy(x => x.TotalStudents)
                .Take(4)
                .Select(x => x.Instructor)
                .ToListAsync();

            HomeVm homeVm = new HomeVm()
            {
                Abouts = abouts,
                Faqs = faqs,
                Courses = courses,
                Instructors = instructors,
                BaseUrl = _configuration["BaseUrl:Azure"]
            };

            return View(homeVm);
        }


        public IActionResult Error(ErrorModel errorModel)
        {
            return View(errorModel);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}