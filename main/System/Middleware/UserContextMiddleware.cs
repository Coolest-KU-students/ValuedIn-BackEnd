using AutoMapper;
using Newtonsoft.Json.Linq;
using ValuedInBE.Models;
using ValuedInBE.Models.Users;
using ValuedInBE.Repositories.Database;
using ValuedInBE.Services.Users;

namespace ValuedInBE.System.Middleware
{
    public class UserContextMiddleware : IMiddleware
    {
        public const string userContextItemName = "UserContext";
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public UserContextMiddleware(IAuthenticationService authenticationService, IMapper mapper)
        {
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
        public static UserContext GetUserContext(this HttpContext context)
        {
            return (UserContext)context.Items[UserContextMiddleware.userContextItemName];
        }
    }

}
