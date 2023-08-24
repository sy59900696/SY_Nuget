引用后无需任何配置可直接使用log4net日志文件。

使用方法：
Logger.Debug("生成Debug级别日志。");

扩展配置：
假设执行文件为a.exe
优先找配置文件：a_log4net.config，log4net.config。若找到配置文件，则应用配置文件。
若没有配置文件，则生成a.exe_Log.txt

日志输出：
时间 级别 [项目.类] - 填写的内容

2023-05-06 15:58:30,718[1] DEBUG [CS_XXXX.Form1] - Form1_Load()
2023-05-06 15:58:30,718[1] DEBUG [CS_XXXX.Form1] - 生成Debug级别日志。
2023-05-06 15:58:30,735[1] DEBUG [CS_XXXX.Form1] - Form1_Load()完成。
