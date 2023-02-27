using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ValuedInBE.Models.DTOs.Requests.Users;
using ValuedInBE.Models.DTOs.Responses.Authentication;
using ValuedInBE.Security.Users;
using ValuedInBE.Services.Users;
using Xunit;

namespace ValuedInBE.Controllers.Tests
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
        public async void RegisterUserShouldReturnOk()
        {
            NewUser newUser = UserConstants.NewUserInstance;
            _mockAuthenticationService.Setup(service => service.RegisterNewUser(newUser)).Verifiable();
            AuthenticationController controller = MockAuthenticationController();

            ActionResult actionResult = await controller.RegisterUser(newUser);
            Assert.IsType<OkResult>(actionResult);
            _mockAuthenticationService.Verify();
        }

        [Fact()]
        public async Task SelfRegisterUserShouldReturnOk()
        {
            NewUser newUser = UserConstants.NewUserInstance;
            _mockAuthenticationService.Setup(service => service.SelfRegister(newUser)).Verifiable();
            AuthenticationController controller = MockAuthenticationController();

            ActionResult actionResult = await controller.SelfRegisterUser(newUser);
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
            _mockAuthenticationService.Setup(service => service.AuthenticateUser(authRequest))
                .ReturnsAsync(tokenAndRole);
            AuthenticationController controller = MockAuthenticationController();

            ActionResult<TokenAndRole> actionResult = await controller.LogIn(authRequest);
            Assert.NotNull(actionResult.Value);
            Assert.Equal(fakeToken, actionResult.Value!.Token);
            Assert.Equal(UserRoleExtended.DEFAULT, actionResult.Value.Role);
        }

    }
}