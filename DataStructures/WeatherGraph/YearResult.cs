using System;
using Newtonsoft.Json;


namespace WeatherGraph
{
    class YearResult : IJsonResult
    {
        [JsonProperty(PropertyName = "month")]
        public DateTime Date { get; set; }

        private double temperature;
        [JsonProperty(PropertyName = "temperature_mean")]
        public double? Temperature
        {
            get => temperature;
            set => temperature = value ?? double.NaN;
        }

        [JsonConstructor]
        public YearResult(DateTime date, double? temperature)
        {
            Date = date;
            Temperature = temperature;
        }
    }
}
