using ValuedInBE.Models.Entities.Messaging;

namespace ValuedInBE.Models.Events
{
    public class NewMessageEvent
    {
        public List<string> OtherParticipantIDs { get; set; }
        public ChatMessage ChatMessage { get; set; }
    }
}
 