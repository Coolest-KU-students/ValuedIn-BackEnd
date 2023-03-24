using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace ValuedInBE.WebSockets.Services
{
    public class ActiveWebSocketTracker : IWebSocketTracker, IHostedService
    {
        public ConcurrentDictionary<string, List<WebSocket>> UserWebSocketDictionary { get; init; }

        private const int heartbeatIntervalsInSeconds = 30;
        private readonly TimeSpan _heartbeatIntervalTimespan = TimeSpan.FromSeconds(heartbeatIntervalsInSeconds);
        private const int heartbeatTimeoutInSeconds = 2;
        private readonly TimeSpan _heartbeatTimeoutTimespan = TimeSpan.FromSeconds(heartbeatTimeoutInSeconds);
        private const string heartbeatSendMessage = "ping";
        private readonly Timer _heartbeatTimer;
        private readonly ILogger<ActiveWebSocketTracker> _logger;
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private bool _stillInCheck = false; //failsafe in case I'm stupid with the timer
        private readonly Encoding _encoding = Encoding.UTF8;


        public ActiveWebSocketTracker(ILogger<ActiveWebSocketTracker> logger)
        {
            _heartbeatTimer = new Timer(CheckHeartbeat, null, Timeout.Infinite, Timeout.Infinite);
            _logger = logger;
            UserWebSocketDictionary = new();
        }

        public void Add(string userId, WebSocket webSocket)
        {
            _logger.LogDebug("Added web socket for user ID: {userId}", userId);
            UserWebSocketDictionary.AddOrUpdate(userId, new List<WebSocket>() { webSocket }, UpdateExistingSocketList);
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
            foreach (WebSocket socket in UserWebSocketDictionary.Values.SelectMany(value => value))
            {
                closingTasks.Add(CloseSocketAndDisposeAsync(socket, cancellationToken));
            }
            UserWebSocketDictionary.Clear();
            await Task.WhenAll(closingTasks);
        }

        public IEnumerable<WebSocket> GetSockets(string userId)
        {
            _logger.LogTrace("Getting sockets for user ID: {userId}", userId);
            return UserWebSocketDictionary.TryGetValue(userId, out List<WebSocket> sockets)
                    ? sockets
                    : new List<WebSocket>();
        }

        private Func<string, List<WebSocket>, List<WebSocket>> UpdateExistingSocketList =>
            (userId, newList) =>
            {
                List<WebSocket> currentList = GetSockets(userId).ToList();
                currentList.AddRange(newList);
                return currentList; 
            };

        private async void CheckHeartbeat(object state)
        {
            if (_stillInCheck) { return; } //skip if it hasn't finished?
            _stillInCheck = true;
            _logger.LogDebug("Checking Heartbeat for WebSockets");
            List<Task> tasks = new();
            foreach (List<WebSocket> webSockets in UserWebSocketDictionary.Values)
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
                        _logger.LogTrace("Tracker disposing of socket {socket}",   socket);
                        socket.Dispose();
                        webSockets.Remove(socket);
                    }
                    tasks.Add(TryPingPongWithinTimeAsync(socket, _cancellationTokenSource.Token));
                }
            );
        }

        private static async Task CloseSocketAndDisposeAsync(WebSocket socket, CancellationToken cancellationToken)
        {
            if (socket.State != WebSocketState.Open) return;
            
            await socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Service is closing", cancellationToken);
            socket.Dispose();
        }

        private async Task TryPingPongWithinTimeAsync(WebSocket webSocket, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new();
            try
            {
                Task echoTask = PingPongAsync(webSocket, cancellationTokenSource.Token);
                await Task.Delay(_heartbeatTimeoutTimespan, cancellationToken);
                
                if (echoTask.IsCompleted)
                    return;
            }
            catch (TaskCanceledException)
            {
                _logger.LogWarning("Task Cancellation issued within Socker Tracker");
            }
            cancellationTokenSource.Cancel();
            await webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Heartbeat timeout", CancellationToken.None);
        }

        private async Task PingPongAsync(WebSocket webSocket, CancellationToken cancellationToken)
        {
            var buffer = new byte[1024 * 4];
            await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
            if (cancellationToken.IsCancellationRequested 
            || webSocket.State != WebSocketState.Open) 
                    return;
            await SendMessageAsync(webSocket, heartbeatSendMessage, cancellationToken);
        }

        public async Task SendMessageAsync(string message, string userId, CancellationToken token)
        {
            IEnumerable<WebSocket> userWebSockets = GetSockets(userId);
            if(!userWebSockets.Any()) { return; }

            await Task.WhenAll(userWebSockets.Select(socket => SendMessageAsync(socket, message, token)));
        }
    
        public async Task SendMessageAsync(WebSocket socket, string message, CancellationToken? token = null)
        {
            token ??= _cancellationTokenSource.Token;
            await socket.SendAsync(_encoding.GetBytes(message), WebSocketMessageType.Text, true, token.Value);
        }
    }
}

