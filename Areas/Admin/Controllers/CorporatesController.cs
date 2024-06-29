using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.ViewModel.CorporateVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = PolicyConstants.AdminPolicy)]
    public class CorporatesController : Controller
    {
        private readonly ICorporateService _corporateService;

        public CorporatesController(ICorporateService corporateService)
        {
            _corporateService = corporateService;
        }

        public async Task<IActionResult> Index()
            => View(await _corporateService.ListCorporatesAsync());

        public async Task<IActionResult> Edit(Guid id)
            => View(await _corporateService.GetCorporateUpdateDataAsync(id));

        [HttpPost]
        public async Task<IActionResult> Edit(CorporateUpdateVm corporateUpdateVm)
        {
            bool result = await _corporateService.EditCorporateAsync(corporateUpdateVm);

            if (!result)
            {
                return View(corporateUpdateVm);
            }

            TempData["CorporateEdited"] = true;
            return Redirect("/Admin/Corporates/Index");
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
            => new JsonResult(new { success = await _corporateService.DeleteCorporateAsync(id) });

        [HttpPost]
        public async Task<JsonResult> ChangeShowcase(Guid id)
            => new JsonResult(new { success = await _corporateService.ChangeShowcaseAsync(id) });

        [HttpPost]
        public async Task<JsonResult> Approve(Guid id)
            => new JsonResult(new { success = await _corporateService.ApproveAsync(id) });

        [HttpPost]

        public async Task<JsonResult> UploadLogo(Guid id, IFormFile file)
            => new JsonResult(new { success = await _corporateService.UploadLogoAsync(id, file) });

        [HttpPut]
        public async Task<JsonResult> ChangeLogo(Guid id, IFormFile file)
            => new JsonResult(new { success = await _corporateService.ChangeLogoAsync(id, file) });

        [HttpDelete]
        public async Task<JsonResult> DeleteLogo(Guid id)
            => new JsonResult(new { success = await _corporateService.DeleteLogoAsync(id) });
    }
}
