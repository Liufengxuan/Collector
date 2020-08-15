using System;
using System.Collections.Generic;

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
        public  object obj = null;
        public   Socket client = null;
        private string IpAddress = "";
        private int Port = 0;
        private int ReceiveTimeout, SendTimeout;

        public TcpChannel(string ip, int port, int sendTimeOut, int RecTimeOut)
        {
           
            IpAddress = ip;
            Port = port;
           // client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ReceiveTimeout = RecTimeOut;
            SendTimeout = sendTimeOut;
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
            if (client != null)
            {
               // client.Disconnect(false);
                client.Dispose();
            }
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.ReceiveTimeout = ReceiveTimeout;
            client.SendTimeout = SendTimeout;
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
