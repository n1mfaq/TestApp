using Microsoft.EntityFrameworkCore;
using TestApp.Models;

namespace TestApp;

public class ApplicationDbContext : DbContext
{
    public DbSet<Experiment> Experiments { get; set; }
    public DbSet<DeviceExperiment> DeviceExperiments { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        ConfigureModel(modelBuilder);
        SeedData(modelBuilder);
    }

    private void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DeviceExperiment>()
            .HasKey(de => new { de.DeviceId, de.ExperimentId });

        modelBuilder.Entity<DeviceExperiment>()
            .HasOne(de => de.Experiment)
            .WithMany(e => e.DeviceExperiments)
            .HasForeignKey(de => de.ExperimentId);

        modelBuilder.Entity<Experiment>()
            .HasMany(e => e.DeviceExperiments)
            .WithOne(de => de.Experiment)
            .HasForeignKey(de => de.ExperimentId);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        SeedExperiments(modelBuilder);
    }

    private void SeedExperiments(ModelBuilder modelBuilder)
    {
        const string buttonColorKey = "button_color";
        const string priceKey = "price";

        var buttonColorExperiment = new Experiment
        {
            Id = Guid.NewGuid(),
            Key = buttonColorKey,
            Options = new List<string> { "#FF0000", "#00FF00", "#0000FF" }
        };

        var priceExperiment = new Experiment
        {
            Id = Guid.NewGuid(),
            Key = priceKey,
            Options = new List<string> { "10", "20", "50", "5" }
        };

        modelBuilder.Entity<Experiment>().HasData(
            buttonColorExperiment,
            priceExperiment
        );
    }
}