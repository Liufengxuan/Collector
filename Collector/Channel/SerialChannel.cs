using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        private int ReadTimeOut=0;

        public override bool Close()
        {
            return SPapi.Close();
        }
        public override bool Open()
        {
            ReadTimeOut = Convert.ToInt32(Parameters.iniOper.ReadIniData("SPService", "ReadTimeOut", ""));
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



        private Stopwatch sw = new Stopwatch();
        private List<byte> readList = new List<byte>();
        public override byte[] Read(int NumBytes)
        {
            readList.Clear();
            sw.Reset();
            sw.Start();
            while (sw.ElapsedMilliseconds < ReadTimeOut)
            {
                long a = sw.ElapsedMilliseconds;
                readList.AddRange(SPapi.Read(NumBytes));
            }
            SPapi.ClearPortData();
            sw.Stop();         
            return readList.ToArray();
        }

        public override int Write(byte[] WriteBytes)
        {
            return SPapi.Write(WriteBytes);
        }

        public override string GetChannelType()
        {
            return "SerialPort";
           
        }
    }
}
