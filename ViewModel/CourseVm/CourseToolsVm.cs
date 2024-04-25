using CodinaxProjectMvc.DataAccess.Models;

namespace CodinaxProjectMvc.ViewModel.CourseVm
{
    public class CourseToolsVm
    {
        public IEnumerable<Tool>? Tools { get; set; }
        public Course Course { get; set; }
        public string BaseUrl { get; set; }
    }
}
