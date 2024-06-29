
using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.ApplicationVm
{
    public class ApplicationCreateVm
    {
        [Required(ErrorMessage = "Course Id is required")]
        public Guid CourseId { get; set; }

        [Required(ErrorMessage = "File is required")]
        [DataType(DataType.Upload)]
        [Display(Name = "Application Icon")]
        public IFormFile? File { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [Display(Name = "Application Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Url is required")]
        [Url(ErrorMessage = "Invalid Url")]
        [Display(Name = "Application URL")]
        public string Url { get; set; }

        public ApplicationCreateVm()
        {
            Title = string.Empty;
            Url = string.Empty;
        }

        public ApplicationCreateVm(Guid courseId)
        {
            CourseId = courseId;
            Title = string.Empty;
            Url = string.Empty;
        }
    }
}
