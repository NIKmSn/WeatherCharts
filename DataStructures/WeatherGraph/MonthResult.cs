using System;
using Newtonsoft.Json;

namespace WeatherGraph
{
    class MonthResult : IJsonResult
    {
        private DateTime date;
        [JsonProperty(PropertyName = "month")]
        public DateTime Date { get; set; }
        private double temperatureMean;

        [JsonProperty(PropertyName = "temperature_mean")]
        public double? Temperature
        {
            get => temperatureMean;
            set => temperatureMean = value ?? double.NaN;
        }
    }
}
