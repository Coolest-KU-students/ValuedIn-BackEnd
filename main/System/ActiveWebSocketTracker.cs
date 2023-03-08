using Microsoft.AspNetCore.DataProtection;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Xml.Serialization;
using ValuedInBE.Models;

namespace ValuedInBE.System
{
    public class ActiveWebSocketTracker : ConcurrentDictionary<string, List<WebSocket>>, IWebSocketTracker, IHostedService
    {
        private const int heartbeatIntervalsInSeconds = 30;
        private const int heartbeatTimeoutInSeconds = 10;
        private readonly Timer _heartbeatTimer;
        private readonly ILogger<ActiveWebSocketTracker> _logger;
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private bool _stillInCheck = false; //failsafe in case I'm stupid with the timer

        public ActiveWebSocketTracker(ILogger<ActiveWebSocketTracker> logger)
        {
            _heartbeatTimer = new Timer(CheckHeartbeat, null, Timeout.Infinite, Timeout.Infinite);
            _logger = logger;
        }

        public void Add(string userId, WebSocket webSocket)
        {
            _logger.LogDebug("Added web socket for user ID: {userId}", userId);
            AddOrUpdate(userId, new List<WebSocket>() { webSocket }, UpdateExistingSocketList);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Web Socket Tracker has activated");
            _heartbeatTimer.Change(0, heartbeatIntervalsInSeconds);
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Web Socker Tracker has deactivated, closing all sockets;");
            List<Task> closingTasks = new();
            foreach (WebSocket socket in Values.SelectMany(value => value))
            {
                closingTasks.Add(socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Service is closing", cancellationToken));
            }
            Clear();
            _heartbeatTimer.Dispose();
            _cancellationTokenSource.Cancel();
            await Task.WhenAll(closingTasks);
        }

        public List<WebSocket> GetSockets(string userId)
        {
            _logger.LogTrace("Getting sockets for user ID: {userId}", userId);
            return TryGetValue(userId, out List<WebSocket> sockets)
                    ? sockets
                    : new List<WebSocket>();
        }

        private Func<string, List<WebSocket>, List<WebSocket>> UpdateExistingSocketList =>
            (userId, newList) =>
            {
                List<WebSocket> currentList = GetSockets(userId);
                currentList.AddRange(newList);
                return currentList;
            };

        private async void CheckHeartbeat(object state)
        {
            if(_stillInCheck) { return; }
            _stillInCheck = true;
            _logger.LogDebug("Checking Heartbeat for WebSockets");
            List<Task> tasks = new();
            foreach(List<WebSocket> webSockets in Values)
            {
                CheckWebSockets(tasks, webSockets);
            }
            await Task.WhenAll(tasks);
            _stillInCheck = false;
        }

        private void CheckWebSockets(List<Task> tasks, List<WebSocket> webSockets)
        {
            webSockets.RemoveAll(socket => socket.CloseStatus.HasValue);//clear all closed ones
            foreach (WebSocket socket in webSockets)
            {
                tasks.Add(TryPingPongWithinTime(socket, _cancellationTokenSource.Token));
            }
        }

        private static async Task TryPingPongWithinTime(WebSocket webSocket, CancellationToken cancellationToken)
        {
            Task echoTask = PingPong(webSocket, cancellationToken);
            await Task.Delay(heartbeatTimeoutInSeconds * 1000, cancellationToken);
            if(!echoTask.IsCompleted && !webSocket.CloseStatus.HasValue)
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Heartbeat timeout", CancellationToken.None);
        }

        private static async Task PingPong(WebSocket webSocket, CancellationToken cancellationToken)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken); //either the first request, or the one sent previous round;
            await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, cancellationToken);
        }
    }
}

