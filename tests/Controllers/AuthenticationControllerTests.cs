using Microsoft.AspNetCore.Mvc;
using Moq;
using ValuedInBE.Models.DTOs.Requests.Users;
using ValuedInBE.Services.Users;
using Xunit;

namespace ValuedInBE.Controllers.Tests
{
    public class AuthenticationControllerTests
    {
        private readonly Mock<IAuthenticationService> _mockAuthenticationService = new();

        private AuthenticationController MockAuthenticationController()
        {
            return new(_mockAuthenticationService.Object);
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
            string fakeToken = "Fake token indeed";
            AuthRequest authRequest = UserConstants.AuthRequestInstance;
            _mockAuthenticationService.Setup(service => service.AuthenticateUser(authRequest))
                .ReturnsAsync(fakeToken);
            AuthenticationController controller = MockAuthenticationController();

            ActionResult<string> actionResult = await controller.LogIn(authRequest);
            Assert.NotNull(actionResult.Value);
            Assert.Equal(fakeToken, actionResult.Value);
        }
    }
}