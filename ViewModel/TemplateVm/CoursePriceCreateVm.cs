using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.TemplateVm
{
    public class CoursePriceCreateVm
    {
        [Required(ErrorMessage = "CourseId is required.")]
        public Guid TemplateId { get; set; }


        [Required(ErrorMessage = "Title is required.")]
        [StringLength(225, ErrorMessage = "Title cannot be longer than 225 characters.")]
        public string Title
        {
            get; set;
        }

        public IEnumerable<CoursePriceInfoVm> CoursePriceInfos { get; set; }
    }

    public class CoursePriceInfoVm
    {

        [Required(ErrorMessage = "Price info content is reqired.")]
        [StringLength(225, ErrorMessage = "Price info content cannot be longer than 225 characters.")]
        public string Content { get; set; }
    }
}
