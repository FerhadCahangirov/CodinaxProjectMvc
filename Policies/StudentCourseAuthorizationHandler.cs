using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Policies
{

    public class StudentCourseAuthorizationRequirement : IAuthorizationRequirement
    {
    }

    public class StudentCourseAuthorizationHandler : AuthorizationHandler<StudentAuthorizationRequirement>
    {
        private readonly IReadRepository<Student> _studentReadRepository;
        private readonly IActionContextAccessor _actionContextAccessor;

        public StudentCourseAuthorizationHandler(IReadRepository<Student> studentReadRepository, IActionContextAccessor actionContextAccessor)
        {
            _studentReadRepository = studentReadRepository;
            _actionContextAccessor = actionContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, StudentAuthorizationRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated || !context.User.IsInRole(Roles.Student.ToString()))
            {
                context.Fail();
                return;
            }

            var username = context.User.Identity.Name;

            Student? student = await _studentReadRepository
                .Table.Include(x => x.Courses)
                .SingleOrDefaultAsync(x => x.UserName == username);

            if (student != null && student.IsApproved == true && student.IsBanned == false && student.IsDeleted == false)
            {
                Guid? id = _actionContextAccessor.ActionContext.RouteData.Values.TryGetValue("id", out var value) ? (Guid?)value : null;

                if (id == null)
                {
                    context.Fail();
                    return;
                }

                if (!student.Courses.Any(x => x.Id == id))
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
