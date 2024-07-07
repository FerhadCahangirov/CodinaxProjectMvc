namespace CodinaxProjectMvc.ViewModel.TemplateVm
{
    public class CoursePriceUpdateVm
    {
        public Guid TemplateId { get; set; }
        public Guid PriceId { get; set; }
        public string Title { get; set; }
        public string? TitleRu { get; set; }
        public string? TitleTr { get; set; }

        public IEnumerable<CoursePriceInfoVm>? CoursePriceInfos { get; set; }
    }
}
