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
using System.Windows.Media.Animation;
using System.Xml;
using System.Text.RegularExpressions;

namespace ACB1600
{
        /// <summary>
        /// MainWindow.xaml 的交互逻辑
        /// </summary>
        public partial class MainWindow : Window
        {
                List<RadioButton> rbList;
                static int rowHeight = 35;//每行高度
                static int rbWidth = 75;//RadioButton标签宽度
                List<List<Node>> zoneList;
                Com com;
                ComConverter cvt;
                byte comAddress;
                public MainWindow()
                {
                        InitializeComponent();
                        initCom();
                        initCtrls();
                }

                private void initCom()
                {
                        com = new Com(ComType.SP, Tools.GetConfig("COM"));
                        comAddress = byte.Parse(Tools.GetConfig("Address"));
                        cvt = new ComConverter();
                }

                void initCtrls()
                {
                        loadItems();
                        rbList[0].IsChecked = true;
                        Binding binding1 = new Binding("Background");
                        binding1.Source = rbList.First();
                        rect_left.SetBinding(Rectangle.FillProperty, binding1);
                        Binding binding2 = new Binding("Background");
                        binding2.Source = rbList.Last();
                        rect_right.SetBinding(Rectangle.FillProperty, binding2);
                        rect_right.Margin = new Thickness(rbWidth * rbList.Count, 0, 0, 0);
                }

                void loadItems()
                {
                        XmlDocument doc = new XmlDocument();
                        doc.Load("ACB1600.xml");
                        XmlElement device = doc.DocumentElement;
                        int index = 0;

                        rbList = new List<RadioButton>();
                        zoneList = new List<List<Node>>();
                        foreach (XmlElement xe in device.ChildNodes)
                        {
                                List<Node> zone = new List<Node>();
                                addZone(xe.Attributes["alias"].Value, index);
                                addRadioBox(xe.Attributes["address"].Value, index);
                                index += 5;
                                foreach (XmlElement x in xe.ChildNodes)
                                {
                                        Node node = new Node()
                                        {
                                                Address = Convert.ToInt32(x["Address"].InnerText, 16),
                                                Name = x["Name"].InnerText,
                                                Alias = x["Alias"].InnerText,
                                                NodeAuthority = (Authority)Enum.Parse(typeof(Authority), x["Authority"].InnerText),
                                                ShowType = int.Parse(x["ShowType"].InnerText),
                                                Unit = x["Unit"].InnerText,
                                                Extra = x["Extra"].InnerText
                                        };
                                        if (node.ShowType != 11)
                                        {
                                                zone.Add(node);
                                        }
                                        addItemLabel(node.Name, index, true);
                                        bool hasUnit = node.Unit != "/";
                                        addItemBox(node.Address, index,hasUnit);
                                        if (hasUnit)
                                        {
                                                addItemLabel(node.Unit, index, false);
                                        }
                                        index++;
                                }
                                index = ((index - 1) / 5 + 1) * 5;
                                zoneList.Add(zone);
                        }
                }

                void addRadioBox(string name, int index)
                {
                        RadioButton rb = new RadioButton();
                        rb.Name = "rb_" + name;
                        rb.Checked += RadioButton_Checked;
                        rb.Content = name;
                        rb.Margin = new Thickness((rbWidth + 1) * rbList.Count, 0, 0, 0);
                        rb.Tag = index;
                        grid_rbs.Children.Add(rb);
                        rbList.Add(rb);
                }

                void addZone(string title, int index)
                {
                        Label label = new Label();
                        label.Content = title;
                        label.Height = rowHeight;
                        label.HorizontalContentAlignment = HorizontalAlignment.Center;
                        label.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF9A9A9A");
                        label.Foreground = Brushes.White;
                        label.VerticalAlignment = VerticalAlignment.Top;
                        label.Margin = new Thickness(0, index / 5 * rowHeight, 0, 0);
                        Grid.SetColumnSpan(label, 5 * 3);
                        grid_content.Children.Add(label);
                }

                /// <summary>
                /// 添加固定文本：Item.Name和Item.Unit
                /// </summary>
                /// <param name="text"></param>
                /// <param name="index"></param>
                /// <param name="isName"></param>
                void addItemLabel(string text, int index, bool isName)
                {
                        Label label = new Label();
                        label.Content = text;
                        label.Height = rowHeight;
                        label.HorizontalContentAlignment = isName ? HorizontalAlignment.Right : HorizontalAlignment.Left;
                        label.VerticalAlignment = VerticalAlignment.Top;
                        label.Margin = new Thickness(0, index / 5 * rowHeight, 5, 0);
                        if (!isName)
                        {
                                label.Foreground = Brushes.Gray;
                        }
                        int col = isName ? index % 5 * 3 : index % 5 * 3 + 2;
                        Grid.SetColumn(label, col);
                        grid_content.Children.Add(label);
                }

                void addItemBox(int address, int index,bool hasUnit)
                {
                        TextBox tbox = new TextBox();
                        //删除Name命名规则中的非法字符
                        tbox.Name = "A" + address;
                        //取值0.8*，使行间留白，上下各留白0.1*，如下Margin.Top
                        tbox.Height = 0.8 * rowHeight;
                        tbox.VerticalContentAlignment = VerticalAlignment.Center;
                        tbox.HorizontalContentAlignment = HorizontalAlignment.Center;
                        tbox.VerticalAlignment = VerticalAlignment.Top;
                        double right = hasUnit ? 0 : 10;
                        tbox.Margin = new Thickness(0, index / 5 * rowHeight + 0.1 * rowHeight, right, 0);
                        Grid.SetColumn(tbox, index % 5 * 3 + 1);
                        Grid.SetColumnSpan(tbox, hasUnit ? 1 : 2);
                        grid_content.Children.Add(tbox);
                }

                private void btn_close_Click(object sender, RoutedEventArgs e)
                {
                        this.Close();
                }

                private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
                {
                        this.DragMove();
                }

                private void btn_min_Click(object sender, RoutedEventArgs e)
                {
                        this.WindowState = WindowState.Minimized;
                }

                private void btn_size_Click(object sender, RoutedEventArgs e)
                {
                        if (this.WindowState == WindowState.Normal)
                        {
                                this.WindowState = WindowState.Maximized;
                        }
                        else
                        {
                                this.WindowState = WindowState.Normal;
                        }
                }

                private void RadioButton_Checked(object sender, RoutedEventArgs e)
                {
                        RadioButton rb = sender as RadioButton;
                        int height = Convert.ToInt32(rb.Tag) / 5 * rowHeight;
                        animateMove(height);
                        int rbIndex = rbList.FindIndex(r => r.IsChecked == true);
                        readZoneByIndex(rbIndex);
                }

                private void readZoneByIndex(int index)
                {
                        Task.Factory.StartNew(() =>
                        {
                                List<Node> zone = zoneList[index];
                                int address = zone[0].Address;
                                int count = zone.Count;
                                byte[] snd = new byte[6];
                                snd[0] = comAddress;
                                snd[1] = 0x3;
                                snd[2] = (byte)(address / 256);
                                snd[3] = (byte)(address % 256);
                                snd[4] = (byte)(count / 256);
                                snd[5] = (byte)(count % 256);
                                byte[] rcv = com.Execute(snd);
                                if (rcv.Length == count * 2)
                                {
                                        this.Dispatcher.Invoke(new Action(() =>
                                        {
                                                for (int i = 0; i < count; i++)
                                                {
                                                        TextBox tbox = Tools.FindChild<TextBox>(grid_content, "A" + zone[i].Address);
                                                        byte[] source = new byte[] { rcv[2 * i], rcv[2 * i + 1] };
                                                        tbox.Text = typeof(ComConverter).GetMethod("CvtR" + zone[i].ShowType).Invoke(cvt, new object[] { source, zone[i].Extra }).ToString();
                                                }
                                        }));
                                }
                        });
                }

                private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
                {
                        //鼠标滚轮切换片区,1000片区数据多分两次切换
                        int rbIndex = rbList.FindIndex(rb => rb.IsChecked == true);
                        int height = Convert.ToInt32(rbList[rbIndex].Tag) / 5 * rowHeight;
                        if (rbIndex == 1 && e.Delta < 0 && -grid_content.Margin.Top == height)
                        {
                                height += 10 * rowHeight;
                        }
                        else
                        {
                                rbIndex += (int)-e.Delta / 120;
                                rbIndex = rbIndex < 0 ? 0 : (rbIndex == rbList.Count ? rbList.Count - 1 : rbIndex);
                                rbList[rbIndex].IsChecked = true;
                                height = Convert.ToInt32(rbList[rbIndex].Tag) / 5 * rowHeight;
                        }
                        animateMove(height);
                }

                /// <summary>
                /// 切换不同片区上下移动时动画
                /// </summary>
                /// <param name="height"></param>
                private void animateMove(double height)
                {
                        //移动到底部时不再继续，否则页面留白太多
                        Control last = grid_content.Children[grid_content.Children.Count - 1] as Control;
                        double totalHeight = last.Margin.Top + rowHeight;
                        double visualHeight = this.ActualHeight - rowHeight * 3;//grid_content可视高度
                        if (height > totalHeight - visualHeight)
                        {
                                height = totalHeight - visualHeight;
                        }
                        ThicknessAnimation anim = new ThicknessAnimation(new Thickness(1, -height, 1, 1), new Duration(TimeSpan.FromSeconds(0.3)));
                        grid_content.BeginAnimation(Grid.MarginProperty, anim);
                }
        }
}
