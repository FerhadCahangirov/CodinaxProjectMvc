using CodinaxProjectMvc.DataAccess.Models.Common;
using CodinaxProjectMvc.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Course : BaseEntity
    {
        public string? Title { get; set; }
        
        public string? CourseCode { get; set; }

        public bool Showcase { get; set; } = true;

        public bool IsPrimary { get; set; } = false; 

        public CourseLevels CourseLevel { get; set; }

        public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<Student> Students { get; set; }
        public IEnumerable<Module> Modules { get; set; }
        public Category Category { get; set; }

        public Template Template { get; set; }

        public Course()
        {
            Instructors = new List<Instructor>();
            Students = new List<Student>();
            Category = new Category();
            Modules = new List<Module>();
            Template = new Template();
        }
    }
}
