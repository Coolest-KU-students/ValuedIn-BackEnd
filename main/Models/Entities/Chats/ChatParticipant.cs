using System.ComponentModel.DataAnnotations;
using ValuedInBE.Models.Users;

namespace ValuedInBE.Models.Entities.Messaging
{
    public class ChatParticipant
    {

        [MaxLength(128)]
        public string UserID { get; set; }
        public long ChatID { get; set; }
        public UserDetails User { get; set; }
        public Chat Chat { get; set; }
    }
}
