using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.FeatureVm
{
    public class FeatureCreateVm
    {
        [Display(Name = "Feature Title")]
        [Required(ErrorMessage = "Feature Title is required")]
        public string? Title { get; set; }

        [Display(Name = "Feature Content")]
        [Required(ErrorMessage = "Feature Content is required")]
        public string? Content { get; set; }

        [Display(Name = "Feature Icon")]
        [Required(ErrorMessage = "Feature Icon is required")]
        public IFormFile? Icon { get; set; }
    }
}
