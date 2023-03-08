using System.Net.WebSockets;

namespace ValuedInBE.System
{
    public interface IWebSocketTracker
    {

        void Add(string userId, WebSocket webSocket);

        List<WebSocket> GetSockets(string userId);


    }
}
