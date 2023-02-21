using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Extensions;
using System.Security.Claims;
using System.Text.Encodings.Web;
using ValuedInBE.Security.Users;

namespace ValuedInBETests.IntegrationTests.Config
{
    public class TestAuthHandlerOptions : AuthenticationSchemeOptions
    {
        public string DefaultUserId { get; set; } = null!;
    }

    public class TestAuthHandler : AuthenticationHandler<TestAuthHandlerOptions>
    {
        public const string UserId = "UserId";

        public const string AuthenticationScheme = "Test";
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
            if (Context.Request.Headers.TryGetValue(UserId, out var userIdValues))
            {
                userId = userIdValues[0];
            }

            List<Claim> claims = new() { new Claim(ClaimTypes.Name, userId) };
            foreach (string role in GetRoleNamesBasedOnUser(userId))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

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
    }
}
