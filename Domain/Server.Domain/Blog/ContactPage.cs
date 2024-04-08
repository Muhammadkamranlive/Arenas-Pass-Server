namespace Server.Domain
{
    public class ContactPage
    {
        public Guid Id            { get; set; }
        public string Name        { get; set; }
        public string email       { get; set; }
        public string description { get; set; }
        public int TenantId       { get; set; } = 1;
        public DateTime? createAt { get; set; } = DateTime.UtcNow;

    }

    public class BaseEntity
    {
        public int TenantId { get; private set; }
    }
}
