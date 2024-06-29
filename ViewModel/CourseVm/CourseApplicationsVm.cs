using CodinaxProjectMvc.DataAccess.Models;
using Microsoft.IdentityModel.Tokens;

namespace CodinaxProjectMvc.ViewModel.CourseVm
{
    public class CourseApplicationsVm
    {
        public IEnumerable<Application> Applications { get; set; }
        public Course Course { get; set; }

        public string BaseUrl { get; set; }

        public CourseApplicationsVm()
        {
            Applications = new List<Application>();
            Course = new Course();
            BaseUrl = string.Empty;
        }
    }
}
