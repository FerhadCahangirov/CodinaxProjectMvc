using CodinaxProjectMvc.ViewModel.PlayerVm;

namespace CodinaxProjectMvc.Business.Abstract.PersistenceServices
{
    public interface IPlayerService
    {
        Task<PlayerListVm> ListStudentVideosAsync(Guid id);
        Task<PlayerSingleVm> GetActiveVideoAsync(Guid id);
    }
}
