using CodinaxProjectMvc.ViewModel.ProgressVm;

namespace CodinaxProjectMvc.Business.Abstract.PersistenceServices
{
    public interface IProgressService
    {
        Task AddOrUpdateProgressAsync(ProgressAddOrUpdateVm vm);
    }
}
