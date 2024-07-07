using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.TemplateVm
{
    public class CoursePriceCreateVm
    {
        [Required(ErrorMessage = "CourseId is required.")]
        public Guid TemplateId { get; set; }


        [Required(ErrorMessage = "Title is required.")]
        public string Title
        {
            get; set;
        }

        public string? TitleRu
        {
            get; set;
        }

        public string? TitleTr
        {
            get; set;
        }

        public IEnumerable<CoursePriceInfoVm>? CoursePriceInfos { get; set; }
    }

    public class CoursePriceInfoVm
    {

        [Required(ErrorMessage = "Price info content is reqired.")]
        public string Content { get; set; }
        public string? ContentRu { get; set; }
        public string? ContentTr { get; set; }
    }
}
