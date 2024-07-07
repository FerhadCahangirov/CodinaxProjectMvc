using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.AdminVm
{
    public class AdminAccountVm
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string? EmailAddress { get; set; }

        [Display(Name = "Password")]
        public string? Password { get; set; }

        [Display(Name = "Confirm Password")]
        public string? ConfirmPassword { get; set; }
    }
}
