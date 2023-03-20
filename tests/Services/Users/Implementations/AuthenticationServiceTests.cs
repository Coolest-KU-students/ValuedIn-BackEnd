using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Extensions;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using ValuedInBE.System.Security.Users;
using ValuedInBE.Users.Models;
using ValuedInBE.Users.Models.DTOs.Request;
using ValuedInBE.Users.Models.DTOs.Response;
using ValuedInBE.Users.Models.Entities;
using ValuedInBE.Users.Services;
using ValuedInBE.Users.Services.Implementations;
using Xunit;

namespace ValuedInBE.Services.Users.Implementations.Tests
{
    public class AuthenticationServiceTests
    {
        private const string jwtKey = "VeryLongKeyThatIsVerySecret";
        private const string jwtIssuer = "Issuer";
        private const string jwtAudience = "Audience";
        private const int jwtExpirationInHours = 1;

        private static readonly Dictionary<string, string> _inMemoryJWTConfig = new()
        {
            {"Jwt:Key", jwtKey },
            {"Jwt:Issuer", jwtIssuer },
            {"Jwt:Audience", jwtAudience },
            {"Jwt:ExpirationInHours", jwtExpirationInHours.ToString() }
        };

        private readonly Mock<ILogger<AuthenticationService>> _loggerMock = new();
        private readonly Mock<IUserService> _userServiceMock = new();
        private readonly Mock<IPasswordHasher<User>> _hasherMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly IConfiguration _fakedConfiguration = new ConfigurationBuilder().AddInMemoryCollection(_inMemoryJWTConfig).Build();

        private AuthenticationService MockAuthenticationService()
        {
            return new(
                _loggerMock.Object,
                _userServiceMock.Object,
                _fakedConfiguration,
                _hasherMock.Object,
                _mapperMock.Object
                );
        }

        [Fact()]
        public async Task AuthenticateUserTest()
        {
            AuthRequest auth = UserConstants.AuthRequestInstance;
            UserCredentials userCredentials = UserConstants.UserCredentialsInstance;
            User user = UserConstants.UserInstance;
            JwtSecurityTokenHandler tokenHandler = new();

            _userServiceMock
                .Setup(service => service.GetUserCredentialsByLogin(UserConstants.login))
                .ReturnsAsync(userCredentials);
            _mapperMock
                .Setup(mock => mock.Map<User>(userCredentials))
                .Returns(() => user);
            _hasherMock
                .Setup(hasher => hasher.VerifyHashedPassword(user, UserConstants.hashedPassword, UserConstants.password))
                .Returns(() => PasswordVerificationResult.Success);

            AuthenticationService service = MockAuthenticationService();

            TokenAndRole tokenReturned = await service.AuthenticateUser(auth);
            Assert.True(tokenHandler.CanReadToken(tokenReturned.Token));
            Assert.Equal(tokenHandler.TokenLifetimeInMinutes / 60, jwtExpirationInHours);

            JwtSecurityToken jwt = tokenHandler.ReadJwtToken(tokenReturned.Token);
            Assert.Equal(jwt.Issuer, jwtIssuer);
            Assert.Contains(jwtAudience, jwt.Audiences);
        }

        [Fact()]
        public async Task RegisterNewUserTestAsync()
        {
            NewUser newUser = UserConstants.NewUserInstance;
            User user = UserConstants.UserInstance;
            _hasherMock
                .Setup(hasher => hasher.HashPassword(user, UserConstants.password)) //has to hash the password
                .Returns(UserConstants.hashedPassword);
            _userServiceMock
                .Setup(service => service.CreateNewUser(newUser, null))
                .Verifiable();
            _mapperMock
                .Setup(mapper => mapper.Map<User>(newUser))
                .Returns(user);
            AuthenticationService service = MockAuthenticationService();
            await service.RegisterNewUser(newUser);
            _userServiceMock.Verify();
        }

        [Fact()]
        public async Task SelfRegisterTestAsync()
        {
            NewUser newUser = UserConstants.NewUserInstance;
            User user = UserConstants.UserInstance;
            _hasherMock
                .Setup(hasher => hasher.HashPassword(user, UserConstants.password)) //has to hash the password
                .Returns(UserConstants.hashedPassword);
            _userServiceMock
                .Setup(service => service.CreateNewUser(newUser, It.IsAny<UserContext>()))
                .Verifiable();
            _mapperMock
                .Setup(mapper => mapper.Map<User>(newUser))
                .Returns(user);

            AuthenticationService service = MockAuthenticationService();
            await Assert.ThrowsAnyAsync<Exception>(() => service.SelfRegister(CreateNewUserWithOnlyRole(UserRole.SYS_ADMIN)));
            await Assert.ThrowsAnyAsync<Exception>(() => service.SelfRegister(CreateNewUserWithOnlyRole(UserRole.ORG_ADMIN)));
            await Assert.ThrowsAnyAsync<Exception>(() => service.SelfRegister(CreateNewUserWithOnlyRole(UserRole.HR)));
            _userServiceMock.VerifyNoOtherCalls();

            await service.SelfRegister(newUser);
            _userServiceMock.Verify();

        }

        private static NewUser CreateNewUserWithOnlyRole(UserRole role)
            => new()
            {
                Role = role.GetDisplayName()
            };
    }
}