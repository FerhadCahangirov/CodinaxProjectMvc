using CodinaxProjectMvc.DataAccess.Models.Common;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Feature : BaseEntity
    {
        public string? Title { get; set; }
        public string? TitleRu { get; set; }
        public string? TitleTr { get; set; }

        public string? IconName { get; set; }
        public string? IconPathOrContainer { get; set; }
    }
}
