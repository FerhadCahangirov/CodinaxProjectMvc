using CodinaxProjectMvc.DataAccess.Models.Common;
using CodinaxProjectMvc.DataAccess.Models.Identity;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Review : BaseEntity
    {
        public decimal Rate { get; set; }
        public string? Content { get; set; }

        public AppUser User { get; set; }
        public Course Course { get; set; }
        public IEnumerable<Reply> Replies { get; set; }

        public Review()
        {
            User = new AppUser();
            Course = new Course();
            Replies = new List<Reply>();
        }
    }
}
