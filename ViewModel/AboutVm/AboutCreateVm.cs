using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.AboutVm
{
    public class AboutCreateVm
    {
        [Required(ErrorMessage = "About Content Required")]
        [Display(Name = "Content For About Us")]
        public string? Content { get; set; }

        [Display(Name = "Translate content to Russian language")]
        public string? ContentRu { get; set; }

        [Display(Name = "Translate content to Turkish language")]
        public string? ContentTr { get; set; }
    }
}
