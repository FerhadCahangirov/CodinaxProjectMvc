using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.ViewModel.CategoryVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = PolicyConstants.AdminPolicy)]
    public class CategoryController : Controller
    {

        private const string _categoriesPartialConstant = "CategoriesPartial";
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        
        // There can be send parameters page and size when converting to pagination and use another constructor of PaginationVm

        [HttpPost]
        public async Task<IActionResult> CategoriesPartial(string? searchFilter)
        {
            var data = await _categoryService.GetCategoriesPaginationAsync(searchFilter);

            if (data.Items?.ToList().Count == 0)
            {
                ViewBag.Message = "Nothing Found";
            }

            return PartialView(viewName: _categoriesPartialConstant, data);
        }

        public async Task<IActionResult> Index()
        {
            var data = await _categoryService.GetCategoriesPaginationAsync();
            
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
        public async Task<IActionResult> Create(CategoryCreateVm vm)
        {
            bool result = await _categoryService.CreateAsync(vm);

            if (!result)
                return View(vm);

            TempData["Created"] = true;
            return Redirect("/Admin/Category/Index");
        }

        private async Task<bool> CheckIsCategoryNameExistsAsync(string name)
            => await _categoryService.CheckIsExistsAsync(name);

        public async Task<IActionResult> Update(Guid id)
        {
            CategoryUpdateVm vm = await _categoryService.GetCategoryUpdateDataAsync(id);

            TempData["Updated"] = false;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CategoryUpdateVm vm)
        {
            bool result = await _categoryService.UpdateAsync(vm);
            if (!result)
                return View(vm);
            TempData["Updated"] = true;
            return Redirect("/Admin/Category/Index");
        }

        [HttpPost]
        public async Task<IActionResult> Archive(Guid id)
        {
            bool result = await _categoryService.ArchiveAsync(id);
            return new JsonResult(new { success = result });
        }

        [HttpPost]
        public async Task<IActionResult> UnArchive(Guid id)
        {
            bool result = await _categoryService.UnArchiveAsync(id);
            return new JsonResult(new { success = result });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool result = await _categoryService.DeleteAsync(id);
            return new JsonResult(new { success = result });
        }

        [HttpPost]
        public async Task<IActionResult> Restore(Guid id)
        {
            bool result = await _categoryService.RestoreAsync(id);
            return new JsonResult(new { success = result });
        }
    }
}
