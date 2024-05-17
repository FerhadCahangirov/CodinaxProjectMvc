using CodinaxProjectMvc.DataAccess.Models.Common;
using CodinaxProjectMvc.Enums;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Lecture : BaseEntity
    {
        public string? Title { get; set; }

        public Module Module { get; set; }

        public IEnumerable<LectureFile> LectureFiles { get; set; }

        public Lecture() : base()
        {
            Module = new Module();
            LectureFiles = new List<LectureFile>();
        }
    }
}
