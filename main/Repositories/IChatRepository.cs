using ValuedInBE.Models.Entities.Messaging;

namespace ValuedInBE.Repositories
{
    public interface IChatRepository
    {
        List<ChatParticipant> GetParticipantsFromChat(long chatId);
    }
}
