using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ValuedInBE.System.DataControls.Paging;
using ValuedInBE.Users.Controllers;
using ValuedInBE.Users.Models.DTOs.Request;
using ValuedInBE.Users.Models.DTOs.Response;
using ValuedInBE.Users.Services;
using Xunit;

namespace ValuedInBE.Controllers.Tests
{
    public class UserCredentialsControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock = new();
        private readonly Mock<ILogger<UserCredentialsController>> _logger = new();

        private UserCredentialsController MockUserCredentialsController()
        {
            return new(_userServiceMock.Object, _logger.Object);
        }

        [Fact]
        public async void GetUserPageReturnsAPage()
        {
            UserPageRequest userPageRequest = new(0, 10, new(), true);
            Page<UserSystemInfo> pageExpected = Page<UserSystemInfo>.Empty();
            _userServiceMock
                .Setup(mock => mock.GetUserPageAsync(userPageRequest))
                .ReturnsAsync(pageExpected);

            UserCredentialsController controller = MockUserCredentialsController();
            var actionResult = await controller.GetUserPageAsync(userPageRequest);
            Assert.NotNull(actionResult);
            Page<UserSystemInfo> pageReturned = actionResult.Value ?? throw new();
            Assert.NotNull(pageReturned);
            Assert.Same(pageExpected, pageReturned);
        }

        [Fact]
        public async void GetUserSystemInfoByLoginReturnsObjectCorrectly()
        {
            string nonExistingLogin = "Non-ExistingLogin";
            UserSystemInfo userExpected = UserConstants.UserSystemInfoInstance;
            _userServiceMock
                .Setup(mock => mock.GetUserSystemInfoByLoginAsync(UserConstants.login))
                .ReturnsAsync(userExpected);
            _userServiceMock
                .Setup(mock => mock.GetUserSystemInfoByLoginAsync(nonExistingLogin))
                .ReturnsAsync(null as UserSystemInfo);


            UserCredentialsController controller = MockUserCredentialsController();
            ActionResult<UserSystemInfo> actionResultOnExisting = await controller.GetUserSystemInfoAsync(UserConstants.login);
            Assert.NotNull(actionResultOnExisting);
            Assert.NotNull(actionResultOnExisting.Value);
            UserSystemInfo userReturned = actionResultOnExisting.Value ?? throw new();
            Assert.NotNull(userReturned);
            Assert.Same(userExpected, userReturned);

            ActionResult<UserSystemInfo> actionResulstOnNonExisting = await controller.GetUserSystemInfoAsync(nonExistingLogin);
            Assert.NotNull(actionResulstOnNonExisting);
            Assert.Null(actionResulstOnNonExisting.Value);
            Assert.IsType<NoContentResult>(actionResulstOnNonExisting.Result);
        }

        [Fact]
        public async Task UpdateUserCallsServiceToUpdate()
        {
            UpdatedUser userExpected = UserConstants.UpdatedUserInstance;
            _userServiceMock.Setup(mock => mock.UpdateUserAsync(userExpected)).Verifiable();

            UserCredentialsController controller = MockUserCredentialsController();
            await controller.UpdateUserAsync(userExpected);
            _userServiceMock.Verify();
        }

        [Fact]
        public async Task ExpireUserCallsService()
        {
            UserSystemInfo userExpected = UserConstants.UserSystemInfoInstance;
            _userServiceMock.Setup(mock => mock.ExpireUserAsync(UserConstants.login)).Verifiable();

            UserCredentialsController controller = MockUserCredentialsController();
            await controller.ExpireUserAsync(UserConstants.login);
            _userServiceMock.Verify();
        }
    }
}