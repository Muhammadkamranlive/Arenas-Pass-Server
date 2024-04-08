

namespace Server.Domain
{
    public class GENERALTASK
    {
        public Guid     Id                     { get; set; }
        public string   Title                  { get; set; }
        public string   Description            { get; set; }
        public DateTime StartDate              { get; set; }
        public DateTime DueDate                { get; set; }
        public string   Type                   { get; set; }
        public string   UserId                 { get; set; }
        public string   Progress               { get; set; }
        public int TenantId                    { get; set; } = 1;

    }
}
