using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace StaticCode
{
    /// <summary>
    /// 背景线程类
    /// </summary>
    public class C_BackThread : IDisposable
    {
        /// <summary>
        /// 线程
        /// </summary>
        public Thread m_Thread;

        /// <summary>
        /// //0:设备线程。1:管段计算线程。10:ModbusTag线程。11:格式化TcpData的线程。12:TcpData入库线程。13:TcpListen线程。14:TcpListen子线程。15:Clear子线程。16:报警Sound子线程。20:Test子线程。
        /// </summary>
        public E_ThreadType m_iType = E_ThreadType.a0设备线程;

        /// <summary>
        /// 0:未启动  -1:收到消息，希望停止线程。 
        /// </summary>
        public E_ThreadState m_iState = E_ThreadState.a0未启动;

        /// <summary>
        /// 针对m_iType=0,1,10的主键ID。其他情况下默认=0
        /// </summary>
        public int m_iResID = 0;

        public void Dispose()
        {
            try
            {
                if (m_Thread != null)
                {
                    m_Thread.Abort();
                }
            }
            catch (Exception ex)
            {
                Logger.Debug(string.Format("C_BackThread.Dispose()Error: {0}", ex.Message.ToString()));
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public C_BackThread()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_p0"></param>
        /// <param name="_sName">线程名称</param>
        /// <param name="_type"></param>
        public C_BackThread(ParameterizedThreadStart _p0, string _sName, E_ThreadType _type)
        {
            m_Thread = new Thread(_p0);
            m_Thread.IsBackground = true;
            m_Thread.Name = _sName;
            m_iType = _type;
        }
    }

    /// <summary>
    /// 线程类型
    /// </summary>
    public enum E_ThreadType
    {
        /// <summary>
        /// DTU, ModbusTcp 等 读取线程。
        /// </summary>
        a0设备线程 = 0,
        a1管段计算线程 = 1,
        a10ModbusTag线程 = 10,
        a11格式化TcpData的线程 = 11,
        a12TcpData入库线程 = 12,
        a13TcpListen线程 = 13,
        a14TcpListen子线程 = 14,
        a15Clear子线程 = 15,
        a16报警Sound子线程 = 16,
        a200Debug线程 = 200,
        a201TcpTest线程 = 201,
        a202TcpTest子线程 = 202,
        a203AlarmTest子线程 = 203,
    }

    /// <summary>
    /// 线程是否运行
    /// </summary>
    public enum E_ThreadState
    {
        a0未启动 = 0,
        a1收到消息希望停止线程 = 1
    }
}
