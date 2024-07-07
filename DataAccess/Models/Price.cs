using CodinaxProjectMvc.DataAccess.Models.Common;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Price : BaseEntity
    {
        public string? Title { get; set; }
        public string? TitleRu { get; set; }
        public string? TitleTr { get; set; }

        public IEnumerable<PriceInfo> PriceInfos { get; set; }

        public Template Template { get; set; }

        public Price()
        {
            PriceInfos = new List<PriceInfo>();
            Template = new Template();
        }

    }
}
