# 介绍
本解决方案下包含了多个项目，均可单独使用，已发布至nuget，可直接搜索。

## 【CL_Logger.Core】 为日志器。(Standard2.0; net40)
引用后无需任何配置可直接使用log4net日志文件。

使用方法： Logger.Debug("生成Debug级别日志。");

扩展配置： 假设执行文件为a.exe 优先找配置文件：a_log4net.config，log4net.config。若找到配置文件，则应用配置文件。 若没有配置文件，则生成a.exe_Log.txt

日志输出： 时间 级别 [项目.类] - 填写的内容

2023-05-06 15:58:30,718[1] DEBUG [CS_XXXX.Form1] - Form1_Load() 

2023-05-06 15:58:30,718[1] DEBUG [CS_XXXX.Form1] - 生成Debug级别日志。 

2023-05-06 15:58:30,735[1] DEBUG [CS_XXXX.Form1] - Form1_Load()完成。


## 【CL_Unit.Core】静态工具类。

TextHelper // 文本、字符编码等相关处理
ByteArrayUtil // byte数组与数值互转。
ShowAttributeHelper // 用作给datagridview显示时，控制是否显示及显示标题。

IniHelper // 生成或操作2级数据的ini文件。
IniHelper.M_Write()会自动生成或扩展文件。
ini文件示例：
[users]
name=张三


## 【CL_Unit_Office.Core】 静态工具类。用作对Excel，Word的导入导出

### 待续。。。

