using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using ValuedInBE.Models;
using ValuedInBE.System;
using ValuedInBE.System.Middleware;

namespace ValuedInBE.Controllers
{
    [Route("ws")]
    public class WebSocketController : ControllerBase
    {
        private static readonly TimeSpan _connectionVerificationIntervals = TimeSpan.FromSeconds(10);
        private readonly IWebSocketTracker _socketTracker;
        private readonly ILogger<WebSocketController> _logger;


        public WebSocketController(IWebSocketTracker socketTracker, ILogger<WebSocketController> logger)
        {
            _socketTracker = socketTracker;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> Establish()
        {
            if (!HttpContext.WebSockets.IsWebSocketRequest)
            {
                return BadRequest();
            }

            using WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            //UserContext userContext = HttpContext.GetUserContext();

            //_socketTracker.Add(userContext.UserID, webSocket);
            _socketTracker.Add("aaaaaaaaaaa", webSocket);
            do
            {
                await Task.Delay(_connectionVerificationIntervals);
            } while (!webSocket.CloseStatus.HasValue);

            webSocket.Dispose();
            return NoContent();
        }
    }
}
