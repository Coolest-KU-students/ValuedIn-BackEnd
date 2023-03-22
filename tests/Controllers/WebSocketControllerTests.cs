using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using Microsoft.Extensions.Logging;
using ValuedInBE.WebSockets.Controllers;
using ValuedInBE.Tokens.Services;
using ValuedInBE.Users.Models;
using ValuedInBE.WebSockets.Services;

namespace ValuedInBE.Controllers.Tests
{
    public class WebSocketControllerTests
    {
        private readonly Mock<IWebSocketTracker> _mockWebSocketTracker = new();
        private readonly Mock<ILogger<WebSocketController>> _mockLogger = new();
        private readonly Mock<ITokenService> _mockTokenService = new();
        private readonly Mock<HttpContext> _mockHttpContext = new();
        private readonly Mock<WebSocketManager> _mockWebSocketManager = new();
        private readonly Mock<WebSocket> _mockWebSocket = new();
        private readonly UserContext _testingContext = UserConstants.UserContextInstance;
        private const string token = "token";
        private const string tokenType = "WebSocketToken";

        private static readonly TimeSpan _connectionVerificationIntervals = TimeSpan.FromSeconds(10);

        private WebSocketController MockWebSocketController()
        {
            _mockTokenService.Setup(service => service.GetUserContextFromToken(token, tokenType))
                .Returns(_testingContext);
            return new(_mockWebSocketTracker.Object, _mockLogger.Object, _mockTokenService.Object); 
        }


        [Fact()]
        public async Task FailsToEstablishWhenNotAWebSocket()
        {
            _mockWebSocketManager.SetupGet(manager => manager.IsWebSocketRequest)
                .Returns(true);

            _mockHttpContext.SetupGet(context => context.WebSockets)
                .Returns(_mockWebSocketManager.Object);

            WebSocketController socketController = MockWebSocketController();
            socketController.ControllerContext.HttpContext = _mockHttpContext.Object;
            ActionResult emptyTokenEstablish = await socketController.EstablishAsync("");
            Assert.NotNull(emptyTokenEstablish);
            Assert.IsType<BadRequestResult>(emptyTokenEstablish);

            socketController.ControllerContext.HttpContext = new DefaultHttpContext();
            Task<ActionResult> establishTask = socketController.EstablishAsync("bad token");
            //ensure synchronisity:
            await Task.Delay(1); 
            Assert.True(establishTask.IsCompleted);
            ActionResult result = await establishTask;
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task TestEstablishingAWebSocketAndCuttingItOff()
        {
            _mockWebSocket.SetupGet(socket => socket.CloseStatus)
                .Returns(WebSocketCloseStatus.NormalClosure);
            _mockWebSocket.Setup(socket => socket.Dispose()).Verifiable();

            _mockWebSocketManager.Setup(manager => manager.AcceptWebSocketAsync())
                .ReturnsAsync(_mockWebSocket.Object);
            _mockWebSocketManager.SetupGet(manager => manager.IsWebSocketRequest)
                .Returns(true);

            _mockHttpContext.SetupGet(context => context.WebSockets)
                .Returns(_mockWebSocketManager.Object);

            _mockWebSocketTracker.Setup(tracker => tracker.Add(_testingContext.UserID, It.IsAny<WebSocket>()))
                .Verifiable();

            WebSocketController socketController = MockWebSocketController();
            socketController.ControllerContext.HttpContext = _mockHttpContext.Object;
            Task<ActionResult> establishTask = socketController.EstablishAsync(token);
            
            await Task.Delay(_connectionVerificationIntervals*0.1); //to ensure it ran at least one loop
            Assert.False(establishTask.IsCompleted);

            await Task.Delay(_connectionVerificationIntervals);
            Assert.True(establishTask.IsCompleted);
            Mock.VerifyAll();

            ActionResult result = await establishTask;
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
        }
    }
}