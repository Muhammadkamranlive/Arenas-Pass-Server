

namespace Server.Domain
{
    public class HRNotes
    {
        public Guid Id              { get; set; }
        public string UserId        { get; set; }
        public string HRId          { get; set; }
        public string Note          { get; set; }

        public int TenantId { get; set; } = 1;
    }
}