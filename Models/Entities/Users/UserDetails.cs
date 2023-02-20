using System.ComponentModel.DataAnnotations;
using ValuedInBE.Models.Entities.Messaging;

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
        [MaxLength(128)]
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }

        public ICollection<ChatParticipant> ChatParticipants { get; set; }

    }
}
