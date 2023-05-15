using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ValuedInBE.System.Security.Users;
using ValuedInBE.TestingConstants;
using ValuedInBE.Users.Models.DTOs.Request;
using ValuedInBE.Users.Models.DTOs.Response;
using ValuedInBE.Users.Services;
using Xunit;

namespace ValuedInBE.Users.Controllers.Tests
{
    public class AuthenticationControllerTests
    {
        private readonly Mock<IAuthenticationService> _mockAuthenticationService = new();
        private readonly Mock<ILogger<AuthenticationController>> _mockLogger = new();
        private const string fakeToken = "Fake token indeed";


        private AuthenticationController MockAuthenticationController()
        {
            return new(_mockAuthenticationService.Object, _mockLogger.Object);
        }

        [Fact()]
        public async Task RegisterUserShouldReturnOk()
        {
            NewUser newUser = UserConstants.NewUserInstance;
            _mockAuthenticationService.Setup(service => service.RegisterNewUserAsync(newUser)).Verifiable();
            AuthenticationController controller = MockAuthenticationController();

            ActionResult actionResult = await controller.RegisterUserAsync(newUser);
            Assert.IsType<OkResult>(actionResult);
            _mockAuthenticationService.Verify();
        }

        [Fact()]
        public async Task LogInShouldReturnJwtToken()
        {
            TokenAndRole tokenAndRole = new()
            {
                Token = fakeToken,
                Role = UserRoleExtended.DEFAULT
            };
            AuthRequest authRequest = UserConstants.AuthRequestInstance;
            _mockAuthenticationService.Setup(service => service.AuthenticateUserAsync(authRequest))
                .ReturnsAsync(tokenAndRole);
            AuthenticationController controller = MockAuthenticationController();

            ActionResult<TokenAndRole> actionResult = await controller.LogInAsync(authRequest);
            Assert.NotNull(actionResult.Value);
            Assert.Equal(fakeToken, actionResult.Value!.Token);
            Assert.Equal(UserRoleExtended.DEFAULT, actionResult.Value.Role);
        }

    }
}