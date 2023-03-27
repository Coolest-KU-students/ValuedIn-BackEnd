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
            List<NewUser> newUsers = new();
            newUsers.AddRange(_defaultUsers);
            newUsers.AddRange(_usersForChatTesting);
            newUsers.AddRange(_usersForUserTesting);
            return newUsers;
        }

        private static readonly List<NewUser> _defaultUsers = new()
        {
            new()
            {
                Login = "SYS_ADMIN",
                Role = UserRoleExtended.SYS_ADMIN,
                FirstName = "SYS_ADMIN",
                LastName = "SYS_ADMIN",
                Password = "SYS_ADMIN",
                Telephone = "",
                Email = ""
            },
            new()
            {
                Login = "DEFAULT",
                Role = UserRoleExtended.DEFAULT,
                FirstName = "DEFAULT",
                LastName = "DEFAULT",
                Password = "DEFAULT",
                Telephone = "",
                Email = ""
            },
            new()
            {
                Login = "ORG_ADMIN",
                Role = UserRoleExtended.ORG_ADMIN,
                FirstName = "ORG_ADMIN",
                LastName = "ORG_ADMIN",
                Password = "ORG_ADMIN",
                Telephone = "",
                Email = ""
            },
            new()
            {
                Login = "HR",
                Role = UserRoleExtended.HR,
                FirstName = "HR",
                LastName = "HR",
                Password = "HR",
                Telephone = "",
                Email = ""
            }
        };
        private static readonly List<NewUser> _usersForChatTesting = new()
        {
                new()
                {
                    Login = "ChatMessageSender",
                    Role = UserRoleExtended.DEFAULT,
                    FirstName = "ChatMessageSender",
                    LastName = "ChatMessageSender",
                    Password = "ChatMessageSender",
                    Telephone = "",
                    Email = ""
                },
                new()
                {
                    Login = "ChatMessageReceiver",
                    Role = UserRoleExtended.DEFAULT,
                    FirstName = "ChatMessageReceiver",
                    LastName = "ChatMessageReceiver",
                    Password = "ChatMessageReceiver",
                    Telephone = "",
                    Email = ""
                }
        };
        private static readonly List<NewUser> _usersForUserTesting = new()
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
                }
        };
    }
}
