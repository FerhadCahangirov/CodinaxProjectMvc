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
        public string? CourseContentRu { get; set; }
        public string? CourseContentTr { get; set; }

        [Display(Name = "Course Header Description")]
        [Required(ErrorMessage = "Course Header Description is required")]
        public string? CourseHeaderDescription { get; set; }
        public string? CourseHeaderDescriptionRu { get; set; }
        public string? CourseHeaderDescriptionTr { get; set; }

        [Display(Name = "Course Description")]
        [Required(ErrorMessage = "Course Description is required")]
        public string? CourseDescription { get; set; }
        public string? CourseDescriptionRu { get; set; }
        public string? CourseDescriptionTr { get; set; }

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
        public string? CourseHeadingRu { get; set; }
        public string? CourseHeadingTr { get; set; }

        [Display(Name = "Course Location")]
        [Required(ErrorMessage = "Course Location is required")]
        public string? CourseLocation { get; set; }
        public string? CourseLocationRu { get; set; }
        public string? CourseLocationTr { get; set; }

        [Display(Name = "Course Properties")]
        [Required(ErrorMessage = "Course Properties is required")]
        public string? CourseProperties { get; set; }
        public string? CoursePropertiesRu { get; set; }
        public string? CoursePropertiesTr { get; set; }

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
        public string? FutureJobDescriptionRu { get; set; }
        public string? FutureJobDescriptionTr { get; set; }

        [Display(Name = "Future Job Salary")]
        [Required(ErrorMessage = "Future Job Salary is required")]
        public string? FutureJobSalary { get; set; }
        public string? FutureJobSalaryRu { get; set; }
        public string? FutureJobSalaryTr { get; set; }

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
                CourseContentRu = course.ContentRu,
                CourseContentTr = course.ContentTr,
                CourseDescription = course.Description,
                CourseDescriptionRu = course.DescriptionRu,
                CourseDescriptionTr = course.DescriptionTr,
                CourseHeaderDescription = course.HeadingDescription,
                CourseHeaderDescriptionRu = course.HeadingDescriptionRu,
                CourseHeaderDescriptionTr = course.HeadingDescriptionTr,
                CourseFragmentVideo = null,
                CourseFragmentVideoName = course.CourseFragmentVideoName,
                CourseFragmentVideoPathOrContainer = course.CourseFragmentVideoPathOrContainer,
                CourseImage = null,
                CourseImageName = course.CourseImageName,
                CourseImagePathOrContainer = course.CourseImagePathOrContainer,
                CourseHeading = course.Heading,
                CourseHeadingRu = course.HeadingRu,
                CourseHeadingTr = course.HeadingTr,
                CourseLocation = course.Location,
                CourseLocationRu = course.LocationRu,
                CourseLocationTr = course.LocationTr,
                CourseProperties = course.Properties,
                CoursePropertiesRu = course.PropertiesRu,
                CoursePropertiesTr = course.PropertiesTr,
                CourseStartingDate = course.StartingDate.Value,
                CourseFinishingDate = course.FinishingDate.HasValue ? course.FinishingDate.Value : null,
                FutureJobDescription = course.FutureJobDesc,
                FutureJobDescriptionTr = course.FutureJobDescTr,
                FutureJobDescriptionRu = course.FutureJobDescRu,
                FutureJobSalary = course.FutureJobSalary,
                FutureJobSalaryRu = course.FutureJobSalaryRu,
                FutureJobSalaryTr = course.FutureJobSalaryTr,
                FutureJobTitles = course.FutureJobTitles.Select(x => new FutureJobTitleVm
                {
                    Content = x.Content,
                    ContentRu = x.ContentRu,
                    ContentTr = x.ContentTr
                }),
                Topics = course.Topics.Select(x => new TopicVm
                {
                    Content = x.Content,
                    ContentRu = x.ContentRu,
                    ContentTr = x.ContentTr
                }),
            };
        }
    }
}
