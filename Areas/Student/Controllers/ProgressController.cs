using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.ViewModel.ProgressVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Policy = PolicyConstants.StudentPolicy)]

    public class ProgressController : Controller
    {
        private readonly IProgressService _progressService;

        public ProgressController(IProgressService progressService)
        {
            _progressService = progressService;
        }

        public async Task<JsonResult> Index(ProgressAddOrUpdateVm vm)
        {
            await _progressService.AddOrUpdateProgressAsync(vm);
            return Json(new { success = true });
        }
    }
}
