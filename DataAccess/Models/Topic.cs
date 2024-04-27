using CodinaxProjectMvc.DataAccess.Models.Common;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Topic : BaseEntity
    {
        public string? Content { get; set; }

        public Template Template { get; set; }
        public Topic()
        {
            Template = new Template();
        }
    }
}
