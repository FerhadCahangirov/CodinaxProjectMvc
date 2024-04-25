using CodinaxProjectMvc.DataAccess.Models;

namespace CodinaxProjectMvc.ViewModel.InstructorVm
{
    public class InstructorsVm
    {
        public IEnumerable<Instructor>? Instructors { get; set; }
        public IEnumerable<Faq>? Faqs { get; set; }
        public string BaseUrl { get; set; }
    }
}
