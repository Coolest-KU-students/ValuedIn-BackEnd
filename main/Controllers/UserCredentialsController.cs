using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ValuedInBE.DataControls.Paging;
using ValuedInBE.Models.DTOs.Requests.Users;
using ValuedInBE.Models.DTOs.Responses.Users;
using ValuedInBE.Services.Users;

namespace ValuedInBE.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserCredentialsController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserCredentialsController(IUserService userCredentialsService)
        {
            _userService = userCredentialsService;
        }

        [Authorize(Roles = "SYS_ADMIN")]
        [HttpPost("page")]
        public async Task<ActionResult<Page<UserSystemInfo>>> GetUserPage(UserPageRequest pageRequest)
        {
            return await _userService.GetUserPage(pageRequest);
        }

        [Authorize(Roles = "SYS_ADMIN")]
        [HttpGet("{login}")]
        public async Task<ActionResult<UserSystemInfo>> GetUserSystemInfo(string login)
        {
            UserSystemInfo user = await _userService.GetUserSystemInfoByLogin(login);
            return user != null ? user : NoContent();
        }

        [Authorize(Roles = "SYS_ADMIN")]
        [HttpPut]
        public async Task UpdateUser(UpdatedUser updatedUser)
        {
            await _userService.UpdateUser(updatedUser);
        }

        [Authorize(Roles = "SYS_ADMIN")]
        [HttpDelete("expire/{login}")]
        public async Task ExpireUser(string login)
        {
            await _userService.ExpireUser(login);
        }
    }
}
