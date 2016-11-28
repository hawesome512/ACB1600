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
using Hawesome;

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
                                        addItemTextBox(node.Address, index, node);
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
                        label.Background = Tools.GetBrush("#FF9A9A9A");
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

                void addItemTextBox(int address, int index, Node node)
                {
                        string[] items = node.Extra.Split('_');
                        bool useCbox = (node.NodeAuthority != Authority.R) && items.Length > 1;
                        Control box = useCbox ? new ComboBox() as Control : new TextBox() as Control;
                        if (useCbox)
                        {
                                (box as ComboBox).ItemsSource = items.Skip(1).ToArray();
                        }
                        box.Name = "A" + address;
                        box.Tag = node;
                        //取值0.8*，使行间留白，上下各留白0.1*，如下Margin.Top
                        box.Height = 0.8 * rowHeight;
                        box.VerticalContentAlignment = VerticalAlignment.Center;
                        box.HorizontalContentAlignment = HorizontalAlignment.Center;
                        box.VerticalAlignment = VerticalAlignment.Top;
                        double right = node.Unit != "/" ? 0 : 10;
                        box.Margin = new Thickness(0, index / 5 * rowHeight + 0.1 * rowHeight, right, 0);
                        Grid.SetColumn(box, index % 5 * 3 + 1);
                        Grid.SetColumnSpan(box, node.Unit != "/" ? 1 : 2);
                        grid_content.Children.Add(box);
                        if (node.ShowType == 3 || node.ShowType == 11)
                        {
                                box.MouseDoubleClick += tbox_MouseDoubleClick;
                                box.BorderBrush = Tools.GetBrush("#FFFF6501");
                                box.BorderThickness = new Thickness(2);
                        }
                        if (node.NodeAuthority == Authority.RW)
                        {
                                box.GotFocus += tbox_GotFocus;
                        }
                }

                void tbox_GotFocus(object sender, RoutedEventArgs e)
                {
                        Control box = sender as Control;
                        Point p = box.TranslatePoint(new Point(), grid_content);
                        btn_write.Margin = new Thickness(p.X + box.ActualWidth - 1, p.Y, 0, 0);
                        btn_write.Visibility = Visibility.Visible;
                        btn_write.Tag = box;
                }

                void tbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
                {
                        detail.Visibility = Visibility.Visible;
                        Control box = sender as Control;
                        Node node = box.Tag as Node;
                        Point p = box.TranslatePoint(new Point(), grid_content);
                        Point p0 = box.TranslatePoint(new Point(), this);
                        if (p0.Y <= this.Height / 2)
                        {
                                detail.Margin = new Thickness(0, p.Y + rowHeight * 0.8, 0, 0);
                                detail.img_top.Margin = new Thickness(p.X + box.ActualWidth / 2 - detail.img_top.Width / 2, 0, 0, 0);
                                detail.img_top.Visibility = Visibility.Visible;
                                detail.img_bottom.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                                detail.Margin = new Thickness(0, p.Y - detail.Height, 0, 0);
                                detail.img_bottom.Margin = new Thickness(p.X + box.ActualWidth / 2 - detail.img_bottom.Width / 2, 0, 0, 0);
                                detail.img_bottom.Visibility = Visibility.Visible;
                                detail.img_top.Visibility = Visibility.Hidden;
                        }
                        if (node.ShowType == 11)
                        {
                                read(node.Address, int.Parse(node.Extra), true);
                        }
                        else
                        {
                                detail.showSwitch(node.Extra, Convert.ToInt32(AutoBox.getText(box), 16));
                        }
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
                        List<Node> zone = zoneList[rbIndex];
                        read(zone[0].Address, zone.Count);
                }

                private void read(int address, int length, bool readArray = false)
                {
                        Task.Factory.StartNew(() =>
                        {
                                byte[] snd = new byte[6];
                                snd[0] = comAddress;
                                snd[1] = 0x3;
                                snd[2] = (byte)(address / 256);
                                snd[3] = (byte)(address % 256);
                                snd[4] = (byte)(length / 256);
                                snd[5] = (byte)(length % 256);
                                byte[] rcv = com.Execute(snd);
                                if (rcv.Length == length * 2)
                                {
                                        this.Dispatcher.Invoke(new Action(() =>
                                        {
                                                if (readArray)
                                                {
                                                        Control tbox = Tools.GetChild<Control>(grid_content, "A" + address);
                                                        Node node = tbox.Tag as Node;
                                                        int[] data = (int[])typeof(ComConverter).GetMethod("CvtR" + node.ShowType).Invoke(cvt, new object[] { rcv, node.Extra });
                                                        detail.showChart(data);
                                                }
                                                else
                                                {
                                                        for (int i = 0; i < length; i++)
                                                        {
                                                                Control box = Tools.GetChild<Control>(grid_content, "A" + (address + i));
                                                                byte[] source = new byte[] { rcv[2 * i], rcv[2 * i + 1] };
                                                                Node node = box.Tag as Node;
                                                                string result = typeof(ComConverter).GetMethod("CvtR" + node.ShowType).Invoke(cvt, new object[] { source, node.Extra }).ToString();
                                                                AutoBox.setText(box, result);
                                                        }
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

                private void btn_write_Click(object sender, RoutedEventArgs e)
                {
                        Control box = btn_write.Tag as Control;
                        Node node = box.Tag as Node;
                        byte[] snd = new byte[9];
                        snd[0] = comAddress;
                        snd[1] = 0x10;
                        snd[2] = (byte)(node.Address / 256);
                        snd[3] = (byte)(node.Address % 256);
                        snd[5] = 0x1;
                        snd[6] = 0x2;
                        byte[] data = (byte[])typeof(ComConverter).GetMethod("CvtW" + node.ShowType).Invoke(cvt, new object[] { AutoBox.getText(box), node.Extra });
                        data.CopyTo(snd, 7);
                        byte[] rcv = com.Execute(snd);
                }
        }
}
