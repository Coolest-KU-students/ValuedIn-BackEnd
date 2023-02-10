
using ValuedInBE.Security.Users;

namespace ValuedInBE.Models.DTOs.Requests
{
    public class NewUser
    {
        public string Login { get; set; }
        public UserRole? Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public String Telephone { get; set; }

        public NewUser(string login, UserRole role, string firstName, string lastName, string password, string email, string telephone)
        {
            Login = login;
            Role = role;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            Email = email;
            Telephone = telephone;
        }
        public NewUser() //For request contructing
        {

        }
    }
}
