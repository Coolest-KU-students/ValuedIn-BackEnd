using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using ValuedInBE.Models.DTOs.Requests;
using ValuedInBE.Models.Users;
using ValuedInBE.Repositories;
using ValuedInBE.Security.Users;
using ValuedInBE.Services.Users;

namespace ValuedInBE.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IUserService userService, IAuthenticationService authenticationService)
        {
            _userService = userService;
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
            await _userService.CreateNewUser(newUser);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> LogIn(AuthRequest authRequest)
        {
            string token = await _authenticationService.AuthenticateUser(authRequest);
            return token;
        }
    }
}
