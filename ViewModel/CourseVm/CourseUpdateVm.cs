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

        [Display(Name = "Course Content")]
        [Required(ErrorMessage = "Course Content is required")]
        public string? CourseContent { get; set; }

        [Display(Name = "Course Header Description")]
        [Required(ErrorMessage = "Course Header Description is required")]
        public string? CourseHeaderDescription { get; set; }

        [Display(Name = "Course Description")]
        [Required(ErrorMessage = "Course Description is required")]
        public string? CourseDescription { get; set; }

        [Display(Name = "Course Fragment Video")]
        public IFormFile? CourseFragmentVideo { get; set; }

        public string? CourseFragmentVideoName { get; set; }

        public string? CourseFragmentVideoPathOrContainer { get; set; }

        [Display(Name = "Course Cover Image")]
        public IFormFile? CourseImage { get; set; }

        public string? CourseImageName { get; set; }

        public string? CourseImagePathOrContainer { get; set; }

        [Display(Name = "Course Heading")]
        [Required(ErrorMessage = "Course Heading is required")]
        public string? CourseHeading { get; set; }

        [Display(Name = "Course Level")]
        [Required(ErrorMessage = "Course Level is required")]
        public string? CourseLevel { get; set; }

        [Display(Name = "Course Location")]
        [Required(ErrorMessage = "Course Location is required")]
        public string? CourseLocation { get; set; }

        [Display(Name = "Course Properties")]
        [Required(ErrorMessage = "Course Properties is required")]
        public string? CourseProperties { get; set; }

        [Display(Name = "Course Starting Date")]
        [Required(ErrorMessage = "Course Starting Date is required")]
        [DataType(DataType.Date)]
        public DateTime? CourseStartingDate { get; set; }

        [Display(Name = "Course Finishing Date")]
        [DataType(DataType.Date)]
        public DateTime? CourseFinishingDate { get; set; }

        [Display(Name = "Course Title")]
        [Required(ErrorMessage = "Course Title is required")]
        public string? CourseTitle { get; set; }

        [Display(Name = "FutureJob Description")]
        [Required(ErrorMessage = "Future Job Description is required")]
        public string? FutureJobDescription { get; set; }

        [Display(Name = "Future Job Salary")]
        [Required(ErrorMessage = "Future Job Salary is required")]
        public string? FutureJobSalary { get; set; }

        [Display(Name = "Course Code")]
        [Required(ErrorMessage = "Course Code is required")]
        public string? CourseCode { get; set; }

        public string? BaseUrl { get; set; }

        public IEnumerable<FutureJobTitleVm>? FutureJobTitles { get; set; }
        public IEnumerable<TopicVm>? Topics { get; set; }

    }

}
