using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ClosedXML.Excel;
using System.Drawing;

namespace курсач3
{
    public class ChartForm : Form
    {
        private Chart chartControl;

        public ChartForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.chartControl = new Chart();
            this.SuspendLayout();

            this.Text = "График накопления";
            this.Width = 844;
            this.Height = 543;
            this.Location = new Point(762, 422);

            chartControl.Dock = DockStyle.Fill;
            chartControl.BackColor = Color.WhiteSmoke;
            chartControl.BorderlineDashStyle = ChartDashStyle.NotSet;
            chartControl.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chartControl.Palette = ChartColorPalette.BrightPastel;
            chartControl.Titles.Add(new Title("Изменение накоплений"));
        }

        private void AddSeries(Chart chart)
        {
           

            
        }

        private void AddInflationSeries (Chart chart)
        {
            // Линия 1: Изменения целевой суммы из-за инфляции
            Series seriesInflation = new Series("Инфляция")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.Red,
                MarkerSize = 7,
                IsVisibleInLegend = true
            };


        }

        private void AddSavingsSeries (Chart chart)
        {
            // Линия 2: Рост накоплений
            Series seriesAccumulation = new Series("Накопления")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.Blue,
                MarkerSize = 7,
                IsVisibleInLegend = true
            };
        }


    }
}
