using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.AuthVm
{
    public class LoginVm
    {
        [Required(ErrorMessage = "Email Address is required.")]
        [Display(Name = "Email Address")]
        public string? EmailAddress { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(255, ErrorMessage = "Password must be between 5 and 255 characters.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool IsRemember { get; set; } = true;
    }
}
