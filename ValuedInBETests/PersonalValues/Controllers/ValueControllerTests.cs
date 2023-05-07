using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ValuedInBE.PersonalValues.Controllers;
using ValuedInBE.PersonalValues.Models.DTOs.Requests;
using ValuedInBE.PersonalValues.Models.Entities;
using ValuedInBE.PersonalValues.Service;
using Xunit;

namespace ValuedInBE.PersonalValues.Controllers.Tests
{
    public class ValueControllerTests
    {
        private readonly Mock<IPersonalValueService> _valueServiceMock = new();
        private readonly Mock<ILogger<ValueController>> _logger = new();

        private ValueController MockValueController()
        {
            return new ValueController(_valueServiceMock.Object, _logger.Object);
        }

        [Fact]
        public async Task ListAllValuesReturnsValues()
        {
            IEnumerable<PersonalValue> valuesExpected = new List<PersonalValue>
            {
                new PersonalValue { Name = "Value1" },
                new PersonalValue { Name = "Value2" }
            };

            _valueServiceMock
                .Setup(mock => mock.GetAllValuesExceptUsers(It.IsAny<string>()))
                .ReturnsAsync(valuesExpected);

            var controller = MockValueController();

            var actionResult = await controller.ListAllValues(null);

            Assert.NotNull(actionResult);
            Assert.IsType<ActionResult<IEnumerable<PersonalValue>>>(actionResult);

            var valuesReturned = actionResult.Value;

            Assert.NotNull(valuesReturned);
            Assert.Equal(valuesExpected, valuesReturned);
        }

        [Fact]
        public async Task CreateValueCallsServiceToCreateValue()
        {
            var valueExpected = new NewValue { Name = "Test Value" };

            _valueServiceMock.Setup(mock => mock.CreateValue(valueExpected)).Verifiable();

            var controller = MockValueController();

            await controller.CreateValue(valueExpected);

            _valueServiceMock.Verify();
        }

        [Fact]
        public async Task UpdateValueCallsServiceToUpdateValue()
        {
            var valueExpected = new UpdatedValue { ValueId = 123, Name = "Test Value" };

            _valueServiceMock.Setup(mock => mock.UpdateValue(valueExpected)).Verifiable();

            var controller = MockValueController();

            await controller.UpdateValue(valueExpected);

            _valueServiceMock.Verify();
        }
    }
}
