using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusHelper
{
    interface IModbusReceive
    {
        byte[] Receive(Collector.ITaskContext t, Collector.Channel.BaseChannel channel);
        int Send(Collector.ITaskContext t, Collector.Channel.BaseChannel channel);
    }
}
