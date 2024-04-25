using CodinaxProjectMvc.DataAccess.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.InstructorVm
{
    public class InstructorApplyVm
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

        [Required(ErrorMessage = "Phone number is required.")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Country Of Birth")]
        [Required(ErrorMessage = "Country Of Birth is required.")]
        public string? CountryOfBirth { get; set; }

        [Display(Name = "Country Of Residence")]
        [Required(ErrorMessage = "Country Of Residence is required.")]
        public string? CountryOfResidence { get; set; }

        [Display(Name = "Profession")]
        [Required(ErrorMessage = "Profession is required.")]
        public string? Profession { get; set; }

        [Display(Name = "Additional Notes")]
        public string? AdditionalNotes { get; set; }

    }

    public static class InstructorApplyVmConvertion
    {
        public static Instructor FromInstructorApplyVm_ToInstructor(this InstructorApplyVm instructorApplyVm)
        {
            return new Instructor()
            {
                FirstName = instructorApplyVm.FirstName,
                LastName = instructorApplyVm.LastName,
                Email = instructorApplyVm.EmailAddress,
                PhoneNumber = instructorApplyVm.PhoneNumber,
                UserName = instructorApplyVm.EmailAddress,
                CountryOfBirth = instructorApplyVm.CountryOfBirth,
                CountryOfResidence = instructorApplyVm.CountryOfResidence,
                AdditionalNotes = instructorApplyVm.AdditionalNotes,
                Profession = instructorApplyVm.Profession,
            };
        }
    }
}