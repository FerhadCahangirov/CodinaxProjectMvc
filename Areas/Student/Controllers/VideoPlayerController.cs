using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Areas.Student.Controllers
{
    [Area("Student")]
    public class VideoPlayerController : Controller
    {
        private readonly IPlayerService _playerService;

        public VideoPlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet("[area]/video-player/{id}")]
        public async Task<IActionResult> Index(Guid id)
            => View(await _playerService.GetActiveVideoAsync(id));

    }
}
