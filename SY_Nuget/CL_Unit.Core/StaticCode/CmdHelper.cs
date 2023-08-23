using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace StaticCode
{
    /// <summary>
    /// 用于启动某个exe程序。
    /// </summary>
    public static class CmdHelper
    { 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sProcessName">启动的exe的路径。如：C:\tmp\test1.exe; tmp\test1.exe </param>
        /// <param name="processWindowStyle">窗口状态</param>
        /// <param name="args">exe的启动参数（可为空）</param> 
        /// <returns></returns>
        public static bool Start(string sProcessName, ProcessWindowStyle processWindowStyle, params string[] args)
        {
            try
            {
                string _sName = "";
                string _sFullNameJdUpLoad = "";
                if (!sProcessName.EndsWith(".exe")) _sFullNameJdUpLoad = sProcessName + ".exe";
                else _sFullNameJdUpLoad = sProcessName;
                _sFullNameJdUpLoad = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _sFullNameJdUpLoad); 
                _sName = FileHelper.GetName(_sFullNameJdUpLoad);

                Process[] pTargets = Process.GetProcessesByName(_sName);
                Logger.WriteLog(string.Format("发现{0}个名为{1}的进程。准备kill掉。", pTargets.Length, _sName));
                foreach (Process _p in pTargets) _p.Kill();
                pTargets = Process.GetProcessesByName(FileHelper.GetShortName(_sName));
                Logger.WriteLog(string.Format("发现{0}个名为{1}的进程。准备kill掉。", pTargets.Length, FileHelper.GetShortName(_sName)));
                foreach (Process _p in pTargets) _p.Kill();
                if (!System.IO.File.Exists(_sFullNameJdUpLoad)) throw new Exception(string.Format("程序文件根目录下的{0} 未找到。请核对路径后重试！", sProcessName));

                Logger.WriteLog(string.Format("准备开启:{0}", _sFullNameJdUpLoad));
                ProcessStartInfo procInfo = new ProcessStartInfo();
                procInfo.FileName = _sFullNameJdUpLoad;
                if (null != args)
                    if (args.Length > 0)
                    {
                        procInfo.Arguments = args[0];
                        Logger.WriteLog(string.Format("传入参数:{0}", args[0]));
                    }
                procInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                procInfo.WindowStyle = processWindowStyle;
                Process proc = Process.Start(procInfo);
                Logger.WriteLog(string.Format("程序开启{0}.exe成功！", sProcessName));
                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(string.Format("程序开启{0}.exe失败。程序继续。异常：{1}", sProcessName, ex.Message.ToString()));
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sProcessName">启动的exe的路径。如：C:\tmp\test1.exe; tmp\test1.exe </param>
        /// <param name="args">exe的启动参数（可为空）</param>
        /// <returns></returns>
        public static bool Start(string sProcessName, params string[] args)
        {
            try
            {
                string _sName = "";
                string _sFullNameJdUpLoad = "";
                if (sProcessName.Contains(":"))
                {
                    _sName = FileHelper.GetShortName(sProcessName);
                    _sFullNameJdUpLoad = sProcessName;
                }
                else
                {
                    _sName = FileHelper.GetShortName(sProcessName);
                    _sFullNameJdUpLoad = string.Format("{0}\\{1}.exe", AppDomain.CurrentDomain.BaseDirectory, sProcessName);
                }
                Process[] pTargets = Process.GetProcessesByName(_sName);
                if (pTargets.Length <= 0)
                {
                    if (!System.IO.File.Exists(_sFullNameJdUpLoad)) throw new Exception(string.Format("程序文件根目录下的{0}.exe 未找到。请核对路径后重试！", sProcessName));
                    Logger.WriteLog(string.Format("准备开启:{0}", _sFullNameJdUpLoad));
                    ProcessStartInfo procInfo = new ProcessStartInfo();
                    procInfo.FileName = _sFullNameJdUpLoad;
                    if (null != args)
                        if (args.Length > 0)
                        {
                            procInfo.Arguments = args[0];
                            Logger.WriteLog(string.Format("传入参数:{0}", args[0]));
                        }
                    procInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    procInfo.WindowStyle = ProcessWindowStyle.Normal;
                    Process proc = Process.Start(procInfo);
                    Logger.WriteLog(string.Format("程序开启{0}.exe成功！", sProcessName));
                }
                else
                {
                    Logger.WriteLog(string.Format("程序开启{0}.exe时，发现已在运行。程序继续。", sProcessName));
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(string.Format("程序开启{0}.exe失败。程序继续。异常：{1}", sProcessName, ex.Message.ToString()));
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sProcessName">启动的exe的路径。如：C:\tmp\test1.exe; tmp\test1.exe </param>
        /// <param name="iCount">除自己外，允许exe同时存在的数量。若小于零，则不限。等于零，则只能自己运行。大于零，则表示除自己外的个数</param>
        /// <param name="args">exe的启动参数（可为空）</param>
        /// <returns></returns>
        public static bool Start(string sProcessName, int iCount, params string[] args)
        {
            string _sName = "";
            string _sFullNameJdUpLoad = "";
            try
            {
                if (sProcessName.Contains(":"))
                {
                    _sName = FileHelper.GetShortName(sProcessName);
                    _sFullNameJdUpLoad = sProcessName;
                }
                else
                {
                    _sFullNameJdUpLoad = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, sProcessName);
                }

                // 如果文件不存在，则查看是否有扩展名，如没有，则加上。exe, bat, vbs, com 都试试。
                if (!File.Exists(_sFullNameJdUpLoad) && string.IsNullOrEmpty(FileHelper.GetExName(_sFullNameJdUpLoad)))
                {
                    if (File.Exists(string.Format("{0}.exe", _sFullNameJdUpLoad))) _sFullNameJdUpLoad = string.Format("{0}.exe", _sFullNameJdUpLoad);
                    else if (File.Exists(string.Format("{0}.com", _sFullNameJdUpLoad))) _sFullNameJdUpLoad = string.Format("{0}.com", _sFullNameJdUpLoad);
                    else if (File.Exists(string.Format("{0}.bat", _sFullNameJdUpLoad))) _sFullNameJdUpLoad = string.Format("{0}.bat", _sFullNameJdUpLoad);
                    else if (File.Exists(string.Format("{0}.vbs", _sFullNameJdUpLoad))) _sFullNameJdUpLoad = string.Format("{0}.vbs", _sFullNameJdUpLoad);
                }

                // 如果文件仍然没找到，则异常。
                if (!File.Exists(_sFullNameJdUpLoad)) throw new Exception(string.Format("程序文件{0} 未找到。(包括exe,com,bat,vbs) 请核对路径后重试！", _sFullNameJdUpLoad));

                if (iCount >= 0)
                {
                    Process[] _pTargets = Process.GetProcessesByName(_sName);
                    Logger.WriteLog(string.Format("输入的参数iCount = {0}, 发现名字为{1}的进程共{2}个，需要Kill()掉{3}个", iCount, _sName, _pTargets.Count(), _pTargets.Count() - iCount));
                    for (int i = 0; i < _pTargets.Count() - iCount; i++) _pTargets[i].Kill();
                }

                Logger.WriteLog(string.Format("准备开启:{0}", _sFullNameJdUpLoad));
                ProcessStartInfo procInfo = new ProcessStartInfo();
                procInfo.FileName = _sFullNameJdUpLoad;
                if (null != args)
                    if (args.Length > 0)
                    {
                        procInfo.Arguments = args[0];
                        Logger.WriteLog(string.Format("传入参数:{0}", args[0]));
                    }
                procInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                procInfo.WindowStyle = ProcessWindowStyle.Maximized;
                Process proc = Process.Start(procInfo);
                Logger.WriteLog(string.Format("程序开启{0} 成功！", _sFullNameJdUpLoad));

                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(string.Format("程序开启{0}失败。程序继续。异常：{1}", _sFullNameJdUpLoad, ex.Message.ToString()));
                return false;
            }
        }
    }
}
