
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using Chart = System.Windows.Forms.DataVisualization.Charting.Chart;
using System.Drawing;
using System.Runtime.CompilerServices;
using DocumentFormat.OpenXml.Vml;

namespace курсач3
{
    public class MyChart
    {
        static Chart chart;
        public static void AddChartToForm(Plan plan)
        {
            // Создаем новый график
            chart = new Chart();

            // Настройка размеров и положения
            chart.Width = 510;
            chart.Height = 350;
            chart.Location = new System.Drawing.Point(666, 280);
            chart.Name = "chartOfPlan";

            // Добавляем область для рисования
            ChartArea chartArea = new ChartArea("MainArea");
            chart.ChartAreas.Add(chartArea);

            // Настраиваем оси
            chartArea.AxisX.Title = "Накопленная сумма";
            chartArea.AxisY.Title = "Количество месяцев";
            chartArea.AxisX.MajorGrid.Enabled = true;
            chartArea.AxisY.MajorGrid.Enabled = true;

            // Добавляем заголовок
            Title title = new Title("График накоплений", Docking.Top, new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold), System.Drawing.Color.Black);
            chart.Titles.Add(title);

            // Добавляем легенду
            //Legend legend = new Legend();
            //chart.Legends.Add(legend);

            Series series1 = new Series("Плановые накопления")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 3,
                Color = System.Drawing.Color.Red,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 8
            };
            chart.Series.Add(series1);

            
            Series series2 = new Series("Реальные накопления")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 3,
                Color = System.Drawing.Color.AliceBlue,
                MarkerStyle = MarkerStyle.Square,
                MarkerSize = 8
            };
            chart.Series.Add(series2);

            Series series3 = new Series("Целевая сумма")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 3,
                Color = System.Drawing.Color.YellowGreen,
                MarkerStyle = MarkerStyle.Square,
                MarkerSize = 8
            };
            chart.Series.Add(series3);

            //Строим график
            FillChart(plan);

            // Добавляем график на форму
            Form2 form = Form2.CurrentForm;

            form.Controls.Add(chart);
        }

        private static void FillChart(Plan plan)
        {
            double payment = plan.paymentAmount;
            double step = Form1.Step(plan.frequency);
            double x = 0;
            double y = plan.startAmount + plan.investAmount;
            chart.Series[0].Points.Clear();
            chart.Series[1].Points.Clear();
            chart.Series[2].Points.Clear();

            //плановые накопления
            while (y <= plan.amountWithInflation)
            {
                chart.Series[0].Points.AddXY(x, y);
                x+= step;
                y += payment;
            }

            //рост целевой суммы
            step = 1;
            x = 0;
            y = plan.goalAmount;
            double endGraph = plan.amountWithInflation;

            while (x <= plan.time / 4)
            {
                chart.Series[2].Points.AddXY(x, y);
                x+= step;
                y += y * (plan.inflation / 12);
            }

            //настроить таймер для обновления планов 
            //сделать линию накоплений на графике
        }

        
    }
}
