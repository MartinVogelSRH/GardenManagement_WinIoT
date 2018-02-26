using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GardenManagement.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using Windows.UI.Popups;

namespace GardenManagement.Tests
{
    class DatabaseTest
    {

        public static void addDummyData()
        {
            //We will just generate some made up data
            Garden testGarden = new Garden
            {
                GardenName = "Dummy Garden",
                NumberOfColumns = 3,
                NumberOfRows = 4
            };          
            SensorType testSoilSensor = new SensorType
            {
                SensorTypeName = "Dummy Soil Moisture",
                HowToRead = "readDummySoilMoisture",
                //ParameterName1 = "Dummy Parameter",
            };
            SensorType testDF0193 = new SensorType
            {
                SensorTypeName = "DF0193 behind MCP3008",
                HowToRead = "readDF0193BehindMCP3008Async",
                //ParameterName1 = "Dummy Parameter",
            };          
            SensorType testTemperatureSensor = new SensorType
            {
                SensorTypeName = "Dummy Temperature",
                HowToRead = "readDummyTemperature",
                //ParameterName1 = "Dummy Parameter",
            };
            SensorType testHumiditySensor = new SensorType
            {
                SensorTypeName = "Dummy Humidity",
                HowToRead = "readDummyHumidity",
                //ParameterName1 = "Dummy Parameter",
            };
            SensorType testWaterFillingSensor = new SensorType
            {
                SensorTypeName = "Dummy Watertank",
                HowToRead = "readDummyWatertankFill",
                //ParameterName1 = "Dummy Parameter",
            };
            Sensor testDF0193_1 = new Sensor
            {
                SensorName = "DF0193 behind MCP3008 _ 1",
                SensorType = testDF0193,
                ReadingTimeframe = 600,
                RelevantGarden = testGarden,
                Parameters = new ObservableCollection<ReadingParameter> { new ReadingParameter() { ParameterName = "spiChannel" , ParameterString = "0"}, new ReadingParameter() {  ParameterName = "mcp3008channel", ParameterString = "0" } }
            };
            Sensor testSpecificSoilSensor = new Sensor
            {
                SensorName = "Dummy Soil Moisture Sensor",
                SensorType = testSoilSensor,
                ReadingTimeframe = 1800,
                //Parameter1 = "25",
                RelevantGarden = testGarden,
                Parameters = new ObservableCollection<ReadingParameter> { new ReadingParameter() { ParameterName = "Parameter1", ParameterString = "25" } }
            };
            Sensor testSpecificHumiditySensor = new Sensor
            {
                SensorName = "Dummy Humidity Sensor",
                SensorType = testHumiditySensor,
                ReadingTimeframe = 30,
                //Parameter1 = "50",
                Parameters = new ObservableCollection<ReadingParameter> { new ReadingParameter() { ParameterName = "Parameter1", ParameterString = "50" } },
                RelevantGarden = testGarden
            };
            Sensor testSpecificTemperatureSensor = new Sensor
            {
                SensorName = "Dummy Temperature Sensor",
                SensorType = testTemperatureSensor,
                ReadingTimeframe = 30,
                //Parameter1 = "20",
                Parameters = new ObservableCollection<ReadingParameter> { new ReadingParameter() { ParameterName = "Parameter1", ParameterString = "20" } },
                RelevantGarden = testGarden
            };
            Sensor testSpecificWatertankSensor = new Sensor
            {
                SensorName = "Dummy Watertank Sensor",
                SensorType = testWaterFillingSensor,
                ReadingTimeframe = 30,
                //Parameter1 = "45",
                Parameters = new ObservableCollection<ReadingParameter> { new ReadingParameter() { ParameterName = "Parameter1", ParameterString = "45" } },
                RelevantGarden = testGarden
            };
            Plant testPlant11 = new Plant
            {
                LastWateringTime = DateTime.Now,
                LocationColumn = 1,
                LocationRow = 1,
                PlantName = "Tom Tomato",
                RelevantGarden = testGarden,
                WateringThreshold = 25,
                RelevantSoilMoistureSensor = testSpecificSoilSensor,
                Parameters = new ObservableCollection<ReadingParameter> { new ReadingParameter() { ParameterName = "Parameter1", ParameterString = "20" } },
                HowToWater = "waterDummyPlant"
            };
            Plant testPlant12 = new Plant
            {
                LastWateringTime = DateTime.Now,
                LocationColumn = 2,
                LocationRow = 1,
                PlantName = "Timo Thyme",
                RelevantGarden = testGarden,
                WateringThreshold = 22,
                RelevantSoilMoistureSensor = testSpecificSoilSensor,
                Parameters = new ObservableCollection<ReadingParameter> { new ReadingParameter() { ParameterName = "Parameter1", ParameterString = "20" } },
                HowToWater = "waterDummyPlant"
            };
            Plant testPlant13 = new Plant
            {
                LastWateringTime = DateTime.Now,
                LocationColumn = 3,
                LocationRow = 1,
                PlantName = "Chilja Chili",
                RelevantGarden = testGarden,
                WateringThreshold = 30,
                RelevantSoilMoistureSensor = testSpecificSoilSensor,
                Parameters = new ObservableCollection<ReadingParameter> { new ReadingParameter() { ParameterName = "Parameter1", ParameterString = "20" } },
                HowToWater = "waterDummyPlant"
            };
            Plant testPlant21 = new Plant
            {
                LastWateringTime = DateTime.Now,
                LocationColumn = 1,
                LocationRow = 2,
                PlantName = "Sally Strawberry",
                RelevantGarden = testGarden,
                WateringThreshold = 25,
                RelevantSoilMoistureSensor = testSpecificSoilSensor,
                Parameters = new ObservableCollection<ReadingParameter> { new ReadingParameter() { ParameterName = "Parameter1", ParameterString = "20" } },
                HowToWater = "waterDummyPlant"
            };
            Plant testPlant23 = new Plant
            {
                LastWateringTime = DateTime.Now,
                LocationColumn = 3,
                LocationRow = 2,
                PlantName = "Basti Basil",
                RelevantGarden = testGarden,
                WateringThreshold = 30,
                RelevantSoilMoistureSensor = testSpecificSoilSensor,
                Parameters = new ObservableCollection<ReadingParameter> { new ReadingParameter() { ParameterName = "Parameter1", ParameterString = "20" } },
                HowToWater = "waterDummyPlant"
            };
            Plant testPlant41 = new Plant
            {
                LastWateringTime = DateTime.Now,
                LocationColumn = 1,
                LocationRow = 4,
                PlantName = "Toni Tomato",
                RelevantGarden = testGarden,
                WateringThreshold = 25,
                RelevantSoilMoistureSensor = testSpecificSoilSensor,
                Parameters = new ObservableCollection<ReadingParameter> { new ReadingParameter() { ParameterName = "Parameter1", ParameterString = "20" } },
                HowToWater = "waterDummyPlant"
            };
            Plant testPlant42 = new Plant
            {
                LastWateringTime = DateTime.Now,
                LocationColumn = 2,
                LocationRow = 4,
                PlantName = "Olly Onion",
                RelevantGarden = testGarden,
                WateringThreshold = 22,
                RelevantSoilMoistureSensor = testSpecificSoilSensor,
                Parameters = new ObservableCollection<ReadingParameter> { new ReadingParameter() { ParameterName = "Parameter1", ParameterString = "20" } },
                HowToWater = "waterDummyPlant"
            };
            Plant testPlant43 = new Plant
            {
                LastWateringTime = DateTime.Now,
                LocationColumn = 3,
                LocationRow = 4,
                PlantName = "Chris Chive",
                RelevantGarden = testGarden,
                WateringThreshold = 30,
                RelevantSoilMoistureSensor = testSpecificSoilSensor,
                Parameters = new ObservableCollection<ReadingParameter> { new ReadingParameter() { ParameterName = "Parameter1", ParameterString = "20" } },
                HowToWater = "waterDummyPlant"
            };
            
            //Now, let´s add this to our database
            App._db.Garden.Add(testGarden);          
            App._db.Plant.Add(testPlant11);
            App._db.Plant.Add(testPlant12);
            App._db.Plant.Add(testPlant13);
            App._db.Plant.Add(testPlant21);
            App._db.Plant.Add(testPlant13);
            App._db.Plant.Add(testPlant41);
            App._db.Plant.Add(testPlant42);
            App._db.Plant.Add(testPlant43);
            testGarden.fill_plants();
            //Due to interpendencies of our elements, we need to save to the database in between
            App._db.SaveChanges();
            App._db.SensorType.Add(testTemperatureSensor);
            App._db.SensorType.Add(testHumiditySensor);
            App._db.SensorType.Add(testWaterFillingSensor);
            App._db.SensorType.Add(testDF0193);
            App._db.Sensor.Add(testDF0193_1);
            App._db.Sensor.Add(testSpecificTemperatureSensor);
            App._db.Sensor.Add(testSpecificHumiditySensor);
            App._db.Sensor.Add(testSpecificWatertankSensor);
            testGarden.TemperatureSensor = testSpecificTemperatureSensor;
            testGarden.AirHumiditySensor = testSpecificHumiditySensor;
            testGarden.WatertankSensor = testSpecificWatertankSensor;
            App._db.SaveChanges();

            //Now let´s generate some random soil moisture readings for the last 7 days.
            Random rand = new Random();
                for (DateTime date = DateTime.Now.AddDays(-7).Date; date < DateTime.Now.AddMinutes(-30); date = date.AddMinutes(30))
                {
                App._db.SensorValue.Add(new SensorValue()
                {
                    Sensor = testSpecificSoilSensor,
                    Timestamp = date,
                    Value = rand.Next(10,50)
                }
                );
            }
            App._db.SaveChanges();
            Electronics.SensorHelper.ReadAsync(testSpecificSoilSensor);
            Electronics.SensorHelper.ReadAsync(testSpecificHumiditySensor);
            Electronics.SensorHelper.ReadAsync(testSpecificTemperatureSensor);
            Electronics.SensorHelper.ReadAsync(testSpecificWatertankSensor);
        }

        internal static void recreateDatabase()
        {
            //This deletes the actual database file
            try
            {
                App._db.Database.EnsureDeleted();
                //This regenerates the actual database file
                App._db.Database.Migrate();
                //We need a new DatabaseContext, since all our cached entities do not exist anymore.
                App._db = new Data.DatabaseContext();
                new MessageDialog("Database was recreated").ShowAsync();
            }
            catch
            {
                new MessageDialog("Something went wrong. Please try again.").ShowAsync();
            }

            
        }
    }
}
