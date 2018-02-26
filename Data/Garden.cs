using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GardenManagement.Data
{
    public class Garden : ValidatableBase
    {
        public int GardenID { get; set; }
        public string GardenName { get; set; }
        public int NumberOfColumns { get; set; }
        public int NumberOfRows { get; set; }
        public ObservableCollection<Plant> Plants { get; set; }

        
        public ObservableCollection<Sensor> Sensors { get; set; }

        [InverseProperty("GardenWatertankSensor")]
        public Sensor WatertankSensor { get; set; }

        [InverseProperty("GardenAirHumiditySensor")]
        public Sensor AirHumiditySensor { get; set; }

        [InverseProperty("GardenTemperatureSensor")]
        public Sensor TemperatureSensor { get; set; }

        public void fill_plants()
        {
            //Check, whether all posible slots are filled with plants. If not, fill them with an empty plant.
            Plant emptyPlant = new Plant();
            emptyPlant.PlantName = "Empty Slot";
            if (Plants != null)
            {
                // we need to order our plants for the for-loops later
                Plants = new ObservableCollection<Plant>(Plants.OrderBy(x => x.LocationColumn).OrderBy(x => x.LocationRow).ToList());
            }
            else
            {
                // no plants exist at all yet (new garden). Let´s create them
                Plants = new ObservableCollection<Plant>();
            }
            int CurrentItemID = 0;
            //We will go through all possible slots in the garden and check, whether or not there is a plant available.
            for (int i = 1; i <= NumberOfRows; i++)
            {
                for (int j = 1; j <= NumberOfColumns; j++)
                {
                    Plant CurrentItem = null;
                    try { CurrentItem = (Plant)Plants[CurrentItemID]; }
                    catch { }
                    if (CurrentItem == null || CurrentItem.LocationRow != i || CurrentItem.LocationColumn != j)
                    {
                        //No plant exists at this location. Let´s create it.
                        emptyPlant.LocationRow = i;
                        emptyPlant.LocationColumn = j;
                        Plants.Insert(CurrentItemID, emptyPlant);
                        emptyPlant = new Plant();
                        emptyPlant.PlantName = "Empty Slot";
                        emptyPlant.Parameters = new ObservableCollection<ReadingParameter>();
                    }
                    else { }
                    CurrentItemID++;
                    CurrentItem = null;
                }
            }
            Plants = new ObservableCollection<Plant>(Plants.OrderBy(x => x.LocationColumn).OrderBy(x => x.LocationRow).ToList());
        }

        public override void Validate()
        {
            //This method implements the last piece of ValidatableBase 
            ValidateProperty(ValidateGardenName, "The garden name is mandatory and was not filled.", "Garden name");
            ValidateProperty(ValidateNumberOfColumns, "The number of columns has to be between 1 and 10.", "Number of columns");
            ValidateProperty(ValidateNumberOfRows, "The number of rows has to be between 1 and 10.", "Number of rows");
            base.OnPropertyChanged("Garden");
        }

        public IValidationMessage ValidateGardenName(string property)
        {
            const string gardenNameMandatory = "The garden name is mandatory and was not filled.";
            if (string.IsNullOrEmpty(GardenName))
            {
                var msg = new ValidationErrorMessage(gardenNameMandatory);
                return msg;
            }
            return null;
        }
        public IValidationMessage ValidateNumberOfColumns(string property)
        {
            const string columnsOneToTen = "The number of columns has to be between 1 and 10.";
            if ( 1 > NumberOfColumns || NumberOfColumns > 10)
            {
                var msg = new ValidationErrorMessage(columnsOneToTen);
                return msg;
            }
            return null;
        }
        public IValidationMessage ValidateNumberOfRows(string property)
        {
            const string rowsOneToTen = "The number of rows has to be between 1 and 10.";
            if (1 > NumberOfRows || NumberOfRows > 10)
            {
                var msg = new ValidationErrorMessage(rowsOneToTen);
                return msg;
            }
            return null;
        }


    }

}
