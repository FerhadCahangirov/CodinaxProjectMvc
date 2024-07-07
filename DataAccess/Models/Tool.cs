using CodinaxProjectMvc.DataAccess.Models.Common;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Tool : FileEntity
    {
        public string? Content { get; set; }
        public string? ContentTr { get; set; }
        public string? ContentRu { get; set; }
        public Template Template { get; set; }

        public Tool()
        {
            Template = new Template();
        }
    }
}
