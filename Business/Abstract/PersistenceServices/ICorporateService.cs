using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel.CorporateVm;

namespace CodinaxProjectMvc.Business.Abstract.PersistenceServices
{
    public interface ICorporateService
    {
        Task<bool> SendCorporateAsync(CorporateSendVm corporateSendVm);
        Task<bool> EditCorporateAsync(CorporateUpdateVm corporateUpdateVm);
        Task<CorporateUpdateVm> GetCorporateUpdateDataAsync(Guid id);
        Task<bool> DeleteCorporateAsync(Guid id);
        Task<CorporateListVm> ListCorporatesAsync();
        Task<CorporateListVm> CorporatesPartialAsync(string? searchFilter = null);
        Task<CorporateListVm> ListApprovedCorporatesAsync();
        Task<bool> ApproveAsync(Guid id);
        Task<bool> ChangeShowcaseAsync(Guid id);
        Task<bool> UploadLogoAsync(Guid id, IFormFile file);
        Task<bool> ChangeLogoAsync(Guid id, IFormFile file);
        Task<bool> DeleteLogoAsync(Guid id);


    }
}
