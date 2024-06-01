using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.ViewModel.ModuleVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Policy = PolicyConstants.InstructorPolicy)]
    public class ModulesController : Controller
    {
        private readonly IModuleService _moduleService;

        public ModulesController(IModuleService moduleService)
        {
            _moduleService = moduleService;
        }

        public async Task<IActionResult> Index(Guid id)
            => View(await _moduleService.GetModuleByIdAsync(id));

        public IActionResult Create(Guid id)
            => View(new ModuleCreateVm(id));

        [HttpPost]
        public async Task<IActionResult> Create(ModuleCreateVm moduleCreateVm)
        {
            bool result = await _moduleService.CreateModuleAsync(moduleCreateVm);

            if (!result)
            {
                return View(moduleCreateVm);
            }

            TempData["ModuleCreatedSuccess"] = true;
            return Redirect($"/Instructor/Courses/Single/{moduleCreateVm.CourseId}");
        }

        public async Task<IActionResult> Update(Guid id)
            => View(await _moduleService.GetModuleUpdateDataAsync(id));


        [HttpPost]
        public async Task<IActionResult> Update(ModuleUpdateVm moduleUpdateVm)
        {
            bool result = await _moduleService.UpdateModuleAsync(moduleUpdateVm);

            if (!result)
            {
                return View(moduleUpdateVm);
            }

            TempData["ModuleUpdatedSuccess"] = true;
            return View(moduleUpdateVm);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
            => new JsonResult(new { success = await _moduleService.DeleteModuleAsync(id)});
    }
}
