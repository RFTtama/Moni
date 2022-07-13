using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ScottPlot;

namespace Moni
{
    public static class ChartViewer
    {
        public static DateTime[] dateTimes;

        public static void ShowChart(double[] y, string yName, string title, string head)
        {
            Form chartForm = new Form();
            chartForm.Size = new System.Drawing.Size(200, 200);
            FormsPlot plot = new FormsPlot();
            plot.Dock = DockStyle.Fill;
            chartForm.Controls.Add(plot);

            plot.Plot.AddBar(ArrayConverters.ConvertDateTimeToDouble(dateTimes), y);
            plot.Render();

            chartForm.Show();
        }
    }
}
