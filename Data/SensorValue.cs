using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GardenManagement.Data
{
    public class SensorValue
    {
        public int SensorValueID { get; set; }
        public Sensor Sensor { get; set; }
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
    }
}
