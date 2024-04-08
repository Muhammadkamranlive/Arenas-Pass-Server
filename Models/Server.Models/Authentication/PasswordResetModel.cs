namespace Server.Models
{
    public class PasswordResetModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string OTP { get; set; }
        public DateTime ExpireTime { get; set; }
    }
}
