using CodinaxProjectMvc.DataAccess.Models;

namespace CodinaxProjectMvc.ViewModel.TemplateVm
{
    public class CoursePricesVm
    {
        public PaginationVm<IEnumerable<Price>> Data { get; set; }


        public Guid TemplateId { get; set; }
    }
}
