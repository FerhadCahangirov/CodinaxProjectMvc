using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.SubscribeVm
{
    public class SubscribeSendVm
    {
        [Required(ErrorMessage = "Mail Subject is required.")]
        [Display(Name = "Mail Subject")]
        public string? Subject { get; set; }

        [Required(ErrorMessage = "Mail Content is required.")]
        [Display(Name = "Mail Content")]
        public string? Content { get; set; }
    }
}
