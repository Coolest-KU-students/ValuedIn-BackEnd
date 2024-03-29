﻿using ValuedInBE.System.Security.Users;
using ValuedInBE.Users.Models;
using ValuedInBE.Users.Models.DTOs.Request;
using ValuedInBE.Users.Models.DTOs.Response;
using ValuedInBE.Users.Models.Entities;

namespace ValuedInBE.TestingConstants
{
    public static class UserConstants
    {
        public const string login = "DEFAULT";
        public const string email = "Test@Test.test";
        public const string telephone = "9945123-5554";
        public const string password = "DEFAULT";
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
        public static UserData UserInstance
        {
            get
            {
                UserData user = new()
                {
                    UserID = userId,
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
                    UserID = userId,
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
                    Login = "SYS_ADMIN",
                    Password = "SYS_ADMIN",
                    RememberMe = false
                };
            }
        }

        public static UserContext UserContextInstance
        {
            get
            {
                return new UserContext()
                {
                    Login = login,
                    Role = UserRole.DEFAULT,
                    UserID = userId
                };
            }
        }
    }
}
