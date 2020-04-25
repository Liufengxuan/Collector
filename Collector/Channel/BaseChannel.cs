using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collector.Channel
{
    public enum ChannelState
    {
        Opened,
        Closed
    }
  
    public  abstract class BaseChannel
    {
    
        public BaseChannel()
        {
        
            //ReadTimeout = Convert.ToInt32(Parameters.iniOper.ReadIniData("Common", "ReadTimeOut", ""));
            //WriteTimeout = Convert.ToInt32(Parameters.iniOper.ReadIniData("Common", "WriteTimeOut", ""));
        }





        //public int ReadTimeout=20;
        //public int WriteTimeout = 20;


        public abstract bool Open();
        public abstract bool Close();
        public abstract string GetChannelType();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="NumBytes">计划读取到的字节数(参考作用)</param>
        /// <returns>截止到超时时间实际读取到的字节</returns>
        public abstract byte[] Read(int NumBytes);
     /// <summary>
     /// 
     /// </summary>
     /// <param name="WriteBytes">要写入的字节数组</param>
     /// <returns>写入成功的长度</returns>
        public abstract int Write(byte[] WriteBytes);
        public abstract ChannelState GetState();


        public abstract void ClearRecBuffer();
        public abstract void ClearSendBuffer();


    }
}
