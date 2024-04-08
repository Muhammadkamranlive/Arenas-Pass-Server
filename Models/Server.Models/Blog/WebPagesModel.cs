namespace Server.Models
{
    public class WebPagesModel
    {
       
        public string? Title           { get; set; }
        public string? Description     { get; set; }
        public string? MetaDescription { get; set; }
        public string? image           { get; set; }
        public DateTime CreatedAt      { get; set; }
        public string MainCategory     { get; set; }
        public string SubCategory      { get; set; }
        public WebPagesModel()
        {
           
            CreatedAt = DateTime.UtcNow;
        }

    }
}
