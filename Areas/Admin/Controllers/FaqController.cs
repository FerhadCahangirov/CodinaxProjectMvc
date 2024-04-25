using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.Context;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel;
using CodinaxProjectMvc.ViewModel.FaqVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = PolicyConstants.AdminPolicy)]
    public class FaqController : Controller
    {
        private readonly CodinaxDbContext _db;
        private const string _faqsPartialConstant = "FaqsPartial";
        private const int _size = 4;

        private readonly IFaqService _faqService;

        public FaqController(CodinaxDbContext db, IFaqService faqService)
        {

            _db = db;
            _faqService = faqService;
        }

        [HttpPost]
        public async Task<IActionResult> FaqsPartial(string? questionSearchFilter, string? answerSearchFilter)
        {
            var data = await _faqService.GetFaqsPaginationAsync(questionSearchFilter, answerSearchFilter);

            if (data.Items?.ToList().Count == 0)
            {
                ViewBag.Message = "Nothing Found";
            }

            return PartialView(viewName: _faqsPartialConstant, data);
        }

        public async Task<IActionResult> Index()
        {
            var data = await _faqService.GetFaqsPaginationAsync();

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
        public async Task<IActionResult> Create(FaqCreateVm vm)
        {
            bool result = await _faqService.CreateAsync(vm);

            if (!result)
                return View(vm);

            TempData["Created"] = true;
            return Redirect("/Admin/Faq/Index");
        }

        public async Task<IActionResult> Update(Guid id)
        {
            FaqUpdateVm vm = await _faqService.GetFaqUpdateDataAsync(id);

            TempData["Updated"] = false;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(FaqUpdateVm vm)
        {
            bool result = await _faqService.UpdateAsync(vm);

            if(!result)
                return View(vm);

            TempData["Updated"] = true;
            return Redirect("/Admin/Faq/Index");
        }

        [HttpPost]
        public async Task<IActionResult> Archive(Guid id)
        {
            bool result = await _faqService.ArchiveAsync(id);
            return new JsonResult(new { success = result });
        }

        [HttpPost]
        public async Task<IActionResult> UnArchive(Guid id)
        {
            bool result = await _faqService.UnArchiveAsync(id);
            return new JsonResult(new { success = result });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool result = await _faqService.DeleteAsync(id);
            return new JsonResult(new { success = result });
        }

        [HttpPost]
        public async Task<IActionResult> Restore(Guid id)
        {
            bool result = await _faqService.RestoreAsync(id);
            return new JsonResult(new { success = result });
        }
    }
}
