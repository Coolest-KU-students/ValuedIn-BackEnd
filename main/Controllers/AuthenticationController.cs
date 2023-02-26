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

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [Authorize(Roles = "SYS_ADMIN")]
        [HttpPost("registerUser")]
        public async Task<ActionResult> RegisterUser(NewUser newUser)
        {
            await _authenticationService.RegisterNewUser(newUser);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> SelfRegisterUser(NewUser newUser) //TODO: need a differentiation
        {
            await _authenticationService.SelfRegister(newUser);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<TokenAndRole>> LogIn(AuthRequest authRequest)
        {
            return await _authenticationService.AuthenticateUser(authRequest);
        }

        [HttpGet]
        public async Task<ActionResult<TokenAndRole>> CheckAuthentication()
        {
            string jwtToken = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            return await _authenticationService.VerifyToken(jwtToken);
        }
    }
}
