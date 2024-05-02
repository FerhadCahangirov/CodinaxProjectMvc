using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.TemplateVm
{
    public class CourseToolUpdateVm
    {
        public Guid TemplateId { get; set; }
        public Guid ToolId { get; set; }
        [Required]
        public string? Content { get; set; }
        public IFormFile? Icon { get; set; }
        public string? IconPathOrContainer { get; set; }
        public string? IconName { get; set; }
    }
}
