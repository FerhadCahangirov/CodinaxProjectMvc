using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel.StudentVm;
using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.StudentVm
{
    public class StudentAccountVm
    {
        [Required]
        public Guid Id { get; set; }

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

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number Required!")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Country Of Birth")]
        [Required(ErrorMessage = "Country Of Birth is required.")]
        public string? CountryOfBirth { get; set; }

        [Display(Name = "Country Of Residence")]
        [Required(ErrorMessage = "Country Of Residence is required.")]
        public string? CountryOfResidence { get; set; }

        [Display(Name = "Additional Notes")]
        public string? AdditionalNotes { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public string? BaseUrl { get; set; }

        public string? ProfileName { get; set; }
        public string? ProfilePathOrContainer { get; set; }

        [Display(Name = "Password")]
        public string? Password { get; set; }

        [Display(Name = "Confirm Password")]
        public string? ConfirmPassword { get; set; }


    }

    public static class StudentAccountVmConvertion
    {
        public static StudentAccountVm FromStudent_ToStudentAccountVm(this Student student)
        {
            return new StudentAccountVm
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                PhoneNumber = student.PhoneNumber,
                EmailAddress = student.Email,
                CountryOfResidence = student.CountryOfResidence,
                CountryOfBirth = student.CountryOfBirth,
                ProfileName = student.ProfileImageName,
                ProfilePathOrContainer = student.ProfileImagePathOrContainer,
                AdditionalNotes = student.AdditionalNotes,
                IsEmailConfirmed = student.EmailConfirmed
            };
        }
    }
}
