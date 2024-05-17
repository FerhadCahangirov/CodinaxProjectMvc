namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Content : Bookmark
    {
        public Lecture Lecture { get; set; }

        public Content()
        {
            Lecture = new Lecture();
        }
    }
}
