using CodinaxProjectMvc.DataAccess.Models;

namespace CodinaxProjectMvc.ViewModel.CourseVm
{
    public class CourseStudentsAssignVm
    {
        public PaginationVm<IEnumerable<Student>>? StudentPagination { get; set; }
        public Course Course { get; set; }
    }
}
