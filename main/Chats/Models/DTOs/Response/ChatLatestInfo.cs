using Microsoft.Build.Framework;

namespace ValuedInBE.Chats.Models.DTOs.Response
{
    public class ChatInfo
    {
        [Required]
        public long Id { get; set; }
        public string ChatName { get; set; }
        [Required]
        public ICollection<string> ParticipatingUsers { get; set; }
        [Required]
        public DateTime LastMessage { get; set; }
        [Required]
        public string LastMessageContent { get; set; }
        public bool Unread { get; set; }
    }
}