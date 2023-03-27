using AutoMapper;
using ValuedInBE.Users.Models;
using ValuedInBE.Users.Models.Entities;
using ValuedInBE.Users.Services;

namespace ValuedInBE.System.WebConfigs.Middleware
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
            if (!string.IsNullOrEmpty(jwtToken))
            {
                try
                {
                    UserCredentials credentials = await _authenticationService.GetUserFromTokenAsync(jwtToken);
                    UserContext user = _mapper.Map<UserContext>(credentials);
                    context.Items[userContextItemName] = user;
                }
                catch (Exception)
                {
                    //TODO: log this?
                }
            }
            await next.Invoke(context);
        }
    }

    public static class UserContextMiddlewareExtensions
    {
        public static UserContext GetUserContext(this HttpContext context)
        {
            return (UserContext)context.Items[UserContextMiddleware.userContextItemName] ?? null;
        }
    }
}
