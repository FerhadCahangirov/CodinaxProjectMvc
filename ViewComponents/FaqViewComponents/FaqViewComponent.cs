using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.ViewComponents.FaqViewComponents
{
    public class FaqViewComponent : ViewComponent
    {
        private readonly IReadRepository<Faq> _faqReadRepository;

        public FaqViewComponent(IReadRepository<Faq> faqReadRepository)
        {
            _faqReadRepository = faqReadRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        => View(await _faqReadRepository.GetWhere(x => !x.IsDeleted && !x.IsArchived)
               .ToListAsync());
        
    }
}
