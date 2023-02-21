using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using ValuedInBE;
using ValuedInBE.Models.DTOs.Requests.Users;
using ValuedInBE.Security.Users;
using ValuedInBETests.IntegrationTests.Config;
using Xunit;

namespace ValuedInBETests.IntegrationTests.Users
{
    public class AuthenticationIntegrationTests
        : IntegrationTestBase
    {
        private readonly string _sysAdmin = UserRoleExtended.SYS_ADMIN;
        private readonly string[] _rolesThatAreNotSysAdmin =
            UserRoleExtended.ExtendedRoles
            .Where(role => role != UserRoleExtended.SYS_ADMIN)
            .Select(role => role.ToString()).ToArray();

        private const string registerUserRoute = "/api/registerUser";
        private const string selfRegisterRoute = "/api/register";
        private const string logInRoute = "/api/login";

        public AuthenticationIntegrationTests(IntegrationTestWebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task RegisteringUserAsSysAdmin()
        {
            NewUser newUser = UserConstants.NewUserInstance;
            newUser.Login = "RegisteringUserAsSysAdmin"; //unique name
            StringContent requestContent = SerializeIntoJsonHttpContent(newUser);
            foreach (string user in _rolesThatAreNotSysAdmin) //check if others are restricted
            {
                AddUserIdToClient(user);
                HttpResponseMessage httpResponse = await _client.PostAsync(registerUserRoute, requestContent);
                Assert.Equal(System.Net.HttpStatusCode.Forbidden, httpResponse.StatusCode);
                RemoveUserIdFromClient();
            }
            AddUserIdToClient(_sysAdmin);
            HttpResponseMessage response = await _client.PostAsync(registerUserRoute, requestContent);
            Assert.True(response.IsSuccessStatusCode);
            Task<bool> credentialsExistTask = _valuedInContext.UserCredentials.AnyAsync(creds => creds.Login == newUser.Login);
            Task<bool> detailsExistTask = _valuedInContext.UserDetails.AnyAsync(details => details.Login == newUser.Login);
            Assert.True(await credentialsExistTask);
            Assert.True(await detailsExistTask);
        }

        [Fact]
        public async Task SelfRegisteringUser()
        {
            NewUser newUser = UserConstants.NewUserInstance;
            newUser.Login = "SelfRegisteringUser"; //unique name
            StringContent content = SerializeIntoJsonHttpContent(newUser);
            HttpResponseMessage response = await _client.PostAsync(selfRegisterRoute, content);
            Assert.True(response.IsSuccessStatusCode, $"Status code should be ok, but is: {response.StatusCode}");
            Task<bool> credentialsExistTask = _valuedInContext.UserCredentials.AnyAsync(creds => creds.Login == newUser.Login);
            Task<bool> detailsExistTask = _valuedInContext.UserDetails.AnyAsync(details => details.Login == newUser.Login);
            Assert.True(await credentialsExistTask);
            Assert.True(await detailsExistTask);
        }

        [Fact]
        public async Task LoggingInReturnsJwtOnSuccess()
        {
            AuthRequest incorrectCredentials = new() { Login = "Fake", Password = "Fake", RememberMe = false };
            StringContent requestContent = SerializeIntoJsonHttpContent(incorrectCredentials);
            HttpResponseMessage badResponse = await _client.PostAsync(logInRoute, requestContent);
            Assert.False(badResponse.IsSuccessStatusCode);

            AuthRequest sysAdmin = UserConstants.SysAdminAuthRequestInstance;
            JwtSecurityTokenHandler tokenHandler = new();
            requestContent = SerializeIntoJsonHttpContent(sysAdmin);
            HttpResponseMessage response = await _client.PostAsync(logInRoute, requestContent);
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response.Content);
            string responceContent = await response.Content.ReadAsStringAsync();
            Assert.True(tokenHandler.CanReadToken(responceContent)); //unit tests check if the token has correct metadata
        }



    }
}
