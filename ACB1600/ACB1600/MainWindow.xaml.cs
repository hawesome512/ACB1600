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
using System.Net;

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
                byte comRepeat;
                public MainWindow()
                {
                        InitializeComponent();
                        checkUpdate();
                        initCom();
                        initCtrls();
                }

                #region 初始化控件
                private void initCom()
                {
                        var ports = System.IO.Ports.SerialPort.GetPortNames().ToList();
                        cbox_com.ItemsSource = ports;
                        string strCom = Tools.GetConfig("COM");
                        string strRepeat = Tools.GetConfig("Repeat");
                        tbox_repeat.Text = strRepeat;
                        comRepeat = byte.Parse(strRepeat);
                        if (ports.Contains(strCom))
                        {
                                cbox_com.SelectedValue = strCom;
                                com = new Com(ComType.SP, strCom,comRepeat);
                        }
                        string strAddress = Tools.GetConfig("Address");
                        comAddress = byte.Parse(strAddress);
                        tbox_address.Text = strAddress;
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
                                addZone(xe.Attributes["alias"].Value, index, xe.HasAttribute("offset"));
                                addRadioBox(xe.Attributes["address"].Value, index, xe.HasAttribute("offset"));
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
                                                Unit = x["Unit"].InnerText == "/" ? string.Empty : x["Unit"].InnerText,
                                                Extra = x["Extra"].InnerText
                                        };
                                        XmlElement infs = x["Influences"];
                                        if (infs != null)
                                        {
                                                node.Influences = new List<Influence>();
                                                foreach (XmlElement inf in infs.ChildNodes)
                                                {
                                                        node.Influences.Add(new Influence(inf.InnerText));
                                                }
                                        }
                                        if (node.ShowType != 11)
                                        {
                                                zone.Add(node);
                                        }
                                        addItemLabel(node.Address, node.Name, index, true);
                                        addItemTextBox(node.Address, index, node);
                                        addItemLabel(node.Address, node.Unit, index, false);
                                        index++;
                                }
                                index = ((index - 1) / 5 + 1) * 5;
                                zoneList.Add(zone);
                        }
                }

                void addRadioBox(string name, int index, bool hasOffset)
                {
                        RadioButton rb = new RadioButton();
                        rb.Name = "rb_" + name;
                        rb.Checked += RadioButton_Checked;
                        rb.Content = name;
                        rb.Margin = new Thickness((rbWidth + 1) * rbList.Count, 0, 0, 0);
                        rb.Tag = index;
                        grid_rbs.Children.Add(rb);
                        rbList.Add(rb);
                        if (hasOffset)
                        {
                                rb.Checked += (s, o) =>
                                {
                                        TextBox zBox = Tools.GetChild<TextBox>(grid_content, "Zone" + Convert.ToInt32(name, 16));
                                        zBox.Text = "0";
                                };
                        }
                }

                void addZone(string title, int index, bool hasOffset)
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

                        if (hasOffset)
                        {
                                TextBox box = new TextBox();
                                box.Height = rowHeight;
                                box.Width = box.Height;
                                box.Text = "0";
                                string addr = Regex.Match(title, @"(?<=\()[0-9A-F]+(?=\))").Value;
                                box.Name = "Zone" + Convert.ToInt32(addr, 16);
                                box.Foreground = Brushes.White;
                                box.Background = Tools.GetBrush("#FFFF6501");
                                box.VerticalAlignment = VerticalAlignment.Top;
                                box.HorizontalContentAlignment = HorizontalAlignment.Center;
                                box.VerticalContentAlignment = VerticalAlignment.Center;
                                box.HorizontalAlignment = HorizontalAlignment.Left;
                                box.TextChanged += box_TextChanged;
                                box.Margin = new Thickness(0, index / 5 * rowHeight, 0, 0);
                                Grid.SetColumn(box, 9);
                                grid_content.Children.Add(box);
                        }
                }

                void box_TextChanged(object sender, TextChangedEventArgs e)
                {
                        TextBox box = sender as TextBox;
                        string strA = Regex.Match(box.Name, @"\d+").Value;
                        int nA = int.Parse(strA);
                        int zoneIndex = nA / (int)Math.Pow(16, 3);
                        int nRecord;
                        if (int.TryParse(box.Text, out nRecord))
                        {
                                int length = zoneList[zoneIndex].Count;
                                read(nA, length,false,nRecord);
                        }
                }

                /// <summary>
                /// 添加固定文本：Item.Name和Item.Unit
                /// </summary>
                /// <param name="text"></param>
                /// <param name="index"></param>
                /// <param name="isName"></param>
                void addItemLabel(int address, string text, int index, bool isName)
                {
                        Label label = new Label();
                        label.Name = isName ? "Name" + address : "Unit" + address;
                        label.Content = text;
                        label.Height = rowHeight;
                        label.HorizontalContentAlignment = isName ? HorizontalAlignment.Right : HorizontalAlignment.Left;
                        label.VerticalContentAlignment = VerticalAlignment.Center;
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
                        List<string> items = node.Extra.Split('_').ToList();
                        //判断何时用下拉框
                        bool useCbox = (node.NodeAuthority != Authority.R) && ((node.ShowType == 1 && node.Extra.Contains('_')) || node.ShowType == 2);
                        Control box = useCbox ? new ComboBox() as Control : new TextBox() as Control;
                        box.Name = "Box" + address;
                        box.Tag = node;
                        if (useCbox)
                        {
                                AutoBox.setBoxItems(box as ComboBox);
                        }
                        //取值0.8*，使行间留白，上下各留白0.1*，如下Margin.Top
                        box.Height = 0.8 * rowHeight;
                        box.VerticalContentAlignment = VerticalAlignment.Center;
                        box.HorizontalContentAlignment = HorizontalAlignment.Center;
                        box.VerticalAlignment = VerticalAlignment.Top;
                        box.Margin = new Thickness(0, index / 5 * rowHeight + 0.1 * rowHeight, 10, 0);
                        Grid.SetColumn(box, index % 5 * 3 + 1);
                        Grid.SetColumnSpan(box, 2);
                        grid_content.Children.Add(box);
                        if (node.ShowType == 3 || node.ShowType == 11 || node.ShowType == 13)
                        {
                                box.MouseDoubleClick += tbox_MouseDoubleClick;
                                box.BorderBrush = Tools.GetBrush("#FFFF6501");
                                box.BorderThickness = new Thickness(2);
                        }
                        if (node.NodeAuthority != Authority.R)
                        {
                                box.GotFocus += tbox_GotFocus;
                        }
                }

                void tbox_GotFocus(object sender, RoutedEventArgs e)
                {
                        Control box = sender as Control;
                        Point p = box.TranslatePoint(new Point(), grid_content);
                        double left = p.X / grid_content.ActualWidth > 0.85 ? p.X - btn_write.ActualWidth : p.X + box.ActualWidth;//最右一列
                        btn_write.Margin = new Thickness(left, p.Y, 0, 0);
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
                        else if (node.ShowType == 13)
                        {
                                detail.showOutput(box as TextBox);
                        }
                        else
                        {
                                detail.showSwitch(node.Extra, Convert.ToInt32(AutoBox.getText(box), 16), box);
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
                #endregion

                #region 切换标签
                private void RadioButton_Checked(object sender, RoutedEventArgs e)
                {
                        RadioButton rb = sender as RadioButton;
                        int height = Convert.ToInt32(rb.Tag) / 5 * rowHeight;
                        animateMove(height);
                        int rbIndex = rbList.FindIndex(r => r.IsChecked == true);
                        List<Node> zone = zoneList[rbIndex];
                        read(zone[0].Address, zone.Count);
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
                        double totalHeight = last.Margin.Top + rowHeight + 0.5 * rowHeight;//多加0.5*否则最后一行会被遮挡
                        double visualHeight = this.ActualHeight - rowHeight * 3;//grid_content可视高度
                        if (height > totalHeight - visualHeight)
                        {
                                height = totalHeight - visualHeight;
                        }
                        ThicknessAnimation anim = new ThicknessAnimation(new Thickness(1, -height, 1, 1), new Duration(TimeSpan.FromSeconds(0.3)));
                        grid_content.BeginAnimation(Grid.MarginProperty, anim);
                }
                #endregion

                #region 通信
                private void read(int address, int length, bool readArray = false,int offset=0)
                {
                        Task.Factory.StartNew(() =>
                        {
                                int realAddr = address + offset * length;
                                byte[] snd = new byte[6];
                                snd[0] = comAddress;
                                snd[1] = 0x3;
                                snd[2] = (byte)(realAddr / 256);
                                snd[3] = (byte)(realAddr % 256);
                                snd[4] = (byte)(length / 256);
                                snd[5] = (byte)(length % 256);
                                byte[] rcv = com.Execute(snd);
                                this.Dispatcher.Invoke(new Action(() =>
                                {
                                        if (rcv.Length == length * 2)
                                        {
                                                lbl_msg.Content = string.Format("读取：{0:X4}，位数：{1}【成功】{2}", address, length, DateTime.Now.ToLongTimeString());
                                                if (readArray)
                                                {
                                                        Control tbox = Tools.GetChild<Control>(grid_content, "Box" + address);
                                                        Node node = tbox.Tag as Node;
                                                        int[] data = (int[])typeof(ComConverter).GetMethod("CvtR" + node.ShowType).Invoke(cvt, new object[] { rcv, node.Extra });
                                                        detail.showChart(data);
                                                }
                                                else
                                                {
                                                        for (int i = 0; i < length; i++)
                                                        {
                                                                Control box = Tools.GetChild<Control>(grid_content, "Box" + (address + i));
                                                                byte[] source = new byte[] { rcv[2 * i], rcv[2 * i + 1] };
                                                                Node node = box.Tag as Node;
                                                                int nResult = source[0] * 256 + source[1];
                                                                string result = typeof(ComConverter).GetMethod("CvtR" + node.ShowType).Invoke(cvt, new object[] { source, node.Extra }).ToString();
                                                                AutoBox.setText(box, result);
                                                                if (node.Influences != null)
                                                                {
                                                                        foreach (Influence inf in node.Influences)
                                                                        {
                                                                                inf.executeInfluence(grid_content, nResult, AutoBox.getText(box));
                                                                        }
                                                                }
                                                                //处理一些没有规律特殊的节点
                                                                if (node.Address == 0x7007 || node.Address == 0x6007)
                                                                {
                                                                        Control box0 = Tools.GetChild<Control>(grid_content, "Box" + (node.Address - 7));
                                                                        if (AutoBox.getText(box0) == "相序")
                                                                        {
                                                                                AutoBox.setText(box, nResult == 0 ? "逆序" : "正序");
                                                                        }
                                                                }
                                                                else if (node.Address == 0x8001)
                                                                {
                                                                        Control box0x8000 = Tools.GetChild<Control>(grid_content, "Box" + (0x8000));
                                                                        string value = AutoBox.getText(box0x8000);
                                                                        if (value == "存储器故障")
                                                                        {
                                                                                AutoBox.setText(box, string.Format("0x{0:X4}", nResult));
                                                                        }
                                                                        else if (value == "相序")
                                                                        {
                                                                                AutoBox.setText(box, nResult == 0 ? "逆序" : "正序");
                                                                        }
                                                                }
                                                        }
                                                }
                                        }
                                        else
                                        {
                                                lbl_msg.Content = string.Format("读取：{0:X4}，位数：{1}【失败】{2}", address, length, DateTime.Now.ToLongTimeString());
                                        }
                                }));
                        });
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
                        if (data == null)
                        {
                                lbl_msg.Content = string.Format("修改：0x{0:X4}  {1} 【失败】——格式出错", node.Address, node.Name);
                        }
                        else
                        {
                                data.CopyTo(snd, 7);
                                byte[] rcv = com.Execute(snd);
                                if (rcv == null || rcv.Length != 1)
                                {
                                        lbl_msg.Content = string.Format("修改：0x{0:X4}  {1} 【失败】{2}", node.Address, node.Name, DateTime.Now.ToLongTimeString());
                                }
                                else
                                {
                                        lbl_msg.Content = string.Format("修改：0x{0:X4}  {1} 【成功】{2}", node.Address, node.Name, DateTime.Now.ToLongTimeString());
                                        read(node.Address, 1);
                                }
                        }
                }
                #endregion

                #region 自动更新
                void checkUpdate()
                {
                        Task.Factory.StartNew(new Action(() =>
                        {
                                Version version = getVersion(@"http://172.16.65.88:7072/ACB1600/ACB1600最新版本号.txt");
                                Version now = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                                this.Dispatcher.Invoke(new Action(() =>
                                {
                                        if (version != null && version > now)
                                        {
                                                var result = MsgBox.Show(string.Format("发现可用更新，新版本为:{0}\r\n是否现在更新？", version.ToString()), "更新", MsgBox.Buttons.YesNo, MsgBox.Icons.Question);
                                                if (result == System.Windows.Forms.DialogResult.Yes)
                                                {
                                                        update(version.ToString());
                                                }
                                        }
                                }));
                        }));
                }

                void update(string version)
                {
                        string address = @"http://172.16.65.88:7072/ACB1600";
                        string dir = AppDomain.CurrentDomain.BaseDirectory;
                        if (dir.Contains(" "))
                        {
                                this.Dispatcher.Invoke(new Action(() =>
                                {
                                        MsgBox.Show(string.Format("软件地址:{0}中存在空格，请删除空格后重试！", dir), "更新失败", MsgBox.Buttons.OK, MsgBox.Icons.Error);
                                }));
                        }
                        else
                        {
                                address = string.Format(@"{0}/ACB1600_{1}.rar", address, version);
                                string args = string.Format("{0} {1}", address, AppDomain.CurrentDomain.BaseDirectory);
                                System.Diagnostics.Process.Start(@"Update\Update.exe", args);
                        }
                        this.Close();
                }

                private static Version getVersion(string url)
                {
                        WebClient client = new WebClient();
                        try
                        {
                                string v = Encoding.ASCII.GetString(client.DownloadData(url));
                                return new Version(v);
                        }
                        catch
                        {
                                return null;
                        }
                }
                #endregion

                #region 设置
                private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
                {
                        TimeSpan ts = TimeSpan.FromSeconds(0.5);
                        ThicknessAnimation animation = new ThicknessAnimation(new Thickness(0, 0, 0, 0), new Duration(ts));
                        panel_setting.BeginAnimation(StackPanel.MarginProperty, animation);
                        img_setting.Source = Tools.GetImgSource("Images/next.png", "ACB1600");
                }

                private void StackPanel_MouseLeave(object sender, MouseEventArgs e)
                {
                        TimeSpan ts = TimeSpan.FromSeconds(0.5);
                        ThicknessAnimation animation = new ThicknessAnimation(new Thickness(0, 0, -300, 0), new Duration(ts));
                        panel_setting.BeginAnimation(StackPanel.MarginProperty, animation);
                        img_setting.Source = Tools.GetImgSource("Images/previous.png", "ACB1600");
                        Tools.SetConfig("COM", cbox_com.Text);
                        Tools.SetConfig("Address", tbox_address.Text);
                        Tools.SetConfig("Repeat",tbox_repeat.Text);
                        comAddress = byte.Parse(tbox_address.Text);
                        comRepeat = byte.Parse(tbox_repeat.Text);
                        com.Dispose();
                        com = new Com(ComType.SP, cbox_com.Text,comRepeat);
                }
                #endregion
        }
}
