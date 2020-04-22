using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Collector.Channel;

namespace UnitTestProject1
{
    [TestClass]
    public class ChannelTest
    {
        [TestMethod]
        public void TestTcpChannel()
        {
            BaseChannel tcp = new TcpChannel();
            Assert.AreEqual(tcp.Open(), true);
            Assert.AreEqual(tcp.GetState(),ChannelState.Opened);

            // 00 02 00 00 00 05 06 03 02 00 00
            
            byte[] a = new byte[] { 0x00, 0x02, 0x00, 0x00, 0x00, 0x06, 0x0A, 0x03, 0x00, 0x0D, 0x00, 0x01 };
            int i = 0;
            int success = 0;
            while (true)
            {
                if (i == 100)
                {
                    return;
                }
              
                tcp.Write(a);
                byte[] buf=  tcp.Read(256);
                if (buf.Length==11) success++;
                i++;
                Console.WriteLine(BitConverter.ToString(buf, 0).Replace("-", string.Empty).ToUpper());
            }

          


        }

        [TestMethod]
        public void TestSPChannel()
        {
            BaseChannel tcp = new SerialChannel();
            Assert.AreEqual(tcp.Open(), true);
            Assert.AreEqual(tcp.GetState(), ChannelState.Opened);

            // 00 02 00 00 00 05 06 03 02 00 00

            byte[] a = new byte[] { 0x0A, 0x03, 0x00, 0x00, 0x00, 0x06, 0x12,0x34 };
            int i = 0;
            int success = 0;
            while (true)
            {
                if (i == 100)
                {
                    return;
                }

                tcp.Write(a);
                byte[] buf = tcp.Read(256);
                success++;
                i++;
                Console.WriteLine(BitConverter.ToString(buf, 0).Replace("-", string.Empty).ToLower());
            }




        }
    }
}
