using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class RegisterRequest
    {
        [Required] public string Username { get; set; }
        [Required] public string Password { get; set; }
        [Required] public int Role { get; set; }  // 0 = Admin, 1 = Customer
    }
}
