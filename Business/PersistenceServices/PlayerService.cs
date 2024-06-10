using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.DataAccess.Models.Identity;
using CodinaxProjectMvc.Enums;
using CodinaxProjectMvc.ViewModel.PlayerVm;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Business.PersistenceServices
{
    public class PlayerService : IPlayerService
    {
        private readonly IReadRepository<LectureFile> _lectureFileReadRepository;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IReadRepository<Student> _studentReadRepository;
        private readonly IConfiguration _configuration;
        private readonly IHistoryService _historyService;

        public PlayerService(
            IReadRepository<LectureFile> lectureFileReadRepository,
            IActionContextAccessor actionContextAccessor,
            IReadRepository<Student> studentReadRepository,
            IConfiguration configuration,
            IHistoryService historyService)
        {
            _lectureFileReadRepository = lectureFileReadRepository;
            _actionContextAccessor = actionContextAccessor;
            _studentReadRepository = studentReadRepository;
            _configuration = configuration;
            _historyService = historyService;
        }


        public Task<PlayerListVm> ListStudentVideosAsync(Guid id)
        {
            List<Lecture> lectures = ActiveStudent.Courses
                .First(x => !x.IsDeleted && !x.IsArchived && x.Modules.Any(_x => _x.Lectures.Any(__x => __x.LectureFiles.Any(___x => ___x.Id == id))))
                .Modules.First(x => !x.IsDeleted && !x.IsArchived && x.Lectures.Any(_x => _x.LectureFiles.Any(__x => __x.Id == id)))
                .Lectures.Where(x => !x.IsDeleted && !x.IsArchived && x.LectureFiles.Any(x => !x.IsDeleted && !x.IsArchived && x.FileType == FileType.MP4))
                .OrderBy(x => x.CreatedDate).ToList();

            return Task.FromResult(new PlayerListVm(lectures)); 
        }

        public async Task<PlayerSingleVm> GetActiveVideoAsync(Guid id)
        {
            LectureFile? current_video = await _lectureFileReadRepository.Table
                .Include(x => x.Lecture)
                .ThenInclude(x => x.Module)
                .ThenInclude(x => x.Course)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (current_video == null)
            {
                return new PlayerSingleVm()
                {
                    CurrentVideo = new LectureFile(),
                    BaseUrl = _configuration[ConfigurationStrings.AzureBaseUrl]
                };
            }

            await _historyService.AddOrUpdateHistoryAsync(current_video.Id);

            return new PlayerSingleVm()
            {
                CurrentVideo = current_video,
                BaseUrl = _configuration[ConfigurationStrings.AzureBaseUrl]
            };
        }


        private Student ActiveStudent
        {
            get
            {
                return _studentReadRepository.Table
                    .Include(x => x.Courses)
                    .ThenInclude(x => x.Modules)
                    .ThenInclude(x => x.Lectures)
                    .ThenInclude(x => x.LectureFiles)
                    .FirstAsync(x => x.Email == _actionContextAccessor.ActionContext.HttpContext.User.Identity.Name).Result;
            }
        }

    }
}
