using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.Enums;
using CodinaxProjectMvc.ViewModel;
using CodinaxProjectMvc.ViewModel.CourseVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public CoursesController(ICourseService courseService, IReadRepository<Course> courseReadRepository, IReadRepository<Category> categoryReadRepository, IInstructorService instructorService, IReadRepository<Tool> toolReadRepository, IReadRepository<Template> templateReadRepository)
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
        {
            IEnumerable<Course> courses = await _courseService.GetCoursesAsync();
            return View(courses);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_categoryReadRepository.GetWhere(x => !x.IsDeleted && !x.IsArchived), nameof(Category.Id), nameof(Category.Content));

            ViewBag.Templates = new SelectList(_templateReadRepository.GetWhere(x => !x.IsDeleted && !x.IsArchived), nameof(Template.Id), nameof(Template.Heading));

            var courseLevels = Enum.GetValues(typeof(CourseLevels));

            var selectListItems = new List<SelectListItem>();

            foreach (var value in courseLevels)
            {
                selectListItems.Add(new SelectListItem
                {
                    Text = Enum.GetName(typeof(CourseLevels), value),
                    Value = ((int)value).ToString()
                });
            }

            ViewBag.CourseLevels = new SelectList(selectListItems, "Value", "Text");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CourseCreateVm courseCreateVm)
        {
            bool result = await _courseService.CreateCourseAsync(courseCreateVm);

            if (!result)
                return View(courseCreateVm);

            TempData["CourseCreatedSuccessfull"] = true ;
            return Redirect($"/Admin/Courses/{nameof(Index)}");
        }

        public async Task<IActionResult> Update(Guid id)
        {
            CourseUpdateVm courseUpdateVm = await _courseService.GetCourseUpdateDataAsync(id);

            ViewBag.Categories = new SelectList(_categoryReadRepository.GetWhere(x => !x.IsDeleted && !x.IsArchived), nameof(Category.Id), nameof(Category.Content));

            ViewBag.Templates = new SelectList(_templateReadRepository.GetWhere(x => !x.IsDeleted && !x.IsArchived), nameof(Template.Id), nameof(Template.Heading));

            var courseLevels = Enum.GetValues(typeof(CourseLevels));

            var selectListItems = new List<SelectListItem>();

            foreach (var value in courseLevels)
            {
                selectListItems.Add(new SelectListItem
                {
                    Text = Enum.GetName(typeof(CourseLevels), value),
                    Value = ((int)value).ToString()
                });
            }

            ViewBag.CourseLevels = new SelectList(selectListItems, "Value", "Text");

            return View(courseUpdateVm);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CourseUpdateVm courseUpdateVm)
        {
            bool result = await _courseService.UpdateCourseAsync(courseUpdateVm);

            if (!result)
                return View(courseUpdateVm);

            TempData["CourseUpdatedSuccessfull"] = true;
            return Redirect($"/Admin/Courses/{nameof(Index)}");
        }

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

