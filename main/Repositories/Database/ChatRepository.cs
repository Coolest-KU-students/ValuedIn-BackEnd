using ValuedInBE.Contexts;
using ValuedInBE.Models.Entities.Messaging;

namespace ValuedInBE.Repositories.Database
{
    public class ChatRepository : IChatRepository
    {
        private readonly ValuedInContext _context;

        public ChatRepository(ValuedInContext context)
        {
            _context = context;
        }

        public List<ChatParticipant> GetParticipantsFromChat(long chatId)
        {
            IQueryable<ChatParticipant> a = from p in _context.ChatParticipants
                                                        .Where(p => p.ChatID == chatId)
                                            select p;

            return a.ToList();
        }
    }
}
