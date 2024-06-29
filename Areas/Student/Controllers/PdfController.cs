using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using FFmpeg.AutoGen;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Policy = PolicyConstants.StudentPolicy)]
    public class PdfController : Controller
    {
        private readonly IReadRepository<LectureFile> _lectureFileReadRepository;
        private string _baseUrl;

        public PdfController(IReadRepository<LectureFile> lectureFileReadRepository, IConfiguration configuration)
        {
            _lectureFileReadRepository = lectureFileReadRepository;
            _baseUrl = configuration[ConfigurationStrings.AzureBaseUrl];
        }

        public async Task<IActionResult> View(Guid id)
        {
            var lectureFile = await _lectureFileReadRepository.GetSingleAsync(x => x.Id == id);

            var fileUrl = $"{_baseUrl}/{lectureFile.PathOrContainer}/{lectureFile.FileName}";

            if (!Uri.IsWellFormedUriString(fileUrl, UriKind.Absolute))
            {
                return BadRequest("Invalid file URL.");
            }

            return File(new HttpClient().GetStreamAsync(fileUrl).Result, "application/pdf");
        }

    }
}
