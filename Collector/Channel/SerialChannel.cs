using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.Text;

namespace Collector.Channel
{
    public class SerialChannel:BaseChannel
    {
        public static object obj = null;
        public SerialChannel() 
        {
            SerialPortAPI sp = new SerialPortAPI();
          
            sp.StopBits = Convert.ToByte(Parameters.iniOper.ReadIniData("SPService", "StopBits", ""));
            sp.PortNum = Convert.ToInt32(Parameters.iniOper.ReadIniData("SPService", "PortNum", ""));
            sp.ByteSize = Convert.ToByte(Parameters.iniOper.ReadIniData("SPService", "ByteSize", ""));
            sp.BaudRate = Convert.ToInt32(Parameters.iniOper.ReadIniData("SPService", "BaudRate", ""));
            sp.Parity = Convert.ToByte(Parameters.iniOper.ReadIniData("SPService", "Parity", ""));
            SPapi = sp;
        }
      

        private SerialPortAPI SPapi = null;
      

        public override bool Close()
        {
            return SPapi.Close();
        }
        public override bool Open()
        {
            SPapi.ReadTimeout = Convert.ToInt32(Parameters.iniOper.ReadIniData("SPService", "ReadTimeOut", ""));
            SPapi.WriteTimeout = Convert.ToInt32(Parameters.iniOper.ReadIniData("SPService", "WriteTimeOut", ""));
            return SPapi.Open();
        }

        public override ChannelState GetState()
        {
            if (SPapi.Opened)
            {
                return ChannelState.Opened;
            }
            else
            {
                return ChannelState.Closed;
            }
        }




        public  override byte[] Read(int NumBytes)
        {
            byte[] a = SPapi.Read(NumBytes);        
            return a;
        }

        public override int Write(byte[] WriteBytes)
        {
            return SPapi.Write(WriteBytes);
        }

        public override string GetChannelType()
        {
            return "SerialPort";          
        }

        /// <summary>
        /// 清空接收缓冲区
        /// </summary>
        public override void ClearRecBuffer()
        {
            SPapi.ClearReceiveBuf();
        }
        /// <summary>
        /// 清空发送缓冲区
        /// </summary>
        public override void ClearSendBuffer()
        {
            SPapi.ClearSendBuf();
        }
    }
}
