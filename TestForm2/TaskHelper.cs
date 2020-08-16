using Collector;
using ModbusHelper;

namespace TestForm2
{

  

    public   class  TaskHelper
    {
        public TaskHelper(Collector.CollectorTask<TaskContext> c,ModbusCvt.ModbusType modbusType)
        {
            Comm = c;

            CurrentModbusType = modbusType;
            if (!Comm.IsRun)
            {
                Comm.Run();
            }
        }



        public   Collector.CollectorTask<TaskContext> Comm;
        public   ModbusCvt.ModbusType CurrentModbusType = ModbusCvt.ModbusType.RTU;
        public   void AddTask(string Name,string station, ModbusCvt.OperationCode operationCode,string data)
        {
            //获得完整报文
            byte[] a = ModbusCvt.DataPacking(CurrentModbusType, operationCode, ModbusCvt.HexStringToBytes(station)[0], ModbusCvt.HexStringToBytes(data));
            TaskContext t = new TaskContext();
            t.TaskName = Name;
            t.TX = a;
            if (Comm != null)
            {
                Comm.AddOrUpdateTaskToQueue(t);
            }
        }
        public   void AddTempTask(string Name, string station, ModbusCvt.OperationCode operationCode, string data)
        {
            byte[] a = ModbusCvt.DataPacking(CurrentModbusType, operationCode, ModbusCvt.HexStringToBytes(station)[0], ModbusCvt.HexStringToBytes(data));
            #region 
            TaskContext t = new TaskContext();
            t.TaskName = Name;
            t.TX = a;
            t.ExecuteOnce = true;//只发送一次  ！：如果不为ture 这个任务会被循环执行
            t.Priority = TaskPriority.High;//即刻发送  ！：设置优先级使这个任务立即被执行一次，然后不管会不会成功 这个任务都会被降级为普通任务
            t.IsTempTask = true;//临时任务 ：在调用查找该任务的同时会把该任务删除  ！：在查找方法返回结果值后、这个被查找的任务就被删除掉了
            if (Comm != null)
            {
                Comm.AddOrUpdateTaskToQueue(t);
            }
            #endregion

        }

        public   void AddTempTask(string Name, string station, ModbusCvt.OperationCode operationCode, params byte[] data)
        {
            byte[] a = ModbusCvt.DataPacking(CurrentModbusType, operationCode, ModbusCvt.HexStringToBytes(station)[0], data);
            #region 
            TaskContext t = new TaskContext();
            t.TaskName = Name;
            t.TX = a;
            t.ExecuteOnce = true;//只发送一次  ！：如果不为ture 这个任务会被循环执行
            t.Priority = TaskPriority.High;//即刻发送  ！：设置优先级使这个任务立即被执行一次，然后不管会不会成功 这个任务都会被降级为普通任务
            t.IsTempTask = true;//临时任务 ：在调用查找该任务的同时会把该任务删除  ！：在查找方法返回结果值后、这个被查找的任务就被删除掉了
            if (Comm != null)
            {
                Comm.AddOrUpdateTaskToQueue(t);
            }
            #endregion

        }

        public   void AddControlTask(string Name, string station, ModbusCvt.OperationCode operationCode, string data)
        {
            byte[] a = ModbusCvt.DataPacking(CurrentModbusType, operationCode, ModbusCvt.HexStringToBytes(station)[0], ModbusCvt.HexStringToBytes(data));
            #region 
            TaskContext t = new TaskContext();
            t.TaskName = Name;
            t.TX = a;
            t.IsControlTask = true;
            t.ExecuteOnce = true;//只发送一次  ！：如果不为ture 这个任务会被循环执行
            t.Priority = TaskPriority.High;//即刻发送  ！：设置优先级使这个任务立即被执行一次，然后不管会不会成功 这个任务都会被降级为普通任务
            t.IsTempTask = true;//临时任务 ：在调用查找该任务的同时会把该任务删除  ！：在查找方法返回结果值后、这个被查找的任务就被删除掉了
            if (Comm != null)
            {
                Comm.AddOrUpdateTaskToQueue(t);
            }
            #endregion

        }
        public   void DelTask(string Name)
        {
            Comm.RemoveTaskToQueue((s) => { return s.TaskName == Name; });

        }
        public   void DelControlTask()
        {
            Comm.RemoveTaskToQueue((s) => { return s.IsControlTask; });

        }

        public   short[] GetTaskRst(string Name)
        {
            short[] a = null;
            TaskContext t = Comm.GetTask(s => { return s.TaskName.Equals(Name); });
            if (!t.IsSuccess) return a;
            a = ModbusCvt.DataUnPackingToShort(CurrentModbusType, t.RX);
            return a;
        }

        public   bool CheckTaskRst(string Name)
        {
            short[] a = null;
            TaskContext t = Comm.GetTask(s => { return s.TaskName.Equals(Name); });
            return t.IsSuccess;
        }

    }
}
