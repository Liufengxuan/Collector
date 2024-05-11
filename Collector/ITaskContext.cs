using System;
using System.Collections.Generic;

using System.Text;


namespace Collector
{
    /// <summary>
    /// 任务执行的优先级
    /// </summary>
    public enum TaskPriority
    {
        Normal,
        High
    }

    public interface ITaskContext
    {
       
        string TaskName { get; set; }

        /// <summary>
        /// 任务优先级
        /// </summary>
        TaskPriority Priority { get; set; }

        /// <summary>
        /// 是否为临时任务,如果是的话，该任务会再被在外界获取本任务的同时删除本任务
        /// </summary>
        bool IsTempTask { get; set; }

        /// <summary>
        /// 是否通讯成功，指示最近一次的通信是否成功执行
        /// </summary>
        bool IsSuccess { get; set; }


        /// <summary>
        /// 是否只通信一次、如果通信成功IsSuccess=true 则后续不再进行通讯；
        /// </summary>
        bool ExecuteOnce { get; set; }

        /// <summary>
        /// 是否只通信一次、如果通信成功IsSuccess=true 则自动删除。
        /// </summary>
        bool ExecuteOnce_Del { get; set; }


        /// <summary>
        /// 获取要发送的byte[]
        /// </summary>
        /// <returns></returns>
        byte[] GetTX();


        /// <summary>
        /// 接收后内部赋值
        /// </summary>
        /// <param name="rx"></param>
        void SetRX(byte[] rx);
    }

 
}
