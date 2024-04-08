

namespace Server.Models
{
    public class CaseCommentModel
    {
        public Guid Id               { get; set; }= Guid.NewGuid();
        public string Text           { get; set; }
        public DateTime CreatedAt    { get; set; } = DateTime.UtcNow;
        public Guid     CaseId       { get; set; }
        public string   UserId       { get; set; } 
        
    }
}
