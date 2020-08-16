using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Collector
{
    internal partial class WatchForm : Form
    {
        public WatchForm()
        {
            InitializeComponent();
        }
        StringBuilder sb = new StringBuilder();
        List<string> cache = new List<string>();


        public void WatchData( ITaskContext t,byte[] rx)
        {
        
            if (!timer_refresh.Enabled) return;
            if (cache.Count > 200)
            {
                int rCount = cache.Count - 200;
                cache.RemoveRange(0, rCount);
            }
            string time = $"*{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}.{DateTime.Now.Millisecond}\r\n";
            sb.Clear();
            sb.Append(time);
            sb.Append($" 任务名={t.TaskName}\r\n");
            sb.Append($" 优先级={(t.Priority == TaskPriority.High ? "高" : "低")}");
            sb.Append($" 临时任务={t.IsTempTask.ToString()}");
            sb.Append($" 执行结果={t.IsSuccess.ToString()}");
            sb.Append($" 只执行一次={t.ExecuteOnce.ToString()}\r\n");
            sb.Append($" TX={BitConverter.ToString(t.GetTX()).Replace('-', ' ')}\r\n");
            sb.Append($" RX={BitConverter.ToString(rx).Replace('-', ' ')}\r\n\r\n");
            cache.Add(sb.ToString());
         
          //  ShowMsg(sb.ToString());          
        }
    

    
        

        private void WatchForm_FormClosing(object sender, FormClosedEventArgs e)
        {
            timer_refresh.Enabled = false;

        }

        private void btn_StopRefresh_Click(object sender, EventArgs e)
        {
           timer_refresh.Enabled = !timer_refresh.Enabled;
            if (!timer_refresh.Enabled) { btn_StopRefresh.Text = "继续更新"; btn_StopRefresh.BackColor = Color.Red; }
            if (timer_refresh.Enabled) { btn_StopRefresh.Text = "停止更新"; btn_StopRefresh.BackColor = Color.Green; }
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void timer_refresh_Tick(object sender, EventArgs e)
        {
          
                timer_refresh.Enabled = false;
            textBox1.Clear();
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(textBox_Search.Text))
                {
                                  
                    List<string> a = cache.FindAll((s) => { return s.Contains($" 任务名={textBox_Search.Text}"); });
                    foreach (var item in a)
                    {
                        sb.Append(item);
                    }
                    textBox1.AppendText(sb.ToString());
              
                }
                else
                {                              
                    foreach (var item in cache)
                    {
                        sb.Append(item);
                    }
                    textBox1.AppendText(sb.ToString());             
            }



            textBox1.ScrollToCaret();
            timer_refresh.Enabled = true;
            
        }

        private void textBox_Search_TextChanged(object sender, EventArgs e)
        {
         
        }

        private void button_search_Click(object sender, EventArgs e)
        {
            StringBuilder sb2 = new StringBuilder();
                timer_refresh.Enabled = false;
              List<string> a=  cache.FindAll((s) => { return s.Contains($" 任务名={textBox_Search.Text}"); });
                foreach (var item in a)
                {
                sb2.Append(item);
                }
                textBox1.Text = sb2.ToString();
            textBox1.ScrollToCaret();
            if (!timer_refresh.Enabled) { btn_StopRefresh.Text = "继续更新"; btn_StopRefresh.BackColor = Color.Red; }
            if (timer_refresh.Enabled) { btn_StopRefresh.Text = "停止更新"; btn_StopRefresh.BackColor = Color.Green; }
        }
    }
}
