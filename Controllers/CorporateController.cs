using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.ViewModel.CorporateVm;
using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Controllers
{
    public class CorporateController : Controller
    {
        private readonly ICorporateService _corporateService;

        public CorporateController(ICorporateService corporateService)
        {
            _corporateService = corporateService;
        }

        public IActionResult Index()
        {
            return View();
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
