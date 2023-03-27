using System.ComponentModel.DataAnnotations;
using ValuedInBE.System.PersistenceLayer.Entities;
using ValuedInBE.Users.Models.Entities;

namespace ValuedInBE.Chats.Models.Entities
{
    public class ChatParticipant : AuditCreatedUpdateBase
    {
        [MaxLength(128)]
        public string UserId { get; set; } = null!;
        [Required]
        public long ChatId { get; set; }
        public UserDetails? User { get; set; }
        public Chat? Chat { get; set; }
    }
}