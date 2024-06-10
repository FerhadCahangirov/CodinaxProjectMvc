using CodinaxProjectMvc.DataAccess.Models.Common;
using CodinaxProjectMvc.Enums;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class LectureFile : FileEntity
    {
        public string? Title { get; set; }

        public bool IsCompleted { get; set; }

        public FileType FileType { get; set; }

        public string? FileSize { get; set; }

        public string? Duration { get; set; }

        public string? Url { get; set; }

        public Lecture Lecture { get; set; }

        public IEnumerable<Bookmark> Bookmarks { get; set; }
        public IEnumerable<History> Histories { get; set; }
        public IEnumerable<Progress> Progresses { get; set; }

        public LectureFile()
        {
            Progresses = new List<Progress>();
            Lecture = new Lecture();
            Bookmarks = new List<Bookmark>();
            Histories = new List<History>();
        }
    }
}
