using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Policy = PolicyConstants.StudentPolicy)]
    public class BookmarksController : Controller
    {
        private readonly IBookmarkService _bookmarkService;

        public BookmarksController(IBookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
        }

        public async Task<IActionResult> Index()
            => View(await _bookmarkService.GetBookmarksAsync(HttpContext.User.Identity.Name));

        [HttpPost]
        public async Task<IActionResult> AddBookmark(Guid id, BookmarkType bookmarkType)
            => new JsonResult(new
            {
                success = await _bookmarkService.AddBookmarkAsync(
                HttpContext.User.Identity.Name,
                id,
                bookmarkType)
            });


        [HttpDelete]
        public async Task<IActionResult> RemoveBookmark(Guid id, BookmarkType bookmarkType)
            => new JsonResult(new
            {
                success = await _bookmarkService.RemoveBookmarkAsync(
                HttpContext.User.Identity.Name,
                id,
                bookmarkType)
            });

    }
}
