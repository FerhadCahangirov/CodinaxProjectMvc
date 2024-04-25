using CodinaxProjectMvc.DataAccess.Models;

namespace CodinaxProjectMvc.ViewModel.CourseVm
{
    public class CourseInstructorsVm
    {
        public IEnumerable<Instructor>? Instructors { get; set; }
        public Course Course { get; set; }
        public string BaseUrl { get; set; }
    }
}
