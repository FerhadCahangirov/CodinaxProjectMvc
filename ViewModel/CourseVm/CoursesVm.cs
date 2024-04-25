using CodinaxProjectMvc.DataAccess.Models;

namespace CodinaxProjectMvc.ViewModel.CourseVm
{
    public class CoursesVm
    {
        public IEnumerable<Course>? Courses { get; set; }
        public IEnumerable<Category>? Categories { get; set; }
        public IEnumerable<Instructor>? Instructors { get; set; }
        public IEnumerable<Faq>? Faqs { get; set; }
        public string? BaseUrl { get; set; }
    }
}
