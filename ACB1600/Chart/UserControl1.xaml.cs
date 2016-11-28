using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms.DataVisualization.Charting;

namespace ChartDemo
{
        /// <summary>
        /// UserControl1.xaml 的交互逻辑
        /// </summary>
        public partial class UserControl1 : UserControl
        {
                Chart colChart;
                public UserControl1()
                {
                        InitializeComponent();
                        initColChart();
                }
                private void initColChart()
                {
                        colChart = MyHost.Child as Chart;
                        colChart.BackColor = System.Drawing.Color.Red;
                        ChartArea area = new ChartArea();
                        area.AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                        area.AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                        area.AxisY.IsInterlaced = true;
                        area.AxisY.InterlacedColor = System.Drawing.Color.FromArgb(0x88, 0xdd, 0xdd, 0xdd);
                        area.BackColor = System.Drawing.Color.Transparent;
                        colChart.ChartAreas.Add(area);
                        Series series = new Series("now");
                        series.ChartType = SeriesChartType.Column;
                        series.Color = System.Drawing.Color.FromArgb(0xff, 0x00, 0x96, 0x88);
                        series.Points.AddXY(1, 10);
                        colChart.Series.Add(series);
                }
        }
}
