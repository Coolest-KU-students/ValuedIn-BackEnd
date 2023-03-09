using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Extensions;
using System.Security.Claims;
using System.Text.Encodings.Web;
using ValuedInBE.Models;
using ValuedInBE.Security.Users;
using ValuedInBE.System;

namespace ValuedInBETests.IntegrationTests.Config
{
    public class TestAuthHandlerOptions : AuthenticationSchemeOptions
    {
        public string DefaultUserId { get; set; } = null!;
    }

    public class TestAuthHandler : AuthenticationHandler<TestAuthHandlerOptions>
    {
        public const string userId = "UserId";
        public const string authenticationScheme = "Test";
        private readonly string _defaultUserId;

        public TestAuthHandler(
            IOptionsMonitor<TestAuthHandlerOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _defaultUserId = options.CurrentValue.DefaultUserId;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string userId = _defaultUserId;
            // Extract User ID from the request headers if it exists,
            // otherwise use the default User ID from the options.
            if (Context.Request.Headers.TryGetValue(TestAuthHandler.userId, out var userIdValues))
            {
                userId = userIdValues[0];
            }
            AddUserContextIfMissing(userId);
            List<Claim> claims = new() { new Claim(ClaimTypes.Name, userId) };
            foreach (string role in GetRoleNamesBasedOnUser(userId))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, authenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, authenticationScheme);

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }

        private static List<string> GetRoleNamesBasedOnUser(string user)
        {
            UserRoleExtended userRole =
                user switch
                {
                    "SYS_ADMIN" => UserRoleExtended.SYS_ADMIN,
                    "HR" => UserRoleExtended.HR,
                    "ORG_ADMIN" => UserRoleExtended.ORG_ADMIN,
                    _ => UserRoleExtended.DEFAULT
                };

            return userRole.FlattenRoleHierarchy()
                           .Select(role => role.GetDisplayName())
                           .ToList();

        }

        private void AddUserContextIfMissing(string user)
        {
            Context.Items[UserContextMiddleware.userContextItemName] ??=
                new UserContext()
                {
                    UserID = user,
                    Login = user,
                    Role = UserRoleExtended.FromString(user) ?? UserRole.DEFAULT,
                };
        }
    }
}
