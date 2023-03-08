using ValuedInBE.DataControls.Paging;
using ValuedInBE.Models;
using ValuedInBE.Models.DTOs.Requests.Chats;
using ValuedInBE.Models.DTOs.Responses.Chats;

namespace ValuedInBE.Services.Chats.Implementations
{
    public class ChatService : IChatService
    {
        public Task<MessageDTO> CreateNewMessageAsync(long chatId, NewMessage newMessage, UserContext userContext)
        {
            throw new NotImplementedException();
        }

        public Task<ChatInfo> FetchOrCreateChatAsync(NewChatRequest newChatRequest, UserContext userContext)
        {
            throw new NotImplementedException();
        }

        public Task<ChatInfo> GetChatInfoAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<OffsetPage<ChatInfo, DateTime>> GetChatsAsync(ChatPageRequest chatPage, UserContext userContext)
        {
            throw new NotImplementedException();
        }

        public Task<OffsetPage<MessageDTO, DateTime>> GetMessagesAsync(MessagePageRequest messagePage, long chatId, UserContext userContext)
        {
            throw new NotImplementedException();
        }
    }
}
