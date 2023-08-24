
一次调用后周期性执行：将按时间（日）将文件夹下的文件分组，打包成zip，删除清理源文件以释放磁盘空间。
最佳实践：程序长时间运行后，会生成了大量log文件占用磁盘空间（如CL_Logger生成的mylog日志文件夹）。可用本功能压缩释放空间。
使用方法：
CL_MyLogZip.MyLogZip.M_SetTime(2, 0, 25); //设定，每天凌晨2:0:0触发执行。
CL_MyLogZip.MyLogZip.M_Start(); //启动任务。

