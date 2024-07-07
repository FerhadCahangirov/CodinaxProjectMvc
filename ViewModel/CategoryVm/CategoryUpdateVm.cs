using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.CategoryVm
{
    public class CategoryUpdateVm
    {
        [Required(ErrorMessage = "Category Id Required")]
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Category Name Required")]
        [Display(Name = "Name For Category")]
        public string? CategoryName { get; set; }

        [Display(Name = "Name For Category Russian")]
        public string? CategoryNameRu { get; set; }

        [Display(Name = "Name For Category Turkish")]
        public string? CategoryNameTr { get; set; }
    }
}
