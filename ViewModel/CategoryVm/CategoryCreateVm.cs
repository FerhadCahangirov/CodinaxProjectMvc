using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.CategoryVm
{
    public class CategoryCreateVm
    {
        [Required(ErrorMessage = "Category Name Required")]
		[Display(Name = "Name For Category")]
        public string? CategoryName { get; set; }

        [Display(Name = "Name For Category Russian")]
        public string? CategoryNameRu { get; set; }

        [Display(Name = "Name For Category Turkish")]
        public string? CategoryNameTr { get; set; }
    }
}
