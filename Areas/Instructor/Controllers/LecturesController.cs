using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel.FeatureVm;
using CodinaxProjectMvc.ViewModel.LectureVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Policy = PolicyConstants.InstructorPolicy)]
    public class LecturesController : Controller
    {
        private readonly ILectureService _lectureService;

        public LecturesController(ILectureService lectureService)
        {
            _lectureService = lectureService;
        }

        public IActionResult Create(Guid id)
            => View(new LectureCreateVm(id));

        [HttpPost]
        public async Task<IActionResult> Create(LectureCreateVm lectureCreateVm)
        {
            bool result = await _lectureService.CreateLectureAsync(lectureCreateVm);
            if (!result)
                return View(lectureCreateVm);
            TempData["LectureCreatedSuccess"] = true;
            return Redirect($"/Instructor/Modules/Index/{lectureCreateVm.ModuleId}");
        }

        public async Task<IActionResult> Update(Guid id)
            => View(await _lectureService.GetLectureUpdateDataAsync(id));

        [HttpPost]
        public async Task<IActionResult> Update(LectureUpdateVm lectureUpdateVm)
        {
            bool result = await _lectureService.UpdateLecturesAsync(lectureUpdateVm);
            if (!result)
                return View(lectureUpdateVm);
            TempData["LectureUpdatedSuccess"] = true;
            return View(lectureUpdateVm);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
            => new JsonResult(new { success = await _lectureService.DeleteLectureAsync(id) });

        [HttpGet("Instructor/[controller]/{action}/{moduleId}/{lectureId}")]
        public IActionResult AddFile(Guid moduleId, Guid lectureId)
            => View(new LectureFileCreateVm(moduleId, lectureId));

        [HttpPost]
        public async Task<IActionResult> AddFile(LectureFileCreateVm lectureFileCreateVm)
        {
            bool result = await _lectureService.AddLectureFileAsync(lectureFileCreateVm);
            if (!result)
                return View(lectureFileCreateVm);

            TempData["LectureFileCreatedSuccess"] = true;
            return Redirect($"/Instructor/Modules/Index/{lectureFileCreateVm.ModuleId}");
        }

        public async Task<IActionResult> UpdateFile(Guid id)
            => View(await _lectureService.GetLectureFileUpdateDataAsync(id));


        [HttpPost]
        public async Task<IActionResult> UpdateFile(LectureFileUpdateVm lectureFileUpdateVm)
        {
            bool result = await _lectureService.UpdateLectureFileAsync(lectureFileUpdateVm);
            if (!result)
                return View(lectureFileUpdateVm);
            TempData["LectureFileUpdatedSuccess"] = true;
            return View(lectureFileUpdateVm);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFile(Guid id)
            => new JsonResult(new { success = await _lectureService.DeleteLectureFileAsync(id) });

    }
}
