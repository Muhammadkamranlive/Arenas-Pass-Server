

namespace Server.Domain
{
    public class Attachments
    {
        public Guid Id            { get; set; }
        public string userId      { get; set; }
        public string DocType     { get; set; }
        public string DocPlacementType { get; set; }
        public string DocName { get; set; }
        public string DocumentUrl { get; set; }
        public string? SenderId { get; set; }
        public string? ReceiverId { get; set; }
        public string? Status { get; set; }
        public string? state { get; set; }
        public DateTime? ExpDate { get; set; }
        public DateTime? IssueDate { get; set; }

        public int TenantId { get; set; } = 1;

    }
}
