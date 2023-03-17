
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ValuedInBE.Models.Users;

namespace ValuedInBE.Models.Entities.Messaging
{
    public class ChatMessage : AuditCreatedBase
    {
        public ChatMessage()
        {
        }

        [Key]
        public long Id { get; set; }
        public long ChatId { get; set; }
        public string Message { get; set; }
    }
}
