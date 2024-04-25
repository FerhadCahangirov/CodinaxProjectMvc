using CodinaxProjectMvc.DataAccess.Models.Common;
using Microsoft.AspNetCore.Identity;

namespace CodinaxProjectMvc.DataAccess.Models.Identity
{
    public class AppRole : IdentityRole<Guid>, IBaseGenericEntity
    {
        public AppRole() : base() { }

        public AppRole(string roleName) : base(roleName) { }
    }
}
