using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using GardenManagement.Data;

namespace GardenManagement.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20170612230944_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("GardenManagement.Data.Garden", b =>
                {
                    b.Property<int>("GardenID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AirHumiditySensorSensorID");

                    b.Property<string>("GardenName");

                    b.Property<int>("NumberOfColumns");

                    b.Property<int>("NumberOfRows");

                    b.Property<int?>("TemperatureSensorSensorID");

                    b.Property<int?>("WatertankSensorSensorID");

                    b.HasKey("GardenID");

                    b.HasIndex("AirHumiditySensorSensorID");

                    b.HasIndex("TemperatureSensorSensorID");

                    b.HasIndex("WatertankSensorSensorID");

                    b.ToTable("Garden");
                });

            modelBuilder.Entity("GardenManagement.Data.Plant", b =>
                {
                    b.Property<int>("PlantID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("HowToWater");

                    b.Property<DateTime>("LastWateringTime");

                    b.Property<int>("LocationColumn");

                    b.Property<int>("LocationRow");

                    b.Property<string>("PlantName");

                    b.Property<int?>("RelevantGardenGardenID");

                    b.Property<int?>("RelevantSoilMoistureSensorSensorID");

                    b.Property<int>("WateringThreshold");

                    b.HasKey("PlantID");

                    b.HasIndex("RelevantGardenGardenID");

                    b.HasIndex("RelevantSoilMoistureSensorSensorID");

                    b.ToTable("Plant");
                });

            modelBuilder.Entity("GardenManagement.Data.ReadingParameter", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ParameterName");

                    b.Property<string>("ParameterString");

                    b.Property<int?>("RelevantPLantPlantID");

                    b.Property<int?>("RelevantSensorSensorID");

                    b.HasKey("ID");

                    b.HasIndex("RelevantPLantPlantID");

                    b.HasIndex("RelevantSensorSensorID");

                    b.ToTable("ReadingParameter");
                });

            modelBuilder.Entity("GardenManagement.Data.Sensor", b =>
                {
                    b.Property<int>("SensorID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ReadingTimeframe");

                    b.Property<int?>("RelevantGardenGardenID");

                    b.Property<string>("SensorName");

                    b.Property<int?>("SensorTypeID");

                    b.HasKey("SensorID");

                    b.HasIndex("RelevantGardenGardenID");

                    b.HasIndex("SensorTypeID");

                    b.ToTable("Sensor");
                });

            modelBuilder.Entity("GardenManagement.Data.SensorType", b =>
                {
                    b.Property<int>("SensorTypeID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("HowToRead");

                    b.Property<string>("SensorTypeName");

                    b.HasKey("SensorTypeID");

                    b.ToTable("SensorType");
                });

            modelBuilder.Entity("GardenManagement.Data.SensorValue", b =>
                {
                    b.Property<int>("SensorValueID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("SensorID");

                    b.Property<DateTime>("Timestamp");

                    b.Property<double>("Value");

                    b.HasKey("SensorValueID");

                    b.HasIndex("SensorID");

                    b.ToTable("SensorValue");
                });

            modelBuilder.Entity("GardenManagement.Data.Garden", b =>
                {
                    b.HasOne("GardenManagement.Data.Sensor", "AirHumiditySensor")
                        .WithMany("GardenAirHumiditySensor")
                        .HasForeignKey("AirHumiditySensorSensorID");

                    b.HasOne("GardenManagement.Data.Sensor", "TemperatureSensor")
                        .WithMany("GardenTemperatureSensor")
                        .HasForeignKey("TemperatureSensorSensorID");

                    b.HasOne("GardenManagement.Data.Sensor", "WatertankSensor")
                        .WithMany("GardenWatertankSensor")
                        .HasForeignKey("WatertankSensorSensorID");
                });

            modelBuilder.Entity("GardenManagement.Data.Plant", b =>
                {
                    b.HasOne("GardenManagement.Data.Garden", "RelevantGarden")
                        .WithMany("Plants")
                        .HasForeignKey("RelevantGardenGardenID");

                    b.HasOne("GardenManagement.Data.Sensor", "RelevantSoilMoistureSensor")
                        .WithMany("Plants")
                        .HasForeignKey("RelevantSoilMoistureSensorSensorID");
                });

            modelBuilder.Entity("GardenManagement.Data.ReadingParameter", b =>
                {
                    b.HasOne("GardenManagement.Data.Plant", "RelevantPLant")
                        .WithMany("Parameters")
                        .HasForeignKey("RelevantPLantPlantID");

                    b.HasOne("GardenManagement.Data.Sensor", "RelevantSensor")
                        .WithMany("Parameters")
                        .HasForeignKey("RelevantSensorSensorID");
                });

            modelBuilder.Entity("GardenManagement.Data.Sensor", b =>
                {
                    b.HasOne("GardenManagement.Data.Garden", "RelevantGarden")
                        .WithMany("Sensors")
                        .HasForeignKey("RelevantGardenGardenID");

                    b.HasOne("GardenManagement.Data.SensorType", "SensorType")
                        .WithMany("Sensors")
                        .HasForeignKey("SensorTypeID");
                });

            modelBuilder.Entity("GardenManagement.Data.SensorValue", b =>
                {
                    b.HasOne("GardenManagement.Data.Sensor", "Sensor")
                        .WithMany("SensorValues")
                        .HasForeignKey("SensorID");
                });
        }
    }
}
