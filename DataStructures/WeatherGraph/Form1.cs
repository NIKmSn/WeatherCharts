using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
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
            #region Добавление столбца с датами
            DataGridViewColumn date = new DataGridViewTextBoxColumn();
            date.DataPropertyName = "Date";
            date.ReadOnly = true;
            date.Name = "Date";
            dataGridView1.Columns.Add(date);
            dataGridView1.Columns[0].Width = 90;
            #endregion

            #region Добавление столбца с температурой
            DataGridViewColumn temperature = new DataGridViewTextBoxColumn();
            temperature.DataPropertyName = "Temperature";
            temperature.ReadOnly = true;
            temperature.Name = "Temperature";
            dataGridView1.Columns.Add(temperature);
            dataGridView1.Columns[1].Width = 100;
            dataGridView1.Columns[1].DefaultCellStyle.Format = "##0.#";
            #endregion

            #region Установка стандартных параметров построения графика
            cbStep.SelectedIndex = 0; //Шаг изменения - День
            dataGridView1.DataSource = Weather.GetWeatherData(
                new DateTime(2010, 01, 01),
                new DateTime(2020, 1, 31),
                Weather.GraphStep.Day);
            #endregion

            cartesianChart1.LegendLocation = LegendLocation.Bottom;
        }

        private void tsbGraph_Click(object sender, EventArgs e)
        {
            List<IJsonResult> res = (List<IJsonResult>)dataGridView1.DataSource;
            SeriesCollection series = new SeriesCollection();

            #region Добавление координат оси X
            cartesianChart1.AxisX.Clear();
            switch (cbStep.SelectedIndex)
            {
                //Day
                case 0:
                {
                    cartesianChart1.AxisX.Add(new Axis()
                        {
                            Title = "Даты",
                            Labels = (res.Select(x => x.Date.ToShortDateString()).ToList())
                        }
                    );
                    break;
                }
                //Week
                case 1:
                {
                    List<WeekResult> weeks = res.Cast<WeekResult>().ToList();
                    cartesianChart1.AxisX.Add(new Axis()
                        {
                            Title = "Даты",
                            Labels = weeks.Select(x => $"{x.WeekNum}/{x.Date.ToString("MMMM yyyy")}").ToList()
                        }
                    );
                    break;

                }
                //Month
                case 2:
                {
                    cartesianChart1.AxisX.Add(new Axis()
                        {
                            Title = "Даты",
                            Labels = (res.Select(x => $"{x.Date.ToString("MMMM yyyy")}").ToList())
                        }
                    );
                    break;
                    }
                //Year
                case 3:
                {
                    List<YearResult> years = res.Cast<YearResult>().ToList();
                    cartesianChart1.AxisX.Add(new Axis()
                        {
                            Title = "Даты",
                            Labels = years.Select(x => x.Date.Year.ToString()).ToList()
                        }
                    );
                    break;

                }
            }
            #endregion
            
            ChartValues<double> tempValues = new ChartValues<double>(res.Select(x => (double)x.Temperature));
            LineSeries tempLine = new LineSeries();
            tempLine.Title = "Температура";
            tempLine.Values = tempValues;
            series.Add(tempLine);
            cartesianChart1.Series = series;
        }

        private void cbStep_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO: Установка значений дат пользователем
            switch (cbStep.SelectedIndex)
            {
                //Day
                case 0:
                    {
                        dataGridView1.Columns[0].DefaultCellStyle.Format = @"dd/MM/yyyy";
                        dataGridView1.DataSource = Weather.GetWeatherData(
                        new DateTime(2010, 01, 01),
                        new DateTime(2020, 1, 31),
                        Weather.GraphStep.Day);
                        dataGridView1.Update();
                        break;
                    }

                //Week
                case 1:
                    {
                        dataGridView1.Columns[0].DefaultCellStyle.Format = @"MM/yyyy";
                        dataGridView1.DataSource = Weather.GetWeatherData(
                        new DateTime(2010, 01, 01),
                        new DateTime(2020, 1, 31),
                        Weather.GraphStep.Week);
                        dataGridView1.Update();
                        break;
                    }
                //Month
                case 2:
                    {
                        dataGridView1.Columns[0].DefaultCellStyle.Format = @"MM/yyyy";
                        dataGridView1.DataSource = Weather.GetWeatherData(
                            new DateTime(2010, 01, 01),
                            new DateTime(2020, 1, 31),
                            Weather.GraphStep.Month);
                        dataGridView1.Update();
                        break;
                    }
                //Year
                case 3:
                    {
                        dataGridView1.Columns[0].DefaultCellStyle.Format = @"yyyy";
                        dataGridView1.DataSource = Weather.GetWeatherData(
                        new DateTime(2010, 01, 01),
                        new DateTime(2020, 1, 31),
                        Weather.GraphStep.Year);
                        dataGridView1.Update();
                        
                        break;
                    }
            }
        }
    }
}