using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace CL_MyLogZip
{
    /// <summary>
    /// 供外部调用
    /// </summary>
    public class MyLogZip
    {
        private static bool m_IsRun = false;
        private static DateTime m_LastDoTime = DateTime.FromOADate(0);
        private static int m_Hour = 1;
        private static int m_minite = 0;
        private static int m_SleepSecond = 30;

        /// <summary>
        /// 除了MyLog外的待压缩文件夹的列表。在执行完MyLog后，遍历列表，对目录下的文件夹分别打包zip，并删除文件夹。（最好是绝对路径。）比如：/DB/
        /// </summary>
        public static List<string> m_lstZipDir = new List<string>();

        /// <summary>
        /// 设置触发时间点、while循环间隔。
        /// </summary>
        /// <param name="_Hour">几点。取值[0-23]</param>
        /// <param name="_Minite">几分。取值[0-59]</param>
        /// <param name="_SleepSecond">while循环间隔秒。</param>
        public static void M_SetTime(int _Hour, int _Minite, int _SleepSecond)
        {
            m_Hour = _Hour;
            m_minite = _Minite;
            m_SleepSecond = _SleepSecond;
        }

        /// <summary>
        /// 单例线程
        /// </summary>
        [JsonIgnore]
        public static Thread m_Thread0 = null;

        /// <summary>
        /// 启动线程
        /// </summary>
        public static void M_Start()
        {
            try
            {
                Logger.Debug(string.Format("M_Start()"));
                if (m_Thread0 == null)
                {
                    m_IsRun = true;
                    m_Thread0 = new Thread(new ThreadStart(M_bgw_View_DoWork));
                    m_Thread0.IsBackground = true;
                    m_Thread0.Priority = ThreadPriority.Lowest;
                    m_Thread0.Start();
                }

                Logger.Debug(string.Format("M_Start()完成。"));
            }
            catch (Exception ex)
            {
                Logger.Debug(string.Format("M_Start()异常：{0}", ex.Message.ToString()));
            }
        }

        /// <summary>
        /// 停止线程
        /// </summary>
        public static void M_Stop()
        {
            try
            {
                Logger.Debug(string.Format("M_Stop()"));
                m_IsRun = false;
                m_Thread0.Abort();
                Logger.Debug(string.Format("M_Stop()完成。"));
            }
            catch (Exception ex)
            {
                Logger.Debug(string.Format("M_Stop()异常：{0}", ex.Message.ToString()));
            }
        }

        /// <summary>
        /// 被线程调用，执行while循环。
        /// </summary>
        private static void M_bgw_View_DoWork()
        {
            while (m_IsRun)
            {
                try
                {
                    DateTime _dtNow0 = DateTime.Now;
                    if (_dtNow0.Hour == m_Hour && _dtNow0.Minute == m_minite)
                    {
                        m_LastDoTime = _dtNow0;
                        M_DoOnce();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Debug(string.Format("M_bgw_View_DoWork()异常：{0}", ex.Message.ToString()));
                }
                Thread.Sleep(1000 * m_SleepSecond);
            }
        }

        /// <summary>
        /// 处理一次
        /// </summary>
        private static void M_DoOnce()
        {
            try
            {
                Logger.Debug(string.Format("M_Do()"));

                {
                    Logger.Debug(string.Format("开始处理MyLog文件夹。"));
                    string _sPath0 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MyLog");
                    if (Directory.Exists(_sPath0))
                    {
                        M_CleanUp(_sPath0);
                        M_Zip(_sPath0);
                    }
                    else
                    {
                        Logger.Debug(string.Format("M_Do()MyLog路径不存在，系统忽略。_sPath0={0}", _sPath0));
                    }
                }

                foreach (string _sPath00 in m_lstZipDir)
                {
                    Logger.Debug(string.Format("开始处理m_lstZipDir文件夹:{0}。", _sPath00));
                    string _sPath0 = Path.GetFullPath(_sPath00);
                    if (Directory.Exists(_sPath0))
                    {
                        M_Zip(_sPath0);
                    }
                    else
                    {
                        Logger.Debug(string.Format("开始处理m_lstZipDir文件夹时异常，系统继续。_sPath00={0}。", _sPath00));
                    }
                }

                //StaticCode.FileHelper.SetCfgValue("G_Do_FinishedDate", DateTime.Now.ToString());
                Logger.Debug(string.Format("M_Do()完成。"));
            }
            catch (Exception ex)
            {
                Logger.Debug(string.Format("M_Do()异常：{0}", ex.Message.ToString()));
            }
        }

        /// <summary>
        /// 整理MyLog文件夹：根据文件时间合并成多个以日期命名的文件夹
        /// </summary>
        /// <param name="_sPath">输入MyLog文件夹的路径。</param>
        private static void M_CleanUp(string _sPath)
        {
            try
            {
                Logger.Debug(string.Format("M_CleanUp(),_sPath={0}", _sPath));
                /*逻辑：
                今天的log不动，搞昨天及以前的。
                1。根据开始结束时间，找到所有相关的文件。按时间排序。
                log_all.txt
                log_all.txt.2018-07-31
                log_all.txt.2018-07-31.1
                */

                //string _sTmpPath = "leanup_Tmp";
                //if (!Directory.Exists(_sTmpPath)) Directory.CreateDirectory(_sTmpPath);


                string[] _lstFullName = Directory.GetFiles(_sPath, "*.txt.*", SearchOption.TopDirectoryOnly);
                List<C_File0> _lstFile00 = new List<C_File0>();
                foreach (string _sFullName in _lstFullName)
                {
                    string _sFileName = Path.GetFileName(_sFullName);
                    string[] _lstTmp = _sFileName.Split('.');
                    if (_lstTmp.Length >= 3)
                    {
                        DateTime _dt0;
                        if (!DateTime.TryParse(_lstTmp[2], out _dt0)) continue;
                        string _sKey0 = _lstTmp[2];
                        C_File0 _file00 = new C_File0() { m_sDate = _lstTmp[2], m_sFileName = _sFileName, m_sFullName = _sFullName };
                        _lstFile00.Add(_file00);
                    }
                    Thread.Sleep(1000);
                }
                Logger.Debug(string.Format("M_CleanUp(),_lstFile00准备就绪。"));

                foreach (C_File0 _file0 in _lstFile00)
                {
                    string _sTargetPath = Path.Combine(_sPath, _file0.m_sDate);
                    if (!Directory.Exists(_sTargetPath)) Directory.CreateDirectory(_sTargetPath);

                    File.Copy(_file0.m_sFullName, Path.Combine(_sTargetPath, _file0.m_sFileName), true);
                    Logger.Debug(string.Format("M_CleanUp(),File.Copy()完成，_sPath={0}", _file0.m_sFullName));
                    Thread.Sleep(1000);
                    //File.Copy(_file0.m_sFullName, Path.Combine(_sTmpPath, _file0.m_sFileName), true);
                    File.Delete(_file0.m_sFullName);
                    Logger.Debug(string.Format("M_CleanUp(),File.Delete()完成，_sPath={0}", _file0.m_sFullName));
                    Thread.Sleep(1000);
                }
                Thread.Sleep(1000);

                Logger.Debug(string.Format("M_CleanUp(),_sPath={0}完成。。", _sPath));
            }
            catch (Exception ex)
            {
                Logger.Debug(string.Format("M_CleanUp(),_sPath={0} 异常：{1}", _sPath, ex.Message.ToString()));
            }
        }

        /// <summary>
        /// 整理MyLog文件夹：查找本目录下以日期命名的文件夹，分别打zip压缩包
        /// </summary>
        /// <param name="_sPath">输入MyLog文件夹的路径。</param>
        /// <param name="_sPrefix">待压缩文件夹的前缀。即Directory.GetDirectories()的过滤参数。比如：trend_*。默认"*"</param>
        public static void M_Zip(string _sPath, string _sPrefix = "*")
        {
            try
            {
                Logger.Debug(string.Format("M_Zip(),_sPath={0}", _sPath));

                string[] _lstDir0 = Directory.GetDirectories(_sPath, "*", SearchOption.TopDirectoryOnly);

                for (int i = 0; i < _lstDir0.Length; i++)
                {
                    try
                    {
                        //string _sDir0
                        if (_lstDir0[i].EndsWith(@"\")) _lstDir0[i] = _lstDir0[i].Substring(0, _lstDir0[i].Length - 1);

                        string _sShortName = Path.GetFileNameWithoutExtension(_lstDir0[i]);
                        if (DateTime.Now.Date.ToString("yyyyMMdd") == _sShortName)
                        {
                            Logger.Debug(string.Format("文件夹名称为今天【{0}】，系统忽略，继续。。", _sShortName));
                            continue;
                        }

                        string _sFullName_Zip = string.Format("{0}.zip", _lstDir0[i]);
                        string _sShortPath = _lstDir0[i].Substring(_lstDir0[i].LastIndexOf(@"\") + 1);
                        if (File.Exists(_sFullName_Zip))
                        {
                            string _sFullName_New = string.Format("{0}_{1}.zip", _lstDir0[i], DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                            Logger.Debug(string.Format("zip文件已存在【{0}】，则新生成zip文件名【{1}】。", _sFullName_Zip, _sFullName_New));
                            _sFullName_Zip = _sFullName_New;
                        }

                        if (!SharpZip.CompressFile(_lstDir0[i], _sFullName_Zip))
                            throw new Exception(string.Format("M_Zip()SharpZip.CompressFile()失败。"));
                        Logger.Debug(string.Format("M_Zip()创建zip完成。_lstDir0[i]={0}", _lstDir0[i]));
                        Thread.Sleep(1000);

                        Directory.Delete(_lstDir0[i], true);
                        Logger.Debug(string.Format("M_Zip()删除文件夹完成。_lstDir0[i]={0}", _lstDir0[i]));
                        Thread.Sleep(1000);
                    }
                    catch (Exception ex)
                    {
                        Logger.Debug(string.Format("M_Zip()处理文件夹异常。_lstDir0[i]={0}, 异常={1}", _lstDir0[i], ex.Message.ToString()));
                    }
                }
                Logger.Debug(string.Format("M_Zip(),_sPath={0}完成。。", _sPath));
            }
            catch (Exception ex)
            {
                Logger.Debug(string.Format("M_Zip(),_sPath={0} 异常：{1}", _sPath, ex.Message.ToString()));
            }
        }

        /// <summary>
        /// 打印输出
        /// </summary>
        /// <returns></returns>
        public static string ToShowString()
        {
            JObject _j0 = new JObject();
            _j0.Add("m_IsRun", m_IsRun);
            _j0.Add("m_LastDoTime", m_LastDoTime);
            _j0.Add("m_Hour", m_Hour);
            _j0.Add("m_minite", m_minite);
            _j0.Add("m_SleepSecond", m_SleepSecond);
            return JsonConvert.SerializeObject(_j0);
        }

        /// <summary>
        /// 内部文件类
        /// </summary>
        class C_File0
        {
            public string m_sDate = "";
            public string m_sFileName = "";
            public string m_sFullName = "";
        }
    }
}
