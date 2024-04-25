using CodinaxProjectMvc.Context;
using CodinaxProjectMvc.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    //[Authorize(Policy = PolicyConstants.InstructorPolicy)]
    public class CoursesController : Controller
    {
        private readonly CodinaxDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<CoursesController> _logger;
        private const string Storage = "Local";
        private const string PathOrContainer = "Videos";


        public CoursesController(CodinaxDbContext db, IWebHostEnvironment env, ILogger<CoursesController> logger)
        {
            _db = db;
            _env = env;
            _logger = logger;
        }

        private async Task<DataAccess.Models.Instructor> GetActiveInstructorAsync()
            => await _db.Instructors.Include(x => x.Courses).FirstAsync(x => x.UserName == HttpContext.User.Identity.Name);

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_db.Categories, "Id", "Content");

            var levels = Enum.GetValues(typeof(CourseLevels));

            var selectListItems = new List<SelectListItem>();

            foreach (var value in levels)
            {
                selectListItems.Add(new SelectListItem
                {
                    Text = Enum.GetName(typeof(CourseLevels), value),
                    Value = ((int)value).ToString()
                });
            }

            ViewBag.Levels = new SelectList(selectListItems, "Value", "Text");
            return View();
        }

        #region Temp

        //[HttpGet]
        //public async Task<IActionResult> GetCourseData(Guid id)
        //{
        //    Models.Instructor instructor = await GetActiveInstructorAsync();

        //    Course? course = await _db.Courses
        //        .Include(c => c.Category)
        //        .Include(c => c.Sections)
        //        .ThenInclude(c => c.Lectures)
        //        .ThenInclude(c => c.LectureVideo)
        //        .FirstOrDefaultAsync(c => c.Id == id);

        //    if (course == null) return NotFound();

        //    CourseSingleVm courseSingleVm = new()
        //    {
        //        Id = course.Id,
        //        CourseTitle = course.Title,
        //        CourseCategoryName = course.Category.Content,
        //        CourseLevel = course.CourseLevel,
        //        CourseDescription = course.Description,
        //        Sections = course.Sections.Select(section => new CourseSingleSectionsVm()
        //        {
        //            Id = section.Id,
        //            Number = section.Number,
        //            Title = section.Title,
        //            Lectures = section.Lectures.Select(lecture => new CourseSingleSectionLecturesVm()
        //            {
        //                Id = lecture.Id,
        //                Title = lecture.Title,
        //                Number = lecture.LectureNumber,
        //                Description = lecture.Title,
        //                FileUrl = $"/{lecture.LectureVideo.Storage}/{lecture.LectureVideo.PathOrContainer}/{lecture.LectureVideo.FileName}"
        //            }).OrderBy(lecture => lecture.Number)
        //        }).OrderBy(section =>  section.Number)
        //    };

        //    var serializedData = JsonSerializer.Serialize(courseSingleVm);

        //    return Ok(serializedData);
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateCourse(CourseCreateVm vm)
        //{
        //    Models.Instructor instructor = await GetActiveInstructorAsync();

        //    Course? course = new Course()
        //    {
        //        Instructor = instructor,
        //        Title = vm.CourseTitle,
        //        Description = vm.CourseDescription,
        //        IsDrafted = true,
        //        CourseLevel = vm.CourseLevel,
        //        Category = _db.Categories.Single(x => x.Content == vm.CourseCategory)
        //    };

        //    await _db.Courses.AddAsync(course);
        //    await _db.SaveChangesAsync();

        //    return new JsonResult(new { success = true, id = course?.Id });
        //}

        //[HttpPost]
        //public async Task<IActionResult> AddSection(CourseSectionCreateVm vm)
        //{
        //    Course? course = await _db.Courses.FirstOrDefaultAsync(x => x.Id == vm.CourseId);

        //    if (course == null) return NotFound();

        //    Section section = new Section()
        //    {
        //        Number = vm.Number,
        //        Title = vm.Title,
        //        Course = course
        //    };

        //    await _db.Sections.AddAsync(section);
        //    await _db.SaveChangesAsync();

        //    return new JsonResult(new { success = true });
        //}

        //[HttpPost]
        //public async Task<IActionResult> AddLecture([FromForm] CourseLectureCreateVm vm)
        //{
        //    Course? course = await _db.Courses.FirstOrDefaultAsync(x => x.Id == vm.CourseId);

        //    if (course == null) return NotFound();

        //    Section? section = await _db.Sections
        //        .Where(s => s.Course == course)
        //        .FirstOrDefaultAsync(x => x.Id == vm.SectionId);

        //    if (section == null) return NotFound();

        //    string filename = Guid.NewGuid() + Path.GetExtension(vm.File.FileName);

        //    using (Stream fs = new FileStream(Path.Combine(_env.WebRootPath, Storage, PathOrContainer, filename), FileMode.Create))
        //    {
        //        await vm.File.CopyToAsync(fs);

        //        LectureVideo lectureVideo = new LectureVideo()
        //        {
        //            Storage = Storage,
        //            PathOrContainer = PathOrContainer,
        //            FileName = filename,
        //        };

        //        await _db.LectureVideos.AddAsync(lectureVideo);
        //        await _db.SaveChangesAsync();
        //    }

        //    Lecture lecture = new Lecture()
        //    {
        //        LectureNumber = vm.Number,
        //        Title = vm.Title,
        //        Section = section,
        //        LectureVideo = _db.LectureVideos.Single(x => x.FileName == filename)
        //    };


        //    await _db.Lectures.AddAsync(lecture);
        //    await _db.SaveChangesAsync();

        //    return new JsonResult(new { success = true });
        //}

        #endregion 
    }
}
