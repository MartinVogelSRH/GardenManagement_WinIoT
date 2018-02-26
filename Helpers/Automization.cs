using GardenManagement.Data;
using GardenManagement.Electronics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace GardenManagement.Helpers
{
    public class Automization
    {
        DatabaseContext db = new DatabaseContext();
        DispatcherTimer backgroundRunner;
        public Automization()
        {
            //For automization, we will need a task running in background. However, it should be threadsafe so that we can use db --->DispatcherTimer
            backgroundRunner = new DispatcherTimer();
            backgroundRunner.Tick += backgroundRunner_Tick;
            int interval = 0;
            if(db.Sensor.Any())
            { 
                interval = db.Sensor.OrderBy(x => x.ReadingTimeframe).FirstOrDefault().ReadingTimeframe;
            }
            if (interval > 0)
            {
                backgroundRunner.Interval = TimeSpan.FromSeconds(interval);
                
            }
            else
            {
                //No Sensors found so far. We will still start the background worker. We will start it with an interval that does not equal a full second in order to be able to later check this is our defauls interval.
                backgroundRunner.Interval = TimeSpan.FromMilliseconds(30005);
            }
            backgroundRunner.Start();
        }

        private void backgroundRunner_Tick(object sender, object e)
        {
            //First, let´s check if the background task was started with the default interval, because no sensor was available and change that if possible.
            if (db.Sensor.Any())
            {
                if (backgroundRunner.Interval.Milliseconds == 30005)
                {
                    int interval = db.Sensor.OrderBy(x => x.ReadingTimeframe).FirstOrDefault().ReadingTimeframe;
                    if (interval > 0)
                    {
                        backgroundRunner.Interval = TimeSpan.FromSeconds(interval);
                    }
                }

                //Now we get all sensors of which the next reading is due and read them
                List<Sensor> sensorsToRead = db.Sensor
                    .Where(x => x.SensorValues.OrderByDescending(y => y.Timestamp).FirstOrDefault().Timestamp < DateTime.Now.AddSeconds(0 - x.ReadingTimeframe)).ToList();
                foreach (Sensor sensorToRead in sensorsToRead)
                {
                    SensorHelper.ReadAsync(sensorToRead);
                }

                //since we now have all our sensor readings, we check, whether the plants need water or not.
                List<Plant> plantsToWater = db.Plant
                    .Where(x => x.WateringThreshold > x.RelevantSoilMoistureSensor.SensorValues.OrderByDescending(y => y.Timestamp).FirstOrDefault().Value).ToList();

                foreach (Plant plantToWater in plantsToWater)
                {
                    WateringHelper.WaterAsync(plantToWater);
                }
            }
        }
    }
}
