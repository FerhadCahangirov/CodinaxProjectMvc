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
            IEnumerable<Course> courses = _courseReadRepository.Table
                .Include(c => c.Category)
                .Include(c => c.Template)
                .OrderBy(OrderFilters.ByTitle)
                .Where(QueryFilters.CoursesLayoutFilter())
                .ToList();

            List<Category> categories = courses.Select(c => c.Category).Distinct().ToList();

            List<Instructor> instructors = _instructorReadRepository.Table
                .Include(x => x.Courses)
                .ThenInclude(x => x.Students)
                .Where(UserQueryFilters<Instructor>.GeneralFilter)
                .Select(x => new
                {
                    Instructor = x,
                    TotalStudents = x.Courses.SelectMany(c => c.Students).Count()
                })
                .OrderBy(x => x.TotalStudents)
                .Take(4)
                .Select(x => x.Instructor)
                .ToList();

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

        public async Task<IActionResult> CoursesPartial(string? categoryName = null)
        {
            IQueryable<Course>? courses = _courseReadRepository.Table
                .Include(c => c.Category)
                .Include(c => c.Template)
                .OrderBy(OrderFilters.ByTitle)
                .Where(QueryFilters.CoursesLayoutFilter()).AsQueryable();

            List<Category> categories = courses.Select(c => c.Category).Distinct().ToList();

            if (!string.IsNullOrEmpty(categoryName) && courses != null)
            {
                courses = courses.Where(x => x.Category.Content.ToLower() == categoryName.ToLower());
            }

            List<Instructor> instructors = _instructorReadRepository.Table
                .Include(x => x.Courses)
                .ThenInclude(x => x.Students)
                .Where(UserQueryFilters<Instructor>.GeneralFilter)
                .Select(x => new
                {
                    Instructor = x,
                    TotalStudents = x.Courses.SelectMany(c => c.Students).Count()
                })
                .OrderBy(x => x.TotalStudents)
                .Take(4)
                .Select(x => x.Instructor)
                .ToList();

            List<Faq> faqs = await _faqReadRepository.Table
               .Where(x => !x.IsDeleted && !x.IsArchived)
               .ToListAsync();

            CoursesVm coursesVm = new CoursesVm()
            {
                Courses = courses?.ToList(),
                Categories = categories,
                Instructors = instructors,
                Faqs = faqs,
                BaseUrl = _configuration["BaseUrl:Azure"],
            };
            return PartialView(viewName: nameof(CoursesPartial), coursesVm);
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
            .FirstOrDefaultAsync(QueryFilters.CoursesLayoutFilter(id));

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
                BaseUrl = _configuration["BaseUrl:Azure"],
            };

            if(advice != null)
            {
                if(advice.FirstAdvicedCourse != null && advice.FirstAdvicedCourse.Showcase)
                {
                    courseSingleVm.FirstAdvicedCourse = advice.FirstAdvicedCourse;  
                }

                if(advice.SecondAdvicedCourse != null && advice.SecondAdvicedCourse.Showcase)
                {
                    courseSingleVm.SecondAdvicedCourse = advice.SecondAdvicedCourse;
                }
            }

            return View(courseSingleVm);
        }
    }
}
