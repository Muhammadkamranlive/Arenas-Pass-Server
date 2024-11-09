namespace Server.Domain
{
    public class ArenasTenants
    {
        public Guid Id                        { get; set; }
        public string CompanyName             { get; set; }
        public int    CompanyId               { get; set; }
        public string? PhoneNumber            { get; set; } 
        public string? AddressLine1           { get; set; } 
        public string? City                   { get; set; } 
        public string? State                  { get; set; } 
        public string? Country                { get; set; }
        public string? ZipCode                { get; set; }
        public string? isMailingAddress       { get; set; } 
        public string? isPhysicalAddress      { get; set; } 
        public string ProfileStatus           { get; set; }
        public string CompanyStatus           { get; set; }
        public DateTime CreatedAt             { get; set; }
        public ArenasTenants()
        {
                CreatedAt = DateTime.Now;
        }




    }

    public class TenantDetailModel
    {
        public string  Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Image { get; set; }
        public string CompanyName { get; set; }
        public int TenantId { get; set; }
        public string CompanyDesignation { get; set; }
        public string CompanyStatus { get; set; }
        public string ProfileStatus { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string IsMailingAddress { get; set; }
        public string IsPhysicalAddress { get; set; }
        public string CreatedAt { get; set; } 
    }

}
