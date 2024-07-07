using CodinaxProjectMvc.DataAccess.Models.Common;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Template : BaseEntity
    {
        public string? Heading { get; set; }
        public string? HeadingRu { get; set; }
        public string? HeadingTr { get; set; }
        public string? Description { get; set; }
        public string? DescriptionRu { get; set; }
        public string? DescriptionTr { get; set; }
        public string? HeadingDescription { get; set; }
        public string? HeadingDescriptionRu { get; set; }
        public string? HeadingDescriptionTr { get; set; }

        public string? Location { get; set; }
        public string? LocationTr { get; set; }
        public string? LocationRu { get; set; }

        public DateTime? StartingDate { get; set; }
        public DateTime? FinishingDate { get; set; }

        public string? Content { get; set; }
        public string? ContentRu { get; set; }
        public string? ContentTr { get; set; }

        public string? FutureJobDesc { get; set; }
        public string? FutureJobDescRu { get; set; }
        public string? FutureJobDescTr { get; set; }
        public string? FutureJobSalary { get; set; }
        public string? FutureJobSalaryRu { get; set; }
        public string? FutureJobSalaryTr { get; set; }

        public string? Properties { get; set; }
        public string? PropertiesRu { get; set; }
        public string? PropertiesTr { get; set; }

        public string? CourseFragmentVideoName { get; set; }

        public string? CourseFragmentVideoPathOrContainer { get; set; }

        public string? CourseImageName { get; set; }

        public string? CourseImagePathOrContainer { get; set; }


        public IEnumerable<Tool> Tools { get; set; }
        public IEnumerable<Topic> Topics { get; set; }
        public IEnumerable<FutureJobTitle> FutureJobTitles { get; set; }

        public IEnumerable<Price> Prices { get; set; }

        public IEnumerable<Course> Courses { get; set; }

        public Template()
        {
            Tools = new List<Tool>();
            Topics = new List<Topic>();
            FutureJobTitles = new List<FutureJobTitle>();
            Prices = new List<Price>();
            Courses = new List<Course>();
        }

    }
}
