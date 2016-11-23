using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Net.Sockets;
using System.Threading;


namespace ACB1600
{
        /// <summary>
        /// 通信方式：TCP,SP(串口)
        /// </summary>
        public enum ComType
        {
                TCP,
                SP,
        }

        public class Com : IDisposable
        {
                SerialPort sp;
                TcpClient tcp;
                string comTag;
                ComType cType;
                bool disposed = false;
                public Com(ComType type,string tag)
                {
                        cType = type;
                        comTag=tag;
                }

                ~Com()
                {
                        Dispose();
                }

                public void Dispose()
                {
                        if (!disposed)
                        {
                                if (sp != null)
                                {
                                        sp.Dispose();
                                }
                                disposed = true;
                                GC.SuppressFinalize(this);
                        }
                }
                /// <summary>
                /// 执行通信：读、写
                /// </summary>
                /// <param name="snd"></param>
                /// <returns>Array.Length,0:通信失败；1:写入；2*n:读取</returns>
                public byte[] Execute(byte[] snd)
                {
                        switch (cType)
                        {
                                case ComType.SP:
                                        return spCom(snd);
                                case ComType.TCP:
                                        return tcpCom(snd);
                                default:
                                        return null;
                        }
                }

                private byte[] tcpCom(byte[] _snd)
                {
                        tcp = new TcpClient();
                        tcp.SendTimeout = 200;
                        tcp.ReceiveTimeout = 300;
                        try
                        {
                                tcp.Connect(comTag, 502);
                        }
                        catch
                        {
                                return new byte[] { };
                        }

                        byte[] snd = new byte[6 + _snd.Length];
                        snd[5] = (byte)_snd.Length;
                        _snd.CopyTo(snd, 6);
                        NetworkStream stream = tcp.GetStream();
                        stream.Write(snd, 0, snd.Length);
                        byte[] rcv = new byte[256];
                        try
                        {
                                stream.Read(rcv, 0, rcv.Length);
                        }
                        catch
                        {
                                return new byte[] { };
                        }
                        finally
                        {
                                stream.Dispose();
                                tcp.Close();
                        }
                        if (rcv[7] == 3 && (rcv[5] - rcv[8] == 3))
                        {
                                byte[] rcv1 = new byte[snd[11] * 2];
                                rcv.ToList().CopyTo(9, rcv1, 0, rcv1.Length);
                                return rcv1;
                        }
                        else if (rcv[7] == 16)
                        {
                                return new byte[] { 1 };
                        }
                        else
                        {
                                return new byte[] { };
                        }
                }

                private byte[] spCom(byte[] snd)
                {
                        int count = snd[4] * 256 + snd[5];
                        //数据太长直接判定通信失败
                        if (string.IsNullOrEmpty(comTag)||count>120)
                                return new byte[] { };
                        if (sp == null || !sp.IsOpen)
                        {
                                sp = new SerialPort(comTag);
                                sp.ReadTimeout = 200;
                                sp.WriteTimeout = 200;
                                sp.StopBits = StopBits.Two;
                                sp.RtsEnable = true;
                                sp.Open();
                        }

                        //频繁通信，排除可能残留未读取数据
                        byte[] rcv = new byte[256];
                        if (sp.BytesToRead > 0)
                        {
                                sp.Read(rcv, 0, rcv.Length);
                                rcv = new byte[256];
                        }
                        sp.Write(CRCck(snd), 0, snd.Length + 2);
                        //根据不同的count设定不同的间隔等待时间
                        Thread.Sleep(((count + 19) / 20 + 1) * 100);
                        if (sp.BytesToRead > 0)
                                sp.Read(rcv, 0, rcv.Length);
                        if (rcv[1] == 3)
                        {
                                byte[] rtn = new byte[snd[5] * 2];
                                rcv.ToList().CopyTo(3, rtn, 0, rtn.Length);
                                return rtn;
                        }
                        else if (rcv[1] == 16)
                        {
                                return new byte[] { 1 };
                        }
                        else
                        {
                                return new byte[] { };
                        }
                }

                byte[] CRCck(byte[] data)
                {
                        byte CRC_L = 0xFF;
                        byte CRC_H = 0xFF;   //CRC寄存器 
                        byte SH;
                        byte SL;
                        int j;

                        for (int i = 0; i < data.Length; i++)
                        {
                                CRC_L = (byte)(CRC_L ^ data[i]); //每一个数据与CRC寄存器进行异或 
                                for (j = 0; j < 8; j++)
                                {
                                        SH = CRC_H;
                                        SL = CRC_L;
                                        CRC_H = (byte)(CRC_H >> 1);      //高位右移一位
                                        //CRC_H = (byte)(CRC_H & 0x7F);
                                        CRC_L = (byte)(CRC_L >> 1);      //低位右移一位 
                                        //CRC_L = (byte)(CRC_L & 0x7F);
                                        if ((SH & 0x01) == 0x01) //如果高位字节最后一位为1 
                                        {
                                                CRC_L = (byte)(CRC_L | 0x80);   //则低位字节右移后前面补1 
                                        }             //否则自动补0 
                                        if ((SL & 0x01) == 0x01) //如果低位为1，则与多项式码进行异或 
                                        {
                                                CRC_H = (byte)(CRC_H ^ 0xA0);
                                                CRC_L = (byte)(CRC_L ^ 0x01);
                                        }
                                }
                        }

                        byte[] rtn = new byte[data.Length + 2];
                        data.CopyTo(rtn, 0);
                        rtn[data.Length] = CRC_L;
                        rtn[data.Length + 1] = CRC_H;
                        return rtn;
                        //return CRC_L * 256 + CRC_H;
                }
        }
}
