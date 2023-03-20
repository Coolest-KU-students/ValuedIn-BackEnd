using ValuedInBE.Chats.Models.Events;

namespace ValuedInBE.Chats.EventHandlers
{
    public interface IMessageEventHandler
    {
        Task HandleSentMessageEvent(NewMessageEvent messageEvent);
    }
}