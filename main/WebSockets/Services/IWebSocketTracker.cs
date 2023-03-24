﻿using System.Net.WebSockets;

namespace ValuedInBE.WebSockets.Services
{
    public interface IWebSocketTracker
    {
        void Add(string userId, WebSocket webSocket);

        IEnumerable<WebSocket> GetSockets(string userId);

        Task SendMessageAsync(string message, string userId, CancellationToken token);
    }
}
