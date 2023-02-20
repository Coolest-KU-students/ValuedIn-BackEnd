using ValuedInBE.Security.Users;

namespace ValuedInBE.Models.DTOs.Requests.Users
{
    public class NewUser
    {
        public string Login { get; set; }
        public UserRole? Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }

    }
}
