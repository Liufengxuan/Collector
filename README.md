这是一个上位机通讯组件  

他适用于符合以下条件的上位机开发。  
1、使用串口通讯和网口TCP/IP协议通讯；  
2、需要实时读取下位状态信息、和发送控制指令；  


使用方法  
1、修改通讯参数  
   
```      
[SPService]#串口的参数  
PortNum=1  
StopBits=1  
ByteSize=8  
BaudRate=57600  
Parity=0  

[TCPService]#网口的参数  
IP=192.168.4.100  
Prot=8139  

[Common]  
ReadTimeOut=20  
WriteTimeOut=20  
#当与设备连接断开至下次重新连接的间隔时间 毫秒  
  
ReConnectWaitMillisecond=40 
``` 

2、自定义一个通讯任务结构体  
  结构体必须继承于Collector.ITaskContext  
  并按要求实现相关方法  
    
```
     public interface ITaskContext  
    {    
        ///一个任务的唯一标识。  
        string TaskName { get; set; }  
        /// 任务优先级  
        TaskPriority Priority { get; set; }  

        /// 是否为临时任务,如果是的话，该任务会再被在外界获取本任务的同时删除本任务  
        bool IsTempTask { get; set; }  

        /// 是否通讯成功，指示最近一次的通信是否成功执行  
        bool IsSuccess { get; set; }  

        /// 是否只通信一次、如果通信成功IsSuccess=true 则后续不再进行通讯；  
        bool ExecuteOnce { get; set; }  

        /// 获取要发送的byte[] //原始报文  
        byte[] GetTX();  
 
        /// 接收后内部赋值   //原始报文  
        void SetRX(byte[] rx);  
    }
```
    
    
3、创建一个工作实例。  

//需要传递一个通讯管道参数  
//可以是串口的管道和网口套接字管道  
```Collector.Task<MyTaskContext> task=new Collector.Task<MyTaskContext>(new Collector.Channel.TcpChannel());  ```
   
   
 //定义用于处理通讯错误的委托方法    
 // 这个事件会传递出两个参数：异常信息和连续出现了几次异常    
 ``` task.ExceptionEvent += HandleError;   ```
   
    
    
 //让任务跑起来     
``` task.Run();  ```
 
 
添加任务：  
1、创建你的TaskContext；  
    
  ······创建一个读取数据的任务······    
  ```
  TestContext t = new TestContext();    
  //唯一标识    
  t.TaskName = TaskName;  
  //tx为已经打包好的原始报文    
  t.TX = tx;  
  //添加  
  task.AddOrUpdateTaskToQueue(t);    
  //获取RX数据   
  TestContext t = task.GetTask(s => { return s.TaskName.Equals("taskName"); });    
  ```
  
   ······或者创建一个发送控制指令的任务······ 
   ```
    TestContext t = new TestContext();  
    t.TaskName = tb_taskName.Text;  
    t.TX = a;  
    //只发送一次 如果不为ture 这个任务会被循环执行  
    t.ExecuteOnce = true;  
    //即刻发送  设置优先级使这个任务立即被执行一次，然后不管会不会成功 这个任务都会被降级为普通任务  
    t.Priority = TaskPriority.High;  
    t.IsTempTask = true;//临时任务 在调用查找该任务的同时会把该任务删除  ！：在查找方法返回结果值后、这个被查找的任务就被删除掉了  
    task.AddOrUpdateTaskToQueue(t);  
    ```
     t.Priority = TaskPriority.High;对用需要
  
  
  
  
  
  
  




  
  
  
  


