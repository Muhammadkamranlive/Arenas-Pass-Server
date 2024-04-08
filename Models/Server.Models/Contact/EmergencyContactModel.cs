
namespace Server.Models
{
    public class EmergencyContactModel
    {
        public Guid Id              { get; set; }= Guid.NewGuid();
        public string Name          { get; set; }
        public string Relationship  { get; set; }
        public string Phone         { get; set; }
        public string Email         { get; set; }
        public string Address       { get; set; }
        public string Type          { get; set; }
        public bool IsPrimary       { get; set; } 
        public string Notes         { get; set; } 
        public string userId        { get; set; }
        
    }
}
