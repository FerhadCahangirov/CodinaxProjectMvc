using CodinaxProjectMvc.DataAccess.Models.Identity;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Instructor : AppUser
    {
        public IEnumerable<Course> Courses { get; set; }
        public string? Description { get; set; }
        public string? Profession { get; set; }
        public IEnumerable<Event> Events { get; set; }

        public bool Showcase { get; set; }

        public Instructor()
        {
            Courses = new List<Course>();
            Events = new List<Event>();
        }
    }
}
