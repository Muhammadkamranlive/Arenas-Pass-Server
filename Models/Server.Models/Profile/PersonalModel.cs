

namespace Server.Models
{
    public class PersonalModel
    {
        public Guid Id                                { get; set; }= Guid.NewGuid();
         // Personal Information
        public string UserId                          { get; set; }
        public string? SNumber { get; set; }
        public DateTime DOB                           { get; set; }
        public int Age                                { get; set; }
        public string Gender                          { get; set; }
        public string Race                            { get; set; }
        public string Ethnicity                       { get; set; }
        public string Nationality                     { get; set; }
        public string Photo                           { get; set; }

        // Contact Information
        public string HomePhone                       { get; set; }
        public string MobilePhone                     { get; set; }
        public string Carrier                         { get; set; }
        public string PersonalEmail                   { get; set; }
        public string BusinessEmail                   { get; set; }
      

        // Address Information
        public string Address                         { get; set; }
        public string City                            { get; set; }
        public string State                           { get; set; }
        public string ZipCode                         { get; set; }
        public string birthcountry                    { get; set; }
        public string Country                         { get; set; }

        // Additional Information
        public string BestTimeToContact               { get; set; }
        public string HowDidYouHearAboutUs            { get; set; }
        

        // Professional Information
       
        public string Specialty                        { get; set; }
        public string TypeOfEmployment                 { get; set; }
        public int YearsOfExperience                   { get; set; }
        public string ComputerChartingSystemExperience { get; set; }
        public string DesiredTravelArea                { get; set; }
        public string LocationPreference               { get; set; }

        public string? Position                        { get; set; }

        // Agreement
        public bool AcceptTermsAndConditions           { get; set; }
    }
}
