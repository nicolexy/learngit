using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace commLib
{
    /// <summary>
    /// 导出Excel
    /// </summary>
    public class NPOIExportToExcel : ExcelHelper
    {
        /// <summary>
        /// 数据源
        /// </summary>
        public DataTable dt { get; set; }
        /// <summary>
        /// 要导出的Excel名称
        /// </summary>
        public string excelName { get; set; }
        /// <summary>
        /// 工作区域名称 如：sheet1
        /// </summary>
        public string sheetName { get; set; }
        /// <summary>
        /// 大标题
        /// </summary>
        public string lineTitle { get; set; }
        /// <summary>
        /// 隐藏列集合
        /// </summary>
        public IList<int> hiddenColumnIndexs { get; set; }
        //public Guid CustomerID { get; set; }
        /// <summary>
        /// 导出Excel的Excel名称
        /// </summary>
        protected override string FileName
        {
            get
            {
                string fName = HttpUtility.UrlEncode(excelName, System.Text.Encoding.UTF8);
                return "" + fName + ".xls";
            }
        }


        protected override void GenerateData()
        {
            int nextRowStartIndex = 0;//开始编辑行的索引
            ISheet sheet1 = hssfworkbook.CreateSheet(sheetName);

            SetColumnWidth(sheet1);//设置列宽

            int columnNameStartIndex=SetBigTitle(sheet1);//设置表格大标题

            int rowDataStartIndex = SetColumnName(sheet1, columnNameStartIndex);//设置列名

            SetRowValue(sheet1, rowDataStartIndex);


            SetDataTypeValidata(sheet1);
            //SetRegion(sheet1);
            if (hiddenColumnIndexs != null && hiddenColumnIndexs.Count > 0 && hiddenColumnIndexs.Count > 0)
            {
                SetColumnHidden(sheet1);
            }
        }

        /// <summary>
        /// 设置表格单元格的值
        /// </summary>
        /// <param name="sheet1"></param>
        private void SetRowValue(ISheet sheet1, int rowDataStartIndex)
        {
            int index = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row = sheet1.CreateRow(index + rowDataStartIndex);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string value = dt.Rows[i][j].ToString();
                    row.CreateCell(j).SetCellValue(value);
                }
                index++;
            }
        }
        /// <summary>
        /// 将数据验证添加到下载的Excel模板中
        /// </summary>
        /// <param name="sheet1"></param>
        public virtual void SetDataTypeValidata(ISheet sheet1)
        {
            ////建立币种下拉框数据源
            //ISheet sheet2 = hssfworkbook.CreateSheet("CurrencyType");
            //IList<Currency> currencies = Currency.GetAll().Where(p => p.Enable = true).ToList();
            //int currencyCount = 0;
            //foreach (var currency in currencies)
            //{
            //    sheet2.CreateRow(currencyCount++).CreateCell(0).SetCellValue(currency.Name.ToString());
            //}

            //HSSFName range = (HSSFName)hssfworkbook.CreateName();
            //range.RefersToFormula = "CurrencyType!$A$1:$A$" + currencies.Count + "";
            //range.NameName = "dicRange";

            //CellRangeAddressList regions4 = new CellRangeAddressList(2, sheet1.LastRowNum, 2, 2);
            //DVConstraint constraint4 = DVConstraint.CreateFormulaListConstraint(range.NameName);
            //HSSFDataValidation dataValidate4 = new HSSFDataValidation(regions4, constraint4);
            //((HSSFSheet)sheet1).AddValidationData(dataValidate4);

            //CellRangeAddressList regions5 = new CellRangeAddressList(2, sheet1.LastRowNum, 3, 3);
            //DVConstraint constraint5 = DVConstraint.CreateNumericConstraint(DVConstraint.ValidationType.DECIMAL, DVConstraint.OperatorType.NOT_EQUAL, "0.00", "100000.00");
            //HSSFDataValidation dataValidate5 = new HSSFDataValidation(regions5, constraint5);
            //dataValidate5.CreateErrorBox("error", "报价应为小数格式！");
            //((HSSFSheet)sheet1).AddValidationData(dataValidate5);
        }

        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="sheet1"></param>
        /// <param name="startRowNum"></param>
        /// <param name="startCellNum"></param>
        /// <param name="endRowNum"></param>
        /// <param name="endCellNum"></param>
        private static void SetRegion(ISheet sheet1)
        {
            for (int r = 2; r <= sheet1.LastRowNum; r++)
            {
                for (int c = 0; c < sheet1.GetRow(r).Cells.Count; c++)
                {
                    int index = 0;
                    try
                    {
                        if ((c == 0) && (sheet1.GetRow(r).GetCell(c).StringCellValue.ToString() == sheet1.GetRow(r + 1).GetCell(c).StringCellValue.ToString()))
                        {
                            index = r;
                            index++;
                            sheet1.AddMergedRegion(new CellRangeAddress(r, index, c, c));
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        /// <summary>
        /// 设置列宽
        /// </summary>
        /// <param name="sheet1"></param>
        private void SetColumnWidth(ISheet sheet1)
        {            
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sheet1.SetColumnWidth(i, 5000);
            }
        }

        /// <summary>
        /// 设置Excel列名
        /// </summary>
        /// <param name="sheet1">Excel表</param>
        /// <param name="ExcelColumnName">列名集合</param>
        private int SetColumnName(ISheet sheet1, int columnNameStartIndex)
        {
            int rowDataStartIndex = 0;
            ICell cell;
            IRow row = sheet1.CreateRow(columnNameStartIndex);//1
            rowDataStartIndex = columnNameStartIndex + 1;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                cell = row.CreateCell(i);
                cell.SetCellValue(dt.Columns[i].ColumnName);
            }
            return rowDataStartIndex;
        }

        private int SetBigTitle(ISheet sheet1)
        {
            int nextRowStartIndex = 0;
            if (!string.IsNullOrEmpty(lineTitle) && lineTitle.Length > 0)
            {
                IRow row = sheet1.CreateRow(0);
                row.Height = 800;
                ICell cell = row.CreateCell(0);
                cell.SetCellValue(lineTitle);
                ICellStyle style = hssfworkbook.CreateCellStyle();
                style.Alignment = HorizontalAlignment.Center;
                style.VerticalAlignment = VerticalAlignment.Center;
                IFont font = hssfworkbook.CreateFont();
                font.FontHeight = 20 * 20;
                style.SetFont(font);
                cell.CellStyle = style;
                sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, dt.Columns.Count - 1));
                nextRowStartIndex = 1;
            }
            return nextRowStartIndex;            
        }
        /// <summary>
        /// 隐藏列
        /// </summary>
        /// <param name="sheet1"></param>
        /// <param name="indexColumn"></param>
        private void SetColumnHidden(ISheet sheet1)
        {
            for (int i = 0; i < hiddenColumnIndexs.Count; i++)
            {
                sheet1.SetColumnHidden(int.Parse(hiddenColumnIndexs[i].ToString()), true);
            }
        }
    }
}
