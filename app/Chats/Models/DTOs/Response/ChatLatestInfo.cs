using Microsoft.Build.Framework;

namespace ValuedInBE.Chats.Models.DTOs.Response
{
    public class ChatInfo
    {
        [Required]
        public long Id { get; set; }
        public string ChatName { get; set; } = string.Empty;
        [Required]
        public ICollection<string> ParticipatingUsers { get; set; } = null!;
        [Required]
        public DateTime LastMessage { get; set; }
        [Required]
        public string LastMessageContent { get; set; } = null!;
        public bool Unread { get; set; } = false;
    }
}