using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using TENCENT.OSS.C2C.Finance.BankLib;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.KF_Web.Check_WebService;
using System.Text;
using System.Xml;
using System.Threading;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CommLib;
using System.Collections.Generic;
using System.Collections;
using CFT.Apollo.Logging;

namespace TENCENT.OSS.CFT.KF.KF_Web
{
    /// <summary>
    /// PublicRes 的摘要说明。
    /// </summary>
    public class PublicRes
    {
        public static bool isLatin = false;
        //furion 20051003 
        public static string sBeginTime = "1940-01-01 00:00:00";
        public static string sEndTime = "2040-01-01 00:00:00";

        private static string f_strDatabase;
        private static string f_strDataSource;
        private static string f_strUserID;
        private static string f_strPassword;


        private static string f_strDatabase_ht;
        private static string f_strDataSource_ht;
        private static string f_strUserID_ht;
        private static string f_strPassword_ht;

        //权限系统数据库 au
        private static string f_strDatabase_au;
        private static string f_strDataSource_au;
        private static string f_strUserID_au;
        private static string f_strPassword_au;


        //业务库备机 -->修改为资料库备机
        private static string f_strDatabase_zlb;
        private static string f_strDataSource_zlb;
        private static string f_strUserID_zlb;
        private static string f_strPassword_zlb;

        //新增一个时间跨度,只查询从今天算起指定天内的数据.
        public static int PersonInfoDayCount = Int32.Parse(ConfigurationManager.AppSettings["PersonInfoDayCount"].Trim());
        public static int GROUPID = 0;

        public static string CharSet = "latin1";

        public static bool IgnoreLimitCheck = ConfigurationManager.AppSettings["IgnoreLimitCheck"].Trim().ToLower() == "true";

        public static string OutFileToUser = ConfigurationManager.AppSettings["OutFileToUser"]; //test
        static PublicRes()
        {
            f_strDatabase = "mysql";
            f_strDataSource = ConfigurationManager.AppSettings["DataSource"];
            f_strUserID = ConfigurationManager.AppSettings["UserID"];
            f_strPassword = ConfigurationManager.AppSettings["Password"];

            f_strDatabase_zlb = "mysql";
            f_strDataSource_zlb = ConfigurationManager.AppSettings["DataSource_ZLB"];
            f_strUserID_zlb = ConfigurationManager.AppSettings["UserID_ZLB"];
            f_strPassword_zlb = ConfigurationManager.AppSettings["Password_ZLB"];

            f_strDatabase_ht = "mysql";
            f_strDataSource_ht = ConfigurationManager.AppSettings["DataSource_ht"];
            f_strUserID_ht = ConfigurationManager.AppSettings["UserID_ht"];
            f_strPassword_ht = ConfigurationManager.AppSettings["Password_ht"];

            f_strDatabase_au = "mysql";
            f_strDataSource_au = ConfigurationManager.AppSettings["DataSource_au"];
            f_strUserID_au = ConfigurationManager.AppSettings["UserID_au"];
            f_strPassword_au = ConfigurationManager.AppSettings["Password_au"];


            //			f_strServerIP = ConfigurationManager.AppSettings["ServerIP"];
            //			f_iServerPort = Int32.Parse(ConfigurationManager.AppSettings["ServerPort"]);


            GROUPID = Int32.Parse(ConfigurationManager.AppSettings["GROUPID"]);

            CharSet = ConfigurationManager.AppSettings["CharSet"];
        }

        public static string GetConnString()
        {
            return GetConnString("YWB");
        }

        public static string GetConnString(string strDBType)
        {
            string sConnStr = "";

            //string connModule = "Driver=[MySQL ODBC 3.51 Driver]; Server={0}; Database={3}; UID={1}; PWD={2}; Option=3";
            //string db50list = ConfigurationManager.AppSettings["DB50List"];
            //if (db50list.IndexOf(";" + strDBType + ";") > -1)
            //{
            //    connModule = "Driver=[mysql ODBC 5.2a Driver]; Server={0}; Database={3}; UID={1}; PWD={2};charset=latin1; Option=3";
            //}

            if (strDBType.ToUpper() == "YW")
            {
                //sConnStr = String.Format(connModule,f_strDataSource,f_strUserID,f_strPassword,f_strDatabase);
                return DbConnectionString.Instance.GetConnectionString("YW");
            }
            //furion 20090610 web层的这个数据库连接只用来查询数字据字典表的，现改成资料库连接
            else if (strDBType.ToUpper() == "ZLB")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_zlb, f_strUserID_zlb, f_strPassword_zlb, f_strDatabase_zlb);
                return DbConnectionString.Instance.GetConnectionString("ZLB");
            }
            else if (strDBType.ToUpper() == "AU")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_au, f_strUserID_au, f_strPassword_au, f_strDatabase_au);
                return DbConnectionString.Instance.GetConnectionString("AU");
            }
            else if (strDBType.ToUpper() == "HT")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_ht, f_strUserID_ht, f_strPassword_ht, f_strDatabase_ht);
                return DbConnectionString.Instance.GetConnectionString("HT");
            }
            else if (strDBType.ToUpper() == "ZW")
            {
                //sConnStr = String.Format(connModule, f_strDataSource_zw, f_strUserID_zw, f_strPassword_zw, f_strDatabase_zw);
                return DbConnectionString.Instance.GetConnectionString("ZW");
            }
            return sConnStr;//.Replace("[", "{").Replace("]", "}");
        }

        public static string GetString(object aValue)
        {
            try
            {
                if (aValue != null)
                {
                    return aValue.ToString().Replace("\\", "\\\\").Replace("'", "\\'");
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }

        public static string GetCurName(string curCode)
        {
            string yunTongCurInfo = ConfigurationManager.AppSettings["YunTongCurInfo"].Trim();
            string[] curinfos = yunTongCurInfo.Split('|');
            foreach (string oneInfo in curinfos)
            {
                if (oneInfo.StartsWith(curCode + "="))
                {
                    return oneInfo.Replace(curCode + "=", "");
                }
            }

            return "未知币种（" + curCode + "）";
        }

        public static string GetDateTime(object aValue)
        {
            try
            {
                if (aValue != null)
                {
                    DateTime dt = DateTime.Parse(aValue.ToString());
                    return dt.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }

        public static string GetInt(object aValue)
        {
            try
            {
                if (aValue != null)
                {
                    int num = Int32.Parse(aValue.ToString());
                    return num.ToString();
                }
                else
                {
                    return "0";
                }
            }
            catch
            {
                return "0";
            }
        }

        //		public static string f_strServerIP;
        //		public static int f_iServerPort;
        //
        //		/// <summary>
        //		/// 验证用户名和密码
        //		/// </summary>
        //		/// <param name="LoginUserID">用户名</param>
        //		/// <param name="strPassword">密码</param>
        //		/// <returns></returns>
        //		public static Query_Service.TCreateSessionReply ValidateUser(string LoginUserID, string strPassword, string IP)
        //		{
        //			Query_Service.Query_Service qs = new Query_Service.Query_Service();
        //			
        //			return    qs.ValidUser(LoginUserID, strPassword, IP);         
        //		}
        //
        //		/// <summary>
        //		/// 验证权限.
        //		/// </summary>
        //		/// <param name="szKey">验证服务器返回的KEY</param>
        //		/// <param name="iOperId">验证服务器返回的操作ID</param>
        //		/// <param name="strRightCode">功能ID</param>
        //		/// <returns></returns>
        //		public static bool ValidateRight(string szKey, int iOperId, string strRightCode)
        //		{
        //			int itmp = 0;
        //			try
        //			{
        //				itmp = Int32.Parse(strRightCode);
        //			}
        //			catch
        //			{
        //				return false;
        //			}
        //
        //			return UserRight.ValidateRight(szKey, iOperId, GROUPID, itmp, f_strServerIP, f_iServerPort);
        //		}
        //
        //		/// <summary>
        //		/// 删除验证SESSION
        //		/// </summary>
        //		/// <param name="szKey">验证服务器返回的KEY</param>
        //		/// <param name="iOperId">验证服务器返回的操作ID</param>
        //		public static void DelLoginUser(string szKey, int iOperId)
        //		{
        //			UserRight.DelLoginUser(szKey, iOperId, f_strServerIP, f_iServerPort);
        //		}


        public static void writeSysLog(string strUserID, string ip, string type, string actionEvent, int sign, string id, string opType) //opType: 操作对象的类型
        {/*客服系统日志是和帐务记一起的，现在要分离出来，客服不记这里
			string signStr;
			
			if(sign == 1)
				signStr = "成功!";
			else
				signStr = "失败!";

			string detail = strUserID + "对" + id + "进行“" + actionEvent + "”操作。" + signStr;

			//写入日志： 用户 + 时间 + 动作类型 + 具体动作 + IP地址
			string logStr = "Insert c2c_fmdb.t_log (FUserID,FactionTime,FactionType,FActionID,FActionName,Fsign,FMemo,Fip) Values (" +
				"'" + strUserID         + "',"  + 
				" NOW() ,"        +           //取数据库时间
				"'" + type        + "',"+     //类型 tz  (调帐)   数据字典  
				"'" + id          + "',"+     //ID为标识某一操作的关键ID。如交易单的ID，查询的QQ号。
				"'" + actionEvent + "',"    + //描述：充值调整
				"'" + sign        + "',"  +   //成功失败标志  1表示成功 0表示失败
				"'" + detail        + "',"  + 
				"'" + ip          + "')" ;

			MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
			try
			{
				da.OpenConn();
				da.ExecSql(logStr);
			}
			catch(Exception e)
			{
				throw new Exception("写日志失败！请及时联系系统管理员！详细错误：" + e.Message.ToString().Replace("'","’"));
			}
			finally
			{
				da.Dispose();
			}
*/
        }

        public static string GetErrorMsg(string ExceptionMsg)
        {

            if (ExceptionMsg == null || ExceptionMsg.Trim().Length == 0) return " ";

            string resultstr = ExceptionMsg;

            //下面是捕获Soap异常的语句
            string pattern = "";

            if (ExceptionMsg.IndexOf("SoapException") > 1)
            {
                pattern = "---> [^:]+:(.*)\n";
            }
            else
            {
                pattern = "--> (.*)$";
            }

            MatchCollection mc = Regex.Matches(ExceptionMsg, pattern);

            if (mc.Count > 0)
            {
                string str = mc[0].Groups[1].Value;
                resultstr = str.Replace("'", "’").Replace("\r\n", "");
            }

            return resultstr.Replace("'", "‘").Replace("\r", " ").Replace("\n", " ");


        }

        public static string ExecuteOne(string sqlStr, string dbStr) //查询单个结果
        {
            MySqlAccess da = null;
            try
            {
                da = new MySqlAccess(PublicRes.GetConnString(dbStr));  //连接数据库
                da.OpenConn();
                return da.GetOneResult(sqlStr);
            }
            finally
            {
                da.Dispose();
            }
        }


        public static DataSet returnDSAll(string strCmd, string dbStr)
        {
            MySqlAccess da = null;

            try
            {
                da = new MySqlAccess(PublicRes.GetConnString(dbStr));  //连接数据库类型
                da.OpenConn();
                return da.dsGetTotalData(strCmd);
            }
            finally
            {
                da.Dispose();
            }
        }

        public static bool sendMail(string mailToStr, string mailFromStr, string subject, string content)  //发送邮件
        {
            if (PublicRes.IgnoreLimitCheck)
                return true;

            try
            {
                TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend newMail = new TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend();
                newMail.SendMail(mailToStr, "", subject, content, false, null);

                //				MailMessage mail = new MailMessage();
                //				mail.From = mailFromStr;        //发件人
                //				mail.To   = mailToStr;          //收件人
                //				//mail.BodyEncoding = System.Text.Encoding.Unicode;
                //				mail.BodyFormat = MailFormat.Text;
                //				mail.Body = content; //邮件内容
                //				mail.Priority = MailPriority.High; //优先级
                //				mail.Subject  = subject;           //邮件主题
                //
                //				SmtpMail.SmtpServer = ConfigurationManager.AppSettings["smtpServer"].ToString(); //"192.168.1.27";  邮件服务器地址
                //
                //				SmtpMail.Send(mail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static string strNowTime
        {
            get
            {
                MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("HT"));
                try
                {
                    da.OpenConn();
                    string tmp = da.GetOneResult("select now()");
                    return "'" + tmp + "'";
                }
                catch
                {
                    string tmp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    return "'" + tmp + "'";
                }
                finally
                {
                    da.Dispose();
                }
            }
        }

        //查询数据库中的配置值
        public static string GetZWDicValue(string key)
        {
            BatchPay_Service.BatchPay_Service bs = new TENCENT.OSS.CFT.KF.KF_Web.BatchPay_Service.BatchPay_Service();
            return bs.GetZWDicValueByKey(key);
        }

        public static int FtpUploadFile(string ip, string uname, string upwd, string path, string filename)
        {
            FileInfo fi = new FileInfo(path);
            if (!fi.Exists)
            {
                return -1;
            }

            //string ip = "172.25.39.54";
            // string uname = "cgi_ceppact";
            //string upwd = "cgi!@34";
            string uri = "ftp://" + ip + "/" + filename;

            FtpWebRequest reqFtp = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

            try
            {
                reqFtp.Credentials = new NetworkCredential(uname, upwd);
                reqFtp.KeepAlive = false;
                reqFtp.Method = WebRequestMethods.Ftp.UploadFile;
                reqFtp.UseBinary = true;
                reqFtp.ContentLength = fi.Length;

                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;
                FileStream fs = fi.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                Stream strm = reqFtp.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);

                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();

                return 0;
            }
            catch (Exception err)
            {
                reqFtp.Abort();
                throw new Exception("上传文件到FTP失败" + err.Message);
            }
        }

        public static DataSet readXls(string path)
        {
            DataTable ExcelTable;
            DataSet ds = new DataSet();
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path
                    + ";Extended Properties='Excel 8.0;IMEX=1';";
            OleDbConnection objConn = new OleDbConnection(strConn);
            objConn.Open();
            DataTable schemaTable = objConn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
            string tableName = schemaTable.Rows[0][2].ToString().Trim();
            string strSql = "select * from [" + tableName + "]";
            OleDbCommand objCmd = new OleDbCommand(strSql, objConn);
            OleDbDataAdapter myData = new OleDbDataAdapter(strSql, objConn);
            myData.Fill(ds, tableName);
            objConn.Close();
            ExcelTable = ds.Tables[tableName];

            return ds;
        }

        public static DataSet readXls(string path, string cols)
        {
            DataTable ExcelTable;
            DataSet ds = new DataSet();
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path
                    + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1';";
            if (path.EndsWith(".xlsx"))
            {
                //此连接可以操作.xls与.xlsx文件 (支持Excel2003 和 Excel2007 的连接字符串)
                strConn = "Provider=Microsoft.Ace.OleDb.12.0;data source=" + path
                  + ";Extended Properties='Excel 12.0; HDR=NO; IMEX=1'"; 
            }
            OleDbConnection objConn = new OleDbConnection(strConn);
            objConn.Open();
            DataTable schemaTable = objConn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
            string tableName = schemaTable.Rows[0][2].ToString().Trim();
            string strSql = "";
            strSql = "select " + cols + " from [" + tableName + "]";
            OleDbCommand objCmd = new OleDbCommand(strSql, objConn);
            OleDbDataAdapter myData = new OleDbDataAdapter(strSql, objConn);
            myData.Fill(ds, tableName);
            objConn.Close();
            if (ds.Tables[tableName].Rows.Count > 0)
            {
                int rows=ds.Tables[tableName].Rows.Count;
                int colss=ds.Tables[tableName].Columns.Count;

                //去掉空行
                for (int i = rows-1; i>=0; i--)
                {
                    bool mark = true;
                    for (int j = 0; j < colss; j++)
                    {
                        if (ds.Tables[tableName].Rows[i][j].ToString().Trim() != "")
                        {
                            mark = false;
                            break;
                        }
                    }
                    if (mark)
                    {
                        ds.Tables[tableName].Rows.Remove(ds.Tables[tableName].Rows[i]);
                    }
                }

                    ds.Tables[tableName].Rows.Remove(ds.Tables[tableName].Rows[0]);
                    ds.AcceptChanges();
            }

            ExcelTable = ds.Tables[tableName];

            return ds;
        }

        public static void writeXls(string path, string cont)
        {
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path
                    + ";Extended Properties='Excel 8.0;HDR=No';";
            OleDbConnection objConn = new OleDbConnection(strConn);
            objConn.Open();
            DataTable schemaTable = objConn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
            int row_count = schemaTable.Rows.Count;
            row_count++;
            string tableName = schemaTable.Rows[0][2].ToString().Trim();
            string strSql = "insert into [" + tableName + "](F2) values('{0}')";
            strSql = String.Format(strSql, cont);
            OleDbCommand objCmd = new OleDbCommand(strSql, objConn);
            objCmd.ExecuteNonQuery();

            objConn.Close();

        }

        public static string Export(System.Data.DataTable dt, string path, int ColumnWidth = 25, int fontsize = 10)
        {
            try
            {
                #region 生成excel文件
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
                Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
                Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];//取得sheet1
                Microsoft.Office.Interop.Excel.Range range;

                int colindex = 0;
                foreach (DataColumn col in dt.Columns)
                {
                    colindex++;
                    range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, colindex];
                    range.ColumnWidth = ColumnWidth;
                    range.NumberFormatLocal = "@";
                    range.Font.Size = fontsize;
                    range.Value2 = col.ColumnName;
                }

                int rowindex = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    rowindex++;

                    colindex = 0;
                    foreach (DataColumn col in dt.Columns)
                    {
                        colindex++;
                        range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[rowindex + 1, colindex];
                        range.ColumnWidth = ColumnWidth;
                        range.NumberFormatLocal = "@";
                        range.Font.Size = fontsize;
                        range.Value2 = dr[colindex - 1];
                    }

                }

                workbook.Saved = true;
                //workbook.SaveCopyAs(path);  //2007版本
                xlApp.DisplayAlerts = false;
                workbook.SaveAs(path, 56, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                xlApp.DisplayAlerts = true;
                range = null;

                workbooks = null;
                workbook = null;

                if (xlApp != null)
                {
                    xlApp.Workbooks.Close();
                    xlApp.Quit();
                    xlApp = null;
                }

                return "";

                #endregion
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                GC.Collect();
            }

        }

        public static string objectToString(DataTable dt, string col_name,bool showMessage = false)
        {
            return objectToString(dt, 0, col_name, showMessage);
        }

        public static string objectToString(DataTable dt, int row_id, string col_name, bool showMessage = false)
        {
            string ret = "";
            try
            {
                if (col_name == null || col_name == "")
                {
                    return "";
                }
                return dt.Rows[row_id][col_name].ToString();
            }
            catch (Exception ex)
            {
                if (showMessage)
                {
                    ret = ex.Message;
                }
                else
                {
                    ret = "";
                }
            }

            return ret;
        }

        public static bool isWhiteOfSeparate(string spid, string uid)
        {
            //1205005501=fadyzhuang;amoszhang&1212921601=fadyzhuang;jasoncai
            string str_white = ConfigurationManager.AppSettings["WhiteOfSeparate"].ToString();
            string[] strChars = str_white.Split(new char[] { '|' });
            foreach (string strChar in strChars)
            {
                string[] strSPs = strChar.Split(new char[] { '=' });
                if (strSPs[0] == spid)
                {
                    if (strSPs[1].IndexOf(uid) > -1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void GetCashTypeList(System.Web.UI.WebControls.DropDownList ddl)
        {
            string str_cashtype = ConfigurationManager.AppSettings["CashTypeList"].ToString();
            string[] strChars = str_cashtype.Split(new char[] { '|' });
            foreach (string strChar in strChars)
            {
                string[] strSPs = strChar.Split(new char[] { '=' });
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(strSPs[1], strSPs[0]);
                ddl.Items.Add(li);
            }
        }

        /// <summary>
        /// 为对象的所有成员创建Param
        /// </summary>
        public static Check_WebService.Param[] CreateParam(object obj)
        {
            FieldInfo[] fi = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            Check_WebService.Param[] pa = new Check_WebService.Param[fi.Length];
            for (int i = 0; i < fi.Length; i++)
            {
                pa[i] = new Check_WebService.Param();
                int indexN = fi[i].Name.IndexOf("Field");
                pa[i].ParamName = fi[i].Name.Substring(0, indexN);
                //  pa[i].ParamName = fi[i].Name;
                pa[i].ParamValue = Convert.ToString(fi[i].GetValue(obj));
                pa[i].ParamFlag = "";
            }
            return pa;
        }

        public static Check_Service CreateCheckService(System.Web.UI.Page page)
        {
            Check_WebService.Finance_Header fh = new Check_WebService.Finance_Header();
            fh.UserIP = page.Request.UserHostAddress;
            fh.UserName = page.Session["uid"].ToString();
            fh.UserPassword = "";
            fh.SzKey = page.Session["SzKey"].ToString();
            //    fh.RightString = page.Session["key"].ToString();
            fh.OperID = Int32.Parse(page.Session["OperID"].ToString());


            Check_Service cs = new Check_Service();
            cs.Finance_HeaderValue = fh;
            return cs;
        }

        /// <summary>
        /// DataTable分页
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="PageIndex">页索引,注意：从1开始</param>
        /// <param name="PageSize">每页大小</param>
        /// <returns>分好页的DataTable数据</returns>              第1页        每页10条
        public static DataTable GetPagedTable(DataTable dt, int PageIndex, int PageSize)
        {
            if (PageIndex == 0)
            {
                return dt;
            }
            DataTable newdt = dt.Copy();
            newdt.Clear();
            int rowbegin = (PageIndex - 1) * PageSize;
            int rowend = PageIndex * PageSize;
            if (rowbegin >= dt.Rows.Count)
            {
                return newdt;
            }
            if (rowend > dt.Rows.Count)
            {
                rowend = dt.Rows.Count;
            }
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = newdt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                newdt.Rows.Add(newdr);
            }
            return newdt;
        }
     
        //产生银行公告ID
        public static int NewStaticNo = 10; //初始值 当达到99后，则循环，从10开始
        public static bool NewStaticNoManageSign = true;

        /// <summary>
        /// 保证不重复管理器
        /// 初始值为10，使用时，每次+1；当达到100时，循环使用。 跟在秒后使用。
        /// </summary>

        public static string NewStaticNoManage()
        {
            //如果标志位为false,则等待
            try
            {
                while (!NewStaticNoManageSign)
                {
                    Thread.Sleep(50);
                }

                NewStaticNoManageSign = false;

                NewStaticNo++;

                if (NewStaticNo > 99)
                {
                    NewStaticNo = 10;  //清空为初始状态
                }
            }
            finally
            {
                NewStaticNoManageSign = true;
            }
            return NewStaticNo.ToString();
        }


        //产生审批单号
        public static int StaticNo = 10; //初始值 当达到99后，则循环，从10开始
        public static bool StaticNoManageSign = true;

        /// <summary>
        /// 保证不重复管理器
        /// 初始值为10，使用时，每次+1；当达到100时，循环使用。 跟在秒后使用。
        /// </summary>

        public static string StaticNoManage()
        {
            //如果标志位为false,则等待
            try
            {
                while (!StaticNoManageSign)
                {
                    Thread.Sleep(50);
                }

                StaticNoManageSign = false;

                StaticNo++;

                if (StaticNo > 99)
                {
                    StaticNo = 10;  //清空为初始状态
                }
            }
            finally
            {
                StaticNoManageSign = true;
            }
            return StaticNo.ToString();
        }

        //将两个表结构一致的dataset合并到一个dataset
        public static DataSet ToOneDataset(DataSet ds, DataSet ds2)
        {
            DataSet dsAll = new DataSet();

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dsAll.Tables.Add(ds.Tables[0].Copy());
                if (ds2 != null && ds2.Tables.Count > 0)
                {//分库表不为null
                    foreach (DataTable tbl in ds2.Tables)
                        if (tbl.Rows.Count > 0)//分库表不为null
                        {
                            foreach (DataRow dr in tbl.Rows)
                            {
                                dsAll.Tables[0].ImportRow(dr);//将记录加入到一个表里
                            }
                        }
                }
            }
            else
            {
                if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                    dsAll.Tables.Add(ds2.Tables[0].Copy());
            }
            return dsAll;
        }

        //证件类型
        public static string GetCreType(string creid)
        {

            if (creid == null || creid.Trim() == "")
                return "未指定类型";

            int icreid = 0;
            try
            {
                icreid = Int32.Parse(creid);
            }
            catch
            {
                return "不正确类型" + creid;
            }

            if (icreid >= 1 && icreid <= 11)
            {
                if (icreid == 1)
                {
                    return "身份证";
                }
                else if (icreid == 2)
                {
                    return "护照";
                }
                else if (icreid == 3)
                {
                    return "军官证";
                }
                else if (icreid == 4)
                {
                    return "士兵证";
                }
                else if (icreid == 5)
                {
                    return "回乡证";
                }
                else if (icreid == 6)
                {
                    return "临时身份证";
                }
                else if (icreid == 7)
                {
                    return "户口簿";
                }
                else if (icreid == 8)
                {
                    return "警官证";
                }
                else if (icreid == 9)
                {
                    return "台胞证";
                }
                else if (icreid == 10)
                {
                    return "营业执照";
                }
                else if (icreid == 11)
                {
                    return "其它证件";
                }
                else
                {
                    return "不正确类型" + creid;
                }
            }
            else
            {
                return "不正确类型" + creid;
            }
        }

        public static Check_WebService.Param[] ToParamArray(string[,] param)
        {
            Check_WebService.Param[] pa = new Check_WebService.Param[param.GetLength(0)];
            for (int i = 0; i < pa.Length; i++)
            {
                pa[i] = new Check_WebService.Param();
                pa[i].ParamName = param[i, 0];
                pa[i].ParamValue = Convert.ToString(param[i, 1]);
                pa[i].ParamFlag = "";
            }
            return pa;
        }

        public static TENCENT.OSS.C2C.Finance.BankLib.Param[] ToParamArrayStruct(string[,] param)
        {
            TENCENT.OSS.C2C.Finance.BankLib.Param[] pa = new TENCENT.OSS.C2C.Finance.BankLib.Param[param.GetLength(0)];
            for (int i = 0; i < pa.Length; i++)
            {
                pa[i].ParamName = param[i, 0];
                pa[i].ParamValue = Convert.ToString(param[i, 1]);
            }
            return pa;
        }

        public static void CreateDirectory(string targetPath)
        {
            try
            {
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }
            }
            catch
            {
                throw new Exception("创建文件夹失败！");
            }
        }

        public static string upImage(HtmlInputFile file, string folderName)
        {
            string objid = "";
            try
            {
                //上传需要的图片，并返回对应服务器上的地址
                //存放文件
                // string s1 = File1.Value;
                string s1 = file.Value;
                //放到外面控制
                //if (s1 == "")
                //{
                //    throw new Exception("请上传图片");
                //}
                string szTypeName = s1.Substring(s1.Length - 4, 4);
                string alPath;
                HtmlInputFile inputFile = file;
                string upStr = null;
                string src = System.Configuration.ConfigurationManager.AppSettings["KFWebSrc"].ToString();


                if (string.IsNullOrEmpty(folderName.Trim()))
                {
                    throw new Exception("业务文件夹名为空");
                }
                if (szTypeName.ToLower() != ".jpg" && szTypeName.ToLower() != ".gif" && szTypeName.ToLower() != ".bmp")
                {
                    throw new Exception("上传的文件不正确，必须为jpg,gif,bmp");
                }

                if (inputFile.Value != "")
                {
                    objid = System.DateTime.Now.ToString("yyyyMMddHHmmss") + PublicRes.StaticNoManage();
                    //  string fileName = "kf" + DateTime.Now.ToString("yyyyMMddHHmmss") + szTypeName; //
                    string fileName = objid + szTypeName; //

                    upStr = "uploadfile\\" + System.DateTime.Now.ToString("yyyyMMdd") + "\\CSOMS\\" + folderName;//System.Configuration.ConfigurationManager.AppSettings["uploadPath"].ToString();

                    string targetPath = src + upStr;

                    PublicRes.CreateDirectory(targetPath);

                    string path = targetPath + "\\" + fileName;
                    inputFile.PostedFile.SaveAs(path);

                    //alPath.Add(upStr+ "/" +fileName);	
                    //    alPath = upStr + "/" + fileName;
                    alPath = upStr.Replace("\\", "/") + "/" + fileName;
                    return alPath;
                }
                else
                {
                    throw new Exception("请上传正确的图片");
                }
            }
            catch (Exception eStr)
            {
                string errMsg = "上传文件失败！" + PublicRes.GetErrorMsg(eStr.Message.ToString());
                throw new Exception(errMsg);
            }
        }

        /// <summary>
        /// 绑定下拉列表
        /// </summary>
        /// <param name="ds">Data数据</param>
        /// <param name="dl">下拉菜单obj</param>
        /// <param name="Msg"></param>
        public static void BindDropDownList(DataSet ds, DropDownList dl, out string Msg)
        {
            Msg = "";
            //dl = new DropDownList(); 此处不能使用New
            //dl.Items.Clear();

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                Msg = "传入DataSet数据为空!";
            }

            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                ListItem li = new ListItem();
                li.Text = "【" + dr["Value"].ToString().Trim() + "】" + dr["Text"].ToString().Trim();
                li.Value = dr["Value"].ToString().Trim();

                dl.Items.Add(li);
            }
        }

        public static void GetDllDownList(DropDownList dll, Dictionary<string, string> dic,string defVal,string defKey)
        {
            dll.Items.Add(new ListItem(defVal, defKey));
            foreach (var item in dic)
            {
                dll.Items.Add(new ListItem(item.Value, item.Key));
            }
            dll.DataBind();
        }

        /// <summary>
        /// 获取DataGird中checkbox列的勾选
        /// </summary>
        /// <param name="datagird"></param>
        /// <param name="CheckBoxColNum">checkbox所在列下标</param>
        /// <param name="valueColNum">待获取值所在列下标</param>
        /// <param name="listValues">待获取值列表</param>
        /// <param name="itemCounts">勾选个数</param>
        /// <returns></returns>
        public static ArrayList GetCheckData(DataGrid datagird, int CheckBoxColNum, int valueColNum,string checkboxName,out int itemCounts)
        {
            ArrayList listValues = new ArrayList();
            itemCounts = 0;
            try
            {
                int count = datagird.Items.Count;

                for (int i = 0; i < count; i++)
                {
                    System.Web.UI.Control obj = datagird.Items[i].Cells[CheckBoxColNum].FindControl(checkboxName);
                    if (obj != null && obj.Visible)
                    {
                        CheckBox cb = (CheckBox)obj;
                        if (cb.Checked)
                        {
                            string listValue = datagird.Items[i].Cells[valueColNum].Text.Trim();

                            listValues.Add(listValue);
                            itemCounts++;
                        }
                    }
                }

                return listValues;

            }
            catch (Exception ex)
            {
                throw new Exception("获取勾选项数据异常："+ex.Message);
            }
        }

        /// <summary>
        /// datagird列头勾选列中所有可选checkbox项
        /// </summary>
        /// <param name="sender">列头checkbox</param>
        /// <param name="datagrid"></param>
        /// <param name="checkBoxColNum">列头checkbox所在列下标</param>
        /// <param name="checkboxName">需勾选行checkbox所在列下标</param>
         public static void CheckAll(object sender,DataGrid datagrid,int checkBoxColNum,string checkboxName)
        {
            try
            {
                int count = datagrid.Items.Count;
                CheckBox allCheck = (CheckBox)sender;

                for (int i = 0; i < count; i++)
                {
                    System.Web.UI.Control obj = datagrid.Items[i].Cells[checkBoxColNum].FindControl(checkboxName);
                    if (obj != null && obj.Visible)
                    {
                        CheckBox cb = (CheckBox)obj;
                        if (allCheck.Checked)
                        {
                            if (cb.Enabled != false)
                                cb.Checked = true;
                        }
                        else
                        {
                            cb.Checked = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogInfo("勾选全部出现异常：" + ex.Message);
            }
        }

        /// <summary>
        /// 绑定下拉列表
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="ddl"></param>
        public static void GetDropdownlist(Dictionary<string, string> dic, DropDownList ddl)
        {
            foreach (var item in dic)
            {
                ddl.Items.Add(new ListItem(item.Value, item.Key));
            }
            ddl.DataBind();
        }
    }

}
