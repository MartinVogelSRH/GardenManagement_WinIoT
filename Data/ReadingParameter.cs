using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenManagement.Data
{
    public class ReadingParameter
    {
        public int ID { get; set; }
        public string ParameterName { get; set; }
        public string ParameterString { get; set; }
        public Sensor RelevantSensor { get; set; }
        public Plant RelevantPLant { get; set; }
    }
}
