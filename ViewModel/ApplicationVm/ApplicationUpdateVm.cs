using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.ApplicationVm
{
    public class ApplicationUpdateVm
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "CourseId is required")]
        public Guid CourseId { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Application File")]
        public IFormFile? File { get; set; }

        public string? FileName { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [Display(Name = "Application Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Url is required")]
        [Url(ErrorMessage = "Invalid Url")]
        [Display(Name = "Application URL ")]
        public string Url { get; set; }

        public ApplicationUpdateVm()
        {
            Title = string.Empty;
            Url = string.Empty;
        }
    }
}
