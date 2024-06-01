using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.DataAccess.Models.Identity;
using CodinaxProjectMvc.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Filters
{

    [AttributeUsage(AttributeTargets.Method)]
    public class EventViewFilterFactoryAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            EventViewFilter filter = serviceProvider.GetRequiredService<EventViewFilter>();

            return filter;
        }
    }

    public class EventViewFilter : IAsyncActionFilter
    {
        private readonly IReadRepository<Course> _courseReadRepository;
        private readonly IReadRepository<Instructor> _instructorReadRepository;
        private readonly UserManager<AppUser> _userManager;

        public EventViewFilter(IReadRepository<Course> courseReadRepository, IReadRepository<Instructor> instructorReadRepository, UserManager<AppUser> userManager)
        {
            _courseReadRepository = courseReadRepository;
            _instructorReadRepository = instructorReadRepository;
            _userManager = userManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Controller? controller = context.Controller as Controller;

            if (controller != null)
            {
                AppUser user = await _userManager.FindByNameAsync(context.HttpContext.User.Identity.Name);

                if (await _userManager.IsInRoleAsync(user, Roles.Instructor.ToString()))
                {
                    controller.ViewBag.Courses = _courseReadRepository.Table
                        .Include(x => x.Instructors)
                        .Where(x => !x.IsDeleted && !x.IsArchived)
                        .Where(x => x.Instructors.Any(_x => _x.Email == user.Email))
                        .Select(x => new SelectListItem()
                        {
                            Value = x.Id.ToString(),
                            Text = x.Title
                        });

                    controller.ViewBag.Instructors = _instructorReadRepository
                        .Table.Where(UserQueryFilters<Instructor>.GeneralFilter)
                        .Select(x => new SelectListItem()
                        {
                            Value = x.Id.ToString(),
                            Text = x.FirstName + " " + x.LastName
                        });
                }
                else
                {
                    controller.ViewBag.Courses = _courseReadRepository.Table
                        .Include(x => x.Students)
                        .Where(x => !x.IsDeleted && !x.IsArchived)
                        .Where(x => x.Students.Any(_x => _x.Email == user.Email))
                        .Select(x => new SelectListItem()
                        {
                            Value = x.Id.ToString(),
                            Text = x.Title
                        });

                    controller.ViewBag.Instructors = _instructorReadRepository
                        .Table.Include(x => x.Courses).ThenInclude(x => x.Students)
                        .Where(UserQueryFilters<Instructor>.GeneralFilter)
                        .Where(x => x.Courses.Any(_x => _x.Students.Any(__x => __x.Email == user.Email)))
                        .Select(x => new SelectListItem()
                        {
                            Value = x.Id.ToString(),
                            Text = x.FirstName + " " + x.LastName
                        });
                }
            }

            await next();
        }
    }
}
