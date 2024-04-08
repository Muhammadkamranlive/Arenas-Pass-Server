namespace Server.Models
{
    public class ProfessionalLicenseModel
    {
        public Guid   Id               { get; set; }
        public string userId           { get; set; }
        public string LicenseName      { get; set; }
        public string LicenseType      { get; set; }
        public string StateOfLicense   { get; set; }
        public string LicenseNumber    { get; set; }
        public string Notes            { get; set; }
        public DateTime IssueDate      { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
