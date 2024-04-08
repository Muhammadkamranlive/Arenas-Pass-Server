namespace Server.Domain
{
    public class BlogPage
    {
        public Guid Id                    { get; set; }
        public string?   Title            { get; set; }
        public string?   Description      { get; set; }
        public string?   MetaDescription  { get; set; }
        public string?   image            { get; set; }
        public string    userId           { get; set; }
        public DateTime CreatedAt         { get; set; }
        public int TenantId               { get; set; } = 1;
        public BlogPage()
        {
            CreatedAt = DateTime.UtcNow;
        }
   }
}
