namespace TestApp.Models.DTO;

public class ExperimentStatisticsViewModel
{
    public string ExperimentKey { get; init; }
    public int TotalDevices { get; init; }
    public Dictionary<string, int> OptionsDistribution { get; init; }
}