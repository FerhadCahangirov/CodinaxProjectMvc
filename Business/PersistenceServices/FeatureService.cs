using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Abstract.Storages;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel;
using CodinaxProjectMvc.ViewModel.FeatureVm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Business.PersistenceServices
{
    public class FeatureService : IFeatureService
    {
        private readonly IReadRepository<Feature> _featureReadRepository;
        private readonly IWriteRepository<Feature> _featureWriteRepository;
        private readonly IAzureStorage _storage;
        private readonly IConfiguration _configuration;
        private readonly IActionContextAccessor _actionContextAccessor;
        private const int _maxFeatureCount = 3;

        public FeatureService(IReadRepository<Feature> featureReadRepository, IWriteRepository<Feature> featureWriteRepository, IConfiguration configuration, IActionContextAccessor actionContextAccessor, IAzureStorage storage)
        {
            _featureReadRepository = featureReadRepository;
            _featureWriteRepository = featureWriteRepository;
            _configuration = configuration;
            _actionContextAccessor = actionContextAccessor;
            _storage = storage;
        }

        public async Task<FeatureActionReturnType> CreateFeatureAsync(FeatureCreateVm featureCreateVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
                return FeatureActionReturnType.Failure;

            if (_featureReadRepository.GetWhere(x => !x.IsDeleted && !x.IsArchived).Count() >= _maxFeatureCount)
                return FeatureActionReturnType.Oversized;

            IFormFileCollection files = new FormFileCollection { featureCreateVm.Icon };

            var uploadedFiles = await _storage.UploadAsync("feature-icons", files);
            var uploadedFile = uploadedFiles[0];

            Feature feature = new Feature()
            {
                Title = featureCreateVm.Title,
                IconPathOrContainer = uploadedFile.pathOrContainerName,
                IconName = uploadedFile.fileName,
                TitleRu = featureCreateVm.TitleRu,
                TitleTr = featureCreateVm.TitleTr,
            };

            await _featureWriteRepository.AddAsync(feature);
            await _featureWriteRepository.SaveAsync();

            return FeatureActionReturnType.Success;

        }

        public async Task<PaginationVm<IEnumerable<Feature>>> GetFeaturesAsync(string? searchFilter = null)
        {
            var query = _featureReadRepository.GetWhere(x => !x.IsDeleted);

            if (!string.IsNullOrEmpty(searchFilter))
            {
                searchFilter = searchFilter.ToLower();
                query = query.Where(x =>
                    x.Title.ToLower().Contains(searchFilter)
                );
            }

            var features = await query.ToListAsync();
            int total = await query.CountAsync();

            PaginationVm<IEnumerable<Feature>> data = new PaginationVm<IEnumerable<Feature>>(total, features);

            data.BaseUrl = _configuration[ConfigurationStrings.AzureBaseUrl];

            return data;
        }

        public async Task<FeatureUpdateVm> GetFeatureUpdateDataAsync(Guid id)
        {
            Feature? feature = await _featureReadRepository.GetSingleAsync(x => x.Id == id);
            if (feature == null) return new FeatureUpdateVm();

            FeatureUpdateVm featureUpdateVm = new FeatureUpdateVm()
            {
                Title = feature.Title,
                TitleRu = feature.TitleRu,
                TitleTr = feature.TitleTr,
                IconPathOrContainer = feature.IconPathOrContainer,
                IconName = feature.IconName,
                Id = id
            };

            return featureUpdateVm;
        }

        public async Task<bool> UpdateFeatureAsync(FeatureUpdateVm featureUpdateVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
                return false;

            Feature? feature = await _featureReadRepository.GetSingleAsync(x => x.Id == featureUpdateVm.Id);
            if (feature == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(FeatureUpdateVm.Id), "Feature Not Found");
                return false;
            };

            if (featureUpdateVm.Icon != null)
            {
                IFormFileCollection files = new FormFileCollection { featureUpdateVm.Icon };

                var uploadedFiles = await _storage.UploadAsync(AzureContainerNames.FeatureIcons, files);
                var (fileName, pathOrContainerName) = uploadedFiles[0];

                feature.IconPathOrContainer = pathOrContainerName;
                feature.IconName = fileName;
            }

            feature.Title = featureUpdateVm.Title;
            feature.TitleTr = featureUpdateVm.TitleTr;
            feature.TitleRu = featureUpdateVm.TitleRu;

            _featureWriteRepository.Update(feature);
            await _featureWriteRepository.SaveAsync();

            return true;
        }

        public async Task<bool> ArchiveAsync(Guid id)
        {
            Feature? feature = await _featureReadRepository.GetSingleAsync(x => x.Id == id);
            if (feature == null) return false;

            feature.IsArchived = true;

            _featureWriteRepository.Update(feature);
            await _featureWriteRepository.SaveAsync();

            return true;
        }

        public async Task<FeatureActionReturnType> UnarchiveAsync(Guid id)
        {

            Feature? feature = await _featureReadRepository.GetSingleAsync(x => x.Id == id);
            if (feature == null) return FeatureActionReturnType.Failure;

            if (_featureReadRepository.GetWhere(x => !x.IsDeleted && !x.IsArchived).Count() >= _maxFeatureCount)
                return FeatureActionReturnType.Oversized;

            feature.IsArchived = false;

            _featureWriteRepository.Update(feature);
            await _featureWriteRepository.SaveAsync();

            return FeatureActionReturnType.Success;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            Feature? feature = await _featureReadRepository.GetSingleAsync(x => x.Id == id);
            if (feature == null) return false;

            feature.IsDeleted = true;

            _featureWriteRepository.Update(feature);
            await _featureWriteRepository.SaveAsync();

            return true;
        }

        public async Task<FeatureActionReturnType> RestoreAsync(Guid id)
        {
            Feature? feature = await _featureReadRepository.GetSingleAsync(x => x.Id == id);
            if (feature == null)
            {
                return FeatureActionReturnType.Failure;
            }

            if (_featureReadRepository.GetWhere(x => !x.IsDeleted && !x.IsArchived).Count() >= _maxFeatureCount)
            {
                return FeatureActionReturnType.Oversized;
            }

            feature.IsDeleted = false;

            _featureWriteRepository.Update(feature);
            await _featureWriteRepository.SaveAsync();

            return FeatureActionReturnType.Success;
        }
    }
}
