using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GardenManagement.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Garden",
                columns: table => new
                {
                    GardenID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GardenName = table.Column<string>(nullable: true),
                    NumberOfColumns = table.Column<int>(nullable: false),
                    NumberOfRows = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Garden", x => x.GardenID);
                });

            migrationBuilder.CreateTable(
                name: "Plant",
                columns: table => new
                {
                    PlantID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LastWateringTime = table.Column<DateTime>(nullable: false),
                    LocationColumn = table.Column<int>(nullable: false),
                    LocationRow = table.Column<int>(nullable: false),
                    PlantName = table.Column<string>(nullable: true),
                    RelevantGardenGardenID = table.Column<int>(nullable: true),
                    WateringThreshold = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plant", x => x.PlantID);
                    table.ForeignKey(
                        name: "FK_Plant_Garden_RelevantGardenGardenID",
                        column: x => x.RelevantGardenGardenID,
                        principalTable: "Garden",
                        principalColumn: "GardenID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plant_RelevantGardenGardenID",
                table: "Plant",
                column: "RelevantGardenGardenID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Plant");

            migrationBuilder.DropTable(
                name: "Garden");
        }
    }
}
