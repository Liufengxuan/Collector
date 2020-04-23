using Collector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace TestForm
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        public struct TestContext : ITaskContext
        {
            private string _TaskName;
            public string TaskName
            {
                get { return _TaskName; }
                set { _TaskName = value; }
            }

            private TaskPriority _taskPriority;
            public   TaskPriority Priority
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
                RX = rx;
            }
        }

        private static Collector.Task<TestContext> task;
        private static ModbusHelper.ModbusType modbusType;

        #region 初始化任务执行容器
        private void btn_TCPStart_Click(object sender, EventArgs e)
        {
            btn_close_Click(null, null);
            task = new Task<TestContext>(new Collector.Channel.TcpChannel());//创建任务类并 给予一个数据通道
            task.ExceptionEvent += ShowMsg;//订阅Collector 中出错抛出的异常
            modbusType = ModbusHelper.ModbusType.Tcp;
            panel1.Enabled = true;
            task.Run();
        }
        private void btn_SPstart_Click(object sender, EventArgs e)
        {
            btn_close_Click(null, null);
            task = new Task<TestContext>(new Collector.Channel.SerialChannel());
            task.ExceptionEvent += ShowMsg;
            modbusType = ModbusHelper.ModbusType.RTU;
            panel1.Enabled = true;
            task.Run();
        }
        #endregion

        #region 删除一个任务Demo
        private void btn_RemoveTask_Click(object sender, EventArgs e)
        {

            ///创建并指定taskname 然后添加进删除队列
            TestContext t = new TestContext();
            t.TaskName = tb_taskName.Text;
            task.RemoveTaskToQueue(t);
        }
        #endregion

        #region 查找任务Demo
        private void btn_Find_Click(object sender, EventArgs e)
        {
            ///自定义表达式 查找这个任务
            if (task != null)
            {
                TestContext t = task.GetTask(s => { return s.TaskName.Equals(tb_taskName.Text); });
                if (t.TaskName != null)
                {
                    MessageBox.Show(ModbusHelper.BytesToHexString(t.RX));
                }
            }
        }

        #endregion

        #region 创建一个控制输出的任务 Demo
        private void button2_Click(object sender, EventArgs e)
        {
            ModbusHelper.OperationCode c = (ModbusHelper.OperationCode)Convert.ToByte(tb_Code.Text);//获取操作码

            //将给定的  操作码、站号 、数据  按照modbusType 打包为标准报文格式
            byte[] a = ModbusHelper.DataPacking(modbusType, c, ModbusHelper.HexStringToBytes(tb_Station.Text)[0], ModbusHelper.HexStringToBytes(tb_data.Text));





            //按钮点动输出 都应该这样设置特别是t.ExecuteOnce = true;  如果不写这一句、会一直发送指令给仪表。
            //执行完你创建的任务应该把他删除、否则他会一直存在用任务列表、影响迭代效率
            #region 创建一个控制输出的任务 Demo
            TestContext t = new TestContext();
            t.TaskName = tb_taskName.Text;
            t.TX = a;
            t.ExecuteOnce = true;//只发送一次  ！：如果不为ture 这个任务会被循环执行
            t.Priority = TaskPriority.Normal;//即刻发送  ！：设置优先级使这个任务立即被执行一次，然后不管会不会成功 这个任务都会被降级为普通任务
            t.IsTempTask = true;//临时任务 ：在调用查找该任务的同时会把该任务删除  ！：在查找方法返回结果值后、这个被查找的任务就被删除掉了
            if (task != null)
            {
                task.AddOrUpdateTaskToQueue(t);
            }
            #endregion





        }
        #endregion

        #region 创建一个状态读取任务 DEMO
        private void AddTask(string TaskName, byte[] tx)
        {
            // 创建一个状态读取任务
            TestContext t = new TestContext();
            t.TaskName = TaskName;
            t.TX = tx;

            if (task != null)
            {
                task.AddOrUpdateTaskToQueue(t);
            }


        }
        #endregion



















        private void TestForm_Load(object sender, EventArgs e)
        {

        }
        private void ShowMsg(Exception  format,int errcount)
        {
            try
            {

                //string msg = string.Format("[" + DateTime.Now.ToLongTimeString() + "]:" + format, org0);
                Action<string> actionDelegate = (x) =>
                {
                    textBox1.AppendText(x.ToString());
                    textBox1.ScrollToCaret();

                };
                if (textBox1.InvokeRequired)
                {
                    textBox1.Invoke(actionDelegate, format.Message+"{"+errcount.ToString()+"}" + "\r\n");
                }
                else
                {
                    textBox1.AppendText(format.Message + "{" + errcount.ToString() + "}" + "\r\n");
                    textBox1.ScrollToCaret();
                }

            }
            catch
            {

            }

        }


     

        private void btn_OpenConfig_Click(object sender, EventArgs e)
        {
            if (task != null)
            {
                task.OpenConfig();
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            if (task != null)
            {
                if (task.IsRun)
                {
                    task.Stop();
                }
                else
                {
                    task.Run();
                }
            }
            //   task = null;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (task != null)
            {
                label1.Text = task.CurrentChan.GetChannelType();
                if (task.IsRun)
                {
                    panel1.Enabled = true;
                }
                else
                {
                    panel1.Enabled = false;
                }

                if (task.IsRun)
                {
                    btn_close.Text = "停止";
                }
                else
                {
                    btn_close.Text = "运行";
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (task != null)
            {
                if (task.IsRun)
                {
                    task.Stop();
                    task = null;
                }
            }
          
        }

        public static bool a=false;
        private void timer2_Tick(object sender, EventArgs e)
        {
            textBox2.Text = "";
            if (a) return;
            a = true;
            if (task == null) { a = false; return;  } 


          List<TestContext>  l= task.GetAllTask(s=> { return true; });
              
                if (l.Count > 0)
                {
                textBox2.Text = "";
                ShowMsg(DateTime.Now.ToString());
                foreach (TestContext item in l)
                    {
                    if (item.RX != null && item.TX != null)
                    {
                        string s = string.Format("taskname={0} TX={1} RX={2} IsSuccess={3} IsTempTask={4} Priority={5}", item.TaskName, BitConverter.ToString(item.TX, 0).Replace("-", string.Empty).ToUpper(), BitConverter.ToString(item.RX, 0).Replace("-", string.Empty).ToUpper()
                            ,item.IsSuccess.ToString(),item.IsTempTask.ToString(),item.Priority.ToString());
                        ShowMsg(s);
                    }
                        
                    }
                }
         
            a = false;

           
           
         
          
        }
        private void ShowMsg(string format)
        {
            try
            {

                //string msg = string.Format("[" + DateTime.Now.ToLongTimeString() + "]:" + format, org0);
                Action<string> actionDelegate = (x) =>
                {
                    textBox2.AppendText(x.ToString());
                    textBox2.ScrollToCaret();

                };
                if (textBox2.InvokeRequired)
                {
                    textBox2.Invoke(actionDelegate, format + "\r\n");
                }
                else
                {
                    textBox2.AppendText(format + "\r\n");
                    textBox2.ScrollToCaret();
                }

            }
            catch
            {

            }

        }


        int addr = 1;
        private void btn_addtask_Click(object sender, EventArgs e)
        {

         

            ModbusHelper.OperationCode c =(ModbusHelper.OperationCode)Convert.ToByte(tb_Code.Text);

            byte[] a= ModbusHelper.DataPacking(modbusType, c, ModbusHelper.HexStringToBytes(tb_Station.Text)[0], ModbusHelper.HexStringToBytes(tb_data.Text));
           
            addr++;
            string s = addr.ToString().PadLeft(4, '0')+"0006";
            tb_data.Text = s;
            tb_taskName.Text = "t" + addr.ToString(); ;
            AddTask(tb_taskName.Text, a);


        }

        private void button3_Click(object sender, EventArgs e)
        {
            byte[] a = new byte[] { 0x0A, 0x03, 0x04, 0x00, 0xA6, 0x00, 0xAC, 0xA0, 0xAD };
            byte[] b = new byte[] { 0x00,0x00, 0x00, 0x00, 0x00, 0x09, 0x0A, 0x03, 0x04, 0x00, 0xA6, 0x00, 0xAC, 0xA0, 0xAD };
            ModbusHelper.DataUnPackingToDouble(ModbusHelper.ModbusType.RTU, a,3);
            ModbusHelper.DataUnPackingToDouble(ModbusHelper.ModbusType.Tcp, b, 3);


            ModbusHelper.DataUnPackingToShort(ModbusHelper.ModbusType.RTU, a);
            ModbusHelper.DataUnPackingToShort(ModbusHelper.ModbusType.Tcp, b);
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            button2_Click(null, null);
        }

        private void button2_MouseUp(object sender, MouseEventArgs e)
        {
            button2_Click(null, null);
        }
    }
}
