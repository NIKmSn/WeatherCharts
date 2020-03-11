using System;
using Newtonsoft.Json;

namespace WeatherGraph
{
    class WeekResult : IJsonResult
    {
        public int WeekNum { get; set; }

        [JsonProperty(PropertyName = "date")]
        public DateTime Date { get; set; }

        private double temperature;
        [JsonProperty(PropertyName = "temperature")]
        public double? Temperature
        {
            get => temperature;
            set => temperature = value ?? double.NaN;
        }

        [JsonConstructor]
        public WeekResult(DateTime date, double? temperature)
        {
            Date = date;
            Temperature = temperature;
        }

        public WeekResult(int weekNum, DateTime date, double? temperature)
        {
            WeekNum = weekNum;
            Date = date;
            Temperature = temperature;
        }

        public WeekResult(int weekNum, double? temperature)
        {
            WeekNum = weekNum;
            Temperature = temperature;
        }

    }
}
