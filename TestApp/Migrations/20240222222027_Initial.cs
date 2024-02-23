using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TestApp.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Experiments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Options = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceExperiments",
                columns: table => new
                {
                    DeviceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExperimentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExperimentKey = table.Column<string>(type: "text", nullable: false),
                    Option = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceExperiments", x => new { x.DeviceId, x.ExperimentId });
                    table.ForeignKey(
                        name: "FK_DeviceExperiments_Experiments_ExperimentId",
                        column: x => x.ExperimentId,
                        principalTable: "Experiments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Experiments",
                columns: new[] { "Id", "Key", "Options" },
                values: new object[,]
                {
                    { new Guid("3719329f-548b-4f31-bc03-65dcc10633ee"), "button_color", "[\"#FF0000\",\"#00FF00\",\"#0000FF\"]" },
                    { new Guid("f950c0ca-b81a-4f9d-8c8c-ab1b4fbc59f0"), "price", "[\"10\",\"20\",\"50\",\"5\"]" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceExperiments_ExperimentId",
                table: "DeviceExperiments",
                column: "ExperimentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceExperiments");

            migrationBuilder.DropTable(
                name: "Experiments");
        }
    }
}
