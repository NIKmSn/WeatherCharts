using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LiveCharts;
using LiveCharts.Wpf;

namespace WeatherGraph
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbStep.SelectedIndex = 0;
            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString("https://api.meteostat.net/v1/history/daily?station=10637&start=2010-01-01&end=2020-01-31&key=nKHFiWZ6");
                JObject googleSearch = JObject.Parse(json);
                IList<JToken> results = googleSearch["data"].Children().ToList();
                IList<JsonResult> searchResults = new List<JsonResult>();
                foreach (JToken result in results)
                {
                    JsonResult searchResult = JsonConvert.DeserializeObject<JsonResult>(result.ToString());
                    searchResults.Add(searchResult);
                }
                dataGridView1.DataSource = searchResults;
            }

            cartesianChart1.LegendLocation = LegendLocation.Bottom;
        }

        private void tsbGraph_Click(object sender, EventArgs e)
        {
            List<JsonResult> res = (List<JsonResult>)dataGridView1.DataSource;

            SeriesCollection series = new SeriesCollection();

            ChartValues<double> tempValues = new ChartValues<double>(res.Select(x => (double)x.Temperature));

            List<String> dates = new List<String>();


            cartesianChart1.AxisX.Clear();
            cartesianChart1.AxisX.Add(new Axis()
                {
                    Title = "Даты",
                    Labels = (res.Select(x => x.Date.ToShortDateString()).ToList())
                }
            );
            LineSeries tempLine = new LineSeries();
            tempLine.Title = "St. Petersburg";
            tempLine.Values = tempValues;

            series.Add(tempLine);
            cartesianChart1.Series = series;
        }
    }
}