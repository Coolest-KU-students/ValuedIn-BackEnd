using ValuedInBE.Chats.Models.Entities;

namespace ValuedInBETests.TestingConstants
{
    internal class ChatConstants
    {
        public static long messageID = 1;
        public static long chatID = 3;
        public static DateTime created = DateTime.Now;
        public static string message = "This is a message";
        public static string senderUserId = "sender";

        public static ChatMessage ChatMessage
        {
            get
            {
                var chatMessage = new ChatMessage()
                {
                    Id = messageID,
                    ChatId = chatID,
                    CreatedOn = created,
                    Message = message,
                    CreatedBy = senderUserId
                };
                return chatMessage;
            }
        }
    }
}
