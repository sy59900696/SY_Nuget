using Newtonsoft.Json;

namespace StaticCode
{
    /// <summary>
    /// 字典结构
    /// </summary>
    public class C_Dir
    {
        public int m_iID { get; set; }
        public string m_sClass { get; set; }
        public string m_sKey { get; set; }
        public int m_iValue { get; set; }
        public object m_oOther { get; set; }
        public C_Dir()
        {

        }
        public C_Dir(string _sKey)
        {
            if (string.IsNullOrWhiteSpace(_sKey))
            {
                m_sKey = "==不限==";
            }
            else
            {
                m_sKey = _sKey;
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
