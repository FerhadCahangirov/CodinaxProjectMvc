using CodinaxProjectMvc.Context;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.Filters;
using CodinaxProjectMvc.ViewModel.CourseVm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Controllers
{
    [CurrentLangFilterFactory]
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

        public IActionResult Index()
        {
            IEnumerable<Course> courses = _courseReadRepository.Table
                .Include(c => c.Category)
                .Include(c => c.Template)
                .OrderBy(OrderFilters.ByTitle)
                .Where(QueryFilters.CoursesLayoutFilter())
                .ToList();

            List<Category> categories = courses.Select(c => c.Category).Distinct().ToList();

            List<Instructor> instructors = _instructorReadRepository.Table
                .Where(x => !x.IsDeleted && x.Showcase && x.IsApproved && !x.IsBanned).ToList();

            CoursesVm coursesVm = new CoursesVm()
            {
                Courses = courses,
                Categories = categories,
                Instructors = instructors,
                BaseUrl = _configuration["BaseUrl:Azure"],
            };
            return View(coursesVm);
        }

        public IActionResult CoursesPartial(string? categoryName = null)
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

            List<Instructor> instructors = _instructorReadRepository
                .GetWhere(x => !x.IsDeleted && x.Showcase && x.IsApproved && !x.IsBanned).ToList();

            CoursesVm coursesVm = new CoursesVm()
            {
                Courses = courses?.ToList(),
                Categories = categories,
                Instructors = instructors,
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
