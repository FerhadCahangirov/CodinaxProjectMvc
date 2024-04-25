using CodinaxProjectMvc.DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.StudentVm
{
    public class StudentApplyVm
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

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number Required!")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
        //           ErrorMessage = "Entered phone format is not valid.")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Country Of Birth")]
        [Required(ErrorMessage = "Country Of Birth is required.")]
        public string? CountryOfBirth { get; set; }

        [Display(Name = "Country Of Residence")]
        [Required(ErrorMessage = "Country Of Residence is required.")]
        public string? CountryOfResidence { get; set; }

        [Display(Name = "Additional Notes")]
        public string? AdditionalNotes { get; set; }

        [Display(Name = "Discount Code Or Reference")]
        public string? DiscountOrReference { get; set; }

        [Display(Name = "Course")]
        [Required(ErrorMessage = "Course is required.")]
        public Guid? CourseId { get; set; }
    }

    public static class StudentApplyVmConvertion
    {
        public static Student FromStudentApplyVm_ToStudent(this StudentApplyVm studentApplyVm)
        {
            return new Student()
            {
                FirstName = studentApplyVm.FirstName,
                LastName = studentApplyVm.LastName,
                Email = studentApplyVm.EmailAddress,
                UserName = studentApplyVm.EmailAddress,
                PhoneNumber = studentApplyVm.PhoneNumber,
                CountryOfBirth = studentApplyVm.CountryOfBirth,
                CountryOfResidence = studentApplyVm.CountryOfResidence,
                AdditionalNotes = studentApplyVm.AdditionalNotes,
                DiscountOrReference = studentApplyVm.DiscountOrReference
            };
        }
    }
}
