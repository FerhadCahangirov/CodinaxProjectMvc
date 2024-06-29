using CodinaxProjectMvc.DataAccess.Models;

namespace CodinaxProjectMvc.ViewModel.PlayerVm
{
    public class PlayerSingleVm
    {
        public LectureFile CurrentVideo { get; internal set; }
        public string BaseUrl { get; internal set; }
        public TimeSpan? CurrentTime { get; set; }

        public PlayerSingleVm()
        {
            CurrentVideo = new LectureFile();
            BaseUrl = string.Empty;
        }
    }
}
