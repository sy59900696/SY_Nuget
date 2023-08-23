using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;

namespace StaticCode
{
    /// <summary>
    /// 针对于DataGridView的表头，该取什么值。
    /// </summary>
    public enum E_ExcelHeadType
    {
        /// <summary>
        /// 表头
        /// </summary>
        HeadText = 0,

        /// <summary>
        /// Name
        /// </summary>
        /// 
        Name = 1,
        /// <summary>
        /// DataPropertyName
        /// </summary>
        DataPropertyName = 2
    }

    /// <summary>
    /// 对Excel的导入导出
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// 导出当前List到excel
        /// </summary>
        /// <param name="_lstBase"></param>
        /// <param name="sSheetname">工作表名</param>
        /// <param name="_FileName">文件绝对路径。若为空，则自动生成_FileName = string.Format("导出excel_{0:yyyyMMddHHmmssfff}.xls");</param>
        /// <returns></returns>
        public static void ExportExcel<T>(List<T> _lstBase, string sSheetname = "Test001", string _FileName = "") where T : class
        {
            try
            {
                Logger.Debug("ExportExcel()");
                IWorkbook book = new HSSFWorkbook();

                ISheet sheet = book.CreateSheet(sSheetname);

                // 添加表头
                IRow row = sheet.CreateRow(0);
                int index = 0;

                string _sJson0 = JsonConvert.SerializeObject(_lstBase[0]);
                PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

                if (properties.Length <= 0) throw new Exception("List<T>中T的properties.Length<0");

                foreach (PropertyInfo item in properties)
                {
                    string name = item.Name;
                    ICell cell = row.CreateCell(index);
                    cell.SetCellType(CellType.String);
                    cell.SetCellValue(name);
                    index++;
                }


                // 添加数据

                for (int i = 0; i < _lstBase.Count; i++)
                {
                    index = 0;
                    row = sheet.CreateRow(i + 1);
                    T _t0 = _lstBase[i];
                    foreach (PropertyInfo item in properties)
                    {
                        string name = item.Name;
                        object value = item.GetValue(_t0, null);
                        ICell cell = row.CreateCell(index);
                        cell.SetCellType(CellType.String);
                        cell.SetCellValue(value.ToString());
                        index++;
                    }
                }
                // 写入 
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                book = null;

                if (string.IsNullOrWhiteSpace(_FileName))
                {
                    _FileName = string.Format("导出excel_{0:yyyyMMddHHmmssfff}.xls");
                }
                using (FileStream fs = new FileStream(_FileName, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }

                ms.Close();
                ms.Dispose();
                Logger.Debug("ExportExcel()完成。");
            }
            catch (Exception ex)
            {
                string _sMsg = (string.Format("ExportExcel()异常：{0}", ex.Message.ToString()));
                Logger.Debug(_sMsg);
                throw new Exception(_sMsg);
            }
        }

        ///// <summary>
        ///// 导出当前DataGridView界面上的内容到excel
        ///// </summary>
        ///// <param name="_dgvw0"></param>
        ///// <param name="sSheetname"></param>
        ///// <returns></returns>
        //public static string ExportExcel(DataGridView _dgvw0, string sSheetname = "Test001", E_ExcelHeadType _e0 = E_ExcelHeadType.HeadText, bool _IsAlowVisable = false)
        //{
        //    string _sFileName = "";
        //    try
        //    {
        //        Logger.Debug("ExportExcel()");

        //        SaveFileDialog sflg = new SaveFileDialog();
        //        sflg.Filter = "Excel(*.xls)|*.xls|Excel(*.xlsx)|*.xlsx";
        //        if (sflg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
        //        {
        //            return "";
        //        }
        //        //this.gridView1.ExportToXls(sflg.FileName);
        //        //NPOI.xs book = new NPOI.HSSF.UserModel.HSSFWorkbook();
        //        IWorkbook book = null;
        //        if (sflg.FilterIndex == 1)
        //        {
        //            book = new NPOI.HSSF.UserModel.HSSFWorkbook();
        //        }
        //        else
        //        {
        //            book = new NPOI.XSSF.UserModel.XSSFWorkbook();
        //        }

        //        NPOI.SS.UserModel.ISheet sheet = book.CreateSheet(sSheetname);

        //        // 添加表头
        //        NPOI.SS.UserModel.IRow row = sheet.CreateRow(0);
        //        int index = 0;
        //        foreach (DataGridViewColumn item in _dgvw0.Columns)
        //        {
        //            if (item.Visible)
        //            {
        //                NPOI.SS.UserModel.ICell cell = row.CreateCell(index);
        //                cell.SetCellType(NPOI.SS.UserModel.CellType.String);
        //                switch (_e0)
        //                {
        //                    case E_ExcelHeadType.HeadText:
        //                        cell.SetCellValue(item.HeaderText);
        //                        break;
        //                    case E_ExcelHeadType.Name:
        //                        cell.SetCellValue(item.Name);
        //                        break;
        //                    case E_ExcelHeadType.DataPropertyName:
        //                    default:
        //                        cell.SetCellValue(item.DataPropertyName);
        //                        break;
        //                }
        //                index++;
        //            }
        //        }

        //        // 添加数据

        //        for (int i = 0; i < _dgvw0.RowCount; i++)
        //        {
        //            index = 0;
        //            row = sheet.CreateRow(i + 1);
        //            foreach (DataGridViewColumn item in _dgvw0.Columns)
        //            {

        //                if (item.Visible)
        //                {
        //                    NPOI.SS.UserModel.ICell cell = row.CreateCell(index);
        //                    cell.SetCellType(NPOI.SS.UserModel.CellType.String);
        //                    cell.SetCellValue(_dgvw0.Rows[i].Cells[item.Index].Value.ToString());
        //                    index++;
        //                }
        //            }
        //        }
        //        // 写入 
        //        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        //        book.Write(ms);
        //        book = null;

        //        using (FileStream fs = new FileStream(sflg.FileName, FileMode.Create, FileAccess.Write))
        //        {
        //            byte[] data = ms.ToArray();
        //            fs.Write(data, 0, data.Length);
        //            fs.Flush();
        //        }

        //        ms.Close();
        //        ms.Dispose();
        //        Logger.Debug("ExportExcel()完成。");
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Debug(string.Format("ExportExcel()异常：{0}", ex.Message.ToString()));
        //    }
        //    return _sFileName;
        //}

        /// <summary>  
        /// 将excel中的数据导入到DataTable中  
        /// </summary>  
        /// <param name="fileName"></param> 
        /// <param name="sheetName">excel工作薄sheet的名称</param>  
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>  
        /// <returns>返回的DataTable</returns>  
        public static DataTable ExcelToDataTable(string fileName, string sheetName, bool isFirstRowColumn)
        {
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            try
            {
                IWorkbook workbook = null;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本  
                    workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0) // 2003版本  
                    workbook = new HSSFWorkbook(fs);

                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet  
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数  

                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }

                    //最后一列的标号  
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　  

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null  
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }
    }
}
