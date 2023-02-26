using Microsoft.AspNetCore.Mvc;
using Moq;
using ValuedInBE.DataControls.Paging;
using ValuedInBE.Models.DTOs.Requests.Users;
using ValuedInBE.Models.DTOs.Responses.Users;
using ValuedInBE.Services.Users;
using Xunit;

namespace ValuedInBE.Controllers.Tests
{
    public class UserCredentialsControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock = new();

        private UserCredentialsController MockUserCredentialsController()
        {
            return new(_userServiceMock.Object);
        }

        /*[Fact]
        public async void GetUserPageReturnsAPage()
        {
            PageConfig pageConfig = new(0, 10, new());
            Page<UserSystemInfo> pageExpected = Page<UserSystemInfo>.Empty();
            _userServiceMock
                .Setup(mock => mock.GetUserPage(pageConfig))
                .ReturnsAsync(pageExpected);

            UserCredentialsController controller = MockUserCredentialsController();
            var actionResult = await controller.GetUserPage(pageConfig);
            Assert.NotNull(actionResult);
            Page<UserSystemInfo> pageReturned = actionResult.Value ?? throw new();
            Assert.NotNull(pageReturned);
            Assert.Same(pageExpected, pageReturned);
        }*/

        [Fact]
        public async void GetUserSystemInfoByLoginReturnsObjectCorrectly()
        {
            string nonExistingLogin = "Non-ExistingLogin";
            UserSystemInfo userExpected = UserConstants.UserSystemInfoInstance;
            _userServiceMock
                .Setup(mock => mock.GetUserSystemInfoByLogin(UserConstants.login))
                .ReturnsAsync(userExpected);
            _userServiceMock
                .Setup(mock => mock.GetUserSystemInfoByLogin(nonExistingLogin))
                .ReturnsAsync(null as UserSystemInfo);


            UserCredentialsController controller = MockUserCredentialsController();
            ActionResult<UserSystemInfo> actionResultOnExisting = await controller.GetUserSystemInfo(UserConstants.login);
            Assert.NotNull(actionResultOnExisting);
            Assert.NotNull(actionResultOnExisting.Value);
            UserSystemInfo userReturned = actionResultOnExisting.Value ?? throw new();
            Assert.NotNull(userReturned);
            Assert.Same(userExpected, userReturned);

            ActionResult<UserSystemInfo> actionResulstOnNonExisting = await controller.GetUserSystemInfo(nonExistingLogin);
            Assert.NotNull(actionResulstOnNonExisting);
            Assert.Null(actionResulstOnNonExisting.Value);
            Assert.IsType<NoContentResult>(actionResulstOnNonExisting.Result);
        }

        [Fact]
        public async Task UpdateUserCallsServiceToUpdate()
        {
            UpdatedUser userExpected = UserConstants.UpdatedUserInstance;
            _userServiceMock.Setup(mock => mock.UpdateUser(userExpected)).Verifiable();

            UserCredentialsController controller = MockUserCredentialsController();
            await controller.UpdateUser(userExpected);
            _userServiceMock.Verify();
        }

        [Fact]
        public async Task ExpireUserCallsService()
        {
            UserSystemInfo userExpected = UserConstants.UserSystemInfoInstance;
            _userServiceMock.Setup(mock => mock.ExpireUser(UserConstants.login)).Verifiable();

            UserCredentialsController controller = MockUserCredentialsController();
            await controller.ExpireUser(UserConstants.login);
            _userServiceMock.Verify();
        }
    }
}