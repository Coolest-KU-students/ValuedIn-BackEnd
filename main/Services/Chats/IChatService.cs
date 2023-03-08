using Microsoft.AspNetCore.Mvc;
using ValuedInBE.DataControls.Paging;
using ValuedInBE.Models;
using ValuedInBE.Models.DTOs.Requests.Chats;
using ValuedInBE.Models.DTOs.Responses.Chats;

namespace ValuedInBE.Services.Chats
{
    public interface IChatService
    {
        Task<OffsetPage<ChatInfo, DateTime>> GetChatsAsync(ChatPageRequest chatPage, UserContext userContext);
        Task<ChatInfo> GetChatInfoAsync(long id);
        Task<OffsetPage<MessageDTO, DateTime>> GetMessagesAsync(MessagePageRequest messagePage, long chatId, UserContext userContext);
        Task<MessageDTO> CreateNewMessageAsync(long chatId, NewMessage newMessage, UserContext userContext);
        Task<ChatInfo> FetchOrCreateChatAsync(NewChatRequest newChatRequest, UserContext userContext);
    }
}
