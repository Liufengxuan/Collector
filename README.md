这是一个适用于对各种设备实时采集数据和发送控制指令的高效率通讯组件(如对接各种控制器和PLC)
--

特点
--
1)通讯方式广泛：按照接口规范可以实现任意通讯方式（目前已有com口和TCP/ip的实现），通讯方式可以自行扩展也可以随时切换  
2)协议自定义：重写收发解析的实现即可对接各种通讯协议（目前只有MODBUS一种）  
3)通讯稳定: 完善的中断处理机制,能应对各种复杂环境  
4)调试方便:有统一的异常输出接口，方便自行处理可能出现的异常，延迟记录，原始报文监控等功能  
5)可多实例运行：方便扩展分布式的设备    

实际应用:
--
   设备芯片：STM32407VG   
   通讯协议：Modbus  
   实测串口在波特率为 19200时单条指令收发时间可以在11-15ms内  
   网线单条指令收发5-9毫秒内。  
   指令数在30条时 发送控制指令从鼠标点击到继电器触发依旧没有不跟手的感觉  



局限性
--
只适用于轮询的通讯方式。  


TestForm 即为Demo  详情看内容和代码注释
---


以下为旧版本内容！！！！！ 有空更新：
----



~~使用方法  
---
1、修改通讯参数  
   
```      
[SPService]#串口的参数  
PortNum=1  
StopBits=1  
ByteSize=8  
BaudRate=57600  
Parity=0  
ReadTimeOut=20  
WriteTimeOut=20  

[TCPService]#网口的参数  
IP=192.168.4.100  
Prot=8139  
ReadTimeOut=20  
WriteTimeOut=20  

[Common]  
 
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
  
  ·第一种 需要传递一个通讯管道参数  
   可以是串口的管道和网口套接字管道  
   ```  public Task(BaseChannel channel)```  
  
  
  
   ·第二种 需要传递一个通讯管道参数,一个从缓冲区读取的数据的委托和一个从写入缓冲区的委托方法    
  ``` public Task(BaseChannel channel,ReceiveAction receiveFunc,SendAction sendFunc)  ```  
  
      //如果你要自定义如何从缓冲区读取数据需要使用这个委托来实现：  
      /*比如串口通讯需要自定义协议，串口返回的数据是多条粘在一起的、你需要把他们区分开，  
        就可以调用channel.read来读取判断有没有到你协议//中的结束符。*/    
   ```public delegate byte[] ReceiveAction(T t, BaseChannel channel);  ```    
   ```public delegate int SendAction(T t, BaseChannel channel);   ```    

      
    
  让任务跑起来     
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
    
    
    
4、定义用于处理通讯错误的委托方法   
这个事件会传递出两个参数：异常信息和连续出现了几次异常 
``` task.ExceptionEvent += HandleError;  ```









项目应用问题：
1、串口占用cpu资源过高：如果自己实现串口读取的超时、需要把串口api的readtimeout参数由0改为1。否则由于调用过快会非常占用cpu
2、读取延迟过高：调试查看读取和接收委托方法

![Image text](https://github.com/Liufengxuan/MasterPCCollector/blob/master/%E9%A1%B9%E7%9B%AE%E5%BA%94%E7%94%A81.jpg)





  
  
  
  




  
  
  
  


