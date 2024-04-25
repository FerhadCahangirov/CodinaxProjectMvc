using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.FeatureVm
{
    public class FeatureUpdateVm
    {
        [Required]
        public Guid Id { get; set; }

        [Display(Name = "Feature Title")]
        [Required(ErrorMessage = "Feature Title is required")]
        public string? Title { get; set; }

        [Display(Name = "Feature Content")]
        [Required(ErrorMessage = "Feature Content is required")]
        public string? Content { get; set; }

        public IFormFile? Icon { get; set; }

        public string? IconPathOrContainer { get; set; }
        public string? IconName { get; set; }
    }
}
