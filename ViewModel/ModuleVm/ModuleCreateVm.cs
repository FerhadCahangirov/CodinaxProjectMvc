using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.ModuleVm
{
    public class ModuleCreateVm
    {
        [Required]
        public Guid CourseId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "Title should be minimum 3 characters and a maximum of 250 characters")]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        public ModuleCreateVm(Guid courseId)
        {
            CourseId = courseId;
            Title = string.Empty;
        }

        public ModuleCreateVm()
        {
            CourseId = Guid.NewGuid();
            Title= string.Empty;
        }
    }
}
