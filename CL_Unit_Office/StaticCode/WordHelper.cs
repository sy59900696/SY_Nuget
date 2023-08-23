using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using NPOI.XWPF.UserModel;
using System.Data;

/*============= 调用示例 =========================================

    static class Program
    {
        static System.Text.StringBuilder sb = null;
        static string sql = "";
        static DataSet ds;
        static WordHelper word = new WordHelper();
          
        static void Main()
        {
            try
            {
                word.openWord(@"模板.docx");
                Dictionary<string, object> d = new Dictionary<string, object>();
                //段落
                d = new Dictionary<string, object>();
                d.Add("no", DateTime.Now.ToString());
                word.setCellValueParagraphs(d);
                //表格内单元格局部替换
                d = new Dictionary<string, object>();
                d.Add("tableusername", "郑州张三");
                word.setCellValuetTables(d, 0);
                //表格内单元格全替换
                d = new Dictionary<string, object>();
                d.Add("#tableuserid$", "001");
                d.Add("#tableusername$", "郑州张三");
                //d.Add("#img$", "$img{" + context.Server.MapPath(@"~/test/wordhelper/猫.jpg") + "}(5000000,5000000)"); //插入图片(宽，高)
                word.setCellValuetTables1(d, 1);
                //循环表格
                DataSet ds_table = new DataSet();
                DataTable dt_table = new DataTable();
                dt_table.Columns.Add("itemid");
                dt_table.Columns.Add("itemname");
                for (int i = 0; i < 5; i++)
                {
                    DataRow dr = dt_table.NewRow();
                    dr["itemid"] = i.ToString();
                    dr["itemname"] = i.ToString() + "name";
                    dt_table.Rows.Add(dr);
                }
                ds_table.Tables.Add(dt_table);
                word.setCellValuetTables(ds_table, 1, word.tables[2]);

                string newfile = "test/wordhelper/export/" + DateTime.Now.ToString("yyyyMMddHHmm") + ".docx";
                word.SaveToFile(@"D:\D\moohigh\P_MP_SY_En_253\trunk\P_MP_SY_En\CS_Test123\bin\Debug\模板_输出1.docx");
                //context.Response.Write("{\"flag\":true,\"msg\":\"" + newfile + "\"}");
            }
            catch (Exception ex)
            { 
                throw;
            } 
        }
    }
*/

namespace CL_Unit_Office
{
    public partial class WordHelper
    {
        public WordHelper()
        {
        }
        public XWPFDocument doc;
        public IList<XWPFTable> tables;//doc中的表格
        public List<XWPFTable> suntables = new List<XWPFTable>();//表格中的子表
        private string rtext = "[A-Za-z]{2,}[0-9]{0,}";
        private string rcelltext = "#\\w{1,}\\$";
        private string imgtext = "\\$img\\{.*\\}";
        //保存
        public void openWord(string filepath)
        {
            FileStream stream = new FileStream(filepath, FileMode.Open);
            doc = new XWPFDocument(stream);
            tables = doc.Tables;
            foreach (XWPFTable table in tables)
            {
                foreach (XWPFTableRow row in table.Rows)
                {
                    foreach (XWPFTableCell cell in row.GetTableCells())
                    {
                        if (cell.Tables.Count > 0)
                        {
                            foreach (XWPFTable suntable in cell.Tables)
                            {
                                suntables.Add(suntable);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 段落操作
        /// </summary>
        /// <param name="d"></param>
        public void setCellValueParagraphs(Dictionary<string, object> d)
        {
            IList<NPOI.XWPF.UserModel.XWPFParagraph> Paragraphs = doc.Paragraphs;
            foreach (NPOI.XWPF.UserModel.XWPFParagraph p in Paragraphs)
            {
                string ptext = p.ParagraphText;
                if (Regex.IsMatch(ptext, rtext))
                {
                    IList<XWPFRun> runs = p.Runs;
                    foreach (XWPFRun run in runs)
                    {
                        string runtext = run.GetText(0);
                        if (Regex.IsMatch(runtext, rtext))
                        {
                            string dvalue = runtext.Trim();
                            if (d.ContainsKey(dvalue))
                            {
                                run.SetText(d[dvalue] == null ? "" : d[dvalue].ToString(), 0);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 表格操作
        /// </summary>
        /// <param name="d"></param>
        /// <param name="tablesindex">表格索引</param>
        public void setCellValuetTables(Dictionary<string, object> d, int tablesindex)
        {
            XWPFTable table = tables[tablesindex];
            setCellValuetTables(d, table);
        }
        /// <summary>
        /// 表格操作   cell内部仅仅替换变量
        /// </summary>
        /// <param name="d"></param>
        /// <param name="table"></param>
        public void setCellValuetTables(Dictionary<string, object> d, XWPFTable table)
        {
            foreach (XWPFTableRow row in table.Rows)
            {
                foreach (XWPFTableCell cell in row.GetTableCells())
                {
                    foreach (XWPFTable table1 in cell.Tables)
                    {
                        setCellValuetTables(d, table1);
                    }
                    if (Regex.IsMatch(cell.GetText(), rtext))
                    {
                        if (cell.GetText().Split('#').Length > 1)
                        {
                            break;
                        }
                        IList<NPOI.XWPF.UserModel.XWPFParagraph> Paragraphs = cell.Paragraphs;
                        foreach (NPOI.XWPF.UserModel.XWPFParagraph p in Paragraphs)
                        {
                            foreach (XWPFRun run in p.Runs)
                            {
                                string runtext = run.GetText(0);
                                if (Regex.IsMatch(runtext == null ? "" : runtext, rtext))
                                {
                                    string dvalue = runtext.Trim();
                                    if (d.ContainsKey(dvalue))
                                    {
                                        run.SetText(d[dvalue] == null ? "" : d[dvalue].ToString(), 0);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 表格操作#$  替换整个cell
        /// </summary>
        /// <param name="d"></param>
        /// <param name="tablesindex"></param>
        public void setCellValuetTables1(Dictionary<string, object> d, int tablesindex)
        {
            XWPFTable table = tables[tablesindex];
            setCellValuetTables1(d, table);
        }
        /// <summary>
        /// 子表格操作#$
        /// </summary>
        /// <param name="d"></param>
        /// <param name="table"></param>
        public void setCellValuetTables1(Dictionary<string, object> d, XWPFTable table)
        {
            foreach (XWPFTableRow row in table.Rows)
            {
                foreach (XWPFTableCell cell in row.GetTableCells())
                {
                    if (Regex.IsMatch(cell.GetText(), rcelltext))
                    {
                        string runtext = cell.GetText().Trim();
                        if (d.ContainsKey(runtext))
                        {
                            if (d[runtext] == null) { d[runtext] = ""; }
                            if (Regex.IsMatch(d[runtext].ToString(), imgtext))
                            {//匹配图片
                             // var mat = Regex.Match(d[runtext].ToString(),imgtext);
                                cell.RemoveParagraph(0);
                                XWPFParagraph p1 = cell.AddParagraph();
                                //插入图片
                                string runvalue = d[runtext].ToString().Substring(5);
                                int width = 1200000, height = 120000;
                                if (runvalue.Substring(runvalue.Length - 1) == ")")
                                {
                                    string wh = runvalue.Substring(runvalue.LastIndexOf("("));
                                    runvalue = runvalue.Substring(0, runvalue.LastIndexOf(wh) - 1);
                                    string[] whs = wh.Replace("(", "").Replace(")", "").Split(',');
                                    width = Convert.ToInt32(whs[0]); height = Convert.ToInt32(whs[1]);
                                }
                                else
                                {
                                    runvalue = runvalue.Substring(0, runvalue.Length - 1);
                                }
                                FileStream gfs = new FileStream(runvalue, FileMode.Open, FileAccess.ReadWrite);
                                XWPFRun r1 = p1.CreateRun();
                                r1.AddPicture(gfs, (int)NPOI.XWPF.UserModel.PictureType.PNG, runtext + "png", width, height);
                                gfs.Close();
                            }
                            else
                            {//匹配字符
                                cell.RemoveParagraph(0);
                                //1
                                //cell.SetText(d[runtext].ToString());
                                //2

                                string[] texts = d[runtext].ToString().Replace("\r\n", "$").Split('$');
                                for (int m = 0; m < texts.Length; m++)
                                {
                                    XWPFParagraph p = cell.AddParagraph();
                                    XWPFRun r = p.CreateRun();
                                    r.SetText(texts[m]);
                                }
                                if (d[runtext].ToString() == "") { cell.SetText(d[runtext].ToString()); }
                            }
                            //XWPFParagraph p1 = cell.Paragraphs[0];
                            //int runlength = p1.Runs.Count;
                            //for (int i = 0; i < runlength; i++)
                            //{
                            //    p1.Runs[i].ReplaceText(p1.Runs[i].Text, "参数3的内容"+(char)13+"换行");
                            //    //p1.RemoveRun(0);
                            //}
                            //XWPFRun r1 = p1.CreateRun();
                            //r1.SetText(d[runtext] == null ? "" : d[runtext].ToString());
                            //r1.FontSize = 10;
                            //r1.ReplaceText("1","参数3的内容\r换行");
                            //r1.SetBold(false);
                            //r1.SetFontFamily("Courier");
                            //r1.SetUnderline(UnderlinePatterns.DotDotDash);
                            //r1.SetTextPosition(100); 
                            //cell.SetText(d[cell.GetText().Trim()].ToString());
                        }
                        //IList<NPOI.XWPF.UserModel.XWPFParagraph> Paragraphs = cell.Paragraphs;
                        //foreach (NPOI.XWPF.UserModel.XWPFParagraph p in Paragraphs)
                        //{
                        //    foreach (XWPFRun run in p.Runs)
                        //    {
                        //        string runtext = run.GetText(0);
                        //        if (Regex.IsMatch(runtext, rtext))
                        //        {
                        //            string dvalue = runtext.Trim();
                        //            if (d.ContainsKey(dvalue))
                        //            {
                        //                run.SetText(d[dvalue].ToString(), 0);
                        //            }
                        //        }
                        //    }
                        //}
                    }
                }
            }
        }
        /// <summary>
        /// 创建表格操作
        /// </summary>
        /// <param name="d"></param>
        /// <param name="table">表格</param>
        public void setCellValuetTables(DataSet ds, int startrowindex, XWPFTable table)
        {
            //设置顺序
            Dictionary<int, int> cellindexs = new Dictionary<int, int>();
            XWPFTableRow templaterow = table.GetRow(startrowindex);
            int i = 0;
            foreach (XWPFTableCell cell in templaterow.GetTableCells())
            {
                string key = cell.GetText().Trim();
                cellindexs.Add(i, ds.Tables[0].Columns.IndexOf(key));
                i++;
            }
            //删除模板行
            table.RemoveRow(startrowindex);
            //开始赋值
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                XWPFTableRow row = table.CreateRow();
                int m = 0;
                foreach (XWPFTableCell cell in row.GetTableCells())
                {
                    int cellindex = i;
                    if (cellindexs[m] >= 0)
                    {//未找到则不赋值
                        cell.SetText(item[cellindexs[m]].ToString());
                    }
                    m++;
                }
            }

        }
        /// <summary>
        /// 得到单元格
        /// </summary>
        /// <param name="cellid"></param>
        /// <returns></returns>
        public XWPFTableCell getCellObject(string cellid)
        {
            XWPFTableCell rcell = null;
            foreach (XWPFTable table in tables)
            {
                foreach (XWPFTableRow row in table.Rows)
                {
                    foreach (XWPFTableCell cell in row.GetTableCells())
                    {
                        string celltext = cellid.IndexOf("#") < 0 ? ("#" + cellid + "$") : cellid;
                        if (cell.GetText() == celltext)
                        {
                            rcell = cell;
                            break;
                        }
                    }
                }
            }
            return rcell;
        }
        /// <summary>
        /// 得到单元格无#$
        /// </summary>
        /// <param name="cellid"></param>
        /// <returns></returns>
        public XWPFTableCell getCellObject1(string cellid)
        {
            XWPFTableCell rcell = null;
            foreach (XWPFTable table in tables)
            {
                foreach (XWPFTableRow row in table.Rows)
                {
                    foreach (XWPFTableCell cell in row.GetTableCells())
                    {
                        if (cell.GetText() == cellid)
                        {
                            rcell = cell;
                            break;
                        }
                    }
                }
            }
            return rcell;
        }
        /// <summary>
        /// 画斜线
        /// </summary>
        /// <param name="d"></param>
        /// <param name="table"></param>
        public void setCellline(Dictionary<string, string> d, int tablesindex, string imgsrc)
        {
            XWPFTable table = tables[tablesindex];
            int i = 0;
            foreach (XWPFTableRow row in table.Rows)
            {
                foreach (XWPFTableCell cell in row.GetTableCells())
                {
                    if (Regex.IsMatch(cell.GetText(), rcelltext) || Regex.IsMatch(cell.GetText(), rtext))
                    {
                        string runtext = cell.GetText().Trim();
                        if (d.ContainsKey(runtext))
                        {
                            XWPFParagraph p1 = cell.Paragraphs[0];
                            if (d[runtext] == "1")
                            {
                                i++;
                                FileStream gfs = new FileStream(imgsrc, FileMode.Open, FileAccess.ReadWrite);
                                XWPFRun r1 = p1.CreateRun();
                                r1.AddPicture(gfs, (int)NPOI.XWPF.UserModel.PictureType.PNG, i.ToString() + "png", 1200000, 120000);
                                gfs.Close();
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 画印章
        /// </summary>
        /// <param name="d"></param>
        /// <param name="table"></param>
        public void setCellseal(Dictionary<string, string> d, int tablesindex, string imgsrc)
        {
            XWPFTable table = tables[tablesindex];
            int i = 0;
            foreach (XWPFTableRow row in table.Rows)
            {
                foreach (XWPFTableCell cell in row.GetTableCells())
                {
                    if (Regex.IsMatch(cell.GetText(), rcelltext) || Regex.IsMatch(cell.GetText(), rtext))
                    {
                        string runtext = cell.GetText().Trim();
                        if (d.ContainsKey(runtext))
                        {
                            XWPFParagraph p1 = cell.Paragraphs[0];
                            FileStream gfs = new FileStream(imgsrc, FileMode.Open, FileAccess.ReadWrite);
                            XWPFRun r1 = p1.CreateRun();
                            r1.AddPicture(gfs, (int)NPOI.XWPF.UserModel.PictureType.PNG, i.ToString() + "png", 1000000, 500000);
                            gfs.Close();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 表格循环操作
        /// </summary>
        /// <param name="d"></param>
        /// <param name="tablesindex">表格索引</param>
        public void setCellValuetDataList(DataSet ds, int tablesindex)
        {
            DataTable ds_table = ds.Tables[0];
            XWPFTable table = tables[tablesindex];
            foreach (XWPFTableRow row in table.Rows)
            {
                foreach (XWPFTableCell cell in row.GetTableCells())
                {
                    MatchCollection matchCol = Regex.Matches(cell.GetText(), rcelltext, RegexOptions.Multiline);
                    if (matchCol.Count > 0)
                    {
                        foreach (Match item in matchCol)
                        {
                            string key = item.Value.Replace("#", "").Replace("$", "");
                            foreach (DataTable ds_tableitem in ds.Tables)
                            {
                                if (ds_tableitem.Columns.Contains(key))
                                {
                                    foreach (DataRow dsrowitem in ds_tableitem.Rows)
                                    {
                                        table.CreateRow();
                                    }
                                    break;
                                }
                            }

                        }
                    }
                }
            }
        }
        public void SaveToFile(string fileName)
        {
            string path = fileName.Substring(0, fileName.LastIndexOf("\\"));
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            MemoryStream ms = new MemoryStream();
            doc.Write(ms);
            ms.Flush();
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();

                fs.Write(data, 0, data.Length);
                fs.Flush();
                data = null;
            }
        }
    }
}