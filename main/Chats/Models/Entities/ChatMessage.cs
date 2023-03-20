
using System.ComponentModel.DataAnnotations;
using ValuedInBE.System.PersistenceLayer.Entities;

namespace ValuedInBE.Chats.Models.Entities
{
    public class ChatMessage : AuditCreatedBase
    {
        [Key]
        public long Id { get; set; }
        public long ChatId { get; set; }
        public string Message { get; set; }
    }
}