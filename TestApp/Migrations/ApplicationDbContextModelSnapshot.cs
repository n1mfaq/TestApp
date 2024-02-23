﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TestApp;

#nullable disable

namespace TestApp.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TestApp.Models.DeviceExperiment", b =>
                {
                    b.Property<Guid>("DeviceId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ExperimentId")
                        .HasColumnType("uuid");

                    b.Property<string>("ExperimentKey")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Option")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("DeviceId", "ExperimentId");

                    b.HasIndex("ExperimentId");

                    b.ToTable("DeviceExperiments");
                });

            modelBuilder.Entity("TestApp.Models.Experiment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Key")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("OptionsJson")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("Options");

                    b.HasKey("Id");

                    b.ToTable("Experiments");

                    b.HasData(
                        new
                        {
                            Id = new Guid("3719329f-548b-4f31-bc03-65dcc10633ee"),
                            Key = "button_color",
                            OptionsJson = "[\"#FF0000\",\"#00FF00\",\"#0000FF\"]"
                        },
                        new
                        {
                            Id = new Guid("f950c0ca-b81a-4f9d-8c8c-ab1b4fbc59f0"),
                            Key = "price",
                            OptionsJson = "[\"10\",\"20\",\"50\",\"5\"]"
                        });
                });

            modelBuilder.Entity("TestApp.Models.DeviceExperiment", b =>
                {
                    b.HasOne("TestApp.Models.Experiment", "Experiment")
                        .WithMany("DeviceExperiments")
                        .HasForeignKey("ExperimentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Experiment");
                });

            modelBuilder.Entity("TestApp.Models.Experiment", b =>
                {
                    b.Navigation("DeviceExperiments");
                });
#pragma warning restore 612, 618
        }
    }
}
