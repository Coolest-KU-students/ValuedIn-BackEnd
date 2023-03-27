using System.Net.WebSockets;

namespace ValuedInBE.WebSockets.Services
{
    public interface IWebSocketTracker
    {
        void Add(string userId, WebSocket webSocket);

        List<WebSocket> GetUserSockets(string userId);

        Task SendMessageAsync(string message, string userId, CancellationToken token);
    }
}
