using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ModbusHelper
{
    public class ModbusRtuReceive : IModbusReceive
    {
        public ModbusRtuReceive(int timeOut)
        {
            TimeOut = timeOut;
        }


        private  Stopwatch sw = new Stopwatch();

        /// <summary>
        /// 实际的超时时间为TimeOut+需要接收的字节数
        /// </summary>
        public  int TimeOut =0;
        private  int recLength = 0;
        private  byte[] res = new byte[0];
        private  byte[] sendByte;
        private  List<byte> buf = new List<byte>();
        private  int ErrorCount=0;

       

        /// <summary>
        /// 如果返回的是错误码、抛出异常 ；其他情况返回空。
        /// </summary>
        /// <param name="t"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public  byte[] Receive(Collector.ITaskContext t, Collector.Channel.BaseChannel channel)
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


            sw.Restart();
            int timeSpan = recLength + TimeOut;
            while (sw.ElapsedMilliseconds < timeSpan)
            {               
                buf.AddRange(channel.Read(64));

                if (buf.Count > 2) //判断返回的是否有错误
                {
                    if (buf[1] != sendByte[1]) {
                        buf.Clear();
                        break;
                    }
                }
                if (buf.Count >= recLength)//接收完指定长度后，判断crc是否通过
                {
                    sw.Stop();
                    if (buf.Count != recLength)
                    {
                        buf.Clear();//////////////////////
                    }
                    else if (!ModbusCvt.CheckDataCrc16(buf.ToArray()) || buf.Count != recLength)
                    {
                        buf.Clear();//////////////////////
                    }
                    break;
                }
            }
            sw.Stop();
            if (buf.Count > 0)
            {
                ErrorCount = 0;
            }
            else
            {
                ErrorCount++;
            }
            if (ErrorCount > 3)
            {
               
                throw new Exception("串口多次未读取到数据、请检查通讯是否有问题");
            }
            return buf.ToArray();
        }

        public  byte[] Receive2(Collector.ITaskContext t, Collector.Channel.BaseChannel channel)
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


            int i = 0;
            int timeSpan = recLength + TimeOut;
            while (i< timeSpan)
            {
                i++;
                buf.AddRange(channel.Read(128));

                if (buf.Count > 2) //判断返回的是否有错误
                {
                    if (buf[1] != sendByte[1])
                    {
                        buf.Clear();
                        break;
                    }
                }
                if (buf.Count >= recLength)//接收完指定长度后，判断crc是否通过
                {
                    if (buf.Count != recLength)
                    {
                        buf.Clear();//////////////////////
                    } 


                  else  if (!ModbusCvt.CheckDataCrc16(buf.ToArray()) || buf.Count != recLength)
                    {
                        buf.Clear();//////////////////////
                    }
                    break;
                }
            }
          
            if (buf.Count > 0)
            {
                ErrorCount = 0;
            }
            else
            {
                ErrorCount++;
            }
            if (ErrorCount > 3)
            {

                throw new Exception("串口多次未读取到数据、请检查通讯是否有问题");
            }
            return buf.ToArray();
        }

        public  int Send(Collector.ITaskContext t, Collector.Channel.BaseChannel channel)
        {

            int i= channel.Write(t.GetTX());
   

            return i;
        }

    }
}
