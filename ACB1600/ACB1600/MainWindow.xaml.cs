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

namespace ACB1600
{
        /// <summary>
        /// MainWindow.xaml 的交互逻辑
        /// </summary>
        public partial class MainWindow : Window
        {
                List<RadioButton> rbList;
                int rbIndex = 0;
                public MainWindow()
                {
                        InitializeComponent();
                        initCtrls(12);
                }

                void initCtrls(int length)
                {
                        rbList = new List<RadioButton>();
                        for (int i = 0; i < length; i++)
                        {
                                RadioButton rb = new RadioButton();
                                rb.Name = "rb_" + i;
                                rb.Checked += RadioButton_Checked;
                                rb.Content = string.Format("{0:X}000", i);
                                rb.Margin=new Thickness(75*i,0,0,0);
                                grid_rbs.Children.Add(rb);
                                rbList.Add(rb);
                        }
                        rbList[rbIndex].IsChecked = true;
                        Binding binding1 = new Binding("Background");
                        binding1.Source = rbList.First();
                        rect_left.SetBinding(Rectangle.FillProperty, binding1);
                        Binding binding2 = new Binding("Background");
                        binding2.Source = rbList.Last();
                        rect_right.SetBinding(Rectangle.FillProperty, binding2);
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
                        animateMove();
                }

                private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
                {
                        rbIndex += (int)-e.Delta / 120;
                        rbIndex = rbIndex < 0 ? 0 : (rbIndex == rbList.Count ? rbList.Count-1 : rbIndex);
                        rbList[rbIndex].IsChecked = true;
                        animateMove();
                }

                private void animateMove()
                {
                        ThicknessAnimation anim = new ThicknessAnimation(new Thickness(1, -rbIndex*200, 1, 1), new Duration(TimeSpan.FromSeconds(0.3)));
                        grid_content.BeginAnimation(Grid.MarginProperty, anim);
                }
        }
}
