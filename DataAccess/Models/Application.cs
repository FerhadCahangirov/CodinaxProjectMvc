using CodinaxProjectMvc.DataAccess.Models.Common;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Application : BaseEntity
    {
        public string IconName { get; set; }
        public string IconPath { get; set; }

        public string Title { get; set; }
        public string Url { get; set; }

        public Course Course { get; set; }

        public Application()
        {
            IconName = string.Empty;
            IconPath = string.Empty;
            Title = string.Empty;
            Url = string.Empty;
            Course = new Course();
        }
    }
}
