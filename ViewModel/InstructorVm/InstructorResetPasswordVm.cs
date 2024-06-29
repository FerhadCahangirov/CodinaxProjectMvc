using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.InstructorVm
{
    public class InstructorResetPasswordVm
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password), ErrorMessage = "Passwords Do Not Match.")]
        public string? ConfirmPassword { get; set; }
    }
}
