using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TestApp.ApiControllers;
using TestApp.Models;
using TestApp.Services;
using Xunit;

namespace TestApp.Tests.ApiTests;

[Collection("Database collection")]
public class ExperimentApiControllerTests : IClassFixture<TestApplicationDbContextFactory>
{
    private readonly TestApplicationDbContextFactory _factory;

    public ExperimentApiControllerTests(TestApplicationDbContextFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetButtonColorExperiment_ExistingDevice_ReturnsCorrectResult()
    {
        var options = new List<string> { "#FF0000", "#00FF00", "#0000FF" };
        var existingDevice = new DeviceExperiment
        {
            ExperimentKey = "button_color",
            DeviceId = Guid.NewGuid(),
            Experiment = new Experiment { Key = "button_color", Options = options },
            Option = "#FF0000"
        };

        await using var dbContext = _factory.CreateContext();
        await dbContext.DeviceExperiments.AddAsync(existingDevice);
        await dbContext.SaveChangesAsync();

        var optionChooserService = new Mock<IOptionChooserService>();
        optionChooserService.Setup(x => x.ChooseRandomOption(It.IsAny<Experiment>())).Returns("#00FF00");

        var controller = new ExperimentApiController(_factory.CreateContext(), optionChooserService.Object,
            new NullLogger<ExperimentApiController>());

        // Act
        var result = await controller.GetButtonColorExperiment(existingDevice.DeviceId);

        // Assert

        result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();


        var objectResultOk = (OkObjectResult)result;
        var resultData = (ExperimentResult)objectResultOk.Value!;
        Assert.Equal("button_color", resultData.Key);
        Assert.Equal(existingDevice.Option, resultData.Value);
    }
}