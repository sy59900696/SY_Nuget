using CL_TestBase.Stand;
using System;

namespace CL_Test01.Stand
{
    /// <summary>
    /// C_Lab_Test01
    /// </summary>
    public class C_Lab_Test01
    {
        /// <summary>
        /// 求 a + b + c + d
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int M_Add4(int a, int b, int c, int d)
        {
            int _ab = C_Lab_TestBase.M_Add(a, b);
            int _cd = C_Lab_TestBase.M_Add(a, b);
            return C_Lab_TestBase.M_Add(_ab, _cd);
        }
    }
}
