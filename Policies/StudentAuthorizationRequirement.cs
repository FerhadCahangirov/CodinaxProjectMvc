using CodinaxProjectMvc.Context;
using CodinaxProjectMvc.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Policies
{
    public class StudentAuthorizationRequirement : IAuthorizationRequirement
    {
    }

    public class StudentAuthorizationHandler : AuthorizationHandler<StudentAuthorizationRequirement>
    {
        private readonly CodinaxDbContext _db;

        public StudentAuthorizationHandler(CodinaxDbContext db)
        {
            _db = db;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, StudentAuthorizationRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated || !context.User.IsInRole(Roles.Student.ToString()))
            {
                context.Fail();
                return;
            }

            var userName = context.User.Identity.Name;

            var student = await _db.Students.SingleOrDefaultAsync(x => x.UserName == userName);

            if (student != null && student.IsApproved == true && student.IsBanned == false)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}
