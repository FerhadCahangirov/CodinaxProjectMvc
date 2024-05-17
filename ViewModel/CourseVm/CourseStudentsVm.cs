using CodinaxProjectMvc.DataAccess.Models;

namespace CodinaxProjectMvc.ViewModel.CourseVm
{
    public class CourseStudentsVm
    {
        public IEnumerable<Student>? Students { get; set; }
        public Course Course { get; set; }
        public string BaseUrl { get; set; }
    }

}
