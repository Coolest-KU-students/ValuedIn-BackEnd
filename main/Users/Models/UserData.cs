using ValuedInBE.System.Security.Users;

namespace ValuedInBE.Users.Models
{
    public class UserData
    {
        public string UserID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
