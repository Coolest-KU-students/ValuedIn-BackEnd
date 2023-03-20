using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace ValuedInBE.WebSockets.Services
{
    public class ActiveWebSocketTracker : ConcurrentDictionary<string, List<WebSocket>>, IWebSocketTracker, IHostedService
    {
        private const int heartbeatIntervalsInSeconds = 30;
        private readonly TimeSpan _heartbeatIntervalTimespan = TimeSpan.FromSeconds(heartbeatIntervalsInSeconds);
        private const int heartbeatTimeoutInSeconds = 2;
        private readonly TimeSpan _heartbeatTimeoutTimespan = TimeSpan.FromSeconds(heartbeatTimeoutInSeconds);
        private const string heartbeatSendMessage = "ping";
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
            _heartbeatTimer.Change(TimeSpan.Zero, _heartbeatIntervalTimespan);
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Web Socker Tracker has deactivated, closing all sockets;");
            _heartbeatTimer.Dispose();
            _cancellationTokenSource.Cancel(false);
            List<Task> closingTasks = new();
            foreach (WebSocket socket in Values.SelectMany(value => value))
            {
                closingTasks.Add(CloseSocketAndDispose(socket, cancellationToken));
            }
            Clear();
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
            if (_stillInCheck) { return; } //skip if it hasn't finished?
            _stillInCheck = true;
            _logger.LogDebug("Checking Heartbeat for WebSockets");
            List<Task> tasks = new();
            foreach (List<WebSocket> webSockets in Values)
            {
                CheckWebSockets(tasks, webSockets);
            }
            await Task.WhenAll(tasks);
            _stillInCheck = false;
        }


        private void CheckWebSockets(List<Task> tasks, List<WebSocket> webSockets)
        {
            webSockets.RemoveAll(socket => socket.CloseStatus.HasValue);//clear all closed ones
            webSockets.ForEach(
                socket =>
                {
                    if (socket.State != WebSocketState.Open)
                    {
                        socket.Dispose();
                        webSockets.Remove(socket);
                    }
                    tasks.Add(TryPingPongWithinTime(socket, _cancellationTokenSource.Token));
                }
            );
        }

        private static async Task CloseSocketAndDispose(WebSocket socket, CancellationToken cancellationToken)
        {
            if (socket.State == WebSocketState.None)
                await socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Service is closing", cancellationToken);
        }

        private async Task TryPingPongWithinTime(WebSocket webSocket, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            try
            {

                Task echoTask = PingPong(webSocket, cancellationTokenSource.Token);
                await Task.Delay(_heartbeatTimeoutTimespan, cancellationToken);
                if (echoTask.IsCompleted)
                    return;
            }
            catch (TaskCanceledException)
            {
                //TODO: die silently
            }
            cancellationTokenSource.Cancel();
            await webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Heartbeat timeout", CancellationToken.None);
        }

        private static async Task PingPong(WebSocket webSocket, CancellationToken cancellationToken)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
            if (cancellationToken.IsCancellationRequested) return;
            await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(heartbeatSendMessage), 0, heartbeatSendMessage.Length), result.MessageType, true, cancellationToken);
        }
    }
}

