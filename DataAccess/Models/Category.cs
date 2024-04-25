using CodinaxProjectMvc.DataAccess.Models.Common;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Category : BaseEntity
    {
        public string? Content { get; set; }
        public IEnumerable<Course> Courses { get; set; }

        public Category()
        {
            Courses = new List<Course>();
        }
    }
}
