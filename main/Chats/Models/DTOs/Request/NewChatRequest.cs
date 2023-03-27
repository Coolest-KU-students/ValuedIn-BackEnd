using System.ComponentModel.DataAnnotations;

namespace ValuedInBE.Chats.Models.DTOs.Request
{
    public class NewChatRequest
    {
        [Required]
        public List<string> Participants { get; set; }

        [Required]
        public string MessageContent { get; set; }
    }
}