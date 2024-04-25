using CodinaxProjectMvc.Business.Abstract.InfrastructureServices;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel.LayoutVm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Controllers
{
    public class ContactController : Controller
    {
        private readonly IReadRepository<Faq> _faqReadRepository;
        private readonly IContactService _contactService;
        public ContactController(IContactService contactService, IReadRepository<Faq> faqReadRepository)
        {
            _contactService = contactService;
            _faqReadRepository = faqReadRepository;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Faq> faqs = await _faqReadRepository.GetWhere(x => !x.IsDeleted && !x.IsArchived).ToListAsync();

            ContactVm contactVm = new ContactVm(faqs);
            return View(contactVm);
        }

        [HttpPost]
        public async Task<IActionResult> Send(ContactVm contactVm)
        {
            IEnumerable<Faq> faqs = await _faqReadRepository.GetWhere(x => !x.IsDeleted && !x.IsArchived).ToListAsync();
            contactVm.Faqs = faqs;
            bool result = await _contactService.SendAsync(contactVm);

            if (!result)
            {
                TempData["ContactMailSendError"] = true;
                return View(viewName: nameof(Index), contactVm);
            }

            TempData["ContactMailSendSuccess"] = true;
            return RedirectToAction(nameof(Index));
        }
    }
}
