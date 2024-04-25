using CodinaxProjectMvc.DataAccess.Models;

namespace CodinaxProjectMvc.ViewModel.LayoutVm
{
    public class HomeVm
    {
        public IEnumerable<About>? Abouts { get; set; }
        public IEnumerable<Faq>? Faqs { get; set; }
        public IEnumerable<Course>? Courses { get; set; }
        public IEnumerable<Instructor>? Instructors { get; set; }
        public string BaseUrl { get; set; }
    }
}
