
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ValuedInBE.System.PersistenceLayer.Entities;

namespace ValuedInBE.Chats.Models.Entities
{
    public class ChatMessage : AuditCreatedBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }
        [Required]
        public long ChatId { get; set; }
        [Required]
        public string Message { get; set; } = string.Empty;
    }
}