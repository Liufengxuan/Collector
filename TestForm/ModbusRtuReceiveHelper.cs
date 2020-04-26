using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TestForm
{
    public class ModbusRtuReceiveHelper
    {
        private static Stopwatch sw = new Stopwatch();

        /// <summary>
        /// 实际的超时时间为TimeOut+需要接收的字节数
        /// </summary>
        public static int TimeOut =8;
        private static int recLength = 0;
        private static byte[] res = new byte[0];
        private static byte[] sendByte;
        private static List<byte> buf = new List<byte>();


        /// <summary>
        /// 如果返回的是错误码、抛出异常 ；其他情况返回空。
        /// </summary>
        /// <param name="t"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public static byte[] Receive(Collector.ITaskContext t, Collector.Channel.BaseChannel channel)
        {
            sendByte = t.GetTX();
           
            buf.Clear();
            #region 

            switch (sendByte[1])
            {
                case 0x03:
                    recLength = (BitConverter.ToInt16(new byte[] { sendByte[sendByte.Length - 3], sendByte[sendByte.Length - 4] }, 0) * 2) + 5;
                    break;
                case 0x06:
                    recLength = sendByte.Length;
                    break;
                case 0x10:
                    recLength = 8;
                    break;
                default:
                    throw new Exception("发送指令中存在不支持的指令码：" + sendByte[1].ToString());
            }


            #endregion


            sw.Reset();
            sw.Start();
            while (sw.ElapsedMilliseconds < recLength+ TimeOut)
            {
                buf.AddRange(channel.Read(recLength));

                if (buf.Count > 2) //判断返回的是否有错误
                {
                    if (buf[1] != sendByte[1]) {
                        channel.ClearRecBuffer();
                        return buf.ToArray();
                    }
                }
                if (buf.Count >= recLength)//接收完指定长度后，判断crc是否通过
                {
                    sw.Stop();
                    if (!ModbusHelper.CheckDataCrc16(buf.ToArray())|| buf.Count!= recLength)
                    {
                        channel.ClearRecBuffer();
                    }
                    break;
                }
            }
            sw.Stop();
            return buf.ToArray();
        }
     
    }
}
