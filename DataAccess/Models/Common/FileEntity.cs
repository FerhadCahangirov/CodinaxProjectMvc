namespace CodinaxProjectMvc.DataAccess.Models.Common
{
    public class FileEntity : BaseEntity
    {
        public string? FileName { get; set; }
        public string? PathOrContainer { get; set; }
    }
}
