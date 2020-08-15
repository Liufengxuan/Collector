using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;

namespace Collector.Channel
{
    public class SerialChannel:BaseChannel
    {
        public  object obj = null;
        public SerialChannel(byte stopBits,int portNum,byte byteSize,int baudRate,byte Parity,int readTimeOut ,int writeTimeOut) 
        {
            SerialPortAPI sp = new SerialPortAPI();
            sp.StopBits = stopBits;
            sp.PortNum = portNum;
            sp.ByteSize = byteSize;
            sp.BaudRate = baudRate;
            sp.Parity = Parity;
            sp.ReadTimeout = readTimeOut;
            sp.WriteTimeout = writeTimeOut;
            SPapi = sp;
        }
      

        private SerialPortAPI SPapi = null;
      

        public override bool Close()
        {
            return SPapi.Close();
        }
        public override bool Open()
        {
          
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
