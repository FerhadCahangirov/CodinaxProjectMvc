using CodinaxProjectMvc.DataAccess.Models;

namespace CodinaxProjectMvc.ViewModel.PlayerVm
{
    public class PlayerListVm
    {
        public IEnumerable<Lecture> Lectures { get; set; }

        public PlayerListVm()
        {
            Lectures = new List<Lecture>();
        }

        public PlayerListVm(List<Lecture> lectures)
        {
            Lectures = lectures;
        }
    }
}
