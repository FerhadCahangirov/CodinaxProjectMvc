using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.AboutVm
{
    public class AboutCreateVm
    {
        [Required(ErrorMessage = "About Content Required")]
        [Display(Name = "Content For About Us")]
        public string? Content { get; set; }
    }
}
