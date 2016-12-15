using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Hawesome;
using System.Windows.Forms.DataVisualization.Charting;

namespace ACB1600
{
        /// <summary>
        /// Detail.xaml 的交互逻辑
        /// </summary>
        public partial class Detail : UserControl
        {
                int rowHeight = 35;
                int switchData;
                Control box;
                Chart colChart;
                System.Windows.Forms.DataVisualization.Charting.HitTestResult hitTest;
                public Detail()
                {
                        InitializeComponent();
                        initColChart();
                }

                private void btn_close_Click(object sender, RoutedEventArgs e)
                {
                        this.Visibility = Visibility.Hidden;
                }

                public void showChart(int[] data)
                {
                        myHost.Visibility = Visibility.Visible;
                        mySwitch.Visibility = Visibility.Hidden;
                        this.Height = 400;
                        colChart.Series[0].Points.DataBindY(data);
                }

                public void showSwitch(String switches, int data, Control box)
                {
                        switchData = data;
                        this.box = box;
                        myHost.Visibility = Visibility.Hidden;
                        mySwitch.Visibility = Visibility.Visible;
                        mySwitch.Children.Clear();
                        string[] swList = switches.Split('_');
                        this.Height = 80 + Math.Ceiling(swList.Length / 4.0) * rowHeight;
                        int index = 0;
                        foreach (string sw in swList)
                        {
                                if (sw == "*")
                                        continue;
                                List<string> sws = new List<string>(sw.Split('/'));
                                addName(index++, sws[0]);
                                addSwitch(index++, sws, Tools.GetBit(data, index / 2 - 1));
                        }
                }

                public void showOutput(TextBox box)
                {
                        this.box = box;
                        if (Regex.IsMatch(box.Text, @"^\w \w$"))
                        {
                                Node node = box.Tag as Node;
                                myHost.Visibility = Visibility.Hidden;
                                mySwitch.Visibility = Visibility.Visible;
                                mySwitch.Children.Clear();
                                this.Height = 80 + rowHeight;
                                addBox("output1", box.Text[0], node.Extra, 1);
                                addBox("output2", box.Text[2], node.Extra, 5);
                        }
                }

                private void addSwitch(int index, List<string> sws, int bit)
                {
                        Border border = addBorder(index, new Thickness(1, 1, 1, 1));
                        OnOff btn = new OnOff();
                        btn.toggleSwitch.Tag = (index - 1) / 2;//标注开关所在的位数
                        btn.toggleSwitch.Click += toggleSwitch_Click;
                        btn.toggleSwitch.IsChecked = bit != 1 ? true : false;
                        btn.toggleSwitch.FontSize = 15;
                        btn.toggleSwitch.CornerRadius = new CornerRadius(btn.Height / 2);
                        if (sws.Count == 3)
                        {
                                if (sws[1] == sws[2])
                                        btn.toggleSwitch.IsEnabled = false;

                                btn.toggleSwitch.CheckedText = sws[2];
                                btn.toggleSwitch.UncheckedText = sws[1];
                        }
                        border.Child = btn;
                }

                void toggleSwitch_Click(object sender, RoutedEventArgs e)
                {
                        WPFSpark.ToggleSwitch ts = sender as WPFSpark.ToggleSwitch;
                        int index = int.Parse(ts.Tag.ToString());
                        int bit = ts.IsChecked == true ? 0 : 1;
                        switchData = Tools.SetBit(switchData, index, bit);
                        AutoBox.setText(box, string.Format("0x{0:X4}", switchData));
                }

                private void addName(int index, string sw)
                {
                        Border border = addBorder(index, new Thickness(1, 1, 1, 1));
                        TextBlock text = new TextBlock();
                        text.HorizontalAlignment = HorizontalAlignment.Right;
                        text.VerticalAlignment = VerticalAlignment.Center;
                        text.Margin = new Thickness(0, 0, 10, 0);
                        text.TextAlignment = TextAlignment.Right;
                        text.FontWeight = FontWeights.Bold;
                        text.FontSize = 14;
                        text.Text = sw;
                        text.Foreground = Brushes.Black;
                        border.Child = text;
                }

                private Border addBorder(int index, Thickness thick)
                {
                        Border border = new Border();
                        border.BorderBrush = Brushes.Black;
                        border.BorderThickness = thick;
                        System.Windows.Controls.Grid.SetColumn(border, index % 8);
                        border.Height = rowHeight;
                        border.VerticalAlignment = VerticalAlignment.Top;
                        border.Margin = new Thickness(0, index / 8 * rowHeight, 0, 0);
                        mySwitch.Children.Add(border);
                        return border;
                }

                private void addBox(string name, char value, string extra, int column)
                {
                        var items = extra.Split('_').ToList();
                        string item = Convert.ToInt32(value).ToString();
                        int index = items.IndexOf(item);
                        item = index < 0 ? string.Empty : items[index + 1];
                        ComboBox box = new ComboBox();
                        box.Name = name;
                        box.SelectionChanged += box_SelectionChanged;
                        box.ItemsSource = items.Where((t, i) => i % 2 == 1).ToList();
                        AutoBox.setText(box, item);
                        box.Height = rowHeight;
                        System.Windows.Controls.Grid.SetColumn(box, column);
                        System.Windows.Controls.Grid.SetColumnSpan(box, 2);
                        mySwitch.Children.Add(box);
                }

                void box_SelectionChanged(object sender, SelectionChangedEventArgs e)
                {
                        ComboBox cBox = sender as ComboBox;
                        Node node = box.Tag as Node;
                        List<string> items = node.Extra.Split('_').ToList();
                        int item = int.Parse(items[2 * cBox.SelectedIndex]);
                        string selectedValue = Convert.ToChar(item).ToString();
                        string value = null;
                        if (cBox.Name == "output1")
                        {
                                value = Convert.ToChar(item) + " " + AutoBox.getText(box)[2];
                        }
                        else
                        {
                                value = AutoBox.getText(box)[0] + " " + Convert.ToChar(item);
                        }
                        AutoBox.setText(box, value);
                }

                private void initColChart()
                {
                        colChart = myHost.Child as Chart;
                        colChart.MouseUp += colChart_MouseUp;
                        colChart.MouseDown += colChart_MouseDown;
                        colChart.BackColor = System.Drawing.Color.Transparent;
                        ChartArea area = new ChartArea();
                        area.AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                        area.AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                        area.AxisY.IsInterlaced = true;
                        area.AxisY.InterlacedColor = System.Drawing.Color.FromArgb(0x88, 0xdd, 0xdd, 0xdd);
                        area.BackColor = System.Drawing.Color.Transparent;
                        colChart.ChartAreas.Add(area);
                        Series series = new Series();
                        series.ChartType = SeriesChartType.Column;
                        series.Color = System.Drawing.Color.FromArgb(0xff, 0x00, 0x96, 0x88);
                        colChart.Series.Add(series);
                }

                void colChart_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
                {
                        hitTest = colChart.HitTest(e.X, e.Y);
                        if (hitTest.ChartElementType == ChartElementType.DataPoint)
                        {
                                DataPoint point = hitTest.Object as DataPoint;
                                point.IsValueShownAsLabel = false;
                                point.BackHatchStyle = ChartHatchStyle.None;
                        }
                }

                void colChart_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
                {
                        hitTest = colChart.HitTest(e.X, e.Y);
                        if (hitTest.ChartElementType == ChartElementType.DataPoint)
                        {
                                DataPoint point = hitTest.Object as DataPoint;
                                point.IsValueShownAsLabel = true;
                                point.BackHatchStyle = ChartHatchStyle.Percent25;
                        }
                }
        }
}
