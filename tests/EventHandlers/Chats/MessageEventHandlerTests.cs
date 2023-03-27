using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Confluent.Kafka;
using ValuedInBE.Models.Entities.Messaging;
using ValuedInBE.System;
using ValuedInBE.System.Kafka;
using ValuedInBETests.TestingConstants;
using System.Net.WebSockets;
using System.Linq.Expressions;
using ValuedInBE.Events.Handlers;
using ValuedInBE.Models.Events;

namespace ValuedInBE.Events.Chats.Tests
{
    public class MessageEventHandlerTests
    {
        private const string newChatMessageTopic = "New-Chat-Message";
        private readonly Mock<ILogger<MessageEventHandler>> _logger = new();
        private readonly Mock<IWebSocketTracker> _webSocketTracker = new();
        private readonly Mock<IConsumer<long, NewMessageEvent>> _consumer = new();
        private readonly Mock<IProducer<long, NewMessageEvent>> _producer = new();
        private readonly Mock<IKafkaConfigurationBuilder<long, NewMessageEvent>> _configurationBuilder = new();

        private MessageEventHandler MockMessageEventHandler()
        {
            _configurationBuilder.Setup(builder => builder.ConfigureConsumer(It.IsAny<ConsumerConfig>()))
                .Returns(_consumer.Object);

            _configurationBuilder.Setup(builder => builder.ConfigureProducer())
                .Returns(_producer.Object);

            return new(
                _logger.Object,
                _webSocketTracker.Object,
                _configurationBuilder.Object
                );
        }

        [Fact()]
        public async void TestHandleSentMessageEvent()
        {
            ChatMessage chatMessage = ChatConstants.ChatMessage;
            NewMessageEvent newMessageEvent = new(){ OtherParticipantIDs = new List<string>(), ChatMessage = chatMessage };
            Message<long, NewMessageEvent> message = new() { Key = chatMessage.Id, Value = newMessageEvent };
            DeliveryResult<long, NewMessageEvent> deliveryResult = new()
            {
                Message = message,
                Status = PersistenceStatus.Persisted
            };
            _producer
                .Setup(procuder => 
                    procuder.ProduceAsync(newChatMessageTopic, It.Is<Message<long, NewMessageEvent>>(exp => exp.Key.Equals(chatMessage.Id)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(deliveryResult);

            MessageEventHandler messageEventHandler = MockMessageEventHandler();
            await messageEventHandler.HandleSentMessageEvent(newMessageEvent);
        }

        [Fact()]
        public void MessageConsumptionOnStartTest()
        {
            /***
             * Mock Sockets
             */
            Mock<WebSocket> mockedOnlineWebSocket1 = new();
            Mock<WebSocket> mockedOnlineWebSocket2 = new();
            Mock<WebSocket> mockedOfflineWebSocket1 = new();
            Mock<WebSocket> mockedOfflineWebSocket2 = new();

            mockedOnlineWebSocket1.SetupGet(socket => socket.CloseStatus)
                .Returns(WebSocketCloseStatus.Empty);
            mockedOnlineWebSocket2.SetupGet(socket => socket.CloseStatus)
                .Returns(WebSocketCloseStatus.Empty);
            mockedOfflineWebSocket1.SetupGet(socket => socket.CloseStatus)
                .Returns(WebSocketCloseStatus.NormalClosure);
            mockedOfflineWebSocket2.SetupGet(socket => socket.CloseStatus)
                .Returns(WebSocketCloseStatus.NormalClosure);

            mockedOnlineWebSocket1.Setup(MockedSendAsyncExpression()).Verifiable("Online Web Socket 1 was not called");
            mockedOnlineWebSocket2.Setup(MockedSendAsyncExpression()).Verifiable("Online Web Socket 2 was not called");

            mockedOfflineWebSocket1.Setup(MockedSendAsyncExpression()).Throws(new Exception("Offline Web Socket 1 should not be called"));
            mockedOfflineWebSocket2.Setup(MockedSendAsyncExpression()).Throws(new Exception("Offline Web Socket 1 should not be called"));

            List<WebSocket> firstUserSockets = new() { mockedOnlineWebSocket1.Object, mockedOfflineWebSocket1.Object };
            List<WebSocket> secondUserSockets = new() { mockedOnlineWebSocket2.Object, mockedOfflineWebSocket2.Object };

            /** Mocking users **/

            ChatMessage chatMessage = ChatConstants.ChatMessage;
            string firstUserId = "first";
            string secondUserId = "second";
            long chatId = chatMessage.Id;

            NewMessageEvent newMessageEvent = new()
            {
                OtherParticipantIDs = new() { firstUserId, secondUserId }
                , ChatMessage = chatMessage
            };

            /** Web Socket Tracker setup **/

            _webSocketTracker.Setup(tracker => tracker.GetSockets(firstUserId))
                .Returns(firstUserSockets);
            _webSocketTracker.Setup(tracker => tracker.GetSockets(secondUserId))
                .Returns(secondUserSockets);

            /** Consumer Setup **/
            Message<long, NewMessageEvent> message = new() { Key = chatMessage.Id, Value = newMessageEvent };
            ConsumeResult<long, NewMessageEvent> consumeResult = new()
            {
                Message = message
            };

            CancellationTokenSource tokenSource = new();

            _consumer.Setup(consumer => consumer.Consume(It.IsAny<CancellationToken>()))
                .Returns(consumeResult)
                .Callback(tokenSource.Cancel);


            /** Test */
            MessageEventHandler messageEventHandler = MockMessageEventHandler();
            Task startedEventHandler = messageEventHandler.StartAsync(tokenSource.Token);

            Mock.Verify(); //verify that the sockets were called;
            Assert.True(startedEventHandler.IsCompleted, "The request did not cancel if the task is not completed"); 
        }

        private static Expression<Func<WebSocket, Task>> MockedSendAsyncExpression()
        {
            return socket =>
                            socket.SendAsync(
                                It.IsAny<ArraySegment<byte>>(),
                                It.IsAny<WebSocketMessageType>(),
                                It.IsAny<bool>(),
                                It.IsAny<CancellationToken>()
                            );
        }
    }
}