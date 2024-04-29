using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.DataAccess;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Abstract.Storages;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.Enums;
using CodinaxProjectMvc.ViewModel.CourseVm;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;


namespace CodinaxProjectMvc.Business.PersistenceServices
{
    public class CourseService : ICourseService
    {
        #region Repositories Initialize
        private readonly IReadRepository<Course> _courseReadRepository;
        private readonly IReadRepository<Category> _categoryReadRepository;
        private readonly IWriteRepository<Course> _courseWriteRepository;
        private readonly IReadRepository<Instructor> _instructorReadRepository;
        private readonly IReadRepository<Template> _templateReadRepository;
        private readonly IWriteRepository<Advice> _adviceWriteRepository;   
        #endregion

        private readonly IAzureStorage _storage;
        private readonly IConfiguration _configuration;
        private readonly IActionContextAccessor _actionContextAccessor;

        #region Constructor for Dependency Injection
        public CourseService(
            IReadRepository<Course> courseReadRepository,
            IWriteRepository<Course> courseWriteRepository,
            IActionContextAccessor actionContextAccessor,
            IReadRepository<Category> categoryReadRepository,
            IAzureStorage storage,
            IConfiguration configuration,
            IReadRepository<Instructor> instructorReadRepository,
            IReadRepository<Template> templateReadRepository,
            IWriteRepository<Advice> adviceWriteRepository)
        {
            _courseReadRepository = courseReadRepository;
            _courseWriteRepository = courseWriteRepository;
            _actionContextAccessor = actionContextAccessor;
            _categoryReadRepository = categoryReadRepository;
            _storage = storage;
            _configuration = configuration;
            _instructorReadRepository = instructorReadRepository;
            _templateReadRepository = templateReadRepository;
            _adviceWriteRepository = adviceWriteRepository;
        }

        #endregion


        #region Manage Course Services

        public async Task<bool> CreateCourseAsync(CourseCreateVm courseCreateVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
                return false;

            Course? firstAdvicedCourse = null;
            Course? secondAdvicedCourse = null;

            Category? category = await _categoryReadRepository.GetByIdAsync(courseCreateVm.CourseCategory);

            if (category == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(CourseCreateVm.CourseCategory), "This Category Is Not Found");
                return false;
            }

            Template? template = await _templateReadRepository.GetByIdAsync(courseCreateVm.Template);
            if (template == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(CourseCreateVm.Template), "Tjis Template is not found");
                return false;
            }


            if (courseCreateVm.FirstAdvicedCourse != null)
            {
                firstAdvicedCourse = await _courseReadRepository.GetByIdAsync(courseCreateVm.FirstAdvicedCourse);

                if (firstAdvicedCourse == null)
                {
                    _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(CourseCreateVm.FirstAdvicedCourse), "Advice Course Not Found");
                    return false;
                }

            }

            if (courseCreateVm.SecondAdvicedCourse != null)
            {
                secondAdvicedCourse = await _courseReadRepository.GetByIdAsync(courseCreateVm.SecondAdvicedCourse);

                if (secondAdvicedCourse == null)
                {
                    _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(CourseCreateVm.SecondAdvicedCourse), "Advice Course Not Found");
                    return false;
                }
            }

            Course course = new Course()
            {
                CourseCode = courseCreateVm.CourseCode,
                Title = courseCreateVm.CourseTitle,
                Category = category,
                Template  = template,
                CourseLevel = Enum.Parse<CourseLevels>(courseCreateVm.CourseLevel),
            };

            await _courseWriteRepository.AddAsync(course);
            await _courseWriteRepository.SaveAsync();

            if (firstAdvicedCourse != null || secondAdvicedCourse != null)
            {
                Advice advice = new()
                {
                    MainCourse = course
                };

                if (firstAdvicedCourse != null)
                {
                    advice.FirstAdvicedCourse = firstAdvicedCourse;
                }

                if (secondAdvicedCourse != null)
                {
                    advice.SecondAdvicedCourse= secondAdvicedCourse;
                }

                await _adviceWriteRepository.AddAsync(advice);
                await _adviceWriteRepository.SaveAsync();
            }


            return true;
        }
        public async Task<bool> UpdateCourseAsync(CourseUpdateVm courseUpdateVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
                return false;


            Category? category = await _categoryReadRepository.GetByIdAsync(courseUpdateVm.CourseCategory);

            if (category == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(CourseUpdateVm.CourseCategory), "This Course Category Not Found");
                return false;
            }

            Template? template = await _templateReadRepository.GetByIdAsync(courseUpdateVm.Template);
            if(template == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(CourseUpdateVm.Template), "This Template is not found");
                return false;
            }

            Course? course = await _courseReadRepository.Table
                .Include(x => x.Category)
                .Include(x => x.Template)
                .FirstOrDefaultAsync(x => x.Id == courseUpdateVm.Id);

            if (course == null)
                return false;

            
            course.CourseCode = courseUpdateVm.CourseCode;
            course.Title = courseUpdateVm.CourseTitle;
            course.Category = category;
            course.CourseLevel = Enum.Parse<CourseLevels>(courseUpdateVm.CourseLevel);
            course.Template = template;

            _courseWriteRepository.Update(course);
            await _courseWriteRepository.SaveAsync();

            return true;
        }

        public async Task<bool> ChangeShowcaseAsync(Guid id) 
        {
            Course? course = await _courseReadRepository.GetSingleAsync(x => x.Id == id);
            
            if(course == null)
            {
                return false;
            }

            course.Showcase = !course.Showcase;

            _courseWriteRepository.Update(course);
            await _courseWriteRepository.SaveAsync();

            return true;
        }

        public Task<IEnumerable<Course>> GetCoursesAsync()
        {
            IEnumerable<Course> courses = _courseReadRepository
                .GetWhere(x => !x.IsDeleted && !x.IsArchived)
                .Include(c => c.Instructors)
                .Include(c => c.Students)
                .Include(c => c.Modules);

            return Task.FromResult(courses);
        }

        public async Task<CourseSingleVm> GetCourseSingleAsync(Guid id)
        {
            Course? course = await _courseReadRepository.Table
                .Include(x => x.Instructors)
                .Include(x => x.Students)
                .Include(x => x.Category)
                .Include(x => x.Template)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (course == null)
                return new CourseSingleVm();

            CourseSingleVm courseSingleVm = new()
            {
                Course = course,
                BaseUrl = _configuration["BaseUrl:Azure"],
            };

            return courseSingleVm;
        }

        public async Task<CourseUpdateVm> GetCourseUpdateDataAsync(Guid id)
        {
            Course? course = await _courseReadRepository.Table
                .Include(x => x.Category)
                .Include(x => x.Template)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (course == null)
                return new CourseUpdateVm();


            CourseUpdateVm courseUpdateVm = new CourseUpdateVm
            {
                Id = course.Id,
                CourseCategory = course.Category.Content,
                CourseLevel = course.CourseLevel.ToString(),
                CourseTitle = course.Title,
                Template = course.Template.Id.ToString(),
                CourseCode = course.CourseCode,
            };

            return courseUpdateVm;
        }

        public async Task<bool> DeleteCourseAsync(Guid id)
        {
            Course? course = await _courseReadRepository.GetSingleAsync(x => x.Id == id);
            if (course == null)
                return false;

            course.IsDeleted = true;

            _courseWriteRepository.Update(course);
            await _courseWriteRepository.SaveAsync();
            return true;
        }


        #endregion

        #region Manage Instructors Services
        public async Task<CourseInstructorsVm> GetCourseInstructorsAsync(Guid id, string? searchFilter)
        {
            var course = await _courseReadRepository.Table
                .Include(x => x.Instructors)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (course == null) return new CourseInstructorsVm();

            var query = course.Instructors.AsQueryable();

            if (!string.IsNullOrEmpty(searchFilter))
            {
                searchFilter = searchFilter.ToLower();
                query = query.Where(x => x.UserName.ToLower().Contains(searchFilter) ||
                    x.FirstName.ToLower().Contains(searchFilter) ||
                    x.LastName.ToLower().Contains(searchFilter) ||
                    x.Email.ToLower().Contains(searchFilter));
            }

            var instructors = await query.ToListAsync();

            CourseInstructorsVm instructorsVm = new CourseInstructorsVm()
            {
                Instructors = instructors,
                Course = course,
                BaseUrl = _configuration["BaseUrl:Azure"]
            };

            return instructorsVm;
        }

        public async Task<bool> ReassignInstructorAsync(Guid courseId, Guid instructorId)
        {
            Course? course = await _courseReadRepository.Table
                .Include(x => x.Instructors)
                .FirstOrDefaultAsync(x => x.Id == courseId);

            if (course == null) return false;

            Instructor? instructor = await _instructorReadRepository.GetSingleAsync(x => x.Id == instructorId);
            if (instructor == null) return false;

            if (course.Instructors.Any(x => x.Id == instructor.Id))
            {
                List<Instructor> instructors = course.Instructors.ToList();
                instructors.Remove(instructor);
                course.Instructors = instructors;
                _courseWriteRepository.Update(course);
                await _courseWriteRepository.SaveAsync();
            }

            return true;
        }

        public async Task<bool> AssignInstructorAsync(Guid courseId, Guid instructorId)
        {
            Course? course = await _courseReadRepository.Table
                .Include(x => x.Instructors)
                .FirstOrDefaultAsync(x => x.Id == courseId);

            if (course == null) return false;

            Instructor? instructor = await _instructorReadRepository.GetSingleAsync(x => x.Id == instructorId);
            if (instructor == null) return false;

            List<Instructor> instructors = course.Instructors.ToList();
            instructors.Add(instructor);

            course.Instructors = instructors;

            _courseWriteRepository.Update(course);
            await _courseWriteRepository.SaveAsync();

            return true;
        }

        #endregion


    }
}
