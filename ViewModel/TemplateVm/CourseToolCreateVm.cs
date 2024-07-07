using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.TemplateVm
{
    public class CourseToolCreateVm
    {
        [Required(ErrorMessage = "Template Id is required.")]
        public Guid TemplateId { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        [Display(Name = "Course Content")]
        public string? Content { get; set; }

        [Display(Name = "Course Content Translate To Russian")]
        public string? ContentRu { get; set; }

        [Display(Name = "Course Content Translate To Turkish")]
        public string? ContentTr { get; set; }

        [Required(ErrorMessage = "Icon is required.")]
        [DataType(DataType.Upload)]
        [Display(Name = "Course File")]
        public IFormFile Icon { get; set; }
    }
}
