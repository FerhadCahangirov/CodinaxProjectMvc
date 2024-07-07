using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.CorporateVm
{
    public class CorporateUpdateVm
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Working Company")]
        public string WorkingCompany { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Occupation")]
        public string Occupation { get; set; }

        [StringLength(500)]
        [Display(Name = "Additional Information")]
        public string? AdditionalInfo { get; set; }

        public string? LogoName { get; set; }

        public string? LogoPath { get; set; }

        public bool IsApproved { get; set; }

        public bool Showcase { get; set; }

        public string? BaseUrl { get; set; }
    }
}
