using AutoMapper;
using Newtonsoft.Json.Linq;
using ValuedInBE.Models;
using ValuedInBE.Models.Users;
using ValuedInBE.Repositories.Database;
using ValuedInBE.Services.Users;

namespace ValuedInBE.System
{
    public class UserContextMiddleware : IMiddleware
    {
        public const string userContextItemName = "UserContext";
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public UserContextMiddleware(IUserService userService, IAuthenticationService authenticationService, IMapper mapper)
        {
            _userService = userService;
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string jwtToken = context.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(jwtToken)) return;
            UserCredentials credentials = await _authenticationService.GetUserFromToken(jwtToken);
            UserContext user = _mapper.Map<UserContext>(credentials);
            context.Items[userContextItemName] = user;
        }
    }

    public static class UserContextMiddlewareExtensions
    {
        public static UserContext GetUserFromRequestContext(this HttpContext context)
        {
            return (UserContext)context.Items[UserContextMiddleware.userContextItemName];
        }
    }
    
}
