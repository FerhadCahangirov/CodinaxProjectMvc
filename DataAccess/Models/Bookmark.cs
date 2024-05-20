using CodinaxProjectMvc.DataAccess.Models.Common;
using CodinaxProjectMvc.Enums;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Bookmark : BaseEntity
    {
        public Student Student { get; set; }
        public Lecture? Lecture { get; set; }
        public Module? Module { get; set; }
        public LectureFile? LectureFile { get; set; }
        public BookmarkType BookmarkType { get; set; }

        public Bookmark()
        {
            Student = new Student();
        }

        public Bookmark(Student student, Module module)
        {
            Student = student;
            Module = module;
            BookmarkType = BookmarkType.Module;
        }

        public Bookmark(Student student, Lecture lecture)
        {
            Student = student;
            Lecture = lecture;
            BookmarkType = BookmarkType.Lecture;
        }

        public Bookmark(Student student, LectureFile lectureFile, BookmarkType bookmarkType)
        {
            Student = student;
            LectureFile = lectureFile;
            BookmarkType = bookmarkType;
        }
    }
}
