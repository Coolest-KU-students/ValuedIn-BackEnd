using ValuedInBE.Models;
using ValuedInBE.Models.DTOs.Requests.Users;
using ValuedInBE.Models.DTOs.Responses.Users;
using ValuedInBE.Models.Users;
using ValuedInBE.Security.Users;

namespace ValuedInBE
{
    public static class UserConstants
    {
        public const string login = "Test";
        public const string email = "Test@Test.test";
        public const string telephone = "9945123-5554";
        public const string password = "Password";
        public const string hashedPassword = "This is Hashed";
        public const string userId = "This is faked";

        public static UserSystemInfo UserSystemInfoInstance
        {
            get
            {
                return new()
                {
                    Login = login,
                    UserID = userId,
                    IsExpired = false,
                    LastActive = DateTime.Now,
                    Role = UserRoleExtended.DEFAULT,
                    FirstName = login,
                    LastName = login,
                    Email = email,
                    Telephone = telephone
                };
            }
        }
        public static UpdatedUser UpdatedUserInstance
        {
            get
            {
                UpdatedUser user = new()
                {
                    UserID = userId,
                    Role = UserRoleExtended.DEFAULT,
                    FirstName = login,
                    LastName = login,
                    Email = email,
                    Telephone = telephone
                };
                return user;
            }
        }
        public static NewUser NewUserInstance
        {
            get
            {
                return new()
                {
                    Login = login,
                    Role = UserRoleExtended.DEFAULT,
                    FirstName = login,
                    LastName = login,
                    Password = password,
                    Email = email,
                    Telephone = telephone
                };
            }
        }
        public static User UserInstance
        {
            get
            {
                User user = new()
                {
                    Login = login,
                    Role = UserRole.DEFAULT,
                    Password = password
                };
                return user;
            }
        }
        public static AuthRequest AuthRequestInstance
        {
            get
            {
                AuthRequest request = new()
                {
                    Login = login,
                    Password = password,
                    RememberMe = false
                };
                return request;
            }
        }
        public static UserDetails UserDetailsInstance
        {
            get
            {
                return new UserDetails()
                {
                    UserID = userId,
                    FirstName = login,
                    LastName = login,
                    Email = email,
                    Telephone = telephone
                };
            }
        }
        public static UserCredentials UserCredentialsInstance
        {
            get
            {
                return new UserCredentials()
                {
                    Login = login,
                    Password = hashedPassword,
                    IsExpired = false,
                    LastActive = DateTime.Now,
                    Role = UserRole.DEFAULT,
                    UserDetails = UserDetailsInstance
                };
            }
        }
        public static AuthRequest SysAdminAuthRequestInstance
        {
            get
            {
                return new()
                {
                    Login = "SetupUser",
                    Password = "Password1",
                    RememberMe = false
                };
            }
        }
    }
}
