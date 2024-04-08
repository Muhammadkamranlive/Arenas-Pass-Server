namespace Server.Models
{
    public class CaseModel
    {
        public Guid Id                            { get; set; }
        public string Title                       { get; set; }
        public string Description                 { get; set; }
        public DateTime CreatedAt                 { get; set; } = DateTime.UtcNow;
        public string   Status                    { get; set; } = "Open";
        public string CustomerId                  { get; set; } 
        public string AgentId                     { get; set; } 
      
        
    }
}
