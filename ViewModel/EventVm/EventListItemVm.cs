using CodinaxProjectMvc.DataAccess.Models;

namespace CodinaxProjectMvc.ViewModel.EventVm
{
    public class EventListItemVm
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string BackgroundColor { get; set; } //+
        public string TextColor { get; set; } //+
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsBelong { get; set; }

        public Guid? InstructorId { get; set; }

        public Guid CourseId { get; set; }
    }
}
