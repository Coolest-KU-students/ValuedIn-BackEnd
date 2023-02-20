using System.ComponentModel.DataAnnotations;

namespace ValuedInBE.Models.Entities.Messaging
{
    public class Chat
    {
        [Key]
        public long ID { get; set; }

        public List<ChatMessage> Messages { get; set; }
        public List<ChatParticipant> Participants { get; set; }
    }
}
