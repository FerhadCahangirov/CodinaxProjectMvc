using CodinaxProjectMvc.DataAccess.Models.Common;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Bookmark : BaseEntity
    {
        public Student Student { get; set; }

        public Bookmark()
        {
            Student = new Student();
        }
    }
}
