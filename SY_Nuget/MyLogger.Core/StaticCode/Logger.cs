using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;

using log4net;
using log4net.Config;
using log4net.Appender;


[assembly: XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
/// <summary>
/// 日志管理类
/// </summary>
//[assembly: log4net.Config.XmlConfigurator(Watch = true)]
public static class Logger
{
    /// <summary>
    /// Debug委托
    /// </summary>
    /// <param name="message">日志信息</param>
    public delegate void DDebug(object message);

    /// <summary>
    /// Info委托
    /// </summary>
    /// <param name="message">日志信息</param>
    public delegate void DInfo(object message);

    /// <summary>
    /// Warn委托
    /// </summary>
    /// <param name="message">日志信息</param>
    public delegate void DWarn(object message);

    /// <summary>
    /// Error委托
    /// </summary>
    /// <param name="message">日志信息</param>
    public delegate void DError(object message);

    /// <summary>
    /// Fatal委托
    /// </summary>
    /// <param name="message">日志信息</param>
    public delegate void DFatal(object message);

    /// <summary>
    /// WriteLog
    /// </summary>
    public static DInfo WriteLog
    {
        //get { return LogManager.GetLogger("Form2").Debug; }
        get { return LogManager.GetLogger((new StackTrace()).GetFrame(1).GetMethod().DeclaringType).Debug; }
    }

    /// <summary>
    /// 获取格式化Debug
    /// </summary>
    public static DDebug Debug
    {
        get { return LogManager.GetLogger((new StackTrace()).GetFrame(1).GetMethod().DeclaringType).Debug; }
    }

    /// <summary>
    /// Info
    /// </summary>
    public static DInfo Info
    {
        get { return LogManager.GetLogger((new StackTrace()).GetFrame(1).GetMethod().DeclaringType).Info; }
    }

    /// <summary>
    /// Warn
    /// </summary>
    public static DWarn Warn
    {
        get { return LogManager.GetLogger((new StackTrace()).GetFrame(1).GetMethod().DeclaringType).Warn; }
    }

    /// <summary>
    /// Error
    /// </summary>
    public static DError Error
    {
        get { return LogManager.GetLogger((new StackTrace()).GetFrame(1).GetMethod().DeclaringType).Error; }
    }

    /// <summary>
    /// Fatal
    /// </summary>
    public static DFatal Fatal
    {
        get { return LogManager.GetLogger((new StackTrace()).GetFrame(1).GetMethod().DeclaringType).Fatal; }
    }

    /// <summary>
    /// 静态构造函数。初始化日志器。
    /// </summary>
    static Logger()
    {
        //if (AppDomain.CurrentDomain.SetupInformation.ConfigurationFile.ToLower().EndsWith("web.config"))
        //{
        //    return;
        //}
        //FileAppender appender0 = new FileAppender();
        //appender0.Name = "root";
        //appender0.File = AppDomain.CurrentDomain.FriendlyName + "_log.txt";
        //appender0.AppendToFile = true;

        //log4net.Layout.PatternLayout layout0 = new log4net.Layout.PatternLayout("%d{yyyy-MM-dd HH:mm:ss,fff}[%t] %-5p [%c] - %m%n");
        //appender0.Layout = layout0;

        ////log4net.Filter.LoggerMatchFilter _filter = new log4net.Filter.LoggerMatchFilter();
        ////_filter.LoggerToMatch = "CS_Test00.Program";
        ////_filter.AcceptOnMatch = false;
        ////appender0.AddFilter(_filter);

        //BasicConfigurator.Configure(appender0);
        //appender0.ActivateOptions();
        //return;


        string path = string.Format("log4net.config");
        if (File.Exists(path))
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(path));
        }
        else
        {
            M_FileAppender();
        }
    }

    /// <summary>
    /// 单个文件追加方式
    /// </summary>
    private static void M_FileAppender()
    {
        FileAppender appender0 = new FileAppender();
        appender0.Name = "root";
        appender0.File = AppDomain.CurrentDomain.FriendlyName + "_log.txt";
        appender0.AppendToFile = true;

        log4net.Layout.PatternLayout layout0 = new log4net.Layout.PatternLayout("%d{yyyy-MM-dd HH:mm:ss,fff}[%t] %-5p [%c] - %m%n");
        appender0.Layout = layout0;

        //log4net.Filter.LoggerMatchFilter _filter = new log4net.Filter.LoggerMatchFilter();
        //_filter.LoggerToMatch = "CS_Test00.Program";
        //_filter.AcceptOnMatch = false;
        //appender0.AddFilter(_filter);

        BasicConfigurator.Configure(appender0);
        appender0.ActivateOptions();
    }

    /// <summary>
    /// 文件滚动方式
    /// </summary>
    private static void M_RollingFileAppender()
    {
        RollingFileAppender appender = new RollingFileAppender();
        appender.Name = "mylog";
        appender.File = @"Mylog\log123.txt";
        appender.AppendToFile = true;
        appender.Encoding = Encoding.UTF8;
        appender.RollingStyle = RollingFileAppender.RollingMode.Composite;
        appender.DatePattern = @"yyyyMMdd-HHmmssfff";
        appender.MaximumFileSize = "10MB";// "10MB";
        appender.MaxSizeRollBackups = 1024;
        log4net.Layout.PatternLayout layout = new log4net.Layout.PatternLayout("%d{yyyy-MM-dd HH:mm:ss,fff}[%t] %-5p [%c] : %m%n");
        appender.Layout = layout;
        BasicConfigurator.Configure(appender);
        appender.ActivateOptions();
    }
}