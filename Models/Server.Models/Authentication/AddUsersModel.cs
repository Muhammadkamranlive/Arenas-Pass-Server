

namespace Server.Models
{
    public class AddUsersModel
    {
        public string FirstName          { get; set; }
        public string LastName           { get; set; }
        public string? MiddleName        { get; set; }
        public string  role              { get; set; }
        public string Email              { get; set; }
        public string Password           { get; set; }
        public bool isAdmin              { get; set; } = false;
        public bool isEmployee           { get; set; } = false;
        public string Designation        { get; set; }
    }
}
