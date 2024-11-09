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

    public class UserRegisterModel
    {
        [Required(ErrorMessage = "First Name Required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name Required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email Required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Requird")]
        public string Password { get; set; }


    }
}
