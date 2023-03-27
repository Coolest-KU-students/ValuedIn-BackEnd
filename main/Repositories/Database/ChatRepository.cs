using Microsoft.EntityFrameworkCore;
using ValuedInBE.Contexts;
using ValuedInBE.Models;
using ValuedInBE.Models.Entities.Messaging;
using ValuedInBE.System.Extensions;
using ValuedInBE.System.Middleware;

namespace ValuedInBE.Repositories.Database
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

        public async Task AddChatParticipantsAsync(List<ChatParticipant> participants)
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

        public async Task<Chat> GetChatFromParticipantsAsync(List<string> allParticipants)
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
            UserContext userContext = _contextAccessor.HttpContext.GetUserContext();
            var a = from c in _context.Chats
                                .Include(c=>c.Messages.OrderByDescending(m => m.CreatedOn).Take(1))
                                .Include(c=>c.Participants)
                                .Take(pageSize)
                                .Where(c=>
                                    c.Participants.Select(p=>p.UserId).Contains(userContext.UserID)
                                    && (createdSince.HasValue && c.CreatedOn > createdSince)
                                    )
                                .OrderByDescending(c=>c.CreatedOn)
                                select c;
            await a.LoadAsync();
            return a;
        }

        public List<ChatParticipant> GetParticipantsFromChat(long chatId)
        {
            IQueryable<ChatParticipant> a = from p in _context.ChatParticipants
                                                        .Where(p => p.ChatId == chatId)
                                            select p;

            return a.ToList();
        }

        public async Task<List<string>> GetParticipantIdsFromChat(long chatId)
        {
            IQueryable<string> a = from p in _context.ChatParticipants
                                                        .Where(p => p.ChatId == chatId)
                                            select p.UserId;

            return await a.ToListAsync();
        }

        private void CheckEntityAuditing()
        {
            UserContext userContext = _contextAccessor.HttpContext.GetUserContext();
            _context.ChangeTracker.CheckAuditing(userContext);
        }

        public async Task<Chat> GetChatMessagesWithParticipantsDetails(long chatId, int size, DateTime? createdSince)
        {
            UserContext userContext = _contextAccessor.HttpContext.GetUserContext();
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
    }
}
