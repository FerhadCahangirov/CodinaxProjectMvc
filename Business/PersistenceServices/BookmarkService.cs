using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.Enums;
using CodinaxProjectMvc.ViewModel.BookmarkVm;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Business.PersistenceServices
{
    public class BookmarkService : IBookmarkService
    {
        private readonly IReadRepository<Bookmark> _bookmarkReadRepository;
        private readonly IWriteRepository<Bookmark> _bookmarkWriteRepository;
        private readonly IReadRepository<Student> _studentReadRepository;
        private readonly IReadRepository<Module> _moduleReadRepository;
        private readonly IReadRepository<Lecture> _lectureReadRepository;
        private readonly IReadRepository<LectureFile> _lectureFileReadRepository;

        private readonly IHistoryService _historyService;

        public BookmarkService(
            IReadRepository<Bookmark> bookmarkReadRepository,
            IWriteRepository<Bookmark> bookmarkWriteRepository,
            IReadRepository<Student> studentReadRepository,
            IReadRepository<Module> moduleReadRepository,
            IReadRepository<Lecture> lectureReadRepository,
            IReadRepository<LectureFile> lectureFileReadRepository,
            IHistoryService historyService)
        {
            _bookmarkReadRepository = bookmarkReadRepository;
            _bookmarkWriteRepository = bookmarkWriteRepository;
            _studentReadRepository = studentReadRepository;
            _moduleReadRepository = moduleReadRepository;
            _lectureReadRepository = lectureReadRepository;
            _lectureFileReadRepository = lectureFileReadRepository;
            _historyService = historyService;
        }

        public async Task<bool> AddBookmarkAsync(string email, Guid id, BookmarkType bookmarkType)
        {
            Student? student = await _studentReadRepository.GetSingleAsync(x => x.Email == email);

            if (student == null) return false;

            switch (bookmarkType)
            {
                case BookmarkType.Module:
                    Module? module = await _moduleReadRepository.GetSingleAsync(x => x.Id == id);
                    if (module == null) return false;

                    Bookmark? bookmark_module = await _bookmarkReadRepository.Table
                        .Include(x => x.Student)
                        .Include(x => x.Module)
                        .FirstOrDefaultAsync(x => x.Module == module && x.Student == student);

                    if (bookmark_module == null)
                    {
                        bookmark_module = new Bookmark(student, module);
                        await _bookmarkWriteRepository.AddAsync(bookmark_module);
                        await _bookmarkWriteRepository.SaveAsync();
                    }

                    return true;
                case BookmarkType.Video:
                    LectureFile? lectureFile_video = await _lectureFileReadRepository.GetSingleAsync(x => x.Id == id);
                    if (lectureFile_video == null) return false;

                    Bookmark? bookmark_video = await _bookmarkReadRepository.Table
                        .Include(x => x.Student)
                        .Include(x => x.LectureFile)
                        .FirstOrDefaultAsync(x => x.LectureFile == lectureFile_video && x.Student == student);

                    if (bookmark_video == null)
                    {
                        bookmark_video = new Bookmark(student, lectureFile_video, bookmarkType);

                        await _bookmarkWriteRepository.AddAsync(bookmark_video);
                        await _bookmarkWriteRepository.SaveAsync();
                    }

                    return true;
                case BookmarkType.Lecture:
                    Lecture? lecture = await _lectureReadRepository.GetSingleAsync(x => x.Id == id);
                    if (lecture == null) return false;

                    Bookmark? bookmark_lecture = await _bookmarkReadRepository.Table
                        .Include(x => x.Student)
                        .Include(x => x.Lecture)
                        .FirstOrDefaultAsync(x => x.Lecture == lecture && x.Student == student);

                    if (bookmark_lecture == null)
                    {
                        bookmark_lecture = new Bookmark(student, lecture);

                        await _bookmarkWriteRepository.AddAsync(bookmark_lecture);
                        await _bookmarkWriteRepository.SaveAsync();
                    }

                    return true;
                case BookmarkType.Content:
                    LectureFile? lectureFile_content = await _lectureFileReadRepository.GetSingleAsync(x => x.Id == id);
                    if (lectureFile_content == null) return false;

                    Bookmark? bookmark_content = await _bookmarkReadRepository.Table
                        .Include(x => x.Student)
                        .Include(x => x.LectureFile)
                        .FirstOrDefaultAsync(x => x.LectureFile == lectureFile_content && x.Student == student);

                    if (bookmark_content == null)
                    {
                        bookmark_content = new Bookmark(student, lectureFile_content, bookmarkType);

                        await _bookmarkWriteRepository.AddAsync(bookmark_content);
                        await _bookmarkWriteRepository.SaveAsync();
                    }

                    return true;
                default:
                    return false;
            }
        }

        public async Task<ListBookmarksVm> GetBookmarksAsync(string email)
        {
            List<Bookmark>? bookmarks = await _bookmarkReadRepository.Table
                .Include(x => x.Student)
                .Include(x => x.LectureFile).ThenInclude(x => x.Lecture).ThenInclude(x => x.Module).ThenInclude(x => x.Course)
                .Include(x => x.Lecture).ThenInclude(x => x.Module).ThenInclude(x => x.Course)
                .Include(x => x.Module).ThenInclude(x => x.Course)
                .Where(x => x.Student.Email == email).ToListAsync();

            if (bookmarks == null) return new ListBookmarksVm();

            ListBookmarksVm vm = new ListBookmarksVm()
            {
                Modules = bookmarks
                .Where(x => x.Module != null).Select(x => x.Module)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate).ToList(),
                Lectures = bookmarks
                .Where(x => x.Lecture != null).Select(x => x.Lecture)
                .Where(x => !x.IsDeleted && !x.Module.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .ToList(),
                Contents = bookmarks
                    .Where(x => x.LectureFile != null && x.LectureFile.FileType != FileType.MP4)
                    .Select(x => x.LectureFile)
                    .Where(x => !x.IsDeleted && !x.Lecture.IsDeleted && !x.Lecture.Module.IsDeleted)
                    .OrderByDescending(x => x.CreatedDate)
                    .ToList(),
                Videos = bookmarks
                    .Where(x => x.LectureFile != null && x.LectureFile.FileType == FileType.MP4)
                    .Select(x => x.LectureFile)
                    .Where(x => !x.IsDeleted && !x.Lecture.IsDeleted && !x.Lecture.Module.IsDeleted)
                    .OrderByDescending(x => x.CreatedDate)
                    .ToList(),
                Histories = await _historyService.ListHistoriesAsync()
            };

            return vm;
        }

        public async Task<bool> RemoveBookmarkAsync(string email, Guid id, BookmarkType bookmarkType)
        {
            Student? student = await _studentReadRepository.GetSingleAsync(x => x.Email == email);

            if (student == null) return false;

            switch (bookmarkType)
            {
                case BookmarkType.Lecture:
                    Bookmark? bookmark_lecture = await _bookmarkReadRepository.Table
                        .Include(x => x.Student)
                        .Include(x => x.Lecture)
                        .FirstOrDefaultAsync(x => x.Student.Email == email && x.Lecture.Id == id);

                    if (bookmark_lecture != null)
                    {
                        _bookmarkWriteRepository.Remove(bookmark_lecture);
                        await _bookmarkWriteRepository.SaveAsync();
                    }

                    return true;
                case BookmarkType.Module:
                    Bookmark? bookmark_module = await _bookmarkReadRepository.Table
                        .Include(x => x.Student)
                        .Include(x => x.Module)
                        .FirstOrDefaultAsync(x => x.Student.Email == email && x.Module.Id == id);

                    if (bookmark_module != null)
                    {
                        _bookmarkWriteRepository.Remove(bookmark_module);
                        await _bookmarkWriteRepository.SaveAsync();
                    }

                    return true;
                case BookmarkType.Video:
                    Bookmark? bookmark_lectureFile = await _bookmarkReadRepository.Table
                        .Include(x => x.Student)
                        .Include(x => x.LectureFile)
                        .FirstOrDefaultAsync(x => x.LectureFile.Id == id && x.Student.Email == email);

                    if (bookmark_lectureFile != null)
                    {
                        _bookmarkWriteRepository.Remove(bookmark_lectureFile);
                        await _bookmarkWriteRepository.SaveAsync();
                    }

                    return true;
                case BookmarkType.Content:
                    Bookmark? bookmark_content = await _bookmarkReadRepository.Table
                        .Include(x => x.Student)
                        .Include(x => x.LectureFile)
                        .FirstOrDefaultAsync(x => x.LectureFile.Id == id && x.Student.Email == email);

                    if (bookmark_content != null)
                    {
                        _bookmarkWriteRepository.Remove(bookmark_content);
                        await _bookmarkWriteRepository.SaveAsync();
                    }
                    return true;
                default: return false;
            }
        }
    }
}
