namespace Server.Models
{
    public class UpdateUserModel
    {
        public string FirstName  { get; set; }
        public string LastName   { get; set; }
        public string? MiddleName { get; set; }
        public string Image      { get; set; }
        public string Email      { get; set; }
    }
}
