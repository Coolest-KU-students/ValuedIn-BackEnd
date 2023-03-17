namespace ValuedInBE.Models.DTOs.Responses.Chats
{
    public class ChatInfo
    {
        public long Id { get; set; }
        public string ChatName { get; set; }
        public List<string> ParticipatingUsers { get; set; }
        public DateTimeOffset LastMessage { get; set; }
        public string LastMessageContent { get; set; }
        public bool Unread { get; set; }
    }
}
