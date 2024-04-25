using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.FaqVm
{
    public class FaqCreateVm
    {
        [Required]
        public string? Question { get; set; }

        [Required]
        public string? Answer { get; set; }
    }
}
