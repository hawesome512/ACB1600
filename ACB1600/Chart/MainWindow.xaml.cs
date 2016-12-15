using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.ComponentModel;

namespace ChartDemo
{
        public class Node
        {
                public string Extra;
        }

        public class String2Items : IValueConverter
        {
                public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
                {
                        Node node = value as Node;
                        if (node.Extra == null)
                        {
                                return null;
                        }
                        else
                        {
                                return node.Extra.Split('_');
                        }
                }

                public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
                {
                        throw new NotImplementedException();
                }
        }

        /// <summary>
        /// MainWindow.xaml 的交互逻辑
        /// </summary>
        public partial class MainWindow : Window,INotifyPropertyChanged
        {
                CancellationTokenSource cancelToken;
                Node node = new Node()
                {
                       Extra="1_2_3"
                };
                public Node MyNode
                {
                        get
                        {
                                return node;
                        }
                        set
                        {
                                node = value;
                                if (PropertyChanged != null)
                                        this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("MyNode"));
                        }
                }
                public MainWindow()
                {
                        InitializeComponent();

                        this.DataContext = this;
                        box.Tag = node;
                        Binding binding = new Binding()
                        {
                                Source=this, Path=new PropertyPath(".MyNode"),Converter=new String2Items()
                        };
                        box.SetBinding(ComboBox.ItemsSourceProperty, binding);
                }

                private void btnDo_Click(object sender, RoutedEventArgs e)
                {
                        if (btnDo.Content.ToString() == "计时")
                        {
                                btnDo.Content = "停止";
                                Task.Factory.StartNew(new Action(() =>
                                {
                                        cancelToken = new CancellationTokenSource();
                                        ParallelOptions options = new ParallelOptions();
                                        options.CancellationToken = cancelToken.Token;
                                        while (true)
                                        {
                                                options.CancellationToken.ThrowIfCancellationRequested();
                                                this.Dispatcher.Invoke(new Action(() =>
                                                {
                                                        lblShow.Content = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss.fff");
                                                }));
                                                Thread.Sleep(100);
                                        }
                                }));
                        }
                        else
                        {
                                btnDo.Content = "计时";
                                cancelToken.Cancel();
                        }
                }

                private void btnChange_Click(object sender, RoutedEventArgs e)
                {
                        Random random=new Random();
                        int[] items = Enumerable.Repeat(1, 5).Select(r => random.Next(1, 10)).ToArray();
                        MyNode = new Node()
                        {
                                Extra = string.Join("_", items)
                        };
                }

                public event PropertyChangedEventHandler PropertyChanged;
        }
}
