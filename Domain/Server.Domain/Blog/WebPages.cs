namespace Server.Domain
{
    public class WebPages
    {
        public Guid Id                 { get; set; }
        public string? Title           { get; set; }
        public string? Description     { get; set; }
        public string? MetaDescription { get; set; }
        public string? image           { get; set; }
        public DateTime CreatedAt      { get; set; }
        public string   MainCategory   { get; set; }
        public string   SubCategory    { get; set; }
        public int      TenantId       { get; set; } = 1;
        public WebPages()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }
}
