using CodinaxProjectMvc.DataAccess.Models.Common;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class About : BaseEntity
    {
        public string? Content { get; set; }
        public string? ContentRu { get; set; }
        public string? ContentTr { get; set; }
    }
}
