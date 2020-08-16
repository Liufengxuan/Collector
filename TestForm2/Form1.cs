using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestForm2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Collector.Channel.TcpChannel tcpChannel = new Collector.Channel.TcpChannel("127.0.0.1", 502, 60, 60);

            ModbusHelper.ModbusTcpReceive r = new ModbusHelper.ModbusTcpReceive(50);
            Collector.CollectorTask<TaskContext> collectorTask = new Collector.CollectorTask<TaskContext>(tcpChannel, r.Receive,r.Send);
            TaskHelper taskHelper = new TaskHelper(collectorTask,ModbusHelper.ModbusCvt.ModbusType.Tcp);
            taskHelper.Comm.ExceptionEvent += ShowMsg;
            taskHelper.AddTask("11_dasd312", "01", ModbusHelper.ModbusCvt.OperationCode.Code03, "00000006");
            taskHelper.AddTask("12_dasd312", "01", ModbusHelper.ModbusCvt.OperationCode.Code03, "00010005");
            taskHelper.AddTask("13_dasd312", "01", ModbusHelper.ModbusCvt.OperationCode.Code03, "00020004");
            taskHelper.AddTask("14_dasd312", "01", ModbusHelper.ModbusCvt.OperationCode.Code03, "00030003");
            taskHelper.Comm.ShowWatchForm();



         


        }
        private void button2_Click(object sender, EventArgs e)
        {
            Collector.Channel.TcpChannel tcpChannel = new Collector.Channel.TcpChannel("127.0.0.1", 503, 60, 60);

            ModbusHelper.ModbusTcpReceive r = new ModbusHelper.ModbusTcpReceive(50);
            Collector.CollectorTask<TaskContext> collectorTask = new Collector.CollectorTask<TaskContext>(tcpChannel, r.Receive, r.Send);
            TaskHelper taskHelper = new TaskHelper(collectorTask, ModbusHelper.ModbusCvt.ModbusType.Tcp);
            taskHelper.Comm.ExceptionEvent += ShowMsg;
            taskHelper.AddTask("21_asdasd", "01", ModbusHelper.ModbusCvt.OperationCode.Code03, "00000006");
            taskHelper.AddTask("22_asdasd", "01", ModbusHelper.ModbusCvt.OperationCode.Code03, "00010005");
            taskHelper.AddTask("23_asdasd", "01", ModbusHelper.ModbusCvt.OperationCode.Code03, "00020004");
            taskHelper.AddTask("24_asdasd", "01", ModbusHelper.ModbusCvt.OperationCode.Code03, "00030003");
            taskHelper.Comm.ShowWatchForm();
        }
        private void ShowMsg(Exception format, int errcount)
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
                    textBox1.Invoke(actionDelegate, format.Message + "{" + errcount.ToString() + "}" + "\r\n");
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

       
    }
}
