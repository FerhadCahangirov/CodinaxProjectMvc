using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel;
using CodinaxProjectMvc.ViewModel.FaqVm;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Business.PersistenceServices
{
    public class FaqService : IFaqService
    {
        private readonly IReadRepository<Faq> _faqReadRepository;
        private readonly IWriteRepository<Faq> _faqWriteRepository;
        private readonly IActionContextAccessor _actionContextAccessor;

        public FaqService(IReadRepository<Faq> faqReadRepository, IWriteRepository<Faq> faqWriteRepository, IActionContextAccessor actionContextAccessor)
        {
            _faqReadRepository = faqReadRepository;
            _faqWriteRepository = faqWriteRepository;
            _actionContextAccessor = actionContextAccessor;
        }

        public async Task<bool> CreateAsync(FaqCreateVm faqCreateVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
            {
                return false;
            }

            Faq faq = new Faq()
            {
                Question = faqCreateVm.Question,
                Answer = faqCreateVm.Answer,
            };

            await _faqWriteRepository.AddAsync(faq);
            await _faqWriteRepository.SaveAsync();

            return true;
        }

        public async Task<PaginationVm<IEnumerable<Faq>>> GetFaqsPaginationAsync(string? questionSearchFilter = null, string? answerSearchFilter = null)
        {
            var query = _faqReadRepository.GetWhere(x => !x.IsDeleted);

            if (!string.IsNullOrEmpty(questionSearchFilter))
            {
                questionSearchFilter = questionSearchFilter.ToLower();
                query = query.Where(x => x.Question.ToLower().Contains(questionSearchFilter));
            }

            if (!string.IsNullOrEmpty(answerSearchFilter))
            {
                answerSearchFilter = answerSearchFilter.ToLower();
                query = query.Where(x => x.Answer.ToLower().Contains(answerSearchFilter));
            }

            var faqs = await query.ToListAsync();

            int total = await query.CountAsync();

            PaginationVm<IEnumerable<Faq>> pagination = new PaginationVm<IEnumerable<Faq>>(total, faqs);

            return pagination;
        }

        public async Task<FaqUpdateVm> GetFaqUpdateDataAsync(Guid id)
        {
            Faq? faq = await _faqReadRepository.GetSingleAsync(x => x.Id == id);
            if (faq == null) return new FaqUpdateVm();

            return new FaqUpdateVm()
            {
                Question = faq.Question,
                Answer = faq.Answer,
                Id = id
            };
        }

        public async Task<bool> UpdateAsync(FaqUpdateVm faqUpdateVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
            {
                return false;
            }

            Faq? faq = await _faqReadRepository.GetSingleAsync(x => x.Id == faqUpdateVm.Id);
            if (faq == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(FaqUpdateVm.Id), "Faq Item Not Found");
                return false;
            }

            faq.Question = faqUpdateVm.Question;
            faq.Answer = faqUpdateVm.Answer;

            _faqWriteRepository.Update(faq);
            await _faqWriteRepository.SaveAsync();
            return true;
        }

        public async Task<bool> ArchiveAsync(Guid id)
        {
            Faq? faq = await _faqReadRepository.GetSingleAsync(x => x.Id == id);
            if (faq == null) return false;

            faq.IsArchived = true;

            _faqWriteRepository.Update(faq);
            await _faqWriteRepository.SaveAsync();
            return true;
        }

        public async Task<bool> UnArchiveAsync(Guid id)
        {
            Faq? faq = await _faqReadRepository.GetSingleAsync(x => x.Id == id);
            if (faq == null)
            {
                return false;
            }

            faq.IsArchived = false;

            _faqWriteRepository.Update(faq);
            await _faqWriteRepository.SaveAsync();

            return false;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            Faq? faq = await _faqReadRepository.GetSingleAsync(x => x.Id == id);
            if (faq == null) return false;

            faq.IsDeleted = true;

            _faqWriteRepository.Update(faq);
            await _faqWriteRepository.SaveAsync();

            return true;
        }

        public async Task<bool> RestoreAsync(Guid id)
        {
            Faq? faq = await _faqReadRepository.GetSingleAsync(x => x.Id == id);
            if (faq == null)
            {
                return false;
            };

            faq.IsDeleted = false;

            _faqWriteRepository.Update(faq);
            await _faqWriteRepository.SaveAsync();

            return true;
        }
    }
}
