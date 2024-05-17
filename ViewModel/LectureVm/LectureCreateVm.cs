using CodinaxProjectMvc.Enums;
using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.LectureVm
{
    public class LectureCreateVm
    {
        [Required(ErrorMessage = "Module Id is required.")]
        [Display(Name = "Module Id")]
        public Guid ModuleId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(250, ErrorMessage = "Title cannot be longer than 250 characters.")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        public LectureCreateVm()
        {
            Title = string.Empty;
        }

        public LectureCreateVm(Guid moduleId)
        {
            ModuleId = moduleId;
            Title = string.Empty;
        }

        public LectureCreateVm(Guid moduleId, string title)
        {
            ModuleId = moduleId;
            Title = title;
        }
    }
}
