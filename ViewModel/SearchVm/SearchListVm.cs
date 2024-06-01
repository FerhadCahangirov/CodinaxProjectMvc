using CodinaxProjectMvc.DataAccess.Models;

namespace CodinaxProjectMvc.ViewModel.SearchVm
{
    public class SearchListVm
    {
        public IEnumerable<Course>? Courses { get; set; }
        public IEnumerable<Module>? Modules { get; set; }
        public IEnumerable<Lecture>? Lectures { get; set; }
        public IEnumerable<LectureFile>? Contents { get; set; }

        public string Query { get; set; } = string.Empty;
    }
}
