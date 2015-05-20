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
using CommLib;
using ReCommQuery = TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery;
using TENCENT.OSS.C2C.Finance.BankLib;
using System.Collections;//解决命名冲突

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

        public static string GetTName(string strTable, string strID)
        {
            return GetTName("c2c_db", strTable, strID);
        }

        /// <summary>
        /// 返回根据ID分机器的连接字符串
        /// </summary>
        /// <param name="FlagName">分机器使用的标志</param>
        /// <param name="sepvalue">分机器的判断value，如内部ID后两位</param>
        /// <returns></returns>
        public static string GetConnString(string FlagName, string sepvalue)
        {
            MySqlAccess daht = new MySqlAccess(DbConnectionString.Instance.GetConnectionString("HT"));
            try
            {
                string strSql = " select FconfigName from c2c_fmdb.t_sepdb where FMinValue<=" + sepvalue + " and FMaxValue>=" + sepvalue
                    + " and FFlagName='" + FlagName.ToLower() + "'";

                daht.OpenConn();
                string configname = daht.GetOneResult(strSql);

                return DbConnectionString.Instance.GetConnectionString(configname);

            }
            catch
            {
                return "";
            }
            finally
            {
                daht.Dispose();
            }
        }

        public static string ConvertToFuid(string QQID)
        {
            try
            {
                //furion 20061115 email登录相关
                if (QQID == null || QQID.Trim().Length < 3)
                    return null;

                //start
                string qqid = QQID.Trim();
 
                string errMsg = "";
                string strSql = "uin=" + qqid;
                string struid = ReCommQuery.GetOneResultFromICE(strSql, ReCommQuery.QUERY_RELATION, "fuid", out errMsg);

                if (struid == null)
                    return null;
                else
                    return struid;

            }
            catch (Exception ex)
            {
                return null;
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

         public static string EncryptZerosPadding(string source)
         {
             if (source.Trim() == "")
                 return "";

             try
             {

                 DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                 //把字符串放到byte数组中  
                 byte[] inputByteArray = Encoding.Default.GetBytes(source);

                 byte[] key = { 0x4a, 0x08, 0x80, 0x58, 0x13, 0xad, 0x46, 0x89 };

                 byte[] iv = { 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18 };

                 des.Key = key;
                 des.IV = iv;
                 des.Padding = System.Security.Cryptography.PaddingMode.Zeros;//0填充
                 MemoryStream ms = new MemoryStream();
                 CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

                 cs.Write(inputByteArray, 0, inputByteArray.Length);
                 cs.FlushFinalBlock();
                 string bas64Str = Convert.ToBase64String(ms.ToArray());
                 return bas64Str.Replace("+", "-").Replace("/", "_");//.Replace("=","%3d").Replace("=","%3D");


             }
             catch (Exception ex)
             {
                 return "";
             }
         }

         /// <summary>
         /// 一点通银行卡解密方法
         /// </summary>
         /// <param name="base64Bankid"></param>
         /// <returns></returns>
         public static string BankIDEncode_ForBankCardUnbind(string base64Bankid)
         {
             byte[] iv = { 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18 };
             byte[] newkey = { 0x4a, 0x08, 0x80, 0x58, 0x13, 0xad, 0x46, 0x89 };

             try
             {
                 DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                 //middle的base64转换不标准，这个地方需要替换下 
                 byte[] inputByteArray = Convert.FromBase64String(base64Bankid.Replace("-", "+").Replace("_", "/").Replace("%3d", "=").Replace("%3D", "="));

                 //建立加密对象的密钥和偏移量，此值重要，不能修改 

                 des.Key = newkey;
                 des.IV = iv;
                 //Padding设置为None，这个很重要，因为middle是这个加密的
                 des.Padding = System.Security.Cryptography.PaddingMode.None;
                 MemoryStream ms = new MemoryStream();
                 CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                 cs.Write(inputByteArray, 0, inputByteArray.Length);
                 cs.FlushFinalBlock();

                 return System.Text.Encoding.ASCII.GetString(ms.ToArray()).Trim();
             }
             catch (Exception ex)
             {
                 return base64Bankid;
             }
         }

         public static string[] returnlistInfo(string listID, out TENCENT.OSS.C2C.Finance.Common.CommLib.tradeList tl, MySqlAccess da, string noUpdate)  //帐务调整的共用函数：获取交易单的相关信息
         {
             
             string[] ar = new string[37];
             ar[0] = "Fbank_listid";    //银行订单号
             ar[1] = "flistid";         //交易单ID
             ar[2] = "fspid";           //spid
             ar[3] = "fbuy_uid";        //fbuy_uid
             ar[4] = "fbuyid";          //fbuyid
             ar[5] = "fcurtype";        //fcurtype
             ar[6] = "fpaynum";         //fpaynum
             ar[7] = "fip";             //fip
             ar[8] = "Fbuy_bank_type";  //fbuy_bank_type
             ar[9] = "fcreate_time";    //fcreate_time
             ar[10] = "fbuy_name";       //fbuy_name 
             ar[11] = "fsaleid";        //fsaleid
             ar[12] = "fsale_uid";       //fsale_uid
             ar[13] = "fsale_name";      //fsale_name
             ar[14] = "fsale_bankid";    //fsale_bankid
             ar[15] = "Fpay_type";       // 
             ar[16] = "Fbuy_bankid";
             ar[17] = "Fsale_bank_type";
             ar[18] = "Fprice";
             ar[19] = "Fcarriage";
             ar[20] = "Fprocedure";
             ar[21] = "Fmemo";
             ar[22] = "Fcash";
             ar[23] = "Ftoken";
             ar[24] = "Ffee3";       //其它费用
             //ar[25]= "Fstate";      //交易单的状态  Fstate，Fpay_time，Freceive_time，Fmodify_time
             ar[25] = "Ftrade_state";
             ar[26] = "Fpay_time";   //买家付款时间（本地）
             ar[27] = "Freceive_time"; //打款给卖家时间
             ar[28] = "Fmodify_time";  //最后修改时间
             ar[29] = "Ftrade_type";  //交易类型 1 c2c 2 b2c 3 fastpay 4 转帐
             ar[30] = "Flstate";      //交易单的状态
             ar[31] = "Fcoding";
             ar[32] = "Fbank_backid";
             ar[33] = "Ffact";         //总支付费用
             ar[34] = "Fmedi_sign";
             ar[35] = "Fgwq_listid";
             ar[36] = "Fchannel_id";


             string errMsg = "";
             string cmdStr = "listid=" + listID;
             ar = ReCommQuery.GetdrDataFromICE(cmdStr, ReCommQuery.QUERY_ORDER, ar, out errMsg);

             if (ar[9] != null && ar[9] != "" && !ar[9].StartsWith("0000-00"))
                 ar[9] = DateTime.Parse(ar[9]).ToString("yyyy-MM-dd HH:mm:ss");   //格式化时间
             if (ar[26] != null && ar[26] != "" && !ar[26].StartsWith("0000-00"))
                 ar[26] = DateTime.Parse(ar[26]).ToString("yyyy-MM-dd HH:mm:ss");  //格式化时间
             if (ar[27] != null && ar[27] != "" && !ar[27].StartsWith("0000-00"))
                 ar[27] = DateTime.Parse(ar[27]).ToString("yyyy-MM-dd HH:mm:ss");  //格式化时间
             if (ar[28] != null && ar[28] != "" && !ar[28].StartsWith("0000-00"))
                 ar[28] = DateTime.Parse(ar[28]).ToString("yyyy-MM-dd HH:mm:ss");   //格式化时间

             //替换一些敏感字符
             ar[10] = commRes.replaceSqlStr(ar[10]);
             ar[13] = commRes.replaceSqlStr(ar[13]);
             ar[21] = commRes.replaceSqlStr(ar[21]);

             tl = new TENCENT.OSS.C2C.Finance.Common.CommLib.tradeList();

             tl.Fbank_listid = ar[0];
             tl.flistid = ar[1];
             tl.fspid = ar[2];
             tl.fbuy_uid = ar[3];
             tl.fbuyid = ar[4];
             tl.fcurtype = ar[5];
             tl.fpaynum = ar[6];
             tl.fip = ar[7];
             tl.Fbuy_bank_type = ar[8];
             tl.fcreate_time = ar[9];
             tl.fbuy_name = ar[10];
             tl.fsaleid = ar[11];
             tl.fsale_uid = ar[12];
             tl.fsale_name = ar[13];
             tl.fsale_bankid = ar[14];
             tl.Fpay_type = ar[15];
             tl.Fbuy_bankid = ar[16];
             tl.Fsale_bank_type = ar[17];
             tl.Fprice = ar[18];
             tl.Fcarriage = ar[19];
             tl.Fprocedure = ar[20];
             tl.Fmemo = ar[21];
             tl.Fcash = ar[22];
             tl.Ftoken = ar[23];
             tl.Ffee3 = ar[24];
             tl.Fstate = ar[25];
             tl.Fpay_time = ar[26];
             tl.Freceive_time = ar[27];
             tl.Fmodify_time = ar[28];
             tl.Ftrade_type = ar[29];
             tl.Flstate = ar[30];
             tl.Fcoding = ar[31];
             tl.Fbank_backid = ar[32];
             tl.Ffact = ar[33];

             tl.Fmedi_sign = QueryInfo.GetInt(ar[34]);
             tl.Fgwq_listid = QueryInfo.GetString(ar[35]);

             tl.Fchannel_id = ar[36];
             return ar;
         }

         /// <summary>
         /// 查询客服系统日志
         /// </summary>
         /// <param name="log_type">日志类型</param>
         /// <param name="key_name">关键字段名</param>
         /// <param name="key_value">关键字段值</param>
         /// <param name="keyNameList">参数列表</param>
         /// <returns></returns>
         public static DataSet QueryKFLog(string log_type, string key_name, string key_value, ArrayList keyNameList)
         {
             if (string.IsNullOrEmpty(log_type))
             {
                 throw new ArgumentNullException("log_type");
             }
             if (string.IsNullOrEmpty(key_name))
             {
                 throw new ArgumentNullException("key_name");
             }
             if (string.IsNullOrEmpty(key_value))
             {
                 throw new ArgumentNullException("key_value");
             }
             if (keyNameList==null|| keyNameList.Count == 0)
             {
                 throw new ArgumentNullException("keyNameList");
             }
             try
             {
                 //根据参数列表组装成以列表中参数为列名的ds
                 DataSet dsParam = GetKFLogKeyNameDS(keyNameList);

                 using (var da = MySQLAccessFactory.GetMySQLAccess("DataSource_ht"))
                 {
                     da.OpenConn();

                     string Sql = string.Format("select Fid from c2c_fmdb.t_log_kf_all where Flog_type='{0}' and Fkey_name='{1}' and Fkey_value='{2}'",
                        log_type, key_name, key_value);
                     DataSet ds = da.dsGetTotalData(Sql);
                     if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                     {
                         return null;
                     }

                     try
                     {
                         string id = "";
                         DataSet dsParamOne = new DataSet();
                         foreach (DataRow row in ds.Tables[0].Rows)
                         {
                             id = row["Fid"].ToString();
                             Sql = "select * from c2c_fmdb.t_log_kf_param where Flog_id='" + id + "';";
                             dsParamOne = da.dsGetTotalData(Sql);

                             if (dsParamOne == null || dsParamOne.Tables.Count == 0 || dsParamOne.Tables[0].Rows.Count == 0)
                                 LogHelper.LogInfo(" c2c_fmdb.t_log_kf_all 表中 Fid=" + id + "未查询到对应的参数");
                            
                             DataRow dsnew = dsParam.Tables[0].NewRow();
                             foreach (DataRow drP in dsParamOne.Tables[0].Rows)
                             {
                                 string name = drP["Fkey"].ToString();
                                 string value = drP["Fvalue"].ToString();
                                 dsnew[name] = value;
                             }
                             dsParam.Tables[0].Rows.Add(dsnew);
                         }
                     }
                     catch (Exception ex)
                     {
                         throw new Exception("查询日志参数异常：" + ex);
                     }
                 }

                 return dsParam;
             }
             catch (Exception ex)
             {
                 string err = "查询日志异常：" + ex;
                 LogHelper.LogInfo(err);
                 throw new Exception(err);
             }
         }

         /// <summary>
         /// 根据参数列表组装成以列表中参数为列名的ds
         /// 提供给日志查询
         /// </summary>
         /// <param name="keyNameList">参数列表</param>
         /// <returns></returns>
         public static DataSet GetKFLogKeyNameDS(ArrayList keyNameList)
         {
             DataSet dsParam = new DataSet();

             foreach (string para in keyNameList)
             {
                 DataTable dt = new DataTable();
                 dsParam.Tables.Add(dt);
                 dsParam.Tables[0].Columns.Add(para.Trim());//获取参数名
             }

             return dsParam;
         }

         /// <summary>
         /// 客服系统写日志
         /// </summary>
         /// <param name="FObjID">不重复id</param>
         /// <param name="log_type">业务类型</param>
         /// <param name="key_name">关键字段 可用于查询</param>
         /// <param name="key_value">关键字段值</param>
         /// <param name="update_user">更新user</param>
         /// <param name="myParams">参数列表</param>
         /// <returns></returns>
         public static void WirteKFLog(string FObjID, string log_type, string key_name, string key_value, string update_user, Param[] myParams)
         {
             var da = MySQLAccessFactory.GetMySQLAccess("DataSource_ht");
             try
             {
                     da.OpenConn();//Fstate 0 有效 1 删除
                     string Sql = string.Format("Insert c2c_fmdb.t_log_kf_all(FObjID,Flog_type,Fkey_name,Fkey_value,Fupdate_user,Fmodify_time) values('{0}','{1}','{2}','{3}','{4}','{5}')",
                        FObjID, log_type, key_name, key_value, update_user, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                     if (!da.ExecSql(Sql))
                     {
                         throw new Exception("t_check_main 添加记录失败");
                     }

                     Sql = "select Max(FID) from c2c_fmdb.t_log_kf_all where FObjID='" + FObjID + "'";
                     string id = da.GetOneResult(Sql);

                     AddWirteKFLogParam(id, myParams, da);
                     da.Commit();
             }
             catch (Exception ex)
             {
                 da.RollBack();
                 string err = "添加日志记录出错：log_type:" + log_type + " key_name:" + key_name + " key_value:" + key_value+"  "+ex;
                 LogHelper.LogInfo(err);
                 throw new Exception(err);
             }
             finally
             {
                 da.Dispose();
             }
         }

         /// <summary>
         /// 加入参数。
         /// </summary>
         /// <param name="objid">日志id</param>
         /// <param name="myParams">参数列表</param>
         /// <param name="da"></param>
         private static void AddWirteKFLogParam(string id, Param[] myParams, MySqlAccess da)
         {
             if (myParams == null || myParams.Length == 0)
                 throw new Exception("日志参数列表为null");
             try
             {
                 foreach (Param aparam in myParams)
                 {
                     string strSql = "insert c2c_fmdb.t_log_kf_param(Flog_id,Fkey,Fvalue) values(" + id
                         + ",'" + aparam.ParamName + "','" + aparam.ParamValue + "')";
                     da.ExecSql(strSql);
                 }
             }
             catch (Exception ex)
             {
                 throw new Exception("t_log_kf_param 添加记录出错：" + ex.Message);
             }
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

        public static string FenToYuan(string strfen)
        {
            if (strfen == "")
            {
                strfen = "0";
            }

            double yuan = (double)(Int64.Parse(strfen)) / 100;
            yuan = Math.Round(yuan, 2);

            string tmp = yuan.ToString();
            int iindex = tmp.IndexOf(".");
            if (iindex == -1) tmp += ".00";
            if (iindex == tmp.Length - 2) tmp += "0";

            return tmp;
        }

}
