using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Extensions;
using System.Security.Claims;
using System.Text.Encodings.Web;
using ValuedInBE.System.Security.Users;
using ValuedInBE.System.WebConfigs.Middleware;
using ValuedInBE.Users.Models;
using ValuedInBE.Users.Models.Entities;
using ValuedInBE.Users.Repositories;

namespace ValuedInBETests.IntegrationTests.Config
{
    public class TestAuthHandlerOptions : AuthenticationSchemeOptions
    {
        public string DefaultLogin { get; set; } = null!;
    }

    public class TestAuthHandler : AuthenticationHandler<TestAuthHandlerOptions>
    {
        public const string userLoginHeader = "Login";
        public const string authenticationScheme = "Test";
        private readonly string _defaultLogin;
        private readonly IUserCredentialRepository _userCredentialRepository; //Needed to fake user context

        public TestAuthHandler(
            IOptionsMonitor<TestAuthHandlerOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserCredentialRepository userCredentialRepository) : base(options, logger, encoder, clock)
        {
            _defaultLogin = options.CurrentValue.DefaultLogin;
            _userCredentialRepository = userCredentialRepository;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
           string login = _defaultLogin;
            
            if (Context.Request.Headers.TryGetValue(userLoginHeader, out var userIdValues))
            {
                //we're gonna assume a single value until something that requires more comes along
                login = userIdValues[0]; 
            }

            UserCredentials userCredentials =
                await _userCredentialRepository.GetByLoginWithDetailsAsync(login)
                ?? throw new Exception($"Did not find user with login {login}");

            AddUserContextIfMissing(userCredentials);

            List<Claim> claims = new() { new Claim(ClaimTypes.Name, login) };
            foreach (string role in GetRoleNamesBasedOnUser(userCredentials))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, authenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, authenticationScheme);
            var result = AuthenticateResult.Success(ticket);
            return result;
        }

        private static List<string> GetRoleNamesBasedOnUser(UserCredentials user)
        {
            return UserRoleExtended
                    .GetExtended(user.Role)
                    .FlattenRoleHierarchy()
                    .Select(role => role.ToString())
                    .ToList();
        }

        private void AddUserContextIfMissing(UserCredentials credentials)
        {
            Context.Items[UserContextMiddleware.userContextItemName] ??=
                new UserContext()
                {
                    UserID = credentials.UserID,
                    Login = credentials.Login,
                    Role = credentials.Role,
                };
        }

        
    }
}
