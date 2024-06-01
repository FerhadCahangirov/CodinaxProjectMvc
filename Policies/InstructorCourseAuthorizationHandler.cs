using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Policies
{

    public class InstructorCourseAuthorizationRequirement : IAuthorizationRequirement
    {
    }

    public class InstructorCourseAuthorizationHandler : AuthorizationHandler<InstructorCourseAuthorizationRequirement>
    {
        private readonly IReadRepository<Instructor> _instructorReadRepository;
        private readonly IActionContextAccessor _actionContextAccessor;

        public InstructorCourseAuthorizationHandler(IReadRepository<Instructor> instructorReadRepository, IActionContextAccessor actionContextAccessor)
        {
            _instructorReadRepository = instructorReadRepository;
            _actionContextAccessor = actionContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, InstructorCourseAuthorizationRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated || !context.User.IsInRole(Roles.Instructor.ToString()))
            {
                context.Fail();
                return;
            }

            var username = context.User.Identity.Name;

            Instructor? instructor = await _instructorReadRepository
                .Table.Include(x => x.Courses)
                .SingleOrDefaultAsync(x => x.UserName == username);

            if (instructor != null && instructor.IsApproved == true && instructor.IsBanned == false && instructor.IsDeleted == false)
            {

                var actionContext = _actionContextAccessor.ActionContext;

                if (actionContext == null)
                {
                    context.Fail();
                    return;
                }

                if (!actionContext.RouteData.Values.TryGetValue("id", out var value) || value == null || !Guid.TryParse(value.ToString(), out var courseId))
                {
                    context.Fail();
                    return;
                }

                if (!instructor.Courses.Any(x => x.Id == courseId))
                {
                    context.Fail();
                    return;
                }

                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }


        }
    }
}
