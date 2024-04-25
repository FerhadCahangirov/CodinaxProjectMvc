using CodinaxProjectMvc.DataAccess.Models.Common;
using CodinaxProjectMvc.DataAccess.Models.Identity;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Reply : BaseEntity
    {
        public string? Content { get; set; }

        public AppUser User { get; set; }
        public Review Review { get; set; }

        public Reply()
        {
            User = new AppUser();
            Review = new Review();
        }
    }
}
