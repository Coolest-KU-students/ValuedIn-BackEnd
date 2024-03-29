﻿using ValuedInBE.DataControls.Memory;
using ValuedInBE.Tokens.Models;
using ValuedInBE.Users.Models;

namespace ValuedInBE.Tokens.Services
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
            _logger.LogDebug("Generating One Time {type} Token for user {user}", type, userContext.UserID);
            TokenData<UserContext> data = new()
            {
                Value = userContext,
                Type = type
            };

            string token = Guid.NewGuid().ToString();

            //Memoizes the token for the duration
            _memoizationEngine.Memoize(token, data, timeOut ?? _defaultTimeout);
            return token;
        }

        public UserContext? GetUserContextFromToken(string token, string type)
        {
            _logger.LogDebug("Extracting user from {type} token ", token);
            TokenData<UserContext>? tokenData = _memoizationEngine.Extract<string, TokenData<UserContext>>(token);
            if (tokenData?.Type == type)
            {
                return tokenData.Value;
            }
            return null;
        }
    }
}
