using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.Context;
using CodinaxProjectMvc.DataAccess.Abstract.Storages;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel;
using CodinaxProjectMvc.ViewModel.FeatureVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CodinaxProjectMvc.Business.Abstract.PersistenceServices.IFeatureService;

namespace CodinaxProjectMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = PolicyConstants.AdminPolicy)]
    public class FeatureController : Controller
    {
        private readonly CodinaxDbContext _db;
        private readonly IAzureStorage _storage;
        private readonly IConfiguration _configuration;
        private const string _featuresPartialConstant = "FeaturesPartial";
        private const int _maxFeatureCount = 3;

        private readonly IFeatureService _featureService;

        public FeatureController(CodinaxDbContext db, IAzureStorage storage, IConfiguration configuration, IFeatureService featureService)
        {
            _db = db;
            _storage = storage;
            _configuration = configuration;
            _featureService = featureService;
        }

        [HttpPost]
        public async Task<IActionResult> FeaturesPartial(string? searchFilter)
        {
            var data = await _featureService.GetFeaturesAsync(searchFilter);

            if (data.Items?.ToList().Count == 0)
            {
                ViewBag.Message = "Nothing Found";
            }

            return PartialView(viewName: _featuresPartialConstant, data);
        }

        public async Task<IActionResult> Index()
        {
            var data = await _featureService.GetFeaturesAsync();

            if (data.Items?.ToList().Count == 0)
            {
                ViewBag.Message = "Nothing Found";
            }

            return View(data);
        }

        public IActionResult Create(Guid id)
        {
            TempData["Created"] = false;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FeatureCreateVm vm)
        {

            FeatureActionReturnType result = await _featureService.CreateFeatureAsync(vm);

            if (result == FeatureActionReturnType.Failure)
                return View(vm);
            
            if (result == FeatureActionReturnType.Oversized)
            {
                TempData["FeatureOversized"] = true;
                return Redirect("/Admin/Feature/Index");
            }


            TempData["Created"] = true;
            return Redirect("/Admin/Feature/Index");
        }

        public async Task<IActionResult> Update(Guid id)
        {
            FeatureUpdateVm vm = await _featureService.GetFeatureUpdateDataAsync(id);
            TempData["Updated"] = false;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(FeatureUpdateVm vm)
        {
            bool result = await _featureService.UpdateFeatureAsync(vm);

            if (!result)
                return View(vm);

            TempData["Updated"] = true;
            return Redirect("/Admin/Feature/Index");
        }
        [HttpPost]
        public async Task<IActionResult> Archive(Guid id)
        {
            bool result = await _featureService.ArchiveAsync(id);
            return new JsonResult(new { success = result });
        }

        [HttpPost]
        public async Task<IActionResult> UnArchive(Guid id)
        {
            FeatureActionReturnType result = await _featureService.UnarchiveAsync(id);

            return ReturnActionJson(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool result = await _featureService.DeleteAsync(id);
            return new JsonResult(new { success = result});
        }

        [HttpPost]
        public async Task<IActionResult> Restore(Guid id)
        {
            FeatureActionReturnType result = await _featureService.RestoreAsync(id);
            return ReturnActionJson(result);
        }

        [NonAction]
        private IActionResult ReturnActionJson(FeatureActionReturnType featureActionResult)
        {
            if (featureActionResult == FeatureActionReturnType.Oversized)
            {
                return new JsonResult(new { success = false, message = FeatureActionReturnType.Oversized.ToString() });
            }

            if (featureActionResult == FeatureActionReturnType.Failure)
            {
                return new JsonResult(new { success = false, message = FeatureActionReturnType.Failure.ToString() });
            }

            return new JsonResult(new { success = true });

        }
    }
}
