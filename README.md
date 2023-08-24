# 介绍
本解决方案下包含了多个项目，均可单独使用，已发布至nuget，可直接搜索。

## 【CL_Logger.Core】 为日志器。(Standard2.0; net40)
引用后无需任何配置可直接使用log4net日志文件。

使用方法： Logger.Debug("生成Debug级别日志。");

扩展配置： 假设执行文件为a.exe 优先找配置文件：a_log4net.config，log4net.config。若找到配置文件，则应用配置文件。 若没有配置文件，则生成a.exe_Log.txt

日志输出： 时间 级别 [项目.类] - 填写的内容

```C#

2023-05-06 15:58:30,718[1] DEBUG [CS_XXXX.Form1] - Form1_Load() 

2023-05-06 15:58:30,718[1] DEBUG [CS_XXXX.Form1] - 生成Debug级别日志。 

2023-05-06 15:58:30,735[1] DEBUG [CS_XXXX.Form1] - Form1_Load()完成。
```


## 【CL_Unit.Core】静态工具类。(Standard2.0; net40)

TextHelper // Hex16进制、bit2进制、BCD编码、字符串、byte数组 互相转换。
```C#
例：
M_StrToHexByte("F4000DFF")
//执行结果:byte[]_bytes0={0xf4,0x00,0x0d,0xff};
```
ByteArrayUtil // byte数组与数值（short、int16、int32、long、float、double）互转。支持大端(高位)在前、小端(低位)在前等特殊编码要求。

最佳实践：工业Modbus通讯时编码转换。
```C#
例：
int2ByteArray_ABCD(11) //大端(高位)在前。
//执行结果:byte[]_bytes0={0x00,0x00,0x00,0x0B};
int2ByteArray_DCBA(11) //小端(低位)在前。
//执行结果:byte[]_bytes0={0x0B,0x00,0x00,0x00};
同时还有int2ByteArray_CDAB(), int2ByteArray_BADC() 一般用作Modbus通讯时数据编码转换。

```

ShowAttributeHelper // 用作给datagridview显示时，控制是否显示及显示标题。


IniHelper // 生成或操作2级数据的ini文件。

IniHelper.M_Write()会自动生成或扩展文件。
ini文件示例：
```C#
[users]
name=张三
```


## 【CL_Unit_Office.Core】 静态工具类。(Standard2.0; net40)
用作对Excel，Word的导入导出



## 【CL_MyLogZip.Core】 静态工具类。(Standard2.0; net40)
一次调用后周期性执行：将按时间（日）将文件夹下的文件分组，打包成zip，删除清理源文件以释放磁盘空间。
最佳实践：程序长时间运行后，会生成了大量log文件占用磁盘空间（如CL_Logger生成的mylog日志文件夹）。可用本功能压缩释放空间。
使用方法：
```C#
CL_MyLogZip.MyLogZip.M_SetTime(2, 0, 25); //设定，每天凌晨2:0:0触发执行。
CL_MyLogZip.MyLogZip.M_Start(); //启动任务。
```


