using Collector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForm2
{
    public struct TaskContext : ITaskContext
    {
        private string _TaskName;
        public string TaskName
        {
            get { return _TaskName; }
            set { _TaskName = value; }
        }

        private TaskPriority _taskPriority;
        public TaskPriority Priority
        {
            get { return _taskPriority; }
            set { _taskPriority = value; }
        }
        private bool _IsTempTask;
        public bool IsTempTask
        {
            get { return _IsTempTask; }
            set { _IsTempTask = value; }
        }

        private bool _IsControlTask;
        public bool IsControlTask
        {
            get { return _IsControlTask; }
            set { _IsControlTask = value; }
        }

        private bool _IsSuccess;
        public bool IsSuccess
        {
            get { return _IsSuccess; }
            set { _IsSuccess = value; }
        }

        public bool _ExecuteOnce;
        public bool ExecuteOnce
        {
            get { return _ExecuteOnce; }
            set { _ExecuteOnce = value; }
        }
        public byte[] TX;
        public byte[] RX;

        public byte[] GetTX()
        {
            return TX;
        }

        public void SetRX(byte[] rx)
        {
            if (rx == null)
            {
                IsSuccess = false;

            }
            else if (rx.Length == 0)
            {
                IsSuccess = false;
            }

            RX = rx;




        }
    }
}
