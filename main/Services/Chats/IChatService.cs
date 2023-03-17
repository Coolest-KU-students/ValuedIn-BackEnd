using Microsoft.AspNetCore.Mvc;
using ValuedInBE.DataControls.Paging;
using ValuedInBE.Models;
using ValuedInBE.Models.DTOs.Requests.Chats;
using ValuedInBE.Models.DTOs.Responses.Chats;
using ValuedInBE.Models.Entities.Messaging;

namespace ValuedInBE.Services.Chats
{
    public interface IChatService
    {
        Task<OffsetPage<ChatInfo, DateTime>> GetChatsAsync(ChatPageRequest chatPage);
        Task<OffsetPage<MessageDTO, DateTime>> GetMessagesAsync(MessagePageRequest messagePage, long chatId);
        Task<ChatMessage> CreateNewMessageAsync(long chatId, string message); 
        Task<Chat> FetchOrCreateChatAsync(NewChatRequest newChatRequest);
    }
}
