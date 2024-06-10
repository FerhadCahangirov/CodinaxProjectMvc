using CodinaxProjectMvc.DataAccess.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Progress : BaseEntity
    {
        public Student Student { get; set; }

        [ForeignKey("videoId")]
        public LectureFile LectureFile { get; set; }

        public decimal Percentage { get; set; }

        public Progress()
        {
            Student = new Student();
            LectureFile = new LectureFile();
        }

        public Progress(Student student, LectureFile video, decimal percentage)
        {
            Student = student;
            LectureFile = video;
            Percentage = percentage;
        }
    }
}
