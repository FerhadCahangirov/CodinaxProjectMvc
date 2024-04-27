using CodinaxProjectMvc.DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.TemplateVm
{
    public class TemplateCreateVm
    {
        [Display(Name = "Course Content")]
        [Required(ErrorMessage = "Course Content is required")]
        public string? CourseContent { get; set; }

        [Display(Name = "Course Description")]
        [Required(ErrorMessage = "Course Description is required")]
        public string? CourseDescription { get; set; }

        [Display(Name = "Course Header Description")]
        [Required(ErrorMessage = "Course Header Description is required")]
        public string? CourseHeaderDescription { get; set; }

        [Display(Name = "Course Fragment Video")]
        public IFormFile? CourseFragmentVideo { get; set; }

        [Required(ErrorMessage = "Course Cover Image is required")]
        [Display(Name = "Course Cover Image")]
        public IFormFile? CourseImage { get; set; }

        [Display(Name = "Course Heading")]
        [Required(ErrorMessage = "Course Heading is required")]
        public string? CourseHeading { get; set; }

        [Display(Name = "Course Location")]
        [Required(ErrorMessage = "Course Location is required")]
        public string? CourseLocation { get; set; }

        [Display(Name = "Course Properties")]
        [Required(ErrorMessage = "Course Properties is required")]
        public string? CourseProperties { get; set; }

        [Display(Name = "Course Starting Date")]
        [Required(ErrorMessage = "Course Starting Date is required")]
        [DataType(DataType.Date)]
        public DateTime CourseStartingDate { get; set; }

        [Display(Name = "Course Finishing Date")]
        [DataType(DataType.Date)]
        public DateTime? CourseFinishingDate { get; set; }

        [Display(Name = "FutureJob Description")]
        [Required(ErrorMessage = "Future Job Description is required")]
        public string? FutureJobDescription { get; set; }

        [Display(Name = "Future Job Salary")]
        [Required(ErrorMessage = "Future Job Salary is required")]
        public string? FutureJobSalary { get; set; }

        public IEnumerable<FutureJobTitle>? FutureJobTitles { get; set; }

        public IEnumerable<Topic>? Topics { get; set; }
    }

    public sealed class FutureJobTitleVm
    {
        public string? Content { get; set; }
    }

    public sealed class TopicVm
    {
        public string? Content { get; set; }
    }
}

