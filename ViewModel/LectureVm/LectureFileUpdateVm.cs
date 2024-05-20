using CodinaxProjectMvc.Enums;
using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.LectureVm
{
    public class LectureFileUpdateVm
    {
        [Required(ErrorMessage = "Lecture File Id is required.")]
        [Display(Name = "Lecture File Id")]
        public Guid LectureFileId { get; set; }

        public string? Url { get; set; }
        public FileType? FileType  { get; set; }

        public string? FileName { get; set; }
        public string? FilePathOrContainer { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(250, ErrorMessage = "Title cannot be longer than 250 characters.")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "File")]
        public IFormFile? File { get; set; }

        public string? BaseUrl { get; set; }

        public LectureFileUpdateVm()
        {
           Title = string.Empty; 
        }

    }
}
