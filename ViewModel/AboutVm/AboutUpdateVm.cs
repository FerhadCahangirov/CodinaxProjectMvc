using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.AboutVm
{
    public class AboutUpdateVm
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "About Content Required")]
        [Display(Name = "Content For About Us")]
        public string? Content { get; set; }
    }
}
