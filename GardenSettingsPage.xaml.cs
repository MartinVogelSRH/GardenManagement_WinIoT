using GardenManagement.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.EntityFrameworkCore;
using System.Text;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GardenManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GardenSettingsPage : Page
    {
        private ObservableCollection<Garden> gardens;
        private ContentDialog ctdConfirmCancel = new ContentDialog()
        {
            Title = "Confirm Cancellation",
            Content = "Are you sure you want to cancel your changes?",
            CloseButtonText = "No",
            PrimaryButtonText = "Yes"
        };
        private ContentDialog ctdConfirmDeletion = new ContentDialog()
        {
            Title = "Confirm Deletion",
            Content = "This action will result in data loss. Are you sure you want to continue?",
            CloseButtonText = "No",
            PrimaryButtonText = "Yes"
        };
        private ContentDialog ctdSaveMessage = new ContentDialog()
        {
            Title = "Data saved",
            Content = "The data has successfully been saved to the database.",
            PrimaryButtonText = "OK"
        };
        private ObservableCollection<SensorType> sensortypes = new ObservableCollection<SensorType>();
        private ObservableCollection<Sensor> sensors = new ObservableCollection<Sensor>();
        private ObservableCollection<Sensor> sensorsRelevantForGarden = new ObservableCollection<Sensor>();
        public GardenSettingsPage()
        {
            this.InitializeComponent();
        }

        //Handle the Navigation between the different pages using the commend bar
        private void AppBarHome_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
        private void AppBarGeneralSettings_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GeneralSettingsPage));
        }
        private void AppBarGardenSettings_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GardenSettingsPage));
        }



        private void piv_Settings_Loaded(object sender, RoutedEventArgs e)
        {
            //On the first load, initialize all pivots
            initializeSensorTypes();
            initializeGardens();
            initializeSensor();
            initializePlants();
        }
        private void initializeSensorTypes()
        {
            //We need to make sure our entity values are the same as in the physical database.
            App._db.RejectChanges();
            //Now, let´s fetch the values from the db.
            sensortypes = new ObservableCollection<SensorType>(App._db.SensorType.ToList());
            cbox_SelectedSensorType.ItemsSource = sensortypes;

            //For the reading Methods, we will get them using reflection
            List<String> readingMethodsSensorType = (from s in (typeof(Electronics.SensorHelper)).GetMethods(BindingFlags.Public | BindingFlags.Static) select s.Name).ToList();

            cbox_SensorTypeReadingMethod.ItemsSource = readingMethodsSensorType;
        }
        private void initializeGardens()
        {
            //We need to make sure our entity values are the same as in the physical database.
            App._db.RejectChanges();
            //Now, let´s fetch the values from the db.
            gardens = new ObservableCollection<Garden>(App._db.Garden
            .Include(x => x.Plants)
            .ThenInclude(x => x.RelevantSoilMoistureSensor)
            .ThenInclude(x => x.SensorValues)
            .Include(x => x.Plants)
            .ThenInclude(x => x.Parameters)
            .Include(x => x.Sensors)
            .ThenInclude(x => x.Parameters)
            .Include(x => x.AirHumiditySensor)
            .ThenInclude(x => x.SensorValues)
            .Include(x => x.WatertankSensor)
            .ThenInclude(x => x.SensorValues)
            .Include(x => x.TemperatureSensor)
            .ThenInclude(x => x.SensorValues)
            .ToList());
            cbox_SelectedGarden.ItemsSource = gardens;
        }
        private void initializeSensor()
        {
            //We need to make sure our entity values are the same as in the physical database.
            App._db.RejectChanges();
            //Now, let´s fetch the values from the db.
            sensors = new ObservableCollection<Sensor>(App._db.Sensor.ToList());
            //cbox_SelectedSensor.ItemsSource = sensors;
            cbox_SensorSensorType.ItemsSource = sensortypes;
        }
        private void initializePlants()
        {
            List<String> wateringMethodsPlants = (from s in (typeof(Electronics.WateringHelper)).GetMethods(BindingFlags.Public | BindingFlags.Static) select s.Name).ToList();

            cbox_PlantWateringMethod.ItemsSource = wateringMethodsPlants;
            App._db.RejectChanges();
        }



        private void ensureProperGridPlantsLayout()
        {
            //With ensuring that the width of the grids items equals the grids with devided by the number of columns, we ensure that every Item is at it´s proper spot.
            //Here, we are using how a gridview renders its items. It basically keeps adding them horizontally until there is no more space.
            Garden selectedGarden = (Garden)cbox_PlantSelectedGarden.SelectedItem;
            if (selectedGarden != null && grid_Plants.ItemsPanelRoot != null)
            {
                double NewWidth = grid_Plants.ActualWidth / selectedGarden.NumberOfColumns;
                ItemsWrapGrid itemsgrid_grid_Plants = (ItemsWrapGrid)grid_Plants.ItemsPanelRoot;
                itemsgrid_grid_Plants.ItemWidth = NewWidth;
            }
        }

        private void grid_Plants_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ensureProperGridPlantsLayout();
        }

        private void btn_NewSensorType_Click(object sender, RoutedEventArgs e)
        {
            //Just making up dummy data and adding this for the user to change it.
            SensorType newSensor = new SensorType() { SensorTypeName = "Please enter a name" };
            sensortypes.Add(newSensor);
            cbox_SelectedSensorType.SelectedItem = newSensor;
        }
        private void btn_NewGarden_Click(object sender, RoutedEventArgs e)
        {
            //Just making up dummy data and adding this for the user to change it.
            Garden newGarden = new Garden() { GardenName = "Please enter a name" };
            gardens.Add(newGarden);
            cbox_SelectedGarden.SelectedItem = newGarden;
        }
        private void btn_NewSensor_Click(object sender, RoutedEventArgs e)
        {
            //Just making up dummy data and adding this for the user to change it.
            Sensor newSensor = new Sensor() { SensorName = "Please enter a name", Parameters = new ObservableCollection<ReadingParameter>()};
            ((ObservableCollection<Sensor>)cbox_SelectedSensor.ItemsSource).Add(newSensor);
            //sensors.Add(newSensor);
            cbox_SelectedSensor.SelectedItem = newSensor;
            
        }

        private async void btn_SensorTypeSave_Click(object sender, RoutedEventArgs e)
        {
            
            SensorType updatedSensorType = (SensorType)cbox_SelectedSensorType.SelectedItem;
            //We will need to check the validity of the item to save first.
            updatedSensorType.Validate();
            if (!updatedSensorType.HasValidationMessageType<ValidationErrorMessage>())
            {
                //No validation errors - let´s save the data!
                if (updatedSensorType.SensorTypeID == 0)
                {
                    //It is a new item, so we need to add it.
                    App._db.SensorType.Add(updatedSensorType);
                }
                else
                {
                    //We are updating an existing item.
                    App._db.SensorType.Update(updatedSensorType);
                }
                App._db.SaveChanges();
                await ctdSaveMessage.ShowAsync();
            }

            else
            {
                //there have been validation errors - these need to be shown to the user
                showValidationMessages(updatedSensorType.ValidationMessages);
            }
            
        }
        private async void btn_GardenSave_Click(object sender, RoutedEventArgs e)
        { 
            Garden updatedGarden = (Garden)cbox_SelectedGarden.SelectedItem;
            //We will need to check the validity of the item to save first.
            updatedGarden.Validate();
            if (!updatedGarden.HasValidationMessageType<ValidationErrorMessage>())
            {
                //No validation errors - let´s save the data!
                if (updatedGarden.GardenID == 0)
                {   //It is a new item, so we need to add it.
                    //However, we first have to ensure all plant slots are filled.
                    updatedGarden.fill_plants();
                    App._db.Garden.Add(updatedGarden);
                }
                else
                {
                    //It is an existing item. We need to check if any plants have to be deleted.
                    List<Plant> plantsToBeDeleted = (from p in updatedGarden.Plants where p.LocationColumn > updatedGarden.NumberOfColumns || p.LocationRow > updatedGarden.NumberOfRows select p).ToList();
                    if (plantsToBeDeleted.Count != 0)
                    {
                        //Plants have to be deleted - we need confirmation from the user for this.
                        if (await ctdConfirmDeletion.ShowAsync() == ContentDialogResult.Primary)
                        {
                            //Deletion confirmed - let´s remove the plants and update the garden
                            App._db.Plant.RemoveRange(plantsToBeDeleted);
                            App._db.Garden.Update(updatedGarden);
                            
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        //No plants have to be deleted - but maybe some have to be added. We will tell the model to take care of that
                        updatedGarden.fill_plants();
                        App._db.Garden.Update(updatedGarden);
                    }

                }
                App._db.SaveChanges();
                await ctdSaveMessage.ShowAsync();
            }

            else
            {
                //there have been validation errors - these need to be shown to the user
                showValidationMessages(updatedGarden.ValidationMessages);
            }



        }
        private async void btn_SensorSave_Click(object sender, RoutedEventArgs e)
        {
            Sensor updatedSensor = (Sensor)cbox_SelectedSensor.SelectedItem;
            updatedSensor.RelevantGarden = (Garden)cbox_SensorSelectedGarden.SelectedItem;
            //We will need to check the validity of the item to save first.
            updatedSensor.Validate();
            
            if (!updatedSensor.HasValidationMessageType<ValidationErrorMessage>())
            {
                //No validation errors - let´s save the data!
                if (updatedSensor.SensorID == 0)
                {
                    //It is a new item, so we need to add it.
                    App._db.Sensor.Add(updatedSensor);
                }
                else
                {
                    //We are updating an existing item.
                    App._db.Sensor.Update(updatedSensor);
                }
                App._db.SaveChanges();
                await ctdSaveMessage.ShowAsync();
            }
            
            else
            {
                //there have been validation errors - these need to be shown to the user
                showValidationMessages(updatedSensor.ValidationMessages);
            }

        }
        private async void btn_PlantSave_Click(object sender, RoutedEventArgs e)
        {
            Plant updatedPlant = (Plant)grid_Plants.SelectedItem;
            //We will need to check the validity of the item to save first.
            updatedPlant.Validate();
            if (!updatedPlant.HasValidationMessageType<ValidationErrorMessage>())
            {
                //There is no "new plant" in the view - all plants are technically already existing.
                if (updatedPlant != null)
                {
                    App._db.Plant.Update(updatedPlant);
                }
                App._db.SaveChanges();
                await ctdSaveMessage.ShowAsync();
            }
            else
            {
                //there have been validation errors - these need to be shown to the user
                showValidationMessages(updatedPlant.ValidationMessages);
            }
        }

        private void showValidationMessages(Dictionary<string, List<IValidationMessage>> validationMessages)
        {
            //This goes through the validation messages of a certain object and displays them to the user.
            StringBuilder output = new StringBuilder();
            output.AppendLine("The following issues occured during validation:");
            foreach (var validationMessage in validationMessages)
            {
                if (validationMessage.Value.Count > 0)
                {
                    output.AppendLine(validationMessage.Key + ":");
                    foreach (var message in validationMessage.Value)
                    {
                        output.AppendLine(message.Message);
                    }
                }

            }
            new MessageDialog(output.ToString()).ShowAsync();
        }



        private async void btn_SensorTypeCancel_Click(object sender, RoutedEventArgs e)
        {
            //Cancellation requires user confirmation. Let´s get this and then initialize our Sensor Types.
            if (await ctdConfirmCancel.ShowAsync() == ContentDialogResult.Primary)
            {
                initializeSensorTypes();
            }
        }
        private async void btn_GardenCancel_Click(object sender, RoutedEventArgs e)
        {
            //Cancellation requires user confirmation. Let´s get this and then initialize our Sensor Types.
            if (await ctdConfirmCancel.ShowAsync() == ContentDialogResult.Primary)
            {
                initializeGardens();
            }
        }
        private async void btn_SensorCanel_Click(object sender, RoutedEventArgs e)
        {
            //Cancellation requires user confirmation. Let´s get this and then initialize our Sensor Types.
            if (await ctdConfirmCancel.ShowAsync() == ContentDialogResult.Primary)
            {
                initializeSensor();
            }
        }
        private async void btn_PlantCancel_Click(object sender, RoutedEventArgs e)
        {
            //Cancellation requires user confirmation. Let´s get this and then initialize our Sensor Types.
            if (await ctdConfirmCancel.ShowAsync() == ContentDialogResult.Primary)
            {
                initializePlants();
            }
        }

        private void cbox_SensorSensorType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbox_SensorSensorType.SelectedItem != null)
            {
                if (((Sensor)cbox_SelectedSensor.SelectedItem).Parameters == null)
                {
                    ((Sensor)cbox_SelectedSensor.SelectedItem).Parameters = new ObservableCollection<ReadingParameter>();
                }

                if (((Sensor)cbox_SelectedSensor.SelectedItem).Parameters.Count == 0)
                {
                    ParameterInfo[] parameterInfoReadingMethod = (typeof(Electronics.SensorHelper)).GetMethod(((SensorType)cbox_SensorSensorType.SelectedItem).HowToRead).GetParameters();
                    foreach (ParameterInfo item in parameterInfoReadingMethod)
                    {
                        ((Sensor)cbox_SelectedSensor.SelectedItem).Parameters.Add(new ReadingParameter()
                        {
                            ParameterName = item.Name
                        }
                        );
                    }
                }
                lview_PlantSensorParameters.ItemsSource = ((Sensor)cbox_SelectedSensor.SelectedItem).Parameters; 
            }
            else
            {
                lview_PlantSensorParameters.ItemsSource = null;
            }

        }

        private void cbox_PlantWateringMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbox_PlantWateringMethod.SelectedItem != null)
            {
                if ((((Plant)grid_Plants.SelectedItem).Parameters == null) || ((Plant)grid_Plants.SelectedItem).HowToWater != (String)cbox_PlantWateringMethod.SelectedItem)
                {
                    ((Plant)grid_Plants.SelectedItem).Parameters = new ObservableCollection<ReadingParameter>();
                }

                if (((Plant)grid_Plants.SelectedItem).Parameters.Count == 0)
                {
                    ParameterInfo[] parameterInfoReadingMethod = (typeof(Electronics.WateringHelper)).GetMethod(((Plant)grid_Plants.SelectedItem).HowToWater).GetParameters();
                    foreach (ParameterInfo item in parameterInfoReadingMethod)
                    {
                        ((Plant)grid_Plants.SelectedItem).Parameters.Add(new ReadingParameter()
                        {
                            ParameterName = item.Name
                        }
                        );
                    }
                }
                //lview_PlantWateringParameters.ItemsSource = ((Plant)grid_Plants.SelectedItem).Parameters;
            }
            else
            {
                //lview_PlantWateringParameters.ItemsSource = null;
            }
        }
    }
}
