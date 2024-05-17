using CodinaxProjectMvc.Enums;
using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.LectureVm
{
    public class LectureFileCreateVm
    {
        [Required(ErrorMessage = "Lecture Id is required.")]
        [Display(Name = "Lecture Id")]
        public Guid LectureId { get; set; }

        [Required(ErrorMessage = "Module Id is required.")]
        [Display(Name = "Module Id")]
        public Guid ModuleId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(250, ErrorMessage = "Title cannot be longer than 250 characters.")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "File")]
        public IFormFile? File { get; set; }

        [Required(ErrorMessage = "File Type is required.")]
        [Display(Name = "File Type")]
        public FileType FileType { get; set; }

        [Url(ErrorMessage = "Invalid URL.")]
        [Display(Name = "Url")]
        public string? Url { get; set; }

        public LectureFileCreateVm()
        {
            LectureId = Guid.NewGuid();
            Title = string.Empty;
        }

        public LectureFileCreateVm(Guid moduleId, Guid lectureId)
        {
            LectureId = lectureId;
            ModuleId = moduleId;
            Title = string.Empty;
        }
    }
}
