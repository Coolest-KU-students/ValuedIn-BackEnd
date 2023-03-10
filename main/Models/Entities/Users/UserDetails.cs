using System.ComponentModel.DataAnnotations;
using ValuedInBE.Models.Entities.Messaging;

namespace ValuedInBE.Models.Users
{
    public class UserDetails
    {
        public UserDetails() { }

        [Key]
        [MaxLength(128)]
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public ICollection<ChatParticipant> ChatParticipants { get; set; }
    }
}
