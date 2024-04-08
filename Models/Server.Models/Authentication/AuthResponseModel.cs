

namespace Server.Models
{
    public class AuthResponseModel
    {
        public string Token                 { get; set; }
        public string Name                  { get; set; }
        public string UserId                { get; set; }
        public string Message               { get; set; }
        public bool EmailStatus             { get; set; }
        public string CompanyStatus         { get; set; }
        public string ProfileStatus         { get; set; }
        public bool   paid                  { get; set; }

    }
}
