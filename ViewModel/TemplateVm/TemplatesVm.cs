using CodinaxProjectMvc.DataAccess.Models;

namespace CodinaxProjectMvc.ViewModel.TemplateVm
{
    public class TemplatesVm
    {
        public IEnumerable<Template>? Templates { get; set;}
        public string BaseUrl { get; set; }
    }
}
