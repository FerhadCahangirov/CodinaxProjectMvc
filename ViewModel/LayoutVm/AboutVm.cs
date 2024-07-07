using CodinaxProjectMvc.DataAccess.Models;

namespace CodinaxProjectMvc.ViewModel.LayoutVm
{
    public class AboutVm
    {
        public IEnumerable<Feature>? Features { get; set; }
        public string BaseUrl { get; set; }
    }
}
