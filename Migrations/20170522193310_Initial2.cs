using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GardenManagement.Migrations
{
    public partial class Initial2 : Migration
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
                    ParameterName1 = table.Column<string>(nullable: true),
                    ParameterName10 = table.Column<string>(nullable: true),
                    ParameterName2 = table.Column<string>(nullable: true),
                    ParameterName3 = table.Column<string>(nullable: true),
                    ParameterName4 = table.Column<string>(nullable: true),
                    ParameterName5 = table.Column<string>(nullable: true),
                    ParameterName6 = table.Column<string>(nullable: true),
                    ParameterName7 = table.Column<string>(nullable: true),
                    ParameterName8 = table.Column<string>(nullable: true),
                    ParameterName9 = table.Column<string>(nullable: true),
                    SensorTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorType", x => x.SensorTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Sensor",
                columns: table => new
                {
                    SensorID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Parameter1 = table.Column<string>(nullable: true),
                    Parameter10 = table.Column<string>(nullable: true),
                    Parameter2 = table.Column<string>(nullable: true),
                    Parameter3 = table.Column<string>(nullable: true),
                    Parameter4 = table.Column<string>(nullable: true),
                    Parameter5 = table.Column<string>(nullable: true),
                    Parameter6 = table.Column<string>(nullable: true),
                    Parameter7 = table.Column<string>(nullable: true),
                    Parameter8 = table.Column<string>(nullable: true),
                    Parameter9 = table.Column<string>(nullable: true),
                    ReadingTimeframe = table.Column<int>(nullable: false),
                    SensorName = table.Column<int>(nullable: false),
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
                name: "IX_Sensor_SensorTypeID",
                table: "Sensor",
                column: "SensorTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_SensorValue_SensorID",
                table: "SensorValue",
                column: "SensorID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SensorValue");

            migrationBuilder.DropTable(
                name: "Sensor");

            migrationBuilder.DropTable(
                name: "SensorType");
        }
    }
}
