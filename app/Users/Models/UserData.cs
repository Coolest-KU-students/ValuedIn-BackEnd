using System.ComponentModel.DataAnnotations;
using ValuedInBE.System.Security.Users;

namespace ValuedInBE.Users.Models
{
    public class UserData
    {
        [Required]
        public string UserID { get; set; } = null!;
        [Required]
        public string Login { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public UserRole Role { get; set; }
    }
}
