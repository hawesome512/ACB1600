using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ACB1600
{
        //Convert
        //0——16进制，Extra:空
        //1——10进制，Extra:①系数②系数_选项1_选项2……选项n③系数_start*step*end③_SAVE此参数需要保存
        //2——固定值，Extra:十进制1_对应值1_十进制2_对应值2……_十进制n_对应值n
        //3——开关量，Extra:位1name/OFF/ON_位2name/OFF/ON……_位15name/OFF/ON，OFF/ON为缺省必须省略，其他开关选项必须列明，用'/'分割
        //--------------------------------ACB1600专有转换格式-------------------------------------
        //11——数组元素，Extra:数组长度
        //12——版本号，Extra:空
        //13——Output输出触点，Extra:同于固定值

        public class ComConverter
        {
                /// <summary>
                /// 十六进制模式
                /// </summary>
                /// <param name="source">源byte[2]</param>
                /// <param name="extra">暂时无用</param>
                /// <returns></returns>
                public string CvtR0(byte[] source, string extra)
                {
                        return string.Format("{0:X2} {1:X2}", source[0], source[1]);
                }
                /// <summary>
                /// 十六进制
                /// </summary>
                /// <param name="value">写入值</param>
                /// <param name="extra">暂时无用</param>
                /// <returns></returns>
                public byte[] CvtW0(string value, string extra)
                {
                        if (Regex.IsMatch(value, @"^[a-fA-F0-9]{2} [a-fA-F0-9]{2}$"))
                        {
                                string[] values = value.Split(' ');
                                byte[] bts = new byte[2];
                                bts[0] = Convert.ToByte(values[0], 16);
                                bts[1] = Convert.ToByte(values[1], 16);
                                return bts;
                        }
                        else
                        {
                                return null;
                        }
                }

                /// <summary>
                /// 十进制模式
                /// </summary>
                /// <param name="source">源byte[2]</param>
                /// <param name="extra">相除系数</param>
                /// <returns></returns>
                public string CvtR1(byte[] source, string extra)
                {
                        int value = source[0] * 256 + source[1];
                        double factor = double.Parse(extra.Split('_')[0]);
                        return (value / factor).ToString();
                }
                /// <summary>
                /// 十进制模式
                /// </summary>
                /// <param name="value">写入值</param>
                /// <param name="_extra">相乘系数</param>
                /// <returns></returns>
                public byte[] CvtW1(string value, string extra)
                {
                        double data1;
                        if (double.TryParse(value, out data1))
                        {
                                double factor = double.Parse(extra.Split('_')[0]);
                                int data = Convert.ToInt32(data1 * factor);
                                byte[] bts = new byte[2];
                                bts[0] = (byte)(data / 256);
                                bts[1] = (byte)(data % 256);
                                return bts;
                        }
                        else
                        {
                                return null;
                        }
                }

                /// <summary>
                /// 固定值模式
                /// </summary>
                /// <param name="source">源byte[2]</param>
                /// <param name="extra">选项</param>
                /// <returns></returns>
                public string CvtR2(byte[] source, string extra)
                {
                        List<string> listExt = extra.Split('_').ToList();
                        string value = (source[0] * 256 + source[1]).ToString();
                        int index = listExt.Contains(value) ? listExt.IndexOf(value) : 0;
                        return listExt[index + 1];
                }
                /// <summary>
                /// 固定值模式
                /// </summary>
                /// <param name="value">写入值</param>
                /// <param name="_extra">选项</param>
                /// <returns></returns>
                public byte[] CvtW2(string value, string extra)
                {
                        byte[] bts = new byte[2];
                        var items = extra.Split('_');
                        int index = items.ToList().IndexOf(value) - 1;
                        int n = int.Parse(items[index]);
                        bts[0] = (byte)(n / 256);
                        bts[1] = (byte)(n % 256);
                        return bts;
                }

                /// <summary>
                /// 开关量模式
                /// </summary>
                /// <param name="source">源byte[2]</param>
                /// <param name="extra">开关列表</param>
                /// <returns></returns>
                public string CvtR3(byte[] source, string extra)
                {
                        return string.Format("0x{0:X4}", source[0] * 256 + source[1]);
                }
                /// <summary>
                /// 开关量模式
                /// </summary>
                /// <param name="value">写入值</param>
                /// <param name="extra">开关列表</param>
                /// <returns></returns>
                public byte[] CvtW3(string value, string extra)
                {
                        if (Regex.IsMatch(value, @"^0x[A-F0-9]{4}$"))
                        {
                                byte[] bts = new byte[2];
                                int data = Convert.ToInt32(value, 16);
                                bts[0] = (byte)(data / 256);
                                bts[1] = (byte)(data % 256);
                                return bts;
                        }
                        else
                        {
                                return null;
                        }
                }

                /// <summary>
                /// 数组模式
                /// </summary>
                /// <param name="source">源byte[]</param>
                /// <param name="extra"></param>
                /// <returns></returns>
                public int[] CvtR11(byte[] source, string extra)
                {
                        int[] result = new int[source.Length / 2];
                        for (int i = 0; i < result.Length; i++)
                        {
                                result[i] = source[2 * i] * 256 + source[2 * i + 1];
                        }
                        return result;
                }

                /// <summary>
                /// 软硬件版本
                /// </summary>
                /// <param name="source">源byte[2]</param>
                /// <param name="extra"></param>
                /// <returns></returns>
                public string CvtR12(byte[] source, string extra)
                {
                        string value = (source[0] * 256 + source[1]).ToString("X");
                        return string.Format("H{0}.{1} S{2}.{3}", value[0], value[1], value[2], value[3]);
                }

                /// <summary>
                /// Output节点
                /// </summary>
                /// <param name="source">源byte[2]</param>
                /// <param name="extra"></param>
                /// <returns></returns>
                public string CvtR13(byte[] source, string extra)
                {
                        return string.Format("{0} {1}", Convert.ToChar(source[0]), Convert.ToChar(source[1]));
                }

                /// <summary>
                /// Output节点
                /// </summary>
                /// <param name="value">写入值</param>
                /// <param name="extra"></param>
                /// <returns></returns>
                public byte[] CvtW13(string value, string extra)
                {
                        if (Regex.IsMatch(value, @"^\w \w$"))
                        {
                                byte[] bts = new byte[2];
                                bts[0] = Convert.ToByte(value[0]);
                                bts[1] = Convert.ToByte(value[2]);
                                return bts;
                        }
                        else
                        {
                                return null;
                        }
                }
        }
}
