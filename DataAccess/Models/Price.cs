using CodinaxProjectMvc.DataAccess.Models.Common;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Price : BaseEntity
    {
        public string? Title { get; set; }

        public IEnumerable<PriceInfo> PriceInfos { get; set; }

        public Course Course { get; set; }

        public Price()
        {
            PriceInfos = new List<PriceInfo>();
            Course = new Course();
        }

    }
}
