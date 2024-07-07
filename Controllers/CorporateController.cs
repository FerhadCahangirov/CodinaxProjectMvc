using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Filters;
using CodinaxProjectMvc.ViewModel.CorporateVm;
using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Controllers
{
    [CurrentLangFilterFactory]
    public class CorporateController : Controller
    {
        private readonly ICorporateService _corporateService;

        public CorporateController(ICorporateService corporateService)
        {
            _corporateService = corporateService;
        }

        public async Task<IActionResult> Index()
        {
            return View(new CorporateSendVm(await _corporateService.ListCorporatesAsync()));
        }

        [HttpPost]
        public async Task<IActionResult> Index(CorporateSendVm corporateSendVm)
        {
            bool result = await _corporateService.SendCorporateAsync(corporateSendVm);

            if(!result)
            {
                return View(corporateSendVm);
            }

            TempData["CorporateSent"] = true;
            return View();
        }
    }
}
