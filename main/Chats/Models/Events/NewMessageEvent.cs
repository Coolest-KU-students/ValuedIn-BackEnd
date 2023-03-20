using ValuedInBE.Chats.Models.Entities;

namespace ValuedInBE.Chats.Models.Events
{
    public class NewMessageEvent
    {
        public List<string> OtherParticipantIDs { get; set; }
        public ChatMessage ChatMessage { get; set; }
    }
}
