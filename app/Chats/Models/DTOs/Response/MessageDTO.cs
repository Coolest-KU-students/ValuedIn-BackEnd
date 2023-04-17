using System.ComponentModel.DataAnnotations;

namespace ValuedInBE.Chats.Models.DTOs.Response
{
    public class MessageDTO
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string SentByFirstName { get; set; } = string.Empty;
        [Required]
        public string SentByLastName { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
        [Required]
        public DateTime Sent { get; set; }
        public bool Unread { get; set; }
    }
}