using CodinaxProjectMvc.Context;
using CodinaxProjectMvc.DataAccess;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel.CourseVm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Controllers
{
    public class CoursesController : Controller
    {
        private readonly IConfiguration _configuration;
        
        private readonly IReadRepository<Course> _courseReadRepository;
        private readonly IReadRepository<Advice> _adviceReadRepository;
        private readonly IReadRepository<Faq> _faqReadRepository;
        private readonly IReadRepository<Instructor> _instructorReadRepository;

        public CoursesController(
            IConfiguration configuration,
            IReadRepository<Course> courseReadRepository,
            IReadRepository<Advice> adviceReadRepository,
            IReadRepository<Faq> faqReadRepository,
            IReadRepository<Instructor> instructorReadRepository)
        {
            _configuration = configuration;
            _courseReadRepository = courseReadRepository;
            _adviceReadRepository = adviceReadRepository;
            _faqReadRepository = faqReadRepository;
            _instructorReadRepository = instructorReadRepository;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Course> courses = await _courseReadRepository.Table
                .Include(c => c.Category)
                .Include(c => c.Template)
                .Where(c => !c.IsDeleted && !c.IsArchived)
                .ToListAsync();

            List<Category> categories = courses.Select(c => c.Category).Distinct().ToList();

            List<Instructor> instructors = await _instructorReadRepository.Table
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

            List<Faq> faqs = await _faqReadRepository.Table
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
            Course? course = await _courseReadRepository.Table
            .Include(x => x.Category)
            .Include(x => x.Template)
            .ThenInclude(x => x.Topics)
            .Include(x => x.Template)
            .ThenInclude(x => x.Tools)
            .Include(x => x.Template)
            .ThenInclude(x => x.FutureJobTitles)
            .Include(x => x.Template)
            .ThenInclude(x => x.Prices)
            .ThenInclude(x => x.PriceInfos)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted && !x.IsArchived);

            if(course == null) return NotFound();

            Advice? advice = await _adviceReadRepository.Table
               .Include(x => x.FirstAdvicedCourse)
               .ThenInclude(x => x.Category)
               .Include(x => x.FirstAdvicedCourse)
               .ThenInclude(x => x.Template)
               .Include(x => x.SecondAdvicedCourse)
               .ThenInclude(x => x.Category)
               .Include(x => x.SecondAdvicedCourse)
               .ThenInclude(x => x.Template)
               .Include(x => x.MainCourse)
               .FirstOrDefaultAsync(x => x.MainCourse == course);


            CourseSingleVm courseSingleVm = new CourseSingleVm(){
                Course = course,
                FirstAdvicedCourse = advice?.FirstAdvicedCourse,
                SecondAdvicedCourse = advice?.SecondAdvicedCourse,
                BaseUrl = _configuration["BaseUrl:Azure"],
            };

            return View(courseSingleVm);
        }
    }
}
