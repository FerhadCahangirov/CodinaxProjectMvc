using CodinaxProjectMvc.ViewModel.LayoutVm;

namespace CodinaxProjectMvc.Business.Abstract.PersistenceServices
{
    public interface IDashboardService
    {
        Task<DashboardVm> ListDashboardAsync();
    }
}
