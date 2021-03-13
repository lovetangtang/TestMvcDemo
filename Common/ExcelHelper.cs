using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace Common
{
    public class ExcelHelper
    {
        private static MemoryStream WriteToStream(HSSFWorkbook hssfworkbook)
        {
            //Write the stream data of workbook to the root directory
            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);
            return file;
        }

        //Export(DataTable table, string headerText, string sheetName, string[] columnName, string[] columnTitle)
        /// <summary>
        /// 向客户端输出文件(枚举需要sql进行转换)
        /// </summary>
        /// <param name="table">数据表。</param>
        /// <param name="headerText">头部文本。</param>
        /// <param name="sheetName"></param>
        /// <param name="columnName">数据列名称。</param>
        /// <param name="columnTitle">表标题。</param>
        /// <param name="fileName">文件名称。</param>
        public static FileContentResult Write(DataTable table, string headerText, string sheetName, string[] columnName, string[] columnTitle, string fileName
            , string[] roles = null, string[] photoStatus = null, string[] hospitals = null, string[] creators = null)
        {
            HSSFWorkbook hssfworkbook = GenerateData(table, headerText, sheetName, columnName, columnTitle, roles, photoStatus, hospitals, creators);
            var filec = new FileContentResult(WriteToStream(hssfworkbook).GetBuffer(), "application/ms-excel");
            filec.FileDownloadName = fileName;
            return filec;
        }

        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="table"></param>
        /// <param name="subject"></param>
        /// <param name="sheetName"></param>
        /// <param name="columnName"></param>
        /// <param name="columnTitle"></param>
        /// <returns></returns>
        public static HSSFWorkbook GenerateData(DataTable table, string subject, string sheetName, string[] columnName, string[] columnTitle
            , string[] roles = null, string[] photoStatus = null, string[] hospitals = null, string[] creators = null)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            ISheet sheet = hssfworkbook.CreateSheet(sheetName);

            if (subject == "客资导入模板")
            {
                CellRangeAddressList regions = new CellRangeAddressList(1, 65535, 4, 4);
                DVConstraint constraint = DVConstraint.CreateExplicitListConstraint(roles);
                HSSFDataValidation dataValidate = new HSSFDataValidation(regions, constraint);
                sheet.AddValidationData(dataValidate);

                //CellRangeAddressList regions1 = new CellRangeAddressList(1, 65535, 5, 5);
                //DVConstraint constraint1 = DVConstraint.CreateExplicitListConstraint(hospitals);
                //HSSFDataValidation dataValidate1 = new HSSFDataValidation(regions1, constraint1);
                //sheet.AddValidationData(dataValidate1);
                CreateDropDownListForExcel(sheet, hospitals, 1, 500, 6);

                CellRangeAddressList regions4 = new CellRangeAddressList(1, 65535, 12, 12);
                DVConstraint constraint4 = DVConstraint.CreateExplicitListConstraint(photoStatus);
                HSSFDataValidation dataValidate4 = new HSSFDataValidation(regions4, constraint4);
                sheet.AddValidationData(dataValidate4);

                CellRangeAddressList regions2 = new CellRangeAddressList(1, 65535, 13, 13);
                DVConstraint constraint2 = DVConstraint.CreateExplicitListConstraint(new string[] { "已领取", "未领取" });
                HSSFDataValidation dataValidate2 = new HSSFDataValidation(regions2, constraint2);
                sheet.AddValidationData(dataValidate2);

                CellRangeAddressList regions3 = new CellRangeAddressList(1, 65535, 14, 14);
                DVConstraint constraint3 = DVConstraint.CreateExplicitListConstraint(new string[] { "正常", "黑名单" });
                HSSFDataValidation dataValidate3 = new HSSFDataValidation(regions3, constraint3);
                sheet.AddValidationData(dataValidate3);

                //CellRangeAddressList regions5 = new CellRangeAddressList(1, 65535, 8, 8);
                //DVConstraint constraint5 = DVConstraint.CreateExplicitListConstraint(creators);
                //HSSFDataValidation dataValidate5 = new HSSFDataValidation(regions5, constraint5);
                //sheet.AddValidationData(dataValidate5);
                CreateDropDownListForExcel(sheet, creators, 1, 500, 9);

                CellRangeAddressList regions5 = new CellRangeAddressList(1, 65535, 15, 15);
                DVConstraint constraint5 = DVConstraint.CreateExplicitListConstraint(new string[] { "是", "否" });
                HSSFDataValidation dataValidate5 = new HSSFDataValidation(regions5, constraint5);
                sheet.AddValidationData(dataValidate5);

                CellRangeAddressList regions6 = new CellRangeAddressList(1, 65535, 2, 2);
                DVConstraint constraint6 = DVConstraint.CreateExplicitListConstraint(new string[] { "备孕中", "孕妈妈", "宝妈妈" });
                HSSFDataValidation dataValidate6 = new HSSFDataValidation(regions6, constraint6);
                sheet.AddValidationData(dataValidate6);
            }

            ICellStyle dateStyle = hssfworkbook.CreateCellStyle();
            IDataFormat format = hssfworkbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd hh:mm:ss");

            #region 取得列宽

            int[] colWidth = new int[columnName.Length];
            for (int i = 0; i < columnName.Length; i++)
            {
                colWidth[i] = Encoding.GetEncoding(936).GetBytes(columnTitle[i]).Length;
            }
            if (table != null && table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    for (int j = 0; j < columnName.Length; j++)
                    {
                        int intTemp = Encoding.GetEncoding(936).GetBytes(table.Rows[i][columnName[j]].ToString()).Length;
                        if (intTemp > colWidth[j])
                        {
                            colWidth[j] = intTemp;
                        }
                    }
                }
            }

            #endregion

            int rowIndex = 0;
            if (table == null || table.Rows.Count == 0)
            {
                IRow headerRow;
                headerRow = sheet.CreateRow(0);
                ICellStyle headStyle = hssfworkbook.CreateCellStyle();
                headStyle.Alignment = HorizontalAlignment.Center;
                IFont font = hssfworkbook.CreateFont();
                font.FontHeightInPoints = 12;
                font.Boldweight = 700;
                headStyle.SetFont(font);

                for (int i = 0; i < columnName.Length; i++)
                {
                    headerRow.CreateCell(i).SetCellValue(columnTitle[i]);
                    headerRow.GetCell(i).CellStyle = headStyle;
                    //设置列宽
                    sheet.SetColumnWidth(i, (colWidth[i] + 1) * 256 + 2 * 256);
                }
            }
            else
            {
                foreach (DataRow row in table.Rows)
                {
                    #region 新建表，填充表头，填充列头，样式
                    if (rowIndex == 65535 || rowIndex == 0)
                    {
                        if (rowIndex != 0)
                        {
                            sheet = hssfworkbook.CreateSheet(sheetName + ((int)rowIndex / 65535).ToString());
                        }

                        #region 列头及样式
                        {
                            IRow headerRow;
                            headerRow = sheet.CreateRow(0);
                            ICellStyle headStyle = hssfworkbook.CreateCellStyle();
                            headStyle.Alignment = HorizontalAlignment.Center;
                            IFont font = hssfworkbook.CreateFont();
                            font.FontHeightInPoints = 12;
                            font.Boldweight = 700;
                            headStyle.SetFont(font);

                            for (int i = 0; i < columnName.Length; i++)
                            {
                                headerRow.CreateCell(i).SetCellValue(columnTitle[i]);
                                headerRow.GetCell(i).CellStyle = headStyle;
                                //设置列宽

                                if (columnName[i].Contains("ItemPictureURL"))
                                {
                                    sheet.SetColumnWidth(i, 60 * 256);
                                }
                                else
                                {
                                    sheet.SetColumnWidth(i, (colWidth[i] + 1) * 256 + 2 * 256);
                                }
                            }
                        }
                        #endregion
                        rowIndex = 1;
                    }
                    #endregion

                    #region 填充数据

                    IRow dataRow = sheet.CreateRow(rowIndex);
                    for (int i = 0; i < columnName.Length; i++)
                    {
                        ICell newCell = dataRow.CreateCell(i);

                        string drValue = row[columnName[i]].ToString();

                        #region 赋值

                        switch (table.Columns[columnName[i]].DataType.ToString())
                        {
                            case "System.String"://字符串类型  
                                if (drValue.ToUpper() == "TRUE")
                                    newCell.SetCellValue("是");
                                else if (drValue.ToUpper() == "FALSE")
                                    newCell.SetCellValue("否");
                                newCell.SetCellValue(drValue);
                                break;
                            case "System.DateTime"://日期类型   
                                if (string.IsNullOrWhiteSpace(drValue))
                                {
                                    newCell.SetCellValue("");
                                    break;
                                }
                                DateTime dateV;
                                bool flag = DateTime.TryParse(drValue, out dateV);
                                if (flag)
                                {
                                    newCell.SetCellValue(dateV);
                                    newCell.CellStyle = dateStyle;//格式化显示   
                                }
                                break;
                            case "System.Boolean"://布尔型   
                                bool boolV = false;
                                bool.TryParse(drValue, out boolV);
                                if (boolV)
                                    newCell.SetCellValue("是");
                                else
                                    newCell.SetCellValue("否");
                                break;
                            case "System.Int16"://整型   
                            case "System.Int32":
                                if (string.IsNullOrWhiteSpace(drValue))
                                {
                                    newCell.SetCellValue("");
                                    break;
                                }
                                int intV = 0;
                                int.TryParse(drValue, out intV);
                                newCell.SetCellValue(intV);
                                break;
                            case "System.Int64":
                                newCell.SetCellValue(drValue);
                                break;
                            case "System.Byte":
                                newCell.SetCellValue(drValue);
                                break;
                            case "System.Decimal"://浮点型   
                            case "System.Double":
                            case "System.Single":
                                double doubV = 0;
                                double.TryParse(drValue, out doubV);
                                newCell.SetCellValue(doubV);
                                break;
                            case "System.DBNull"://空值处理   
                                newCell.SetCellValue("");
                                break;
                            default:
                                newCell.SetCellValue(drValue);
                                break;
                        }

                        #endregion
                    }

                    #endregion

                    rowIndex++;
                }
            }

            return hssfworkbook;
        }


        private static void CreateDropDownListForExcel(ISheet sheet, string[] dropDownValues, int startRow, int lastRow, int column)
        {
            if (sheet == null)
            {
                return;
            }
            IWorkbook workbook = sheet.Workbook;
            string dropDownName = sheet.SheetName + "DropDownValuesForColumn" + column;
            ISheet hiddenSheet = workbook.CreateSheet(dropDownName);
            for (int i = 0, length = dropDownValues.Length; i < length; i++)
            {
                string name = dropDownValues[i];
                IRow row = hiddenSheet.CreateRow(i);
                ICell cell = row.CreateCell(0);
                cell.SetCellValue(name);
            }
            IName namedCell = workbook.CreateName();
            namedCell.NameName = dropDownName;
            namedCell.RefersToFormula = (dropDownName + "!$A$1:$A$" + dropDownValues.Length);
            HSSFDataValidationHelper dvHelper = new HSSFDataValidationHelper(sheet as HSSFSheet);
            IDataValidationConstraint dvConstraint = dvHelper.CreateFormulaListConstraint(dropDownName);
            CellRangeAddressList addressList = new CellRangeAddressList(startRow, lastRow, column, column);
            HSSFDataValidation validation = (HSSFDataValidation)dvHelper.CreateValidation(dvConstraint, addressList);
            int hiddenSheetIndex = workbook.GetSheetIndex(hiddenSheet);
            workbook.SetSheetHidden(hiddenSheetIndex, SheetState.Hidden);
            sheet.AddValidationData(validation);
        }


        /// <summary>
        /// 将xls文件流读取为datatable
        /// </summary>
        /// <param name="stream"></param>
        public static DataTable ReadXlsToDataTable(Stream stream)
        {
            DataTable dt = new DataTable();
            IWorkbook hssfworkbook = new HSSFWorkbook(stream);
            ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            HSSFRow headerRow = (HSSFRow)sheet.GetRow(0);

            //一行最后一个方格的编号 即总的列数 
            for (int j = 0; j < (sheet.GetRow(0).LastCellNum); j++)
            {
                //SET EVERY COLUMN NAME
                HSSFCell cell = (HSSFCell)headerRow.GetCell(j);

                dt.Columns.Add(cell.ToString());
            }

            while (rows.MoveNext())
            {
                IRow row = (HSSFRow)rows.Current;
                DataRow dr = dt.NewRow();

                if (row.RowNum == 0) continue;//The firt row is title,no need import

                for (int i = 0; i < row.LastCellNum; i++)
                {
                    ICell cell = row.GetCell(i);

                    if (cell == null)
                    {
                        dr[i] = null;
                    }
                    if (cell.CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(cell))
                    {
                        dr[i] = cell.DateCellValue;
                    }
                    else
                    {
                        dr[i] = cell.ToString();
                    }
                }

                dt.Rows.Add(dr);
            }
            return dt;
        }

        #region Read
        public static DataTable GetExcelDataTable(string filePath)
        {
            IWorkbook Workbook;
            DataTable table = new DataTable();
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    //XSSFWorkbook 适用XLSX格式，HSSFWorkbook 适用XLS格式
                    string fileExt = Path.GetExtension(filePath).ToLower();
                    if (fileExt == ".xls")
                    {
                        Workbook = new HSSFWorkbook(fileStream);
                    }
                    else if (fileExt == ".xlsx")
                    {
                        Workbook = new XSSFWorkbook(fileStream);
                    }
                    else
                    {
                        Workbook = null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //定位在第一个sheet
            ISheet sheet = Workbook.GetSheetAt(0);
            //第一行为标题行
            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;
            int rowCount = sheet.LastRowNum;

            //循环添加标题列
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }

            //数据
            for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = table.NewRow();
                if (row != null)
                {
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            dataRow[j] = GetCellValue(row.GetCell(j));
                        }
                    }
                }
                table.Rows.Add(dataRow);
            }
            return table;
        }

        private static string GetCellValue(ICell cell)
        {
            if (cell == null)
            {
                return string.Empty;
            }

            switch (cell.CellType)
            {
                case CellType.Blank:
                    return string.Empty;
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Error:
                    return cell.ErrorCellValue.ToString();
                case CellType.Numeric:
                    return HSSFDateUtil.IsCellDateFormatted(cell) ? cell.DateCellValue.ToString("yyyy-MM-dd") : cell.NumericCellValue.ToString();
                case CellType.Unknown:
                default:
                    return cell.ToString();
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Formula:
                    try
                    {
                        HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                        e.EvaluateInCell(cell);
                        return cell.ToString();
                    }
                    catch
                    {
                        return cell.NumericCellValue.ToString();
                    }
            }
        }

        #endregion

        #region Write
        public static MemoryStream Export(DataTable data, string fileName)
        {
            HSSFWorkbook book = new HSSFWorkbook();
            ISheet sheet = book.CreateSheet(fileName);

            CreaHeader(data, sheet);

            if (data != null)
            {
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    IRow row = sheet.CreateRow(sheet.LastRowNum + 1);
                    DataRow dataRow = data.Rows[i];

                    for (int j = 0; j < data.Columns.Count; j++)
                    {
                        ICell cell = row.CreateCell(j);
                        cell.SetCellValue(ConvertHelper.ToString(dataRow[j]));
                    }
                }
            }

            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);

            return ms;
        }

        public static void CreaHeader(DataTable data, ISheet sheet)
        {
            IRow row = sheet.CreateRow(sheet.LastRowNum);

            if (data != null)
            {
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    ICell cell = row.CreateCell(i);
                    cell.SetCellValue(data.Columns[i].ColumnName);
                }
            }
        }
        #endregion
    }
}
