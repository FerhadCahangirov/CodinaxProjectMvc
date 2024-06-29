using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.Enums;
using CodinaxProjectMvc.ViewModel.LayoutVm;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Business.PersistenceServices
{
    public class DashboardService : IDashboardService
    {
        private readonly IReadRepository<Student> _studentReadRepository;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IHistoryService _historyService;

        public DashboardService(IReadRepository<Student> studentReadRepository, IActionContextAccessor actionContextAccessor, IHistoryService historyService)
        {
            _studentReadRepository = studentReadRepository;
            _actionContextAccessor = actionContextAccessor;
            _historyService = historyService;
        }

        public async Task<DashboardVm> ListDashboardAsync()
            =>
             new DashboardVm()
             {
                 LastWatchedVideos = await _historyService.ListHistoriesAsync(),
                 FavouriteVideos = ActiveStudent.Bookmarks
                    .Where(x => x.LectureFile != null && x.LectureFile.FileType == FileType.MP4)
                    .Select(x => x.LectureFile)
                    .Where(x => !x.IsDeleted && !x.Lecture.IsDeleted && !x.Lecture.Module.IsDeleted)
                    .OrderByDescending(x => x.CreatedDate)
                    .Take(8)
                    .ToList(),
                 Modules = ActiveStudent.Bookmarks
                    .Where(x => x.Module != null).Select(x => x.Module)
                    .Where(x => !x.IsDeleted)
                    .OrderByDescending(x => x.CreatedDate).ToList(),
                 Lectures = ActiveStudent.Bookmarks
                    .Where(x => x.Lecture != null).Select(x => x.Lecture)
                    .Where(x => !x.IsDeleted && !x.Module.IsDeleted)
                    .OrderByDescending(x => x.CreatedDate)
                    .Take(8)
                    .ToList(),
                 Contents = ActiveStudent.Bookmarks
                    .Where(x => x.LectureFile != null && x.LectureFile.FileType != FileType.MP4)
                    .Select(x => x.LectureFile)
                    .Where(x => !x.IsDeleted && !x.Lecture.IsDeleted && !x.Lecture.Module.IsDeleted && !x.Lecture.Module.Course.IsDeleted && !x.IsArchived && !x.Lecture.IsArchived && !x.Lecture.Module.IsArchived && !x.Lecture.Module.Course.IsArchived)
                    .OrderByDescending(x => x.CreatedDate)
                    .Take(8)
                    .ToList(),
                 LastActions = ActiveStudent.Courses
                    .Where(x => !x.IsDeleted && !x.IsArchived)
                    .SelectMany(x => x.Modules)
                    .Where(x => !x.IsDeleted && !x.IsArchived)
                    .SelectMany(x => x.Lectures)
                    .Where(x => !x.IsDeleted && !x.IsArchived)
                    .SelectMany(x => x.LectureFiles)
                    .Where(x => !x.IsDeleted && !x.IsArchived)
                    .OrderByDescending(x => x.CreatedDate).Take(10).ToList()
             };



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
                    .Include(x => x.Bookmarks).ThenInclude(x => x.LectureFile).ThenInclude(x => x.Lecture).ThenInclude(x => x.Module).ThenInclude(x => x.Course)
                    .Include(x => x.Bookmarks).ThenInclude(x => x.Lecture).ThenInclude(x => x.Module).ThenInclude(x => x.Course)
                    .Include(x => x.Bookmarks).ThenInclude(x => x.Module).ThenInclude(x => x.Course)
                    .Include(x => x.Courses).ThenInclude(x => x.Modules).ThenInclude(x => x.Lectures).ThenInclude(x => x.LectureFiles)
                    .AsSplitQuery().AsNoTracking()
                    .FirstAsync(x => x.Email == _actionContextAccessor.ActionContext.HttpContext.User.Identity.Name).Result;
            }
        }
    }
}
