using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.DataAccess;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Abstract.Storages;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.Enums;
using CodinaxProjectMvc.ViewModel.CourseVm;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Identity;
using System.Linq.Expressions;

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
        private readonly IReadRepository<Advice> _adviceReadRepository;
        #endregion

        private readonly IAzureStorage _storage;
        private readonly IConfiguration _configuration;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IInstructorService _instructorService;

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
            IWriteRepository<Advice> adviceWriteRepository,
            IReadRepository<Advice> adviceReadRepository,
            IInstructorService instructorService)
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
            _adviceReadRepository = adviceReadRepository;
            _instructorService = instructorService;
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
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(CourseCreateVm.Template), "This Template is not found");
                return false;
            }
            else if (template.IsDeleted || template.IsArchived)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(CourseCreateVm.Template), "This Template has been deleted you can not add deleted template to the ");
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
                Template = template,
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
                    advice.SecondAdvicedCourse = secondAdvicedCourse;
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

            Course? firstAdvicedCourse = null;
            Course? secondAdvicedCourse = null;


            Category? category = await _categoryReadRepository.GetByIdAsync(courseUpdateVm.CourseCategory);

            if (category == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(CourseUpdateVm.CourseCategory), "This Course Category Not Found");
                return false;
            }

            Template? template = await _templateReadRepository.GetByIdAsync(courseUpdateVm.Template);
            if (template == null)
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

            if (courseUpdateVm.FirstAdvicedCourse != null)
            {
                firstAdvicedCourse = await _courseReadRepository.GetByIdAsync(courseUpdateVm.FirstAdvicedCourse);

                if (courseUpdateVm == null)
                {
                    _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(CourseCreateVm.FirstAdvicedCourse), "Advice Course Not Found");
                    return false;
                }
            }

            if (courseUpdateVm.SecondAdvicedCourse != null)
            {
                secondAdvicedCourse = await _courseReadRepository.GetByIdAsync(courseUpdateVm.SecondAdvicedCourse);

                if (secondAdvicedCourse == null)
                {
                    _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(CourseCreateVm.SecondAdvicedCourse), "Advice Course Not Found");
                    return false;
                }
            }


            course.CourseCode = courseUpdateVm.CourseCode;
            course.Title = courseUpdateVm.CourseTitle;
            course.Category = category;
            course.CourseLevel = Enum.Parse<CourseLevels>(courseUpdateVm.CourseLevel);
            course.Template = template;

            _courseWriteRepository.Update(course);
            await _courseWriteRepository.SaveAsync();

            if (firstAdvicedCourse != null || secondAdvicedCourse != null)
            {
                Advice? advice = await _adviceReadRepository.Table
                    .Include(x => x.MainCourse)
                    .Include(x => x.FirstAdvicedCourse)
                    .Include(x => x.SecondAdvicedCourse)
                    .FirstOrDefaultAsync(x => x.MainCourse == course);

                if (advice == null)
                {
                    advice = new Advice(course, firstAdvicedCourse, secondAdvicedCourse);
                    await _adviceWriteRepository.AddAsync(advice);
                    await _adviceWriteRepository.SaveAsync();
                }
                else
                {
                    advice.FirstAdvicedCourse = firstAdvicedCourse;
                    advice.SecondAdvicedCourse = secondAdvicedCourse;
                    _adviceWriteRepository.Update(advice);
                    await _adviceWriteRepository.SaveAsync();
                }
            }

            return true;
        }

        public async Task<bool> ChangeShowcaseAsync(Guid id)
        {
            Course? course = await _courseReadRepository.GetSingleAsync(x => x.Id == id);

            if (course == null)
            {
                return false;
            }

            if (course.Showcase && course.IsPrimary)
            {
                course.IsPrimary = !course.IsPrimary;
            }

            course.Showcase = !course.Showcase;

            _courseWriteRepository.Update(course);
            await _courseWriteRepository.SaveAsync();

            return true;
        }

        public async Task<PrimaryCourseActionReturnType> SetCoursePrimaryAsync(Guid id)
        {
            Course? course = await _courseReadRepository.GetSingleAsync(x => x.Id == id);

            if (course == null)
            {
                return PrimaryCourseActionReturnType.Failure;
            }

            if (!course.IsPrimary && _courseReadRepository.GetWhere(x => x.IsPrimary).Count() >= 3)
            {
                return PrimaryCourseActionReturnType.Oversized;
            }

            if (!course.Showcase && !course.IsPrimary)
            {
                course.Showcase = !course.Showcase;
            }

            course.IsPrimary = !course.IsPrimary;

            _courseWriteRepository.Update(course);
            await _courseWriteRepository.SaveAsync();

            return PrimaryCourseActionReturnType.Success;
        }

        public Task<IEnumerable<Course>> GetCoursesAsync()
        {
            IEnumerable<Course> courses = _courseReadRepository
                .GetWhere(x => !x.IsDeleted && !x.IsArchived)
                .OrderBy(x => x.Title)
                .OrderBy(x => !x.IsPrimary)
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

            Advice? advice = await _adviceReadRepository.Table
                .Include(x => x.MainCourse)
                .Include(x => x.FirstAdvicedCourse)
                .Include(x => x.SecondAdvicedCourse)
                .FirstOrDefaultAsync(x => x.MainCourse == course);


            CourseUpdateVm courseUpdateVm = new CourseUpdateVm
            {
                Id = course.Id,
                CourseCategory = course.Category.Id.ToString(),
                CourseLevel = course.CourseLevel.ToString(),
                FirstAdvicedCourse = advice?.FirstAdvicedCourse?.Id.ToString(),
                SecondAdvicedCourse = advice?.SecondAdvicedCourse?.Id.ToString(),
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

            List<Advice>? advices = await _adviceReadRepository.Table
                .Include(x => x.FirstAdvicedCourse)
                .Include(x => x.SecondAdvicedCourse)
                .Where(x => x.FirstAdvicedCourse == course || x.SecondAdvicedCourse == course)
                .ToListAsync();

            _courseWriteRepository.Update(course);
            await _courseWriteRepository.SaveAsync();

            foreach (Advice advice in advices)
            {
                if (advice.FirstAdvicedCourse == course)
                {
                    advice.FirstAdvicedCourse = null;
                }

                if (advice.SecondAdvicedCourse == course)
                {
                    advice.SecondAdvicedCourse = null;
                }
            }

            _adviceWriteRepository.UpdateRange(advices);
            await _adviceWriteRepository.SaveAsync();

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

            var query = course.Instructors.Where(UserQueryFilters<Instructor>.GeneralFilter).AsQueryable();

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

        public async Task<CourseInstructorsAssignVm> GetAssignableInstructorsAsync(Guid id, string? searchFilter = null)
        {
            Course? course = await _courseReadRepository.Table
                .Include(x => x.Instructors)
                .Include(x => x.Students)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (course == null) return new CourseInstructorsAssignVm();

            var pagination = await _instructorService.GetAssignableInstructorPaginationAsync(searchFilter);

            CourseInstructorsAssignVm courseInstructorsAssignVm = new CourseInstructorsAssignVm()
            {
                InstructorPagination = pagination,
                Course = course,
            };

            return courseInstructorsAssignVm;

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
