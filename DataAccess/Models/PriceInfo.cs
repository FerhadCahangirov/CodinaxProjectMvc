using CodinaxProjectMvc.DataAccess.Models.Common;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class PriceInfo : BaseEntity
    {
        public string? Content { get; set; }

        public Price Price { get; set; }

        public PriceInfo()
        {
            Price = new Price();
        }
    }
}
