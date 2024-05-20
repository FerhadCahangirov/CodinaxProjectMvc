using CodinaxProjectMvc.DataAccess.Models.Common;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Module : BaseEntity
    {
        public string Title { get; set; }
        public Course Course { get; set; }
        public IEnumerable<Lecture> Lectures { get; set; }

        public IEnumerable<Bookmark> Bookmarks { get; set; }
        public Module()
        {
            Course = new Course();
            Lectures = new List<Lecture>();
            Title = string.Empty;
            Bookmarks = new List<Bookmark>();
        }

        public Module(string title, Course course)
        {
            Course = course;
            Title = title;
            Lectures = new List<Lecture>();
            Bookmarks = new List<Bookmark>();
        }
    }
}
