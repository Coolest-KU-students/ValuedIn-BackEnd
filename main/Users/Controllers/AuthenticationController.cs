using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [Authorize(Roles = "SYS_ADMIN")]
        [HttpPost("registerUser")]
        public async Task<ActionResult> RegisterUserAsync(NewUser newUser)
        {
            _logger.LogTrace("Got a request to RegisterUser with Login: {newUser.Login}; and Role: {newUser.Role}", newUser.Login, newUser.Role);
            await _authenticationService.RegisterNewUserAsync(newUser);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> SelfRegisterUserAsync(NewUser newUser) //TODO: need a differentiation
        {
            _logger.LogTrace("Got a request to SelfRegisterUser with Login: {newUser.Login}", newUser.Login);
            await _authenticationService.SelfRegisterAsync(newUser);
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
            return await _authenticationService.VerifyTokenAsync(jwtToken);
        }
    }
}
