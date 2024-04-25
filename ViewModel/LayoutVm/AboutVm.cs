using CodinaxProjectMvc.DataAccess.Models;

namespace CodinaxProjectMvc.ViewModel.LayoutVm
{
    public class AboutVm
    {
        public IEnumerable<About>? Abouts { get; set; }
        public IEnumerable<Feature>? Features { get; set; }
        public IEnumerable<Faq>? Faqs { get; set; }
        public string BaseUrl { get; set; }
    }
}
