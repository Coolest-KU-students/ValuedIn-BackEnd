﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using ValuedInBE.System.WebConfigs.Middleware;
using ValuedInBE.Tokens.Services;
using ValuedInBE.Users.Models;
using ValuedInBE.WebSockets.Services;

namespace ValuedInBE.WebSockets.Controllers
{
    [Route("ws")]
    public class WebSocketController : ControllerBase
    {
        private static readonly TimeSpan _connectionVerificationIntervals = TimeSpan.FromSeconds(10);
        private readonly IWebSocketTracker _socketTracker;
        private readonly ILogger<WebSocketController> _logger;
        private readonly ITokenService _tokenService;
        private const string tokenType = "WebSocketToken";


        public WebSocketController(IWebSocketTracker socketTracker, ILogger<WebSocketController> logger, ITokenService tokenService)
        {
            _socketTracker = socketTracker;
            _logger = logger;
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<ActionResult> EstablishAsync([FromQuery] string token)
        {
            if (!HttpContext.WebSockets.IsWebSocketRequest || string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }

            UserContext userContext = _tokenService.GetUserContextFromToken(token, tokenType);
            if (userContext == null)
            {
                return Unauthorized();
            }
            using WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            _socketTracker.Add(userContext.UserID, webSocket);
            do
            {
                await Task.Delay(_connectionVerificationIntervals);
            } while (!webSocket.CloseStatus.HasValue);

            webSocket.Dispose();
            return NoContent();
        }

        [Authorize]
        [HttpGet("token")]
        public ActionResult<string> IssueTokenForWebSocket()
        {
            UserContext userContext = HttpContext.GetUserContext();
            return _tokenService.GenerateOneTimeUserAccessToken(userContext, tokenType);
        }
    }
}