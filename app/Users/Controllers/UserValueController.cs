using Microsoft.AspNetCore.Mvc;
using ValuedInBE.PersonalValues.Models.DTOs.Pocos;
using ValuedInBE.PersonalValues.Service;
using ValuedInBE.System.UserContexts.Accessors;
using ValuedInBE.Users.Models;
using ValuedInBE.Users.Services;

namespace ValuedInBE.Users.Controllers
{
    [Route("api/users/values")]
    [ApiController]
    public class UserValueController : ControllerBase 
    {
        private readonly IUserValueService _userValueService;
        private readonly IUserContextAccessor _userContextAccessor;

        public UserValueController(IUserContextAccessor userContextAccessor, IUserValueService userValueService)
        {
            _userContextAccessor = userContextAccessor;
            _userValueService = userValueService;
        }


        [HttpGet]
        public async Task<IEnumerable<IdAndValue>> GetCurrentUserValues()
        {
            UserContext userContext = _userContextAccessor.UserContext;
            return await _userValueService.GetUserValuesAsync(userContext.UserID);
        }

        [HttpPost("{valueId}")]
        public async Task AddValueToCurrentUser(long valueId)
        {
            UserContext userContext = _userContextAccessor.UserContext;
            await _userValueService.AddValueToUserAsync(userContext.UserID, valueId);
        }

        [HttpDelete("{valueId}")]
        public async Task DeleteValueFromCurrentUser(long valueId)
        {
            UserContext userContext = _userContextAccessor.UserContext;
            await _userValueService.RemoveValueFromUserAsync(userContext.UserID, valueId);
        }
    }
}
