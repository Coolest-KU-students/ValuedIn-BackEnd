using ValuedInBE.Security.Users;

namespace ValuedInBE.Models
{
    public class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
