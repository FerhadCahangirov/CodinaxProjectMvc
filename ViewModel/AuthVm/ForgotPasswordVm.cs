using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.AuthVm
{
    public class ForgotPasswordVm
    {
        [Required]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        public ForgotPasswordVm()
        {
            EmailAddress = string.Empty;
        }
    }
}
