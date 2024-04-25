using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.DataAccess.Models.Common;
using Microsoft.AspNetCore.Identity;

namespace CodinaxProjectMvc.DataAccess.Models.Identity
{
    public class AppUser : IdentityUser<Guid>, IBaseGenericEntity
    {
        public bool IsApproved { get; set; }
        public bool IsBanned { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? ProfileImageName { get; set; }
        public string? ProfileImagePathOrContainer { get; set; }

        public string? AdditionalNotes { get; set; }

        public string? CountryOfBirth { get; set; }
        public string? CountryOfResidence { get; set; }

        public IEnumerable<Review>? Reviews { get; set; }
        public IEnumerable<Reply>? Replies { get; set; }

        public AppUser()
        {
            Reviews = new List<Review>();
            Replies = new List<Reply>();
        }
    }
}
