using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.Filters;
using CodinaxProjectMvc.ViewModel.CourseVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = PolicyConstants.AdminPolicy)]
    public class CoursesController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IInstructorService _instructorService;
        private readonly IReadRepository<Course> _courseReadRepository;
        private readonly IReadRepository<Category> _categoryReadRepository;
        private readonly IReadRepository<Tool> _toolReadRepository;
        private readonly IReadRepository<Template> _templateReadRepository;

        public CoursesController(ICourseService courseService,
            IReadRepository<Course> courseReadRepository,
            IReadRepository<Category> categoryReadRepository,
            IInstructorService instructorService,
            IReadRepository<Tool> toolReadRepository,
            IReadRepository<Template> templateReadRepository)
        {
            _courseService = courseService;
            _courseReadRepository = courseReadRepository;
            _categoryReadRepository = categoryReadRepository;
            _instructorService = instructorService;
            _toolReadRepository = toolReadRepository;
            _templateReadRepository = templateReadRepository;
        }

        #region Manage Course Controllers
        public async Task<IActionResult> Index()
            => View(await _courseService.GetCoursesAsync());
        
        [PropertyAccessCourseFilterFactory]
        public IActionResult Create()
            => View();

        [HttpPost]
        [PropertyAccessCourseFilterFactory]
        public async Task<IActionResult> Create([FromForm] CourseCreateVm courseCreateVm)
        {
            bool result = await _courseService.CreateCourseAsync(courseCreateVm);

            if (!result)
                return View(courseCreateVm);

            TempData["CourseCreatedSuccessfull"] = true ;
            return Redirect($"/Admin/Courses/{nameof(Index)}");
        }

        [PropertyAccessCourseFilterFactory]
        public async Task<IActionResult> Update(Guid id)
            => View(await _courseService.GetCourseUpdateDataAsync(id));

        [HttpPost]
        [PropertyAccessCourseFilterFactory]
        public async Task<IActionResult> Update(CourseUpdateVm courseUpdateVm)
        {
            bool result = await _courseService.UpdateCourseAsync(courseUpdateVm);

            if (!result)
                return View(courseUpdateVm);

            TempData["CourseUpdatedSuccessfull"] = true;
            return Redirect($"/Admin/Courses/{nameof(Index)}");
        }

        [HttpPut]
        public async Task<IActionResult> ChangeShowcase(Guid id)
            => new JsonResult(new { success = await _courseService.ChangeShowcaseAsync(id) });


        public async Task<IActionResult> Single(Guid id)
        {
            CourseSingleVm courseSingleVm = await _courseService.GetCourseSingleAsync(id);
            return View(courseSingleVm);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCourse(Guid id)
            => new JsonResult(new { success = await _courseService.DeleteCourseAsync(id) });

        #endregion

        #region Manage Course Instructors Controllers
        [HttpPost]
        public async Task<IActionResult> AssignInstructorsPartial([FromRoute] Guid id, string? searchFilter, int page = 0, int size = 10)
        {
            Course course = await _courseReadRepository.GetByIdAsync(Convert.ToString(id));

            var pagination = await _instructorService.GetInstructorPaginationAsync(searchFilter: searchFilter, page: page, size: size);

            CourseInstructorsAssignVm courseInstructorsAssignVm = new CourseInstructorsAssignVm()
            {
                InstructorPagination = pagination,
                Course = course
            };


            if (pagination.Items?.ToList().Count == 0)
            {
                ViewBag.Message = "Nothing Found";
            }

            return PartialView(viewName: "AssignInstructorPartial", courseInstructorsAssignVm);

        }

        [HttpGet]
        public async Task<IActionResult> AssignInstructors([FromRoute] Guid id)
        {

            Course? course = await _courseReadRepository.Table
                .Include(x => x.Instructors)
                .Include(x => x.Students)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (course == null) return NotFound();

            var pagination = await _instructorService.GetInstructorPaginationAsync(string.Empty);

            CourseInstructorsAssignVm courseInstructorsAssignVm = new CourseInstructorsAssignVm()
            {
                InstructorPagination = pagination,
                Course = course,
            };

            if (pagination.Items?.Count() == 0)
            {
                ViewBag.Message = "Nothing Found";
            }

            return View(courseInstructorsAssignVm);

        }

        [HttpPost]
        public async Task<IActionResult> AssignInstructor(Guid courseId, Guid instructorId)
        {
            bool result = await _courseService.AssignInstructorAsync(courseId, instructorId);
            return new JsonResult(new { success = result });
        }

        [HttpPost]
        public async Task<IActionResult> ReassignInstructor(Guid courseId, Guid instructorId)
        {
            await _courseService.ReassignInstructorAsync(courseId, instructorId);
            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> CourseInstructorsPartial(Guid id, string? searchFilter)
        {
            CourseInstructorsVm courseInstructorsVm = await _courseService.GetCourseInstructorsAsync(id, searchFilter);
            return PartialView(viewName: "CourseInstructorsPartial", courseInstructorsVm);
        }

        #endregion

        
    }
}

