using CodinaxProjectMvc.DataAccess.Models.Identity;
using System.Net;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Student : AppUser
    {
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<History> Histories{ get; set; }
        public string? DiscountOrReference { get; set; }

        public IEnumerable<Bookmark> Bookmarks { get; set; }
        public IEnumerable<Progress> Progresses { get; set; }

        public Student()
        {
            Progresses = new List<Progress>();
            Histories = new List<History>();
            Courses = new List<Course>();
            Bookmarks = new List<Bookmark>();
        }
    }
}
