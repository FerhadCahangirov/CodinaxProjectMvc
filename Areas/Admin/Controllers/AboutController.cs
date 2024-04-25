using CodinaxProjectMvc.Context;
using CodinaxProjectMvc.ViewModel.AboutVm;
using CodinaxProjectMvc.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.Business.Abstract.PersistenceServices;

namespace CodinaxProjectMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = PolicyConstants.AdminPolicy)]
    public class AboutController : Controller
    {
        private const string _aboutsPartialConstant = "AboutsPartial";


        private readonly IAboutService _aboutService;

        public AboutController(IAboutService aboutService)
        {
            _aboutService = aboutService;
        }

        [HttpPost]
        public async Task<IActionResult> AboutsPartial(string? searchFilter)
        {
            var data = await _aboutService.GetAboutsPaginationAsync(searchFilter);

            if (data.Items?.ToList().Count == 0)
            {
                ViewBag.Message = "Nothing Found";
            }

            return PartialView(viewName: _aboutsPartialConstant, data);
        }

        public async Task<IActionResult> Index()
        {

            var data = await _aboutService.GetAboutsPaginationAsync();

            if (data.Items?.ToList().Count == 0)
            {
                ViewBag.Message = "Nothing Found";
            }

            return View(data);
        }

        public IActionResult Create()
        {
            TempData["Created"] = false;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AboutCreateVm vm)
        {
            AboutActionReturnType result = await _aboutService.CreateAsync(vm);

            return ReturnAction(result, () =>
            {
                TempData["Created"] = true;
                return Redirect("/Admin/About/Index");
            }, () =>
            {
                return View(vm);
            }, () =>
            {
                TempData["AboutOversized"] = true;
                return Redirect("/Admin/About/Index");
            });
        }

        public async Task<IActionResult> Update(Guid id)
        {
            AboutUpdateVm vm = await _aboutService.GetAboutUpdateDataAsync(id);
            TempData["Updated"] = false;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AboutUpdateVm vm)
        {
            bool result = await _aboutService.UpdateAsync(vm);

            if (!result)
                return View(vm);

            TempData["Updated"] = true;
            return Redirect("/Admin/About/Index");
        }

        [HttpPost]
        public async Task<IActionResult> Archive(Guid id)
        {
            bool result = await _aboutService.ArchiveAsync(id);
            return new JsonResult(new { success = result });
        }

        [HttpPost]
        public async Task<IActionResult> UnArchive(Guid id)
        {
            AboutActionReturnType result = await _aboutService.UnArchiveAsync(id);

            return ReturnAction(result, () => new JsonResult(new { success = true })
            , () =>
                new JsonResult(new { success = false, message = FeatureActionReturnType.Oversized.ToString() })
            , () =>
               new JsonResult(new { success = false, message = FeatureActionReturnType.Failure.ToString() })
            );
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool result = await _aboutService.DeleteAsync(id);
            return new JsonResult(new { success = result });
        }

        [HttpPost]
        public async Task<IActionResult> Restore(Guid id)
        {
            AboutActionReturnType result = await _aboutService.RestoreAsync(id);

            return ReturnAction(result, () => new JsonResult(new { success = true })
            , () =>
                new JsonResult(new { success = false, message = FeatureActionReturnType.Oversized.ToString() })
            , () =>
               new JsonResult(new { success = false, message = FeatureActionReturnType.Failure.ToString() })
            );
        }

        [NonAction]
        private IActionResult ReturnAction(AboutActionReturnType aboutActionReturnType, SuccessCallBack success, OversizedCallBack oversized, FailureCallBack failure)
        {
            if (aboutActionReturnType == AboutActionReturnType.Failure)
            {
                return failure();
            }

            if (aboutActionReturnType == AboutActionReturnType.Oversized)
            {
                return oversized();
            }

            return success();
        }

        private delegate IActionResult SuccessCallBack();
        private delegate IActionResult FailureCallBack();
        private delegate IActionResult OversizedCallBack();
    }
}
