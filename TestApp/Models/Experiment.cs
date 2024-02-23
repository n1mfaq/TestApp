using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace TestApp.Models;

public class Experiment
{
    public Guid Id { get; init; }
    
    [MaxLength(255)]
    public string? Key { get; init; }
    
    [NotMapped]
    public ICollection<string>? Options { get; set; }
    
    [MaxLength(255)]
    [Column("Options")]
    public string? OptionsJson
    {
        get => Options != null ? JsonSerializer.Serialize(Options) : null;
        init => Options = value != null ? JsonSerializer.Deserialize<List<string>>(value) : null;
    }
    public List<DeviceExperiment> DeviceExperiments { get; set; }
    
}

