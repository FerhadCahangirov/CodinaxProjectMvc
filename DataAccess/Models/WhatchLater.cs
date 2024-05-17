namespace CodinaxProjectMvc.DataAccess.Models
{
    public class WhatchLater : Bookmark
    {
        public Lecture Lecture { get; set; }

        public WhatchLater()
        {
            Lecture = new Lecture();
        }
    }
}
