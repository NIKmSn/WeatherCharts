using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WeatherGraph
{
    static class Weather
    {
        public enum GraphStep
        {
            Day,
            Week,
            Month,
            Year
        };

        public static IList<IJsonResult> GetWeatherData(DateTime start, DateTime end, GraphStep step)
        {
            string key = GenerateJson("../../../WeatherAPI.txt", start, end, step);
            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString(key);
                JObject search = JObject.Parse(json);
                IList<JToken> results = search["data"].Children().ToList();
                switch (step)
                {
                    case GraphStep.Day:
                    {
                        return new List<IJsonResult>(results
                            .Select(result => JsonConvert.DeserializeObject<DayResult>(result.ToString())).ToList());
                    }
                    case GraphStep.Week:
                    {
                        List<IJsonResult> list = new List<IJsonResult>(results.Select(result =>
                            JsonConvert.DeserializeObject<WeekResult>(result.ToString())).ToList());
                        return GetWeekResults(list);
                    }
                    case GraphStep.Month:
                    {
                        return new List<IJsonResult>(results.Select(result =>
                            JsonConvert.DeserializeObject<MonthResult>(result.ToString())).ToList());
                    }
                    case GraphStep.Year:
                    {
                        List<IJsonResult> list = new List<IJsonResult>(results.Select(result =>
                            JsonConvert.DeserializeObject<YearResult>(result.ToString())).ToList());
                            return GetYearResults(list);
                    }
                    default: throw new Exception("Wrong step of change");
                }
            }
        }

        private static IList<IJsonResult> GetWeekResults(List<IJsonResult> list)
        {
            CultureInfo cultureInfo = new CultureInfo("ru-RU");
            //Группировка по неделям
            var summedResult = list.GroupBy(x => new
                {
                    x.Date.Year,
                    x.Date.Month,
                    Week = GetWeekOfMonth(x.Date)
                })
                .Select(g =>
                    new
                    {
                        Date = $"{g.Key.Month}/{g.Key.Year}",
                        WeekNum = g.Key.Week,
                        Sum = g.Sum(item => item.Temperature) / g.Count()
                    });
            List<IJsonResult> newList = new List<IJsonResult>();
            foreach (var res in summedResult)
            {
                newList.Add(new WeekResult(
                    res.WeekNum,
                    DateTime.Parse(res.Date),
                    Double.Parse(res.Sum?.ToString("F", cultureInfo.DateTimeFormat))
                ));
            }

            return newList;
        }

        private static IList<IJsonResult> GetYearResults(List<IJsonResult> list)
        {
            CultureInfo cultureInfo = new CultureInfo("ru-RU");
            //Группировка по годам
            var summedResult = list.GroupBy(x => x.Date.Year)
                .Select(g =>
                    new
                    {
                        Date = $"01/{g.Key}",
                        Sum = g.Sum(item => item.Temperature) / g.Count()
                    });
            List<IJsonResult> newList = new List<IJsonResult>();
            foreach (var res in summedResult)
            {
                newList.Add(new YearResult(
                   DateTime.Parse(res.Date),
                    Double.Parse(res.Sum?.ToString("F", cultureInfo.DateTimeFormat))
                ));
            }

            return newList;
        }

        private static string GenerateJson(string keyPath, DateTime start, DateTime end, GraphStep step)
        {
            string _step = "";
            string startDate = "";
            string endDate = "";
            switch (step)
            {
                case GraphStep.Day:
                case GraphStep.Week:
                {
                    _step = "daily";
                    startDate = start.ToString("yyyy-MM-dd");
                    endDate = end.ToString("yyyy-MM-dd");
                    break;
                }
                case GraphStep.Month:
                case GraphStep.Year:
                    {
                    _step = "monthly";
                    startDate = start.ToString("yyyy-MM");
                    endDate = end.ToString("yyyy-MM");
                    break;
                }
            }

            string id = File.ReadAllLines(keyPath)[0]; //id метеостанции
            string apiKey = File.ReadAllLines("../../../WeatherAPI.txt")[1]; // api Key
            return
                $@"https://api.meteostat.net/v1/history/{_step}?station={id}&start={startDate}&end={endDate}&key={apiKey}";
        }

        static int GetWeekNumberOfMonth(DateTime date)
        {
            date = date.Date;
            DateTime firstMonthDay = new DateTime(date.Year, date.Month, 1);
            DateTime firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            if (firstMonthMonday > date)
            {
                firstMonthDay = firstMonthDay.AddMonths(-1);
                firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            }
            return (date - firstMonthMonday).Days / 7 + 1;
        }

        public static int GetWeekOfMonth(DateTime date)
        {
            DateTime beginningOfMonth = new DateTime(date.Year, date.Month, 1);

            while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                date = date.AddDays(1);

            return (int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / 7f) + 1;
        }
    }
}
