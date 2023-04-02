using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using ValuedInBE;
using ValuedInBE.System.Security.Users;
using ValuedInBE.Users.Models.DTOs.Request;
using ValuedInBE.Users.Models.DTOs.Response;
using ValuedInBETests.IntegrationTests.Config;
using Xunit;

namespace ValuedInBETests.IntegrationTests.Users
{
    public class AuthenticationIntegrationTests
        : IntegrationTestBase
    {
        private readonly string _sysAdmin = UserRoleExtended.SYS_ADMIN;
        private readonly string[] _rolesThatAreNotSysAdmin = { UserRoleExtended.DEFAULT, UserRoleExtended.HR, UserRoleExtended.ORG_ADMIN };

        private const string registerUserRoute = "/api/auth/register";
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
            newUser.Role = UserRoleExtended.SYS_ADMIN; //so that only sysadmins could register
            StringContent requestContent = SerializeIntoJsonHttpContent(newUser);
            foreach (string user in _rolesThatAreNotSysAdmin) //check if others are restricted
            {
                AddLoginHeaderToHttpClient(user);
                HttpResponseMessage httpResponse = await _client.PostAsync(registerUserRoute, requestContent);
                Assert.Equal(HttpStatusCode.Unauthorized, httpResponse.StatusCode);
                RemoveLoginHeaderFromHttpClient();
            }
            AddLoginHeaderToHttpClient(_sysAdmin);
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
            HttpResponseMessage response = await _client.PostAsync(registerUserRoute, content);
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
            requestContent = SerializeIntoJsonHttpContent(sysAdmin);
            HttpResponseMessage response = await _client.PostAsync(logInRoute, requestContent);
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response.Content);
            string responseContent = await response.Content.ReadAsStringAsync();
            TokenAndRole? tokenAndRole = JsonConvert.DeserializeObject<TokenAndRole>(responseContent);
            Assert.NotNull(tokenAndRole);
            Assert.Equal(UserRoleExtended.SYS_ADMIN, tokenAndRole!.Role);
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
