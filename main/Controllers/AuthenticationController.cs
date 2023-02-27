using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ValuedInBE.Models.DTOs.Requests.Users;
using ValuedInBE.Models.DTOs.Responses.Authentication;
using ValuedInBE.Services.Users;

namespace ValuedInBE.Controllers
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
        public async Task<ActionResult> RegisterUser(NewUser newUser)
        {
            _logger.LogTrace("Got a request to RegisterUser with Login: {newUser.Login}; and Role: {newUser.Role}", newUser.Login, newUser.Role);
            await _authenticationService.RegisterNewUser(newUser);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> SelfRegisterUser(NewUser newUser) //TODO: need a differentiation
        {
            _logger.LogTrace("Got a request to SelfRegisterUser with Login: {newUser.Login}", newUser.Login);
            await _authenticationService.SelfRegister(newUser);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<TokenAndRole>> LogIn(AuthRequest authRequest)
        {
            _logger.LogTrace("Got a request to log in from: {authRequest.Login}", authRequest.Login);
            return await _authenticationService.AuthenticateUser(authRequest);
        }

        [HttpGet]
        public async Task<ActionResult<TokenAndRole>> CheckAuthentication()
        {
            string jwtToken = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            _logger.LogTrace("Got a request to check Authentication with token: {jwtToken}", jwtToken);
            return await _authenticationService.VerifyToken(jwtToken);
        }
    }
}
