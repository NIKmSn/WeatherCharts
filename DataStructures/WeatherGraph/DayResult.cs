using Newtonsoft.Json;
using System;

namespace WeatherGraph
{
    class DayResult : IJsonResult
    {
        private DateTime date;
        [JsonProperty(PropertyName = "date")]
        public DateTime Date { get; set; }


        private double temperature;
        [JsonProperty(PropertyName = "temperature")]
        public double? Temperature
        {
            get => temperature;
            set => temperature = value ?? double.NaN;
        }
    }
}
