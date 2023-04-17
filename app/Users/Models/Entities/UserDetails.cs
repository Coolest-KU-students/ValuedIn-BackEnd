
using System.ComponentModel.DataAnnotations;
using ValuedInBE.Chats.Models.Entities;
using ValuedInBE.System.PersistenceLayer.Entities;

namespace ValuedInBE.Users.Models.Entities
{
    public class UserDetails : AuditCreatedUpdateBase
    {
        [Key]
        [MaxLength(128)]
        [Required]
        public string UserID { get; set; } = null!;
        [Required]
        public string FirstName { get; set; } = "";
        [Required]
        public string LastName { get; set; } = "";
        [Required]
        public string Email { get; set; } = "";
        [Required]
        public string Telephone { get; set; } = "";
        public IEnumerable<ChatParticipant>? ChatParticipants { get; set; }
    }
}