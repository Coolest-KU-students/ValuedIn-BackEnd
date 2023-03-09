
using System.ComponentModel.DataAnnotations;

namespace ValuedInBE.Models.Entities.Messaging
{
    public class ChatMessage : AuditCreatedBase
    {
        public ChatMessage()
        {
        }

        [Key]
        public long ID { get; set; }
        public long ChatID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
    }
}
