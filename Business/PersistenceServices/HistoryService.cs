using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel.HistoryVm;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Business.PersistenceServices
{
    public class HistoryService : IHistoryService
    {
        private readonly IReadRepository<Student> _studentReadRepository;
        private readonly IActionContextAccessor _actionContextAccessor;

        private readonly IReadRepository<LectureFile> _lectureFileReadRepository;

        private readonly IReadRepository<History> _historyReadRepository;
        private readonly IWriteRepository<History> _historyWriteRepository;

        public HistoryService(
            IReadRepository<Student> studentReadRepository,
            IActionContextAccessor actionContextAccessor,
            IReadRepository<History> historyReadRepository,
            IWriteRepository<History> historyWriteRepository,
            IReadRepository<LectureFile> lectureFileReadRepository)
        {
            _studentReadRepository = studentReadRepository;
            _actionContextAccessor = actionContextAccessor;
            _historyReadRepository = historyReadRepository;
            _historyWriteRepository = historyWriteRepository;
            _lectureFileReadRepository = lectureFileReadRepository;
        }

        public async Task AddOrUpdateHistoryAsync(Guid videoId)
        {
            History? history = await _historyReadRepository.Table
                .Include(x => x.Student)
                .Include(x => x.LectureFile).SingleOrDefaultAsync(x => x.LectureFile.Id == videoId && x.Student == ActiveStudent);

            LectureFile video = await _lectureFileReadRepository.Table.FirstAsync(x => x.Id == videoId);

            if (history == null)
            {
                history = new History(ActiveStudent, video);

                await _historyWriteRepository.AddAsync(history);
                await _historyWriteRepository.SaveAsync();
            }
            else
            {
                history.LectureFile = video;

                _historyWriteRepository.Update(history);
                await _historyWriteRepository.SaveAsync();
            }
        }

        public Task<List<History>> ListHistoriesAsync()
        {
            return Task.FromResult(
                ActiveStudent.Histories
                    .Where(HistoryQuery)
                    .OrderByDescending(x => x.UpdatedDate)
                    .ThenByDescending(x => x.CreatedDate)
                    .ToList()
            );
        }


        private static Func<History, bool> HistoryQuery
        {
            get
            {
                return x => !x.LectureFile.IsDeleted && !x.LectureFile.IsArchived &&
                            !x.LectureFile.Lecture.IsDeleted && !x.LectureFile.Lecture.IsArchived &&
                            !x.LectureFile.Lecture.Module.IsDeleted && !x.LectureFile.Lecture.Module.IsArchived &&
                            !x.LectureFile.Lecture.Module.Course.IsDeleted && !x.LectureFile.Lecture.Module.Course.IsArchived;
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
                    .Include(x => x.Histories)
                    .ThenInclude(x => x.LectureFile)
                    .ThenInclude(x => x.Progresses)
                    .FirstAsync(x => x.Email == _actionContextAccessor.ActionContext.HttpContext.User.Identity.Name).Result;
            }
        }
    }
}
