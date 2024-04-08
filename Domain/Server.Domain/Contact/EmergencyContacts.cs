

namespace Server.Domain
{
    public class EmergencyContacts
    {
        public Guid Id              { get; set; }
        public string Name          { get; set; }
        public string Relationship  { get; set; }
        public string Phone         { get; set; }
        public string Email         { get; set; }
        public string Address       { get; set; }
        public string Type          { get; set; }
        public bool IsPrimary       { get; set; } 
        public string Notes         { get; set; } 
        public string userId        { get; set; }
        public int TenantId         { get; set; } = 1;
    }

 }   
