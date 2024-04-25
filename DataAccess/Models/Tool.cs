using CodinaxProjectMvc.DataAccess.Models.Common;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Tool : FileEntity
    {
        public string? Content { get; set; }
        public Course Course { get; set; }

        public Tool()
        {
            Course = new Course();
        }
    }
}
