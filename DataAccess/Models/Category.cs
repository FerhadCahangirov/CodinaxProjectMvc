using CodinaxProjectMvc.DataAccess.Models.Common;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Category : BaseEntity
    {
        public string Content { get; set; }
        public string? ContentRu { get; set; }
        public string? ContentTr { get; set; }
        public IEnumerable<Course> Courses { get; set; }

        public Category()
        {
            Courses = new List<Course>();
            Content = string.Empty;
        }
    }
}
