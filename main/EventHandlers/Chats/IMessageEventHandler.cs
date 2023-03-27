using ValuedInBE.Models.Events;

namespace ValuedInBE.Events.Handlers
{
    public interface IMessageEventHandler
    {
        Task HandleSentMessageEvent(NewMessageEvent messageEvent);
    }
}