using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ValuedInBE.PersonalValues.Controllers;
using ValuedInBE.PersonalValues.Models.DTOs.Pocos;
using ValuedInBE.PersonalValues.Models.DTOs.Response;
using ValuedInBE.PersonalValues.Models.Entities;
using ValuedInBE.PersonalValues.Service;
using Xunit;

public class ValueGroupControllerTests
{
    private readonly Mock<IPersonalValueService> _mockPersonalValueService;
    private readonly Mock<ILogger<ValueGroupController>> _mockLogger;
    private readonly ValueGroupController _controller;

    public ValueGroupControllerTests()
    {
        _mockPersonalValueService = new Mock<IPersonalValueService>();
        _mockLogger = new Mock<ILogger<ValueGroupController>>();
        _controller = new ValueGroupController(_mockPersonalValueService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetPersonalValuesFromGroupAsync_ReturnsOkResult_WhenValidGroupId()
    {
        string groupName = "groupName";
        long id = 1;
        ValuesInGroup expectedValuesInGroup = new() { GroupName = groupName, Values = Enumerable.Empty<IdAndValue>() };
        _mockPersonalValueService
            .Setup(x => x.GetValuesFromGroupAsync(id))
            .ReturnsAsync(expectedValuesInGroup);

        ValuesInGroup actualValuesInGroup = await _controller.GetPersonalValuesFromGroupAsync(id);

        Assert.Equal(expectedValuesInGroup.GroupName, actualValuesInGroup.GroupName);
        Assert.Equal(expectedValuesInGroup.Values.Count(), actualValuesInGroup.Values.Count());
    }
}
