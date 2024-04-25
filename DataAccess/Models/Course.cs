using CodinaxProjectMvc.DataAccess.Models.Common;
using CodinaxProjectMvc.Enums;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Course : BaseEntity
    {
        public string? Title { get; set; }
        public string? Heading { get; set; }
        public string? Description { get; set; }
        public string? HeadingDescription { get; set; }

        public string? CourseCode { get; set; }

        public string? Location { get; set; }

        public DateTime? StartingDate { get; set; }
        public DateTime? FinishingDate { get; set; }

        public string? Content { get; set; }

        public string? FutureJobDesc { get; set; }
        public string? FutureJobSalary { get; set; }

        public string? Properties { get; set; }

        public bool IsDrafted { get; set; }

        public string? CourseFragmentVideoName { get; set; }

        public string? CourseFragmentVideoPathOrContainer { get; set; }

        public string? CourseImageName { get; set; }

        public string? CourseImagePathOrContainer { get; set; }

        public CourseLevels CourseLevel { get; set; }
        public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<Student> Students { get; set; }
        public IEnumerable<Section> Sections { get; set; }
        public Category Category { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
        public IEnumerable<Tool> Tools { get; set; }
        public IEnumerable<Topic> Topics { get; set; }
        public IEnumerable<FutureJobTitle> FutureJobTitles { get; set; }

        public IEnumerable<Price> Prices { get; set; }

        public Template Template { get; set; }

        public Course()
        {
            Instructors = new List<Instructor>();
            Students = new List<Student>();
            Category = new Category();
            Sections = new List<Section>();
            Reviews = new List<Review>();
            Tools = new List<Tool>();
            Topics = new List<Topic>();
            FutureJobTitles = new List<FutureJobTitle>();
            Prices = new List<Price>();
            Template = new Template();
        }
    }
}
