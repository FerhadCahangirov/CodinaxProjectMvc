using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Policy = PolicyConstants.StudentPolicy)]
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

        public async Task<JsonResult> List()
            => Json(await _eventService.ListStudentEventsAsync());
    }
}
