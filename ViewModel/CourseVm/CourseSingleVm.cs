using CodinaxProjectMvc.DataAccess.Models;

namespace CodinaxProjectMvc.ViewModel.CourseVm
{
    public class CourseSingleVm
    {
        public Course? Course { get; set; }

        public Course? FirstAdvicedCourse { get; set; }
        public Course? SecondAdvicedCourse { get; set; }    

        public Template? Template { get; set; }
        public string? BaseUrl { get; set; }
    }
}
