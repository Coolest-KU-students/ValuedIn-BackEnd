using Microsoft.OpenApi.Extensions;
using Newtonsoft.Json;
using System.Net;
using ValuedInBE;
using ValuedInBE.DataControls.Paging;
using ValuedInBE.System.DataControls.Paging;
using ValuedInBE.System.Security.Users;
using ValuedInBE.Users.Models.DTOs.Request;
using ValuedInBE.Users.Models.DTOs.Response;
using ValuedInBETests.IntegrationTests.Config;
using Xunit;

namespace ValuedInBETests.IntegrationTests.Users
{
    public class UserAPIIntegrationTests : IntegrationTestBase
    {
        private const string userPageRoute = "/api/users/page";
        private const string getByLoginRoute = "/api/users/{login}";
        private const string updateUserRoute = "/api/users";
        private const string expireUserRoute = "/api/users/expire/{login}";
        private const string updateTargetUser = "GetUserInfoThenUpdateInfoAndThenExpire";

        private static readonly string[] _rolesThatAreNotSysAdmin =
            UserRoleExtended.ExtendedRoles
            .Where(role => role != UserRoleExtended.SYS_ADMIN)
            .Select(role => role.ToString()).ToArray();

        private static readonly string _sysAdminRoleName = UserRole.SYS_ADMIN.GetDisplayName();

        public UserAPIIntegrationTests(IntegrationTestWebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetUserPageOnlyAsSysAdmin()
        {
            PageConfig pageConfig = new() { 
                Page = 0, 
                Size = 10, 
                OrderByColumns = new() 
            };
            StringContent requestContent = SerializeIntoJsonHttpContent(pageConfig);

            foreach (string role in _rolesThatAreNotSysAdmin) //Check that nobody else has access
            {
                AddLoginHeaderToHttpClient(role);
                HttpResponseMessage httpResponse = await _client.PostAsync(userPageRoute, requestContent);
                Assert.Equal(HttpStatusCode.Forbidden, httpResponse.StatusCode);
                RemoveLoginHeaderFromHttpClient();
            }

            AddLoginHeaderToHttpClient(_sysAdminRoleName);
            HttpResponseMessage response = await _client.PostAsync(userPageRoute, requestContent);
            Assert.NotNull(response);

            string responseContent = await response.Content.ReadAsStringAsync();
            Page<UserSystemInfo>? usersPage = JsonConvert.DeserializeObject<Page<UserSystemInfo>>(responseContent);
            Assert.NotNull(usersPage);
            Assert.NotEmpty(usersPage?.Results);
        }

        [Fact]
        public async Task GetUserInfoThenUpdateInfoAndThenExpire()
        {
            string targetLogin = updateTargetUser;
            string getLoginPath = getByLoginRoute.Replace("{login}", targetLogin);
            string updateUserPath = updateUserRoute;
            string expireUserRoute = UserAPIIntegrationTests.expireUserRoute.Replace("{login}", targetLogin);
            string updatedSuffix = "_updated";

            foreach (string role in _rolesThatAreNotSysAdmin) //Check that nobody else has access
            {
                AddLoginHeaderToHttpClient(role);
                HttpResponseMessage httpResponse = await _client.GetAsync(getLoginPath);
                Assert.Equal(HttpStatusCode.Forbidden, httpResponse.StatusCode);
                RemoveLoginHeaderFromHttpClient();
            }

            AddLoginHeaderToHttpClient(_sysAdminRoleName);

            HttpResponseMessage getUserResponse = await _client.GetAsync(getLoginPath);
            Assert.NotNull(getUserResponse);

            string getUserResponseContent = await getUserResponse.Content.ReadAsStringAsync();
            UserSystemInfo? user = JsonConvert.DeserializeObject<UserSystemInfo>(getUserResponseContent);
            Assert.NotNull(user);
            Assert.Equal(user!.Login, targetLogin);
            Assert.False(user.IsExpired);

            RemoveLoginHeaderFromHttpClient(); ; //reset
            UpdatedUser updatedUser = new()
            {
                UserID = user.UserID,
                Email = user.Email,
                Role = user.Role,
                Telephone = user.Telephone,
                FirstName = user.FirstName,
                LastName = user.LastName + updatedSuffix //update last name
            };

            StringContent requestContent = SerializeIntoJsonHttpContent(updatedUser);
            foreach (string role in _rolesThatAreNotSysAdmin) //Check that nobody else has access
            {
                AddLoginHeaderToHttpClient(role);
                HttpResponseMessage httpResponse = await _client.PutAsync(updateUserPath, requestContent);
                Assert.Equal(HttpStatusCode.Forbidden, httpResponse.StatusCode);
                RemoveLoginHeaderFromHttpClient();
            }

            AddLoginHeaderToHttpClient(_sysAdminRoleName);
            HttpResponseMessage updateUserResponse = await _client.PutAsync(updateUserPath, requestContent);
            Assert.True(updateUserResponse.IsSuccessStatusCode);
            Assert.True(
                _valuedInContext.UserDetails
                    .Where(details => details.UserID == updatedUser.UserID //The same login has the updated lastName
                                && details.LastName == updatedUser.LastName)
                    .Any()
                );

            RemoveLoginHeaderFromHttpClient(); ; //reset
            foreach (string role in _rolesThatAreNotSysAdmin) //Check that nobody else has access
            {
                AddLoginHeaderToHttpClient(role);
                HttpResponseMessage httpResponse = await _client.DeleteAsync(expireUserRoute);
                Assert.Equal(HttpStatusCode.Forbidden, httpResponse.StatusCode);
                RemoveLoginHeaderFromHttpClient();
            }

            AddLoginHeaderToHttpClient(_sysAdminRoleName);
            HttpResponseMessage expirationResponse = await _client.DeleteAsync(expireUserRoute);
            Assert.True(expirationResponse.IsSuccessStatusCode);
            Assert.True(
                _valuedInContext.UserCredentials
                    .Where(creds => creds.Login == targetLogin //The same login has the updated lastName
                                && creds.IsExpired)
                    .Any()
                );
        }

    }
}
