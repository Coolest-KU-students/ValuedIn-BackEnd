using Confluent.Kafka;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;
using ValuedInBE.Chats.Models.Entities;
using ValuedInBE.Chats.Models.Events;
using ValuedInBE.System.External.Services.Kafka;
using ValuedInBE.WebSockets.Services;

namespace ValuedInBE.Chats.EventHandlers
{
    public class MessageEventHandler : IHostedService, IMessageEventHandler
    {
        private const string groupId = "chats";
        private const string newChatMessageTopic = "New-Chat-Message";
        private readonly ILogger<MessageEventHandler> _logger;
        private readonly IWebSocketTracker _webSocketTracker;
        private readonly IConsumer<long, NewMessageEvent> _consumer;
        private readonly IProducer<long, NewMessageEvent> _producer;
        private CancellationToken _cancellationToken;
        private Timer _timer;

        public MessageEventHandler(ILogger<MessageEventHandler> logger, IWebSocketTracker webSocketTracker, IKafkaConfigurationBuilder<long, NewMessageEvent> configurationBuilder)
        {
            _logger = logger;
            _webSocketTracker = webSocketTracker;
            _consumer =
                configurationBuilder.ConfigureConsumer(
                new()
                {
                    GroupId = groupId,
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    AllowAutoCreateTopics = true
                });
            _producer = configurationBuilder.ConfigureProducer();
            _consumer.Subscribe(newChatMessageTopic);
        }

        public async Task HandleSentMessageEventAsync(NewMessageEvent messageEvent)
        {
            ChatMessage messageSent = messageEvent.ChatMessage;
            _logger.LogDebug("Registering that message {messageID} was sent in chat {chatID}", messageSent.Id, messageSent.ChatId);

            DeliveryResult<long, NewMessageEvent> delivery = await _producer.ProduceAsync(newChatMessageTopic, new() { Key = messageSent.Id, Value = messageEvent });

            switch (delivery.Status)
            {
                case PersistenceStatus.Persisted:
                    _logger.LogInformation("Message {messageID} registered", messageSent.Id);
                    break;
                case PersistenceStatus.PossiblyPersisted:
                    _logger.LogWarning("Message {messageID} might not be registered", messageSent.Id);
                    break;
                case PersistenceStatus.NotPersisted:
                    _logger.LogCritical("Message {messageID} was not registered, delivery info: {headers}", messageSent.Id, delivery.Headers);
                    break;
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Message event handler has started");
            _cancellationToken = cancellationToken;
            _timer = new Timer(MessageProcessingAsync, null, 0, Timeout.Infinite);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Dispose();
            _logger.LogInformation("Message event handler has stopped");
            return Task.CompletedTask;
        }

        private async void MessageProcessingAsync(object state)
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                ConsumeResult<long, NewMessageEvent> result = ConsumeNextMessage(_cancellationToken);
                long chatId = result.Message.Key;
                IEnumerable<string> participatingUsers = result.Message.Value.OtherParticipantIDs;
                _logger.LogTrace("Received new message in chat {chatID}, attempting to distribute to users {users}", chatId, string.Join(", ", participatingUsers));
                await DistributeMessagesAsync(participatingUsers, result.Message.Value.ChatMessage, _cancellationToken);
            }
        }


        private ConsumeResult<long, NewMessageEvent> ConsumeNextMessage(CancellationToken cancellationToken = default)
        {

            _logger.LogTrace("Awaiting for a message");
            ConsumeResult<long, NewMessageEvent> received = _consumer.Consume(cancellationToken);
            _logger.LogInformation("Received that message {messageID} was sent in chat {chatID}", received.Message.Key, received.Message.Value.ChatMessage.ChatId);
            return received;
        }

        private async Task DistributeMessagesAsync(IEnumerable<string> userIds, ChatMessage chatMessage, CancellationToken cancellationToken)
        {
            List<Task> tasks = new();
            foreach (string user in userIds)
            {
                _logger.LogDebug("Sending a message {chatId} to user {user}", chatMessage.ChatId, user);
                tasks.Add(SendMessageAsync(user, chatMessage, cancellationToken));
            }
            await Task.WhenAll(tasks);
        }

        private async Task SendMessageAsync(string userId, ChatMessage chatMessage, CancellationToken cancellationToken)
        {
            IEnumerable<WebSocket> sockets = _webSocketTracker.GetSockets(userId);
            _logger.LogTrace("User {userId} was found to have {amount} of sockets active when sending a message", userId, sockets.Count());
            List<Task> tasks = new();
            foreach (WebSocket socket in sockets)
            {
                tasks.Add(SendMessageAsync(socket, chatMessage, cancellationToken));
            }
            await Task.WhenAll(tasks);
        }

        private static async Task SendMessageAsync(WebSocket socket, ChatMessage chatMessage, CancellationToken cancellationToken)
        {
            if (socket.State == WebSocketState.Open)
            {
                string messageJson = JsonConvert.SerializeObject(chatMessage);
                await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(messageJson)), 0, true, cancellationToken);
            }
        }
    }
}
