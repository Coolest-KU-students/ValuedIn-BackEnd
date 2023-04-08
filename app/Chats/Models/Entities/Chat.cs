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
        [Required]
        public List<ChatMessage> Messages { get; set; } = new();
        [Required]
        public List<ChatParticipant> Participants { get; set; } = new();
    }
}