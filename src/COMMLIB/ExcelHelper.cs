using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;

namespace commLib
{
    public abstract class ExcelHelper
    {
        protected const string Subject = "";
        protected const string Company = "腾讯";
        protected abstract string FileName { get; }
        /// <summary>
        /// 导入Excel的错误信息
        /// </summary>
        public string ErrorMessage { get; set; }


        /// <summary>
        /// Excel控制对象
        /// </summary>
        protected HSSFWorkbook hssfworkbook;

        MemoryStream WriteToStream()
        {
            //Write the stream data of workbook to the root directory
            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);
            return file;
        }

        /// <summary>
        /// 导入导出方法的具体实现
        /// </summary>
        protected abstract void GenerateData();

        /// <summary>
        /// 导出Excel文件
        /// </summary>
        public void ExportExcel()
        {
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", FileName));
            HttpContext.Current.Response.Clear();

            InitializeWorkbook();
            GenerateData();
            //WriteToStream().WriteTo(HttpContext.Current.Response.OutputStream);
            HttpContext.Current.Response.BinaryWrite(WriteToStream().GetBuffer());
            HttpContext.Current.Response.End();
        }

        void InitializeWorkbook()
        {
            hssfworkbook = new HSSFWorkbook();

            ////create a entry of DocumentSummaryInformation
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = Company;
            hssfworkbook.DocumentSummaryInformation = dsi;

            ////create a entry of SummaryInformation
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = Subject;
            hssfworkbook.SummaryInformation = si;
        }

        void ImportExcel(string excelPath)
        {
            try
            {
                InitializeWorkbook(excelPath);
                GenerateData();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                File.Delete(excelPath);
            }

        }

        void InitializeWorkbook(string path)
        {
            //read the template via FileStream, it is suggested to use FileAccess.Read to prevent file lock.
            //book1.xls is an Excel-2007-generated file, so some new unknown BIFF records are added. 
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }
        }

        /// <summary>
        /// 上传导入Excel
        /// </summary>
        /// <param name="file">上传文件</param>
        /// <returns>上传导入过程中的错误信息</returns>
        public string UploadExcel(HttpPostedFileBase file)
        {
            string fileName = file.FileName;
            string endName = fileName.Substring(fileName.LastIndexOf('.'));
            if (endName == ".xls" || endName == ".xlsx")
            {
                string filePath = HttpContext.Current.Server.MapPath("~/UploadFiles/LoanUpload") + Guid.NewGuid().ToString() + endName;
                file.SaveAs(filePath);
                ImportExcel(filePath);
            }
            else
                ErrorMessage += "<br />*请上传Excel格式文件!";
            return ErrorMessage;
        }
    }
}
