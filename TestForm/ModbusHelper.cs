using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestForm
{

    public static class ModbusHelper
    {
        public enum OperationCode
        {
            Code03 = 0x03,
            Code06 = 0x06,
            Code10 = 0x10,
        }      
        public enum ModbusType
        {
            RTU,
            Tcp
        }

        #region 将数据打包为 Modbus 和 Rtu 或者Tcp报文格式
        /// <summary>
        /// 将数据打包为 Modbus 和 Rtu 或者Tcp报文格式
        /// </summary>
        /// <param name="modbusType">协议类型</param>
        /// <param name="operationCode">操作码字节</param>
        /// <param name="station">单元字节</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static byte[] DataPacking(ModbusType modbusType, OperationCode operationCode, byte station, params byte[] data)
        {
            List<byte> list = new List<byte>();
            byte[] result = null;
            if (modbusType == ModbusType.Tcp)
            {
                list.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00 });
                byte[] length = BitConverter.GetBytes((short)(1 + 1 + data.Count()));
                list.Add(length[1]);
                list.Add(length[0]);
                list.Add(station);
                list.Add((byte)operationCode);
                list.AddRange(data);
                result = list.ToArray();
            }
            if (modbusType == ModbusType.RTU)
            {
                list.Add(station);
                list.Add((byte)operationCode);
                list.AddRange(data);
                byte[] crc = GetModbusCRC16_Byte(list.ToArray());
                list.AddRange(crc);
                result = list.ToArray();
            }
            string s = BytesToHexString(result);
            return result;


        }
        #endregion

        #region 将数据解包为某个类型



        /// <summary>
        /// 将Rtu 或者TCP 的报文数据部分提取出来
        /// </summary>
        /// <param name="modbusType"></param>
        /// <param name="rx"></param>
        /// <returns></returns>
        private static byte[] SplitData(ModbusType modbusType, byte[] rx)
        {
            int tcpDataHeadIndex = 9;
            // int tcpDataEndIndex = rx.Length-1;
            int rtuDataHeadIndex = 3;
            //int rtuDataEndIndex = rx.Length -3;
            byte[] buf = null;

            if (modbusType == ModbusType.RTU)
            {
                if (CheckDataCrc16(rx))
                {

                    int dataLen = Convert.ToInt32(rx[rtuDataHeadIndex - 1]);
                    buf = new byte[dataLen];
                    Buffer.BlockCopy(rx, rtuDataHeadIndex, buf, 0, dataLen);
                }

            }
            if (modbusType == ModbusType.Tcp)
            {
                int dataLen = Convert.ToInt32(rx[tcpDataHeadIndex - 1]);
                buf = new byte[dataLen];
                Buffer.BlockCopy(rx, tcpDataHeadIndex, buf, 0, dataLen);
            }

            return buf;
        }

        /// <summary>
        ///  将返回数据解析为int16 数组
        /// </summary>
        /// <param name="modbusType">modbusType类型</param>
        /// <param name="rx">完整返回报文</param>
        /// <returns></returns>
        public static short[] DataUnPackingToShort(ModbusType modbusType, byte[] rx)
        {
            byte[] byfer=SplitData(modbusType, rx);
            int dataLen = byfer.Length / 2;
            short[] s = new short[dataLen];

            int d = 0;
            for (int i = 0; i < byfer.Length; i=i+2,d++)
            {
                s[d] = BitConverter.ToInt16(new byte[] { byfer[i+1], byfer[i] }, 0);
            }
            return s;

        }
        /// <summary>
        /// 将返回数据解析为Double 数组
        /// </summary>
        /// <param name="modbusType">modbusType类型</param>
        /// <param name="rx">完整返回报文</param>
        /// <param name="pointNum">小数位数</param>
        /// <returns></returns>
        public static double[] DataUnPackingToDouble(ModbusType modbusType, byte[] rx,int pointNum)
        {
            int a = 10;
            if (pointNum == 0) a = 0;
            for (int i = 1; i < pointNum; i++)
            {
                a = a * 10;
            }


            byte[] byfer = SplitData(modbusType, rx);
            int dataLen = byfer.Length / 2;
            double[] s = new double[dataLen];

            int d = 0;
            for (int i = 0; i < byfer.Length; i = i + 2, d++)
            {
                s[d] = BitConverter.ToInt16(new byte[] { byfer[i + 1], byfer[i] }, 0);
                if (a != 0)
                {
                    s[d] = s[d] / a;
                }
               
            }
            return s;

        }



        #endregion






        #region Byte数组 -> 16进制字符串
        public static string BytesToHexString(byte[] InBytes) //112233
        {
            StringBuilder StringOut = new StringBuilder(string.Empty);
            try
            {
                foreach (byte InByte in InBytes)
                    StringOut.Append(Convert.ToString(InByte, 16).PadLeft(2, '0'));
            }
            catch
            { }
            return StringOut.ToString().ToUpper();
        }
        #endregion

        #region 16进制字符串 -> byte数组
        /// <summary>
        /// 字符串转16进制字符数组
        /// </summary>
        /// <param name="str"></param>
        /// <param name="isFilterChinese">是否过滤掉中文字符</param>
        /// <returns></returns>
        public static byte[] HexStringToBytes(string str, bool isFilterChinese = false)
        {
            string hex = isFilterChinese ? FilterChinese(str) : ConvertChinese(str);

            //清除所有空格
            hex = hex.Replace(" ", "");
            //若字符个数为奇数，补一个0
            hex += hex.Length % 2 != 0 ? "0" : "";

            byte[] result = new byte[hex.Length / 2];
            for (int i = 0, c = result.Length; i < c; i++)
            {
                result[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return result;
        }
        private static string FilterChinese(string str)
        {
            StringBuilder s = new StringBuilder();
            foreach (short c in str.ToCharArray())
            {
                if (c > 0 && c < 127)
                {
                    s.Append((char)c);
                }
            }
            return s.ToString();
        }
        private static string ConvertChinese(string str)
        {
            StringBuilder s = new StringBuilder();
            foreach (short c in str.ToCharArray())
            {
                if (c <= 0 || c >= 127)
                {
                    s.Append(c.ToString("X4"));
                }
                else
                {
                    s.Append((char)c);
                }
            }
            return s.ToString();
        }
        #endregion


        #region Crc16 校验码
        /// <summary>
        /// 根据给定数据部分  计算crc校验
        /// </summary>
        /// <param name="data">没有校验码的数据</param>
        /// <param name="isReverse"></param>
        /// <returns>crc校验码（两个字节）</returns>
        private static byte[] GetModbusCRC16_Byte(byte[] data, bool isReverse = true)
        {
            byte[] s = new byte[2];
            s = CRC16(data);
            if (s.Length == 2)
            {
                if (isReverse)
                {
                    byte temp = s[0];
                    s[0] = s[1];
                    s[1] = temp;
                }

            }
            return s;

        }
        /// <summary>
        /// 根据给定数据部分，计算出crc校验
        /// </summary>
        /// <param name="data"></param>
        /// <returns>crc校验码（两个字节）</returns>
        private static byte[] CRC16(byte[] data)
        {
            int len = data.Length;
            if (len > 0)
            {
                ushort crc = 0xFFFF;

                for (int i = 0; i < len; i++)
                {
                    crc = (ushort)(crc ^ (data[i]));
                    for (int j = 0; j < 8; j++)
                    {
                        crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
                    }
                }
                byte hi = (byte)((crc & 0xFF00) >> 8);  //高位置
                byte lo = (byte)(crc & 0x00FF);         //低位置

                return new byte[] { hi, lo };
            }
            return new byte[] { 0, 0 };
        }
        //RX=0A03 0C 8044 0000 0002 08C2 0000 0000 -C1A1 
        /// <summary>
        /// 根据给定的完整数据判断数据里面的crc校验对不对
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool CheckDataCrc16(byte[] data)
        {
            if (data.Length < 6)  return false;
            int len = data.Length - 2;
            byte[] buf = new byte[len];
            Buffer.BlockCopy(data, 0, buf, 0, len);
            byte[] a = CRC16(buf);
            if ((a[a.Length - 2] == data[data.Length - 1]) && (a[a.Length - 1] == data[data.Length - 2]))
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}
