using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel.TemplateVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = PolicyConstants.AdminPolicy)]
    public class TemplateController : Controller
    {
        private readonly ITemplateService _templateService;
        private readonly IReadRepository<Template> _templateReadRepository;

        public TemplateController(ITemplateService templateService, IReadRepository<Template> templateReadRepository)
        {
            _templateService = templateService;
            _templateReadRepository = templateReadRepository;
        }


        #region Managing Templates Controllers
        public async Task<IActionResult> Index()
            => View(await _templateService.GetTemplatesAsync());

        public IActionResult Create()
            => View();

        [HttpPost]
        public async Task<IActionResult> Create(TemplateCreateVm templateCreateVm)
        {
            bool result = await _templateService.CreateTemplateAsync(templateCreateVm);

            if (!result)
            {
                var errors = new List<string>();

                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    if (state?.Errors.Count > 0)
                    {
                        string error = state.Errors.FirstOrDefault().ErrorMessage;
                        errors.Add(error);
                    }
                }

                return new JsonResult(new { success = result, errors = errors });
            }
            return new JsonResult(new { success = result });
        }

        public IActionResult Update(Guid id)
            => View(id);

        public async Task<IActionResult> GetUpdateData(Guid id)
            => Ok(await _templateService.GetTemplateUpdateDataAsync(id));

        [HttpPut]
        public async Task<IActionResult> Update(TemplateUpdateVm templateUpdateVm)
        {
            bool result = await _templateService.UpdateTemplateAsync(templateUpdateVm);


            if (!result)
            {
                var errors = new List<string>();

                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    if (state?.Errors.Count > 0)
                    {
                        string error = state.Errors.FirstOrDefault().ErrorMessage;
                        errors.Add(error);
                    }
                }

                return new JsonResult(new { success = result, errors = errors });
            }
            return new JsonResult(new { success = result });
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
            => new JsonResult(new { success = await _templateService.DeleteTemplateAsync(id) } );

        [HttpDelete]
        public async Task<IActionResult> RemoveCourseFragmentVideo(Guid id)
            => new JsonResult(new { success = await _templateService.RemoveCourseFragmentVideoAsync(id) });

        #endregion

        #region Managing Tools Controllers

        public async Task<IActionResult> Tools(Guid id)
            => View(await _templateService.GetToolsAsync(id));

        public async Task<IActionResult> CreateTool(Guid id)
        {
            Template? template = await _templateReadRepository.GetSingleAsync(x => x.Id == id);
            if (template == null) return NotFound();

            CourseToolCreateVm courseCreateVm = new CourseToolCreateVm()
            {
                TemplateId = template.Id,
            };

            TempData["CourseToolCreated"] = false;
            return View(courseCreateVm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTool(CourseToolCreateVm courseToolCreateVm)
        {
            bool result = await _templateService.CreateToolAsync(courseToolCreateVm);

            if (!result)
                return View(courseToolCreateVm);

            TempData["CourseToolCreated"] = true;
            return Redirect($"/Admin/Template/{nameof(Tools)}/{courseToolCreateVm.TemplateId}");
        }

        public async Task<IActionResult> UpdateTool(Guid id)
        {
            CourseToolUpdateVm courseToolUpdateVm = await _templateService.GetUpdateToolDataAsync(id);

            TempData["CourseToolUpdated"] = false;
            return View(courseToolUpdateVm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTool(CourseToolUpdateVm courseToolUpdateVm)
        {
            bool result = await _templateService.UpdateToolAsync(courseToolUpdateVm);

            if (!result)
                return View(courseToolUpdateVm);

            TempData["CourseToolUpdated"] = true;
            return Redirect($"/Admin/Template/{nameof(Tools)}/{courseToolUpdateVm.TemplateId}");
        }

        [HttpPost]
        public async Task<IActionResult> ArchiveTool(Guid id)
        {
            bool result = await _templateService.ArchiveToolAsync(id);
            return new JsonResult(new { success = result });
        }

        [HttpPost]
        public async Task<IActionResult> UnArchiveTool(Guid id)
        {
            bool result = await _templateService.UnArchiveToolAsync(id);
            return new JsonResult(new { success = result });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTool(Guid id)
        {
            bool result = await _templateService.DeleteToolAsync(id);
            return new JsonResult(new { success = result });
        }

        [HttpPost]
        public async Task<IActionResult> RestoreTool(Guid id)
        {
            bool result = await _templateService.RestoreToolAsync(id);
            return new JsonResult(new { success = result });
        }

        #endregion

        #region Managing Prices Controllers

        public async Task<IActionResult> Prices(Guid id)
            => View(await _templateService.GetPricesAsync(id));

        public async Task<IActionResult> CreatePrice(Guid id)
        {
            Template? template = await _templateReadRepository.GetSingleAsync(x => x.Id == id);
            if (template == null) return NotFound();

            CoursePriceCreateVm courseCreateVm = new CoursePriceCreateVm()
            {
                TemplateId = template.Id,
            };

            TempData["CoursePriceCreated"] = false;
            return View(courseCreateVm);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePrice(CoursePriceCreateVm coursePriceCreateVm)
        {
            bool result = await _templateService.AddCoursePriceAsync(coursePriceCreateVm);

            if (!result)
                return View(coursePriceCreateVm);

            TempData["CoursePriceCreated"] = true;
            return Redirect($"/Admin/Template/{nameof(Prices)}/{coursePriceCreateVm.TemplateId}");
        }

        public async Task<IActionResult> UpdatePrice(Guid courseId, Guid priceId)
        {
            CoursePriceUpdateVm coursePriceUpdateVm = await _templateService.GetCoursePriceUpdateDataAsync(courseId, priceId);

            TempData["CoursePriceUpdated"] = false;
            return View(coursePriceUpdateVm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePrice(CoursePriceUpdateVm coursePriceUpdateVm)
        {
            bool result = await _templateService.UpdateCoursePriceAsync(coursePriceUpdateVm);

            if (!result)
                return View(coursePriceUpdateVm);

            TempData["CoursePriceUpdated"] = true;
            return Redirect($"/Admin/Template/{nameof(Prices)}/{coursePriceUpdateVm.TemplateId}");
        }

        [HttpPost]
        public async Task<IActionResult> ArchivePrice(Guid id)
        {
            bool result = await _templateService.ArchiveCoursePriceAsync(id);
            return new JsonResult(new { success = result });
        }

        [HttpPost]
        public async Task<IActionResult> UnArchivePrice(Guid id)
        {
            bool result = await _templateService.UnArchiveCoursePriceAsync(id);
            return new JsonResult(new { success = result });
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePrice(Guid id)
        {
            bool result = await _templateService.DeleteCoursePriceAsync(id);
            return new JsonResult(new { success = result });
        }

        [HttpPost]
        public async Task<IActionResult> RestorePrice(Guid id)
        {
            bool result = await _templateService.RestoreCoursePriceAsync(id);
            return new JsonResult(new { success = result });
        }

        #endregion


    }
}
