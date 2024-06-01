using CodinaxProjectMvc.DataAccess.Models.Common;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Event : BaseEntity
    {
        public string Title { get; set; } //+
        public string Url { get; set; } //+
        public string BackgroundColor { get; set; } //+
        public string TextColor { get; set; } //+
        public DateTime StartDate { get; set; } //+
        public DateTime EndDate { get; set; } //+

        public Instructor Instructor { get; set; }
        public Course Course { get; set; }
    
        public Event()
        {
            Title = string.Empty;
            Url = string.Empty;
            BackgroundColor = string.Empty;
            TextColor = string.Empty;
            Course = new Course();
            Instructor = new Instructor();
        }
    }
}
