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

        [HttpPut]
        public async Task<IActionResult> SetCoursePrimary(Guid id)
            => ReturnActionJson(await _courseService.SetCoursePrimaryAsync(id));

        public async Task<IActionResult> Single(Guid id)
        {
            CourseSingleVm courseSingleVm = await _courseService.GetCourseSingleAsync(id);
            return View(courseSingleVm);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCourse(Guid id)
            => new JsonResult(new { success = await _courseService.DeleteCourseAsync(id) });

        [NonAction]
        private IActionResult ReturnActionJson(PrimaryCourseActionReturnType primaryCourseActionReturnType)
        {
            if (primaryCourseActionReturnType == PrimaryCourseActionReturnType.Oversized)
            {
                return new JsonResult(new { success = false, message = FeatureActionReturnType.Oversized.ToString() });
            }

            if (primaryCourseActionReturnType == PrimaryCourseActionReturnType.Failure)
            {
                return new JsonResult(new { success = false, message = FeatureActionReturnType.Failure.ToString() });
            }

            return new JsonResult(new { success = true });

        }

        #endregion

        #region Manage Course Instructors Controllers
        [HttpPost]
        public async Task<IActionResult> AssignInstructorsPartial([FromRoute] Guid id, string? searchFilter)
        {
            var data = await _courseService.GetAssignableInstructorsAsync(id, searchFilter);

            if (data.InstructorPagination?.Items?.Count() == 0)
            {
                ViewBag.Message = "Nothing Found";
            }

            return PartialView(viewName: "AssignInstructorPartial", data);
        }

        [HttpGet]
        public async Task<IActionResult> AssignInstructors([FromRoute] Guid id)
        {

            var data = await _courseService.GetAssignableInstructorsAsync(id);

            if (data.InstructorPagination?.Items?.Count() == 0)
            {
                ViewBag.Message = "Nothing Found";
            }

            return View(data);
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

        #region Manage Course Students Controllers
        [HttpPost]
        public async Task<IActionResult> AssignStudentsPartial([FromRoute] Guid id, string? searchFilter)
        {
            var data = await _courseService.GetAssignableStudentsAsync(id, searchFilter);

            if (data.StudentPagination?.Items?.Count() == 0)
            {
                ViewBag.Message = "Nothing Found";
            }

            return PartialView(viewName: "AssignStudentPartial", data);
        }

        [HttpGet]
        public async Task<IActionResult> AssignStudents([FromRoute] Guid id)
        {

            var data = await _courseService.GetAssignableStudentsAsync(id);

            if (data.StudentPagination?.Items?.Count() == 0)
            {
                ViewBag.Message = "Nothing Found";
            }

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> AssignStudent(Guid courseId, Guid studentId)
        {
            bool result = await _courseService.AssignStudentAsync(courseId, studentId);
            return new JsonResult(new { success = result });
        }

        [HttpPost]
        public async Task<IActionResult> ReassignStudent(Guid courseId, Guid studentId)
        {
            await _courseService.ReassignStudentAsync(courseId, studentId);
            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> CourseStudentsPartial(Guid id, string? searchFilter)
        {
            CourseStudentsVm courseStudentsVm = await _courseService.GetCourseStudentsAsync(id, searchFilter);
            return PartialView(viewName: "CourseStudentsPartial", courseStudentsVm);
        }

        #endregion


    }
}

