using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TENCENT.OSS.C2C.Finance.DataAccess;
using System.Configuration;
using System.Security.Cryptography;
using System.IO;
using TENCENT.OSS.CFT.KF.Common;
using System.Data;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using TENCENT.OSS.CFT.KF.DataAccess;
using CFT.CSOMS.DAL.CFTAccount;
using CFT.CSOMS.DAL.Infrastructure;
using CFT.Apollo.Logging;

namespace CFT.CSOMS.DAL.Infrastructure
{
    public class PublicRes
    {
        public static bool IgnoreLimitCheck = System.Configuration.ConfigurationManager.AppSettings["IgnoreLimitCheck"].Trim().ToLower() == "true";
        public static bool CommQuery(string serviceName, string reqParams, bool isCret, out string sReply, out short iResult, out string sMsg) 
        {
            LogHelper.LogInfo(serviceName + " send req:" + reqParams);
            bool isRet = commRes.middleInvoke(serviceName, reqParams, isCret, out sReply, out iResult, out sMsg);
            LogHelper.LogInfo(serviceName + " return:" + sReply);

            return isRet;
        }

        public static DataSet returnDSAll(string strCmd, string dbStr)
        {
            using (var da = MySQLAccessFactory.GetMySQLAccess(dbStr))  //连接数据库类型
            {
                da.OpenConn();
                return da.dsGetTotalData(strCmd);
            }
        }

        public static DataSet returnDSAll_Conn(string strCmd, string connstr)
        {
            MySqlAccess da = null;

            try
            {
                da = new MySqlAccess(connstr);  //连接数据库类型
                da.OpenConn();
                return da.dsGetTotalData(strCmd);
            }
            finally
            {
                da.Dispose();
            }
        }

        public static string ExecuteOne(string sqlStr, string dbStr) //查询单个结果
        {
            using (var da = MySQLAccessFactory.GetMySQLAccess(dbStr))
            {
                da.OpenConn();
                return da.GetOneResult(sqlStr);
            }
        }

        public static string ExecuteOne_Conn(string sqlStr, string connstr) //查询单个结果
        {
            using (var da = MySQLAccessFactory.GetMySQLAccess(connstr))
            {
                da.OpenConn();
                return da.GetOneResult(sqlStr);
            }
        }

        /// <summary>
        /// 证件类型转义
        /// </summary>
        /// <param name="creid"></param>
        /// <returns></returns>
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

        public static DataSet NewMethod(DataSet ds, DataSet ds2)
        {
            DataSet dsAll = new DataSet();
            DataSet result = new DataSet();
            DataTable dtAll = new DataTable();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dsAll.Tables.Add(ds.Tables[0].Copy());
                dsAll.Tables[0].Columns.Add("FidNew", Type.GetType("System.String"));
                //处理Fid,t_tenpay_appeal_trans表fid为int，分库表为varchar
                if (dsAll != null && dsAll.Tables.Count > 0 && dsAll.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsAll.Tables[0].Rows)
                    {
                        dr["FidNew"] = dr["Fid"].ToString();
                    }
                }

                dsAll.Tables[0].Columns.Remove(dsAll.Tables[0].Columns["Fid"]);//删除fid列
                dsAll.Tables[0].Columns["FidNew"].ColumnName = "Fid";//将FidNew列名修改为Fid

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

        public static string GetTName(string dbname, string strTable, string strID)
        {


            if (strID.Length < 8 && Int32.Parse(strID) < 10000000)
            {
                if (strTable == "t_user")
                {
                    return dbname + ".t_middle_user";
                }
                else //还有一个帐户流水判断，需要时间字段暂时不加了。
                {
                    if (strID.Length > 2)
                    {
                        return dbname + "_" + strID.Substring(strID.Length - 2) + "." + strTable + "_" + strID.Substring(strID.Length - 3, 1);
                    }
                    else return dbname + "." + strTable;
                }
            }
            else
            {
                if (strID.Length > 2)
                {
                    return dbname + "_" + strID.Substring(strID.Length - 2) + "." + strTable + "_" + strID.Substring(strID.Length - 3, 1);
                }
                else return dbname + "." + strTable;
            }

        }

        /// <summary>
        /// DataTable分页
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="offset"> 起始行</param>
        /// <param name="limit">行数</param>
        /// <returns>分好页的DataTable数据</returns>              起始行       行数
        public static DataTable GetPagedTable(DataTable dt, int offset, int limit)
        {
            DataTable newdt = dt.Copy();
            newdt.Clear();
            int rowbegin = offset;
            int rowend = offset+limit;
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

        //对插入数据库的字符串的敏感字符进行替换,防止非法sql注入访问
        public static string replaceMStr(string str)
        {
            if (str == null) return null; //furion 20050819

            str = str.Replace("'", "’");
            str = str.Replace("\"", "”");
            str = str.Replace("script", "ｓｃｒｉｐｔ");
            str = str.Replace("<", "〈");
            str = str.Replace(">", "〉");
            str = str.Replace("-", "－");
            return str;
        }
		
		 /// <summary>
        /// 获得数据库的时间 格式：yyyy-MM-dd HH:mm:ss
        /// </summary>
        public static string strNowTimeStander
        {
            get
            {
                using (var da = MySQLAccessFactory.GetMySQLAccess("ZWBasic"))
                {
                    try
                    {
                        da.OpenConn();
                        return da.GetOneResult("select DATE_FORMAT(now(),'%Y-%m-%d %H:%i:%s')");
                    }
                    catch
                    {
                        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
            }
        }

        /// <summary>
        /// 获得数据库的时间 格式：yyyy-MM-dd HH:mm:ss
        /// </summary>
        public static string strNowTime
        {
            get
            {
                using (var da = MySQLAccessFactory.GetMySQLAccess("ht_DB"))
                {
                    try
                    {
                        da.OpenConn();
                        string tmp = da.GetOneResult("select DATE_FORMAT(now(),'%Y-%m-%d %H:%i:%s')");
                        return "'" + tmp + "'";
                    }
                    catch
                    {
                        string tmp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        return "'" + tmp + "'";
                    }
                }
            }
        }
      //通过服务签名
        public static string SingedByService(string SingedString)
        {
            try
            {
                DataSet ds = null;
                string Msg = "";
                string errMsg = "";
                //md5
                string CFTAccount = ConfigurationManager.AppSettings["CFTAccount"];
                string wxzfAccount = ConfigurationManager.AppSettings["wxzfAccount"];
                string relay_ip = ConfigurationManager.AppSettings["Relay_IP"];
                string relay_port = ConfigurationManager.AppSettings["Relay_PORT"];

                //  string sign = "account_no=" + acc_no + "&bank_type=" + bank_type + "&uid=" + uid + "&uidtoc=" + uidtoc + "&key=";
                string sign = SingedString + "&key=";
                sign = System.Web.HttpUtility.UrlEncode(sign, System.Text.Encoding.GetEncoding("gb2312"));

                Msg = "";
                errMsg = "";
                string req_sign = "request_type=132&ver=1&head_u=&sp_id=" + CFTAccount + "&merchant_spid=" + CFTAccount + "&sp_str=" + sign;
                LogHelper.LogInfo("SingedByService:" + req_sign);
                string sign_md5 = commRes.GetFromRelay(req_sign, relay_ip, relay_port, out Msg);
                LogHelper.LogInfo("SingedByService:" + sign_md5);
                if (Msg != "")
                {
                    LogHelper.LogInfo("SingedByService:" + Msg);
                    throw new Exception("1签名出错:" + Msg);
                }
                ds = TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.ParseRelayStr(sign_md5, out errMsg);
                if (errMsg != "")
                {
                    LogHelper.LogInfo("SingedByService:" + Msg);
                    throw new Exception("2签名出错:" + errMsg);
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    sign_md5 = ds.Tables[0].Rows[0]["sp_md5"].ToString();
                }
                return sign_md5;
            }
            catch (Exception ex)
            {
                LogHelper.LogInfo("SingedByService:" + ex.Message);
                throw new LogicException("通过服务签名出错！" + ex.Message);
            }


        }

        public static string getCgiString(string instr)
        {
            if (instr == null || instr.Trim() == "")
                return "";

            //System.Text.Encoding enc = System.Text.Encoding.GetEncoding("GB2312");
            return System.Web.HttpContext.Current.Server.UrlDecode(instr).Replace("\r\n", "").Trim()
                .Replace("%3d", "=").Replace("%20", " ").Replace("%26", "&").Replace("%7c", "|");
        }

        public static string getCgiStringUtil(string instr)
        {
            if (instr == null || instr.Trim() == "")
                return "";

            //System.Text.Encoding enc = System.Text.Encoding.GetEncoding("GB2312");
            return HttpUtility.UrlDecode(instr).Replace("\r\n", "").Trim()
                .Replace("%3d", "=").Replace("%20", " ").Replace("%26", "&").Replace("%7c", "|");
        }

        public static string makePwd()
        {
            string s = null;

            for (int i = 0; i < 8; i++)
            {
                System.Random rd = new Random();

                //furion 这个地方一定需要延时吗？
                System.Threading.Thread.Sleep(1);
                s += rd.Next(10);
            }

            return s;

        }

        public static string GetTableNameQQ(string strTable, string strID)  // 返回按照QQ号分表的表名， QQID
        {
            strID = AccountData.ConvertToFuid(strID);  //先转换成fuid

            if (strID.Length > 2)
            {
                return "c2c_db_" + strID.Substring(strID.Length - 2) + "." + strTable + "_" + strID.Substring(strID.Length - 3, 1);
            }
            else return "c2c_db." + strTable;
        }

         public static string GetTableNameUid(string strTable, string uid)  // 返回按照QQ号分表的表名， QQID
        {
            if (uid.Length > 2)
            {
                return "c2c_db_" + uid.Substring(uid.Length - 2) + "." + strTable + "_" + uid.Substring(uid.Length - 3, 1);
            }
            else return "c2c_db." + strTable;
        }


         public static string objectToString(DataTable dt, string col_name)
         {
             return objectToString(dt, 0, col_name);
         }

         public static string objectToString(DataTable dt, int row_id, string col_name)
         {
             string ret = "";
             try
             {
                 if (col_name == null || col_name == "")
                 {
                     return "";
                 }
                 if (dt.Columns.Contains(col_name))
                 {
                     return dt.Rows[row_id][col_name].ToString();
                 }
             }
             catch (Exception ex)
             {
                 ret = "";
             }

             return ret;
         }

    }
    }

    /// <summary>
    /// 金额转换类。
    /// </summary>
    /// <summary>
    /// 金额转换类。
    /// </summary>
    public class MoneyTransfer
    {
        /// <summary>
        /// 分转化成元
        /// </summary>
        /// <param name="strfen">分</param>
        /// <returns>元</returns>
        public static string FenToYuan(string strfen, string currency_type)
        {

            if (strfen == "")
            {
                return "0";
            }

            int pointNum = GetCurPointNum(currency_type);
            if (pointNum == -1)
            {
                throw new LogicException("金额转换失败，查询" + currency_type + "币种的小数点位数失败！");
            }



            //double yuan = (double)(Int64.Parse(strfen))/100;
            //yuan = Math.Round(yuan,2);
            double yuan = (double)(Int64.Parse(strfen)) / Math.Pow(10, pointNum);
            yuan = Math.Round(yuan, pointNum);

            string tmp = yuan.ToString();

            if (pointNum > 0)
            {
                int iindex = tmp.IndexOf(".");
                string[] tmpNums = tmp.Split('.');
                if (iindex == -1)//无小数点
                {

                    tmp = tmp + "." + "".PadRight(pointNum, '0');
                }
                else if (tmpNums[1].Length < pointNum) //小于小数点位数
                {

                    tmp = tmpNums[0] + "." + tmpNums[1].PadRight(pointNum, '0');
                }
            }


            return tmp;
        }
        //获取小数点位数
        public static int GetCurPointNum(string currency_type)
        {
            if (currency_type.ToUpper().Trim() == "RMB" || currency_type.ToUpper().Trim() == "CNY"
                || currency_type.ToUpper().Trim() == "USD" || currency_type.ToUpper().Trim() == "HKD")
            {
                return 2;
            }

            if (currency_type.Trim() == "")
            {
                //金额转换，传入的币种代码为空！
                return -1;

            }

            using (var da = MySQLAccessFactory.GetMySQLAccess("FCPAY"))
            try
            {
                da.OpenConn();
                string strSql = "select  Fpoint_num  from fcpay_db.t_currency_code where Fcurrency_type='" + currency_type.ToUpper().Trim() + "'";
                DataSet ds = da.dsGetTotalData(strSql);
                if (ds == null || ds.Tables.Count==0 || ds.Tables[0].Rows.Count != 1)
                {
                    throw new LogicException("查询到币种" + currency_type + "对应的 小数点位数 记录数不唯一！");
                }
                return Convert.ToInt32(ds.Tables[0].Rows[0]["Fpoint_num"].ToString());
            }
            catch (Exception ex)
            {
                return -1;
            }
        }


}
