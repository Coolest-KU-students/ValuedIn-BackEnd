using ValuedInBE.Security.Users;

namespace ValuedInBE.Models.DTOs.Responses.Users
{
    public class UserSystemInfo
    {
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
