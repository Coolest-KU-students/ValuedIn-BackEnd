﻿using Xunit;
using ValuedInBE.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using ValuedInBE.System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ValuedInBE.Models;
using System.Net.WebSockets;
using Microsoft.Extensions.Logging;
using ValuedInBE.Services.Tokens;

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
        private readonly UserContext testingContext = UserConstants.UserContextInstance;
        private const string token = "token";
        private const string tokenType = "WebSocketToken";

        private static readonly TimeSpan _connectionVerificationIntervals = TimeSpan.FromSeconds(10);

        private WebSocketController MockWebSocketController()
        {
            _mockTokenService.Setup(service => service.GetUserContextFromToken(token, tokenType))
                .Returns(testingContext);
            return new(_mockWebSocketTracker.Object, _mockLogger.Object, _mockTokenService.Object); 
        }


        [Fact()]
        public async void FailsToEstablishWhenNotAWebSocket()
        {
            _mockWebSocketManager.SetupGet(manager => manager.IsWebSocketRequest)
                .Returns(true);

            _mockHttpContext.SetupGet(context => context.WebSockets)
                .Returns(_mockWebSocketManager.Object);

            WebSocketController socketController = MockWebSocketController();
            socketController.ControllerContext.HttpContext = _mockHttpContext.Object;
            ActionResult emptyTokenEstablish = await socketController.Establish("");
            Assert.NotNull(emptyTokenEstablish);
            Assert.IsType<BadRequestResult>(emptyTokenEstablish);

            socketController.ControllerContext.HttpContext = new DefaultHttpContext();
            Task<ActionResult> establishTask = socketController.Establish("bad token");
            await Task.Delay(1); //to ensure synchronisity;
            Assert.True(establishTask.IsCompleted);
            ActionResult result = await establishTask;
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async void TestEstablishingAWebSocketAndCuttingItOff()
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

            _mockWebSocketTracker.Setup(tracker => tracker.Add(testingContext.UserID, It.IsAny<WebSocket>()))
                .Verifiable();

            WebSocketController socketController = MockWebSocketController();
            socketController.ControllerContext.HttpContext = _mockHttpContext.Object;
            Task<ActionResult> establishTask = socketController.Establish(token);
            
            await Task.Delay(_connectionVerificationIntervals*0.1); //to ensure it ran at least one loop;
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