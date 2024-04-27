using System.ComponentModel.DataAnnotations;
using CodinaxProjectMvc.DataAccess.Models;

namespace CodinaxProjectMvc.ViewModel.CourseVm
{
    public class CourseCreateVm
    {
        [Display(Name = "Course Category")]
        [Required(ErrorMessage = "Course Category is required")]
        public string? CourseCategory { get; set; }

        [Display(Name = "Template")]
        [Required(ErrorMessage = "Template is required")]
        public string? Template { get; set; }

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
