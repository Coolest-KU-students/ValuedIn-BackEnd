namespace ValuedInBE.Models.DTOs.Responses.Chats
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
