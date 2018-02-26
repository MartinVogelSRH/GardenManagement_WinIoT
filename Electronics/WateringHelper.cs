using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GardenManagement.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GardenManagement.Electronics
{
    class WateringHelper
    {
        //This class will contain the logic in order to water the plants.
        //The method that will be called by other classes is "WaterAsync", which handles further watering logic
        public static async Task<bool> WaterAsync(Plant plantToWater)
        {
            if (plantToWater.HowToWater == null || plantToWater.Parameters == null)
            {
                //Error tolerance - if we do not get all data passed in our object, let´s fetch it from the database.
                plantToWater = App._db.Plant
                    .Where(x => x.PlantID == plantToWater.PlantID)
                    .Include(x => x.Parameters)
                    .First();
            }
            if (plantToWater.HowToWater == null || plantToWater.Parameters == null)
            {
                return false;
            }
            //This actually calls the correct ReadingMethod as stored in the database.
            bool value = (bool)typeof(WateringHelper).GetMethod(plantToWater.HowToWater).Invoke(null, plantToWater.Parameters.ToArray());

            if (value)
            {
                plantToWater.LastWateringTime = DateTime.Now;
                App._db.SaveChanges();
            }

            return value;
        }

        public static bool waterDummyPlant(ReadingParameter Parameter1)
        {
            //Just a dummy method which always returns true.
            return true;
        }

    }
}
