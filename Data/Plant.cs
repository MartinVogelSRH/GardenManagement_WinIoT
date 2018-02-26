using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenManagement.Data
{
    public class Plant : ValidatableBase
    {
        public int PlantID { get; set; }
        public Garden RelevantGarden { get; set; }
        public DateTime LastWateringTime { get; set; }
        public int LocationColumn { get; set; }
        public int LocationRow { get; set; }
        public string PlantName { get; set; }
        public int WateringThreshold { get; set; }
        public Sensor RelevantSoilMoistureSensor { get; set; }
        public string HowToWater { get; set; }
        public ObservableCollection<ReadingParameter> Parameters { get; set; }

        public override void Validate()
        {
            //This method implements the last piece of ValidatableBase
            ValidateProperty(ValidatePlantName, "The plant name is mandatory and was not filled.", "Plant name");
            ValidateProperty(ValidateLocationColumn, "The Column has to be between 1 and " + RelevantGarden.NumberOfColumns.ToString() + ".", "Location column");
            ValidateProperty(ValidateLocationRow, "The Column has to be between 1 and " + RelevantGarden.NumberOfRows.ToString() + ".", "Location row");
            ValidateProperty(ValidateWateringThreshold, "The watering threshold has to be between 1 and 100.", "Watering threshold");
            ValidateProperty(ValidateHowToWater, "The watering Method is not allowed to be \"WaterAsync\".", "Watering method");
            base.OnPropertyChanged("Plant");
        }

        public IValidationMessage ValidatePlantName(string property)
        {
            string plantNameMandatory = "The plant name is mandatory and was not filled.";
            if (string.IsNullOrEmpty(PlantName))
            {
                var msg = new ValidationErrorMessage(plantNameMandatory);
                return msg;
            }
            return null;
        }
        public IValidationMessage ValidateLocationColumn(string property)
        {
            string locationColumnInBounds = "The Column has to be between 1 and " + RelevantGarden.NumberOfColumns.ToString() + ".";
            if (1 > LocationColumn || LocationColumn > RelevantGarden.NumberOfColumns)
            {
                var msg = new ValidationErrorMessage(locationColumnInBounds);
                return msg;
            }
            return null;
        }
        public IValidationMessage ValidateLocationRow(string property)
        {
            string locationRowInBounds = "The Column has to be between 1 and " + RelevantGarden.NumberOfRows.ToString() + ".";
            if (1 > LocationRow || LocationRow > RelevantGarden.NumberOfRows)
            {
                var msg = new ValidationErrorMessage(locationRowInBounds);
                return msg;
            }
            return null;
        }
        public IValidationMessage ValidateWateringThreshold(string property)
        {
            string wateringThresholdMandatory = "The watering threshold has to be between 1 and 100.";
            if (1 > WateringThreshold || WateringThreshold > 100)
            {
                var msg = new ValidationErrorMessage(wateringThresholdMandatory);
                return msg;
            }
            return null;
        }
        public IValidationMessage ValidateHowToWater(string property)
        {
            string howToWaterProperMethod = "The watering Method is not allowed to be \"WaterAsync\".";
            if (HowToWater == "WaterAsync")
            {
                var msg = new ValidationErrorMessage(howToWaterProperMethod);
                return msg;
            }
            return null;
        }


    }
}
