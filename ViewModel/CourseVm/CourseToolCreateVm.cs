using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.CourseVm
{
    public class CourseToolCreateVm
    {
        [Required(ErrorMessage = "CourseId is required.")]
        public Guid CourseId { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        [StringLength(100, ErrorMessage = "Content cannot be longer than 100 characters.")]
        [Display(Name = "Course Content")]
        public string? Content { get; set; }

        [Required(ErrorMessage = "Icon is required.")]
        [DataType(DataType.Upload)]
        [Display(Name = "Course File")]
        public IFormFile Icon { get; set; }
    }
}
