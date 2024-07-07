using CodinaxProjectMvc.DataAccess.Models.Common;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Faq : BaseEntity
    {
        public string? Question { get; set; }
        public string? QuestionRu { get; set; }
        public string? QuestionTr { get; set; }
        public string? Answer { get; set; }
        public string? AnswerRu { get; set; }
        public string? AnswerTr { get; set; }
    }
}
