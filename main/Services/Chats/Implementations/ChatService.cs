using ValuedInBE.DataControls.Paging;
using ValuedInBE.Models;
using ValuedInBE.Models.DTOs.Requests.Chats;
using ValuedInBE.Models.DTOs.Responses.Chats;
using ValuedInBE.Models.Entities.Messaging;
using ValuedInBE.Repositories;

namespace ValuedInBE.Services.Chats.Implementations
{/*
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly ILogger<ChatService> _logger;

        public ChatService(IChatRepository chatRepository, ILogger<ChatService> logger)
        {
            _chatRepository = chatRepository;
            _logger = logger;
        }
        public Task<OffsetPage<ChatInfo, DateTime>> GetChatsAsync(ChatPageRequest chatPage, UserContext userContext)
        {
            string userId = userContext.UserID;
            DateTime? createdSince = chatPage.Offset;
            int pageSize = chatPage.Size;

            IEnumerable<Chat> chats = _chatRepository.GetChatsWithLastMessageAsync(userId, pageSize, createdSince);
           // DateTime newOffset = chats.Last() createdSince ?? DateTime.Now;
            
            throw new NotImplementedException();
        }

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
    }*/
}
