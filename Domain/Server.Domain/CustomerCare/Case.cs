namespace Server.Domain
{
    public class Case
    {
        public Guid Id                            { get; set; }
        public string Title                       { get; set; }
        public string Description                 { get; set; }
        public DateTime CreatedAt                 { get; set; } = DateTime.UtcNow;
        public string   Status                    { get; set; } = "Open";
        public string CustomerId                  { get; set; } 
        public string AgentId                     { get; set; }
        public int TenantId                       { get; set; } = 1;
    }

    
}