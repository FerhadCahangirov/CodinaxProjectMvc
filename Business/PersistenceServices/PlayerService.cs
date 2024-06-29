using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.DataAccess.Models.Identity;
using CodinaxProjectMvc.Enums;
using CodinaxProjectMvc.ViewModel.PlayerVm;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.RegularExpressions;

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
                .Include(x => x.Progresses).ThenInclude(x => x.Student)
                .AsNoTracking()
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

            Progress? progress = current_video.Progresses.FirstOrDefault(x => x.Student.Email == ActiveStudent.Email);

            TimeSpan? currentTime = null;

            if (progress != null)
            {
                TimeSpan duration = FromStringToTimespan(current_video.Duration);
                currentTime = CalculatePercentageOfTimeSpan(duration, (double) progress.Percentage);
            }

            return new PlayerSingleVm()
            {
                CurrentTime = currentTime,
                CurrentVideo = current_video,
                BaseUrl = _configuration[ConfigurationStrings.AzureBaseUrl]
            };
        }

        private TimeSpan CalculatePercentageOfTimeSpan(TimeSpan timeSpan, double percentage)
        {
            double factor = percentage / 100.0;
            double totalSeconds = timeSpan.TotalSeconds * factor;
            return TimeSpan.FromSeconds(totalSeconds);
        }

        private TimeSpan FromStringToTimespan(string timeString)
        {
            var match = Regex.Match(timeString, @"(\d+)m\s*(\d+)s");
            int minutes = int.Parse(match.Groups[1].Value);
            int seconds = int.Parse(match.Groups[2].Value);
            return new TimeSpan(0, minutes, seconds); 
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
