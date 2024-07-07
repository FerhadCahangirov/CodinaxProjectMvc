using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.AdminVm
{
    public class AdminResetPasswordVm
    {
        [Required]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password), ErrorMessage = "Passwords Do Not Match.")]
        public string? ConfirmPassword { get; set; }    
    }
}
