

namespace Server.Models
{
    public class ChatModel1
    {
        public Guid Id            { get; set; }
        public string Text        { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name        { get; set; }
        public string Email       { get; set; }
        public string Roles       { get; set; }
        public string Image       { get; set; }
    }
}
