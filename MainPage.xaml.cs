using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using GardenManagement.Data;
using Microsoft.EntityFrameworkCore;
using GardenManagement.Electronics;
using Windows.UI.Popups;
using System.Collections.ObjectModel;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GardenManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            initializePage();
            ensureProperGridPlantsLayout();
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
            Frame.Navigate(typeof(GardenSettingsPage), cbox_SelectedGarden.SelectedItem);
        }

        private async void btn_PlantDisplay_Click(object sender, RoutedEventArgs e)
        {
            if (grid_Plants.SelectedItem != null)
            {
                Frame.Navigate(typeof(PlantDetailsPage), (Plant)grid_Plants.SelectedItem);
            }
            else
            {
                await new MessageDialog("Please select a plant first.").ShowAsync();

            }
        }

        private async void initializePage()
        {
            //Let´s ensure all our entities are just like they actually are in the database.
            App._db.RejectChanges();
            //Now, we will get data from the database
            List<Garden> gardens = App._db.Garden
                .Include(x => x.Plants)
                .ThenInclude(x => x.RelevantSoilMoistureSensor)
                .ThenInclude(x => x.SensorValues)
                .Include(x => x.Sensors)
                .ThenInclude(x => x.Parameters)
                .Include(x => x.AirHumiditySensor)
                .ThenInclude(x => x.SensorValues)
                .Include(x => x.WatertankSensor)
                .ThenInclude(x => x.SensorValues)
                .Include(x => x.TemperatureSensor)
                .ThenInclude(x => x.SensorValues)
                .ToList();

            if (gardens.Count != 0)
            {
                cbox_SelectedGarden.ItemsSource = gardens;
                cbox_SelectedGarden.SelectedIndex = 0;
            }
            else
            {
                //No garden was added so far - so there is no data for this page.
                ContentDialog emptyDatabase = new ContentDialog()
                {
                    Title = "First run",
                    Content = "It seems like you are running this Application for the first time / with an empty database. \r\nIf you want to check the features of the application, select \"Add Sample Data\" \r\n If you want to set up an own garden, please select \"Open Settings\"",
                    PrimaryButtonText = "Add Sample Data",
                    SecondaryButtonText = "Open settings",
                    CloseButtonText = "Do nothing",
                };
                switch (await emptyDatabase.ShowAsync())
                {
                    case ContentDialogResult.None:
                        break;
                    case ContentDialogResult.Primary:
                        Tests.DatabaseTest.addDummyData();
                        initializePage();
                        break;
                    case ContentDialogResult.Secondary:
                        Frame.Navigate(typeof(GardenSettingsPage), cbox_SelectedGarden.SelectedItem);
                        break;
                    default:
                        break;
                }
            }
        }
        private void cbox_SelectedGarden_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Garden selectedGarden = (Garden)cbox_SelectedGarden.SelectedItem;
            //We need to ensure all slots in our garden are filled with a plant or the dummy plant.
            selectedGarden.fill_plants();
            //The plants of the garden become the ItemSource for the gridview.
            grid_Plants.ItemsSource = selectedGarden.Plants;

            getOverallSensorData();
        }

        private void grid_Plants_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ensureProperGridPlantsLayout();
        }

        private void ensureProperGridPlantsLayout()
        {
            //With ensuring that the width of the grids items equals the grids with devided by the number of columns, we ensure that every Item is at it´s proper spot.
            //Here, we are using how a gridview renders its items. It basically keeps adding them horizontally until there is no more space.
            Garden selectedGarden = (Garden)cbox_SelectedGarden.SelectedItem;
            if (selectedGarden != null && grid_Plants.ItemsPanelRoot != null)
            {
                double NewWidth = grid_Plants.ActualWidth / selectedGarden.NumberOfColumns;
                ItemsWrapGrid itemsgrid_grid_Plants = (ItemsWrapGrid)grid_Plants.ItemsPanelRoot;
                itemsgrid_grid_Plants.ItemWidth = NewWidth;
            }
            grid_Plants.UpdateLayout();
        }

        private void grid_Plants_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            ensureProperGridPlantsLayout();
        }

        private void getOverallSensorData(bool forceUpdate = false)
        {
            //Here we take care of the overall sensors. For each sensor we check the user actually maintained a sensor. Then we check if there are any readings so far. If so, we use those. If not, we create a new reading.
            Garden selectedGarden = (Garden)cbox_SelectedGarden.SelectedItem;
            if (selectedGarden.WatertankSensor != null)
            {
                if (selectedGarden.WatertankSensor.SensorValues != null && selectedGarden.WatertankSensor.SensorValues.Count != 0 && !forceUpdate)
                {
                    cvs_Watertank.DataContext = selectedGarden.WatertankSensor.SensorValues.OrderByDescending(x => x.Timestamp).First();
                    tbx_Watertank.DataContext = cvs_Watertank.DataContext;
                }
                else
                {
                    SensorHelper.ReadAsync(selectedGarden.WatertankSensor);
                    cvs_Watertank.DataContext = selectedGarden.WatertankSensor.SensorValues.OrderByDescending(x => x.Timestamp).First();
                    tbx_Watertank.DataContext = cvs_Watertank.DataContext;
                }
            }
            else
            {
                cvs_Watertank.DataContext = null;
                tbx_Watertank.DataContext = null;
            }
            if (selectedGarden.TemperatureSensor != null)
            {
                if (selectedGarden.TemperatureSensor.SensorValues != null && selectedGarden.TemperatureSensor.SensorValues.Count != 0 && !forceUpdate)
                {
                    tbx_Temperature.DataContext = selectedGarden.TemperatureSensor.SensorValues.OrderByDescending(x => x.Timestamp).First();
                }
                else
                {
                    SensorHelper.ReadAsync(selectedGarden.TemperatureSensor);
                    tbx_Temperature.DataContext = selectedGarden.TemperatureSensor.SensorValues.OrderByDescending(x => x.Timestamp).First();
                }
            }
            else
            {
                tbx_Temperature.DataContext = null;
            }
            if (selectedGarden.AirHumiditySensor != null)
            {
                if (selectedGarden.AirHumiditySensor.SensorValues != null && selectedGarden.AirHumiditySensor.SensorValues.Count != 0 && !forceUpdate)
                {
                    tbx_Humidity.DataContext = selectedGarden.AirHumiditySensor.SensorValues.OrderByDescending(x => x.Timestamp).First();
                }
                else
                {
                    SensorHelper.ReadAsync(selectedGarden.AirHumiditySensor);
                    tbx_Humidity.DataContext = selectedGarden.AirHumiditySensor.SensorValues.OrderByDescending(x => x.Timestamp).First();
                }
            }
            else
            {
                tbx_Humidity.DataContext = null;
            }
        }

        private void btn_RefreshOverall_Click(object sender, RoutedEventArgs e)
        {
            getOverallSensorData(true);
        }

        private void grid_Plants_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Frame.Navigate(typeof(PlantDetailsPage), (Plant)grid_Plants.SelectedItem);
        }
    }
}