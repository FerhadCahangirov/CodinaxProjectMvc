using CodinaxProjectMvc.DataAccess.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.AccountVm
{
    public class PasswordSetupVm
    {

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string Token { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }



    }
}
