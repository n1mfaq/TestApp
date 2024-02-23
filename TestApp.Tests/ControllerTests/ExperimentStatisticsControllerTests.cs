using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using TestApp.Controllers;
using TestApp.Models;
using TestApp.Models.DTO;
using Xunit;

namespace TestApp.Tests.ControllerTests;

public class ExperimentStatisticsControllerTests : IClassFixture<TestApplicationDbContextFactory>
{
    private readonly TestApplicationDbContextFactory _factory;

    public ExperimentStatisticsControllerTests(TestApplicationDbContextFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public void Index_ReturnsViewWithCorrectModel()
    {
        // Arrange
        using var dbContext = _factory.CreateContext();
        var controller =
            new ExperimentStatisticsController(dbContext, new NullLogger<ExperimentStatisticsController>());

        // Act
        var result = controller.Index();

        // Assert
        Assert.IsType<ViewResult>(result);
        var viewResult = (ViewResult)result;
        Assert.IsType<List<ExperimentStatisticsViewModel>>(viewResult.Model);
        var model = (List<ExperimentStatisticsViewModel>)viewResult.Model;

        Assert.NotNull(model);
    }

    [Fact]
    public void Index_ReturnsEmptyList_WhenNoData()
    {
        // Arrange
        using var dbContext = _factory.CreateContext();
        var controller =
            new ExperimentStatisticsController(dbContext, new NullLogger<ExperimentStatisticsController>());

        // Act
        var result = controller.Index();

        // Assert
        Assert.IsType<ViewResult>(result);
        var viewResult = (ViewResult)result;
        Assert.IsType<List<ExperimentStatisticsViewModel>>(viewResult.Model);
        var model = (List<ExperimentStatisticsViewModel>)viewResult.Model;

        Assert.Empty(model);
    }

    [Fact]
    public void Index_ReturnsCorrectStatistics_WithData()
    {
        // Arrange
        using var dbContext = _factory.CreateContext();
        var controller =
            new ExperimentStatisticsController(dbContext, new NullLogger<ExperimentStatisticsController>());

        // Add some data to the database
        var experiment = new Experiment
        {
            Id = Guid.NewGuid(),
            Key = "price",
            Options = new List<string> { "10", "20", "50", "5" }
        };

        var deviceExperiment1 = new DeviceExperiment
        {
            ExperimentKey = experiment.Key,
            DeviceId = Guid.NewGuid(),
            ExperimentId = experiment.Id,
            Option = "10"
        };

        var deviceExperiment2 = new DeviceExperiment
        {
            ExperimentKey = experiment.Key,
            DeviceId = Guid.NewGuid(),
            ExperimentId = experiment.Id,
            Option = "20"
        };

        var deviceExperiment3 = new DeviceExperiment
        {
            ExperimentKey = experiment.Key,
            DeviceId = Guid.NewGuid(),
            ExperimentId = experiment.Id,
            Option = "50"
        };

        dbContext.Experiments.Add(experiment);
        dbContext.DeviceExperiments.AddRange(deviceExperiment1, deviceExperiment2, deviceExperiment3);
        dbContext.SaveChanges();

        // Act
        var result = controller.Index();

        // Assert
        Assert.IsType<ViewResult>(result);
        var viewResult = (ViewResult)result;
        Assert.IsType<List<ExperimentStatisticsViewModel>>(viewResult.Model);
        var model = (List<ExperimentStatisticsViewModel>)viewResult.Model;

        Assert.Single(model);

        var experiment1 = model[0];
        Assert.Equal("price", experiment1.ExperimentKey);
        Assert.Equal(3, experiment1.TotalDevices);
        Assert.Equal(1, experiment1.OptionsDistribution["10"]);
        Assert.Equal(1, experiment1.OptionsDistribution["20"]);
        Assert.Equal(1, experiment1.OptionsDistribution["50"]);
    }
}