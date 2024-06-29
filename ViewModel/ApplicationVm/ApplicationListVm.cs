using CodinaxProjectMvc.DataAccess.Models;

namespace CodinaxProjectMvc.ViewModel.ApplicationVm
{
    public class ApplicationListVm
    {
        public IEnumerable<Application> Applications { get; set; }

        public string BaseUrl { get; set; }

        public ApplicationListVm()
        {
            Applications = new List<Application>();
            BaseUrl = string.Empty;
        }
    }
}
