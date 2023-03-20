using ValuedInBE.System.Security.Users;

namespace ValuedInBE.Users.Models
{
    public class UserContext
    {
        public string Login { get; set; }
        public string UserID { get; set; }
        public UserRole Role { get; set; }
    }
}
