using CodinaxProjectMvc.DataAccess.Models;

namespace CodinaxProjectMvc.ViewModel.CourseVm
{
    public class CourseInstructorsAssignVm
    {
        public PaginationVm<IEnumerable<Instructor>>? InstructorPagination { get; set; }
        public Course Course { get; set; }
    }
}
