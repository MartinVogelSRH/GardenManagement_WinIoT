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
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using Microsoft.EntityFrameworkCore;
using GardenManagement.Electronics;
using Windows.UI;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GardenManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlantDetailsPage : Page
    {
        Plant _selectedPlant;

        public PlantDetailsPage()
        {
            this.InitializeComponent();
            //Unfortunately, these values are not possible to be set using xaml.
            linax_Y_SoilMoistureStatistic.Minimum = 0;
            linax_Y_SoilMoistureStatistic.Maximum = 100;
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _selectedPlant = (Plant)e.Parameter;
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            page_PlantDetails.DataContext = _selectedPlant;
            initializePage();
        }

        private void initializePage()
        {
            //Any logic in here does only make sense if the selected plant actually has a soil moisture sensor and that sensor has values.
            if (_selectedPlant != null && _selectedPlant.RelevantSoilMoistureSensor!= null && _selectedPlant.RelevantSoilMoistureSensor.SensorValues.Any())
            {
                //get the latest relevant sensor reading.
                el_MoistureOutside.DataContext = _selectedPlant.RelevantSoilMoistureSensor.SensorValues.OrderByDescending(x => x.Timestamp).FirstOrDefault();
                el_MoistureOutside.UpdateLayout();

                //Unfortunately, I did not find a way so far to change the color of the filling using data binding since Multivalueconverters are not available in UWP.
                if (((SensorValue)el_MoistureOutside.DataContext).Value >= _selectedPlant.WateringThreshold)
                {
                    ((LinearGradientBrush)el_MoistureOutside.Fill).GradientStops[0].Color = Windows.UI.Colors.Green;
                    ((LinearGradientBrush)el_MoistureOutside.Fill).GradientStops[0].Color = Windows.UI.Colors.Green;
                }
                else
                {
                    ((LinearGradientBrush)el_MoistureOutside.Fill).GradientStops[0].Color = Windows.UI.Colors.Red;
                    ((LinearGradientBrush)el_MoistureOutside.Fill).GradientStops[0].Color = Windows.UI.Colors.Red;
                }

                //Let´s also get the sensor values for the statistics.
                List<SensorValue> sensorValues;
                switch (cbox_StatisticTimeframe.SelectedValue)
                {
                    case "Today":
                        sensorValues = _selectedPlant.RelevantSoilMoistureSensor.SensorValues
                            .Where(x => x.Timestamp >= DateTime.Now.Date && x.Timestamp <= DateTime.Now)
                            .OrderBy(x => x.Timestamp)
                            .ToList();
                        (chart_SoilMoistureStatistic.Series[0] as LineSeries).ItemsSource = sensorValues;
                        break;
                    case "Last 7 days":
                        sensorValues = _selectedPlant.RelevantSoilMoistureSensor.SensorValues
                            .Where(x => x.Timestamp >= DateTime.Now.AddDays(-7).Date && x.Timestamp <= DateTime.Now)
                            .OrderBy(x => x.Timestamp)
                            .ToList();
                        (chart_SoilMoistureStatistic.Series[0] as LineSeries).ItemsSource = sensorValues;
                        break;
                    case "Last 30 days":
                        sensorValues = _selectedPlant.RelevantSoilMoistureSensor.SensorValues
                            .Where(x => x.Timestamp >= DateTime.Now.AddDays(-30).Date && x.Timestamp <= DateTime.Now)
                            .OrderBy(x => x.Timestamp)
                            .ToList();
                        (chart_SoilMoistureStatistic.Series[0] as LineSeries).ItemsSource = sensorValues;
                        break;
                    default:
                        sensorValues = _selectedPlant.RelevantSoilMoistureSensor.SensorValues.OrderBy(x => x.Timestamp).ToList();
                        (chart_SoilMoistureStatistic.Series[0] as LineSeries).ItemsSource = sensorValues;
                        break;
                }
            }
        }

        private void btn_ReadSensor_Click(object sender, RoutedEventArgs e)
        {
            if(_selectedPlant.RelevantSoilMoistureSensor != null)
            { 
            SensorHelper.ReadAsync(_selectedPlant.RelevantSoilMoistureSensor);
            initializePage();
            }
        }

        private void btn_WaterNow_Click(object sender, RoutedEventArgs e)
        {
            WateringHelper.WaterAsync(_selectedPlant);

        }

        private void cbox_StatisticTimeframe_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            initializePage();
        }
    }
}
