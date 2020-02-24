using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherGraph
{
    class JsonResult
    {
        public DateTime Date { get; set; }
        private double temperature;

        public double? Temperature
        {
            get => temperature;
            set => temperature = value ?? double.NaN;
        }
    }
}
