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

namespace ChartDemo
{
        /// <summary>
        /// MainWindow.xaml 的交互逻辑
        /// </summary>
        public partial class MainWindow : Window
        {
                CancellationTokenSource cancelToken;
                public MainWindow()
                {
                        InitializeComponent();
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
        }
}
