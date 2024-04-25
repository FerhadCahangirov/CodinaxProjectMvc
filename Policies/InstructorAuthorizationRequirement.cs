using CodinaxProjectMvc.Context;
using CodinaxProjectMvc.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Policies
{
    public class InstructorAuthorizationRequirement : IAuthorizationRequirement
    {
    }

    public class InstructorAuthorizationHandler : AuthorizationHandler<InstructorAuthorizationRequirement>
    {
        private readonly CodinaxDbContext _db;

        public InstructorAuthorizationHandler(CodinaxDbContext db)
        {
            _db = db;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, InstructorAuthorizationRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated || !context.User.IsInRole(Roles.Instructor.ToString()))
            {
                context.Fail();
                return;
            }

            var userName = context.User.Identity.Name;

            var instructor = await _db.Instructors.SingleOrDefaultAsync (x => x.UserName == userName);

            if (instructor != null && instructor.IsApproved == true && instructor.IsBanned == false)
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
