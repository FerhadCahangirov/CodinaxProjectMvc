using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel;
using CodinaxProjectMvc.ViewModel.AboutVm;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Business.PersistenceServices
{
    public class AboutService : IAboutService
    {
        private readonly IReadRepository<About> _aboutReadRepository;
        private readonly IWriteRepository<About> _aboutWriteRepository;
        private readonly IActionContextAccessor _actionContextAccessor;
        private const int _maxAboutCount = 4;

        public AboutService(IReadRepository<About> aboutReadRepository, IWriteRepository<About> aboutWriteRepository, IActionContextAccessor actionContextAccessor)
        {
            _aboutReadRepository = aboutReadRepository;
            _aboutWriteRepository = aboutWriteRepository;
            _actionContextAccessor = actionContextAccessor;
        }

        public async Task<AboutActionReturnType> CreateAsync(AboutCreateVm aboutCreateVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
            {
                return AboutActionReturnType.Failure;
            }

            if (_aboutReadRepository.GetWhere(x => !x.IsDeleted && !x.IsArchived).Count() >= _maxAboutCount)
            {
                return AboutActionReturnType.Oversized;
            }

            About about = new About()
            {
                Content = aboutCreateVm.Content,
            };

            await _aboutWriteRepository.AddAsync(about);
            await _aboutWriteRepository.SaveAsync();

            return AboutActionReturnType.Success;
        }

        public async Task<PaginationVm<IEnumerable<About>>> GetAboutsPaginationAsync(string? searchFilter = null, string? statusFilter = null)
        {
            var query = _aboutReadRepository.GetWhere(x => !x.IsDeleted);

            if (!string.IsNullOrEmpty(searchFilter))
            {
                searchFilter = searchFilter.ToLower();
                query = query.Where(x => x.Content.ToLower().Contains(searchFilter));
            }

            var abouts = await query.ToListAsync();

            int total = await query.CountAsync();

            PaginationVm<IEnumerable<About>> pagination = new PaginationVm<IEnumerable<About>>(total, abouts);

            return pagination;
        }

        public async Task<AboutUpdateVm> GetAboutUpdateDataAsync(Guid id)
        {
            About? about = await _aboutReadRepository.GetSingleAsync(x => x.Id == id);
            if (about == null) return new AboutUpdateVm();

            return new AboutUpdateVm()
            {
                Content = about.Content,
                Id = id
            }; 
        }

        public async Task<bool> UpdateAsync(AboutUpdateVm aboutUpdateVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
            {
                return false;
            }

            About? about = await _aboutReadRepository.GetSingleAsync(x => x.Id == aboutUpdateVm.Id);
            if (about == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(AboutUpdateVm.Id), "About Item Not Found");
                return false;
            }

            about.Content = aboutUpdateVm.Content;

            _aboutWriteRepository.Update(about);
            await _aboutWriteRepository.SaveAsync();
            return true;    
        }

        public async Task<bool> ArchiveAsync(Guid id)
        {
            About? about = await _aboutReadRepository.GetSingleAsync(x => x.Id == id);
            if (about == null) return false;

            about.IsArchived = true;

            _aboutWriteRepository.Update(about);
            await _aboutWriteRepository.SaveAsync();
            return true;
        }

        public async Task<AboutActionReturnType> UnArchiveAsync(Guid id)
        {
            About? about = await _aboutReadRepository.GetSingleAsync(x => x.Id == id);
            if (about == null)
            {
                return AboutActionReturnType.Failure;
            }

            if (_aboutReadRepository.GetWhere(x => !x.IsDeleted && !x.IsArchived).Count() >= _maxAboutCount)
            {
                return AboutActionReturnType.Oversized;
            }

            about.IsArchived = false;

            _aboutWriteRepository.Update(about);
            await _aboutWriteRepository.SaveAsync();

            return AboutActionReturnType.Success;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            About? about = await _aboutReadRepository.GetSingleAsync(x => x.Id == id);
            if (about == null) return false;

            about.IsDeleted = true;

            _aboutWriteRepository.Update(about);
            await _aboutWriteRepository.SaveAsync();

            return true;
        }

        public async Task<AboutActionReturnType> RestoreAsync(Guid id)
        {
            About? about = await _aboutReadRepository.GetSingleAsync(x => x.Id == id);
            if (about == null)
            {
                return AboutActionReturnType.Failure;
            };

            if (_aboutReadRepository.GetWhere(x => !x.IsDeleted && !x.IsArchived).Count() >= _maxAboutCount)
            {
                return AboutActionReturnType.Oversized;
            }

            about.IsDeleted = false;

            _aboutWriteRepository.Update(about);
            await _aboutWriteRepository.SaveAsync();

            return AboutActionReturnType.Success;
        }
    }
}
