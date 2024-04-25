using CodinaxProjectMvc.DataAccess.Models.Identity;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Student : AppUser
    {
        public IEnumerable<Course> Courses { get; set; }
        public string? DiscountOrReference { get; set; }

        public Student()
        {
            Courses = new List<Course>();
        }
    }
}
