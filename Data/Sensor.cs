using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenManagement.Data
{
    public class Sensor : ValidatableBase
    {
        public int SensorID { get; set; }
        public int ReadingTimeframe { get; set; }
        public string SensorName { get; set; }
        public ObservableCollection<ReadingParameter> Parameters { get; set; }
        public SensorType SensorType { get; set; }
        [InverseProperty("Sensors")]
        public Garden RelevantGarden { get; set; }
        public ObservableCollection<SensorValue> SensorValues { get; set; }
        public ObservableCollection<Plant> Plants { get; set; }
        public ObservableCollection<Garden> GardenWatertankSensor { get; set; }
        public ObservableCollection<Garden> GardenAirHumiditySensor { get; set; }
        public ObservableCollection<Garden> GardenTemperatureSensor { get; set; }

        public override void Validate()
        {
            //This method implements the last piece of ValidatableBase
            ValidateProperty(ValidateReadingTimeframe, "The reading timeframe is mandatory and was not filled.", "Reading timeframe");
            ValidateProperty(ValidateSensorName, "The Sensor name is mandatory and was not filled.", "Sensor name");
            ValidateProperty(ValidateGarden, "The garden is mandatory and was not selected.", "Relevant garden");
            ValidateProperty(ValidateSensorType, "The sensor type is mandatory and was not selected.", "Sensor type");
            base.OnPropertyChanged(string.Empty);
        }

        public IValidationMessage ValidateReadingTimeframe(string property)
        {
            const string readingTimeFrameMandatory = "The reading timeframe is mandatory and was not filled.";
            if ( this.ReadingTimeframe == 0)
            {
                var msg = new ValidationErrorMessage(readingTimeFrameMandatory);
                return msg;
            }
            return null;
        }
        public IValidationMessage ValidateSensorName(string property)
        {
            const string sensorNameMandatory = "The Sensor name is mandatory and was not filled.";
            if (string.IsNullOrEmpty(this.SensorName))
            {
                var msg = new ValidationErrorMessage(sensorNameMandatory);
                return msg;
            }
            return null;
        }
        public IValidationMessage ValidateGarden(string property)
        {
            const string gardenMandatory = "The garden is mandatory and was not selected.";
            if (RelevantGarden == null)
            {
                var msg = new ValidationErrorMessage(gardenMandatory);
                return msg;
            }
            return null;
        }
        public IValidationMessage ValidateSensorType(string property)
        {
            const string sensorTypeMandatory = "The sensor type is mandatory and was not selected.";
            if (SensorType == null)
            {
                var msg = new ValidationErrorMessage(sensorTypeMandatory);
                return msg;
            }
            return null;
        }


    }
}
