using System.ComponentModel.DataAnnotations;

namespace ValuedInBE.Chats.Models.DTOs.Response
{
    public class MessageDTO
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string SentByFirstName { get; set; }
        [Required]
        public string SentByLastName { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime Sent { get; set; }
        public bool Unread { get; set; }
    }
}