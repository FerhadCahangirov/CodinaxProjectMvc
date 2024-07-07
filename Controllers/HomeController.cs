using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Context;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.Filters;
using CodinaxProjectMvc.ViewModel.LayoutVm;
using CodinaxProjectMvc.Views.Home;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Controllers
{
    [CurrentLangFilterFactory]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CodinaxDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly ICorporateService _corporateService;

        public HomeController(ILogger<HomeController> logger, CodinaxDbContext db, IConfiguration configuration, ICorporateService corporateService)
        {
            _logger = logger;
            _db = db;
            _configuration = configuration;
            _corporateService = corporateService;
        }

        public async Task<IActionResult> Index()
        {
            List<Course> courses = _db.Courses
                .Include(x => x.Students)
                .Include(x => x.Template)
                .Where(QueryFilters.CoursesLayoutFilter())
                .Where(x => x.IsPrimary)
                .OrderBy(OrderFilters.ByTitle)
                .ToList();

            List<Instructor> instructors = _db.Instructors
                .Where(x => !x.IsDeleted && x.Showcase && x.IsApproved && !x.IsBanned).ToList();

            HomeVm homeVm = new HomeVm()
            {
                Courses = courses,
                Instructors = instructors,
                Corporates =  await _corporateService.ListApprovedCorporatesAsync(),
                BaseUrl = _configuration["BaseUrl:Azure"]
            };

            return View(homeVm);
        }

        public IActionResult ChangeLang(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)), new CookieOptions() 
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1)
            });
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult Error(ErrorModel errorModel)
        {
            return View(errorModel);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Privacy()
            => View();

        public IActionResult Terms()
            => View();

    }
}