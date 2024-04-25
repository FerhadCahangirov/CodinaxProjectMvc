using CodinaxProjectMvc.DataAccess.Models.Common;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Section : BaseEntity
    {
        public string? Title { get; set; }
        public int Number { get; set; }

        public Course Course { get; set; }
        public IEnumerable<Lecture> Lectures { get; set; }

        public Section()
        {
            Course = new Course();
            Lectures = new List<Lecture>();
        }
    }
}
