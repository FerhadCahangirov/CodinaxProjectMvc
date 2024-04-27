using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel.CourseVm;
using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.TemplateVm
{
    public class TemplateUpdateVm
    {
        [Required]
        public Guid Id { get; set; }

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

        [Display(Name = "FutureJob Description")]
        [Required(ErrorMessage = "Future Job Description is required")]
        public string? FutureJobDescription { get; set; }

        [Display(Name = "Future Job Salary")]
        [Required(ErrorMessage = "Future Job Salary is required")]
        public string? FutureJobSalary { get; set; }

        public string? BaseUrl { get; set; }

        public IEnumerable<FutureJobTitleVm>? FutureJobTitles { get; set; }
        public IEnumerable<TopicVm>? Topics { get; set; }

    }

    static partial class TemplateConversion
    {
        public static TemplateUpdateVm FromTemplate_ToTemplateUpdateVm(this Template course)
        {
            return new TemplateUpdateVm()
            {
                Id = course.Id,
                CourseContent = course.Content,
                CourseDescription = course.Description,
                CourseHeaderDescription = course.HeadingDescription,
                CourseFragmentVideo = null,
                CourseFragmentVideoName = course.CourseFragmentVideoName,
                CourseFragmentVideoPathOrContainer = course.CourseFragmentVideoPathOrContainer,
                CourseImage = null,
                CourseImageName = course.CourseImageName,
                CourseImagePathOrContainer = course.CourseImagePathOrContainer,
                CourseHeading = course.Heading,
                CourseLocation = course.Location,
                CourseProperties = course.Properties,
                CourseStartingDate = course.StartingDate.Value,
                CourseFinishingDate = course.FinishingDate.HasValue ? course.FinishingDate.Value : null,
                FutureJobDescription = course.FutureJobDesc,
                FutureJobSalary = course.FutureJobSalary,
                FutureJobTitles = course.FutureJobTitles.Select(x => new FutureJobTitleVm
                {
                    Content = x.Content,
                }),
                Topics = course.Topics.Select(x => new TopicVm
                {
                    Content = x.Content
                }),
            };
        }
    }
}
