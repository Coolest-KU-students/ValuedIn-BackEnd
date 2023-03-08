using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValuedInBE.Models.Entities.Messaging;

namespace ValuedInBETests.TestingConstants
{
    internal class ChatConstants
    {
        public static long messageID = 1;
        public static long chatID = 3;
        public static DateTime created = DateTime.Now;
        public static string message = "This is a message";
        public static string senderUserId = "sender";

        public static ChatMessage ChatMessage { 
            get{
                var chatMessage = new ChatMessage()
                {
                    ID = messageID,
                    ChatID = chatID,
                    Created = created,
                    Message = message,
                    UserId = senderUserId
                };
                return chatMessage;
            } }
    }
}
