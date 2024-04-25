namespace CodinaxProjectMvc.ViewModel.CourseVm
{
    public class CourseToolUpdateVm
    {
        public Guid CourseId { get; set; }
        public Guid ToolId { get; set; }
        public string Content { get; set; }
        public IFormFile? Icon { get; set; }
        public string IconPathOrContainer { get; set; }
        public string IconName { get; set; }
    }
}
