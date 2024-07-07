using CodinaxProjectMvc.DataAccess.Models;

namespace CodinaxProjectMvc.ViewModel.CorporateVm
{
    public class CorporateListVm
    {
        public IEnumerable<Corporate> Items { get; set; }
        public string BaseUrl { get; set; }

        public CorporateListVm()
        {
            Items = new List<Corporate>();
            BaseUrl = string.Empty;
        }
    }
}
