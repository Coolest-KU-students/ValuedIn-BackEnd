using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using ValuedInBE.System.UserContexts.Accessors;
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
        private readonly IUserContextAccessor _userContextAccessor;
        private const string tokenType = "WebSocketToken";



        public WebSocketController(IWebSocketTracker socketTracker, ILogger<WebSocketController> logger, ITokenService tokenService, IUserContextAccessor userContextAccessor)
        {
            _socketTracker = socketTracker;
            _logger = logger;
            _tokenService = tokenService;
            _userContextAccessor = userContextAccessor;
        }

        [HttpGet]
        public async Task<ActionResult> EstablishAsync([FromQuery] string token)
        {
            _logger.LogDebug("Received Request to establish a web socket");
            if (!HttpContext.WebSockets.IsWebSocketRequest || string.IsNullOrEmpty(token))
            {
                _logger.LogTrace("Is not a web socket request or no token provided");
                //TODO: differentiate what is the problem
                return BadRequest();
            }

            UserContext? userContext = _tokenService.GetUserContextFromToken(token, tokenType);
            if (userContext == null)
            {
                _logger.LogTrace("Did not find any user context associated with the token");
                //TODO: differentiate what is the problem
                return Unauthorized();
            }

            using WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            _logger.LogTrace("Established a web socket");
            _socketTracker.Add(userContext.UserID, webSocket);
            do
            {
                await Task.Delay(_connectionVerificationIntervals);
            } while (!webSocket.CloseStatus.HasValue);

            _logger.LogTrace("Web Socket has been closed");
            return NoContent();
        }

        [Authorize]
        [HttpGet("token")]
        public ActionResult<string> IssueTokenForWebSocket()
        {
            UserContext userContext = _userContextAccessor.UserContext;
            _logger.LogDebug("Issuing web socket token for user {userId}", userContext.UserID);
            return _tokenService.GenerateOneTimeUserAccessToken(userContext, tokenType);
        }
    }
}
