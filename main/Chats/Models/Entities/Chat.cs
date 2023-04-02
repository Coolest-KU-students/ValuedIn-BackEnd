using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ValuedInBE.System.PersistenceLayer.Entities;

namespace ValuedInBE.Chats.Models.Entities
{
    public class Chat : AuditCreatedBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }
        public List<ChatMessage> Messages { get; set; }
        public List<ChatParticipant> Participants { get; set; }
    }
}