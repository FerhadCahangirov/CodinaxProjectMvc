using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel.ModuleVm;

namespace CodinaxProjectMvc.Business.Abstract.PersistenceServices
{
    public interface IModuleService
    {
        Task<bool> CreateModuleAsync(ModuleCreateVm moduleCreateVm);

        Task<ModuleUpdateVm> GetModuleUpdateDataAsync(Guid id);

        Task<bool> UpdateModuleAsync(ModuleUpdateVm moduleUpdateVm);

        Task<Module?> GetModuleByIdAsync(Guid id); 
    }
}
