namespace Server.Models
{
    public class CandidateModel
    {
         public Guid  Id                        { get; set; }
         public string userId                   { get; set; }
         public string OrganizationName         { get; set; }
         public string BusinessType             { get; set; }
         public string Address                  { get; set; }
         public string Website                  { get; set; }
         public string Email                    { get; set; }
         public string Phone                    { get; set; }
         public string ContactName              { get; set; }
         public string Position                 { get; set; }
         public string BusinessEmail            { get; set; }
         public string PersonalEmail            { get; set; }
         public string MobilePhone              { get; set; }
         public string OfficePhone              { get; set; }
         public string Notes                    { get; set; }
    }
}
