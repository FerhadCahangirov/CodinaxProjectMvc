using CodinaxProjectMvc.DataAccess.Models.Common;
using CodinaxProjectMvc.Enums;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class LectureFile : FileEntity
    {
        public Lecture Lecture { get; set; }
        public FileTypes FileType { get; set; }

        public LectureFile()
        {
            Lecture = new Lecture();
        }
    }
}
