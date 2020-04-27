using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Collector.Channel
{

    /// <summary>
    /// 向串口行为看齐
    /// </summary>
    public class TcpChannel : BaseChannel
    {
        public static object obj = null;
        public   Socket client = null;
        private string IpAddress = "";
        private int Port = 0;
   

        public TcpChannel()
        {

           
        }


        /// <summary>
        /// 此方法没有实现
        /// </summary>
        public override void ClearRecBuffer()
        {
            //byte[] buf = new byte[256];
            //client.Receive(buf, SocketFlags.None);
            return;
        }

        /// <summary>
        /// 此方法没有实现
        /// </summary>
        public override void ClearSendBuffer()
        {
            return;
        }

        public override bool Close()
        {

            client.Close();
            client.Dispose();
            return true;
        }

        public override string GetChannelType()
        {
            return "Tcp/Ip";
        }

        public override ChannelState GetState()
        {
            if (client == null)
            {
                return ChannelState.Closed;
            }
            if (client.Connected)
            {
                return ChannelState.Opened;
            }
            else
            {
                return ChannelState.Closed;
            }
        }

        public override bool Open()
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IpAddress = Parameters.iniOper.ReadIniData("TCPService", "IP", "");
            Port = Convert.ToInt32(Parameters.iniOper.ReadIniData("TCPService", "Prot", ""));
            client.ReceiveTimeout = Convert.ToInt32(Parameters.iniOper.ReadIniData("TCPService", "ReadTimeOut", ""));
            client.SendTimeout = Convert.ToInt32(Parameters.iniOper.ReadIniData("TCPService", "WriteTimeOut", ""));
            client.Connect(IPAddress.Parse(IpAddress), Port);
            return true;
        }

        public override byte[] Read(int NumBytes)
        {
            byte[] buf = new byte[NumBytes];
            byte[] outBuf;
            int a = client.Receive(buf, SocketFlags.None);





            outBuf = new byte[a];
            Array.Copy(buf, outBuf, a);
            return outBuf;
        }

        public override int Write(byte[] WriteBytes)
        {
           return client.Send(WriteBytes);
        }
    }
}
