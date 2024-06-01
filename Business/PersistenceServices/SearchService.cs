using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.DataAccess.Models.Common;
using CodinaxProjectMvc.DataAccess.Models.Identity;
using CodinaxProjectMvc.Enums;
using CodinaxProjectMvc.ViewModel.SearchVm;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Business.PersistenceServices
{
    public class SearchService : ISearchService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IReadRepository<Student> _studentReadRepository;
        private readonly IReadRepository<Instructor> _instructorReadRepository;
        private readonly IReadRepository<Course> _courseReadRepository;


        public SearchService(
            UserManager<AppUser> userManager,
            IActionContextAccessor actionContextAccessor,
            IReadRepository<Student> studentReadRepository,
            IReadRepository<Instructor> instructorReadRepository,
            IReadRepository<Course> courseReadRepository
        )
        {
            _userManager = userManager;
            _actionContextAccessor = actionContextAccessor;
            _studentReadRepository = studentReadRepository;
            _instructorReadRepository = instructorReadRepository;
            _courseReadRepository = courseReadRepository;
        }

        public async Task<SearchListVm> SearchAsync(string query)
        {
            query = query.ToLower();

            switch (ActiveUserRole)
            {
                case nameof(Roles.Instructor):
                    Instructor instructor = await _instructorReadRepository.Table
                    .Include(x => x.Courses)
                    .ThenInclude(x => x.Modules)
                    .ThenInclude(x => x.Lectures)
                    .ThenInclude(x => x.LectureFiles)
                    .FirstAsync(x => x.Id == ActiveUser.Id);

                    IEnumerable<Course> instructor_courses = instructor.Courses
                        .Where(x => !x.IsDeleted && !x.IsArchived && x.Title.ToLower().Contains(query))
                        .OrderByDescending(x => x.CreatedDate)
                        .OrderBy(x => x.Title)
                        .Take(3).ToList();

                    IEnumerable<Module> instructor_modules = instructor.Courses
                         .Where(x => !x.IsDeleted && !x.IsArchived)
                         .SelectMany(course => course.Modules)
                         .Where(x => !x.IsDeleted && !x.IsArchived && x.Title.ToLower().Contains(query))
                         .OrderByDescending(x => x.CreatedDate)
                         .OrderBy(x => x.Title)
                         .Take(3)
                         .ToList();

                    IEnumerable<Lecture> instructor_lectures = instructor.Courses
                         .Where(x => !x.IsDeleted && !x.IsArchived)
                         .SelectMany(course => course.Modules)
                         .Where(x => !x.IsDeleted && !x.IsArchived)
                         .SelectMany(module => module.Lectures)
                         .Where(x => !x.IsDeleted && !x.IsArchived && x.Title.ToLower().Contains(query))
                         .OrderByDescending(x => x.CreatedDate)
                         .OrderBy(x => x.Title)
                         .Take(3)
                         .ToList();

                    IEnumerable<LectureFile> instructor_contents = instructor.Courses
                         .Where(x => !x.IsDeleted && !x.IsArchived)
                         .SelectMany(course => course.Modules)
                         .Where(x => !x.IsDeleted && !x.IsArchived)
                         .SelectMany(module => module.Lectures)
                         .Where(x => !x.IsDeleted && !x.IsArchived)
                         .SelectMany(lecture => lecture.LectureFiles)
                         .Where(x => !x.IsDeleted
                                 && !x.IsArchived
                                 && x.Title.ToLower().Contains(query))
                         .OrderByDescending(x => x.CreatedDate)
                         .OrderBy(x => x.Title)
                         .Take(3)
                         .ToList();

                    return new SearchListVm()
                    {
                        Courses = instructor_courses,
                        Modules = instructor_modules,
                        Lectures = instructor_lectures,
                        Contents = instructor_contents,
                        Query = query
                    };

                case nameof(Roles.Student):
                    Student student = await _studentReadRepository.Table
                    .Include(x => x.Courses)
                    .ThenInclude(x => x.Modules)
                    .ThenInclude(x => x.Lectures)
                    .ThenInclude(x => x.LectureFiles)
                    .FirstAsync(x => x.Id == ActiveUser.Id);

                    IEnumerable<Course> student_courses = student.Courses
                        .Where(x => !x.IsDeleted && !x.IsArchived && x.Title.ToLower().Contains(query))
                        .OrderByDescending(x => x.CreatedDate)
                        .OrderBy(x => x.Title)
                        .Take(3).ToList();

                    IEnumerable<Module> student_modules = student.Courses
                         .Where(x => !x.IsDeleted && !x.IsArchived)
                         .SelectMany(course => course.Modules)
                         .Where(x => !x.IsDeleted && !x.IsArchived && x.Title.ToLower().Contains(query))
                         .OrderByDescending(x => x.CreatedDate)
                         .OrderBy(x => x.Title)
                         .Take(3)
                         .ToList();

                    IEnumerable<Lecture> student_lectures = student.Courses
                         .Where(x => !x.IsDeleted && !x.IsArchived)
                         .SelectMany(course => course.Modules)
                         .Where(x => !x.IsDeleted && !x.IsArchived)
                         .SelectMany(module => module.Lectures)
                         .Where(x => !x.IsDeleted && !x.IsArchived && x.Title.ToLower().Contains(query))
                         .OrderByDescending(x => x.CreatedDate)
                         .OrderBy(x => x.Title)
                         .Take(3).ToList();

                    IEnumerable<LectureFile> student_contents = student.Courses
                         .Where(x => !x.IsDeleted && !x.IsArchived)
                         .SelectMany(course => course.Modules)
                         .Where(x => !x.IsDeleted && !x.IsArchived)
                         .SelectMany(module => module.Lectures)
                         .Where(x => !x.IsDeleted && !x.IsArchived)
                         .SelectMany(lecture => lecture.LectureFiles)
                         .Where(x => !x.IsDeleted
                                 && !x.IsArchived
                                 && x.Title.ToLower().Contains(query))
                         .OrderByDescending(x => x.CreatedDate)
                         .OrderBy(x => x.Title)
                         .Take(3).ToList();

                    return new SearchListVm()
                    {
                        Courses = student_courses,
                        Modules = student_modules,
                        Lectures = student_lectures,
                        Contents = student_contents,
                        Query = query
                    };

                case nameof(Roles.Admin):

                    IEnumerable<Course> courses = _courseReadRepository.Table
                        .Include(x => x.Modules)
                        .ThenInclude(x => x.Lectures)
                        .ThenInclude(x => x.LectureFiles)
                        .Where(x => !x.IsDeleted && !x.IsArchived);

                    IEnumerable<Module> modules = courses
                        .SelectMany(x => x.Modules)
                        .Where(x => !x.IsDeleted && !x.IsArchived);

                    IEnumerable<Lecture> lectures = modules
                        .SelectMany(x => x.Lectures)
                        .Where(x => !x.IsDeleted && !x.IsArchived);

                    IEnumerable<LectureFile> contents = lectures
                        .SelectMany(x => x.LectureFiles)
                        .Where(x => !x.IsDeleted && !x.IsArchived);

                    return new SearchListVm()
                    {
                        Courses = courses.Where(x => x.Title.ToLower().Contains(query))
                        .OrderBy(x => x.Title).Take(3).ToList(),
                        Modules = modules.Where(x => x.Title.ToLower().Contains(query))
                        .OrderBy(x => x.Title).Take(3).ToList(),
                        Lectures = lectures.Where(x => x.Title.ToLower().Contains(query))
                        .OrderBy(x => x.Title).Take(3).ToList(),
                        Contents = contents.Where(x => x.Title.ToLower().Contains(query))
                        .OrderBy(x => x.Title).Take(3).ToList(),
                        Query = query
                    };

                default:
                    return new SearchListVm();
            }
        }

        public IEnumerable<TEntity>? Search<TEntity>(string query) where TEntity : BaseEntity
        {
            query = query.ToLower();

            IEnumerable<Course> courses = _courseReadRepository.Table
                        .Include(x => x.Instructors)
                        .Include(x => x.Students)
                        .Include(x => x.Modules)
                        .ThenInclude(x => x.Lectures)
                        .ThenInclude(x => x.LectureFiles)
                        .Where(x => !x.IsDeleted && !x.IsArchived);


            switch (ActiveUserRole)
            {
                case nameof(Roles.Instructor):

                    if (typeof(TEntity) == typeof(Course))
                    {
                        return courses
                            .Where(x => x.Instructors.Contains(ActiveUser)).Where(x => x.Title.ToLower().Contains(query))
                            .OrderBy(x => x.Title).ToList() as IEnumerable<TEntity>;
                    }
                    else if (typeof(TEntity) == typeof(Module))
                    {
                        return courses.Where(x => x.Instructors.Contains(ActiveUser))
                            .SelectMany(x => x.Modules)
                            .Where(x => !x.IsDeleted && !x.IsArchived).Where(x => x.Title.ToLower().Contains(query))
                            .OrderBy(x => x.Title).ToList() as IEnumerable<TEntity>;
                    }
                    else if (typeof(TEntity) == typeof(Lecture))
                    {
                        return courses.Where(x => x.Instructors.Contains(ActiveUser))
                            .SelectMany(x => x.Modules)
                            .Where(x => !x.IsDeleted && !x.IsArchived)
                            .SelectMany(x => x.Lectures)
                            .Where(x => !x.IsDeleted && !x.IsArchived).Where(x => x.Title.ToLower().Contains(query))
                            .OrderBy(x => x.Title).ToList() as IEnumerable<TEntity>;
                    }
                    else if (typeof(TEntity) == typeof(LectureFile))
                    {
                        return courses.Where(x => x.Instructors.Contains(ActiveUser))
                            .SelectMany(x => x.Modules)
                            .Where(x => !x.IsDeleted && !x.IsArchived)
                            .SelectMany(x => x.Lectures)
                            .Where(x => !x.IsDeleted && !x.IsArchived)
                            .SelectMany(x => x.LectureFiles)
                            .Where(x => !x.IsDeleted && !x.IsArchived).Where(x => x.Title.ToLower().Contains(query))
                            .OrderBy(x => x.Title).ToList() as IEnumerable<TEntity>;
                    }
                    else
                    {
                        return new List<TEntity>();
                    }
                case nameof(Roles.Student):
                    if (typeof(TEntity) == typeof(Course))
                    {
                        return courses
                            .Where(x => x.Students.Contains(ActiveUser)).Where(x => x.Title.ToLower().Contains(query))
                            .OrderBy(x => x.Title).ToList() as IEnumerable<TEntity>;
                    }
                    else if (typeof(TEntity) == typeof(Module))
                    {
                        return courses.Where(x => x.Students.Contains(ActiveUser))
                            .SelectMany(x => x.Modules)
                            .Where(x => !x.IsDeleted && !x.IsArchived).Where(x => x.Title.ToLower().Contains(query))
                            .OrderBy(x => x.Title).ToList() as IEnumerable<TEntity>;
                    }
                    else if (typeof(TEntity) == typeof(Lecture))
                    {
                        return courses.Where(x => x.Students.Contains(ActiveUser))
                            .SelectMany(x => x.Modules)
                            .Where(x => !x.IsDeleted && !x.IsArchived)
                            .SelectMany(x => x.Lectures)
                            .Where(x => !x.IsDeleted && !x.IsArchived).Where(x => x.Title.ToLower().Contains(query))
                            .OrderBy(x => x.Title).ToList() as IEnumerable<TEntity>;
                    }
                    else if (typeof(TEntity) == typeof(LectureFile))
                    {
                        return courses.Where(x => x.Students.Contains(ActiveUser))
                            .SelectMany(x => x.Modules)
                            .Where(x => !x.IsDeleted && !x.IsArchived)
                            .SelectMany(x => x.Lectures)
                            .Where(x => !x.IsDeleted && !x.IsArchived)
                            .SelectMany(x => x.LectureFiles)
                            .Where(x => !x.IsDeleted && !x.IsArchived).Where(x => x.Title.ToLower().Contains(query))
                            .OrderBy(x => x.Title).ToList() as IEnumerable<TEntity>;
                    }
                    else
                    {
                        return new List<TEntity>();
                    }
                case nameof(Roles.Admin):
                    if (typeof(TEntity) == typeof(Course))
                    {
                        return courses.Where(x => x.Title.ToLower().Contains(query)).OrderBy(x => x.Title).ToList() as IEnumerable<TEntity>;
                    }
                    else if (typeof(TEntity) == typeof(Module))
                    {
                        return courses.SelectMany(x => x.Modules)
                            .Where(x => !x.IsDeleted && !x.IsArchived).Where(x => x.Title.ToLower().Contains(query))
                            .OrderBy(x => x.Title).ToList() as IEnumerable<TEntity>;
                    }
                    else if (typeof(TEntity) == typeof(Lecture))
                    {
                        return courses.SelectMany(x => x.Modules)
                            .Where(x => !x.IsDeleted && !x.IsArchived)
                            .SelectMany(x => x.Lectures)
                            .Where(x => !x.IsDeleted && !x.IsArchived).Where(x => x.Title.ToLower().Contains(query))
                            .OrderBy(x => x.Title).ToList() as IEnumerable<TEntity>;
                    }
                    else if (typeof(TEntity) == typeof(LectureFile))
                    {
                        return courses.SelectMany(x => x.Modules)
                            .Where(x => !x.IsDeleted && !x.IsArchived)
                            .SelectMany(x => x.Lectures)
                            .Where(x => !x.IsDeleted && !x.IsArchived)
                            .SelectMany(x => x.LectureFiles)
                            .Where(x => !x.IsDeleted && !x.IsArchived).Where(x => x.Title.ToLower().Contains(query))
                            .OrderBy(x => x.Title).ToList() as IEnumerable<TEntity>;
                    }
                    else
                    {
                        return new List<TEntity>();
                    }
                default:
                    return new List<TEntity>();
            }


        }

        private string ActiveUserRole
        {
            get
            {
                var userRole = string.Empty;

                if (_userManager.IsInRoleAsync(ActiveUser, Roles.Instructor.ToString()).Result)
                {
                    userRole = Roles.Instructor.ToString();
                }
                else if (_userManager.IsInRoleAsync(ActiveUser, Roles.Student.ToString()).Result)
                {
                    userRole = Roles.Student.ToString();
                }
                else if (_userManager.IsInRoleAsync(ActiveUser, Roles.Admin.ToString()).Result)
                {
                    userRole = Roles.Admin.ToString();
                }

                return userRole;
            }
        }

        private AppUser ActiveUser
        {
            get
            {
                return _userManager.FindByEmailAsync(_actionContextAccessor.ActionContext.HttpContext.User.Identity.Name).Result;
            }
        }
    }
}
