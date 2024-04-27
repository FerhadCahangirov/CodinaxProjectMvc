using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.CourseVm
{
    public class CourseUpdateVm
    {
        [Required]
        public Guid Id { get; set; }    

        [Display(Name = "Course Category")]
        [Required(ErrorMessage = "Course Category is required")]
        public string? CourseCategory { get; set; }

       
        [Display(Name = "Course Level")]
        [Required(ErrorMessage = "Course Level is required")]
        public string? CourseLevel { get; set; }

      
        [Display(Name = "Course Title")]
        [Required(ErrorMessage = "Course Title is required")]
        public string? CourseTitle { get; set; }


        [Display(Name = "Course Code")]
        [Required(ErrorMessage = "Course Code is required")]
        public string? CourseCode { get; set; }

    }

}
