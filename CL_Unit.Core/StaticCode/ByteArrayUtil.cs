using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticCode
{
    /// <summary>
    /// byte数组与数值互转
    /// </summary>
    public class ByteArrayUtil
    {
        /// <summary>
        /// 测试
        /// </summary>
        public static void M_Main()
        {
            try
            {
                Logger.Debug(string.Format("M_Main()"));
                byte[] _byte00 = new byte[] { 0, 0, 0, 1 };
                byte[] _byte0000 = M_C4_ABCD(_byte00);
                //short _short0 = byteArray2Short_Big_Endian(_byte00);
                int _i000 = byteArray2Int_BADC(_byte00);

                List<object> _lstLong00 = new List<object>();
                List<string> _lstHex00 = new List<string>();
                List<byte[]> _lstBytes = new List<byte[]>();
                float _f00 = 123.456f;
                _lstBytes.Add(float2ByteArray_DCBA(_f00));
                _lstHex00.Add(StaticCode.TextHelper.M_ByteToHexStr(_lstBytes[_lstBytes.Count - 1]));
                _lstLong00.Add(byteArray2Float_DCBA(_lstBytes[_lstBytes.Count - 1]));
                _lstBytes.Add(float2ByteArray_ABCD(_f00));
                _lstHex00.Add(StaticCode.TextHelper.M_ByteToHexStr(_lstBytes[_lstBytes.Count - 1]));
                _lstLong00.Add(byteArray2Float_ABCD(_lstBytes[_lstBytes.Count - 1]));
                _lstBytes.Add(float2ByteArray_BADC(_f00));
                _lstHex00.Add(StaticCode.TextHelper.M_ByteToHexStr(_lstBytes[_lstBytes.Count - 1]));
                _lstLong00.Add(byteArray2Float_BADC(_lstBytes[_lstBytes.Count - 1]));
                _lstBytes.Add(float2ByteArray_CDAB(_f00));
                _lstHex00.Add(StaticCode.TextHelper.M_ByteToHexStr(_lstBytes[_lstBytes.Count - 1]));
                _lstLong00.Add(byteArray2Float_CDAB(_lstBytes[_lstBytes.Count - 1]));

                double _d00 = 1234.5678;
                _lstBytes.Add(double2ByteArray_ABCDEFGH(_d00));
                _lstHex00.Add(StaticCode.TextHelper.M_ByteToHexStr(_lstBytes[_lstBytes.Count - 1]));
                _lstLong00.Add(byteArray2Double_ABCDEFGH(_lstBytes[_lstBytes.Count - 1]));

                _lstBytes.Add(double2ByteArray_HGFEDCBA(_d00));
                _lstHex00.Add(StaticCode.TextHelper.M_ByteToHexStr(_lstBytes[_lstBytes.Count - 1]));
                _lstLong00.Add(byteArray2Double_HGFEDCBA(_lstBytes[_lstBytes.Count - 1]));

                _lstBytes.Add(double2ByteArray_BADCFEHG(_d00));
                _lstHex00.Add(StaticCode.TextHelper.M_ByteToHexStr(_lstBytes[_lstBytes.Count - 1]));
                _lstLong00.Add(byteArray2Double_BADCFEHG(_lstBytes[_lstBytes.Count - 1]));

                _lstBytes.Add(double2ByteArray_GHEFCDAB(_d00));
                _lstHex00.Add(StaticCode.TextHelper.M_ByteToHexStr(_lstBytes[_lstBytes.Count - 1]));
                _lstLong00.Add(byteArray2Double_GHEFCDAB(_lstBytes[_lstBytes.Count - 1]));

                string _sJson00 = JsonConvert.SerializeObject(_lstLong00);
                string _sJson01 = JsonConvert.SerializeObject(_lstHex00);
                Console.WriteLine();
                Logger.Info(string.Format("M_Main()完成。"));
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("M_Main()异常：{0}", ex.Message.ToString()));
            }
        }

        /// <summary>
        /// 字节数组转 short，小端
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static short byteArray2Short_Little_Endian(byte[] array)
        {

            // 数组长度有误
            //if (array.Length > 2)
            //{
            //    return 0;
            //}

            short value = 0;
            for (int i = 0; i < array.Length && i < 2; i++)
            {
                // & 0xff，除去符号位干扰
                value |= (short)((array[i] & 0xff) << (i * 8));
            }
            return value;
        }

        /// <summary>
        /// 字节数组转 short，大端
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static short byteArray2Short_Big_Endian(byte[] array)
        {

            // 数组长度有误
            //if (array.Length > 2)
            //{
            //    return 0;
            //}

            short value = 0;
            for (int i = 0; i < array.Length && i < 2; i++)
            {
                value |= (short)((array[i] & 0xff) << ((array.Length - i - 1) * 8));
            }
            return value;
        }

        /// <summary>
        /// 字节数组转 int，小端 DCBA
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static int byteArray2Int_Little_Endian(byte[] array)
        {

            // 数组长度有误
            //if (array.Length > 4)
            //{
            //    return 0;
            //}

            int value = 0;
            for (int i = 0; i < array.Length && i < 4; i++)
            {
                value |= ((array[i] & 0xff) << (i * 8));

            }
            return value;
        }

        private static byte[] M_C4_ABCD(byte[] array)
        {
            byte[] _byte00 = new byte[4];
            // 数组长度有误
            if (array.Length < 4) throw new Exception("数组长度小于4");
            _byte00[0] = array[3];
            _byte00[1] = array[2];
            _byte00[2] = array[1];
            _byte00[3] = array[0];

            int _i000 = BitConverter.ToInt32(_byte00, 0);
            return _byte00;
        }
        private static byte[] M_C4_DCBA(byte[] array)
        {
            byte[] _byte00 = new byte[4];
            // 数组长度有误
            if (array.Length < 4) throw new Exception("数组长度小于4");
            return array;
        }
        private static byte[] M_C4_CDAB(byte[] array)
        {
            byte[] _byte00 = new byte[4];
            // 数组长度有误
            if (array.Length < 4) throw new Exception("数组长度小于4");
            _byte00[0] = array[2];
            _byte00[1] = array[3];
            _byte00[2] = array[0];
            _byte00[3] = array[1];
            return _byte00;
        }
        private static byte[] M_C4_BADC(byte[] array)
        {
            byte[] _byte00 = new byte[4];
            // 数组长度有误
            if (array.Length < 4) throw new Exception("数组长度小于4");
            _byte00[0] = array[1];
            _byte00[1] = array[0];
            _byte00[2] = array[3];
            _byte00[3] = array[2];
            return _byte00;
        }

        private static byte[] M_8_ABCDEFGH(byte[] array)
        {
            byte[] _byte00 = new byte[8];
            // 数组长度有误
            if (array.Length < 8) throw new Exception("数组长度小于8");
            _byte00[0] = array[7];
            _byte00[1] = array[6];
            _byte00[2] = array[5];
            _byte00[3] = array[4];
            _byte00[4] = array[3];
            _byte00[5] = array[2];
            _byte00[6] = array[1];
            _byte00[7] = array[0];

            int _i000 = BitConverter.ToInt32(_byte00, 0);
            return _byte00;
        }
        private static byte[] M_8_HGFEDCBA(byte[] array)
        {
            byte[] _byte00 = new byte[8];
            // 数组长度有误
            if (array.Length < 8) throw new Exception("数组长度小于8");
            return array;
        }

        private static byte[] M_8_BADCFEHG(byte[] array)
        {
            byte[] _byte00 = new byte[8];
            // 数组长度有误
            if (array.Length < 8) throw new Exception("数组长度小于8");
            _byte00[0] = array[6];
            _byte00[1] = array[7];
            _byte00[2] = array[4];
            _byte00[3] = array[5];
            _byte00[4] = array[2];
            _byte00[5] = array[3];
            _byte00[6] = array[0];
            _byte00[7] = array[1];

            int _i000 = BitConverter.ToInt32(_byte00, 0);
            return _byte00;
        }
        private static byte[] M_8_GHEFCDAB(byte[] array)
        {
            byte[] _byte00 = new byte[8];
            // 数组长度有误
            if (array.Length < 8) throw new Exception("数组长度小于8");
            _byte00[0] = array[1];
            _byte00[1] = array[0];
            _byte00[2] = array[3];
            _byte00[3] = array[2];
            _byte00[4] = array[5];
            _byte00[5] = array[4];
            _byte00[6] = array[7];
            _byte00[7] = array[6];

            int _i000 = BitConverter.ToInt32(_byte00, 0);
            return _byte00;
        }

        /**
         * 字节数组转 int，大端 ABCD
         */
        public static int byteArray2Int_ABCD(byte[] array)
        {

            // 数组长度有误
            //if (array.Length < 4)
            //{
            //    return 0;
            //}
            if (array.Length < 4) throw new Exception("数组长度应不小于4");

            int value = 0;
            for (int i = 0; i < array.Length && i < 4; i++)
            {
                value |= ((array[i] & 0xff) << ((array.Length - i - 1) * 8));
            }
            return value;
        }

        public static int byteArray2Int_DCBA(byte[] array)
        {

            // 数组长度有误
            //if (array.Length < 4)
            //{
            //    return 0;
            //}

            int value = 0;
            if (array.Length < 4) throw new Exception("数组长度应不小于4");
            value |= ((array[0] & 0xff) << (0 * 8));//d
            value |= ((array[1] & 0xff) << (1 * 8));//c
            value |= ((array[2] & 0xff) << (2 * 8));//b
            value |= ((array[3] & 0xff) << (3 * 8));//a
            return value;
        }
        public static int byteArray2Int_CDAB(byte[] array)
        {

            // 数组长度有误
            //if (array.Length < 4)
            //{
            //    return 0;
            //}

            int value = 0;
            if (array.Length < 4) throw new Exception("数组长度应不小于4");
            value |= ((array[0] & 0xff) << (1 * 8));//
            value |= ((array[1] & 0xff) << (0 * 8));//
            value |= ((array[2] & 0xff) << (3 * 8));//
            value |= ((array[3] & 0xff) << (2 * 8));//
            return value;
        }

        public static int byteArray2Int_BADC(byte[] array)
        {

            // 数组长度有误
            //if (array.Length < 4)
            //{
            //    return 0;
            //}

            int value = 0;
            if (array.Length < 4) throw new Exception("数组长度应不小于4");
            value |= ((array[0] & 0xff) << (2 * 8));//
            value |= ((array[1] & 0xff) << (3 * 8));//
            value |= ((array[2] & 0xff) << (0 * 8));//
            value |= ((array[3] & 0xff) << (1 * 8));//
            return value;
        }

        /**
         * 字节数组转 float，小端
         */
        public static float byteArray2Float_DCBA(byte[] array)
        {
            return BitConverter.ToSingle(M_C4_DCBA(array), 0);
        }

        /**
         * 字节数组转 float，大端
         */
        public static float byteArray2Float_ABCD(byte[] array)
        {
            return BitConverter.ToSingle(M_C4_ABCD(array), 0);
        }
        public static float byteArray2Float_CDAB(byte[] array)
        {
            return BitConverter.ToSingle(M_C4_BADC(array), 0);
        }
        public static float byteArray2Float_BADC(byte[] array)
        {
            return BitConverter.ToSingle(M_C4_CDAB(array), 0);
        }

        /**
         * 字节数组转 long，小端
         */
        public static long byteArray2Long_Little_Endian(byte[] array)
        {

            // 数组长度有误
            //if (array.Length != 8)
            //{
            //    return 0;
            //}
            if (array.Length < 8) throw new Exception("数组长度应不小于8");

            long value = 0;
            for (int i = 0; i < array.Length && i < 8; i++)
            {
                // 需要转long再位移，否则int丢失精度
                value |= ((long)(array[i] & 0xff) << (i * 8));
            }
            return value;
        }

        /**
         * 字节数组转 long，大端
         */
        public static long byteArray2Long_Big_Endian(byte[] array)
        {

            // 数组长度有误
            //if (array.Length != 8)
            //{
            //    return 0;
            //}

            long value = 0;
            if (array.Length < 8) throw new Exception("数组长度应不小于8");
            for (int i = 0; i < array.Length && i < 8; i++)
            {
                value |= ((long)(array[i] & 0xff) << ((array.Length - i - 1) * 8));
            }
            return value;
        }

        public static long byteArray2Long_HGFEDCBA(byte[] array)
        {

            // 数组长度有误
            //if (array.Length < 8)
            //{
            //    return 0;
            //}

            long value = 0;
            if (array.Length < 8) throw new Exception("数组长度应不小于8");
            value |= ((long)(array[0] & 0xff) << (0 * 8));
            value |= ((long)(array[1] & 0xff) << (1 * 8));
            value |= ((long)(array[2] & 0xff) << (2 * 8));
            value |= ((long)(array[3] & 0xff) << (3 * 8));
            value |= ((long)(array[4] & 0xff) << (4 * 8));
            value |= ((long)(array[5] & 0xff) << (5 * 8));
            value |= ((long)(array[6] & 0xff) << (6 * 8));
            value |= ((long)(array[7] & 0xff) << (7 * 8));
            return value;
        }

        public static long byteArray2Long_GHEFCDAB(byte[] array)
        {

            // 数组长度有误
            //if (array.Length < 8)
            //{
            //    return 0;
            //}

            long value = 0;
            if (array.Length < 8) throw new Exception("数组长度应不小于8");
            value |= ((long)(array[0] & 0xff) << (1 * 8));
            value |= ((long)(array[1] & 0xff) << (0 * 8));
            value |= ((long)(array[2] & 0xff) << (3 * 8));
            value |= ((long)(array[3] & 0xff) << (2 * 8));
            value |= ((long)(array[4] & 0xff) << (5 * 8));
            value |= ((long)(array[5] & 0xff) << (4 * 8));
            value |= ((long)(array[6] & 0xff) << (7 * 8));
            value |= ((long)(array[7] & 0xff) << (6 * 8));
            return value;
        }

        public static long byteArray2Long_ABCDEFGH(byte[] array)
        {

            // 数组长度有误
            //if (array.Length < 8)
            //{
            //    return 0;
            //}

            long value = 0;
            if (array.Length < 8) throw new Exception("数组长度应不小于8");
            value |= ((long)(array[0] & 0xff) << (7 * 8));
            value |= ((long)(array[1] & 0xff) << (6 * 8));
            value |= ((long)(array[2] & 0xff) << (5 * 8));
            value |= ((long)(array[3] & 0xff) << (4 * 8));
            value |= ((long)(array[4] & 0xff) << (3 * 8));
            value |= ((long)(array[5] & 0xff) << (2 * 8));
            value |= ((long)(array[6] & 0xff) << (1 * 8));
            value |= ((long)(array[7] & 0xff) << (0 * 8));
            return value;
        }

        public static long byteArray2Long_BADCFEHG(byte[] array)
        {

            // 数组长度有误
            //if (array.Length < 8)
            //{
            //    return 0;
            //}

            long value = 0;
            if (array.Length < 8) throw new Exception("数组长度应不小于8");
            value |= ((long)(array[0] & 0xff) << (6 * 8));
            value |= ((long)(array[1] & 0xff) << (7 * 8));
            value |= ((long)(array[2] & 0xff) << (4 * 8));
            value |= ((long)(array[3] & 0xff) << (5 * 8));
            value |= ((long)(array[4] & 0xff) << (2 * 8));
            value |= ((long)(array[5] & 0xff) << (3 * 8));
            value |= ((long)(array[6] & 0xff) << (0 * 8));
            value |= ((long)(array[7] & 0xff) << (1 * 8));
            return value;
        }
        public static double byteArray2Double_HGFEDCBA(byte[] array)
        {
            return BitConverter.ToDouble(array, 0);
        }
        public static double byteArray2Double_GHEFCDAB(byte[] array)
        {
            return BitConverter.ToDouble(M_8_GHEFCDAB(array), 0);
        }
        public static double byteArray2Double_ABCDEFGH(byte[] array)
        {
            return BitConverter.ToDouble(M_8_ABCDEFGH(array), 0);
        }
        public static double byteArray2Double_BADCFEHG(byte[] array)
        {
            return BitConverter.ToDouble(M_8_BADCFEHG(array), 0);
        }


        //---------------------------------华丽的分割线-------------------------------------

        /**
         * short 转字节数组，小端
         */
        public static byte[] short2ByteArray_Little_Endian(short s)
        {

            byte[] array = new byte[2];

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = (byte)(s >> (i * 8));
            }
            return array;
        }

        /**
         * short 转字节数组，大端
         */
        public static byte[] short2ByteArray_Big_Endian(short s)
        {

            byte[] array = new byte[2];

            for (int i = 0; i < array.Length; i++)
            {
                array[array.Length - 1 - i] = (byte)(s >> (i * 8));
            }
            return array;
        }

        /**
         * int 转字节数组，小端 DCBA。输入11, 输出0B000000
         */
        public static byte[] int2ByteArray_DCBA(int s)
        {

            byte[] array = new byte[4];

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = (byte)(s >> (i * 8));
            }
            return array;
        }

        /**
         * int 转字节数组，大端。输入11, 输出0000000B
         */
        public static byte[] int2ByteArray_ABCD(int s)
        {

            byte[] array = new byte[4];

            for (int i = 0; i < array.Length; i++)
            {
                array[array.Length - 1 - i] = (byte)(s >> (i * 8));
            }
            return array;
        }

        public static byte[] int2ByteArray_CDAB(int s)
        {
            byte[] array = new byte[4];
            array[1] = (byte)(s >> (0 * 8));//d
            array[0] = (byte)(s >> (1 * 8));//c
            array[3] = (byte)(s >> (2 * 8));//b
            array[2] = (byte)(s >> (3 * 8));//a
            return array;
        }

        public static byte[] int2ByteArray_BADC(int s)
        {
            byte[] array = new byte[4];
            array[2] = (byte)(s >> (0 * 8));//d
            array[3] = (byte)(s >> (1 * 8));//c
            array[0] = (byte)(s >> (2 * 8));//b
            array[1] = (byte)(s >> (3 * 8));//a
            return array;
        }


        /**
         * float 转字节数组，小端
         */
        public static byte[] float2ByteArray_DCBA(float f)
        {
            byte[] _bytes0 = BitConverter.GetBytes(f);
            return _bytes0;
        }

        /**
         * float 转字节数组，大端
         */
        public static byte[] float2ByteArray_ABCD(float f)
        {
            byte[] _bytes0 = BitConverter.GetBytes(f);
            byte[] _result0 = M_C4_ABCD(_bytes0);
            return _result0;
        }
        public static byte[] float2ByteArray_CDAB(float f)
        {
            byte[] _bytes0 = BitConverter.GetBytes(f);
            byte[] _result0 = M_C4_BADC(_bytes0);
            return _result0;
        }
        public static byte[] float2ByteArray_BADC(float f)
        {
            byte[] _bytes0 = BitConverter.GetBytes(f);
            byte[] _result0 = M_C4_CDAB(_bytes0);
            return _result0;
        }

        public static byte[] double2ByteArray_HGFEDCBA(double d)
        {
            byte[] array = BitConverter.GetBytes(d);
            return M_8_HGFEDCBA(array);
        }
        public static byte[] double2ByteArray_GHEFCDAB(double d)
        {
            byte[] array = BitConverter.GetBytes(d);
            return M_8_GHEFCDAB(array);
        }
        public static byte[] double2ByteArray_ABCDEFGH(double d)
        {
            byte[] array = BitConverter.GetBytes(d);
            return M_8_ABCDEFGH(array);
        }
        public static byte[] double2ByteArray_BADCFEHG(double d)
        {
            byte[] array = BitConverter.GetBytes(d);
            return M_8_BADCFEHG(array);
        }

    }
}
