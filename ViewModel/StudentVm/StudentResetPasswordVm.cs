using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.StudentVm
{
    public class StudentResetPasswordVm
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
