using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.AuthVm
{
    public class ResetPasswordVm
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password), ErrorMessage = "Passwords are not mathing.")]
        public string PasswordConfirm { get; set; }

        public ResetPasswordVm()
        {
            Email = string.Empty;
            Token = string.Empty;
            Password = string.Empty;
            PasswordConfirm = string.Empty;
        }

        public ResetPasswordVm(string email, string token)
        {
            Email = email;
            Token = token;
            Password = string.Empty;
            PasswordConfirm = string.Empty;
        }
    }
}
