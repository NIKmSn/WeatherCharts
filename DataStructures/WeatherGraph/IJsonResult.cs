using System;

namespace WeatherGraph
{
    internal interface IJsonResult
    {
        DateTime Date { get; set; }
        double? Temperature { get; set; }
    }
}