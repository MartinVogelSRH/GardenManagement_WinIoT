using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenManagement.Data
{
    public class SensorType : ValidatableBase
    {
        public int SensorTypeID { get; set; }
        public string SensorTypeName { get; set; }
        public string HowToRead { get; set; }
        //public string ParameterName1 { get; set; }
        //public string ParameterName2 { get; set; }
        //public string ParameterName3 { get; set; }
        //public string ParameterName4 { get; set; }
        //public string ParameterName5 { get; set; }
        //public string ParameterName6 { get; set; }
        //public string ParameterName7 { get; set; }
        //public string ParameterName8 { get; set; }
        //public string ParameterName9 { get; set; }
        //public string ParameterName10 { get; set; }
        public ObservableCollection<Sensor> Sensors { get; set; }

        public override void Validate()
        {
            //This method implements the last piece of ValidatableBase
            ValidateProperty(ValidateSensorTypeName, "The sensor type name is mandatory and was not filled.", "Sensor type name"); 
            ValidateProperty(ValidateHowToReadEmpty, "The definition of how to read the sensor is mandatory and was not filled.", "How to read");
            ValidateProperty(ValidateHowToRead, "The reading method is not allowed to be \"ReadAsync\".", "How to read");
            base.OnPropertyChanged("Sensor Type");

        }


        public IValidationMessage ValidateSensorTypeName(string property)
        {
            const string sensorTypeNameMandatory = "The sensor type name is mandatory and was not filled.";
            if (string.IsNullOrEmpty(SensorTypeName))
            {
                var msg = new ValidationErrorMessage(sensorTypeNameMandatory);
                return msg;
            }

            return null;
        }
        public IValidationMessage ValidateHowToReadEmpty(string property)
        {
            const string howToReadMandatory = "The definition of how to read the sensor is mandatory and was not filled.";
            if (string.IsNullOrEmpty(HowToRead))
            {
                var msg = new ValidationErrorMessage(howToReadMandatory);
                return msg;
            }
            return null;
        }
        public IValidationMessage ValidateHowToRead(string property)
        {
            string howToReadProperMethod = "The reading method is not allowed to be \"ReadAsync\".";
            if (HowToRead == "ReadAsync")
            {
                var msg = new ValidationErrorMessage(howToReadProperMethod);
                return msg;
            }
            return null;
        }

    }
}
