
using System.ComponentModel.DataAnnotations;

namespace ValuedInBE.Models.Entities.Messaging
{
    public class ChatMessage
    {
        public ChatMessage(long iD, long chatID, string firstName, string secondName, string message, DateTime created)
        {
            ID = iD;
            ChatID = chatID;
            FirstName = firstName;
            SecondName = secondName;
            Message = message;
            Created = created;
        }
        public ChatMessage()
        {
        }

        [Key]
        public long ID { get; set; }

        public long ChatID { get; set; }

        public string FirstName { get; set; }
        public string SecondName { get; set; }

        public string Message { get; set; }

        public DateTime Created { get; set; }
    }
}
