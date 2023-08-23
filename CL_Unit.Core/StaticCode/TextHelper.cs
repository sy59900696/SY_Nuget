using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaticCode
{
    /// <summary>
    /// 文本、字符编码等相关处理
    /// </summary>
    public class TextHelper
    {
        /// <summary>
        /// 数字和字节之间互转
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static int M_IntToBitConverter(int num)
        {
            int temp = 0;
            byte[] bytes = BitConverter.GetBytes(num);//将int32转换为字节数组
            temp = BitConverter.ToInt32(bytes, 0);//将字节数组内容再转成int32类型
            return temp;
        }

        /// <summary>
        /// 将字符串转为16进制字符，允许中文。例：M_StringToHexString("abcd", Encoding.UTF8, " ")执行结果："61 62 63 64 "
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string M_StringToHexString(string s, Encoding encode, string spanString)
        {
            byte[] b = encode.GetBytes(s);//按照指定编码将string编程字节数组
            string result = string.Empty;
            for (int i = 0; i < b.Length; i++)//逐字节变为16进制字符
            {
                result += Convert.ToString(b[i], 16) + spanString;
            }
            return result;
        }

        /// <summary>
        /// 将16进制字符串转为字符串。例：M_HexStringToString("41414141", System.Text.Encoding.UTF8),执行结果："AAAA"
        /// </summary>
        /// <param name="hs"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string M_HexStringToString(string hs, Encoding encode)
        {
            string strTemp = "";
            byte[] b = new byte[hs.Length / 2];
            for (int i = 0; i < hs.Length / 2; i++)
            {
                strTemp = hs.Substring(i * 2, 2);
                b[i] = Convert.ToByte(strTemp, 16);
            }
            //按照指定编码将字节数组变为字符串
            return encode.GetString(b);
        }

        /// <summary>
        /// byte[]转为16进制字符串。例：byte[]_bytes0={0xf4,0x00,0x0d,0xff,0xfe,0xfd,0xfc,0x01,0x01,0x01,0x01,0xf6,0x4f};执行结果:"F4000DFFFEFDFC01010101F64F"
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="iStart"></param>
        /// <param name="iLength"></param>
        /// <returns></returns> 
        public static string M_ByteToHexStr(byte[] bytes, int iStart = 0, int iLength = 0)
        {
            string returnStr = "";
            if (bytes != null)
            {
                if (iLength <= 0) iLength = bytes.Length - iStart;
                for (int i = iStart; i < iLength + iStart; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        /// <summary>
        /// 将16进制的字符串转为byte[]。例："F4000DFFFEFDFC01010101F64F"，执行结果:byte[]_bytes0={0xf4,0x00,0x0d,0xff,0xfe,0xfd,0xfc,0x01,0x01,0x01,0x01,0xf6,0x4f};
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] M_StrToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary>
        /// 将16进制的字符串转为byte[]。例："68010201000000001040370100"，执行结果:byte[]_bytes0={0x68,0x01,0x02,0x01,0x00,....};
        /// </summary>
        /// <param name="_sCmd"></param>
        /// <returns></returns>
        public static List<byte> M_BcdToByte(string _sCmd)
        {
            List<byte> _lstBytes0 = new List<byte>();
            for (int i = 0; i < _sCmd.Length; i += 2)
            {
                byte _b0 = (byte)Convert.ToInt32(_sCmd.Substring(i, 2), 16);
                _lstBytes0.Add(_b0);
            }
            return _lstBytes0;
        }

        /// <summary>
        ///  byte[]转为16进制字符串。截取buffer，从iStart，向后 例：byte[]_bytes0={0x68,0x01,0x02,0x01,0x00,....};执行结果:"68010201000000001040370100"
        /// </summary>
        /// <param name="_buffer"></param>
        /// <param name="_iStart"></param>
        /// <param name="_iCount">0:则默认为_buffer.Length</param>
        /// <returns></returns>
        public static string M_ByteToBcd(byte[] _buffer, int _iStart = 0, int _iCount = 0)
        {
            StringBuilder _sb0 = new StringBuilder();

            _iCount = _iStart + _iCount;
            if (_iCount <= 0 || _iCount > _buffer.Length) _iCount = _buffer.Length;

            for (int i = _iStart; i < _iCount; i++)
            {
                int _iByte = (int)_buffer[i];
                _sb0.Append(Convert.ToString(_iByte, 16).PadLeft(2, '0').ToUpper());
            }
            return _sb0.ToString();
        }

        /// <summary>
        /// byte[]转为16进制字符串。例：byte[]_bytes0={0x68,0x01,0x02,0x01,0x00,....};执行结果:"68010201000000001040370100"
        /// </summary>
        /// <param name="_buffer"></param>
        /// <returns></returns>
        public static string M_ByteToBcd(List<byte> _buffer)
        {
            StringBuilder _sb0 = new StringBuilder();
            for (int i = 0; i < _buffer.Count; i++)
            {
                int _iByte = (int)_buffer[i];
                _sb0.Append(Convert.ToString(_iByte, 16).PadLeft(2, '0').ToUpper());
            }
            return _sb0.ToString();
        }

        /// <summary>
        /// byte转为16进制字符串。例：_bytes0=0x68;执行结果:"68"
        /// </summary>
        /// <param name="_bytes0"></param>
        /// <returns></returns>
        public static string M_ByteToBcd(byte _bytes0)
        {
            int _iByte = (int)_bytes0;
            return (Convert.ToString(_iByte, 16).PadLeft(2, '0').ToUpper());
        }

        /// <summary>
        /// 将BCD码字符串进行按位翻转。如卡号：0100000000040132，进行翻转操作得：3201040000000001
        /// </summary>
        /// <param name="_sCmd"></param>
        /// <returns></returns>
        public static string M_BcdRevert(string _sCmd)
        {
            List<string> _lstTmp = new List<string>();
            for (int i = 0; i < _sCmd.Length; i += 2) _lstTmp.Add(_sCmd.Substring(i, 2));
            _lstTmp.Reverse();

            StringBuilder _sb0 = new StringBuilder();
            foreach (string _s0 in _lstTmp) _sb0.Append(_s0);
            return _sb0.ToString();
        }

        /// <summary>
        /// byte转bits。例：(byte)7 执行结果（不含空格）：00000111
        /// </summary>
        /// <param name="_byte0"></param>
        /// <returns></returns>
        public static String M_ByteToBit(byte _byte0)
        {
            return ""
                    + (byte)((_byte0 >> 7) & 0x1) + (byte)((_byte0 >> 6) & 0x1)
                    + (byte)((_byte0 >> 5) & 0x1) + (byte)((_byte0 >> 4) & 0x1)
                    + (byte)((_byte0 >> 3) & 0x1) + (byte)((_byte0 >> 2) & 0x1)
                    + (byte)((_byte0 >> 1) & 0x1) + (byte)((_byte0 >> 0) & 0x1);
        }

        /// <summary>
        /// byte转bits。例：[0x1,0x7] 执行结果（不含空格）：00000001 00000111
        /// </summary>
        /// <param name="_lstBytes"></param>
        /// <returns></returns>
        public static string M_ByteToBit(byte[] _lstBytes)
        {
            StringBuilder _sb0 = new StringBuilder();
            foreach (byte item in _lstBytes)
            {
                _sb0.Append(M_ByteToBit(item));
            }
            return _sb0.ToString();
        }

        /// <summary>
        /// bit数组转byte。例（自动忽略空格，自动左补0）：00000001，执行结果：1
        /// </summary>
        /// <param name="_sBits"></param>
        /// <returns></returns>
        public static byte M_BitToByte(string _sBits)
        {
            _sBits = _sBits.Replace(" ", "");
            int _iNum = 0;
            for (int i = 0; i < _sBits.Length; i++)
            {
                _iNum *= 2;
                char _c00 = _sBits[i];
                if (_c00 == '1')
                {
                    _iNum += 1;
                }
            }
            return (byte)_iNum;
        }

        /// <summary>
        /// bit数组转数值。例（自动忽略空格，自动左补0）：00000001 00000001，执行结果：256+1==257
        /// </summary>
        /// <param name="_sBits"></param>
        /// <returns></returns>
        public static UInt64 M_BitToNumber(string _sBits)
        {
            _sBits = _sBits.Replace(" ", "");
            UInt64 _iNum = 0;
            for (int i = 0; i < _sBits.Length; i++)
            {
                _iNum *= 2;
                char _c00 = _sBits[i];
                if (_c00 == '1')
                {
                    _iNum += 1;
                }
            }
            return _iNum;
        }
    }
}
