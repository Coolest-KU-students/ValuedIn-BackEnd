using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ValuedInBE.System.DataControls.Paging;
using ValuedInBE.Users.Models.DTOs.Request;
using ValuedInBE.Users.Models.DTOs.Response;
using ValuedInBE.Users.Services;

namespace ValuedInBE.Users.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserCredentialsController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserCredentialsController> _logger;

        public UserCredentialsController(IUserService userCredentialsService, ILogger<UserCredentialsController> logger)
        {
            _userService = userCredentialsService;
            _logger = logger;
        }

        [Authorize(Roles = "SYS_ADMIN")]
        [HttpPost("page")]
        public async Task<ActionResult<Page<UserSystemInfo>>> GetUserPage(UserPageRequest pageRequest)
        {

            _logger.LogTrace("Got a request to fetch a User Page");
            return await _userService.GetUserPage(pageRequest);
        }

        [Authorize(Roles = "SYS_ADMIN")]
        [HttpGet("{login}")]
        public async Task<ActionResult<UserSystemInfo>> GetUserSystemInfo(string login)
        {
            _logger.LogTrace("Got a request to fetch a User with Login: {login}", login);
            UserSystemInfo user = await _userService.GetUserSystemInfoByLogin(login);
            return user != null ? user : NoContent();
        }

        [Authorize(Roles = "SYS_ADMIN")]
        [HttpPut]
        public async Task UpdateUser(UpdatedUser updatedUser)
        {
            _logger.LogTrace("Got a request to update a User with User Id: {updatedUser.UserID}", updatedUser.UserID);
            await _userService.UpdateUser(updatedUser);
        }

        [Authorize(Roles = "SYS_ADMIN")]
        [HttpDelete("expire/{login}")]
        public async Task ExpireUser(string login)
        {
            _logger.LogTrace("Got a request to expire a User with User Id: {login}", login);
            await _userService.ExpireUser(login);
        }
    }
}
