
namespace Server.Models
{
    public class DependentModel
    {
        public Guid Id              { get; set; } = Guid.NewGuid();
        public string userId        { get; set; }
        public string Name          { get; set; }
        public string Relationship  { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool HasSpecialNeeds { get; set; }
        public string Notes         { get; set; }
    }
}
