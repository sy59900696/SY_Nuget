using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaticCode
{ 
    public class C_Enum
    {
        public static List<C_KeyValue> G_lst_UserPower = new List<C_KeyValue>();

        public static void M_Init()
        {
            G_lst_UserPower.Clear();
            G_lst_UserPower = M_ConvertEnum2List(((Enum)new E_UserPower()).GetType());
        }  

        private static List<C_KeyValue> M_ConvertEnum2List(Type source)
        {
            List<C_KeyValue> _lstResult = new List<C_KeyValue>();

            foreach (string name in Enum.GetNames(source))
            {
                C_KeyValue _c0 = new C_KeyValue();
                _c0.sKey = name;
                _c0.iValue = (int)Enum.Parse(source, name);
                _lstResult.Add(_c0);
            }
            return _lstResult;
        }
    }

    public enum E_UserPower
    {
        普通用户 = 0,
        管理员 = 1
    }

    public class C_KeyValue
    {
        private string _sKey = "";
        private int _iValue = 0;

        public int iValue
        {
            set { _iValue = value; }
            get { return _iValue; }
        }

        public string sKey
        {
            set { _sKey = value; }
            get { return _sKey; }
        } 
    }
}
