using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.TemplateVm
{
    public class CourseToolUpdateVm
    {
        public Guid TemplateId { get; set; }
        public Guid ToolId { get; set; }
        [Required]
        public string Content { get; set; }

        [Display(Name = "Translate Tool Content To Russian")]
        public string? ContentRu { get; set; }

        [Display(Name = "Translate Tool Content To Turkish")]
        public string? ContentTr { get; set; }
        public IFormFile? Icon { get; set; }
        public string? IconPathOrContainer { get; set; }
        public string? IconName { get; set; }
    }
}
