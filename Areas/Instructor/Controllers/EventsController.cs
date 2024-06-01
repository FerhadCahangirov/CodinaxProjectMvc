using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.Filters;
using CodinaxProjectMvc.ViewModel.EventVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Policy = PolicyConstants.InstructorPolicy)]
    public class EventsController : Controller
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [EventViewFilterFactory]
        public IActionResult Index()
            => View();

        [HttpGet]
        public async Task<JsonResult> ListEvents()
            => Json(await _eventService.ListEventsAsync());

        [HttpPost]
        public async Task<JsonResult> AddOrUpdateEvent(EventActionVm eventActionVm)
            => Json(new { success = await _eventService.EventAddOrUpdateAsync(eventActionVm) });

        [HttpDelete]
        public async Task<JsonResult> DeleteEvent(Guid id)
            => Json(new { success = await _eventService.DeleteEventAsync(id) });

    }
}
