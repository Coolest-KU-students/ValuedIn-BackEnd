using ValuedInBE.System.Security.Users;

namespace ValuedInBE.Users.Models
{
    public class User
    {
        public string UserID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
