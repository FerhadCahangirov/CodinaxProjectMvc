using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using CodinaxProjectMvc.Constants;
using Microsoft.AspNetCore.Authorization;

namespace CodinaxProjectMvc.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Policy = PolicyConstants.StudentPolicy)]
    public class TxtController : Controller
    {
        private readonly IReadRepository<LectureFile> _lectureFileReadRepository;
        private readonly string _baseUrl;

        public TxtController(IReadRepository<LectureFile> lectureFileReadRepository, IConfiguration configuration)
        {
            _lectureFileReadRepository = lectureFileReadRepository;
            _baseUrl = configuration[ConfigurationStrings.AzureBaseUrl];
        }

        public async Task<IActionResult> Download(Guid id)
        {
            var lectureFile = await _lectureFileReadRepository.GetSingleAsync(x => x.Id == id);

            if (lectureFile == null)
            {
                return NotFound("File not found.");
            }

            var fileUrl = $"{_baseUrl}/{lectureFile.PathOrContainer}/{lectureFile.FileName}";

            if (!Uri.IsWellFormedUriString(fileUrl, UriKind.Absolute))
            {
                return BadRequest("Invalid file URL.");
            }

            using var httpClient = new HttpClient();
            var stream = await httpClient.GetStreamAsync(fileUrl);

            return File(stream, "text/plain", $"{lectureFile.FileName}");
        }
    }
}
