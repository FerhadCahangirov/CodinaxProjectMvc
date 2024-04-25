namespace CodinaxProjectMvc.DataAccess.Models.Common
{

    public class BaseEntity : IBaseGenericEntity
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsArchived { get; set; }
    }
}
