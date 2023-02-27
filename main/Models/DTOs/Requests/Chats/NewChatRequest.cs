namespace ValuedInBE.Models.DTOs.Requests.Chats
{
    public class NewChatRequest
    {
        public List<string> Participants { get; set; }
        public string MessageContent { get; set; }

    }
}
