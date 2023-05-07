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

namespace ValuedInBE.PersonalValues.Tests.Controllers
{
    public class ValueControllerTests
    {
        private readonly Mock<IPersonalValueService> _personalValueServiceMock = new();
        private readonly Mock<ILogger<ValueController>> _loggerMock = new();
        private const string filter = "some filter";

        private ValueController MockValueController()
        {
            return new ValueController(_personalValueServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task ListAllValuesReturnsValues()
        {
            string search = filter;
            IEnumerable<PersonalValue> valuesExpected = new List<PersonalValue> { new PersonalValue(), new PersonalValue() };
            _personalValueServiceMock
                .Setup(mock => mock.GetAllValuesExceptUsers(search))
                .ReturnsAsync(valuesExpected);

            ValueController controller = MockValueController();
            Task<IEnumerable<PersonalValue>> valueResultTask = controller.ListAllValues(search);
            IEnumerable<PersonalValue> valuesReturned = await valueResultTask;
            Assert.NotNull(valuesReturned);
            Assert.Same(valuesExpected, valuesReturned);
        }

        [Fact]
        public async Task CreateValueCallsServiceToCreate()
        {
            NewValue valueExpected = new();
            _personalValueServiceMock.Setup(mock => mock.CreateValueAsync(valueExpected)).Verifiable();

            ValueController controller = MockValueController();
            await controller.CreateValue(valueExpected);
            _personalValueServiceMock.Verify();
        }

        [Fact]
        public async Task UpdateValueCallsServiceToUpdate()
        {
            UpdatedValue valueExpected = new();
            _personalValueServiceMock.Setup(mock => mock.UpdateValueAsync(valueExpected)).Verifiable();

            ValueController controller = MockValueController();
            await controller.UpdateValue(valueExpected);
            _personalValueServiceMock.Verify();
        }
    }
}
