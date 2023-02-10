using System.ComponentModel.DataAnnotations;

namespace ValuedInBE.Models.Users
{
    public class UserDetails
    {
        public UserDetails() { }
        public UserDetails(string login, string firstName, string lastName, string email, string telephone)
        {
            Login = login;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Telephone = telephone;
        }

        [Key]
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
    }
}
