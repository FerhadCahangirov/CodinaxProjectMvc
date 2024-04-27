using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.Enums;

namespace CodinaxProjectMvc.ViewModel.CourseVm
{
    public class CourseSingleVm
    {
        public Course? Course { get; set; }

        public Template? Template { get; set; }
        public string? BaseUrl { get; set; }
    }


}
