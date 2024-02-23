namespace TestApp.Models;

public class DeviceExperiment
{
    public Guid DeviceId { get; init; }
    public Experiment? Experiment { get; init; }
    public string ExperimentKey { get; init; }
    public Guid ExperimentId { get; init; }
    public string Option { get; init; } 
}
