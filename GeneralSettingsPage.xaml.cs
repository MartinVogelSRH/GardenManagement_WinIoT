using GardenManagement.Data;
using GardenManagement.Electronics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GardenManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GeneralSettingsPage : Page
    {
        public GeneralSettingsPage()
        {
            this.InitializeComponent();


        }

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



        private void btn_DropData_Click(object sender, RoutedEventArgs e)
        {
            GardenManagement.Tests.DatabaseTest.recreateDatabase();
            App._db.RejectChanges();
        }

        private void btn_AddData_Click(object sender, RoutedEventArgs e)
        {
            GardenManagement.Tests.DatabaseTest.addDummyData();
        }

        private void piv_Settings_Loaded(object sender, RoutedEventArgs e)
        {
            initializeSensors();
        }

        private void initializeSensors()
        {
            ObservableCollection<Sensor> sensors = new ObservableCollection<Sensor>(App._db.Sensor.ToList());
            //cbox_SelectedSensor.ItemsSource = sensors;
            cbox_SensorToTest.ItemsSource = sensors;
        }

        private async void btn_TestSensor_Click(object sender, RoutedEventArgs e)
        {
            if (cbox_SensorToTest.SelectedItem != null)
            {
                await SensorHelper.ReadAsync((Sensor)cbox_SensorToTest.SelectedItem);
                tbx_Result.Text = ((Sensor)cbox_SensorToTest.SelectedItem).SensorValues.OrderByDescending(x => x.Timestamp).FirstOrDefault().Timestamp.ToString() + ": " + ((Sensor)cbox_SensorToTest.SelectedItem).SensorValues.OrderByDescending(x => x.Timestamp).FirstOrDefault().Value.ToString();
            }
        }
    }
}
