using Microsoft.Build.Framework;

namespace ValuedInBE.Chats.Models.DTOs.Request
{
    public class NewMessage
    {
        [Required]
        public string Content { get; set; }
    }
}