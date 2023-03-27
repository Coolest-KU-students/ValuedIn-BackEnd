using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using ValuedInBE.System.Security.Users;
using ValuedInBE.System.WebConfigs.Middleware;
using ValuedInBE.Users.Models;
using ValuedInBE.Users.Models.DTOs.Request;
using ValuedInBE.Users.Models.DTOs.Response;
using ValuedInBE.Users.Services;

namespace ValuedInBE.Users.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthenticationService authenticationService, ILogger<AuthenticationController> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> RegisterUserAsync(NewUser newUser) 
        {
            _logger.LogTrace("Got a request to register Login: {newUser.Login}", newUser.Login);
            try
            {
                await _authenticationService.RegisterNewUserAsync(newUser);
            }
            catch (Exception ex) // TODO: should be auth exception
            {
                return Unauthorized(ex.Message);
            }
            
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<TokenAndRole>> LogInAsync(AuthRequest authRequest)
        {
            _logger.LogTrace("Got a request to log in from: {authRequest.Login}", authRequest.Login);
            return await _authenticationService.AuthenticateUserAsync(authRequest);
        }

        [HttpGet]
        public async Task<ActionResult<TokenAndRole>> CheckAuthenticationAsync()
        {
            string jwtToken = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            _logger.LogTrace("Got a request to check Authentication with token: {jwtToken}", jwtToken);
            return await _authenticationService.VerifyAndIssueNewTokenAsync(jwtToken);
        }
    }
}
