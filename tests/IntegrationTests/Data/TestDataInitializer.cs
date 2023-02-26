using ValuedInBE.Models.DTOs.Requests.Users;
using ValuedInBE.Security.Users;
using ValuedInBE.Services.Users;

namespace ValuedInBETests.IntegrationTests.Data
{
    public static class TestDataInitializer
    {

        public static async Task Initialize(IAuthenticationService authenticationService)
        {
            await RegisterAllTestingUsersAsync(authenticationService);
        }


        private static async Task RegisterAllTestingUsersAsync(IAuthenticationService authenticationService)
        {
            foreach (NewUser user in GetTestingNewUserList())
            {
                await authenticationService.RegisterNewUser(user);
            }
        }

        private static List<NewUser> GetTestingNewUserList()
        {
            return new()
            {
                new()
                {
                    Login = "GetUserInfoThenUpdateInfoAndThenExpire",
                    Password = "Fake",
                    Email = "",
                    Telephone = "",
                    FirstName = "FirstName",
                    LastName = "LastName",
                    Role = UserRoleExtended.DEFAULT
                },
                new()
                {
                    Login = "SetupUser",
                    Role = UserRoleExtended.SYS_ADMIN,
                    FirstName = "Setup",
                    LastName = "User",
                    Password = "Password1",
                    Telephone = "",
                    Email = ""
                }
            };
        }

    }
}
