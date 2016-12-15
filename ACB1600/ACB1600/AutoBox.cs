using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ACB1600
{
        class AutoBox
        {
                public static void setText(Control c, string text)
                {
                        if (c is ComboBox)
                        {
                                ComboBox cBox = c as ComboBox;
                                double value;
                                if (double.TryParse(text, out value))
                                {
                                        string[] strs = new string[cBox.Items.Count];
                                        cBox.Items.CopyTo(strs, 0);
                                        List<double> items=strs.ToList().ConvertAll(s => Math.Abs(double.Parse(s)-value));
                                        cBox.SelectedIndex = items.FindIndex(i => i == items.Min());
                                }
                                else if (cBox.Items.Contains(text))
                                {
                                        cBox.SelectedValue = text;
                                }
                                else
                                {
                                        cBox.SelectedIndex = -1;
                                }
                        }
                        else if (c is TextBox)
                        {
                                TextBox tBox = c as TextBox;
                                tBox.Text = text;
                        }
                        else if (c is Label)
                        {
                                Label lbl = c as Label;
                                lbl.Content = text;
                        }
                }

                public static String getText(Control c)
                {
                        if (c is ComboBox)
                        {
                                ComboBox cBox = c as ComboBox;
                                return cBox.Text;
                        }
                        else if (c is TextBox)
                        {
                                TextBox tBox = c as TextBox;
                                return tBox.Text;
                        }
                        else
                        {
                                Label lbl = c as Label;
                                return lbl.Content.ToString();
                        }
                }

                public static void setBoxItems(ComboBox box)
                {
                        Node node = box.Tag as Node;
                        List<string> items = node.Extra.Split('_').ToList();
                        if (node.ShowType == 2)
                        {
                                items = items.Where((value, i) => i % 2 == 1).ToList();
                        }
                        else if (node.ShowType == 1)
                        {
                                var array = items[1].Split('*');
                                if (array.Length == 3)
                                {
                                        int start = Convert.ToInt32(double.Parse(array[0]) * 1000);
                                        int step = Convert.ToInt32(double.Parse(array[1]) * 1000);
                                        int end = Convert.ToInt32(double.Parse(array[2]) * 1000);
                                        items.Clear();
                                        for (int i = start; i <= end; i += step)
                                        {
                                                items.Add((i / 1000.0).ToString());
                                        }
                                }
                                else
                                {
                                        items = items.Skip(1).ToList();
                                }
                        }
                        (box as ComboBox).ItemsSource = items;
                }
        }
}
