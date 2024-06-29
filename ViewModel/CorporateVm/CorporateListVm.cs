using CodinaxProjectMvc.DataAccess.Models;

namespace CodinaxProjectMvc.ViewModel.CorporateVm
{
    public class CorporateListVm
    {
        public IEnumerable<Corporate> Corporates { get; set; }
        public string BaseUrl { get; set; }

        public CorporateListVm()
        {
            Corporates = new List<Corporate>();
            BaseUrl = string.Empty;
        }
    }
}
