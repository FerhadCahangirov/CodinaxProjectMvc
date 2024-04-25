using CodinaxProjectMvc.DataAccess.Models.Common;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Faq : BaseEntity
    {
        public string? Question { get; set; }
        public string? Answer { get; set; }
    }
}
