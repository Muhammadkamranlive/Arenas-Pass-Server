

namespace Server.Models
{
    public class ContactModel
    {
        public Guid Id { get; set; }= Guid.NewGuid();
        public string Name { get; set; }
        public string email { get; set; }
        public string description { get; set; }
    }

}
