using AutoMapper;
using ValuedInBE.System.UserContexts.Accessors;
using ValuedInBE.Users.Models;
using ValuedInBE.Users.Models.Entities;
using ValuedInBE.Users.Services;

namespace ValuedInBE.System.WebConfigs.Middleware
{
    public class UserContextMiddleware : IMiddleware
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;
        private readonly IUserContextAccessor _userContextAccessor;

        public UserContextMiddleware(IAuthenticationService authenticationService, IMapper mapper, IUserContextAccessor userContextAccessor)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
            _userContextAccessor = userContextAccessor;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string jwtToken = context.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                try
                {
                    UserCredentials? credentials = await _authenticationService.GetUserFromTokenAsync(jwtToken);
                    if (credentials != null) { 
                        UserContext user = _mapper.Map<UserContext>(credentials);
                        _userContextAccessor.UserContext = user;
                    }
                }
                catch (Exception)
                {
                    //TODO: log this?
                }
            }
            await next.Invoke(context);
        }
    }
}
