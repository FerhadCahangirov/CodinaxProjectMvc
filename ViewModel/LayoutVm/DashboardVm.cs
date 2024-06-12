using CodinaxProjectMvc.DataAccess.Models;

namespace CodinaxProjectMvc.ViewModel.LayoutVm
{
    public class DashboardVm
    {
        public IEnumerable<History>? LastWatchedVideos { get; set; }
        public IEnumerable<LectureFile>? FavouriteVideos { get; set; }
        public IEnumerable<LectureFile>? Contents { get; set; }
        public IEnumerable<Lecture>? Lectures { get; set; }
        public IEnumerable<Module>? Modules { get; set; }
        public IEnumerable<LectureFile>? LastActions { get; set; }
    }
}
