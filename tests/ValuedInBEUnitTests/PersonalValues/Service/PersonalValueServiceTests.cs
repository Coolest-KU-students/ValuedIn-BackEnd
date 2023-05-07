using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using ValuedInBE.PersonalValues.Exceptions;
using ValuedInBE.PersonalValues.Models.DTOs.Requests;
using ValuedInBE.PersonalValues.Models.DTOs.Response;
using ValuedInBE.PersonalValues.Models.Entities;
using ValuedInBE.PersonalValues.Repositories;
using ValuedInBE.PersonalValues.Service;
using ValuedInBE.System.UserContexts.Accessors;
using ValuedInBE.TestingConstants;
using ValuedInBE.Users.Models;
using ValuedInBE.Users.Models.Entities;
using Xunit;

namespace ValuedInBE.PersonalValues.Tests.Service
{
    public class PersonalValueServiceTests
    {
        private readonly Mock<IPersonalValuesRepository> _personalValuesRepositoryMock = new();
        private readonly Mock<IUserContextAccessor> _userContextAccessorMock = new();
        private readonly UserContext _userContext = new() { UserID = "1" };

        private PersonalValueService MockPersonalValueService()
        {
            _userContextAccessorMock.SetupGet(mock => mock.UserContext).Returns(_userContext);
            return new PersonalValueService(_personalValuesRepositoryMock.Object, _userContextAccessorMock.Object);
        }

        [Fact]
        public async Task GetAllValuesExceptUsers_Returns_All_Personal_Values_Except_User_Values_When_No_Search_String()
        {
            // Arrange

            List<PersonalValue> personalValues = new()
            {
                new PersonalValue { Id = 1, Name = "Value 1", GroupId = 1, Modifier = 1 },
                new PersonalValue { Id = 2, Name = "Value 2", GroupId = 1, Modifier = 1 },
                new PersonalValue { Id = 3, Name = "Value 3", GroupId = 2, Modifier = 1 },
                new PersonalValue { Id = 4, Name = "Value 4", GroupId = 2, Modifier = 1 }
            };

            List<UserValue> userValues = new()
            {
                new UserValue { UserId = "1", ValueId = 1 },
                new UserValue { UserId = "2", ValueId = 2 }
            };

            _userContextAccessorMock.SetupGet(mock => mock.UserContext).Returns(UserConstants.UserContextInstance);
            _personalValuesRepositoryMock.Setup(mock => mock.GetUserValuesWithDataAsync(_userContext.UserID)).ReturnsAsync(userValues);
            _personalValuesRepositoryMock.Setup(mock => mock.GetAllPersonalValuesAsync()).ReturnsAsync(personalValues);


            PersonalValueService service = MockPersonalValueService();
            // Act
            IEnumerable<PersonalValue> result = await service.GetAllValuesExceptUsers(null);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(personalValues[2], result);
            Assert.Contains(personalValues[3], result);
        }

        [Fact]
        public async Task GetAllValuesExceptUsers_Returns_Filtered_Values_When_Search_String_Is_Not_NullOrEmpty()
        {
            // Arrange
            string search = "Value2";
            UserContext userContext = new UserContext { UserID = UserConstants.userId };
            IEnumerable<PersonalValue> personalValues = new List<PersonalValue>
            {
                new PersonalValue { Id = 1, Name = "Value1" },
                new PersonalValue { Id = 2, Name = "Value2" },
                new PersonalValue { Id = 3, Name = "Value3" }
            };
            IEnumerable<UserValue> userValues = new List<UserValue>
            {
                new UserValue { UserId = userContext.UserID, ValueId = 1 }
            };
            _userContextAccessorMock.Setup(x => x.UserContext).Returns(userContext);
            _personalValuesRepositoryMock.Setup(x => x.GetUserValuesWithDataAsync(userContext.UserID)).ReturnsAsync(userValues);
            _personalValuesRepositoryMock.Setup(x => x.GetAllPersonalValuesAsync()).ReturnsAsync(personalValues);

            PersonalValueService service = MockPersonalValueService();

            // Act
            IEnumerable<PersonalValue> result = await service.GetAllValuesExceptUsers(search);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.DoesNotContain(personalValues.Single(p => p.Id == 1), result);
            Assert.Contains(personalValues.Single(p => p.Id == 2), result);
            Assert.DoesNotContain(personalValues.Single(p => p.Id == 3), result);
        }

        [Fact]
        public async Task GetValuesFromGroupAsyncReturnsCorrectValues()
        {
            long groupId = 1;
            PersonalValueGroup groupExpected = new()
            {
                Name = "Group 1",
                PersonalValues = new List<PersonalValue>
                    {
                        new PersonalValue { Id = 1, Name = "Value 1", Modifier = 1, GroupId = 1 },
                        new PersonalValue { Id = 2, Name = "Value 2", Modifier = 2, GroupId = 1 },
                        new PersonalValue { Id = 3, Name = "Value 3", Modifier = 3, GroupId = 1 }
                    }
            };
            _personalValuesRepositoryMock
                .Setup(mock => mock.GetAllPersonalValuesByGroupIdAsync(groupId))
                .ReturnsAsync(groupExpected);

            PersonalValueService service = MockPersonalValueService();
            ValuesInGroup result = await service.GetValuesFromGroupAsync(groupId);

            Assert.Equal(groupExpected.Name, result.GroupName);
            Assert.Equal(groupExpected.PersonalValues.Count, result.Values.Count());
            Assert.All(groupExpected.PersonalValues, v => Assert.Contains(result.Values, iv => iv.Id == v.Id && iv.Value == v.Name));
        }

        [Fact]
        public async Task GetValuesFromGroupAsync_WhenNoGroupFound_ShouldThrowException()
        {
            long groupId = 1;
            _personalValuesRepositoryMock
                .Setup(mock => mock.GetAllPersonalValuesByGroupIdAsync(groupId))
                .ThrowsAsync(new ValueGroupNotFoundException(groupId));

            PersonalValueService service = MockPersonalValueService();
            await Assert.ThrowsAsync<ValueGroupNotFoundException>(() => service.GetValuesFromGroupAsync(groupId));
        }

        [Fact]
        public async Task CreateValue_Should_Call_Repository_With_Correct_Value()
        {
            var newValue = new NewValue
            {
                GroupId = 1,
                Name = "Test Value",
                Modifier = 2
            };


            PersonalValueService service = MockPersonalValueService();
            await service.CreateValueAsync(newValue);

            _personalValuesRepositoryMock.Verify(x => x.CreateValueAsync(
                It.Is<PersonalValue>(p =>
                    p.GroupId == newValue.GroupId &&
                    p.Name == newValue.Name &&
                    p.Modifier == newValue.Modifier)
            ), Times.Once);
        }

        [Fact]
        public async Task CreateValueGroup_Should_Call_Repository_With_Correct_ValueGroup()
        {
            // Arrange
            var name = "Test Value Group";

            PersonalValueService service = MockPersonalValueService();
            await service.CreateValueGroupAsync(name);

            // Assert
            _personalValuesRepositoryMock.Verify(x => x.CreateValueGroupAsync(
                It.Is<PersonalValueGroup>(p => p.Name == name)
            ), Times.Once);
        }


        [Fact]
        public async Task UpdateValue_Should_Call_Repository_With_Correct_Value()
        {
            // Arrange
            var updatedValue = new UpdatedValue
            {
                ValueId = 1,
                Name = "Updated Value",
                Modifier = 3,
                GroupId = 2
            };

            PersonalValue oldPersonalValue = new()
            {
                Id = 1,
                Name = "Old Value",
                Modifier = 0,
                GroupId = 0
            };

            _personalValuesRepositoryMock
                .Setup(x => x.GetPersonalValueByIdAsync(updatedValue.ValueId))
                .ReturnsAsync(oldPersonalValue);

            // Act
            PersonalValueService service = MockPersonalValueService();
            await service.UpdateValueAsync(updatedValue);

            // Assert
            _personalValuesRepositoryMock.Verify(x => x.UpdateValueAsync(
                It.Is<PersonalValue>(p =>
                    p.Id == updatedValue.ValueId &&
                    p.Name == updatedValue.Name &&
                    p.Modifier == updatedValue.Modifier &&
                    p.GroupId == updatedValue.GroupId)
            ), Times.Once);
        }

        [Fact]
        public async Task UpdateValue_Should_Throw_ValueNotFoundException_If_Value_Not_Found()
        {
            // Arrange
            var updatedValue = new UpdatedValue
            {
                ValueId = 1,
                Name = "Updated Value",
                Modifier = 3,
                GroupId = 2
            };

            _personalValuesRepositoryMock.Setup(x => x.GetPersonalValueByIdAsync(updatedValue.ValueId))
                .ReturnsAsync((PersonalValue?)null);

            // Act & Assert
            PersonalValueService service = MockPersonalValueService();
            await Assert.ThrowsAsync<ValueNotFoundException>(() => service.UpdateValueAsync(updatedValue));
        }
    }
}