using ValuedInBE.Security.Users;

namespace ValuedInBE.Models.DTOs.Responses
{

    public class UserSystemInfo
    {
        public UserSystemInfo(string login, bool isExpired, DateTime? lastActive, UserRole role, string firstName, string lastName, string email, string telephone)
        {
            Login = login;
            IsExpired = isExpired;
            LastActive = lastActive;
            Role = role;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Telephone = telephone;
        }

        public string Login { get; set; }
        public bool IsExpired { get; set; }
        public DateTime? LastActive { get; set; }
        public UserRole Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
    }
}
