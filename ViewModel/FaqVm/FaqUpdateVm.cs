using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.FaqVm
{
    public class FaqUpdateVm
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string? Question { get; set; }

        [Required]
        public string? Answer { get; set; }
    }
}
