using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GardenManagement.Data;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Windows.Devices.Spi;
using Windows.Devices.Enumeration;

namespace GardenManagement.Electronics
{
    public class SensorHelper
    {
        //This class will contain the logic in order to read sensors.
        //The method that will be called by other classes is "ReadAsync", which handles further reading
        private static SpiDevice _mcp3008;
        public static async Task<double> ReadAsync(Sensor sensorToRead )
        {
            
            if (sensorToRead.SensorType == null)
            {
                //Error tolerance - if we do not get all data passed in our object, let´s fetch it from the database.
                sensorToRead = App._db.Sensor
                    .Where(x => x.SensorID == sensorToRead.SensorID)
                    .Include(x => x.Parameters)
                    .Include(x => x.SensorType)
                    .First();
            }

            //This actually calls the correct ReadingMethod as stored in the database.
            double value = ((Task<double>)typeof(SensorHelper).GetMethod(sensorToRead.SensorType.HowToRead).Invoke(null, sensorToRead.Parameters.ToArray())).Result;

            //Now that we got the value, we will store it to the database.
            App._db.SensorValue.Add(new SensorValue() { Sensor = sensorToRead, Timestamp = DateTime.Now, Value = value });
            App._db.SaveChanges();
            return value;
        }

        public static async Task<double> readDummySoilMoisture(ReadingParameter Parameter1)
        {
            //Just call the generation method for dummy values...
            return readDummyValue(Parameter1);
        }
        public static async Task<double> readDummyTemperature(ReadingParameter Parameter1)
        {
            //Just call the generation method for dummy values...
            return readDummyValue(Parameter1);
        }
        public static async Task<double> readDummyHumidity(ReadingParameter Parameter1)
        {
            //Just call the generation method for dummy values...
            return readDummyValue(Parameter1);
        }
        public static async Task<double> readDummyWatertankFill(ReadingParameter Parameter1)
        {
            //Just call the generation method for dummy values...
            return readDummyValue(Parameter1);
        }
        public static double readDummyValue(ReadingParameter Parameter1)
        {
            //Let´s just generate some random values around the Parameter given by the user. If the Parameter is not a double, it will be around 50.
            double par1 = 50;
            double.TryParse((string)Parameter1.ParameterString, out par1);
            Random rand = new Random();
            double value = (par1 + rand.Next(-10, 10));
            return value;
        }


        public static async Task<double> readDF0193BehindMCP3008Async(ReadingParameter spiChannel, ReadingParameter mcp3008channel)
        {
            
            SpiConnectionSettings spiSettings = new SpiConnectionSettings(int.Parse(spiChannel.ParameterString));
            spiSettings.ClockFrequency = 1000000; 
            spiSettings.Mode = SpiMode.Mode0;
            
            string spiQuery = SpiDevice.GetDeviceSelector("SPI0");
            var deviceInfo = await DeviceInformation.FindAllAsync(spiQuery);
            if (deviceInfo != null && deviceInfo.Count > 0)
            {
                _mcp3008 = await SpiDevice.FromIdAsync(deviceInfo[0].Id, spiSettings);
                /* Buffer to hold read data*/
                byte[] readBuffer = new byte[3];
                byte[] writeBuffer = new byte[3] { 0x01, 0x80, 0x00 };
                /* Setting up ADC Configuration: from the data sheet */

                /* Read data from the ADC */
                _mcp3008.TransferFullDuplex(writeBuffer, readBuffer);

                //first byte returned is 0 (00000000), 
                //second byte returned we are only interested in the last 2 bits 00000011 (mask of &3) 
                //then shift result 8 bits to make room for the data from the 3rd byte (makes 10 bits total)
                //third byte, need all bits, simply add it to the above result 
                var result = ConvertPulseToInt(readBuffer);
                //LM35 == 10mV/1degC ... 3.3V = 3300.0 mV, 10 bit chip # steps is 2 exp 10 == 1024
                var mv = result;
                return mv;

            }
            else
            {
                //something is wrong - returning stupid value
                return 1234567890;
            }
        }

        public static int ConvertPulseToInt(byte[] data)
        {
            int result = 0;
            try
            {
                result = data[1] & 0x03;
                /* left-shift assignment. Shift the value of result left by 8 */
                result <<= 8;
                /* Add the 3rd byte of data to our result */
                result += data[2];
                return result;
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex.Message);
                return 0;
            }
        }
        public static async Task<double> readDHT11(ReadingParameter gpioPinData)
        {
            //Not implemented yet - returning 0
            return 0;
        }
    }


}
