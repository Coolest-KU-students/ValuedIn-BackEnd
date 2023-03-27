using System.ComponentModel.DataAnnotations;
using ValuedInBE.Chats.Models.Entities;

namespace ValuedInBE.Chats.Models.Events
{
    public class NewMessageEvent
    {
        [Required]
        public IEnumerable<string> OtherParticipantIDs { get; set; }
        [Required]
        public ChatMessage ChatMessage { get; set; }
    }
}