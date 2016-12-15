using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Data;
using Hawesome;
using System.Windows.Media;

namespace ACB1600
{
        enum Authority
        {
                R,
                W,
                RW
        }
        class Node
        {
                public int Address
                {
                        get;
                        set;
                }
                public string Name
                {
                        get;
                        set;
                }
                public string Alias
                {
                        get;
                        set;
                }
                public string Unit
                {
                        get;
                        set;
                }
                public int ShowType
                {
                        get;
                        set;
                }
                public Authority NodeAuthority
                {
                        get;
                        set;
                }
                public string Extra
                {
                        get;
                        set;
                }
                public string Value
                {
                        get;
                        set;
                }
                public List<Influence> Influences
                {
                        get;
                        set;
                }
        }

        class Influence
        {
                //XMl中Influence节点解释规则：Address(0x2005)/依靠值(-1为全值，或相应的位数)/影响效果(0:Name,1:Unit,2:系数,3:选项)
                //选项与系数一起用'_'间隔,值→选项，例如10_1_1*0.5*5_2_1+2+4+8：表示系数为10；可能取值为1时，系数是1*0.5*5；可能取值为2时，选项为1+2+4+8
                //多个影响效果用分号';'间隔
                public Influence(String strInfluence)
                {
                        string[] items1 = strInfluence.Split('/');
                        Address = Convert.ToInt32(items1[0], 16);
                        InfBitType = int.Parse(items1[1]);
                        string[] items2 = items1[2].Split(';');
                        foreach (string item in items2)
                        {
                                string[] items3 = item.Split('_');
                                switch (items3[0])
                                {
                                        case "0":
                                                InfName = initDic(items3.Skip(1).ToArray());
                                                break;
                                        case "1":
                                                InfUnit = initDic(items3.Skip(1).ToArray());
                                                break;
                                        case "2":
                                                InfFactor = initDic(items3.Skip(1).ToArray());
                                                break;
                                        case "3":
                                                InfItems = initDic(items3.Skip(1).ToArray());
                                                break;
                                        case "4":
                                                InfItemsLinkage = (InfLinkage)Enum.Parse(typeof(InfLinkage), items3[1]);
                                                break;
                                        default:
                                                break;
                                }
                        }
                }

                private Dictionary<int, string> initDic(string[] items)
                {
                        Dictionary<int, string> dic = new Dictionary<int, string>();
                        for (int i = 0; i < items.Length; i += 2)
                        {
                                dic.Add(int.Parse(items[i]), items[i + 1].Replace('+', '_'));
                        }
                        return dic;
                }

                public void executeInfluence(Panel parent, int nResult,string strResult)
                {
                        nResult = InfBitType < 0 ? nResult : Tools.GetBit(nResult, InfBitType);
                        if (InfName != null)
                        {
                                Label lblName = Tools.GetChild<Label>(parent, "Name" + Address);
                                lblName.Content = InfName[nResult] == "-" ? string.Empty : InfName[nResult];
                        }
                        if (InfUnit != null)
                        {
                                Label lblUnit = Tools.GetChild<Label>(parent, "Unit" + Address);
                                lblUnit.Content = InfUnit[nResult] == "-" ? string.Empty : InfUnit[nResult];
                        }
                        if (InfFactor != null)
                        {
                                Control box = Tools.GetChild<Control>(parent, "Box" + Address);
                                Node node = box.Tag as Node;
                                string factor = InfFactor.Count == 0 ? nResult.ToString() : InfFactor[nResult];
                                node.Extra = Regex.Replace(node.Extra, @"^\d+(\.\d+)?", factor);
                        }
                        if (InfItems != null)
                        {
                                ComboBox box = Tools.GetChild<ComboBox>(parent, "Box" + Address);
                                Node node = box.Tag as Node;
                                bool infExtra = true;
                                if (node.Address == 0x200B)
                                {
                                        infExtra = false;
                                        bool isTg = (node.Extra.Contains("接地") || node.Extra.Contains("*")) && InfItems.Values.First().Contains('*');
                                        bool isTf = (node.Extra.Contains("漏电") || node.Extra.Contains("0.8")) && InfItems.Values.First().Contains("0.8");
                                        bool isFirst = node.Extra.Contains("Null");
                                        infExtra |= isTg | isTg | isFirst;
                                }
                                if (infExtra)
                                {
                                        node.Extra = Regex.Match(node.Extra, @"^\d+").Value + "_" + InfItems[nResult];
                                        AutoBox.setBoxItems(box);
                                }
                        }
                        if (InfItemsLinkage != InfLinkage.NULL)
                        {                                
                                ComboBox box = Tools.GetChild<ComboBox>(parent, "Box" + Address);
                                Node node = box.Tag as Node;
                                string pattern = InfItemsLinkage == InfLinkage.LOWER ? @"(?<=_)\d+(\.\d+)?(?=\*)" : @"(?<=\*)\d+(\.\d+)?$";
                                node.Extra = Regex.Replace(node.Extra, pattern, strResult);
                                AutoBox.setBoxItems(box);
                        }
                }

                public int Address
                {
                        get;
                        set;
                }
                public int InfBitType
                {
                        get;
                        set;
                }
                public Dictionary<int, string> InfName
                {
                        get;
                        set;
                }
                public Dictionary<int, string> InfUnit
                {
                        get;
                        set;
                }
                public Dictionary<int, string> InfFactor
                {
                        get;
                        set;
                }
                public Dictionary<int, string> InfItems
                {
                        get;
                        set;
                }
                public InfLinkage InfItemsLinkage
                {
                        get;
                        set;
                }
        }
        enum InfLinkage
        {
                NULL,
                UPPER,
                LOWER
        }

        public class String2Brush:IValueConverter
        {
                public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
                {
                        if (value!=null&&value.ToString().Contains("失败"))
                        {
                                return Tools.GetBrush("#FFFF6501");
                        }
                        else
                        {
                                return Brushes.White;
                        }
                }

                public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
                {
                        throw new NotImplementedException();
                }
        }
}
