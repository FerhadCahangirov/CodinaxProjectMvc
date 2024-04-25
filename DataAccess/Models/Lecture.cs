using CodinaxProjectMvc.DataAccess.Models.Common;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Lecture : BaseEntity
    {
        public string? Title { get; set; }
        public string? Description { get; set; }

        public int LectureNumber { get; set; }
        public bool IsPublic { get; set; }

        public Section Section { get; set; }
        public LectureVideo LectureVideo { get; set; }
        public IEnumerable<LectureFile> LectureFiles { get; set; }

        public Lecture()
        {
            Section = new Section();
            LectureVideo = new LectureVideo();
            LectureFiles = new List<LectureFile>();
        }
    }
}
