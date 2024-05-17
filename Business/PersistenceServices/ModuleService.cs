using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel.ModuleVm;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Business.PersistenceServices
{
    public class ModuleService : IModuleService
    {
        private readonly IReadRepository<Module> _moduleReadRepository;
        private readonly IReadRepository<Course> _courseReadRepository;
        private readonly IWriteRepository<Module> _moduleWriteRepository;

        private readonly IActionContextAccessor _actionContextAccessor;

        public ModuleService(IReadRepository<Module> moduleReadRepository, IWriteRepository<Module> moduleWriteRepository, IReadRepository<Course> courseReadRepository, IActionContextAccessor actionContextAccessor)
        {
            _moduleReadRepository = moduleReadRepository;
            _moduleWriteRepository = moduleWriteRepository;
            _courseReadRepository = courseReadRepository;
            _actionContextAccessor = actionContextAccessor;
        }

        public async Task<bool> CreateModuleAsync(ModuleCreateVm moduleCreateVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
            {
                return false;
            }

            Course? course = await _courseReadRepository.GetSingleAsync(x => x.Id == moduleCreateVm.CourseId);

            if (course == null || course.IsDeleted)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(ModuleCreateVm.CourseId), "This Course Not Found!");
                return false;
            }

            Module module = new Module(moduleCreateVm.Title, course);

            await _moduleWriteRepository.AddAsync(module);
            await _moduleWriteRepository.SaveAsync();

            return true;
        }

        public async Task<bool> UpdateModuleAsync(ModuleUpdateVm moduleUpdateVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
            {
                return false;
            }

            Module? module = await _moduleReadRepository.GetSingleAsync(x => x.Id == moduleUpdateVm.Id);


            if (module == null || module.IsDeleted)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(ModuleUpdateVm.Id), "Module Not Found");
                return false;
            }

            module.Title = moduleUpdateVm.Title;

            _moduleWriteRepository.Update(module);
            await _moduleWriteRepository.SaveAsync();

            return true;
        }

        public async Task<ModuleUpdateVm> GetModuleUpdateDataAsync(Guid id)
        {
            Module? module = await _moduleReadRepository.GetSingleAsync(x => x.Id == id);
            if(module == null)
                return new ModuleUpdateVm();

            return new(module.Id, module.Title);
        }

        public async Task<Module?> GetModuleByIdAsync(Guid id)
            => await _moduleReadRepository.Table
                .Include(x => x.Course)
                .Include(x => x.Lectures)
                .ThenInclude(x => x.LectureFiles)
            .FirstOrDefaultAsync(x => x.Id == id);
                
    }
}