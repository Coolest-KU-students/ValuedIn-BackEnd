using Xunit;
using ValuedInBE.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using System.Net.WebSockets;
using NuGet.Common;

namespace ValuedInBE.System.Tests
{
    public class ActiveWebSocketTrackerTests
    {
        private readonly Mock<ILogger<ActiveWebSocketTracker>> _logger = new();
        private const string firstUserId = "first";
        private const string secondUserId = "second";
        private const string unregisteredId = "unregistered";
        private readonly Mock<WebSocket> _mockedFirstSocket = new();
        private readonly Mock<WebSocket> _mockedSecondSocket = new();
        private readonly Mock<WebSocket> _mockedUnregisteredSocket = new();
        private readonly TimeSpan _toTimeout = TimeSpan.FromSeconds(15);

        private ActiveWebSocketTracker MockActiveWebSocketTracker()
        {
            ActiveWebSocketTracker tracker = new(_logger.Object);
            tracker.AddOrUpdate(firstUserId, new List<WebSocket>(){ _mockedFirstSocket.Object }, (userId, newList) => newList);
            tracker.AddOrUpdate(secondUserId, new List<WebSocket>() { _mockedSecondSocket.Object }, (userId, newList) => newList);
            return tracker;
        }


        [Fact()]
        public void TestGetSockets()
        {
            ActiveWebSocketTracker tracker = MockActiveWebSocketTracker();

            List<WebSocket> emptySockets = tracker.GetSockets(unregisteredId);
            List<WebSocket> firstUserSockets = tracker.GetSockets(firstUserId);
            List<WebSocket> secondUserSockets = tracker.GetSockets(secondUserId);

            Assert.Empty(emptySockets);
            Assert.NotEmpty(firstUserSockets);
            Assert.Single(firstUserSockets, _mockedFirstSocket.Object);
            Assert.NotEmpty(secondUserSockets);
            Assert.Single(secondUserSockets, _mockedSecondSocket.Object);
        }

        [Fact()]
        public void TestAdd()
        {
            ActiveWebSocketTracker tracker = MockActiveWebSocketTracker();
            tracker.Add(unregisteredId, _mockedUnregisteredSocket.Object); //This will add +1 to Count
            tracker.Add(firstUserId, _mockedUnregisteredSocket.Object); //And tehse should not change anything
            tracker.Add(secondUserId, _mockedUnregisteredSocket.Object);

            Assert.Equal(3, tracker.Count);
        }

        [Fact()]
        public async void TestStartAsync()
        {
            WebSocketReceiveResult receiveResult = new(0, WebSocketMessageType.Text, true);
            
            //First socket is alive, retuns correctly
            _mockedFirstSocket.Setup(socket => 
                socket.ReceiveAsync(
                    It.IsAny<ArraySegment<byte>>(),
                    It.IsAny<CancellationToken>()
                )
            ).ReturnsAsync(receiveResult)
            .Verifiable("Response was received");

            _mockedFirstSocket.Setup(socket =>
                socket.SendAsync(
                    It.IsAny<ArraySegment<byte>>(),
                    WebSocketMessageType.Text,
                    true,
                    It.IsAny<CancellationToken>()
               )
            ).Verifiable("A response was sent");

            _mockedFirstSocket.SetupGet(socket => socket.CloseStatus).Returns(WebSocketCloseStatus.Empty);

            //Second socket is dead, responds after timeout

            bool wasCalled = false; //to ensure it was called at all
            _mockedSecondSocket.Setup(socket =>
                socket.ReceiveAsync(
                    It.IsAny<ArraySegment<byte>>(),
                    It.IsAny<CancellationToken>()
                )
            ).Returns(
                Task.Run(
                    async () => {
                        wasCalled = true;
                        await Task.Delay(_toTimeout);
                        return receiveResult;
                    })
              );
            _mockedSecondSocket.SetupGet(socket => socket.CloseStatus).Returns(WebSocketCloseStatus.Empty);
            _mockedSecondSocket.Setup(socket =>
                socket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure, It.IsAny<string>(),
                    It.IsAny<CancellationToken>()
                    )
             ).Returns(Task.CompletedTask).Verifiable();

            ActiveWebSocketTracker tracker = MockActiveWebSocketTracker();
            Task startTask = tracker.StartAsync(CancellationToken.None);

            await Task.Delay(_toTimeout); //so the timer runs out
            Assert.True(startTask.IsCompleted); //should finish immediately
            Assert.True(wasCalled); //timer should launch immediately
            Mock.Verify();
        }

        [Fact()]
        public void TestStopAsync()
        {
            CancellationToken token = new CancellationTokenSource().Token;
            _mockedFirstSocket.Setup(socket =>
                socket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure, It.IsAny<string>(), token
                )
             ).Returns(Task.CompletedTask).Verifiable();
            _mockedSecondSocket.Setup(socket =>
                socket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure, It.IsAny<string>(), token
                )
             ).Returns(Task.CompletedTask).Verifiable();

            ActiveWebSocketTracker tracker = MockActiveWebSocketTracker();
            Task stopTask = tracker.StopAsync(token);

            Assert.True(stopTask.IsCompleted); //No actual IO processes, should complete immediately
            Assert.Empty(tracker);


        }

        
    }
}