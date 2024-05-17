using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.ViewModel.LectureVm
{
    public class LectureUpdateVm
    {
        [Required(ErrorMessage = "Lecture Id is required.")]
        [Display(Name = "Lecture Id")]
        public Guid LectureId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(250, ErrorMessage = "Title cannot be longer than 250 characters.")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        public LectureUpdateVm()
        {
            Title = string.Empty;
        }

        public LectureUpdateVm(Guid lectureId)
        {
            LectureId = lectureId;
            Title = string.Empty;
        }

        public LectureUpdateVm(Guid lectureId, string title)
        {
            LectureId = lectureId;
            Title = title;
        }
    }
}
