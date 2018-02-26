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
                name: "SensorType",
                columns: table => new
                {
                    SensorTypeID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HowToRead = table.Column<string>(nullable: true),
                    SensorTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorType", x => x.SensorTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Plant",
                columns: table => new
                {
                    PlantID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HowToWater = table.Column<string>(nullable: true),
                    LastWateringTime = table.Column<DateTime>(nullable: false),
                    LocationColumn = table.Column<int>(nullable: false),
                    LocationRow = table.Column<int>(nullable: false),
                    PlantName = table.Column<string>(nullable: true),
                    RelevantGardenGardenID = table.Column<int>(nullable: true),
                    RelevantSoilMoistureSensorSensorID = table.Column<int>(nullable: true),
                    WateringThreshold = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plant", x => x.PlantID);
                });

            migrationBuilder.CreateTable(
                name: "Sensor",
                columns: table => new
                {
                    SensorID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReadingTimeframe = table.Column<int>(nullable: false),
                    RelevantGardenGardenID = table.Column<int>(nullable: true),
                    SensorName = table.Column<string>(nullable: true),
                    SensorTypeID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensor", x => x.SensorID);
                    table.ForeignKey(
                        name: "FK_Sensor_SensorType_SensorTypeID",
                        column: x => x.SensorTypeID,
                        principalTable: "SensorType",
                        principalColumn: "SensorTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Garden",
                columns: table => new
                {
                    GardenID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AirHumiditySensorSensorID = table.Column<int>(nullable: true),
                    GardenName = table.Column<string>(nullable: true),
                    NumberOfColumns = table.Column<int>(nullable: false),
                    NumberOfRows = table.Column<int>(nullable: false),
                    TemperatureSensorSensorID = table.Column<int>(nullable: true),
                    WatertankSensorSensorID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Garden", x => x.GardenID);
                    table.ForeignKey(
                        name: "FK_Garden_Sensor_AirHumiditySensorSensorID",
                        column: x => x.AirHumiditySensorSensorID,
                        principalTable: "Sensor",
                        principalColumn: "SensorID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Garden_Sensor_TemperatureSensorSensorID",
                        column: x => x.TemperatureSensorSensorID,
                        principalTable: "Sensor",
                        principalColumn: "SensorID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Garden_Sensor_WatertankSensorSensorID",
                        column: x => x.WatertankSensorSensorID,
                        principalTable: "Sensor",
                        principalColumn: "SensorID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReadingParameter",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ParameterName = table.Column<string>(nullable: true),
                    ParameterString = table.Column<string>(nullable: true),
                    RelevantPLantPlantID = table.Column<int>(nullable: true),
                    RelevantSensorSensorID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingParameter", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ReadingParameter_Plant_RelevantPLantPlantID",
                        column: x => x.RelevantPLantPlantID,
                        principalTable: "Plant",
                        principalColumn: "PlantID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReadingParameter_Sensor_RelevantSensorSensorID",
                        column: x => x.RelevantSensorSensorID,
                        principalTable: "Sensor",
                        principalColumn: "SensorID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SensorValue",
                columns: table => new
                {
                    SensorValueID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SensorID = table.Column<int>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    Value = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorValue", x => x.SensorValueID);
                    table.ForeignKey(
                        name: "FK_SensorValue_Sensor_SensorID",
                        column: x => x.SensorID,
                        principalTable: "Sensor",
                        principalColumn: "SensorID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Garden_AirHumiditySensorSensorID",
                table: "Garden",
                column: "AirHumiditySensorSensorID");

            migrationBuilder.CreateIndex(
                name: "IX_Garden_TemperatureSensorSensorID",
                table: "Garden",
                column: "TemperatureSensorSensorID");

            migrationBuilder.CreateIndex(
                name: "IX_Garden_WatertankSensorSensorID",
                table: "Garden",
                column: "WatertankSensorSensorID");

            migrationBuilder.CreateIndex(
                name: "IX_Plant_RelevantGardenGardenID",
                table: "Plant",
                column: "RelevantGardenGardenID");

            migrationBuilder.CreateIndex(
                name: "IX_Plant_RelevantSoilMoistureSensorSensorID",
                table: "Plant",
                column: "RelevantSoilMoistureSensorSensorID");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingParameter_RelevantPLantPlantID",
                table: "ReadingParameter",
                column: "RelevantPLantPlantID");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingParameter_RelevantSensorSensorID",
                table: "ReadingParameter",
                column: "RelevantSensorSensorID");

            migrationBuilder.CreateIndex(
                name: "IX_Sensor_RelevantGardenGardenID",
                table: "Sensor",
                column: "RelevantGardenGardenID");

            migrationBuilder.CreateIndex(
                name: "IX_Sensor_SensorTypeID",
                table: "Sensor",
                column: "SensorTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_SensorValue_SensorID",
                table: "SensorValue",
                column: "SensorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Plant_Sensor_RelevantSoilMoistureSensorSensorID",
                table: "Plant",
                column: "RelevantSoilMoistureSensorSensorID",
                principalTable: "Sensor",
                principalColumn: "SensorID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Plant_Garden_RelevantGardenGardenID",
                table: "Plant",
                column: "RelevantGardenGardenID",
                principalTable: "Garden",
                principalColumn: "GardenID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sensor_Garden_RelevantGardenGardenID",
                table: "Sensor",
                column: "RelevantGardenGardenID",
                principalTable: "Garden",
                principalColumn: "GardenID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Garden_Sensor_AirHumiditySensorSensorID",
                table: "Garden");

            migrationBuilder.DropForeignKey(
                name: "FK_Garden_Sensor_TemperatureSensorSensorID",
                table: "Garden");

            migrationBuilder.DropForeignKey(
                name: "FK_Garden_Sensor_WatertankSensorSensorID",
                table: "Garden");

            migrationBuilder.DropTable(
                name: "ReadingParameter");

            migrationBuilder.DropTable(
                name: "SensorValue");

            migrationBuilder.DropTable(
                name: "Plant");

            migrationBuilder.DropTable(
                name: "Sensor");

            migrationBuilder.DropTable(
                name: "Garden");

            migrationBuilder.DropTable(
                name: "SensorType");
        }
    }
}
