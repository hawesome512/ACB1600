using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ACB1600
{
	/// <summary>
	/// OnOff.xaml 的交互逻辑
	/// </summary>
	public partial class OnOff : UserControl
	{
		public OnOff()
		{
			this.InitializeComponent();
		}

                private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
                {
                        double facotr1 = e.NewSize.Width / 98;
                        double factor2 = e.NewSize.Height / 37;
                        double factor = facotr1 > factor2 ? factor2 : facotr1;
                        toggleSwitch.Width = 98*factor;
                        toggleSwitch.Height = 37*factor;
                        toggleSwitch.CornerRadius = toggleSwitch.ThumbCornerRadius = new CornerRadius(toggleSwitch.Height / 2);
                }
	}
}