using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using ValuedInBE;
using ValuedInBE.Models.DTOs.Requests.Users;
using ValuedInBE.Models.DTOs.Responses.Authentication;
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

        private const string registerUserRoute = "/api/auth/registerUser";
        private const string selfRegisterRoute = "/api/auth/register";
        private const string logInRoute = "/api/auth/login";
        private const string reAuthRoute = "/api/auth";

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
            Task<bool> credentialsExistTask = _valuedInContext.UserCredentials.AnyAsync(creds => creds.Login == newUser.Login && creds.UserDetails != null);
            Assert.True(await credentialsExistTask);
        }

        [Fact]
        public async Task SelfRegisteringUser()
        {
            NewUser newUser = UserConstants.NewUserInstance;
            newUser.Login = "SelfRegisteringUser"; //unique name
            StringContent content = SerializeIntoJsonHttpContent(newUser);
            HttpResponseMessage response = await _client.PostAsync(selfRegisterRoute, content);
            Assert.True(response.IsSuccessStatusCode, $"Status code should be ok, but is: {response.StatusCode}");
            Task<bool> credentialsExistTask = _valuedInContext.UserCredentials.AnyAsync(creds => creds.Login == newUser.Login && creds.UserDetails != null);
            Assert.True(await credentialsExistTask);
        }

        [Fact]
        public async Task LoggingInReturnsJwtOnSuccessAndThenUseItToReauthenticate()
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
            string responseContent = await response.Content.ReadAsStringAsync();
            TokenAndRole? tokenAndRole = JsonConvert.DeserializeObject<TokenAndRole>(responseContent);
            Assert.NotNull(tokenAndRole);
            Assert.Equal(tokenAndRole!.Role, UserRoleExtended.DEFAULT);
            Assert.NotNull(tokenAndRole.Token);

            await Task.Delay(1000); //delaying so the expiration date is changed

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAndRole.Token);
            response = await _client.GetAsync(reAuthRoute);
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response.Content);
            responseContent = await response.Content.ReadAsStringAsync();
            TokenAndRole? reAuthTokenAndRole = JsonConvert.DeserializeObject<TokenAndRole>(responseContent);
            Assert.NotNull(reAuthTokenAndRole);
            Assert.Equal(tokenAndRole.Role, reAuthTokenAndRole!.Role);
            Assert.NotEqual(tokenAndRole.Token, reAuthTokenAndRole.Token);
        }
    }
}
