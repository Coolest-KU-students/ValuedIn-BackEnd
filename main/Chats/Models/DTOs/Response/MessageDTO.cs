namespace ValuedInBE.Chats.Models.DTOs.Response
{
    public class MessageDTO
    {
        public long Id { get; set; }
        public string SentByFirstName { get; set; }
        public string SentByLastName { get; set; }
        public string Content { get; set; }
        public DateTime Sent { get; set; }
        public bool Unread { get; set; }
    }
}