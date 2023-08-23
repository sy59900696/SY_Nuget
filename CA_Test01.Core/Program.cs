// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Logger.Debug("test123123");
int _i000 = StaticCode.IniHelper.M_Write("users", "name", "张三", Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "test.txt"));
string _s000 = StaticCode.IniHelper.M_Read("users", "name", "没找到", "test.txt");

Console.WriteLine();