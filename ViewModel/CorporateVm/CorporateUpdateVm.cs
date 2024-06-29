namespace CodinaxProjectMvc.ViewModel.CorporateVm
{
    public class CorporateUpdateVm
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string WorkingCompany { get; set; }
        public string Occupation { get; set; }
        public string? AdditionalInfo { get; set; }

        public string? LogoName { get; set; }
        public string? LogoPath { get; set; }

        public bool IsApproved { get; set; }
        public bool Showcase { get; set; }

        public string BaseUrl { get; set; }
    }
}
