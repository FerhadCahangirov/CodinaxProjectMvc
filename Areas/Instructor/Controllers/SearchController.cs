using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Policy = PolicyConstants.InstructorPolicy)]
    public class SearchController : Controller
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        public async Task<IActionResult> Index(string content)
            => PartialView(viewName: "SearchPartial", await _searchService.SearchAsync(content));

        public IActionResult SearchByCourse(string content)
            => View(_searchService.Search<Course>(content));

        public IActionResult SearchByModule(string content)
            => View(_searchService.Search<Module>(content));

        public IActionResult SearchByLecture(string content)
            => View(_searchService.Search<Lecture>(content));

        public IActionResult SearchByContent(string content)
            => View(_searchService.Search<LectureFile>(content));
    }
}
