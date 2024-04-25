using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.CategoryVm
{
    public class CategoryCreateVm
    {
        [Required(ErrorMessage = "Category Name Required")]
		[Display(Name = "Name For Category")]
        public string? CategoryName { get; set; }
    }
}
