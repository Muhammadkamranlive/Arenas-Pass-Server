using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class UserLoginModel
    {
        [Required(ErrorMessage ="Email Required")]
        public string Email   { get; set; }
        [Required(ErrorMessage ="Password is Requird")]
        public string Password { get; set; }
    }
}
