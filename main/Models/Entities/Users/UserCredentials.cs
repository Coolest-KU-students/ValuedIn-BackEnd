
using System.ComponentModel.DataAnnotations;
using ValuedInBE.Security.Users;

namespace ValuedInBE.Models.Users
{
    public class UserCredentials
    {
        public UserCredentials() { }

        public UserCredentials(string login, string password, bool isExpired, DateTime? lastActive, UserRole role, UserDetails userDetails)
        {
            Login = login;
            Password = password;
            IsExpired = isExpired;
            LastActive = lastActive;
            Role = role;
            UserDetails = userDetails;
        }

        [Key]
        [MaxLength(128)]
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsExpired { get; set; }
        public DateTime? LastActive { get; set; }
        public UserRole Role { get; set; }
        public UserDetails UserDetails { get; set; }
    }
}
