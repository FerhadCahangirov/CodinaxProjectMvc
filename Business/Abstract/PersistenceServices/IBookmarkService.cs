using CodinaxProjectMvc.Enums;
using CodinaxProjectMvc.ViewModel.BookmarkVm;

namespace CodinaxProjectMvc.Business.Abstract.PersistenceServices
{
    public interface IBookmarkService
    {
        Task<bool> AddBookmarkAsync(string email, Guid id, BookmarkType bookmarkType);

        Task<bool> RemoveBookmarkAsync(string email, Guid id, BookmarkType bookmarkType);

        Task<ListBookmarksVm> GetBookmarksAsync(string email);
    }
}
