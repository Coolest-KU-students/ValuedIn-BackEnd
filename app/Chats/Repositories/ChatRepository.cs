using Microsoft.EntityFrameworkCore;
using System;
using ValuedInBE.Chats.Models.Entities;
using ValuedInBE.System.PersistenceLayer.Contexts;
using ValuedInBE.System.PersistenceLayer.Extensions;
using ValuedInBE.System.WebConfigs.Middleware;
using ValuedInBE.Users.Models;

namespace ValuedInBE.Chats.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ValuedInContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public ChatRepository(ValuedInContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public async Task AddChatParticipantsAsync(IEnumerable<ChatParticipant> participants)
        {
            _context.ChatParticipants.AddRange(participants);
            CheckEntityAuditing();
            await _context.SaveChangesAsync();
        }

        public async Task SaveNewChatAsync(Chat chat)
        {
            _context.Chats.Add(chat);
            CheckEntityAuditing();
            await _context.SaveChangesAsync();
        }

        public async Task CreateNewChatMessageAsync(ChatMessage chatMessage)
        {
            _context.ChatMessages.Add(chatMessage);
            CheckEntityAuditing();
            await _context.SaveChangesAsync();
        }

        public async Task<Chat?> GetChatFromParticipantsAsync(IEnumerable<string> allParticipants)
        {
            if(!allParticipants.Any()) { return null; }
            var a = from c in _context.Chats
                                .Where(c => c.Participants.Any() &&
                                         c.Participants.All(p => allParticipants.Contains(p.UserId))
                                )
                    select c;

            return await a.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Chat>> GetChatsWithLastMessageAndParticipantsAsync(string userId, int pageSize, DateTime? createdSince)
        {
            UserContext userContext = _contextAccessor.HttpContext!.GetMandatoryUserContext();
            var a = from c in _context.Chats
                                .Include(c => c.Messages.OrderByDescending(m => m.CreatedOn).Take(1))
                                .Include(c => c.Participants)
                                .Take(pageSize)
                                .Where(c =>
                                    c.Participants.Select(p => p.UserId).Contains(userContext.UserID)
                                    && createdSince.HasValue && c.CreatedOn > createdSince
                                    )
                                .OrderByDescending(c => c.CreatedOn)
                    select c;
            await a.LoadAsync();
            return a;
        }

        public async Task<IEnumerable<ChatParticipant>> GetParticipantsFromChatAsync(long chatId)
        {
            IQueryable<ChatParticipant> a = from p in _context.ChatParticipants
                                                        .Where(p => p.ChatId == chatId)
                                            select p;
            await a.LoadAsync();
            return a;
        }

        public async Task<IEnumerable<string>> GetParticipantIdsFromChatAsync(long chatId)
        {
            IQueryable<string> a = from p in _context.ChatParticipants
                                                        .Where(p => p.ChatId == chatId)
                                   select p.UserId;

            await a.LoadAsync();
            return a;
        }


        public async Task<Chat?> GetChatMessagesWithParticipantsDetailsAsync(long chatId, int size, DateTime? createdSince)
        {
            UserContext userContext = _contextAccessor.HttpContext.GetMandatoryUserContext();
            var a = _context.Chats
                        .Include(c => c.Participants)
                            .ThenInclude(p => p.User)
                        .Include(c => c.Messages
                                        .Take(size)
                                        .Where(m => createdSince.HasValue && m.CreatedOn > createdSince)
                                        .OrderByDescending(m => m.CreatedOn)
                        ).FirstOrDefaultAsync(c => c.Id == chatId && c.Participants.Any(p => p.UserId == userContext.UserID));

            return await a;
        }

        private void CheckEntityAuditing()
        {
            UserContext userContext = _contextAccessor.HttpContext.GetMandatoryUserContext();
            _context.ChangeTracker.CheckAuditing(userContext);
        }
    }
}
