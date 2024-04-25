namespace CodinaxProjectMvc.ViewModel.CourseVm
{
    public class CoursePriceUpdateVm
    {
        public Guid CourseId { get; set; }
        public Guid PriceId { get; set; }
        public string Title { get; set; }

        public IEnumerable<CoursePriceInfoVm> CoursePriceInfos { get; set; }
    }
}
