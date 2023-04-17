using ValuedInBE.Chats.Models.Entities;

namespace ValuedInBETests.TestingConstants
{
    public static class ChatConstants
    {
        public const long messageID = 1;
        public const long chatID = 3;
        public static readonly DateTime created = DateTime.Now;
        public const string message = "This is a message";
        public const string senderUserId = "sender";

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
