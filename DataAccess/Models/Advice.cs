using CodinaxProjectMvc.DataAccess.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Advice : BaseEntity
    {
        [ForeignKey("MainCourseId")]
        public Course? MainCourse { get; set; }


        [ForeignKey("FirstAdvicedCourseId")]
        public Course? FirstAdvicedCourse { get; set; }


        [ForeignKey("SecondAdvicedCourseId")]
        public Course? SecondAdvicedCourse { get; set; }

        public Advice(Course mainCourse)
        {
            MainCourse = mainCourse;
        }

        public Advice(Course mainCourse, Course? firstAdvicedCourse, Course? secondAdvicedCourse)
        {
            MainCourse = mainCourse;
            FirstAdvicedCourse = firstAdvicedCourse;
            SecondAdvicedCourse = secondAdvicedCourse;
        }

        public Advice()
        {

        }
    }
}
