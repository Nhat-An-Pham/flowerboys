using System.ComponentModel.DataAnnotations;

namespace Models.UserDTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Please enter your username.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Please enter your password")]

        public string Password { get; set; }
    }
}
