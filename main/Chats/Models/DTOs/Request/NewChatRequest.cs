namespace ValuedInBE.Chats.Models.DTOs.Request
{
    public class NewChatRequest
    {
        public List<string> Participants { get; set; }
        public string MessageContent { get; set; }

    }
}
