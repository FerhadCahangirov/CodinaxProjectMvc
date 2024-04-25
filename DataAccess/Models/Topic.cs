using CodinaxProjectMvc.DataAccess.Models.Common;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Topic : BaseEntity
    {
        public string? Content { get; set; }
        public Course Course { get; set; }

        public Topic()
        {
            Course = new Course();
        }
    }
}
