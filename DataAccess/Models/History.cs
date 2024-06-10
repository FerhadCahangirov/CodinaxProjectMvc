using CodinaxProjectMvc.DataAccess.Models.Common;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class History : BaseEntity
    {
        public Student Student { get; set; }
        public LectureFile LectureFile { get; set; }

        public History()
        {
            Student = new Student();
            LectureFile = new LectureFile();
        }

        public History(Student student, LectureFile lectureFile)
        {
            Student = student;
            LectureFile = lectureFile;
        }
    }
}
