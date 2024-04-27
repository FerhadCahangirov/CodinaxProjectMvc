using CodinaxProjectMvc.DataAccess.Models;

namespace CodinaxProjectMvc.ViewModel.TemplateVm
{
    public class CourseToolsVm
    {
        public IEnumerable<Tool>? Tools { get; set; }
        public Guid TemplateId { get; set; }
        public string BaseUrl { get; set; }
    }
}
