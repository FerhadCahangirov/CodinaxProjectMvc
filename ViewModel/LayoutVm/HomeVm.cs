using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel.CorporateVm;

namespace CodinaxProjectMvc.ViewModel.LayoutVm
{
    public class HomeVm
    {
        public IEnumerable<Course>? Courses { get; set; }
        public IEnumerable<Instructor>? Instructors { get; set; }
        public string BaseUrl { get; set; }
        public CorporateListVm Corporates { get; set; }
    }
}
