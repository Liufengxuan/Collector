using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Collector;
using Collector.Channel;
using System.Threading;

namespace UnitTestProject1
{
    [TestClass]
    public class TaskClassTest
    {
        public struct TestContext : ITaskContext
        {
            private string _TaskName;
            public string TaskName { get { return _TaskName; }
                set { _TaskName = value; } }

            private TaskPriority _taskPriority;
            public TaskPriority Priority
            {
                get { return _taskPriority; }
                set { _taskPriority = value; }
            }
            public bool _IsTempTask;
            public bool IsTempTask
            {
                get { return _IsTempTask; }
                set { _IsTempTask = value; }
            }

            public bool _IsSuccess;
            public bool IsSuccess
            {
                get { return _IsSuccess; }
                set { _IsSuccess = value; }
            }


            public bool _ExecuteOnce;
            public bool ExecuteOnce
            {
                get { return _IsSuccess; }
                set { _IsSuccess = value; }
            }

         

            public byte[] TX;
            public byte[] RX;

            public byte[] GetTX()
            {
                return TX;
            }

            public void SetRX(byte[] rx)
            {
                RX = rx;
            }
        }


        [TestMethod]
        public void TestTaskOpera()
        {
            BaseChannel tcp = new TcpChannel();
            Task<TestContext> task = new Task<TestContext>(tcp);
            task.Run();

            //create task;
            for (int i = 0; i < 100; i++)
            {
                TestContext t = new TestContext() {
                    TaskName = "task" + i.ToString(),
                    IsTempTask = false,
                    Priority = TaskPriority.Normal,
                    TX = new byte[] { 0x00, 0x02, 0x00, 0x00, 0x00, 0x06, 0x0A, 0x03, 0x00, (byte)i, 0x00, 0x01 }

            };
                task.AddOrUpdateTaskToQueue(t);
            }

            Thread.Sleep(1000);//一秒内通信100次测试   
            int success = 0;
                for (int i = 0; i < 100; i++)
                {
                    TestContext t = new TestContext();
                    t.TaskName = "task" + i.ToString();
                  t=  task.GetTask(s=> { return s.Equals(t.TaskName); });
                if (t.RX != null)
                {
                    success++;
                    Console.WriteLine(BitConverter.ToString(t.TX, 0).Replace("-", string.Empty).ToLower());
                    Console.WriteLine(BitConverter.ToString(t.RX, 0).Replace("-", string.Empty).ToLower());
                    Console.WriteLine("-----------------------------------------------------------------");
                }
              


            }
            Console.WriteLine(success.ToString());








            // Assert.AreEqual(task.TaskCount, 100);



        }
    }
}
