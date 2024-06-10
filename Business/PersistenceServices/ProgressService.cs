using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel.ProgressVm;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Business.PersistenceServices
{
    public class ProgressService : IProgressService
    {
        private readonly IReadRepository<Progress> _progressReadRepository;
        private readonly IWriteRepository<Progress> _progressWriteRepository;

        private readonly IReadRepository<LectureFile> _lectureFileReadRepository;

        private readonly IReadRepository<Student> _studentReadRepository;
        private readonly IActionContextAccessor _actionContextAccessor;


        public ProgressService(IReadRepository<Progress> progressReadRepository, IWriteRepository<Progress> writeProgressReadRepository, IReadRepository<Student> studentReadRepository, IActionContextAccessor actionContextAccessor, IReadRepository<LectureFile> lectureFileReadRepository)
        {
            _progressReadRepository = progressReadRepository;
            _progressWriteRepository = writeProgressReadRepository;
            _studentReadRepository = studentReadRepository;
            _actionContextAccessor = actionContextAccessor;
            _lectureFileReadRepository = lectureFileReadRepository;
        }

        public async Task AddOrUpdateProgressAsync(ProgressAddOrUpdateVm vm) 
        {
            Progress? progress = await _progressReadRepository.Table
                .Include(x => x.Student)
                .Include(x => x.LectureFile)
                .FirstOrDefaultAsync(x => x.LectureFile.Id == vm.VideoId && x.Student == ActiveStudent);

            LectureFile video = await _lectureFileReadRepository.Table
                .FirstAsync(x => x.Id == vm.VideoId);

            if (progress == null)
            {
                progress = new Progress(ActiveStudent, video, vm.Percentage);

                await _progressWriteRepository.AddAsync(progress);
                await _progressWriteRepository.SaveAsync();
            }
            else
            {
                if(vm.Percentage > progress.Percentage)
                {
                    progress.Percentage = vm.Percentage;

                    _progressWriteRepository.Update(progress);
                    await _progressWriteRepository.SaveAsync();
                }
            }
        }

        private Student ActiveStudent
        {
            get
            {
                return _studentReadRepository.Table
                    .Include(x => x.Histories)
                    .ThenInclude(x => x.LectureFile)
                    .ThenInclude(x => x.Lecture)
                    .ThenInclude(x => x.Module)
                    .ThenInclude(x => x.Course)
                    .FirstAsync(x => x.Email == _actionContextAccessor.ActionContext.HttpContext.User.Identity.Name).Result;
            }
        }
    }
}
