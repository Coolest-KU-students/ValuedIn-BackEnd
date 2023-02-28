using ValuedInBE.Security.Users;

namespace ValuedInBE.Models
{
    public class UserContext
    {
        public string Login { get; set; }
        public string UserID { get; set; }
        public UserRole Role { get; set; } 
    }
}
