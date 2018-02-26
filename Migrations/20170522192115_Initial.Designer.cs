using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using GardenManagement.Data;

namespace GardenManagement.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20170522192115_Initial")]
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

            modelBuilder.Entity("GardenManagement.Data.Plant", b =>
                {
                    b.HasOne("GardenManagement.Data.Garden", "RelevantGarden")
                        .WithMany("Plants")
                        .HasForeignKey("RelevantGardenGardenID");
                });
        }
    }
}
