using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class LoginRequest
    {
        [Required] public string Username { get; set; }
        [Required] public string Password { get; set; }
    }
}
