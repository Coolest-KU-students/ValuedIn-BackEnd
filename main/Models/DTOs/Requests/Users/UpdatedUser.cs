using ValuedInBE.Security.Users;

namespace ValuedInBE.Models.DTOs.Requests.Users
{
    public class UpdatedUser
    {
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
    }
}
