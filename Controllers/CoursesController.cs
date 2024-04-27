using CodinaxProjectMvc.Context;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel.CourseVm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Controllers
{
    public class CoursesController : Controller
    {
        private readonly CodinaxDbContext _db;
        private readonly IConfiguration _configuration;

        public CoursesController(CodinaxDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Course> courses = await _db.Courses
                .Include(c => c.Instructors)
                .Include(c => c.Students)
                .Include(c => c.Sections)
                .Include(c => c.Category)
                .Where(c => !c.IsDeleted && !c.IsArchived)
                .ToListAsync();

            List<Category> categories = courses.Select(c => c.Category).Distinct().ToList();

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

            List<Faq> faqs = await _db.Faqs
               .Where(x => !x.IsDeleted && !x.IsArchived)
               .ToListAsync();

            CoursesVm coursesVm = new CoursesVm()
            {
                Courses = courses,
                Categories = categories,
                Instructors = instructors,
                Faqs = faqs,
                BaseUrl = _configuration["BaseUrl:Azure"],
            };
            return View(coursesVm);
        }

        [HttpGet]
        public async Task<IActionResult> Single(Guid id)
        {
            Course? course = await _db.Courses
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted && !x.IsArchived);

            if(course == null) return NotFound();

            CourseSingleVm courseSingleVm = new CourseSingleVm(){
                Course = course,
                BaseUrl = _configuration["BaseUrl:Azure"],
            };

            return View(courseSingleVm);
        }
    }
}
