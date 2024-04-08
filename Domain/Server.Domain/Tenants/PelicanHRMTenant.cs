namespace Server.Domain
{
    public class PelicanHRMTenant
    {
        public Guid Id                       { get; set; }
        public string CompanyName            { get; set; }
        public int CompanyId                 { get; set; }
        public string? PhoneNumber            { get; set; } 
        public string? AddressLine1           { get; set; } 
        public string? AddressLine2           { get; set; } 
        public string? City                   { get; set; } 
        public string? State                  { get; set; } 
        public string? Country                { get; set; } 
        public bool?   isMailingAddress       { get; set; } 
        public bool?   isPhysicalAddress      { get; set; } 
        public string? whosisCompany          { get; set; } 
        public int?    noDomesticEmployee     { get; set; }
        public int?    noInterEmployee        { get; set; }
        public int?    noDomesticContractor   { get; set; }
        public int?     noInterContractor      { get; set; }
        public string? EIN                    { get; set; }
        public string? BussinessType          { get; set; }
        public string? LegalName              { get; set; }
        public string? FillingFormIRS         { get; set; }
        public string? Industry               { get; set; }
        public string ProfileStatus          { get; set; }
        public string CompanyStatus          { get; set; }
        public DateTime CreatedAt            { get; set; }
        public PelicanHRMTenant()
        {
                CreatedAt = DateTime.Now;
        }




    }
}
