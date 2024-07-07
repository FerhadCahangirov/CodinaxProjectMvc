using CodinaxProjectMvc.DataAccess.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class FutureJobTitle : BaseEntity
    {
        public string? Content { get; set; }
        public string? ContentRu { get; set; }
        public string? ContentTr { get; set; }

        [ForeignKey("TemplateId")]
        public Template Template { get; set; }

        public FutureJobTitle()
        {
            Template = new Template();
        }

    }
}
