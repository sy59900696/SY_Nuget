/*---------------------------------------------------------------- 
// 作者：孙勇   
// 
// 文件功能描述：文件IO操作类
// 
// 创建日期： 2017-08-08
// 
// 修改标识： 
// 修改描述： 

//----------------------------------------------------------------*/


using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Xml;

namespace StaticCode
{
    /// <summary>
    /// 
    /// </summary>
    public static class FileHelper
    {
        #region ==========共享锁读写文件=============

        /// <summary>
        /// 共享锁。读写文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ShareRead(string file, Encoding encoding)
        {
            string content = string.Empty;
            FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            try
            {
                if (fs.CanRead)
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    content = encoding.GetString(buffer);
                }
            }
            finally
            {
                fs.Close();
                fs.Dispose();
            }
            return content;
        }

        /// <summary>
        /// 共享锁。扩展写文件
        /// </summary>
        /// <param name="content"></param>
        /// <param name="file"></param>
        /// <param name="encoding"></param>
        public static void ShareAppend(string content, string file, Encoding encoding)
        {
            ShareWrite(content, file, encoding, FileMode.Append);
        }

        /// <summary>
        /// 共享锁。写文件
        /// </summary>
        /// <param name="content"></param>
        /// <param name="file"></param>
        /// <param name="encoding"></param>
        /// <param name="fileMode"></param>
        public static void ShareWrite(string content, string file, Encoding encoding, FileMode fileMode)
        {
            FileStream fs = new FileStream(file, fileMode, FileAccess.Write, FileShare.Read);
            try
            {
                if (fs.CanWrite)
                {
                    byte[] buffer = encoding.GetBytes(content);
                    if (buffer.Length > 0)
                    {
                        fs.Write(buffer, 0, buffer.Length);
                        fs.Flush();
                    }
                }
            }
            finally
            {
                fs.Close();
                fs.Dispose();
            }
        }

        #endregion ==========共享锁读写文件=============

        #region ==========计算文件的哈希值===========

        /// <summary>
        /// 计算文件的哈希值。默认sha1算法
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string HashFile(string fileName)
        {
            return HashFile(fileName, "sha1");
        }

        /// <summary>
        /// 计算文件的哈希值
        /// </summary>
        /// <param name="fileName">要计算哈希值的文件名和路径</param>
        /// <param name="algName">算法:sha1,md5</param>
        /// <returns>哈希值16进制字符串</returns>
        public static string HashFile(string fileName, string algName)
        {
            if (!System.IO.File.Exists(fileName))
                return string.Empty;

            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            byte[] hashBytes = HashData(fs, algName);
            fs.Close();
            return ByteArrayToHexString(hashBytes);
        }

        /// <summary>
        /// 计算哈希值。默认sha1算法
        /// </summary>
        /// <param name="stream">要计算哈希值的 Stream</param>
        /// <returns>哈希值字节数组</returns>
        public static byte[] HashData(Stream stream)
        {
            return HashData(stream, "sha1");
        }

        /// <summary>
        /// 计算哈希值
        /// </summary>
        /// <param name="stream">要计算哈希值的 Stream</param>
        /// <param name="algName">算法:sha1,md5</param>
        /// <returns>哈希值字节数组</returns>
        public static byte[] HashData(Stream stream, string algName)
        {
            HashAlgorithm algorithm;
            if (algName == null)
            {
                throw new ArgumentNullException("algName 不能为 null");
            }
            if (string.Compare(algName, "sha1", true) == 0)
            {
                algorithm = SHA1.Create();
            }
            else
            {
                if (string.Compare(algName, "md5", true) != 0)
                {
                    throw new Exception("algName 只能使用 sha1 或 md5");
                }
                algorithm = MD5.Create();
            }
            return algorithm.ComputeHash(stream);
        }

        /// <summary>
        /// 字节数组转换为16进制表示的字符串
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static string ByteArrayToHexString(byte[] buf)
        {
            string returnStr = "";
            if (buf != null)
            {
                for (int i = 0; i < buf.Length; i++)
                {
                    returnStr += buf[i].ToString("X2");
                }
            }
            return returnStr;

        }

        #endregion ==========计算文件的哈希值===========

        #region ==========appSettings下key.value=====

        /// <summary>
        /// 设置xml文件的appSettings下key的配置项。如<add key="Form_Real_width" value="300" />
        /// </summary> 
        /// <param name="sAppKey">key</param>
        /// <param name="sAppValue">value</param>
        public static void SetCfgValue(string sAppKey, string sAppValue)
        {
            try
            {
                string _sFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.FriendlyName + ".config");
                SetCfgValue(_sFileName, sAppKey, sAppValue);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(string.Format("SetCfgValue()异常：{0}", ex.Message.ToString()));
            }
        }

        /// <summary>
        /// 设置xml文件的appSettings下key的配置项。如<add key="Form_Real_width" value="300" />
        /// </summary> 
        /// <param name="sAppKey">key</param>
        /// <param name="sAppValue">value</param>
        public static void SetCfgValue(string sFullName_Config, string sAppKey, string sAppValue)
        {
            try
            {
                if (!File.Exists(sFullName_Config)) throw new Exception(string.Format("配置文件路径不可用。{0}", sFullName_Config));
                XmlDocument xDoc = new XmlDocument();

                //此处配置文件在程序目录下
                xDoc.Load(sFullName_Config);
                XmlNode xNode;
                XmlElement xElem1;
                XmlElement xElem2;
                xNode = xDoc.SelectSingleNode("//appSettings");
                xElem1 = (XmlElement)xNode.SelectSingleNode("//add[@key='" + sAppKey + "']");
                if (xElem1 != null)
                {
                    xElem1.SetAttribute("value", sAppValue);
                }
                else
                {
                    xElem2 = xDoc.CreateElement("add");
                    xElem2.SetAttribute("key", sAppKey);
                    xElem2.SetAttribute("value", sAppValue);
                    xNode.AppendChild(xElem2);
                }
                xDoc.Save(sFullName_Config);
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                Logger.WriteLog(string.Format("JDHelper.FileHelper.SetCfgValue()异常：{0}", ex.Message.ToString()));
            }
        }

        /// <summary>
        /// 读取xml文件的appSettings下key的配置项
        /// </summary>
        /// <param name="sFullName_Config"></param>
        /// <param name="sAppKey"></param> 
        public static string GetCfgValue(string sAppKey)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                string _sFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.FriendlyName + ".config");
                return GetCfgValue(_sFileName, sAppKey);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(string.Format("加载配置文件失败。异常：{0}", ex.Message.ToString()));
            }
            return "";
        }

        /// <summary>
        /// 读取xml文件的appSettings下key的配置项
        /// </summary>
        /// <param name="sFullName_Config"></param>
        /// <param name="sAppKey"></param> 
        public static string GetCfgValue(string sFullName_Config, string sAppKey)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                if (!File.Exists(sFullName_Config)) throw new Exception(string.Format("配置文件路径不可用。{0}", sFullName_Config));
                xDoc.Load(sFullName_Config);
                XmlNode xNode;
                XmlElement xElem1;
                xNode = xDoc.SelectSingleNode("//appSettings");
                xElem1 = (XmlElement)xNode.SelectSingleNode("//add[@key='" + sAppKey + "']");
                if (xElem1 != null) return xElem1.GetAttribute("value");
            }
            catch (Exception ex)
            {
                Logger.WriteLog(string.Format("加载配置文件失败。异常：{0}", ex.Message.ToString()));
            }
            return "";
        }

        /// <summary>
        /// 读取xml文件的appSettings下key的配置项。值为true或者大于0的整形:true; 值为false或其他值（小于等于0、非整形、不存在）:false
        /// </summary>
        /// <param name="sFullName_Config"></param>
        /// <param name="sAppKey"></param> 
        public static bool GetCfgBoolValue(string sAppKey)
        {
            string _sFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.FriendlyName + ".config");
            return GetCfgBoolValue(_sFileName, sAppKey);
        }

        /// <summary>
        /// 读取xml文件的appSettings下key的配置项。值为true或者大于0的整形:true; 值为false或其他值（小于等于0、非整形、不存在）:false
        /// </summary>
        /// <param name="sFullName_Config"></param>
        /// <param name="sAppKey"></param> 
        public static bool GetCfgBoolValue(string sFullName_Config, string sAppKey)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                if (!File.Exists(sFullName_Config)) throw new Exception(string.Format("配置文件路径不可用。{0}", sFullName_Config));
                xDoc.Load(sFullName_Config);
                XmlNode xNode;
                XmlElement xElem1;
                xNode = xDoc.SelectSingleNode("//appSettings");
                xElem1 = (XmlElement)xNode.SelectSingleNode("//add[@key='" + sAppKey + "']");
                if (xElem1 != null)
                {
                    string _sValue = xElem1.GetAttribute("value");
                    bool _IsValue = false;
                    if (bool.TryParse(_sValue, out _IsValue) && _IsValue) return true;

                    int _iValue = 0;
                    if (int.TryParse(_sValue, out _iValue) && _iValue > 0)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(string.Format("加载配置文件失败。异常：{0}", ex.Message.ToString()));
            }
            return false;
        }

        /// <summary>
        /// 读取xml文件的appSettings下key的配置项。正常整形:正常返回; 非整形或不存在则返回0。
        /// </summary>
        /// <param name="sFullName_Config"></param>
        /// <param name="sAppKey"></param> 
        public static int GetCfgIntValue(string sAppKey)
        {
            string _sFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.FriendlyName + ".config");
            return GetCfgIntValue(_sFileName, sAppKey);
        }

        /// <summary>
        /// 读取xml文件的appSettings下key的配置项。正常整形:正常返回; 非整形或不存在则返回0。
        /// </summary>
        /// <param name="sFullName_Config"></param>
        /// <param name="sAppKey"></param> 
        public static int GetCfgIntValue(string sFullName_Config, string sAppKey)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                if (!File.Exists(sFullName_Config)) throw new Exception(string.Format("配置文件路径不可用。{0}", sFullName_Config));
                xDoc.Load(sFullName_Config);
                XmlNode xNode;
                XmlElement xElem1;
                xNode = xDoc.SelectSingleNode("//appSettings");
                xElem1 = (XmlElement)xNode.SelectSingleNode("//add[@key='" + sAppKey + "']");
                if (xElem1 != null)
                {
                    string _sValue = xElem1.GetAttribute("value");
                    int _iValue = 0;
                    if (int.TryParse(_sValue, out _iValue))
                    {
                        return _iValue;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(string.Format("加载配置文件失败。异常：{0}", ex.Message.ToString()));
            }
            return 0;
        }


        /// <summary>
        /// 读取xml文件的appSettings下key的配置项。正常大于0整形:正常返回; 非大于0整形或不存在则返回0。
        /// </summary>
        /// <param name="sFullName_Config"></param>
        /// <param name="sAppKey"></param> 
        public static int GetCfgUIntValue(string sAppKey)
        {
            string _sFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.FriendlyName + ".config");
            return GetCfgUIntValue(_sFileName, sAppKey);
        }

        /// <summary>
        /// 读取xml文件的appSettings下key的配置项。正常大于0整形:正常返回; 非大于0整形或不存在则返回0。
        /// </summary>
        /// <param name="sFullName_Config"></param>
        /// <param name="sAppKey"></param> 
        public static int GetCfgUIntValue(string sFullName_Config, string sAppKey)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                if (!File.Exists(sFullName_Config)) throw new Exception(string.Format("配置文件路径不可用。{0}", sFullName_Config));
                xDoc.Load(sFullName_Config);
                XmlNode xNode;
                XmlElement xElem1;
                xNode = xDoc.SelectSingleNode("//appSettings");
                xElem1 = (XmlElement)xNode.SelectSingleNode("//add[@key='" + sAppKey + "']");
                if (xElem1 != null)
                {
                    string _sValue = xElem1.GetAttribute("value");
                    int _iValue = 0;
                    if (int.TryParse(_sValue, out _iValue) && _iValue > 0)
                    {
                        return _iValue;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(string.Format("加载配置文件失败。异常：{0}", ex.Message.ToString()));
            }
            return 0;
        }

        #endregion ==========appSettings下key.value=====

    }
}
