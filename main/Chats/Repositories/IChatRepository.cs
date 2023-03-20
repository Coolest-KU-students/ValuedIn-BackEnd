﻿using ValuedInBE.Chats.Models.Entities;

namespace ValuedInBE.Chats.Repositories
{
    public interface IChatRepository
    {
        Task AddChatParticipantsAsync(List<ChatParticipant> pasticipants);
        Task SaveNewChatAsync(Chat chat);
        Task CreateNewChatMessageAsync(ChatMessage chatMessage);
        Task<Chat> GetChatFromParticipantsAsync(List<string> allParticipants);
        Task<IEnumerable<Chat>> GetChatsWithLastMessageAndParticipantsAsync(string userId, int pageSize, DateTime? createdSince);
        Task<IEnumerable<ChatParticipant>> GetParticipantsFromChatAsync(long chatId);
        Task<Chat> GetChatMessagesWithParticipantsDetailsAsync(long chatId, int size, DateTime? createdSince);
        Task<IEnumerable<string>> GetParticipantIdsFromChatAsync(long chatId);
    }
}