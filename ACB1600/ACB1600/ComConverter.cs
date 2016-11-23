using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACB1600
{
        public class ComConverter
        {
                /// <summary>
                /// 十六进制模式
                /// </summary>
                /// <param name="source">源byte[2]</param>
                /// <param name="extra">暂时无用</param>
                /// <returns></returns>
                public string CvtR0(byte[]source,string extra)
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
                        string[] values = value.Split('_');
                        byte[] bts = new byte[2];
                        bts[0] = Convert.ToByte(values[0], 16);
                        bts[1] = Convert.ToByte(values[1], 16);
                        return bts;
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
                        double factor = double.Parse(extra);
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
                        double factor = double.Parse(extra);
                        double data1=double.Parse(value);
                        int data = Convert.ToInt32(data1 * factor);
                        byte[] bts = new byte[2];
                        bts[0] = (byte)(data / 256);
                        bts[1] = (byte)(data % 256);
                        return bts;
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
                        int index =listExt.Contains(value)? listExt.IndexOf(value):0;
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
                        int index=items.ToList().IndexOf(value) - 1;
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
                        return string.Format("0x{0:X4}",source[0] * 256 + source[1]);
                }
                /// <summary>
                /// 开关量模式
                /// </summary>
                /// <param name="value">写入值</param>
                /// <param name="extra">开关列表</param>
                /// <returns></returns>
                public byte[] CvtW3(string value, string extra)
                {
                        byte[] bts = new byte[2];
                        int data = int.Parse(value);
                        bts[0] = (byte)(data / 256);
                        bts[1] = (byte)(data % 256);
                        return bts;
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
        }
}
