using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Win_Test.Core
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            M_TestExcel();
            Console.WriteLine();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static void M_TestExcel()
        {
            try
            {
                Logger.Debug(string.Format("M_TestExcel()"));
                List<C_Users> _lstUsers0 = new List<C_Users>();
                for (int i = 0; i < 20; i++) _lstUsers0.Add(new C_Users() { ID = _lstUsers0.Count + 1, Name = string.Format("Name{0}", _lstUsers0.Count + 1), Age = _lstUsers0.Count + 1 });

                //StaticCode.ExcelHelper.ExportExcel(_lstUsers0, "users00", @"C:\t000.xls");

                var _v000 = StaticCode.ExcelHelper.ExcelToDataTable(@"C:\t000.xls", "users00", true);
                Console.WriteLine();

                Logger.Debug(string.Format("M_TestExcel()Íê³É¡£"));
            }
            catch (Exception ex)
            {
                Logger.Debug(string.Format("M_TestExcel()Òì³££º{0}", ex.Message.ToString()));
            }
        }
    }
    public class C_Users
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
