using Azure.Storage.Blobs.Models;
using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Abstract.Storages;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel.CorporateVm;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Build.Execution;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace CodinaxProjectMvc.Business.PersistenceServices
{
    public class CorporateService : ICorporateService
    {
        private readonly IReadRepository<Corporate> _corporateReadRepository;
        private readonly IWriteRepository<Corporate> _corporateWriteRepository;

        private readonly IAzureStorage _storage;
        private readonly IActionContextAccessor _actionContextAccessor;

        private readonly string _baseUrl;

        public CorporateService(
            IReadRepository<Corporate> corporateReadRepository,
            IWriteRepository<Corporate> corporateWriteRepository,
            IAzureStorage storage,
            IConfiguration configuration,
            IActionContextAccessor actionContextAccessor)

        {
            _corporateReadRepository = corporateReadRepository;
            _corporateWriteRepository = corporateWriteRepository;
            _storage = storage;
            _baseUrl = configuration[ConfigurationStrings.AzureBaseUrl];
            _actionContextAccessor = actionContextAccessor;
        }

        public async Task<bool> DeleteCorporateAsync(Guid id)
        {
            Corporate? corporate = await _corporateReadRepository.GetSingleAsync(x => x.Id == id);

            if (corporate == null) return false;

            _corporateWriteRepository.Remove(corporate);
            await _corporateWriteRepository.SaveAsync();

            return true;
        }

        public async Task<bool> EditCorporateAsync(CorporateUpdateVm corporateUpdateVm)
        {
            if(!_actionContextAccessor.ActionContext.ModelState.IsValid) 
            {
                return false;
            }

            Corporate? corporate = await _corporateReadRepository.GetSingleAsync(x => x.Id == corporateUpdateVm.Id);

            if (corporate == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(CorporateUpdateVm.Id), "Corporate Not Found.");
                return false;
            }

            corporate.FirstName = corporateUpdateVm.FirstName;
            corporate.LastName = corporateUpdateVm.LastName;
            corporate.PhoneNumber = corporateUpdateVm.PhoneNumber;
            corporate.Email = corporateUpdateVm.Email;
            corporate.WorkingCompany = corporateUpdateVm.WorkingCompany;
            corporate.Occupation = corporateUpdateVm.Occupation;
            corporate.AdditionalInfo = corporateUpdateVm.AdditionalInfo;
            corporate.IsApproved = corporateUpdateVm.IsApproved;
            corporate.Showcase = corporateUpdateVm.Showcase;

            
            _corporateWriteRepository.Update(corporate);
            await _corporateWriteRepository.SaveAsync();

            return true;
        }

        public async Task<CorporateUpdateVm> GetCorporateUpdateDataAsync(Guid id)
        {
            Corporate? corporate = await _corporateReadRepository.GetSingleAsync(x => x.Id == id);

            if (corporate == null) return new();

            return new()
            {
                Id = id,
                FirstName = corporate.FirstName,
                LastName = corporate.LastName,
                PhoneNumber = corporate.PhoneNumber,
                Email = corporate.Email,
                Occupation = corporate.Occupation,
                WorkingCompany = corporate.WorkingCompany,
                AdditionalInfo = corporate.AdditionalInfo,
                IsApproved = corporate.IsApproved,
                Showcase = corporate.Showcase,
                LogoName = corporate.LogoName,
                LogoPath = corporate.LogoPath,
                BaseUrl = _baseUrl
            };
        }

        public async Task<bool> UploadLogoAsync(Guid id, IFormFile file)
        {
            Corporate? corporate = await _corporateReadRepository.GetSingleAsync(x => x.Id == id);
            if (corporate == null) return false;

            (string fileName, string pathOrContainerName) = await _storage.UploadAsync(AzureContainerNames.CorporateLogos, file);

            corporate.LogoName = fileName;
            corporate.LogoPath = pathOrContainerName;

            _corporateWriteRepository.Update(corporate);
            await _corporateWriteRepository.SaveAsync();

            return true;
        }

        public async Task<bool> ChangeLogoAsync(Guid id, IFormFile file)
        {
            Corporate? corporate = await _corporateReadRepository.GetSingleAsync(x => x.Id == id);
            if (corporate == null) return false;

            await _storage.DeleteAsync(corporate.LogoPath, corporate.LogoName);

            (string fileName, string pathOrContainerName) = await _storage.UploadAsync(AzureContainerNames.CorporateLogos, file);

            corporate.LogoName = fileName;
            corporate.LogoPath = pathOrContainerName;

            _corporateWriteRepository.Update(corporate);
            await _corporateWriteRepository.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteLogoAsync(Guid id)
        {
            Corporate? corporate = await _corporateReadRepository.GetSingleAsync(x => x.Id == id);
            if (corporate == null) return false;

            await _storage.DeleteAsync(corporate.LogoPath, corporate.LogoName);

            corporate.LogoName = null;
            corporate.LogoPath = null;

            _corporateWriteRepository.Update(corporate);
            await _corporateWriteRepository.SaveAsync();

            return true;
        }

        public async Task<CorporateListVm> ListCorporatesAsync()
            => new CorporateListVm() {
                Items = await _corporateReadRepository.GetAll().ToListAsync(),
                BaseUrl = _baseUrl  
            };

        public async Task<CorporateListVm> ListApprovedCorporatesAsync()
            => new CorporateListVm()
            {
                Items = await _corporateReadRepository.GetWhere(x => x.IsApproved && x.Showcase).ToListAsync(),
                BaseUrl = _baseUrl
            };


        public async Task<bool> ApproveAsync(Guid id)
        {
            Corporate? corporate = await _corporateReadRepository.GetSingleAsync(x => x.Id == id);

            if (corporate == null) return false;

            corporate.IsApproved = true;

            _corporateWriteRepository.Update(corporate);
            await _corporateWriteRepository.SaveAsync();

            return true;
        }

        public async Task<bool> ChangeShowcaseAsync(Guid id)
        {
            Corporate? corporate = await _corporateReadRepository.GetSingleAsync(x => x.Id == id);

            if (corporate == null || !corporate.IsApproved)
            {
                return false;
            }

            corporate.Showcase = !corporate.Showcase;

            _corporateWriteRepository.Update(corporate);
            await _corporateWriteRepository.SaveAsync();

            return true;
        }

        public async Task<bool> SendCorporateAsync(CorporateSendVm corporateSendVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
            {
                return false;
            }

            Corporate corporate = new()
            {
                FirstName = corporateSendVm.FirstName,
                LastName = corporateSendVm.LastName,
                PhoneNumber = corporateSendVm.PhoneNumber,
                Email = corporateSendVm.Email,
                Occupation = corporateSendVm.Occupation,
                WorkingCompany = corporateSendVm.WorkingCompany,
                AdditionalInfo = corporateSendVm.AdditionalInfo,
                IsApproved = false,
                Showcase = false
            };

            await _corporateWriteRepository.AddAsync(corporate);
            await _corporateWriteRepository.SaveAsync();

            return true;    
        }

        public async Task<CorporateListVm> CorporatesPartialAsync(string? searchFilter = null)
        {
            IQueryable<Corporate> corporates = _corporateReadRepository.GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(searchFilter))
            {
                searchFilter = searchFilter.ToLower().Trim();
                corporates = corporates.Where(x =>
                    x.FirstName.ToLower().Contains(searchFilter) ||
                    x.LastName.ToLower().Contains(searchFilter) ||
                    x.Occupation.ToLower().Contains(searchFilter) ||
                    x.WorkingCompany.ToLower().Contains(searchFilter)
                );
            }

            return new CorporateListVm()
            {
                Items = await corporates.ToListAsync(),
                BaseUrl = _baseUrl
            };
        }
    }
}
