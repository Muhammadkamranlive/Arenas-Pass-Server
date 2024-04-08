

namespace Server.Models
{
    public class JobExperienceModel
    {
        public Guid     Id                     { get; set; }
        public string   userId                 { get; set; }
        public string   PreviousCompany        { get; set; }
        public string   PreviousCompanyAddress { get; set; }
        public string   JobTitle               { get; set; }
        public string   Status                 { get; set; }
        public DateTime FromDate               { get; set; }
        public DateTime ToDate                 { get; set; }
        public string?   JobDescription         { get; set; }
        public string?   ReasonForLeaving       { get; set; }
    }
}
