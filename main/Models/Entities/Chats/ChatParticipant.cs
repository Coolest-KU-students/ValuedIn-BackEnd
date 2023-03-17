using System.ComponentModel.DataAnnotations;
using ValuedInBE.Models.Users;

namespace ValuedInBE.Models.Entities.Messaging
{
    public class ChatParticipant : AuditCreatedUpdateBase
    {
        [MaxLength(128)]
        public string UserId { get; set; }
        public long ChatId { get; set; }
        public UserDetails User { get; set; }
        public Chat Chat { get; set; }
    }
}
