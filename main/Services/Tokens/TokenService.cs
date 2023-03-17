using ValuedInBE.DataControls.Memory;
using ValuedInBE.Models;

namespace ValuedInBE.Services.Tokens
{
    public class TokenService : ITokenService
    {
        private readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(30);
        private readonly ILogger<TokenService> _logger;
        private readonly IMemoizationEngine _memoizationEngine;

        public TokenService(ILogger<TokenService> logger, IMemoizationEngine memoizationEngine)
        {
            _logger = logger;
            _memoizationEngine = memoizationEngine;
        }


        public string GenerateOneTimeUserAccessToken(UserContext userContext, string type, TimeSpan? timeOut = null)
        {
            TokenData<UserContext> data = new()
            {
                Value = userContext,
                Type = type
            };

            string token = Guid.NewGuid().ToString();

            _memoizationEngine.Memoize(token, data, timeOut ?? _defaultTimeout);
            return token;
        }

        public UserContext? GetUserContextFromToken(string token, string type)
        {
            TokenData<UserContext> tokenData = _memoizationEngine.TryGetAndRemove<string, TokenData<UserContext>>(token);
            if(tokenData?.Type == type) 
            {
                return tokenData.Value;
            }
            return null;
        }
    }
}
