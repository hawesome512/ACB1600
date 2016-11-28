using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace ACB1600
{
        class AutoBox
        {
                public static void setText(Control c, string text)
                {
                        if (c is ComboBox)
                        {
                                ComboBox cBox = c as ComboBox;
                                if (cBox.Items.Contains(text))
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
                                return null;
                        }
                }
        }
}
