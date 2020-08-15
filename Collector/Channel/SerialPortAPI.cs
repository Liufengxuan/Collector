using System;
using System.Collections.Generic;

using System.Runtime.InteropServices;
using System.Text;

namespace Collector.Channel
{
    internal class SerialPortAPI
    {
        #region WINAPI常量
        /// <summary>
        /// 写标志
        /// </summary>
        private const uint GENERIC_READ = 0x80000000;
        /// <summary>
        /// 读标志
        /// </summary>
        private const uint GENERIC_WRITE = 0x40000000;
        /// <summary>
        /// 打开已存在
        /// </summary>
        private const int OPEN_EXISTING = 3;
        /// <summary>
        /// 无效句柄
        /// </summary>
        private const int INVALID_HANDLE_VALUE = -1;
        private const int PURGE_RXABORT = 0x2;
        private const int PURGE_RXCLEAR = 0x8;
        private const int PURGE_TXABORT = 0x1;
        private const int PURGE_TXCLEAR = 0x4;

        private const int ERROR_IO_PENDING = 0x3e5;
        private const int ERROR_IO_INCOMPLETE = 0x3e4;
        #endregion


        #region 成员变量
        /// <summary>
        /// 端口名称(COM1,COM2...COM4...)
        /// </summary>
        public int PortNum;
        /// <summary>
        /// 波特率9600
        /// </summary>
        public int BaudRate;
        /// <summary>
        /// 数据位4-8
        /// </summary>
        public byte ByteSize;
        /// <summary>
        /// 奇偶校验0-4=no,odd,even,mark,space
        /// </summary>
        public byte Parity;
        /// <summary>
        /// 停止位
        /// </summary>
        public byte StopBits; // 0,1,2 = 1, 1.5, 2
                              /// <summary>
                              /// 超时长
                              /// </summary>
        public int ReadTimeout;

        public int WriteTimeout;
        /// <summary>
        /// COM口句柄
        /// </summary>
        private int hComm = INVALID_HANDLE_VALUE;
        /// <summary>
        /// 串口是否已经打开
        /// </summary>
        public bool Opened = false;
        #endregion
        #region DCB rcd
        [StructLayout(LayoutKind.Sequential)]
        public struct DCB
        {
            public int DCBlength;       // DCB长度
            public int BaudRate;        //指定当前波特率
            public uint flags;          //标志位
            public ushort wReserved;    //未使用,必须为0    
            public ushort XonLim;       //指定在XON字符发送这前接收缓冲区中可允许的最小字节数
            public ushort XoffLim;      //指定在XOFF字符发送这前接收缓冲区中可允许的最小字节数
            public byte ByteSize;       //指定端口当前使用的数据位
            public byte Parity;         //指定端口当前使用的奇偶校验方法,可能为: 0-4=no,odd,even,mark,space
            public byte StopBits;       //指定端口当前使用的停止位数,可能为:0,1,2 = 1, 1.5, 2
            public byte XonChar;        //指定用于发送和接收字符XON的值 Tx and Rx XON character
            public byte XoffChar;       //指定用于发送和接收字符XOFF值 Tx and Rx XOFF character
            public byte ErrorChar;      //本字符用来代替接收到的奇偶校验发生错误时的值
            public byte EofChar;        //当没有使用二进制模式时,本字符可用来指示数据的结束
            public byte EvtChar;        //当接收到此字符时,会产生一个事件
            public ushort wReserved1;   //未使用，为0
        }
        #endregion
        /// <summary>
        /// 设备控制块结构体类型
        /// </summary>
        //[StructLayout(LayoutKind.Sequential)]
        //public struct DCB
        //{
        //    /// <summary>
        //    /// DCB长度
        //    /// </summary>
        //    public int DCBlength;
        //    /// <summary>
        //    /// 指定当前波特率
        //    /// </summary>
        //    public int BaudRate;
        //    /// <summary>
        //    /// 指定是否允许二进制模式
        //    /// </summary>
        //    public int fBinary;
        //    /// <summary>
        //    /// 指定是否允许奇偶校验
        //    /// </summary>
        //    public int fParity;
        //    /// <summary>
        //    /// 指定CTS是否用于检测发送控制，当为TRUE是CTS为OFF，发送将被挂起。
        //    /// </summary>
        //    public int fOutxCtsFlow;
        //    /// <summary>
        //    /// 指定CTS是否用于检测发送控制
        //    /// </summary>
        //    public int fOutxDsrFlow;
        //    /// <summary>
        //    /// DTR_CONTROL_DISABLE值将DTR置为OFF, DTR_CONTROL_ENABLE值将DTR置为ON, DTR_CONTROL_HANDSHAKE允许DTR"握手"
        //    /// </summary>
        //    public int fDtrControl;
        //    /// <summary>
        //    /// 当该值为TRUE时DSR为OFF时接收的字节被忽略
        //    /// </summary>
        //    public int fDsrSensitivity;
        //    /// <summary>
        //    /// 指定当接收缓冲区已满,并且驱动程序已经发送出XoffChar字符时发送是否停止。
        //    /// TRUE时，在接收缓冲区接收到缓冲区已满的字节XoffLim且驱动程序已经发送出
        //    /// XoffChar字符中止接收字节之后，发送继续进行。　FALSE时，在接收缓冲区接
        //    /// 收到代表缓冲区已空的字节XonChar且驱动程序已经发送出恢复发送的XonChar之
        //    /// 后，发送继续进行。XOFF continues Tx
        //    /// </summary>
        //    public int fTXContinueOnXoff;
        //    /// <summary>
        //    /// TRUE时，接收到XoffChar之后便停止发送接收到XonChar之后将重新开始 XON/XOFF
        //    /// out flow control
        //    /// </summary>
        //    public int fOutX;
        //    /// <summary>
        //    /// TRUE时，接收缓冲区接收到代表缓冲区满的XoffLim之后，XoffChar发送出去接收
        //    /// 缓冲区接收到代表缓冲区空的XonLim之后，XonChar发送出去 XON/XOFF in flow control
        //    /// </summary>
        //    public int fInX;
        //    /// <summary>
        //    /// 该值为TRUE且fParity为TRUE时，用ErrorChar 成员指定的字符代替奇偶校验错误
        //    /// 的接收字符 enable error replacement
        //    /// </summary>
        //    public int fErrorChar;
        //    /// <summary>
        //    /// eTRUE时，接收时去掉空（0值）字节 enable null stripping
        //    /// </summary>
        //    public int fNull;
        //    /// <summary>
        //    /// RTS flow control RTS_CONTROL_DISABLE时,RTS置为OFF RTS_CONTROL_ENABLE时, RTS置为ON
        //    /// RTS_CONTROL_HANDSHAKE时,当接收缓冲区小于半满时RTS为ON当接收缓冲区超过四分之
        //    /// 三满时RTS为OFF RTS_CONTROL_TOGGLE时,当接收缓冲区仍有剩余字节时RTS为ON ,否则
        //    /// 缺省为OFF
        //    /// </summary>
        //    public int fRtsControl;
        //    /// <summary>
        //    /// TRUE时,有错误发生时中止读和写操作 abort on error
        //    /// </summary>
        //    public int fAbortOnError;
        //    /// <summary>
        //    /// 未使用
        //    /// </summary>
        //    public int fDummy2;
        //    /// <summary>
        //    /// 标志位
        //    /// </summary>
        //    public uint flags;
        //    /// <summary>
        //    /// 未使用,必须为0
        //    /// </summary>
        //    public ushort wReserved;
        //    /// <summary>
        //    /// 指定在XON字符发送这前接收缓冲区中可允许的最小字节数
        //    /// </summary>
        //    public ushort XonLim;
        //    /// <summary>
        //    /// 指定在XOFF字符发送这前接收缓冲区中可允许的最小字节数
        //    /// </summary>
        //    public ushort XoffLim;
        //    /// <summary>
        //    /// 指定端口当前使用的数据位
        //    /// </summary>
        //    public byte ByteSize;
        //    /// <summary>
        //    /// 指定端口当前使用的奇偶校验方法,可能为:EVENPARITY,MARKPARITY,NOPARITY,ODDPARITY 0-4=no,odd,even,mark,space
        //    /// </summary>
        //    public byte Parity;
        //    /// <summary>
        //    /// 指定端口当前使用的停止位数,可能为:ONESTOPBIT,ONE5STOPBITS,TWOSTOPBITS 0,1,2 = 1, 1.5, 2
        //    /// </summary>
        //    public byte StopBits;
        //    /// <summary>
        //    /// 指定用于发送和接收字符XON的值 Tx and Rx XON character
        //    /// </summary>
        //    public char XonChar;
        //    /// <summary>
        //    /// 指定用于发送和接收字符XOFF值 Tx and Rx XOFF character
        //    /// </summary>
        //    public char XoffChar;
        //    /// <summary>
        //    /// 本字符用来代替接收到的奇偶校验发生错误时的值
        //    /// </summary>
        //    public char ErrorChar;
        //    /// <summary>
        //    /// 当没有使用二进制模式时,本字符可用来指示数据的结束
        //    /// </summary>
        //    public char EofChar;
        //    /// <summary>
        //    /// 当接收到此字符时,会产生一个事件
        //    /// </summary>
        //    public char EvtChar;
        //    /// <summary>
        //    /// 未使用
        //    /// </summary>
        //    public ushort wReserved1;
        //}
        /// <summary>
        /// 串口超时时间结构体类型
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct COMMTIMEOUTS
        {
            public UInt32 ReadIntervalTimeout;
            public int ReadTotalTimeoutMultiplier;
            public int ReadTotalTimeoutConstant;
            public int WriteTotalTimeoutMultiplier;
            public int WriteTotalTimeoutConstant;
        }
        /// <summary>
        /// 湓出缓冲区结构体类型
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct OVERLAPPED
        {
            public int Internal;
            public int InternalHigh;
            public int Offset;
            public int OffsetHigh;
            public int hEvent;
        }
        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="lpFileName">要打开的串口名称</param>
        /// <param name="dwDesiredAccess">指定串口的访问方式，一般设置为可读可写方式</param>
        /// <param name="dwShareMode">指定串口的共享模式，串口不能共享，所以设置为0</param>
        /// <param name="lpSecurityAttributes">设置串口的安全属性，WIN9X下不支持，应设为NULL</param>
        /// <param name="dwCreationDisposition">对于串口通信，创建方式只能为OPEN_EXISTING</param>
        /// <param name="dwFlagsAndAttributes">指定串口属性与标志，设置为FILE_FLAG_OVERLAPPED(重叠I/O操作)，指定串口以异步方式通信</param>
        /// <param name="hTemplateFile">对于串口通信必须设置为NULL</param>
        [DllImport("kernel32.dll")]
        private static extern int CreateFile(string lpFileName, uint dwDesiredAccess, int dwShareMode,
        int lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, int hTemplateFile);
        /// <summary>
        /// 得到串口状态
        /// </summary>
        /// <param name="hFile">通信设备句柄</param>
        /// <param name="lpDCB">设备控制块DCB</param>
        [DllImport("kernel32.dll")]
        private static extern bool GetCommState(int hFile, ref DCB lpDCB);


        /// <summary>
        /// 建立串口设备控制块
        /// </summary>
        /// <param name="lpDef">设备控制字符串</param>
        /// <param name="lpDCB">设备控制块</param>
        [DllImport("kernel32.dll")]
        private static extern bool BuildCommDCB(string lpDef, ref DCB lpDCB);


        /// <summary>
        /// 设置串口状态
        /// </summary>
        /// <param name="hFile">通信设备句柄</param>
        /// <param name="lpDCB">设备控制块</param>
        [DllImport("kernel32.dll")]
        private static extern bool SetCommState(int hFile, ref DCB lpDCB);
        /// <summary>
        /// 读取串口超时时间
        /// </summary>
        /// <param name="hFile">通信设备句柄</param>
        /// <param name="lpCommTimeouts">超时时间</param>
        [DllImport("kernel32.dll")]
        private static extern bool GetCommTimeouts(int hFile, ref COMMTIMEOUTS lpCommTimeouts);
        /// <summary>
        /// 设置串口超时时间
        /// </summary>
        /// <param name="hFile">通信设备句柄</param>
        /// <param name="lpCommTimeouts">超时时间</param>
        [DllImport("kernel32.dll")]
        private static extern bool SetCommTimeouts(int hFile, ref COMMTIMEOUTS lpCommTimeouts);
        /// <summary>
        /// 读取串口数据
        /// </summary>
        /// <param name="hFile">通信设备句柄</param>
        /// <param name="lpBuffer">数据缓冲区</param>
        /// <param name="nNumberOfBytesToRead">多少字节等待读取</param>
        /// <param name="lpNumberOfBytesRead">读取多少字节</param>
        /// <param name="lpOverlapped">溢出缓冲区</param>
        [DllImport("kernel32.dll")]
        private static extern bool ReadFile(int hFile, byte[] lpBuffer,
        int nNumberOfBytesToRead, ref int lpNumberOfBytesRead, ref OVERLAPPED lpOverlapped);
        /// <summary>
        /// 写串口数据
        /// </summary>
        /// <param name="hFile">通信设备句柄</param>
        /// <param name="lpBuffer">数据缓冲区</param>
        /// <param name="nNumberOfBytesToWrite">多少字节等待写入</param>
        /// <param name="lpNumberOfBytesWritten">已经写入多少字节</param>
        /// <param name="lpOverlapped">溢出缓冲区</param>
        [DllImport("kernel32.dll")]
        private static extern bool WriteFile(int hFile, byte[] lpBuffer,
        int nNumberOfBytesToWrite, ref int lpNumberOfBytesWritten, ref OVERLAPPED lpOverlapped);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FlushFileBuffers(int hFile);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool PurgeComm(int hFile, uint dwFlags);
        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <param name="hObject">通信设备句柄</param>
        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(int hObject);
        /// <summary>
        /// 得到串口最后一次返回的错误
        /// </summary>
        [DllImport("kernel32.dll")]
        private static extern uint GetLastError();



        #region 设置DCB标志位函数
        internal void SetDcbFlag(int whichFlag, int setting, DCB dcb)
        {
            uint num = 1;
            setting = setting << whichFlag;
            if ((whichFlag == 4) || (whichFlag == 12))
            {
                num = 3;
            }
            else if (whichFlag == 15)
            {
                num = 0x1ffff;
            }
            else
            {
                num = 1;
            }
            dcb.flags &= ~(num << whichFlag);
            dcb.flags |= (uint)setting;
        }
        #endregion
        /// <summary>
        /// 建立与串口的连接
        /// </summary>
        public bool Open()
        {
          
            COMMTIMEOUTS ctoCommPort = new COMMTIMEOUTS();
            // 打开串口
            hComm = CreateFile("\\\\.\\COM" + PortNum, GENERIC_READ | GENERIC_WRITE,
             0, 0, OPEN_EXISTING, 0, 0);
            if (hComm == INVALID_HANDLE_VALUE)
            {
                return false;
            }
            // 设置通信超时时间
            GetCommTimeouts(hComm, ref ctoCommPort);
            ctoCommPort.ReadIntervalTimeout = UInt32.MaxValue;
            ctoCommPort.ReadTotalTimeoutConstant = ReadTimeout;
            ctoCommPort.ReadTotalTimeoutMultiplier = 0;
            ctoCommPort.WriteTotalTimeoutMultiplier =0;
            ctoCommPort.WriteTotalTimeoutConstant = WriteTimeout;
            SetCommTimeouts(hComm, ref ctoCommPort);
            // 设置串口
            DCB dcb = new DCB();
            GetCommState(hComm, ref dcb);
         
            dcb.DCBlength = Marshal.SizeOf(dcb);
         

            dcb.BaudRate = BaudRate;
            dcb.flags = 0;
            //////////////////////////////////sr
            dcb.flags |= 1;
            if (Parity > 0)
            {
                dcb.flags |= 2;
            }
            //////////////////////////////////////
            dcb.Parity = Parity;
            dcb.ByteSize = ByteSize;
            dcb.StopBits = StopBits;
            //------------------------------
            SetDcbFlag(0, 1, dcb);            //二进制方式
            SetDcbFlag(1, (Parity == 0) ? 0 : 1, dcb);
            SetDcbFlag(2, 0, dcb);            //不用CTS检测发送流控制
            SetDcbFlag(3, 0, dcb);            //不用DSR检测发送流控制
            SetDcbFlag(4, 0, dcb);            //禁止DTR流量控制
            SetDcbFlag(6, 0, dcb);            //对DTR信号线不敏感
            SetDcbFlag(9, 1, dcb);            //检测接收缓冲区
            SetDcbFlag(8, 0, dcb);            //不做发送字符控制
            SetDcbFlag(10, 0, dcb);           //是否用指定字符替换校验错的字符
            SetDcbFlag(11, 0, dcb);           //保留NULL字符
            SetDcbFlag(12, 0, dcb);           //允许RTS流量控制
            SetDcbFlag(14, 0, dcb);           //发送错误后，继续进行下面的读写操作
            //--------------------------------
            dcb.wReserved = 0;                //没有使用，必须为0      
            dcb.XonLim = 0;                   //指定在XOFF字符发送之前接收到缓冲区中可允许的最小字节数
            dcb.XoffLim = 0;                  //指定在XOFF字符发送之前缓冲区中可允许的最小可用字节数
            dcb.XonChar = 0;                  //发送和接收的XON字符
            dcb.XoffChar = 0;                 //发送和接收的XOFF字符
            dcb.ErrorChar = 0;                //代替接收到奇偶校验错误的字符
            dcb.EofChar = 0;                  //用来表示数据的结束     
            dcb.EvtChar = 0;                  //事件字符，接收到此字符时，会产生一个事件       
            dcb.wReserved1 = 0;               //没有使用
            #region 防止第一次通讯时不能进行通讯
            #region 校验字符串 //0-4=no,odd,even,mark,space
            string L_Parity = "n";
            switch (Parity)
            {
                case 0:
                    L_Parity = "n";
                    break;
                case 1:
                    L_Parity = "o";
                    break;
                case 2:
                    L_Parity = "e";
                    break;
                case 3:
                    L_Parity = "m";
                    break;
                case 4:
                    L_Parity = "s";
                    break;
                default: break;
            }
            #endregion

            #region 停止位 //StopBits; // 0,1,2 = 1, 1.5, 2
            string L_Stopbit = StopBits.ToString();
            //switch (StopBits)
            //{
            //    case 0:
            //        L_Stopbit = "1";
            //        break;
            //    case 1:
            //        L_Stopbit = "1.5";
            //        break;
            //    case 2:
            //        L_Stopbit = "2";
            //        break;
            //    default: break;
            //}
            #endregion

            string L_Set = BaudRate.ToString() + "," + L_Parity + "," + ByteSize.ToString() + "," + L_Stopbit;

            if (!BuildCommDCB(L_Set, ref dcb))
            {
                return false;
            }
            #endregion

            if (!SetCommState(hComm, ref dcb))
            {
                return false;
            }
            Opened = true;
            return true;
        }
        /// <summary>
        /// 关闭串口,结束通讯
        /// </summary>
        public bool Close()
        {
            if (hComm != INVALID_HANDLE_VALUE)
            {
                CloseHandle(hComm);
                Opened = false;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 读取串口返回的数据
        /// </summary>
        /// <param name="NumBytes">数据长度</param>
        public byte[] Read(int NumBytes)
        {
            byte[] BufBytes;
            byte[] OutBytes;
            BufBytes = new byte[NumBytes];
            if (hComm != INVALID_HANDLE_VALUE)
            {
                OVERLAPPED ovlCommPort = new OVERLAPPED();
                int BytesRead = 0;

                ReadFile(hComm, BufBytes, NumBytes, ref BytesRead, ref ovlCommPort);
                OutBytes = new byte[BytesRead];
                Array.Copy(BufBytes, OutBytes, BytesRead);
                return OutBytes;
            }
            else
            {
                return new byte[0];
            }
        }
        #region 清除接收缓冲区
        public void ClearReceiveBuf()
        {
            try
            {
                if (hComm != INVALID_HANDLE_VALUE)
                {
                    PurgeComm(hComm, PURGE_RXABORT | PURGE_RXCLEAR);
                }
            }
            catch { }
        }
        #endregion
        /// <summary>
        /// 清空COM口缓冲区数据
        /// </summary>
        /// <returns></returns>
        public bool ClearSendBuf()
        {
            if (hComm != INVALID_HANDLE_VALUE)
            {
                PurgeComm(hComm, PURGE_TXABORT | PURGE_TXCLEAR);
            }
            return false;
        }
        /// <summary>
        /// 向串口写数据
        /// </summary>
        /// <param name="WriteBytes">数据数组</param>
        public int Write(byte[] WriteBytes)
        {
            bool result;
            if (hComm != INVALID_HANDLE_VALUE)
            {
                OVERLAPPED ovlCommPort = new OVERLAPPED();
                int BytesWritten = 0;
                WriteFile(hComm, WriteBytes, WriteBytes.Length,
                 ref BytesWritten, ref ovlCommPort);
                return BytesWritten;
            }
            else
            {
                return 0;
            }
        }
    }
}
