using CodinaxProjectMvc.DataAccess.Models.Common;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class FutureJobTitle : BaseEntity
    {
        public string? Content { get; set; }
        public Course Course { get; set; }

        public FutureJobTitle()
        {
            Course = new Course();
        }

    }
}
