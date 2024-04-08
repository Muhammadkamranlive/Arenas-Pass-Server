

namespace Server.Domain
{
    public class CaseComment
    {
        public Guid Id                { get; set; }
        public string Text            { get; set; }
        public DateTime CreatedAt     { get; set; } = DateTime.UtcNow;
        public Guid CaseId            { get; set; }
        public string UserId          { get; set; }
        public int TenantId           { get; set; } = 1;

    }
}
