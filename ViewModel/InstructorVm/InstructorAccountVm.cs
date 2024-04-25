using System.ComponentModel.DataAnnotations;
using CodinaxProjectMvc.DataAccess.Models;

namespace CodinaxProjectMvc.ViewModel.InstructorVm
{
    public class InstructorAccountVm
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

        public string? AdditionalNotes { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public string? BaseUrl { get; set; }

        public string? ProfileName { get; set; }
        public string? ProfilePathOrContainer { get; set; }
    }

    public static class InstructorAccountVmConvertion
    {
        public static InstructorAccountVm FromInstructor_ToInstructorAccountVm(this Instructor instructor)
        {
            return new InstructorAccountVm
            {
                Id = instructor.Id,
                FirstName = instructor.FirstName,
                LastName = instructor.LastName,
                PhoneNumber = instructor.PhoneNumber,
                EmailAddress = instructor.Email,
                CountryOfResidence = instructor.CountryOfResidence,
                CountryOfBirth = instructor.CountryOfBirth,
                Description = instructor.Description,
                AdditionalNotes = instructor.AdditionalNotes,
                IsEmailConfirmed = instructor.EmailConfirmed,
                ProfileName = instructor.ProfileImageName,
                ProfilePathOrContainer = instructor.ProfileImagePathOrContainer,
                Profession = instructor.Profession,
            };
        }
    }
}