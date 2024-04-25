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

        public CoursesController(ICourseService courseService, IReadRepository<Course> courseReadRepository, IReadRepository<Category> categoryReadRepository, IInstructorService instructorService, IReadRepository<Tool> toolReadRepository)
        {
            _courseService = courseService;
            _courseReadRepository = courseReadRepository;
            _categoryReadRepository = categoryReadRepository;
            _instructorService = instructorService;
            _toolReadRepository = toolReadRepository;
        }

        #region Manage Course Controllers
        public async Task<IActionResult> Index()
        {
            IEnumerable<Course> courses = await _courseService.GetCoursesAsync();
            return View(courses);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_categoryReadRepository.GetWhere(x => !x.IsDeleted || !x.IsArchived), "Id", "Content");

            var courseLevels = System.Enum.GetValues(typeof(CourseLevels));

            var selectListItems = new List<SelectListItem>();

            foreach (var value in courseLevels)
            {
                selectListItems.Add(new SelectListItem
                {
                    Text = System.Enum.GetName(typeof(CourseLevels), value),
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
            {
                var errors = new List<string>();

                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    if (state?.Errors.Count > 0)
                    {
                        string error = state.Errors.FirstOrDefault().ErrorMessage;
                        errors.Add(error);
                    }
                }

                return new JsonResult(new { success = result, errors = errors });
            }
            return new JsonResult(new { success = result });
        }

        public async Task<IActionResult> Update(Guid id)
        {
            Course? course = await _courseReadRepository.Table
                .Include(x => x.FutureJobTitles)
                .Include(x => x.Topics)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (course == null) return NotFound();

            ViewBag.Categories = new SelectList(_categoryReadRepository.GetWhere(x => !x.IsDeleted || !x.IsArchived), "Id", "Content");

            var courseLevels = System.Enum.GetValues(typeof(CourseLevels));

            var selectListItems = new List<SelectListItem>();

            foreach (var value in courseLevels)
            {
                selectListItems.Add(new SelectListItem
                {
                    Text = System.Enum.GetName(typeof(CourseLevels), value),
                    Value = ((int)value).ToString()
                });
            }

            ViewBag.CourseLevels = new SelectList(selectListItems, "Value", "Text");

            return View(id);
        }

        public async Task<IActionResult> GetUpdateData(Guid id)
        {
            CourseUpdateVm courseUpdateVm = await _courseService.GetCourseUpdateDataAsync(id);
            return Ok(courseUpdateVm);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CourseUpdateVm courseUpdateVm)
        {
            bool result = await _courseService.UpdateCourseAsync(courseUpdateVm);

            if (!result)
            {
                var errors = new List<string>();

                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    if (state?.Errors.Count > 0)
                    {
                        string error = state.Errors.FirstOrDefault().ErrorMessage;
                        errors.Add(error);
                    }
                }

                return new JsonResult(new { success = result, errors = errors });
            }

            return new JsonResult(new { success = result });
        }

        public async Task<IActionResult> Single(Guid id)
        {
            CourseSingleVm courseSingleVm = await _courseService.GetCourseSingleAsync(id);
            return View(courseSingleVm);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveCourseFragmentVideo(Guid id)
            => new JsonResult(new { success = await _courseService.RemoveCourseFragmentVideoAsync(id) });

        [HttpDelete]
        public async Task<IActionResult> DeleteCourse(Guid id)
            => new JsonResult(new { success = await _courseService.DeleteCourseAsync(id) });

        #endregion

        #region Manage Course Prices Controllers

        [HttpPost]
        public async Task<IActionResult> CoursePricesPartial(Guid id, string? searchFilter)
        {
            PaginationVm<IEnumerable<Price>> data = await _courseService.GetCoursePricesAsync(id, searchFilter);

            if (data.Items?.ToList().Count > 0)
            {
                ViewBag.Message = "Not Found";
            }

            return PartialView(viewName: nameof(CoursePricesPartial), data);
        }

        public IActionResult CreateCoursePrice(Guid id)
        {
            CoursePriceCreateVm courseCreateVm = new CoursePriceCreateVm() { CourseId = id };
            return View(courseCreateVm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCoursePrice(CoursePriceCreateVm coursePriceCreateVm)
        {
            bool result = await _courseService.AddCoursePriceAsync(coursePriceCreateVm);

            if (!result)
                return View(coursePriceCreateVm);

            TempData["CoursePriceAdded"] = true;
            return Redirect($"/Admin/Courses/Single/{coursePriceCreateVm.CourseId}");
        }

        public async Task<IActionResult> UpdateCoursePrice(Guid courseId, Guid priceId)
        {
            CoursePriceUpdateVm coursePriceUpdateVm = await _courseService.GetCoursePriceUpdateDataAsync(courseId, priceId);
            return View(coursePriceUpdateVm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCoursePrice(CoursePriceUpdateVm coursePriceUpdateVm)
        {
            bool result = await _courseService.UpdateCoursePriceAsync(coursePriceUpdateVm);
            if (!result)
                return View(coursePriceUpdateVm);

            TempData["CoursePriceUpdated"] = true;
            return Redirect($"/Admin/Courses/Single/{coursePriceUpdateVm.CourseId}");
        }

        [HttpPost]
        public async Task<IActionResult> ArchivePrice(Guid id)
            => new JsonResult(new { success = await _courseService.ArchiveCoursePriceAsync(id) });

        [HttpPost]
        public async Task<IActionResult> UnArchivePrice(Guid id)
            => new JsonResult(new { success = await _courseService.UnArchiveCoursePriceAsync(id) });

        [HttpDelete]
        public async Task<IActionResult> DeletePrice(Guid id)
            => new JsonResult(new { success = await _courseService.DeleteCoursePriceAsync(id) });

        [HttpPost]
        public async Task<IActionResult> RestorePrice(Guid id)
            => new JsonResult(new { success = await _courseService.RestoreCoursePriceAsync(id) });

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

        #region Manage Tools Controller
        public async Task<IActionResult> CreateTool(Guid id)
        {
            Course? course = await _courseReadRepository.GetSingleAsync(x => x.Id == id);
            if (course == null) return NotFound();

            CourseToolCreateVm courseCreateVm = new CourseToolCreateVm()
            {
                CourseId = course.Id,
            };

            TempData["CourseToolCreated"] = false;
            return View(courseCreateVm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTool(CourseToolCreateVm courseToolCreateVm)
        {
            bool result = await _courseService.CreateToolAsync(courseToolCreateVm);

            if (!result)
                return View(courseToolCreateVm);

            TempData["CourseToolCreated"] = true;
            return Redirect($"/Admin/Courses/Single/{courseToolCreateVm.CourseId}");
        }

        public async Task<IActionResult> UpdateTool(Guid id)
        {
            CourseToolUpdateVm courseToolUpdateVm = await _courseService.GetUpdateToolDataAsync(id);

            TempData["CourseToolUpdated"] = false;
            return View(courseToolUpdateVm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTool(CourseToolUpdateVm courseToolUpdateVm)
        {
            bool result = await _courseService.UpdateToolAsync(courseToolUpdateVm);

            if (!result)
                return View(courseToolUpdateVm);

            TempData["CourseToolUpdated"] = true;
            return Redirect($"/Admin/Courses/Single/{courseToolUpdateVm.CourseId}");
        }

        [HttpPost]
        public async Task<IActionResult> ArchiveTool(Guid id)
        {
            bool result = await _courseService.ArchiveToolAsync(id);
            return new JsonResult(new { success = result });
        }

        [HttpPost]
        public async Task<IActionResult> UnArchiveTool(Guid id)
        {
            bool result = await _courseService.UnArchiveTool(id);
            return new JsonResult(new { success = result });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTool(Guid id)
        {
            bool result = await _courseService.DeleteToolAsync(id);
            return new JsonResult(new { success = result });
        }

        [HttpPost]
        public async Task<IActionResult> RestoreTool(Guid id)
        {
            bool result = await _courseService.RestoreToolAsync(id);
            return new JsonResult(new { success = result });
        }

        #endregion
    }
}

