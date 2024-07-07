using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.FaqVm
{
    public class FaqCreateVm
    {
        [Required]
        public string? Question { get; set; }

        [Display(Name = "Translate question to Russian language")]
        public string? QuestionRu { get; set; }
        [Display(Name = "Translate question to Turkish language")]
        public string? QuestionTr { get; set; }

        [Required]
        public string? Answer { get; set; }

        [Display(Name = "Translate answer to Russian language")]
        public string? AnswerRu { get; set; }
        [Display(Name = "Translate answer to Turkish language")]
        public string? AnswerTr { get; set; }
    }
}
