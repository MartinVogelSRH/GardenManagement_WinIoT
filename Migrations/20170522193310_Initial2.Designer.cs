using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using GardenManagement.Data;

namespace GardenManagement.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20170522193310_Initial2")]
    partial class Initial2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("GardenManagement.Data.Garden", b =>
                {
                    b.Property<int>("GardenID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("GardenName");

                    b.Property<int>("NumberOfColumns");

                    b.Property<int>("NumberOfRows");

                    b.HasKey("GardenID");

                    b.ToTable("Garden");
                });

            modelBuilder.Entity("GardenManagement.Data.Plant", b =>
                {
                    b.Property<int>("PlantID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("LastWateringTime");

                    b.Property<int>("LocationColumn");

                    b.Property<int>("LocationRow");

                    b.Property<string>("PlantName");

                    b.Property<int?>("RelevantGardenGardenID");

                    b.Property<int>("WateringThreshold");

                    b.HasKey("PlantID");

                    b.HasIndex("RelevantGardenGardenID");

                    b.ToTable("Plant");
                });

            modelBuilder.Entity("GardenManagement.Data.Sensor", b =>
                {
                    b.Property<int>("SensorID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Parameter1");

                    b.Property<string>("Parameter10");

                    b.Property<string>("Parameter2");

                    b.Property<string>("Parameter3");

                    b.Property<string>("Parameter4");

                    b.Property<string>("Parameter5");

                    b.Property<string>("Parameter6");

                    b.Property<string>("Parameter7");

                    b.Property<string>("Parameter8");

                    b.Property<string>("Parameter9");

                    b.Property<int>("ReadingTimeframe");

                    b.Property<int>("SensorName");

                    b.Property<int?>("SensorTypeID");

                    b.HasKey("SensorID");

                    b.HasIndex("SensorTypeID");

                    b.ToTable("Sensor");
                });

            modelBuilder.Entity("GardenManagement.Data.SensorType", b =>
                {
                    b.Property<int>("SensorTypeID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("HowToRead");

                    b.Property<string>("ParameterName1");

                    b.Property<string>("ParameterName10");

                    b.Property<string>("ParameterName2");

                    b.Property<string>("ParameterName3");

                    b.Property<string>("ParameterName4");

                    b.Property<string>("ParameterName5");

                    b.Property<string>("ParameterName6");

                    b.Property<string>("ParameterName7");

                    b.Property<string>("ParameterName8");

                    b.Property<string>("ParameterName9");

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

            modelBuilder.Entity("GardenManagement.Data.Plant", b =>
                {
                    b.HasOne("GardenManagement.Data.Garden", "RelevantGarden")
                        .WithMany("Plants")
                        .HasForeignKey("RelevantGardenGardenID");
                });

            modelBuilder.Entity("GardenManagement.Data.Sensor", b =>
                {
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
