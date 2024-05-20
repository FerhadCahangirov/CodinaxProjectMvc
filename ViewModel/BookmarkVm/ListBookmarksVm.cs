using CodinaxProjectMvc.DataAccess.Models;

namespace CodinaxProjectMvc.ViewModel.BookmarkVm
{
    public class ListBookmarksVm
    {
        public IEnumerable<Module>? Modules { get; set; }
        public IEnumerable<Lecture>? Lectures { get; set; }
        public IEnumerable<LectureFile>? Contents{ get; set; }
        public IEnumerable<LectureFile>? Videos { get; set; }
    }
}
