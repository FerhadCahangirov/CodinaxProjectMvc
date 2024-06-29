using System.ComponentModel.DataAnnotations;


namespace CodinaxProjectMvc.ViewModel.CorporateVm
{
    public class CorporateSendVm
    {
        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Working Company is required")]
        [Display(Name = "Working Company")]
        public string WorkingCompany { get; set; }

        [Required(ErrorMessage = "Occupation is required")]
        [Display(Name = "Occupation")]
        public string Occupation { get; set; }

        [Display(Name = "Additional Information")]
        public string? AdditionalInfo { get; set; }

        [Required]
        [Display(Name = "Is Approved")]
        public bool IsApproved { get; set; }

        [Required]
        [Display(Name = "Showcase")]
        public bool Showcase { get; set; }
    }
}
