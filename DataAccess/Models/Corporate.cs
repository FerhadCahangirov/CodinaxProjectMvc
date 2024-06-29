using CodinaxProjectMvc.DataAccess.Models.Common;

namespace CodinaxProjectMvc.DataAccess.Models
{
    public class Corporate : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string WorkingCompany { get; set; }
        public string Occupation { get; set; }
        public string? AdditionalInfo { get; set; }

        public bool IsApproved { get; set; }
        public bool Showcase { get; set; }

        public string? LogoPath { get; set; }
        public string? LogoName { get; set; }
    }
}
