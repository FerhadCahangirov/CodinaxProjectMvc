using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.DataAccess.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodinaxProjectMvc.DataAccess
{
    public class Advice : BaseEntity
    {
        [ForeignKey("MainCourseId")]
        public Course? MainCourse { get; set; }


        [ForeignKey("FirstAdvicedCourseId")]
        public Course? FirstAdvicedCourse { get; set; }


        [ForeignKey("SecondAdvicedCourseId")]
        public Course? SecondAdvicedCourse { get; set; }
    }
}
