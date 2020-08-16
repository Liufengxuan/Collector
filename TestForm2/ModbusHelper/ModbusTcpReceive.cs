using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ModbusHelper
{
    public class ModbusTcpReceive: IModbusReceive
    {
        public ModbusTcpReceive(int timeOut)
        {
            TimeOut = timeOut;
        }

        private    byte[] affair = new byte[2];
        private   Stopwatch sw = new Stopwatch();
        public   int TimeOut =15;
        private   Random random = new Random();
        /// <summary>
        /// 如果返回的是错误码、抛出异常 ；其他情况返回空。
        /// </summary>
        /// <param name="t"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public   byte[] Receive(Collector.ITaskContext t, Collector.Channel.BaseChannel channel)
        {
            sw.Restart();
            byte[] a=new byte[] { };
            while (sw.ElapsedMilliseconds<TimeOut)
            {
                a = channel.Read(256);
                if (a.Length > 2)
                {
                    if (a[0] == affair[0] && a[1] == affair[1])
                    {
                        break;
                    }
                 
                }
              
            }
            sw.Stop();
            return a;

        }

        public   int Send(Collector.ITaskContext t, Collector.Channel.BaseChannel channel)
        {
          
            random.NextBytes(affair);
            byte[] a = t.GetTX();
            a[0] = affair[0];
            a[1] = affair[1];
            return channel.Write(a);
        }


    }
}
