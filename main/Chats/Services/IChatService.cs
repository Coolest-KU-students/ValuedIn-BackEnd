
using ValuedInBE.Chats.Models.DTOs.Request;
using ValuedInBE.Chats.Models.DTOs.Response;
using ValuedInBE.Chats.Models.Entities;
using ValuedInBE.DataControls.Paging;

namespace ValuedInBE.Chats.Services
{
    public interface IChatService
    {
        Task<OffsetPage<ChatInfo, DateTime>> GetChatsAsync(ChatPageRequest chatPage);
        Task<OffsetPage<MessageDTO, DateTime>> GetMessagesAsync(MessagePageRequest messagePage, long chatId);
        Task<ChatMessage> CreateNewMessageAsync(long chatId, string message);
        Task<Chat> FetchOrCreateChatAsync(NewChatRequest newChatRequest);
    }
}
