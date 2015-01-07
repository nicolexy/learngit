using System;
using System.Configuration;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.Common;
using System.Data;
using System.Collections;
using System.IO;
using System.Web.Mail;

using TENCENT.OSS.C2C.Finance.DataAccess;
using TENCENT.OSS.C2C.Finance.Common;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace TENCENT.OSS.CFT.KF.KF_Service
{

    public class QueryInfo
    {
        /// <summary>
        /// 从SELECT语句中返回一个DATATABLE。方便.NET中WEB页的显示。
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static DataSet GetTable(string strSql, int iPageStart, int iPageMax)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString());
            //			DataTable result = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                //				result = da.GetTable(strSql);
                ds = da.dsGetTableByRange(strSql, iPageStart, iPageMax);

                return ds;
            }
            finally
            {
                da.Dispose();
            }
        }

        public static DataSet GetTable(string strSql, int iPageStart, int iPageMax, string dbStr)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString(dbStr));
            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                ds = da.dsGetTableByRange(strSql, iPageStart, iPageMax);
                da.Dispose();
                return ds;
            }
            catch (Exception e)
            {
                da.Dispose();
                throw e;
            }
        }

        public static DataSet GetTable_Conn(string strSql, int iPageStart, int iPageMax, string connstr)
        {
            MySqlAccess da = new MySqlAccess(connstr);
            //			DataTable result = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                da.OpenConn();
                //				result = da.GetTable(strSql);
                ds = da.dsGetTableByRange(strSql, iPageStart, iPageMax);

                da.Dispose();
                return ds;
            }
            catch (Exception e)
            {
                da.Dispose();
                throw e;
            }
        }

        public static int CHAR_HASH(char c)
        {
            return (c - '0') % 10;
        }

        public static string GetDbNum(string uin)
        {
            //是QQ号
            if (uin.Length >= 5 && uin.Length <= 10 & !uin.StartsWith("0"))
            {
                return uin;
            }

            //email用户,一般不会低于5位
            if (uin.Length < 5)
            {
                return "00000";
            }
            char[] chUin = uin.ToCharArray();
            // Email：前2位分库
            int iDb = CHAR_HASH(chUin[0]) * 10 + CHAR_HASH(chUin[1]);

            //如果CHAR_HASH 返回负数，取绝对值
            if (iDb < 0)
            {
                iDb = -iDb;
            }

            // Email：第3位分表
            int iTbl = CHAR_HASH(chUin[2]);

           
            //如果CHAR_HASH 返回负数，取绝对值
            if (iTbl < 0)
            {
                iTbl = -iTbl;

            }

            //主机
            int iHost = CHAR_HASH(chUin[3]) * 10 + CHAR_HASH(chUin[4]);

            //如果CHAR_HASH 返回负数，取绝对值
            if (iHost < 0)
            {
                iHost = -iHost;
            }
            int sum = iDb + iTbl * 100 + iHost * 1000;
            return sum.ToString();
        }

        public static int GetDBNode(string uin)
        { 
            int m_uDBNodeCount = 3;//三台机器
            int uNode = Int32.Parse(uin) % 1367 % m_uDBNodeCount;
            return uNode;
        }

        #region 从数据表中转换出来值（同时进行NULL处理）
        public static string GetInt(object aValue)
        {
            try
            {
                if (aValue != null)
                {
                    long num = long.Parse(aValue.ToString());
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


        public static string GetDateTime(object aValue)
        {
            try
            {
                if (aValue != null)
                {
                    return (DateTime.Parse(aValue.ToString())).ToString("yyyy-MM-dd HH:mm:ss");
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
        #endregion
    }


    public class Query_BaseForNET
    {
        protected string fstrSql;
        protected string fstrSql_count;

        protected string ICESql;
        protected string ICEcommand;
        /// <summary>
        /// 提供给.net调用的函数
        /// </summary>
        /// <returns></returns>
        public DataSet GetResultX(int iPageStart, int iPageMax)
        {
            //			DataSet result = new DataSet();
            //			result.Tables.Add(QueryInfo.GetTable(fstrSql));
            return QueryInfo.GetTable(fstrSql, iPageStart, iPageMax);
        }

        /// <summary>
        /// 提供给.net调用的函数
        /// </summary>
        /// <returns></returns>
        public DataSet GetResultX(int iPageStart, int iPageMax, string dbstr)
        {
            return QueryInfo.GetTable(fstrSql, iPageStart, iPageMax, dbstr);
        }
        
        //返回全部或者部分数据
        public DataSet GetResultX_AllAndLimit(int iPageStart, int iPageMax, string dbstr)
        {
            if (iPageStart == -1 && iPageMax == -1)
                return PublicRes.returnDSAll(fstrSql, dbstr);
            else
                return QueryInfo.GetTable(fstrSql, iPageStart, iPageMax, dbstr);
        }

        public DataSet GetResultX_Conn(int iPageStart, int iPageMax, string connstr)
        {
            //			DataSet result = new DataSet();
            //			result.Tables.Add(QueryInfo.GetTable(fstrSql));
            return QueryInfo.GetTable_Conn(fstrSql, iPageStart, iPageMax, connstr);
        }

        public DataSet GetResultX()
        {
            return PublicRes.returnDSAll(fstrSql, "ywb");
        }

        public DataSet GetResultX_ICE()
        {
            string errmsg = "";
            DataSet ds = CommQuery.GetDataSetFromICE(ICESql, ICEcommand, out errmsg);
            return ds;
        }

        // 2012/4/9
        public DataSet GetResultX_ICE(string serviceName)
        {
            string errmsg = "";
            DataSet ds = CommQuery.GetDataSetFromICE(ICESql, ICEcommand, false, serviceName, out errmsg, false);

            return ds;
        }

        public DataSet GetResultX(string dbstr)
        {
            return PublicRes.returnDSAll(fstrSql, dbstr);
        }

        public DataSet GetResultX_Conn(string connstr)
        {
            return PublicRes.returnDSAll_Conn(fstrSql, connstr);
        }
        public int GetCount()
        {
            return Int32.Parse(PublicRes.ExecuteOne(fstrSql_count, "ywb"));
        }

        public int GetCount(string dbstr)
        {
            return Int32.Parse(PublicRes.ExecuteOne(fstrSql_count, dbstr));
        }

        public int GetCountApeal(string dbstr)
        {
            if (fstrSql_count == "")
                return 0;
            else
                return Int32.Parse(PublicRes.ExecuteOne(fstrSql_count, dbstr));
        }

        public int GetCount_Conn(string connstr)
        {
            return Int32.Parse(PublicRes.ExecuteOne_Conn(fstrSql_count, connstr));
        }
    }

    #region 用户商家工具按钮表查询处理

    /// <summary>
    /// 用户商家工具按钮表的查询类处理
    /// </summary>
    public class Q_BUTTONINFO : Query_BaseForNET
    {
        private string f_strID;
        public Q_BUTTONINFO(string strID, int istr, int imax)
        {
            f_strID = strID;

            string tname = PublicRes.GetTableName("t_button_info", f_strID);
            string sWhereStr = " where " + PublicRes.GetSqlFromQQ(strID, "fowner_uin");

            string count = PublicRes.ExecuteOne("select count(*) from " + tname + sWhereStr, "ZJB");

            //fstrSql = "Select A.*,B.total from " 
            //+ tname + " A,(Select count(1) as total from " + tname + sWhereStr + ") B"
            fstrSql = "select *," + count + " as total from " + tname
                + sWhereStr
                + " ORDER By Fcreate_time DESC limit " + (istr - 1) + "," + imax;

        }
    }
    #endregion


    // 2011/7/21

    #region   2011/7/21


    /// <summary>
    /// 查询投资人签约解约信息
    /// </summary>
    public class Q_InvestorSignInfo : Query_BaseForNET
    {
        public Q_InvestorSignInfo(int signType, string strID, string serialno, string cerNum, string spid, string spName, string beginTimeStr
            , string endTimeStr, int lim_start, int lim_count)
        {
            // finace.xml对应的SQL命令名称
            this.ICEcommand = "QUERY_SIGNINFO_BYDETAIL";

            if (signType != 1 && signType != 2)
                throw new Exception("该查询不支持其他的搜索类型");

            this.ICESql = "fop_type=" + signType;

            string fuid1 = "";
            string fuid2 = "";

            if (strID != null && strID.Trim() != "")
            {
                fuid1 = PublicRes.ConvertToFuid(strID);

                if (fuid1 == null || fuid1.Trim() == "")
                    throw new Exception("财付通帐号无效，请确认输入");

                this.ICESql += "&uid=" + fuid1;
            }

            if (serialno != null && serialno.Trim() != "")
                this.ICESql += "&cft_serialno=" + serialno;

            /*
            if(cerNum != null && cerNum.Trim() != "")
            {
                string errMsg = "";

                fuid2 = CommQuery.GetOneResultFromICE("cre_id="+cerNum,"QUERY_UID_BYCERINFO","Fuid",out errMsg);

                if(fuid1 != null && fuid1.Trim() != "" && fuid1.Trim() != "0" && fuid2 != fuid1)
                {
                    throw new Exception("证件号和财付通帐号不匹配，搜索错误！");
                }

                if(fuid1 == null || fuid1.Trim() == "" || fuid1.Trim() == "0")
                {
                    this.ICESql += "&uid=" + fuid2;
                }
            }
            */

            if (spid != "")
                this.ICESql += "&spid=" + spid;

            if (spName != null && spName.Trim() != "")
                this.ICESql += "&sp_uname=" + spName;

            if (beginTimeStr.Trim() != "" && endTimeStr.Trim() != "")
            {
                this.ICESql += "&starttime=" + beginTimeStr;
                this.ICESql += "&endtime=" + endTimeStr;
            }

            this.ICESql += "&lim_start=" + lim_start;
            this.ICESql += "&lim_count=" + lim_count;
        }
    }


    public class Q_FundInfo : Query_BaseForNET
    {
        /// <summary>
        /// 以财付通订单号帐号或基金宝帐号或两者一齐查询基金宝信息
        /// </summary>
        /// <param name="listID"></param>
        /// <param name="findDate"></param>
        /// <param name="limStart"></param>
        /// <param name="limCount"></param>
        public Q_FundInfo(string listID, string uid, string beginDateStr, string endDateStr, string fundType, int limStart, int limCount)
        {
            if (listID != null && listID.Trim() != "")
            {
                this.ICEcommand = "QUERY_FUNDINFO_LISTID";

                this.ICESql += "listid=" + listID;

                if (fundType != "" && fundType != "0")
                {
                    this.ICESql += "&purtype=" + fundType;
                }

                this.ICESql += "&lim_start=" + limStart;
                this.ICESql += "&lim_count=" + limCount;

                return;
            }
            else if (uid != null || uid.Trim() != "")
            {
                this.ICEcommand = "QUERY_FUNDINFO_UID";

                string fuid = PublicRes.ConvertToFuid(uid);

                if (fuid == null || fuid.Trim() == "")
                    fuid = "0";

                this.ICESql += "uid=" + fuid;
                // 测试用的数据
                //this.ICESql += "uid=" + 0;
            }


            if (beginDateStr != null && beginDateStr.Trim() != "" && endDateStr != null && endDateStr.Trim() != "")
            {
                //后台需要的格式是yyyyMMdd 
                beginDateStr = DateTime.Parse(beginDateStr).ToString("yyyyMMdd");
                endDateStr = DateTime.Parse(endDateStr).ToString("yyyyMMdd");

                this.ICESql += "&begin_date=" + beginDateStr;
                this.ICESql += "&end_date=" + endDateStr;
            }

            if (fundType != "" && fundType != "0")
                this.ICESql += "&purtype=" + fundType;


            this.ICESql += "&lim_start=" + limStart;
            this.ICESql += "&lim_count=" + limCount;
        }

    }




    public class Q_ChargeInfo : Query_BaseForNET
    {
        public Q_ChargeInfo(string qType, string strID, string beginDateStr, string endDateStr, string listid, int lim_start, int lim_count)
        {
            string fuid;
            if (strID != null && strID.Trim() != "")
            {
                this.ICEcommand = "QUERY_USER_BANKROLL_FULL";

                fuid = PublicRes.ConvertToFuid(strID);

                if (fuid == null || fuid.Trim() == "")
                    fuid = "0";

                this.ICESql = "uid=" + fuid;
            }
            else
            {
                throw new Exception("查询必须输入用户帐号");
            }

            // 如果有listid查询，则使用QUERY_BANKROLL_LISTID_2，而这个接口只接受2个参数
            if (listid != null && listid.Trim() != "")
            {
                this.ICEcommand = "QUERY_BANKROLL_LISTID_2";

                this.ICESql += "&listid=" + listid;
            }
            else
            {
                if (qType != "" && qType == "99")
                {
                    this.ICESql += "&subject_list=11,14";
                }
                else if (qType != "" && qType != "0")
                {
                    this.ICESql += "&subject_list=" + qType;
                }

                if (beginDateStr != "" && endDateStr != "")
                {
                    this.ICESql += "&start_time=" + beginDateStr;
                    this.ICESql += "&end_time=" + endDateStr;
                }

                this.ICESql += "&lim_start=" + lim_start;
                this.ICESql += "&lim_count=" + lim_count;

                // curType=2代表基金
                this.ICESql += "&curtype=2";
            }
            string str = this.ICESql;
        }
    }



    #endregion

    // 2012/4

    #region  2012/4

    // 查实名认证中(过程表)信息
    public class QueryUserAuthenStateInfo : Query_BaseForNET
    {
        public QueryUserAuthenStateInfo(string userAcc, string bankID, int bankType)
        {
            if (userAcc.Trim() != "")
            {
                this.ICEcommand = "FINANCE_QUERY_USR_AUTHENING";

                this.ICESql = "qqid=" + userAcc;
            }
            else if (bankID.Trim() != "")
            {
                this.ICEcommand = "FINANCE_QUERY_CARD_AUTHENING";

                this.ICESql = "bank_id=" + bankID + "&bank_type=" + bankType;
            }
            else
            {
                throw new Exception("查询条件必须填写！");
            }
        }
    }


    // 查已通过认证信息
    public class QueryUserAuthenedStateInfo : Query_BaseForNET
    {
        public QueryUserAuthenedStateInfo(string userAcc)
        {
            this.ICEcommand = "FINANCE_QUERY_USER_AUTHENED";
            string fuid = PublicRes.ConvertToFuid(userAcc);
            this.ICESql = "uid=" + fuid;
        }
    }



    public class QueryAuthenRelationInfo : Query_BaseForNET
    {
        public QueryAuthenRelationInfo(string firstAuthenID)
        {
            if (firstAuthenID == null || firstAuthenID.Trim() == "")
            {
                throw new Exception("firstAuthenID不能为空！");
            }

            this.ICEcommand = "FINANCE_QUERY_RELATION";

            this.ICESql = "uid=" + firstAuthenID;
        }
    }



    public class QueryAuthenInfo_ByCre2 : Query_BaseForNET
    {
        public QueryAuthenInfo_ByCre2(string creid, int creType)
        {
            if (creid == null || creid.Trim() == "")
                throw new Exception("证件号不能为空");

            this.ICEcommand = "FINANCE_QUERY_CRE_RELATION";

            this.ICESql = "cre_id=" + creid + "&cre_type=" + creType;
        }
    }







    #endregion

    // 2012/6
    #region	代扣

    public class QueryDKInfo : Query_BaseForNET
    {
        public QueryDKInfo(string cep_id, string strBeginDate, string strEndDate)
        {
            this.ICEcommand = "FINANCE_QUERY_CEPSPLIST";

            this.ICESql += "cep_id=" + cep_id;
            this.ICESql += "&stime=" + strBeginDate;
            this.ICESql += "&etime=" + strEndDate;
            //this.ICESql += "&lim_start=0&lim_count=1";
            this.ICESql += "&strlimit=limit 0,1";
        }

        public QueryDKInfo(string explain, string bankID, string userID, string strBeginDate, string strEndDate, string spid, string spListID
            , string spBatchID, string cep_id,string state, string transaction_id, string bank_type, string service_code, int limStart, int limMax)
        {
            string strWhere = "";
            strWhere = " where Fcreate_time between '" + strBeginDate + "' and '" + strEndDate + "' ";
            if (spid.Trim() != "")
                strWhere += " and Fspid='" + spid.Trim() + "' ";
            if (bankID.Trim() != "")
                strWhere += " and Fbankacc_no='" + bankID.Trim() + "' ";
            if (userID.Trim() != "")
                strWhere += " and Funame='" + userID.Trim() + "' ";
            if (spListID.Trim() != "")
                strWhere += " and Fcoding='" + spListID.Trim() + "' ";
            if (spBatchID.Trim() != "")
                strWhere += " and Fsp_batchid='" + spBatchID.Trim() + "' ";
            if (cep_id.Trim() != "")
                strWhere += " and Fcep_id='" + cep_id.Trim() + "' ";
            if (state.Trim() != "" && state != "0")
            {
                if (state == "3")
                    strWhere += " and Fstate<8 ";
                if (state == "1")
                    strWhere += " and Fstate=8";//状态8数据库中成功
                if (state == "2")
                    strWhere += " and Fstate=9";//状态9数据库中失败
            }

            if (transaction_id.Trim() != "")
                strWhere += " and Ftransaction_id='" + transaction_id + "' ";
            if (bank_type.Trim() != "")
                strWhere += " and Fbank_type='" + bank_type + "' ";
            if (service_code.Trim() != "9999999")
                strWhere += " and Fservice_code like '%" + service_code + "' ";
            if (explain.Trim() != "")
                strWhere += " and Fexplain like'" + explain + "%' ";

            fstrSql = " select * from cft_cep_db.t_cep_list " + strWhere;
            fstrSql_count = " select count(*) from cft_cep_db.t_cep_list " + strWhere;


            this.ICEcommand = "FINANCE_QUERY_CEPSPLIST";

            this.ICESql += "stime=" + strBeginDate;
            this.ICESql += "&etime=" + strEndDate;

            if (spid.Trim() != "")
                this.ICESql += "&sp_id=" + spid;

            if (bankID.Trim() != "")
                this.ICESql += "&bankacc_no=" + bankID;

            if (userID.Trim() != "")
                this.ICESql += "&name=" + userID;

            if (spListID.Trim() != "")
                this.ICESql += "&coding=" + spListID;

            if (spBatchID.Trim() != "")
                this.ICESql += "&sp_batchid=" + spBatchID;

            if (state.Trim() != "")
            {
                // 这里有待和页面确定
                if (state == "3")
                {
                    this.ICESql += "&processing=";
                }
                else if (state == "1")
                {
                    this.ICESql += "&success_state=";
                }
                else if (state == "2")
                {
                    this.ICESql += "&failed_state=";
                }
            }

            this.ICESql += "&upbymodfy=";

            //this.ICESql += "&lim_start=" + limStart + "&lim_count=" + limMax;
            this.ICESql += "&strlimit=limit " + limStart + "," + limMax;

            string str = this.ICESql;
        }
    }



    public class QueryBatchDKInfo : Query_BaseForNET
    {
        public QueryBatchDKInfo(string strBeginDate, string strEndDate, string spid, string spBatchID, string batchid, string state, int limStart, int limMax)
        {
            string strWhere = " where Fcreate_time between '" + strBeginDate + "' and '" + strEndDate + "' ";
            if (spid.Trim() != "")
            {
                strWhere += " and Fspid='" + spid + "' ";
            }

            if (spBatchID.Trim() != "")
                strWhere += " and Fsp_batchid='" + spBatchID + "' ";

            if (batchid.Trim() != "")
                strWhere += " and Fbatchid='" + batchid + "' ";

            if (state != "0")
                strWhere += " and Fstate=" + state;

            fstrSql = " select * from cft_cep_db.t_batch_record " + strWhere;
            fstrSql_count = " select count(*) from cft_cep_db.t_batch_record " + strWhere;

            this.ICEcommand = "FINANCE_QUERY_CEPSPBATCH";

            this.ICESql += "stime=" + strBeginDate;
            this.ICESql += "&etime=" + strEndDate;

            if (spid.Trim() != "")
                this.ICESql += "&sp_id=" + spid;

            if (spBatchID.Trim() != "")
                this.ICESql += "&sp_batchid=" + spBatchID;

            //this.ICESql += "&upbymodfy=";

            this.ICESql += "&strlimit=limit " + limStart + "," + limMax;

            string str = this.ICESql;

        }
    }



    public class CepspService : Query_BaseForNET
    {
        public CepspService(string spid, string service_code)
        {
            this.ICEcommand = "FINANCE_QUERY_CEPSPSERVICE";

            this.ICESql += "sp_id=" + spid;
            this.ICESql += "&service_code=" + service_code;
        }
    }


    #endregion


    #region 用户账户表查询处理

    /// <summary>
    /// 用户帐户表的查询类处理
    /// </summary>
    public class Q_USER : Query_BaseForNET
    {
        private string f_strID;
        public string FlagForTable;
        public Q_USER(string fuid, int fcurtype)
        {
          //  f_strID = strID;

         //   string fuid = PublicRes.ConvertToFuid(strID);//转fuid转到调用函数
            if (fuid == null)
                fuid = "0";

            // TODO: 1客户信息资料外移
            //先把email和mobile从t_user_info中取出,再放入此SQL中.
            //MySqlAccess da_zl = new MySqlAccess(PublicRes.GetConnString("ZL"));
            string femail = "";
            string fmobile = "";
            string fatt_id = "";
            string ftrueName = "";
            string fz_amt = ""; //分账冻结金额 yinhuang 2014/1/8

            ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP, PublicRes.ICEPort);
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("AP"));
            try
            {
                string errMsg = "";
                string strSql = "uid=" + fuid;
                //femail = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Femail", out errMsg);
                //fmobile = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Fmobile", out errMsg);
                fatt_id = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERATT, "Fatt_id", out errMsg);

                fatt_id = QueryInfo.GetString(fatt_id);

                DataTable dt_userInfo = CommQuery.GetTableFromICE(strSql, CommQuery.QUERY_USERINFO, out errMsg);
                if (dt_userInfo != null && dt_userInfo.Rows.Count == 1)
                {
                    femail = dt_userInfo.Rows[0]["Femail"].ToString();
                    fmobile = dt_userInfo.Rows[0]["Fmobile"].ToString();
                    ftrueName = dt_userInfo.Rows[0]["FtrueName"].ToString();

                    string fusertype = QueryInfo.GetString(dt_userInfo.Rows[0]["Fuser_type"]);
                    if (fusertype == "2")//公司类型
                    {
                        ftrueName = dt_userInfo.Rows[0]["Fcompany_name"].ToString();
                    }
                }

                ice.OpenConn();
                string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + fuid + "&");
                strwhere += ICEAccess.URLEncode("fcurtype="+fcurtype+"&");

                string strResp = "";
                DataTable dt = ice.InvokeQuery_GetDataTable(YWSourceType.用户资源, YWCommandCode.查询用户信息, fuid, strwhere, out strResp);
                if (dt == null || dt.Rows.Count == 0)
                    throw new LogicException("调用ICE查询T_user无记录" + strResp);

                // 2012/5/2 因为需要Q_USER_INFO获取准确的用户真实姓名而改动
                //取消多次取t_user_info
                /*
                dt.Columns.Add("UserRealName2", typeof(string));

                try
                {
                    Q_USER_INFO cuser = new Q_USER_INFO(fuid);
                    DataSet ds2 = cuser.GetResultX(1, 1, "ZW");

                    if (ds2 != null && ds2.Tables.Count != 0 && ds2.Tables[0].Rows.Count != 0)
                    {
                        dt.Rows[0]["UserRealName2"] = ds2.Tables[0].Rows[0]["Ftruename"].ToString();
                    }
                }
                catch
                { }
                */

                ice.CloseConn();

                da.OpenConn();
                string sql = "select * from app_platform.t_account_freeze where Fuid = '" + fuid + "'";
                DataTable dt2 = da.GetTable(sql);
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    fz_amt = dt2.Rows[0]["Famount"].ToString();
                }

                //用dt里的一条记录组合出select语句。
                string strtmp = " select ";
                foreach (DataColumn dc in dt.Columns)
                {
                    string valuetmp = QueryInfo.GetString(dt.Rows[0][dc.ColumnName]);
                    strtmp += " '" + valuetmp + "' as " + dc.ColumnName + ",";
                }

                fstrSql = strtmp + "'" + femail + "' as Femail,'" + fmobile + "' as Fmobile, '" + fatt_id + "' as Att_id, '" + ftrueName + "' as UserRealName2, '" + fz_amt + "' as Ffz_amt ";
            }
            finally
            {
                ice.Dispose();
                da.Dispose();
            }
        }

        //为子帐户专用
        public Q_USER(string strID, string Fcurtype)
        {
            f_strID = strID;

            string fuid = PublicRes.ConvertToFuid(strID);
            if (fuid == null)
                fuid = "0";

            // TODO: 1客户信息资料外移

            //MySqlAccess da_zl = new MySqlAccess(PublicRes.GetConnString("ZL"));

            ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP, PublicRes.ICEPort);
            string femail = "";
            string fmobile = "";
            try
            {
                /*
                da_zl.OpenConn();

                string strSql = "select Femail from " + PublicRes.GetTName("t_user_info",fuid) + " where fuid=" + fuid;
				
                femail = QueryInfo.GetString(da_zl.GetOneResult(strSql));

                strSql = "select Fmobile from " + PublicRes.GetTName("t_user_info",fuid) + " where fuid=" + fuid;
				
                fmobile = QueryInfo.GetString(da_zl.GetOneResult(strSql));
                */

                string errMsg = "";
                string strSql = "uid=" + fuid;
                femail = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Femail", out errMsg);
                fmobile = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Fmobile", out errMsg);

                femail = QueryInfo.GetString(femail);
                fmobile = QueryInfo.GetString(fmobile);
            }
            finally
            {
                //da_zl.Dispose();
            }

            /*
            string FlagTable = PublicRes.GetTName("t_user",fuid);
            fstrSql = "Select *,'" + femail +"' as Femail,'" + fmobile + "' as Fmobile from " + FlagTable + "  where fuid='" + fuid + "' and Fcurtype='" + Fcurtype + "'";

            if(FlagTable.IndexOf("t_user") >= 0)
                FlagForTable = "USER";
                */

            ice.OpenConn();
            string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + fuid + "&");
            strwhere += ICEAccess.URLEncode("fcurtype=" + Fcurtype + "&");

            string strResp = "";
            DataTable dt = ice.InvokeQuery_GetDataTable(YWSourceType.用户资源, YWCommandCode.查询用户信息, fuid, strwhere, out strResp);
            if (dt == null || dt.Rows.Count == 0)
                throw new LogicException("调用ICE查询T_user无记录" + strResp);

            ice.CloseConn();

            //用dt里的一条记录组合出select语句。
            string strtmp = " select ";
            foreach (DataColumn dc in dt.Columns)
            {
                string valuetmp = QueryInfo.GetString(dt.Rows[0][dc.ColumnName]);
                strtmp += " '" + valuetmp + "' as " + dc.ColumnName + ",";
            }

            fstrSql = strtmp + "'" + femail + "' as Femail,'" + fmobile + "' as Fmobile ";

        }

        /// <summary>
        /// 提供给其它语言调用的函数，以固定的类返回值。
        /// </summary>
        /// <returns></returns>
        public T_USER GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZL"));
            T_USER result = new T_USER();
            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    result.u_APay = QueryInfo.GetInt(dr["fapay"]);
                    result.u_Balance = QueryInfo.GetInt(dr["fBalance"]);
                    result.u_Con = QueryInfo.GetInt(dr["fcon"]);
                    result.u_CurType = QueryInfo.GetInt(dr["fcurtype"]);
                    result.u_Fetch_Time = QueryInfo.GetDateTime(dr["ffetch_time"]);
                    result.u_Login_IP = QueryInfo.GetString(dr["flogin_ip"]);
                    result.u_Memo = QueryInfo.GetString(dr["fmemo"]);
                    result.u_Modify_Time = QueryInfo.GetDateTime(dr["fmodify_time"]);
                    result.u_Modify_Time_C2C = QueryInfo.GetDateTime(dr["fmodify_time_c2c"]);
                    result.u_Quota = QueryInfo.GetInt(dr["fquota"]);
                    result.u_Quota_Pay = QueryInfo.GetInt(dr["fquota_pay"]);
                    result.u_Save_Time = QueryInfo.GetDateTime(dr["fsave_time"]);
                    result.u_State = QueryInfo.GetInt(dr["fstate"]);
                    result.u_TrueName = QueryInfo.GetString(dr["ftruename"]);
                    result.u_Yday_Balance = QueryInfo.GetInt(dr["fyday_balance"]);
                    result.u_User_Type = QueryInfo.GetInt(dr["fuser_type"]);

                    result.u_QQID = QueryInfo.GetString(dr["fqqid"]);
                }
                else
                {
                    throw new Exception("没有查找到相应的记录");
                }
                da.CloseConn();
                da.Dispose();
                return result;
            }
            catch (Exception e)
            {
                da.CloseConn();
                da.Dispose();
                throw e;
            }
        }
    }

    #endregion


    #region 中介帐户表的查询处理


    /// <summary>
    /// 中介账户表的查询类
    /// </summary>
    public class Q_USER_MED : Query_BaseForNET
    {
        private string f_strID;

        //查询得到所有的商户数据集 edwardzheng 20051110
        public Q_USER_MED()
        {
            /*
            //fstrSql = "Select Fuid,FQqid,FSpid,Ftruename from c2c_db.t_middle_user ORDER BY FSpid";
            fstrSql = "Select FuidMiddle as Fuid,Fspecial as Fqqid,FSpid,Fname as Ftruename from c2c_db.t_merchant_info ORDER BY FSpid";
            */
            //现在应该不再存在取所有商户信息的地方.
            fstrSql = "Select '10001' as Fuid,'10001' as Fqqid,'1000000000' as FSpid,'test' as Ftruename";
        }

        public Q_USER_MED(string strID)
        {
            /*
            //f_strID = PublicRes.ExecuteOne("select fuid from c2c_db.t_middle_user where fspid='" + strID + "'","YW_30");
            //f_strID = PublicRes.ExecuteOne("select fuid from c2c_db.t_middle_user where fspid='" + strID + "'","YWB_30");
            f_strID = PublicRes.ExecuteOne("select fuidMiddle from c2c_db.t_merchant_info where fspid='" + strID + "'","ZL");
            */

            string strSql = "spid=" + strID;
            string errMsg = "";
            f_strID = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MERCHANTINFO, "FuidMiddle", out errMsg);

            if (f_strID == null || f_strID.Trim() == "")
            {
                throw new LogicException("查找不到指定的记录，请确认你的输入是否正确！" + errMsg);
            }


            //MySqlAccess da_zl = new MySqlAccess(PublicRes.GetConnString("ZL"));



            //furion 20080707 因为当前获取方法，相同字段名称的话，会取最后一次出现字段的值。
            string strusertype = "";

            string strtmp = " ";

            ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP, PublicRes.ICEPort);
            try
            {
                //da_zl.OpenConn();

                ice.OpenConn();
                string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + f_strID + "&");
                strwhere += ICEAccess.URLEncode("fcurtype=1&");

                string strResp = "";

                //3.0接口测试需要 furion 20090708
                DataTable dt = ice.InvokeQuery_GetDataTable(YWSourceType.商户资源, YWCommandCode.查询商户信息, f_strID, strwhere, out strResp);
                if (dt == null || dt.Rows.Count == 0)
                    throw new LogicException("调用ICE查询T_user无记录" + strResp);

                ice.CloseConn();

                //				string strSql  = "Select "+
                //					"C.FUid AS FBindUid,C.FFeeContract,C.FContract,C.Fspecial AS FBindQQid,C.FUserId AS FModifyUser,"+
                //					"C.FModify_time AS FModifyTime,C.Fmer_key,C.Fstate,C.Flist_state,"+
                //					"C.Fstandby1,C.Fstandby2,C.Fstandby3,C.Fstandby4,C.Fstandby5,C.Fstandby6,C.Fstandby7,C.Fstandby8,C.Fstandby9,C.Fstandby10,"+
                //					"C.Fmemo AS FExtMemo,C.FIp AS FModifyIp,B.* "+
                //					" from  " + PublicRes.GetTName("t_user_info",f_strID) + " B "+
                //					" LEFT OUTER JOIN c2c_db.t_merchant_info C ON C.Fuidmiddle=B.Fuid "+
                //					" where B.Fuid ='" + f_strID + "'";
                //
                //				DataTable dt = da_zl.GetTable(strSql);

                //				if(dt ==null || dt.Rows.Count != 1)
                //				{
                //					throw new LogicException("查找不到指定的记录，请确认你的输入是否正确！");
                //				}



                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.ColumnName.ToLower() == "fuser_type" || dc.ColumnName.ToLower() == "fatt_id")
                    {
                        //strusertype = " '" + valuetmp + "' as " + dc.ColumnName;
                        continue;
                    }

                    string valuetmp = QueryInfo.GetString(dt.Rows[0][dc.ColumnName]);
                    strtmp += " '" + valuetmp + "' as " + dc.ColumnName + ",";
                }

                strtmp = strtmp.Remove(strtmp.Length - 1, 1);
            }
            finally
            {
                //da_zl.Dispose();
                ice.Dispose();
            }

            /*
            // TODO: 1客户信息资料外移
            //			fstrSql = "Select "+
            //				"C.FUid AS FBindUid,C.FFeeContract,C.FContract,C.Fspecial AS FBindQQid,C.FUserId AS FModifyUser,"+
            //				"C.FModify_time AS FModifyTime,C.Fmer_key,C.Fstate,C.Flist_state,"+
            //				"C.Fstandby1,C.Fstandby2,C.Fstandby3,C.Fstandby4,C.Fstandby5,C.Fstandby6,C.Fstandby7,C.Fstandby8,C.Fstandby9,C.Fstandby10,"+
            //				"C.Fmemo AS FExtMemo,C.FIp AS FModifyIp,A.*,B.* "+
            //				" from c2c_db.t_middle_user A LEFT OUTER JOIN " + PublicRes.GetTName("t_user_info",f_strID) + " B ON B.Fuid=A.Fuid"+
            //				" LEFT OUTER JOIN c2c_db.t_merchant_info C ON A.Fspid=C.FSpid"+
            //				" where A.Fuid ='" + f_strID + "'";

            //furion 20080707
            fstrSql  = "select " +
                "C.FUid AS FBindUid,C.FFeeContract,C.FContract,C.Fspecial AS FBindQQid,C.FUserId AS FModifyUser,"+
                "C.FModify_time AS FModifyTime,C.Fmer_key,C.Fstate,C.Flist_state,"+
                "C.Fstandby1,C.Fstandby2,C.Fstandby3,C.Fstandby4,C.Fstandby5,C.Fstandby6,C.Fstandby7,C.Fstandby8,C.Fstandby9,C.Fstandby10,"+
                "C.Fmemo AS FExtMemo,C.FIp AS FModifyIp,B.*, " + strtmp +
                " from  " + PublicRes.GetTName("t_user_info",f_strID) + " B "+
                " LEFT OUTER JOIN c2c_db.t_merchant_info C ON C.Fuidmiddle=B.Fuid "+
                " where B.Fuid ='" + f_strID + "'";

//			fstrSql = strtmp + "A.* ," + strusertype +
//				" from c2c_db.t_middle_user A  where A.Fuid ='" + f_strID + "'";
*/

            errMsg = "";
            strSql = "spid=" + strID;
            DataTable dtc = CommQuery.GetTableFromICE(strSql, CommQuery.QUERY_MERCHANTINFO, out errMsg);

            if (dtc == null || dtc.Rows.Count != 1)
            {
                throw new LogicException(errMsg);
            }

            string csql = "";
            DataRow dr = dtc.Rows[0];
            csql += " '" + QueryInfo.GetString(dr["Fuid"]) + "' as FBindUid,";
            csql += " '" + QueryInfo.GetString(dr["FFeeContract"]) + "' as FFeeContract,";
            csql += " '" + QueryInfo.GetString(dr["FContract"]) + "' as FContract,";
            csql += " '" + QueryInfo.GetString(dr["Fspecial"]) + "' as FBindQQid,";

            csql += " '" + QueryInfo.GetString(dr["FUserId"]) + "' as FModifyUser,";
            csql += " '" + QueryInfo.GetString(dr["FModify_time"]) + "' as FModifyTime,";
            csql += " '" + QueryInfo.GetString(dr["Fmer_key"]) + "' as Fmer_key,";
            csql += " '" + QueryInfo.GetString(dr["Fstate"]) + "' as Fstate,";
            csql += " '" + QueryInfo.GetString(dr["Flist_state"]) + "' as Flist_state,";

            csql += " '" + QueryInfo.GetString(dr["Fmemo"]) + "' as FExtMemo,";
            csql += " '" + QueryInfo.GetString(dr["FIp"]) + "' as FModifyIp,";

            csql += " '" + QueryInfo.GetString(dr["Fstandby1"]) + "' as Fstandby1,";
            csql += " '" + QueryInfo.GetString(dr["Fstandby2"]) + "' as Fstandby2,";
            csql += " '" + QueryInfo.GetString(dr["Fstandby3"]) + "' as Fstandby3,";
            csql += " '" + QueryInfo.GetString(dr["Fstandby4"]) + "' as Fstandby4,";
            csql += " '" + QueryInfo.GetString(dr["Fstandby5"]) + "' as Fstandby5,";
            csql += " '" + QueryInfo.GetString(dr["Fstandby6"]) + "' as Fstandby6,";
            csql += " '" + QueryInfo.GetString(dr["Fstandby7"]) + "' as Fstandby7,";
            csql += " '" + QueryInfo.GetString(dr["Fstandby8"]) + "' as Fstandby8,";
            csql += " '" + QueryInfo.GetString(dr["Fstandby9"]) + "' as Fstandby9,";
            csql += " '" + QueryInfo.GetString(dr["Fstandby10"]) + "' as Fstandby10,";


            strSql = "uid=" + f_strID;
            DataTable dtb = CommQuery.GetTableFromICE(strSql, CommQuery.QUERY_USERINFO, out errMsg);

            if (dtb == null || dtb.Rows.Count != 1)
            {
                throw new LogicException(errMsg);
            }

            string bsql = "";
            foreach (DataColumn dc in dtb.Columns)
            {
                string valuetmp = QueryInfo.GetString(dtb.Rows[0][dc.ColumnName]);
                bsql += " '" + valuetmp + "' as " + dc.ColumnName + ",";
            }


            fstrSql = "select " + csql + bsql + strtmp;

            strSql = "uid=" + f_strID;
            string Fatt_id = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERATT, "Fatt_id", out errMsg);

            fstrSql += "," + Fatt_id + " as Fatt_id";
        }

        /// <summary>
        /// 提供给其它语言调用的函数，以固定的类返回值。
        /// </summary>
        /// <returns></returns>
        public T_USER_MED GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZL"));
            T_USER_MED result = new T_USER_MED();
            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    result.GetInfoFromDB(dr);
                }
                else
                {
                    throw new LogicException("没有查找到相应的记录");
                }
                return result;
            }
            finally
            {
                da.CloseConn();
                da.Dispose();
            }
        }

        /// <summary>
        /// 提供给其它语言调用的函数，以固定的类返回值。
        /// </summary>
        /// <returns></returns>
        public T_USER_MED GetResult(string DBstr)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString(DBstr));
            T_USER_MED result = new T_USER_MED();
            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    result.LoadFromDB(dr);
                }
                else
                {
                    throw new LogicException("没有查找到相应的记录");
                }
                return result;
            }
            finally
            {
                da.CloseConn();
                da.Dispose();
            }
        }
    }

    #endregion


    #region 用户资料表的查询处理


    /// <summary>
    /// 用户资料表的查询类
    /// </summary>
    public class Q_USER_INFO : Query_BaseForNET
    {
        private string f_strID;
        public Q_USER_INFO(string fuid)
        {
          //  f_strID = strID;

          //  string fuid = PublicRes.ConvertToFuid(strID);

            // TODO: 1客户信息资料外移
            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("YWB_30"));
            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZL"));
            string Fatt_id = "";
            try
            {
                /*
                da.OpenConn();

                //string strSql = "select Fatt_id from " + PublicRes.GetTableName("t_user",f_strID)  + " where " + PublicRes.GetSqlFromQQ(strID,"fuid");
                string strSql = "select Fatt_id from " + PublicRes.GetTName("t_useratt_info",fuid)  + " where fuid=" + fuid;
				
                Fatt_id = QueryInfo.GetString(da.GetOneResult_OBJ(strSql));
                */

                string errmsg = "";
                string strSql = "uid=" + fuid;
                Fatt_id = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERATT, "Fatt_id", out errmsg);
            }
            finally
            {
                //da.Dispose();
            }

            /*
            //fstrSql = "Select * from " + PublicRes.GetTableName("t_user_info",f_strID) + " where " + PublicRes.GetSqlFromQQ(strID,"fuid");
            //furion 20060816
            //			fstrSql = "Select A.*,B.Fatt_id from " + PublicRes.GetTableName("t_user_info",f_strID) + " A,"
            //                    + PublicRes.GetTableName("t_user",f_strID) + " B where A.Fuid=B.Fuid and A." + PublicRes.GetSqlFromQQ(strID,"fuid");

            fstrSql = "Select *,'" + Fatt_id + "' as Fatt_id from " + PublicRes.GetTableName("t_user_info",f_strID) + " where " + PublicRes.GetSqlFromQQ(strID,"fuid");
            */

            string errMsg = "";
            string strsql = "uid=" + fuid;
            DataTable dt = CommQuery.GetTableFromICE(strsql, CommQuery.QUERY_USERINFO, out errMsg);

            if (dt == null || dt.Rows.Count != 1)
            {
                throw new LogicException(errMsg);
            }

            fstrSql = "select ";
            foreach (DataColumn dc in dt.Columns)
            {
                string valuetmp = QueryInfo.GetString(dt.Rows[0][dc.ColumnName]);
                fstrSql += " '" + valuetmp + "' as " + dc.ColumnName + ",";
            }

            fstrSql += "'" + Fatt_id + "' as Fatt_id";

        }

        /// <summary>
        /// 提供给其它语言调用的函数，以固定的类返回值。
        /// </summary>
        /// <returns></returns>
        public T_USER_INFO GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZL"));
            T_USER_INFO result = new T_USER_INFO();
            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    result.u_Address = QueryInfo.GetString(dr["faddress"]);
                    result.u_Age = QueryInfo.GetInt(dr["fage"]);
                    result.u_Area = QueryInfo.GetString(dr["farea"]);
                    result.u_City = QueryInfo.GetString(dr["fcity"]);
                    result.u_Cre_Type = QueryInfo.GetInt(dr["fcre_type"]);
                    result.u_Credit = QueryInfo.GetInt(dr["fcredit"]);
                    result.u_CreID = QueryInfo.GetString(dr["fcreid"]);
                    result.u_Email = QueryInfo.GetString(dr["femail"]);
                    result.u_Memo = QueryInfo.GetString(dr["fmemo"]);
                    result.u_Mobile = QueryInfo.GetString(dr["fmobile"]);
                    result.u_Modify_Time = QueryInfo.GetDateTime(dr["fmodify_time"]);
                    result.u_PCode = QueryInfo.GetString(dr["fpcode"]);
                    result.u_Phone = QueryInfo.GetString(dr["fphone"]);
                    result.u_QQID = QueryInfo.GetString(dr["fqqid"]);
                    result.u_Sex = QueryInfo.GetInt(dr["fsex"]);
                    result.u_TrueName = QueryInfo.GetString(dr["ftruename"]);
                }
                else
                {
                    throw new Exception("没有查找到相应的记录");
                }
                da.CloseConn();
                da.Dispose();
                return result;
            }
            catch (Exception e)
            {
                da.CloseConn();
                da.Dispose();
                throw e;
            }
        }
    }

    #endregion


    #region 用户绑定银行帐户表的查询处理

    /// <summary>
    /// 用户绑定银行帐户表的查询类
    /// </summary>
    public class Q_BANK_USER : Query_BaseForNET
    {
        private string f_strID;
        public Q_BANK_USER(string strID)
        {
            // TODO: 1客户信息资料外移
            f_strID = strID;
            fstrSql = "Select * from " + PublicRes.GetTableName("t_fetch_bank", f_strID) + " where " + PublicRes.GetSqlFromQQ(strID, "fuid");
            string fuid = "";
            try
            {

                fuid = PublicRes.ConvertToFuid(f_strID);
            }
            catch (Exception e)
            {

                throw new Exception("此用户不存在," + e.Message.ToString());
            }
            ICESql = "uid=" + fuid;
            ICESql += "&curtype=1";

            ICEcommand = CommQuery.QUERY_BANKUSER;
        }
        public Q_BANK_USER(string strID, bool isbatch)
        {
            // TODO: 1客户信息资料外移
            f_strID = strID;
            fstrSql = "Select * from " + PublicRes.GetTableName("t_fetch_bank", f_strID) + " where " + PublicRes.GetSqlFromQQ(strID, "fuid");



            string fuid = PublicRes.ConvertToFuid(f_strID);

            ICESql = "uid=" + fuid;


            ICEcommand = CommQuery.BATCH_BANKUSER;


        }

        /// <summary>
        /// 提供给其它语言调用的函数，以固定的类返回值。
        /// </summary>
        /// <returns></returns>
        public T_BANK_USER[] GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZL"));

            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);
                if (dt.Rows.Count > 0)
                {
                    T_BANK_USER[] result = new T_BANK_USER[dt.Rows.Count];
                    int i = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        result[i] = new T_BANK_USER();

                        result[i].u_Area = QueryInfo.GetString(dr["farea"]); //
                        result[i].u_Bank_Name = QueryInfo.GetString(dr["fbank_name"]);//
                        result[i].u_BankID = QueryInfo.GetString(dr["fbankid"]); //
                        result[i].u_Bank_Type = QueryInfo.GetInt(dr["fbank_type"]);
                        result[i].u_City = QueryInfo.GetString(dr["fcity"]); //
                        result[i].u_Login_IP = QueryInfo.GetString(dr["flogin_ip"]);//
                        result[i].u_Memo = QueryInfo.GetString(dr["fmemo"]);//
                        result[i].u_Modify_Time = QueryInfo.GetDateTime(dr["fmodify_time"]);//
                        result[i].u_State = QueryInfo.GetInt(dr["fstate"]); //
                        result[i].u_TrueName = QueryInfo.GetString(dr["ftruename"]); //

                        result[i].u_QQID = QueryInfo.GetString(dr["fqqid"]);//
                        i++;
                    }
                    da.CloseConn();
                    da.Dispose();
                    return result;
                }
                else
                {
                    da.CloseConn();
                    da.Dispose();
                    throw new Exception("没有查找到相应的记录");
                }
            }
            catch (Exception e)
            {
                da.CloseConn();
                da.Dispose();
                throw e;
            }
        }
    }

    #endregion


    #region 交易单表的查询处理

    /// <summary>
    /// 交易单表的查询类
    /// </summary>
    public class Q_PAY_LIST : Query_BaseForNET
    {
        private string f_strID;
        public string ICESQL = "";

        public Q_PAY_LIST(string strID, int iIDType, DateTime dtBegin, DateTime dtEnd, int istr, int imax)
        {
            f_strID = strID;
            if (iIDType == 0)  //根据QQ号码查买家交易单
            {
                //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZJ"));

                try
                {
                    //da.OpenConn();

                    //furion 20061117 email登录修改
                    //查询内部UID
                    //string str = "select fuid from " + PublicRes.GetTName("t_relation",strID) + " where fqqid= '" + strID + "'";
                    //string fuid = da.GetOneResult(str);
                    string fuid = PublicRes.ConvertToFuid(strID);

                    //string tPayList  = PublicRes.GetTName("t_pay_list",fuid);             //交易单的表
                    string tPayList = PublicRes.GetTName("t_user_order", fuid);             //交易单的表

                    //string sWhereStr = " where (fbuy_uid='" + fuid + "' or fsale_uid ='" + fuid + "') and fcreate_time between '" 
                    string sWhereStr = " where (fbuy_uid='" + fuid + "')  and fcurtype=1 and fcreate_time between  '"
                        + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                    //使用or 会导致索引失效。 所以用union代替 rayguo . 但是发现如果用union，查询变的很慢。 所以还用or 暂时。 rayguo 07.07

                    string sWhereStr1 = " where fbuy_uid='" + fuid + "' and fcurtype=1  and fcreate_time between '"
                        + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                    string sWhereStr2 = " where fsale_uid='" + fuid + "' and fcurtype=1 and fcreate_time between '"
                        + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                    //获取总记录数
                    string fstrSql_count1 = "Select count(1) as total from " + tPayList + sWhereStr1;
                    string fstrSql_count2 = "Select count(1) as total from " + tPayList + sWhereStr2;

                    //int iCount1    = Int32.Parse(da.GetOneResult(fstrSql_count1)); //备机
                    //int iCount2    = Int32.Parse(da.GetOneResult(fstrSql_count2)); //备机

                    int iCount = 10000;

                    //fstrSql       = "Select *," + iCount + " as total from " + tPayList + sWhereStr + " ORDER By Fcreate_time DESC limit "+ (istr-1) +"," + imax;
                    fstrSql = "Select *," + iCount + " as total,Ftrade_state as Fstate from " + tPayList + sWhereStr1
                        //+ "UNION Select *," + iCount + " as total,Ftrade_state as Fstate from " + tPayList + sWhereStr2  
                        + " ORDER By Fcreate_time DESC limit " + (istr - 1) + "," + imax;
                }
                catch (Exception e)
                {
                    //					Common.CommLib.commRes.sendLog4Log("Q_PAY_LIST:Query_BaseForNET","查询交易单失败" + e.Message);
                    throw;
                }
                finally
                {
                    //da.Dispose();
                }
            }
            else if (iIDType == 9)  //根据QQ号码查卖家交易单
            {
                //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZJ"));

                try
                {
                    //da.OpenConn();

                    //furion 20061117 email登录修改
                    //查询内部UID
                    //string str = "select fuid from " + PublicRes.GetTName("t_relation",strID) + " where fqqid= '" + strID + "'";
                    //string fuid = da.GetOneResult(str);
                    string fuid = PublicRes.ConvertToFuid(strID);

                    //string tPayList  = PublicRes.GetTName("t_pay_list",fuid);             //交易单的表
                    string tPayList = PublicRes.GetTName("t_user_order", fuid);             //交易单的表

                    //string sWhereStr = " where (fbuy_uid='" + fuid + "' or fsale_uid ='" + fuid + "') and fcreate_time between '" 
                    string sWhereStr = " where ( fsale_uid ='" + fuid + "') and fcurtype=1 and fcreate_time between '"
                        + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                    //使用or 会导致索引失效。 所以用union代替 rayguo . 但是发现如果用union，查询变的很慢。 所以还用or 暂时。 rayguo 07.07

                    string sWhereStr1 = " where fbuy_uid='" + fuid + "' and fcurtype=1  and fcreate_time between '"
                        + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                    string sWhereStr2 = " where fsale_uid='" + fuid + "' and fcurtype=1  and fcreate_time between '"
                        + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                    //获取总记录数
                    string fstrSql_count1 = "Select count(1) as total from " + tPayList + sWhereStr1;
                    string fstrSql_count2 = "Select count(1) as total from " + tPayList + sWhereStr2;

                    //int iCount1    = Int32.Parse(da.GetOneResult(fstrSql_count1)); //备机
                    //int iCount2    = Int32.Parse(da.GetOneResult(fstrSql_count2)); //备机

                    int iCount = 10000;

                    //fstrSql       = "Select *," + iCount + " as total from " + tPayList + sWhereStr + " ORDER By Fcreate_time DESC limit "+ (istr-1) +"," + imax;
                    //					fstrSql       = "Select *," + iCount + " as total,Ftrade_state as Fstate from " + tPayList + sWhereStr1  
                    //						+ "UNION Select *," + iCount + " as total,Ftrade_state as Fstate from " + tPayList + sWhereStr2  
                    //						+ " ORDER By Fcreate_time DESC limit "+ (istr-1) +"," + imax;
                    fstrSql = " Select *," + iCount + " as total,Ftrade_state as Fstate from " + tPayList + sWhereStr2
                        + " ORDER By Fcreate_time DESC limit " + (istr - 1) + "," + imax;
                }
                catch (Exception e)
                {
                    //					Common.CommLib.commRes.sendLog4Log("Q_PAY_LIST:Query_BaseForNET","查询交易单失败" + e.Message);
                    throw;
                }
                finally
                {
                    //da.Dispose();
                }
            }
            // 2012/5/29 新添加根据QQ号查询中介交易
            else if (iIDType == 10)
            {
                try
                {
                    //da.OpenConn();

                    //furion 20061117 email登录修改
                    //查询内部UID
                    //string str = "select fuid from " + PublicRes.GetTName("t_relation",strID) + " where fqqid= '" + strID + "'";
                    //string fuid = da.GetOneResult(str);
                    string fuid = PublicRes.ConvertToFuid(strID);

                    //string fuid = "20364000";
                    //string tPayList  = PublicRes.GetTName("t_pay_list",fuid);             //交易单的表
                    string tPayList = PublicRes.GetTName("t_user_order", fuid);             //交易单的表

                    //string sWhereStr = " where (fbuy_uid='" + fuid + "' or fsale_uid ='" + fuid + "') and fcreate_time between '" 
                    string sWhereStr = " where ( fsale_uid ='" + fuid + "') and fcurtype=1 and fcreate_time between '"
                        + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                    //使用or 会导致索引失效。 所以用union代替 rayguo . 但是发现如果用union，查询变的很慢。 所以还用or 暂时。 rayguo 07.07

                    // 2012/5/29 其实就是在iIDType=9的情况下，加多一下选择中介交易的情况
                    string sWhereStr1 = " where fbuy_uid='" + fuid + "' and fcurtype=1  and fcreate_time between '"
                        + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss")
                        + "' and Fmedi_sign=1 and Ftrade_type=4 ";

                    string sWhereStr2 = " where fsale_uid='" + fuid + "' and fcurtype=1  and fcreate_time between '"
                        + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss")
                        + "' and Fmedi_sign=1 and Ftrade_type=4";

                    //获取总记录数
                    string fstrSql_count1 = "Select count(1) as total from " + tPayList + sWhereStr1;
                    string fstrSql_count2 = "Select count(1) as total from " + tPayList + sWhereStr2;

                    //int iCount1    = Int32.Parse(da.GetOneResult(fstrSql_count1)); //备机
                    //int iCount2    = Int32.Parse(da.GetOneResult(fstrSql_count2)); //备机

                    int iCount = 10000;

                    //fstrSql       = "Select *," + iCount + " as total from " + tPayList + sWhereStr + " ORDER By Fcreate_time DESC limit "+ (istr-1) +"," + imax;
                    //					fstrSql       = "Select *," + iCount + " as total,Ftrade_state as Fstate from " + tPayList + sWhereStr1  
                    //						+ "UNION Select *," + iCount + " as total,Ftrade_state as Fstate from " + tPayList + sWhereStr2  
                    //						+ " ORDER By Fcreate_time DESC limit "+ (istr-1) +"," + imax;
                    fstrSql = " Select *," + iCount + " as total,Ftrade_state as Fstate from " + tPayList + sWhereStr2
                        + " ORDER By Fcreate_time DESC limit " + (istr - 1) + "," + imax;
                }
                catch (Exception e)
                {
                    //					Common.CommLib.commRes.sendLog4Log("Q_PAY_LIST:Query_BaseForNET","查询交易单失败" + e.Message);
                    throw;
                }
                finally
                {
                    //da.Dispose();
                }
            }
            else if (iIDType == 13)  //yinhuang 小额刷卡交易查询
            {
                try
                {
                    //查询内部UID
                    string fuid = PublicRes.ConvertToFuid(strID);

                    string tPayList = PublicRes.GetTName("t_user_order", fuid);             //交易单的表

                    //string sWhereStr = " where (fbuy_uid='" + fuid + "')  and fcurtype=1 and fcreate_time between  '"
                    //    + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                    string sWhereStr1 = " where fbuy_uid='" + fuid + "' and fcurtype=1 and fchannel_id=113";

                   // string sWhereStr2 = " where fsale_uid='" + fuid + "' and fcurtype=1 and fcreate_time between '"
                    //    + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                    //获取总记录数
                    //string fstrSql_count1 = "Select count(1) as total from " + tPayList + sWhereStr1;
                    //string fstrSql_count2 = "Select count(1) as total from " + tPayList + sWhereStr2;

                    int iCount = 10000;

                    fstrSql = "Select *," + iCount + " as total,Ftrade_state as Fstate from " + tPayList + sWhereStr1
                        + " ORDER By Fcreate_time DESC limit " + istr + "," + imax;
                }
                catch (Exception e)
                {
                    throw;
                }
                finally
                {
                    //da.Dispose();
                }
            }
            else if (iIDType == 1)
            {
                ICESQL = "listid=" + f_strID;
                //fstrSql = "Select * from " + PublicRes.GetTName("t_tran_list",strID) + " where flistid='" + f_strID + "'";
                fstrSql = "Select *,Ftrade_state as Fstate from " + PublicRes.GetTName("t_order", strID) + " where flistid='" + f_strID + "'";
            }
            else if (iIDType == 2) //furion 20090805 2的情况应该没有.
            {
                ICESQL = "listid=" + f_strID;
                //fstrSql = "Select * from " + PublicRes.GetTName("t_tran_list",strID) + " where Fbank_backid ='" + f_strID + "'";
                fstrSql = "Select *,Ftrade_state as Fstate from " + PublicRes.GetTName("t_order", strID) + " where Fbank_backid ='" + f_strID + "'";
            }
            else if (iIDType == 4)
            {
                ICESQL = "listid=" + f_strID;
                // TODO: 1furion 数据库优化 20080111 调帐时要查询
                //fstrSql = "Select * from " + PublicRes.GetTName("t_tran_list",strID) + " where flistid='" + f_strID + "'";
                fstrSql = "Select * from " + PublicRes.GetTName("t_order", strID) + " where flistid='" + f_strID + "'";
            }
            else
            {
                throw new Exception("交易单查询传入参数错误！");
            }

        }

        public Q_PAY_LIST(string strID)
        {
            f_strID = strID;
            //string tPayList  = PublicRes.GetTName("t_tran_list",f_strID); 
            string tPayList = PublicRes.GetTName("t_order", f_strID);
            fstrSql = "Select *,Ftrade_state as Fstate from " + tPayList + " where flistid='" + f_strID + "'";

            ICESQL = "listid=" + f_strID;
        }

        /// <summary>
        /// 提供给其它语言调用的函数，以固定的类返回值。
        /// </summary>
        /// <returns></returns>
        public T_PAY_LIST GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("YWB"));
            T_PAY_LIST result = new T_PAY_LIST();
            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    result.u_Bank_ListID = QueryInfo.GetString(dr["fbank_listid"]);
                    result.u_Bargain_Time = QueryInfo.GetDateTime(dr["fbargain_time"]);
                    result.u_Buy_Bank_Type = QueryInfo.GetInt(dr["fbuy_bank_type"]);
                    result.u_Buy_BankID = QueryInfo.GetString(dr["fbuy_bankid"]);
                    result.u_Buy_Name = QueryInfo.GetString(dr["fbuy_name"]);
                    result.u_BuyID = QueryInfo.GetString(dr["fbuyid"]);
                    result.u_Carriage = QueryInfo.GetInt(dr["fcarriage"]);
                    result.u_Cash = QueryInfo.GetInt(dr["fcash"]);
                    result.u_Coding = QueryInfo.GetString(dr["fcoding"]);
                    result.u_Create_Time = QueryInfo.GetDateTime(dr["fcreate_time"]);
                    result.u_Create_Time_C2C = QueryInfo.GetDateTime(dr["fcreate_time_c2c"]);
                    result.u_CurType = QueryInfo.GetInt(dr["fcurtype"]);
                    result.u_Explain = QueryInfo.GetString(dr["fexplain"]);
                    result.u_Fact = QueryInfo.GetInt(dr["ffact"]);
                    result.u_IP = QueryInfo.GetString(dr["fip"]);
                    result.u_LState = QueryInfo.GetInt(dr["flstate"]);
                    result.u_Memo = QueryInfo.GetString(dr["fmemo"]);
                    result.u_Modify_Time = QueryInfo.GetDateTime(dr["fmodify_time"]);
                    result.u_Pay_Type = QueryInfo.GetInt(dr["fpay_type"]);
                    result.u_PayNum = QueryInfo.GetInt(dr["fpaynum"]);
                    result.u_Price = QueryInfo.GetInt(dr["fprice"]);
                    result.u_Procedure = QueryInfo.GetInt(dr["fprocedure"]);
                    result.u_Receive_Time = QueryInfo.GetDateTime(dr["freceive_time"]);
                    result.u_Receive_Time_C2C = QueryInfo.GetDateTime(dr["freceive_time_c2c"]);
                    result.u_Sale_Name = QueryInfo.GetString(dr["fsale_name"]);
                    result.u_SaleID = QueryInfo.GetString(dr["fsaleid"]);
                    result.u_Service = QueryInfo.GetInt(dr["fservice"]);
                    result.u_SPID = QueryInfo.GetString(dr["fspid"]);
                    result.u_State = QueryInfo.GetInt(dr["fstate"]);

                    result.u_ListID = QueryInfo.GetString(dr["flistid"]);

                    result.u_Pay_Time = QueryInfo.GetDateTime(dr["fpay_time"]);
                }
                else
                {
                    throw new Exception("没有查找到相应的记录");
                }
                da.CloseConn();
                da.Dispose();
                return result;
            }
            catch (Exception e)
            {
                da.CloseConn();
                da.Dispose();
                throw e;
            }
        }
    }

    #endregion


    #region 用户帐户流水表的查询处理
    /// <summary>
    /// 用户帐户流水表的查询类  (含中介)
    /// </summary>
    public class Q_BANKROLL_LIST : Query_BaseForNET
    {
        private string f_strID;
        private DateTime f_dtBegin;
        private DateTime f_dtEnd;

        //按交易单查询资金流水专用，返回需要查询的库表名称。
        public ArrayList alTables;

        public Q_BANKROLL_LIST(string strID, DateTime dtBegin, DateTime dtEnd, int istr, int imax)
        {
            f_strID = strID;
            f_dtBegin = dtBegin;
            f_dtEnd = dtEnd;

            string tableStr = PublicRes.GetTableName("t_bankroll_list", f_strID);
            string whereStr = "  where " + PublicRes.GetSqlFromQQ(strID, "fuid") + " and fmodify_time between '" + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

            //fstrSql = "Select A.*,B.total from  " + tableStr + " A, (Select count(1) as total from " + tableStr + whereStr + ") B "
            fstrSql = "Select A.*,10000 as total from  " + tableStr + " A "
                + whereStr
                + " Order by fmodify_time DESC ";
        }

        /// <summary>
        /// 返回需要查的表就可以，因为SQL一样。
        /// </summary>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        /// <param name="listID"></param>
        public Q_BANKROLL_LIST(DateTime dtBegin, DateTime dtEnd, String listID)
        {
            f_dtBegin = dtBegin;
            f_dtEnd = dtEnd;

            //******************************************************************************************************************
            //交易单相关的资金流水存放在3个地方;
            //1 买家fuid分库分表的资金流水 2卖家fuid分库分表的资金流水 3 中介账户分库分表的资金流水
            //所以需要有3个查询语句分别查询，并合并结果，才能完整的显示某listID交易单的相关的所有资金流水

            //根据交易单号查询相关的用户资金流水，因为分库分表的问题不能够直接查询。
            //首先根据交易单查询出相关的买家QQ号，买家QQ号。在根据QQ号码分库分表找到对应的内部Fuid.
            //根据Fuid查询相关的用户资金流水。根据日期查询相关的中介账户流水表内流水
            //******************************************************************************************************************

            // TODO1: furion 数据库优化 20080111
            //string listDb = PublicRes.GetTName("t_tran_list",listID);
            //string listDb = PublicRes.GetTName("t_order",listID);


            MySqlAccess da = null;
            /*
            //da = new MySqlAccess(PublicRes.GetConnString("YW_30"));  //选择读取的数据库
            da = new MySqlAccess(PublicRes.GetConnString("ZJ"));  //选择读取的数据库
            da.OpenConn();   //打开连接
            */
            TENCENT.OSS.C2C.Finance.Common.CommLib.tradeList tl = new TENCENT.OSS.C2C.Finance.Common.CommLib.tradeList();
            string[] ar = PublicRes.returnlistInfo(listID, out tl, da, "noUpdate");
            /*
            da.Dispose();
            */
            //
            //			ar[25]= "Fstate";      //交易单的状态  Fstate，Fpay_time，Freceive_time，Fmodify_time
            //			ar[26]= "Fpay_time";   //买家付款时间（本地）
            //			ar[27]= "Freceive_time"; //打款给卖家时间
            //			ar[28]= "Fmodify_time";  //最后修改时间

            //确定（中介）账户流水表的表名
            string timeStr = null;
            string timeStr2 = null;
            string lstState = ar[25];  //交易单的状态

            //考虑出现跨月的问题，需要与richard进一步商定

            alTables = new ArrayList();

            alTables.Add("UID=" + ar[3].Trim());
            alTables.Add("UID=" + ar[12].Trim());

            if (lstState == "2")       //2 表示买家付款
            {
                timeStr = ar[26].ToString().Trim();  //买家付款时间（本地）
                timeStr2 = null;

                alTables.Add("TME=" + timeStr);
            }
            else if (lstState == "4")  //4 交易结束
            {
                if (ar[27].Substring(0, 10) == ar[26].Substring(0, 10))
                {
                    timeStr = ar[26].ToString().Trim();  //如果买家付款和打款给卖家是同一个月，则取买家付款的月
                    timeStr2 = null;

                    alTables.Add("TME=" + timeStr);
                }
                else
                {
                    timeStr = ar[26].ToString().Trim();  //买家付款和打款给卖家不是在同一个月，则需要在两个库中去查找中介帐户的流水
                    timeStr2 = ar[27].ToString().Trim();  //打款给卖家

                    alTables.Add("TME=" + timeStr);
                    alTables.Add("TME=" + timeStr2);
                }
            }
            else if (lstState == "7")  //7 转入退款
            {
                Hashtable ht = new Hashtable();

                if (!ht.ContainsKey(ar[26].Substring(0, 10)))
                {
                    ht.Add(ar[26].Substring(0, 10), "");
                }

                if (!ht.ContainsKey(ar[28].Substring(0, 10)))
                {
                    ht.Add(ar[28].Substring(0, 10), "");
                }

                //b2c和快速交易查询退款申请单 这里可以不管，大不了查不到数据。
                //if(tl.Ftrade_type == "2" || tl.Ftrade_type == "3")
                {
                    /*
                    string strSql = " select Fcreate_time from c2c_db_inc.t_spm_refund where Ftransaction_id='" + listID + "' and Fstatus != 5";
                    DataTable dt = PublicRes.returnDataTable(strSql,"INC");
                    */

                    string strSql = "transaction_id=" + listID + "&statusno=5";
                    string errMsg = "";
                    DataTable dt = CommQuery.GetTableFromICE(strSql, CommQuery.QUERY_MCH_REFUND, out errMsg);

                    //20141114 FINANCE_OD_QUERY_MCH_REFUND改调relay
                    /////////////////////////////////////
                    //DataTable dt = new DataTable();
                    //string qzj_ip = ConfigurationManager.AppSettings["ComQueryToRelay_IP"];
                    //string qzj_port = ConfigurationManager.AppSettings["ComQueryToRelay_PORT"];

                    //string req = "request_type=100568&ver=1&head_u=&sp_id=&" + strSql;


                    //string Msg = ""; //重置

                    //string answer = commRes.GetFromRelay(req, qzj_ip, qzj_port, out Msg);

                    //if (answer == "")
                    //{
                    //    dt= null;
                    //}
                    //if (Msg != "")
                    //{
                    //    throw new Exception("调relay异常：" + Msg);
                    //}

                    ////解析relay str
                    //DataSet ds = CommQuery.ParseRelayPageRowNum0(answer, out Msg);
                    //if (Msg != "")
                    //{
                    //    throw new Exception("解析relay异常：" + Msg);
                    //}

                    //if (ds != null && ds.Tables.Count > 0 )
                    //{
                    //    dt = ds.Tables[0];
                    //}
                    ///////////////////////////////
                 

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            //string tmp = DateTime.Parse(dr[0].ToString()).ToString("yyyy-MM-dd");
                            string tmp = DateTime.Parse(dr["Fcreate_time"].ToString()).ToString("yyyy-MM-dd");
                            if (!ht.ContainsKey(tmp.Substring(0, 10)))
                            {
                                ht.Add(tmp.Substring(0, 10), "");
                            }
                        }
                    }
                }

                foreach (DictionaryEntry de in ht)
                {
                    alTables.Add("TME=" + de.Key.ToString());
                }
                /*
                //furion V30_FURION改动 need 这个地方因为查到时间只是为了查询资金流水,下面把资金流水查询搞定就行.
                //从退款单中查找退款的时间
                string rTime = "Select Fcreate_time from c2c_db.t_refund_list where flistid = '" + listID + "'"; 
                timeStr2     = PublicRes.ExecuteOne(rTime,"yw_30");
                timeStr2     = DateTime.Parse(timeStr2).ToString("yyyy-MM-dd HH:mm:ss");
                if (ar[26].Substring(0,7) == timeStr2.Substring(0,7))
                {
                    timeStr = ar[26].ToString().Trim();
                    timeStr2= null;
                }
                else
                {
                    timeStr  = ar[26].ToString().Trim();
                    timeStr2 = timeStr2;
                }
                */
            }
            else
            {
                timeStr = ar[9].ToString().Trim();   //默认时间（订单创建时间）
                timeStr2 = null;

                alTables.Add("TME=" + timeStr);
            }



            /*
            //furion 加入强制索引
            string indexstr_mid = "";
            string indexstr_mid2 = "";
            string indexstr_buy = "";
            string indexstr_sale = "";

            string MidSqlStr;
            string strTable_Mid  = PublicRes.GetTableByDate("t_bankroll_list",timeStr);
            indexstr_mid = "XPK1t_bankroll_list_" + timeStr.Replace(":","").Replace(" ","").Replace("-","").Substring(0,6);

            if (timeStr2 == null || timeStr2 == "")
            {				
                //MidSqlStr = "SELECT * FROM " + strTable_Mid  + " where flistid = '" + listID + "'";
                MidSqlStr = "SELECT * FROM " + strTable_Mid  + " force index(" + indexstr_mid + ") where flistid = '" + listID + "'";
            }
            else
            {
				
                string strTable_Mid2 = PublicRes.GetTableByDate("t_bankroll_list",timeStr2);
                indexstr_mid2 = "XPK1t_bankroll_list_" + timeStr2.Replace(":","").Replace(" ","").Replace("-","").Substring(0,6);

                //				MidSqlStr = "SELECT * FROM " + strTable_Mid   + " where flistid = '" + listID + "' UNION " +
                //					"SELECT * FROM " + strTable_Mid2  + " where flistid = '" + listID + "' ";
                MidSqlStr = "SELECT * FROM " + strTable_Mid   + " force index(" + indexstr_mid + ") where flistid = '" + listID + "' UNION " +
                    "SELECT * FROM " + strTable_Mid2  + " force index(" + indexstr_mid2 + ") where flistid = '" + listID + "' ";
            }
			
            //确定（买家）账户流水表的表名
            //			string strSelectBuyID  = "SELECT fbuy_uid  FROM " + listDb + " WHERE flistid ='" + listID + "'" ;
            string strTable_Buy = PublicRes.GetTName("t_bankroll_list",ar[3].ToString().Trim());
            string strindexID = ar[3].ToString().Trim();
            indexstr_buy = "XPK1t_bankroll_list_" + strindexID.Substring(strindexID.Length-3,1);

            //确定（卖家）账户流水表的表名
            //			string strSelectSaleID = "SELECT fsale_uid  FROM " + listDb + " WHERE flistid = '" + listID + "'" ;
            string strTable_Sale = PublicRes.GetTName("t_bankroll_list",ar[12].ToString().Trim());
            strindexID = ar[12].ToString().Trim();
            indexstr_sale = "XPK1t_bankroll_list_" + strindexID.Substring(strindexID.Length-3,1);

            //已修改 furion V30_FURION核心查询需改动 退款单形成的资金流水在3.0里应该还是Flistid=交易单ID
            //furion V30_FURION改动 need
            string reStr = ") ";
            /*
            //还需要退款单的ID
            string rID   = "Select frlistid from c2c_db.t_refund_list where flistid = '" + listID + "'"; 
            string strID = PublicRes.ExecuteOne(rID,"yw");

            string reStr;
            if (strID != null && strID != "")
            {
                reStr = " or flistid = '" + strID + "') ";
            }
            else
            {
                reStr = ") ";
            }			
            /
						

            //			string refoundID　= " '" + listID + "'";

            //主查询语句
            fstrSql = MidSqlStr + " UNION " 
				      
                + "select * from " + strTable_Buy  + " force index(" + indexstr_buy + ") where (flistid = '" + listID + "'" + reStr + " UNION " 
                + "select * from " + strTable_Sale + " force index(" + indexstr_sale + ") where (flistid = '" + listID + "'" + reStr + " order  by Faction_Type DESC";
            */
        }


        /// <summary>
        /// 提供查询所有的正常流水，返回当前的正常的acrionType
        /// </summary>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        /// <param name="listID"></param>
        /// <param name="type"></param>
        public Q_BANKROLL_LIST(DateTime dtBegin, DateTime dtEnd, string listID, string type)
        {
            f_dtBegin = dtBegin;
            f_dtEnd = dtEnd;

            //******************************************************************************************************************
            //交易单相关的资金流水存放在3个地方;
            //1 买家fuid分库分表的资金流水 2卖家fuid分库分表的资金流水 3 中介账户分库分表的资金流水
            //所以需要有3个查询语句分别查询，并合并结果，才能完整的显示某listID交易单的相关的所有资金流水

            //根据交易单号查询相关的用户资金流水，因为分库分表的问题不能够直接查询。
            //首先根据交易单查询出相关的买家QQ号，买家QQ号。在根据QQ号码分库分表找到对应的内部Fuid.
            //根据Fuid查询相关的用户资金流水。根据日期查询相关的中介账户流水表内流水
            //******************************************************************************************************************

            // TODO1: furion 数据库优化 20080111
            //string listDb = PublicRes.GetTName("t_tran_list",listID);
            //string listDb = PublicRes.GetTName("t_order",listID);

            MySqlAccess da = null;
            /*
            //da = new MySqlAccess(PublicRes.GetConnString("YW_30"));  //选择读取的数据库
            da = new MySqlAccess(PublicRes.GetConnString("ZJ"));  //选择读取的数据库
            da.OpenConn();   //打开连接
            */
            TENCENT.OSS.C2C.Finance.Common.CommLib.tradeList tl = new TENCENT.OSS.C2C.Finance.Common.CommLib.tradeList();
            string[] ar = PublicRes.returnlistInfo(listID, out tl, da, "noUpdate");
            /*
            da.Dispose();
            */

            //确定（中介）账户流水表的表名
            string timeStr = null;
            string timeStr2 = null;
            string lstState = ar[25];  //交易单的状态

            //考虑出现跨月的问题，需要与richard进一步商定

            if (lstState == "2")       //2 表示买家付款
            {
                timeStr = ar[26].ToString().Trim();  //买家付款时间（本地）
                timeStr2 = null;
            }
            else if (lstState == "4")  //4 交易结束
            {
                if (ar[27].Substring(0, 7) == ar[26].Substring(0, 7))
                {
                    timeStr = ar[26].ToString().Trim();  //如果买家付款和打款给卖家是同一个月，则取买家付款的月
                    timeStr2 = null;
                }
                else
                {
                    timeStr = ar[26].ToString().Trim();  //买家付款和打款给卖家不是在同一个月，则需要在两个库中去查找中介帐户的流水
                    timeStr2 = ar[27].ToString().Trim();  //打款给卖家
                }
            }
            else if (lstState == "7")  //7 转入退款
            {
                //furion V30_FURION改动 need
                //从退款单中查找退款的时间
                string rTime = "Select Fcreate_time from c2c_db.t_refund_list where flistid = '" + listID + "'";
                timeStr2 = PublicRes.ExecuteOne(rTime, "yw_30");
                timeStr2 = DateTime.Parse(timeStr2).ToString("yyyy-MM-dd HH:mm:ss");
                if (ar[26].Substring(0, 7) == timeStr2.Substring(0, 7))
                {
                    timeStr = ar[26].ToString().Trim();
                    timeStr2 = null;
                }
                else
                {
                    timeStr = ar[26].ToString().Trim();
                    timeStr2 = timeStr2;
                }
            }
            else
            {
                timeStr = ar[9].ToString().Trim();   //默认时间（订单创建时间）
                timeStr2 = null;
            }

            string MidSqlStr;
            string strTable_Mid = PublicRes.GetTableByDate("t_bankroll_list", timeStr);
            if (timeStr2 == null || timeStr2 == "")
            {
                MidSqlStr = "SELECT * FROM " + strTable_Mid + " where flistid = '" + listID + "' and (Flist_sign= 0 || Flist_sign is null)";
            }
            else
            {
                string strTable_Mid2 = PublicRes.GetTableByDate("t_bankroll_list", timeStr2);
                MidSqlStr = "SELECT * FROM " + strTable_Mid + " where flistid = '" + listID + "' and (Flist_sign= 0 || Flist_sign is null)  UNION " +
                    "SELECT * FROM " + strTable_Mid2 + " where flistid = '" + listID + "' and (Flist_sign= 0 || Flist_sign is null) ";
            }

            //确定（买家）账户流水表的表名
            //			string strSelectBuyID  = "SELECT fbuy_uid  FROM " + listDb + " WHERE flistid ='" + listID + "'" ;
            string strTable_Buy = PublicRes.GetTName("t_bankroll_list", ar[3].ToString().Trim());


            //确定（卖家）账户流水表的表名
            //			string strSelectSaleID = "SELECT fsale_uid  FROM " + listDb + " WHERE flistid = '" + listID + "'" ;
            string strTable_Sale = PublicRes.GetTName("t_bankroll_list", ar[12].ToString().Trim());

            //已改动 furion V30_FURION核心查询需改动 退款形成的资金流水在3.0里,应该还是Flistid=交易单ID
            //furion V30_FURION改动 need
            string reStr = ") ";
            //还需要退款单的ID
            /*
            string rID   = "Select frlistid from c2c_db.t_refund_list where flistid = '" + listID + "'"; 
            string strID = PublicRes.ExecuteOne(rID,"yw");

            string reStr;
            if (strID != null && strID != "")
            {
                reStr = " or flistid = '" + strID + "') ";
            }
            else
            {
                reStr = ") ";
            }	
            */

            //			string refoundID　= " '" + listID + "'";

            //主查询语句
            fstrSql = MidSqlStr + " UNION "

                + "select * from " + strTable_Buy + " where (flistid = '" + listID + "' and (Flist_sign= 0 || Flist_sign is null) " + reStr + " UNION "
                + "select * from " + strTable_Sale + " where (flistid = '" + listID + "' and (Flist_sign= 0 || Flist_sign is null) " + reStr + " order  by Faction_type DESC";
        }


        /// <summary>
        /// 提供给其它语言调用的函数，以固定的类返回值。
        /// </summary>
        /// <returns></returns>
        public T_BANKROLL_LIST[] GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString());

            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);
                if (dt.Rows.Count > 0)
                {
                    T_BANKROLL_LIST[] result = new T_BANKROLL_LIST[dt.Rows.Count];
                    int i = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        result[i] = new T_BANKROLL_LIST();

                        result[i].u_ApplyID = QueryInfo.GetString(dr["fapplyid"]);
                        result[i].u_Balance = QueryInfo.GetInt(dr["fbalance"]);
                        result[i].u_Bank_Type = QueryInfo.GetInt(dr["fbank_type"]);
                        //result[i].u_Create_Time = QueryInfo.GetDateTime(dr["fcreate_time"]); fmodify_time
                        result[i].u_Create_Time = QueryInfo.GetDateTime(dr["fmodify_time"]);
                        result[i].u_CurType = QueryInfo.GetInt(dr["fcurtype"]);
                        result[i].u_From_Name = QueryInfo.GetString(dr["ffrom_name"]);
                        result[i].u_FromID = QueryInfo.GetString(dr["ffromid"]);
                        result[i].u_IP = QueryInfo.GetString(dr["fip"]);
                        result[i].u_ListID = QueryInfo.GetString(dr["flistid"]);
                        result[i].u_Memo = QueryInfo.GetString(dr["fmemo"]);
                        result[i].u_Modify_Time = QueryInfo.GetDateTime(dr["fmodify_time"]);
                        result[i].u_PayNum = QueryInfo.GetInt(dr["fpaynum"]);
                        result[i].u_Prove = QueryInfo.GetString(dr["fprove"]);
                        result[i].u_SPID = QueryInfo.GetString(dr["fspid"]);
                        result[i].u_Subject = QueryInfo.GetInt(dr["fsubject"]);
                        result[i].u_True_Name = QueryInfo.GetString(dr["ftrue_name"]);
                        result[i].u_Type = QueryInfo.GetInt(dr["ftype"]);
                        result[i].u_BKID = QueryInfo.GetInt(dr["fbkid"]);
                        result[i].u_UID = QueryInfo.GetString(dr["fuid"]);

                        result[i].u_Action_Type = QueryInfo.GetInt(dr["faction_type"]);

                        i++;
                    }
                    da.CloseConn();
                    da.Dispose();
                    return result;
                }
                else
                {
                    da.CloseConn();
                    da.Dispose();
                    throw new Exception("没有查找到相应的记录");
                }
            }
            catch (Exception e)
            {
                da.CloseConn();
                da.Dispose();
                throw e;
            }
        }
    }
    #endregion


    #region 用户交易流水表的查询处理
    /// <summary>
    /// 用户交易流水表的查询类
    /// </summary>
    public class Q_USERPAY_LIST : Query_BaseForNET
    {
        private string f_strID;
        private DateTime f_dtBegin;
        private DateTime f_dtEnd;


        //			public Q_USERPAY_LIST(string strID, int iIDType, DateTime dtBegin, DateTime dtEnd,int istr, int imax)
        //			{
        //				f_strID = strID;
        //				f_dtBegin = dtBegin;
        //				f_dtEnd = dtEnd;
        //
        //				if(iIDType ==0)
        //				{
        //					fstrSql = "Select * from  " + PublicRes.GetTableName("t_userpay_list",f_strID) + "  where " + PublicRes.GetSqlFromQQ(strID,"fuid");
        //					if(dtBegin.ToString("yyyy-MM-dd") != "1900-01-01" && dtEnd.ToString("yyyy-MM-dd") != "4000-01-01")
        //					{
        //						fstrSql += " and fcreate_time between '" + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" 	
        //							+ dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
        //					}
        //					fstrSql +=  " Order by fude_id DESC limit "+ (istr-1) +"," + imax;
        //				}
        //			}

        /*
            public Q_USERPAY_LIST(string strID, DateTime dtBegin, DateTime dtEnd,String listID)
            {
                f_strID = strID;
                f_dtBegin = dtBegin;
                f_dtEnd = dtEnd;

                //********************************************************************************************************************************
                //交易记录记录流水存放在2个地方。1是根据买家fuid分库分表的t_userpay_list中，另一个是卖家fuid分库分表的t_userpay_list中.
                //所以要分别根据交易单号查询出买家和卖家的内部fuid,再由fuid定位表名,然后分别查询，再组合成一个结果返回
                //********************************************************************************************************************************

                //买家用户fuid
                //string strSelectBuyID  = "SELECT  fbuy_uid   FROM " + PublicRes.GetTName("t_tran_list",listID) + " WHERE flistid ='" + listID + "'" ;
                string strSelectBuyID  = "SELECT  fbuy_uid   FROM " + PublicRes.GetTName("t_order",listID) + " WHERE flistid ='" + listID + "'" ;
                string strTable_Buy = PublicRes.GetTName("t_userpay_list",PublicRes.ExecuteOne(strSelectBuyID,"ZJB"));

                //买家用户fuid
                //string strSelectSaleID  = "SELECT fsale_uid  FROM " + PublicRes.GetTName("t_tran_list",listID) + " WHERE flistid ='" + listID + "'" ;
                string strSelectSaleID  = "SELECT fsale_uid  FROM " + PublicRes.GetTName("t_order",listID) + " WHERE flistid ='" + listID + "'" ;
                string strTable_Sale = PublicRes.GetTName("t_userpay_list",PublicRes.ExecuteOne(strSelectSaleID,"ZJB"));

                fstrSql = "SELECT * FROM " + strTable_Buy   + " where  flistid = '" + listID + "' union " 
                    +"SELECT * FROM " + strTable_Sale  + " where  flistid = '" + listID + "'";

            }
            */


        /// <summary>
        /// 提供给其它语言调用的函数，以固定的类返回值。
        /// </summary>
        /// <returns></returns>
        public T_USERPAY_LIST[] GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString());

            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);
                if (dt.Rows.Count > 0)
                {
                    T_USERPAY_LIST[] result = new T_USERPAY_LIST[dt.Rows.Count];
                    int i = 0;
                    foreach (DataRow dr in dt.Rows) //？
                    {
                        result[i] = new T_USERPAY_LIST();

                        result[i].u_ApplyID = QueryInfo.GetString(dr["fapplyid"]);
                        result[i].u_Balance = QueryInfo.GetInt(dr["fbalance"]);
                        result[i].u_Bank_Type = QueryInfo.GetInt(dr["fbank_type"]);
                        result[i].u_Create_Time = QueryInfo.GetDateTime(dr["fcreate_time"]);
                        result[i].u_CurType = QueryInfo.GetInt(dr["fcurtype"]);
                        result[i].u_From_Balance = QueryInfo.GetInt(dr["ffrom_balance"]);
                        result[i].u_From_Name = QueryInfo.GetString(dr["ffrom_name"]);
                        result[i].u_FromID = QueryInfo.GetString(dr["ffromid"]);
                        result[i].u_IP = QueryInfo.GetString(dr["fip"]);
                        result[i].u_ListID = QueryInfo.GetString(dr["flistid"]);
                        result[i].u_Memo = QueryInfo.GetString(dr["fmemo"]);
                        result[i].u_Modify_Time = QueryInfo.GetDateTime(dr["fmodify_time"]);
                        result[i].u_PayNum = QueryInfo.GetInt(dr["fpaynum"]);
                        result[i].u_Prove = QueryInfo.GetString(dr["fprove"]);
                        result[i].u_SPID = QueryInfo.GetString(dr["fspid"]);
                        result[i].u_Subject = QueryInfo.GetInt(dr["fsubject"]);
                        result[i].u_True_Name = QueryInfo.GetString(dr["ftrue_name"]);
                        result[i].u_Type = QueryInfo.GetInt(dr["ftype"]);
                        result[i].u_Ude_ID = QueryInfo.GetInt(dr["ftype"]);
                        result[i].u_QQID = QueryInfo.GetString(dr["fqqid"]);

                        i++;
                    }
                    da.CloseConn();
                    da.Dispose();
                    return result;
                }
                else
                {
                    da.CloseConn();
                    da.Dispose();
                    throw new Exception("没有查找到相应的记录");
                }
            }
            catch (Exception e)
            {
                da.CloseConn();
                da.Dispose();
                throw e;
            }
        }

    }
    #endregion


    #region 腾讯银行帐户表的查询处理
    /// <summary>
    /// 腾讯银行帐户表的查询类
    /// </summary>
    public class Q_TC_BANK : Query_BaseForNET
    {
        private string f_strID;
        public Q_TC_BANK(string strID)
        {
            f_strID = strID;
            fstrSql = "Select * from c2c_db.t_tc_bank where fbankid='" + f_strID + "'";
        }

        /// <summary>
        /// 提供给其它语言调用的函数，以固定的类返回值。
        /// </summary>
        /// <returns></returns>
        public T_TC_BANK GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZL"));
            T_TC_BANK result = new T_TC_BANK();
            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    result.u_Area = QueryInfo.GetString(dr["farea"]);
                    result.u_Balance = QueryInfo.GetInt(dr["fbalance"]);
                    result.u_Bank_Name = QueryInfo.GetString(dr["fbank_name"]);
                    result.u_Bank_Type = QueryInfo.GetInt(dr["fbank_type"]);
                    result.u_City = QueryInfo.GetString(dr["fcity"]);
                    result.u_Create_Time = QueryInfo.GetDateTime(dr["fcreate_time"]);
                    result.u_CurType = QueryInfo.GetString(dr["fcurtype"]);
                    result.u_IP = QueryInfo.GetString(dr["fip"]);
                    result.u_Memo = QueryInfo.GetString(dr["fmemo"]);
                    result.u_Modify_Time = QueryInfo.GetDateTime(dr["fmodify_time"]);
                    result.u_TrueName = QueryInfo.GetString(dr["ftruename"]);
                    result.u_Yd_Balance = QueryInfo.GetInt(dr["fyd_balance"]);
                }
                else
                {
                    throw new Exception("没有查找到相应的记录");
                }
                da.CloseConn();
                da.Dispose();
                return result;
            }
            catch (Exception e)
            {
                da.CloseConn();
                da.Dispose();
                throw e;
            }
        }
    }

    #endregion


    #region 腾讯收款记录表的查询处理
    /// <summary>
    /// 腾讯收款记录表的查询类
    /// </summary>
    public class Q_TCBANKROLL_LIST : Query_BaseForNET
    {
        public string ICESQL;

        private string f_strID;
        //		public Q_TCBANKROLL_LIST(string strID, int iIDType, DateTime dtBegin, DateTime dtEnd)
        //		{
        //			f_strID = strID;
        //			if(iIDType == 0)  //根据QQID查询
        //			{
        //				string whereStr = " where " + PublicRes.GetSqlFromQQ(strID,"fauid") + " and fpay_front_time between '" 
        //					+ dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "'";
        //				fstrSql = "Select A.*,B.total from c2c_db.t_tcbankroll_list A,(Select count(1) as total from c2c_db.t_tcbankroll_list " + whereStr + ") B "
        //					+ whereStr
        ////					+ " Order by fpay_front_time DESC";
        //					+ " Order by Ftde_id DESC";
        //			}
        //			else
        //			{
        //				fstrSql = "Select * from c2c_db.t_tcbankroll_list where flistid='" + f_strID + "'";
        //			}
        //		}
        public Q_TCBANKROLL_LIST(string strID, int iIDType, DateTime u_BeginTime, DateTime u_EndTime, bool isHistory)
        {
            //			f_strID = strID;
            //			if(iIDType == 0)  //根据QQID查询
            //			{
            //				string whereStr = " where " + PublicRes.GetSqlFromQQ(strID,"fauid") + " and fpay_front_time between '" 
            //					+ dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            //				fstrSql = "Select A.*,B.total from c2c_db.t_tcbankroll_list A,(Select count(1) as total from c2c_db.t_tcbankroll_list " + whereStr + ") B "
            //					+ whereStr
            //					//					+ " Order by fpay_front_time DESC";
            //					+ " Order by Ftde_id DESC";
            //			}
            //			else
            //			{
            //				fstrSql = "Select * from c2c_db.t_tcbankroll_list where flistid='" + f_strID + "'";
            //			}


            string strGroup = "";
            string strWhere = "";

            if (strID == null || strID.Trim() == "")
            {
            }
            else if (strID != null && strID.Trim() != "" && iIDType == 0)  //按照QQ号查询，注意使用内部uid
            {
                //furion 20051101 以后查询全以内部ID开始.
                string uid = PublicRes.ConvertToFuid(strID);
                strWhere += " where fauid='" + uid + "' ";
                ICESQL = "auid=" + uid;
            }
            else if (strID != null && strID.Trim() != "")  //给银行的订单号
            {
                strWhere += " where flistid='" + strID + "' ";
                ICESQL = "listid=" + strID;
            }


            //因为充值记录在个人信息这一页查询的时候begintime肯定等于1940,下面进行改写
            //			u_BeginTime = DateTime.Now.AddMonths(-4);
            //			u_EndTime = DateTime.Now.AddMonths(-1);

            string test = u_BeginTime.ToString("yyyy-MM-dd");

            if (test != "1940-01-01")
            {
                if (strWhere != "")
                {
                    ICESQL += "&fronttime_start=" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss");
                    ICESQL += "&fronttime_end=" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss");

                    strWhere += " and Fpay_front_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                        + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                }
                else
                {
                    ICESQL = "fronttime_start=" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss");
                    ICESQL += "&fronttime_end=" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss");

                    strWhere = " where Fpay_front_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                        + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                }

            }

            if (test != "1940-01-01" && isHistory)
            {
                //				strWhere += " and Fpay_front_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss") 
                //					+ "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                //从效率考虑,只取开始时间和结束时间所在月的历史记录,其它不取.
                //DateTime tmpDate = u_BeginTime.AddMonths(-1);
                DateTime tmpDate = u_BeginTime;
                //while(tmpDate <= u_EndTime.AddMonths(1))
                while (tmpDate <= u_EndTime)
                {
                    //string TableName = "c2c_db_receive.t_tcbankroll_list_" + tmpDate.ToString("yyyyMM");
                    string TableName = "c2c_db_history.t_tcbankroll_list_" + tmpDate.ToString("yyyyMM");

                    strGroup = strGroup + " select *  from " + TableName + strWhere + " union all";

                    tmpDate = tmpDate.AddMonths(1);

                    string strTmp = tmpDate.ToString("yyyy-MM-");
                    tmpDate = DateTime.Parse(strTmp + "01 00:00:01");
                }


            }

            string TableName1 = "c2c_db.t_tcbankroll_list";
            strGroup = strGroup + " select *  from " + TableName1 + strWhere + " ";

            string strorder = " Order by Ftde_id DESC ";


            fstrSql = strGroup + strorder;

            fstrSql_count = "select 100000";//" select count(1) from ( " + strGroup + ") tmpTalbe";

            //ICESQL = "strwhere=" + strWhere;
            //			if(test != "1940-01-01" )			//增加时间参数 andrew 20110322
            //			{
            //				ICESQL += "&start_time=" + test;
            //			}
        }

        /// <summary>
        /// 提供给其它语言调用的函数，以固定的类返回值。
        /// </summary>
        /// <returns></returns>
        public T_TCBANKROLL_LIST GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZJB"));
            T_TCBANKROLL_LIST result = new T_TCBANKROLL_LIST();
            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    result.u_Aid = QueryInfo.GetString(dr["faid"]);
                    result.u_Bank_Acc = QueryInfo.GetString(dr["fbank_acc"]);
                    result.u_Bank_List = QueryInfo.GetString(dr["fbank_list"]);
                    result.u_Bank_Type = QueryInfo.GetInt(dr["fbank_type"]);
                    result.u_CurType = QueryInfo.GetInt(dr["fcurtype"]);
                    result.u_IP = QueryInfo.GetString(dr["fip"]);
                    result.u_Memo = QueryInfo.GetString(dr["fmemo"]);
                    result.u_Modify_Time = QueryInfo.GetDateTime(dr["fmodify_time"]);
                    result.u_Num = QueryInfo.GetInt(dr["fnum"]);
                    result.u_Prove = QueryInfo.GetString(dr["fprove"]);
                    result.u_Sign = QueryInfo.GetString(dr["fsign"]);
                    result.u_SPID = QueryInfo.GetString(dr["fspid"]);
                    result.u_Subject = QueryInfo.GetInt(dr["fsubject"]);
                    result.u_Tde_ID = QueryInfo.GetInt(dr["ftde_id"]);
                    result.u_Type = QueryInfo.GetInt(dr["ftype"]);
                    result.u_UID = QueryInfo.GetString(dr["fuid"]);

                    result.u_State = QueryInfo.GetInt(dr["fstate"]);
                    result.u_Pay_Front_Time = QueryInfo.GetDateTime(dr["fpay_front_time"]);
                    result.u_Pay_Time = QueryInfo.GetDateTime(dr["fpay_time"]);

                    result.u_ListID = QueryInfo.GetString(dr["flistid"]);
                }
                else
                {
                    throw new Exception("没有查找到相应的记录");
                }
                da.CloseConn();
                da.Dispose();
                return result;
            }
            catch (Exception e)
            {
                da.CloseConn();
                da.Dispose();
                throw e;
            }
        }
    }

    #endregion


    #region 腾讯付款记录表的查询处理

    /// <summary>
    /// 腾讯付款记录表的查询类
    /// </summary>
    public class Q_TCBANKPAY_LIST : Query_BaseForNET
    {
        private string f_strID;
        public Q_TCBANKPAY_LIST(string strID, int iIDType, DateTime dtBegin, DateTime dtEnd)
        {
            f_strID = strID;
            if (iIDType == 0)
            {
                string currtable = "";
                string othertable = "";
                PickQueryClass.GetPayListTableFromTime(dtBegin, out currtable, out othertable);

                //**furion提现单改造20120216
                string whereStr = " where " + PublicRes.GetSqlFromQQ(strID, "fuid") + " and Fcurtype=1 and fpay_front_time_acc between '" + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' and Fbankid!=4 ";//去掉提现表中银行退单

                fstrSql = "";

                DateTime tmpDate = dtBegin;
                while (tmpDate <= dtEnd)
                {
                    string TableName = "c2c_db.t_tcpay_list_" + tmpDate.ToString("yyyyMM");

                    fstrSql = fstrSql + " select " + PickQueryClass.GetTcPayListNewFields() + ",10000 as Total from " + TableName + whereStr + " union all";

                    tmpDate = tmpDate.AddMonths(1);

                    string strTmp = tmpDate.ToString("yyyy-MM-");
                    tmpDate = DateTime.Parse(strTmp + "01 00:00:01");
                }

                string TableName1 = "c2c_db.t_tcpay_list";
                fstrSql = fstrSql + " select " + PickQueryClass.GetTcPayListOldFields() + ",10000 as Total from " + TableName1 + whereStr + " ";

                fstrSql += " Order by fpay_front_time_acc DESC";

            }
            else
            {
                string currtable = "";
                string othertable = "";
                PickQueryClass.GetPayListTableFromID(f_strID, out currtable, out othertable);

                //**furion提现单改造20120216
                //furion V30_FURION改动 need
                //				fstrSql = "Select * from c2c_db.t_tcpay_list where flistid=(select frlistid from c2c_db.t_refund_list where flistid ='" + f_strID + "') Order by fpay_front_time DESC";
                fstrSql = "Select " + PickQueryClass.GetTcPayListOldFields() + " from c2c_db.t_tcpay_list where flistid = '" + f_strID + "' or flistid = (select frlistid from c2c_db.t_refund_list where flistid ='" + f_strID + "'and flstate <> 3) ";
                fstrSql += " union all Select " + PickQueryClass.GetTcPayListNewFields() + " from " + currtable + " where flistid = '" + f_strID + "' or flistid = (select frlistid from c2c_db.t_refund_list where flistid ='" + f_strID + "'and flstate <> 3) ";
                fstrSql += " union all Select " + PickQueryClass.GetTcPayListNewFields() + " from " + othertable + " where flistid = '" + f_strID + "' or flistid = (select frlistid from c2c_db.t_refund_list where flistid ='" + f_strID + "'and flstate <> 3) ";
                fstrSql += " Order by fpay_front_time DESC";
            }
        }

        /// <summary>
        /// 提供给其它语言调用的函数，以固定的类返回值。
        /// </summary>
        /// <returns></returns>
        public T_TCBANKPAY_LIST[] GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZJB"));
            //T_TCBANKPAY_LIST result = new T_TCBANKPAY_LIST();
            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);

                if (dt.Rows.Count > 0)
                {
                    T_TCBANKPAY_LIST[] result = new T_TCBANKPAY_LIST[dt.Rows.Count];  //多行数据时，用数组存放
                    int i = 0;
                    foreach (DataRow dr in dt.Rows) //？
                    {
                        result[i] = new T_TCBANKPAY_LIST();               //数组的一行存放一个类，多维数组来存放多个类

                        result[i].u_Tde_ID = QueryInfo.GetString(dr["Ftde_id"]);
                        result[i].u_ListID = QueryInfo.GetString(dr["Flistid"]);
                        result[i].u_Bank_List = QueryInfo.GetString(dr["Fbank_list"]);
                        result[i].u_Bankid = QueryInfo.GetString(dr["Fbankid"]);
                        result[i].u_State = QueryInfo.GetInt(dr["Fstate"]);
                        result[i].u_Type = QueryInfo.GetInt(dr["Ftype"]);
                        result[i].u_Subject = QueryInfo.GetInt(dr["Fsubject"]);
                        result[i].u_Num = QueryInfo.GetInt(dr["Fnum"]);
                        result[i].u_Sign = QueryInfo.GetInt(dr["Fsign"]);
                        result[i].u_Bank_Acc = QueryInfo.GetString(dr["Fbank_acc"]);
                        result[i].u_Bank_Type = QueryInfo.GetInt(dr["Fbank_type"]);
                        result[i].u_Curtype = QueryInfo.GetInt(dr["Fcurtype"]);
                        result[i].u_Aid = QueryInfo.GetString(dr["Faid"]);
                        result[i].u_ABankid = QueryInfo.GetString(dr["Fabankid"]);
                        result[i].u_aName = QueryInfo.GetString(dr["Faname"]);
                        result[i].u_Prove = QueryInfo.GetString(dr["Fprove"]);
                        result[i].u_IP = QueryInfo.GetString(dr["Fip"]);
                        result[i].u_Memo = QueryInfo.GetString(dr["Fmemo"]);
                        result[i].u_Modify_Time = QueryInfo.GetDateTime(dr["Fmodify_time"]);
                        result[i].u_Pay_Front_Time = QueryInfo.GetDateTime(dr["Fpay_front_time"]);
                        result[i].u_Pay_Time = QueryInfo.GetDateTime(dr["Fpay_time"]);
                        result[i].u_Uid = QueryInfo.GetString(dr["Fuid"]);

                        i++;
                    }
                    da.CloseConn();
                    da.Dispose();
                    return result;
                }
                else
                {
                    da.CloseConn();
                    da.Dispose();
                    throw new Exception("没有查找到相应的记录");
                }
            }
            catch (Exception e)
            {
                da.CloseConn();
                da.Dispose();
                throw e;
            }
        }

        public static string GetBankName(string banktype)
        {
            switch (banktype)
            {
                #region
                case "1001":
                    return "招商银行";

                case "1002":
                    return "中国工商银行";

                case "1003":
                    return "中国建设银行";

                case "1004":
                    return "上海浦东发展银行";

                case "1005":
                    return "中国农业银行";

                case "1006":
                    return "中国民生银行";

                case "1007":
                    return "农行国际卡";

                case "1008":
                    return "深圳发展银行";

                case "1009":
                    return "兴业银行";

                case "1010":
                    return "深圳平安银行";

                case "1011":
                    return "中国邮政储蓄银行";

                case "1020":
                    return "中国交通银行";

                case "1021":
                    return "中信实业银行";

                case "1022":
                    return "中国光大银行";

                case "1023":
                    return "农村合作信用社";

                case "1024":
                    return "上海银行";

                case "1025":
                    return "华夏银行";

                case "1026":
                    return "中国银行";

                case "1027":
                    return "广东发展银行";

                case "1028":
                    return "广东银联";

                case "1099":
                    return "其他银行";

                case "1030":
                    return "工行B2B";
                case "1031":
                    return "招行大额";
                case "1032":
                    return "北京银行";
                case "1033":
                    return "网汇通";
                case "1034":
                    return "建行大额";
                case "1037":
                    return "工行大额";
                case "1038":
                    return "招行基础业务";

                case "1039":
                    return "工行直付";

                case "1040":
                    return "建行B2B";
                case "1041":
                    return "民生借记卡";
                case "1042":
                    return "招行B2B";

                case "2001":
                    return "招行一点通";
                case "2002":
                    return "工行一点通";
                case "3001":
                    return "兴业信用卡";
                case "3002":
                    return "中行信用卡";

                #endregion

                case "9999":
                    return "汇总银行";

                case "0000":
                    return "所有银行";
                default:
                    return "";
            }
        }
    }


    #endregion


    #region 退款单表的查询处理
    /// <summary>
    /// 退款单表的查询类
    /// </summary>
    public class Q_REFUND : Query_BaseForNET
    {
        private string f_strID;
        public Q_REFUND(string strID, int iIDType, DateTime dtBegin, DateTime dtEnd)
        {
            f_strID = strID;
            //			if(iIDType == 0)
            //			{
            //				string whereStr = " where (" + PublicRes.GetSqlFromQQ(strID,"fbuy_uid") 
            //					+ " or " + PublicRes.GetSqlFromQQ(strID,"fsale_uid") + ") ";
            //				if(dtBegin.ToString("yyyy-MM-dd") != "1900-01-01" && dtEnd.ToString("yyyy-MM-dd") != "4000-01-01")
            //				{
            //					whereStr += "and fcreate_time between '" + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
            //				}
            //
            //				string count = PublicRes.ExecuteOne("select count(*) from c2c_db.t_refund_list " + whereStr,"ywb");
            //
            //				//fstrSql = "Select A.*,B.total from c2c_db.t_refund_list A, (Select count(1) as total from c2c_db.t_refund_list "  + whereStr + ") B "
            //					//+ whereStr		+ " Order by Fcreate_time DESC";
            //				fstrSql = "select *," + count + " as total from c2c_db.t_refund_list " + whereStr + " Order by Fcreate_time DESC";
            //			}
            if (iIDType == 0 || iIDType == 1)
            {
                string fuid = PublicRes.ConvertToFuid(strID);
                if (fuid == null)
                    fuid = "0";

                string whereStr = " where fbuy_uid=" + fuid + " ";
                if (iIDType == 1)
                {
                    whereStr = " where fsale_uid=" + fuid + " ";
                }

                if (dtBegin.ToString("yyyy-MM-dd") != "1900-01-01" && dtEnd.ToString("yyyy-MM-dd") != "4000-01-01")
                {
                    whereStr += "and fcreate_time between '" + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                }

                //string count = PublicRes.ExecuteOne("select count(*) from c2c_db.t_refund_list " + whereStr,"ywb");
                string count = "10000";

                //fstrSql = "Select A.*,B.total from c2c_db.t_refund_list A, (Select count(1) as total from c2c_db.t_refund_list "  + whereStr + ") B "
                //+ whereStr		+ " Order by Fcreate_time DESC";
                fstrSql = "select *," + count + " as total from c2c_db.t_refund_list " + whereStr + " Order by Fcreate_time DESC";
            }
            else  //客服系统未使用
            {
                fstrSql = "Select * from c2c_db.t_refund_list where flistid='" + f_strID + "'";
            }
        }

        /// <summary>
        /// 提供给其它语言调用的函数，以固定的类返回值。
        /// </summary>
        /// <returns></returns>
        public T_REFUND GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
            T_REFUND result = new T_REFUND();
            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    result.u_Bargain_Time = QueryInfo.GetDateTime(dr["fbargain_time"]);
                    result.u_Buy_Bank_Type = QueryInfo.GetInt(dr["fbuy_bank_type"]);
                    result.u_Buy_BankID = QueryInfo.GetString(dr["fbuy_bankid"]);
                    result.u_Buy_Name = QueryInfo.GetString(dr["fbuy_name"]);
                    result.u_BuyID = QueryInfo.GetString(dr["fbuyid"]);
                    result.u_Create_Time = QueryInfo.GetDateTime(dr["fcreate_time"]);
                    result.u_Explain = QueryInfo.GetString(dr["fexplain"]);
                    result.u_IP = QueryInfo.GetString(dr["fip"]);
                    result.u_LState = QueryInfo.GetInt(dr["flstate"]);
                    result.u_Memo = QueryInfo.GetString(dr["fmemo"]);
                    result.u_Modify_Time = QueryInfo.GetDateTime(dr["fmodify_time"]);
                    result.u_OK_Time = QueryInfo.GetDateTime(dr["fok_time"]);
                    result.u_PayBuy = QueryInfo.GetInt(dr["fpaybuy"]);
                    result.u_PaySale = QueryInfo.GetInt(dr["fpaysale"]);
                    result.u_PayType = QueryInfo.GetInt(dr["fpaytype"]);
                    result.u_Procedure = QueryInfo.GetInt(dr["fprocedure"]);
                    result.u_RListID = QueryInfo.GetInt(dr["frlistid"]);
                    result.u_Sale_Bank_Type = QueryInfo.GetInt(dr["fsale_bank_type"]);
                    result.u_Sale_BankID = QueryInfo.GetString(dr["fsale_bankid"]);
                    result.u_Sale_Name = QueryInfo.GetString(dr["fsale_name"]);
                    result.u_SaleID = QueryInfo.GetString(dr["fsaleid"]);
                    result.u_SPID = QueryInfo.GetString(dr["fspid"]);
                    result.u_State = QueryInfo.GetInt(dr["fsatate"]);

                    result.u_ListID = QueryInfo.GetString(dr["flistid"]);
                }
                else
                {
                    throw new Exception("没有查找到相应的记录");
                }
                da.CloseConn();
                da.Dispose();
                return result;
            }
            catch (Exception e)
            {
                da.CloseConn();
                da.Dispose();
                throw e;
            }
        }
    }

    #endregion

    #region 退单失败后挂异常的记录查询
    public class RefundErrorClass : Query_BaseForNET
    {
        public RefundErrorClass(string batchid, string refundOrder, int orderType, string beginDate, string endDate, int refundType, string bankType,
            int refundPath, int handleType, int errorType, int refundState, string viewOldIds)
        {
            string strWhere = " where t.FoldId=o.FoldID and o.FCreateTime between '" + beginDate + "' and '" + endDate + "' ";

            string uniteFlag = ConfigurationManager.AppSettings["UniteFlag"];

            if (viewOldIds != null && viewOldIds != "")
            {
                viewOldIds = viewOldIds.Replace("|", "','");
                viewOldIds = "'" + viewOldIds + "'";
                strWhere += " and o.FoldId in (" + viewOldIds + ") ";

            }

            if (batchid != null && batchid != "")
            {
                strWhere += " and (o.FHandleBatchId='" + batchid + "' or o.FBeforeBatchId1='" + batchid + "' " +
                    " or o.FBeforeBatchId2='" + batchid + "' or o.FBeforeBatchId3='" + batchid + "' or o.FBeforeBatchId4='" + batchid + "') ";
            }

            if (orderType == 1 && refundOrder != null && refundOrder != "")
            {
                string tmp = uniteFlag + "__" + refundOrder;
                strWhere += " and (t.Fbank_listid='" + refundOrder + "' or t.Fbank_listid like '" + tmp + "')";

            }

            if (orderType == 2 && refundOrder != null && refundOrder != "")
            {

                strWhere += " and t.FPaylistid='" + refundOrder + "' ";

            }

            if (orderType == 3 && refundOrder != null && refundOrder != "")
            {

                strWhere += " and t.FOldId='" + refundOrder + "' ";

            }
            if (orderType == 4 && refundOrder != null && refundOrder != "")
            {
                strWhere += " and  left(t.FPaylistid,10) = '" + refundOrder + "'";
            }


            if (refundType != 99)
            {

                strWhere += " and t.FrefundType=" + refundType + " ";

            }

            if (bankType != "0000")
            {

                strWhere += " and t.Fbanktype=" + bankType + " ";

            }


            if (refundPath != 99)
            {

                strWhere += " and t.FrefundPath=" + refundPath + " ";

            }

            if (errorType != 99)
            {

                strWhere += " and o.FerrorType=" + errorType + " ";

            }

            if (handleType != 99)
            {

                strWhere += " and o.FhandleType=" + handleType + " ";

            }

            if (refundState != 99)
            {

                strWhere += " and t.Fstate=" + refundState + " ";

            }

          //  strWhere += " order by o.Forigbatchid  desc ";

            fstrSql = " select o.FBeforeBatchId1,o.FBeforeBatchId2,o.FBeforeBatchId3,o.FBeforeBatchId4,o.FOrigBatchId,o.FHandleBatchId,o.FPickUser,o.FPickTime,o.FErrMsg,o.FHandleUser,o.FHandleTime,o.FHandleType,o.FHandleMemo,o.FRefundCount,o.FErrorType,o.FRefundMemo,o.FCreateTime,o.Fmodify_Time,o.FAuthorizeFlag,left(o.Forigbatchid ,8) as FBatch_date,t.* "
                + " from c2c_zwdb.t_refund_other o,c2c_zwdb.t_refund_total t " + strWhere;
            fstrSql_count = " select count(*)  from c2c_zwdb.t_refund_other o,c2c_zwdb.t_refund_total t " + strWhere;
        }


        public RefundErrorClass(string oldId)
        {
            fstrSql = " select o.FBeforeBatchId1,o.FBeforeBatchId2,o.FBeforeBatchId3,o.FBeforeBatchId4,o.FOrigBatchId,o.FHandleBatchId,o.FPickUser,o.FPickTime,o.FErrMsg,o.FHandleUser,o.FHandleTime,o.FHandleType,o.FHandleMemo,o.FRefundCount,o.FErrorType,o.FRefundMemo,o.FCreateTime,o.Fmodify_Time,o.FAuthorizeFlag,left(o.Forigbatchid ,8) as FBatch_date,t.* "
                + " from c2c_zwdb.t_refund_other o,c2c_zwdb.t_refund_total t  where o.Foldid=t.foldid and  t.FoldID='" + oldId + "' ";
            fstrSql_count = "select count(*) from c2c_zwdb.t_refund_other o,c2c_zwdb.t_refund_total t  where o.Foldid=t.foldid and t.FoldID='" + oldId + "' ";
        }


    }
    #endregion


    #region B2C退款查询

    public class B2cReturnClass : Query_BaseForNET
    {

        public string ICESQL = "";

        public B2cReturnClass(string transid, string drawid)
        {
            fstrSql = "select * from c2c_zwdb.t_refund_snap where Ftransaction_id='" + transid + "' ";
            fstrSql_count = "select count(*) from c2c_zwdb.t_refund_snap where Ftransaction_id='" + transid + "' ";

            ICESQL = "transaction_id=" + transid;
            if (drawid != null && drawid.Trim() != "")
            {
                fstrSql = "select * from c2c_zwdb.t_refund_snap where Ftransaction_id='" + transid + "' and  Fdraw_id='" + drawid + "'";
                fstrSql_count = "select count(*) from c2c_zwdb.t_refund_snap where Ftransaction_id='" + transid + "' and  Fdraw_id='" + drawid + "'";

                ICESQL = "transaction_id=" + transid + "&draw_id=" + drawid;
            }

        }


        public B2cReturnClass(string spid, string begintime, string endtime, int refundtype,
            int status, string tranid, string buyqq, string banktype, int sumtype, string drawid)
        {
            string strWhere = " where Fmodify_time  between '" + begintime + "' and '" + endtime + "' ";
            //ICESQL = "start_time=" + begintime + "&end_time=" + endtime;
            ICESQL = "start_time_modify=" + begintime + "&end_time_modify=" + endtime;//修改为按照modifytime查询

            if (sumtype != 99)
            {
                if (sumtype == 0)
                {
                    strWhere += " and (Fstandby1 is null or Fstandby1=0) ";
                    ICESQL += "&standby1=0";
                }
                else
                {
                    strWhere += " and Fstandby1=" + sumtype + " ";
                    ICESQL += "&standby1=" + sumtype;
                }
            }

            if (drawid != null && drawid.Trim() != "")
            {
                strWhere += " and Fdraw_id='" + drawid + "' ";
                ICESQL += "&draw_id=" + drawid;
            }


            if (spid != null && spid.Trim() != "")
            {
                strWhere += " and Fspid='" + spid + "' ";
                ICESQL += "&spid=" + spid;
            }

            if (refundtype != 99)
            {
                strWhere += " and Frefund_type=" + refundtype + " ";
                ICESQL += "&refund_type=" + refundtype;
            }

            if (status != 99)
            {
                strWhere += " and Fstatus=" + status + " ";
                ICESQL += "&status=" + status;
            }

            if (tranid != null && tranid.Trim() != "")
            {
                strWhere += " and Ftransaction_id='" + tranid + "' ";
                ICESQL += "&transaction_id=" + tranid;
            }

            if (buyqq != null && buyqq.Trim() != "")
            {
                //furion 20061121 email登录修改.
                //strWhere += " and Fbuyid='" + buyqq + "' ";
                string fuid = PublicRes.ConvertToFuid(buyqq);
                strWhere += " and Fbuy_uid=" + fuid + " ";

                ICESQL += "&buy_uid=" + fuid;
            }

            if (banktype != null && banktype.Trim() != "" && banktype.Trim() != "0000")
            {
                strWhere += " and Fbank_type=" + banktype + " ";
                ICESQL += "&bank_type=" + banktype;
            }

            fstrSql = " select * from c2c_zwdb.t_refund_snap " + strWhere;
            fstrSql_count = "select 10000";//" select count(*) from c2c_db_inc.t_spm_refund " + strWhere;

            //ICESQL = "strwhere=" + strWhere.Trim();
        }

        /// <summary>
        /// 撤销退款
        /// </summary>
        /// <param name="transid">交易单ID</param>
        /// <param name="msg">错误信息</param>
        /// <returns>处理是否成功</returns>
        public static bool SuspendRefundment(ArrayList al, string UserIP, out string msg)
        {
            msg = "";

            if (al == null || al.Count == 0)
            {
                msg = "请给出正确的交易单ID";
                return false;
            }

            //先判断是否有此记录和状态是否正确.Frefund_type=3 - 银行直接退款
            //Fstatus=1 - 待审批
            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
            MySqlAccess dazw = new MySqlAccess(PublicRes.GetConnString("ZWTK"));
            try
            {
                //da.OpenConn();
                dazw.OpenConn();
                //da.StartTran();

                foreach (object obj in al)
                {
                    string transid = obj.ToString();

                    string drawid = "";
                    string refund_type = "";
                    if (transid.IndexOf(";") > 0)
                    {
                        string[] split = transid.Split(';');
                        if (split.Length != 3)
                        {
                            msg = "参照不满足要求，撤销退款失败！tranid=" + transid;
                            return false;
                        }

                        transid = split[0].Trim();
                        drawid = split[1].Trim();
                        refund_type = split[2].Trim();

                    }

                    //					string strSql = " select count(*) from c2c_db_inc.t_spm_refund where Ftransaction_id='" 
                    //						+ transid + "' and Frefund_type=3 and Fstatus=9 and (Fstandby1 is null or Fstandby1=0) ";
                    //
                    //					if(drawid != "")
                    //					{
                    //						strSql = " select count(*) from c2c_db_inc.t_spm_refund where Ftransaction_id='" 
                    //							+ transid + "' and Fdraw_id='" + drawid + "'  and Frefund_type=3 and Fstatus=9 and (Fstandby1 is null or Fstandby1=0) ";
                    //					}

                    /*
                    string tmp = da.GetOneResult(strSql);

                    if(tmp == null || tmp.Trim() == "" || tmp != "1")
                    {
                        da.RollBack();
                        msg = "执行退款撤销操作，不存在此记录或者此记录状态不对tranid=" + transid;
                        return false;
                    }
                    */


                    //查询下汇总表中没有此记录

                    string strSql = "select count(*) from c2c_zwdb.t_refund_total where  fpaylistid='" + transid + "' ";
                    if (drawid != "")
                    {
                        strSql = "select count(*) from c2c_zwdb.t_refund_total where  fpaylistid='" + transid + "' and foldid='" + drawid + "'";
                    }

                    string count = dazw.GetOneResult(strSql);
                    if (count != "0")
                    {
                        //da.RollBack();
                        msg = "执行退款撤销操作，此退款信息已经汇总到退款汇总表中tranid=" + transid;
                        return false;
                    }

                    string memo = "商户申请撤销退款";


                    /*
                    strSql = " update c2c_db_inc.t_spm_refund set Fstatus=8  ,Fip='" 
                        + myheader.UserIP + "',Fmodify_time='" + PublicRes.strNowTimeStander + "',fmemo='"+memo+"',Fstandby1=2 where Ftransaction_id='" 
                        + transid + "' and Frefund_type=3 and Fstatus=9 and (Fstandby1 is null or Fstandby1=0)";

                    if(drawid != "")
                    {
                        strSql = " update c2c_db_inc.t_spm_refund set Fstatus=8  ,Fip='" 
                            + myheader.UserIP + "',Fmodify_time='" + PublicRes.strNowTimeStander + "',fmemo='"+memo+"',Fstandby1=2 where Ftransaction_id='" 
                            + transid + "' and Fdraw_id='" + drawid + "'   and Frefund_type=3 and Fstatus=9 and (Fstandby1 is null or Fstandby1=0)";
                    }

                    if(da.ExecSqlNum(strSql) != 1)
                    {
                        da.RollBack();
                        msg = "执行退款撤销操作，在更新记录状态时出错 transid=" + transid;
                        return false;
                    }
                    */

                    /*调整为新的更新方式 andrew 20120515
                    strSql = "transaction_id=" + transid + "&refund_type="+refund_type+"&status=9&standby1null=0";
                    if(drawid != "")
                    {
                        strSql = "transaction_id=" 
                            + transid + "&draw_id=" + drawid + "&refund_type="+refund_type+"&status=9&standby1null=0";
                    }

                    strSql += "&setstatus=8";
                    strSql += "&setstandby1=2";
                    strSql += "&ip=" + myheader.UserIP;
                    strSql += "&modify_time=" + PublicRes.strNowTimeStander;

                    int iresult = CommQuery.ExecSqlFromICE(strSql,CommQuery.UPDATE_SPMREFUND,out msg);

                    if(iresult != 1)
                    {
                        //da.RollBack();
                        msg = "执行退款撤销操作，在更新记录状态时出错 transid=" +  transid;
                        return false;
                    }
                    */

                    string modifyMsg = "";
                    bool modifyFlag = PublicRes.ModifySPMRefundService((int)PublicRes.ModifySPMRefundType.申请撤销退款23, transid, drawid, out modifyMsg);
                    if (!modifyFlag)
                    {
                        throw new LogicException(modifyMsg);

                    }
                    else
                    {
                        //增加更新快照汇总标志 andrew 20120515
                        string snapMsg = "";
                        bool updateSnap = UpdataRefundSnapSuspend(drawid, transid, UserIP, dazw, true, out snapMsg);
                        if (!updateSnap)
                        {
                            msg = snapMsg;
                            return false;
                        }
                    }

                }
                //da.Commit();
                return true;
            }
            catch (Exception err)
            {
                //da.RollBack();
                log4net.ILog log = log4net.LogManager.GetLogger("B2cReturnClass");
                if (log.IsErrorEnabled) log.Error("SuspendRefundment ", err);
                msg = err.Message;
                return false;
            }
            finally
            {
                dazw.Dispose();
                //da.Dispose();
            }

        }

        //更新快照表汇总标志和状态
        //更新快照表汇总标志和状态
        private static bool UpdataRefundSnapSuspend(string drawid, string tranid, string userIP, MySqlAccess dazw, bool suspend, out string msg)
        {
            msg = "";
            try
            {
                if (drawid == null || drawid == "" || tranid == null || tranid == "")
                {
                    msg = "drawid或tranid不能为空";
                    return false;
                }
                string strSql = "";
                if (suspend)
                {
                    strSql = "update  c2c_zwdb.t_refund_snap set Fstatus=8, Fstandby1=2 ,Fip='"
                        + userIP + "',Fmodify_time='" + PublicRes.strNowTimeStander + "' where Fstatus=9  and Ftransaction_id='"
                        + tranid + "' and Fdraw_id='" + drawid + "' and ifnull(Fstandby1,0) =0 ";
                }
                else
                {
                    strSql = "update  c2c_zwdb.t_refund_snap set Fstatus=9, Fstandby1=0 ,Fip='"
                        + userIP + "',Fmodify_time='" + PublicRes.strNowTimeStander + "' where Fstatus=8  and Ftransaction_id='"
                        + tranid + "' and Fdraw_id='" + drawid + "' and Fstandby1=2 ";
                }
                int count = dazw.ExecSqlNum(strSql);
                if (count != 1)
                {
                    msg = "更新退款申请单快照表记录数为:" + count + " Fdraw_id=" + drawid;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;

            }
        }

    }
    #endregion


    #region 退款查询类

    public class RefundQueryClass : Query_BaseForNET
    {
        public RefundQueryClass(string batchid, int ifromtype, int irefundtype, int irefundstate, int ireturnstate, string listid)
        {
            string strWhere = " where Fbatchid='" + batchid + "' ";
            if (ifromtype != 9)
            {
                strWhere += " and FrefundType=" + ifromtype;
            }

            if (irefundtype != 9)
            {
                strWhere += " and FrefundPath=" + irefundtype;
            }

            if (irefundstate != 9)
            {
                strWhere += " and Fstate=" + irefundstate;
            }

            if (ireturnstate != 9)
            {
                strWhere += " and FreturnState=" + ireturnstate;
            }

            if (listid != null && listid.Trim() != "")
            {
                strWhere += " and FPaylistid='" + listid.Trim() + "' ";
            }

            fstrSql = "select * from c2c_zwdb.t_refund_total " + strWhere;
            fstrSql_count = "select count(*) from c2c_zwdb.t_refund_total " + strWhere;
        }
    }

    #endregion


    #region 商户操作员查询

    public class MediOperatorManageClass : Query_BaseForNET
    {
        public string ICESQL = "";

        public static int GetRole(string spid, string qq, int signorder)
        {
            if (signorder < 1 || signorder > 10)//对照账务系统将4改为10
            {
                throw new LogicException("权限位越界");
            }

            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("YWB_30"));
            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZL"));
            try
            {
                /*
                // TODO: 1客户信息资料外移
                da.OpenConn();
                string strSql = "select Fsign" + signorder + " from c2c_db.t_muser_user where Fqqid='" + qq 
                    + "' and FSpid='" + spid + "'";

                return Int32.Parse(da.GetOneResult(strSql));
                */

                string strSql = "qqid=" + qq + "&spid=" + spid + "";
                string fieldstr = "Fsign" + signorder;
                string errMsg = "";
                string sign = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MUSER, fieldstr, out errMsg);

                int isign = Int32.Parse(sign);
                return isign;
            }
            finally
            {
                //da.Dispose();
            }
        }

        public static bool SetRole(string spid, string qq, int newrole, int signorder)
        {
            if (signorder < 1 || signorder > 4)
            {
                throw new LogicException("权限位越界");
            }

            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("YW_30"));
            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZL"));
            try
            {
                // TODO: 1客户信息资料外移
                /*
                string STRSQL = "update c2c_db.t_muser_user set Fsign" + signorder + "={0} where Fqqid='{1}' and FSpid='" + spid + "'";

                */

                string STRSQL = "qqid={1}&spid=" + spid;
                STRSQL += "&modify_time=" + PublicRes.strNowTimeStander;
                STRSQL += "&sign" + signorder + "={0}";

                //da.OpenConn();
                string strSql = "";

                string str = Convert.ToString((long)newrole, 2);
                str = str.PadLeft(32, '0');

                if (spid == qq)
                {
                    // TODO: 1客户信息资料外移
                    //先校验,如果是修改的管理员,则把操作员的管理员为0的权限位也置0
                    /*
                    strSql = " select Fqqid,Fsign" + signorder + " from  c2c_db.t_muser_user where Fspid='" + spid + "' ";
                    DataTable dt = da.GetTable(strSql);
                    */

                    string errMsg = "";
                    strSql = "spid=" + spid;
                    DataTable dt = CommQuery.GetTableFromICE(strSql, CommQuery.QUERY_MUSER, out errMsg);

                    if (dt == null || dt.Rows.Count == 0)
                        return false;

                    foreach (DataRow dr in dt.Rows)
                    {
                        string operatorqq = dr["Fqqid"].ToString().Trim();
                        if (operatorqq == spid)
                        {
                            int changedrole = Convert.ToInt32(str, 2);
                            strSql = String.Format(STRSQL, changedrole, spid);
                            /*
                            da.ExecSqlNum(strSql);
                            */
                            CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_MUSER, out errMsg);
                        }
                        else
                        {
                            /*
                            strSql = " select Fsign" + signorder + " from c2c_db.t_muser_user where Fqqid='" + operatorqq + "' and Fspid='" + spid + "'";
                            long opersign = long.Parse(da.GetOneResult(strSql));
                            */
                            strSql = "spid=" + spid + "&qqid=" + operatorqq;
                            long opersign = long.Parse(CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MUSER, "Fsign" + signorder, out errMsg));

                            string strspsign = Convert.ToString(opersign, 2);
                            strspsign = strspsign.PadLeft(32, '0');

                            for (int i = 0; i <= 31; i++)
                            {
                                if (str[i] == '0')
                                {
                                    strspsign = strspsign.Substring(0, i) + "0" + strspsign.Substring(i + 1, 32 - i - 1);
                                }
                            }

                            int changedrole = Convert.ToInt32(strspsign, 2);
                            strSql = String.Format(STRSQL, changedrole, operatorqq);
                            /*
                            da.ExecSqlNum(strSql);
                            */
                            CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_MUSER, out errMsg);
                        }
                    }

                    return true;
                }
                else if (qq.StartsWith(spid) && spid != qq)
                {
                    // TODO: 1客户信息资料外移
                    //如果修改的是操作员,则把管理员为0的权限位也置0
                    /*
                    strSql = " select Fsign" + signorder + " from c2c_db.t_muser_user where Fqqid='" + spid + "' and Fspid='" + spid + "'";
                    long spsign = long.Parse(da.GetOneResult(strSql));
                    */

                    string errMsg = "";
                    strSql = "qqid=" + spid + "&spid=" + spid;
                    long spsign = long.Parse(CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MUSER, "Fsign" + signorder, out errMsg));

                    string strspsign = Convert.ToString(spsign, 2);
                    strspsign = strspsign.PadLeft(32, '0');

                    for (int i = 0; i <= 31; i++)
                    {
                        if (strspsign[i] == '0')
                        {
                            str = str.Substring(0, i) + "0" + str.Substring(i + 1, 32 - i - 1);
                        }
                    }

                    int changedrole = Convert.ToInt32(str, 2);
                    strSql = String.Format(STRSQL, changedrole, qq);

                    /*
                    return da.ExecSqlNum(strSql) == 1;
                    */
                    int iresult = CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_MUSER, out errMsg);
                    return iresult == 1;
                }
                else
                    return false;

            }
            finally
            {
                //da.Dispose();
            }
        }

        public MediOperatorManageClass(string spid, string account)
        {
            string strwhere = "";
            if (spid.Trim() != "")
            {
                strwhere += " where Fspid='" + spid + "' ";
                ICESQL = "spid=" + spid;
            }

            if (account.Trim() != "")
            {
                if (strwhere == "")
                {
                    strwhere += " where Fqqid='" + account + "' ";
                    ICESQL = "qqid=" + account;
                }
                else
                {
                    strwhere += " and Fqqid='" + account + "' ";
                    ICESQL += "&qqid=" + account;
                }
            }


            /*
            // TODO: 1客户信息资料外移
            fstrSql = " select * from c2c_db.t_muser_user " + strwhere;*/

            fstrSql = "strwhere=" + strwhere.Trim();
            fstrSql_count = "select 10000";//" select count(*) from c2c_db.t_muser_user " + strwhere;

        }

    }
    #endregion

    #region 提现规则查询
    public class AppealDQuery : Query_BaseForNET
    {
        public AppealDQuery(string FSpid, string FUser, int FPriType, int FState)
        {
            string wheres = " where 1=1 ";

            if (FSpid != null && FSpid != "") wheres += " and FSpid = '" + FSpid + "' ";
            if (FUser != null && FUser != "") wheres += " and FUser like '" + FUser + "' ";
            if (FPriType != -1) wheres += " and FPriType =" + FPriType.ToString() + " ";
            if (FState != -1) wheres += " and FState=" + FState.ToString();

            fstrSql = "SELECT * FROM c2c_db.t_appeal_d " + wheres + " order by FNo desc";
            fstrSql_count = "SELECT count(1) FROM c2c_db.t_appeal_d " + wheres;
        }
    }
    #endregion

    #region 结算规则查询
    public class AppealSQuery : Query_BaseForNET
    {
        public AppealSQuery(string Fno, string FUserType, string FUser, string FState)
        {
            string wheres = " where 1=1 ";

            if (Fno != null && Fno != "") wheres += " and Fno=" + Fno;
            if (FUserType != null && FUserType != "") wheres += " and FUserType=" + FUserType;
            if (FUser != null && FUser != "") wheres += " and FUser like '%" + FUser + "%' ";
            if (FState != null && FState != "") wheres += " and FState=" + FState;

            fstrSql = "SELECT * FROM c2c_merchant.t_appeal_s " + wheres + " order by FNo desc";
            fstrSql_count = "SELECT count(1) FROM c2c_merchant.t_appeal_s " + wheres;
        }
    }
    #endregion


    #region	【买/卖】家交易单表

    /// <summary>
    /// 【买/卖】家交易单表的查询类
    /// </summary>
    public class Q_PAY_LIST_BYTYPE : Query_BaseForNET
    {
        private string f_strID;
        private int f_iType;
        private DateTime f_dtBegin;
        private DateTime f_dtEnd;


        public Q_PAY_LIST_BYTYPE(string strID, DateTime dtBegin, DateTime dtEnd, int iType)
        {
            f_iType = iType;
            f_strID = strID;
            f_dtBegin = dtBegin;
            f_dtEnd = dtEnd;

            //fstrSql = "Select * from  " + PublicRes.GetTableName("t_pay_list",f_strID) ;
            fstrSql = "Select *,Ftrade_state as Fstate from  " + PublicRes.GetTableName("t_user_order", f_strID);
            //如果是买类型,0
            if (f_iType == 0)
            {
                fstrSql += "  where fbuyid='" + f_strID + "' ";
            }
            else
            {
                fstrSql += "  where fsaleid='" + f_strID + "' ";
            }

            if (dtBegin.ToString("yyyy-MM-dd") != "1900-01-01" && dtEnd.ToString("yyyy-MM-dd") != "4000-01-01")
            {
                fstrSql += " and fcreate_time between '" + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") + "' and '"
                    + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
        }

        /// <summary>
        /// 提供给其它语言调用的函数，以固定的类返回值。
        /// </summary>
        /// <returns></returns>
        public T_PAY_LIST[] GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZJB"));

            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();

                dt = da.GetTable(fstrSql);

                if (dt.Rows.Count > 0)
                {
                    T_PAY_LIST[] result = new T_PAY_LIST[dt.Rows.Count];
                    int i = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        result[i] = new T_PAY_LIST();

                        result[i].u_Bank_ListID = QueryInfo.GetString(dr["fbank_listid"]);
                        result[i].u_Bargain_Time = QueryInfo.GetDateTime(dr["fbargain_time"]);
                        result[i].u_Buy_Bank_Type = QueryInfo.GetInt(dr["fbuy_bank_type"]);
                        result[i].u_Buy_BankID = QueryInfo.GetString(dr["fbuy_bankid"]);
                        result[i].u_Buy_Name = QueryInfo.GetString(dr["fbuy_name"]);
                        result[i].u_BuyID = QueryInfo.GetString(dr["fbuyid"]);
                        result[i].u_Carriage = QueryInfo.GetInt(dr["fcarriage"]);
                        result[i].u_Cash = QueryInfo.GetInt(dr["fcash"]);
                        result[i].u_Coding = QueryInfo.GetString(dr["fcoding"]);
                        result[i].u_Create_Time = QueryInfo.GetDateTime(dr["fcreate_time"]);
                        result[i].u_Create_Time_C2C = QueryInfo.GetDateTime(dr["fcreate_time_c2c"]);
                        result[i].u_CurType = QueryInfo.GetInt(dr["fcurtype"]);
                        result[i].u_Explain = QueryInfo.GetString(dr["fexplain"]);
                        result[i].u_Fact = QueryInfo.GetInt(dr["ffact"]);
                        result[i].u_IP = QueryInfo.GetString(dr["fip"]);
                        result[i].u_LState = QueryInfo.GetInt(dr["flstate"]);
                        result[i].u_Memo = QueryInfo.GetString(dr["fmemo"]);
                        result[i].u_Modify_Time = QueryInfo.GetDateTime(dr["fmodify_time"]);
                        result[i].u_Pay_Type = QueryInfo.GetInt(dr["fpay_type"]);
                        result[i].u_PayNum = QueryInfo.GetInt(dr["fpaynum"]);
                        result[i].u_Price = QueryInfo.GetInt(dr["fprice"]);
                        result[i].u_Procedure = QueryInfo.GetInt(dr["fprocedure"]);
                        result[i].u_Receive_Time = QueryInfo.GetDateTime(dr["freceive_time"]);
                        result[i].u_Receive_Time_C2C = QueryInfo.GetDateTime(dr["freceive_time_c2c"]);
                        result[i].u_Sale_Name = QueryInfo.GetString(dr["fsale_name"]);
                        result[i].u_SaleID = QueryInfo.GetString(dr["fsaleid"]);
                        result[i].u_Service = QueryInfo.GetInt(dr["fservice"]);
                        result[i].u_SPID = QueryInfo.GetString(dr["fspid"]);
                        result[i].u_State = QueryInfo.GetInt(dr["ftrade_state"]);

                        result[i].u_Pay_Time = QueryInfo.GetDateTime(dr["fpay_time"]);

                        i++;
                    }
                    da.CloseConn();
                    da.Dispose();
                    return result;
                }
                else
                {
                    da.CloseConn();
                    da.Dispose();
                    throw new Exception("没有查找到相应的记录");
                }
            }
            catch (Exception e)
            {
                da.CloseConn();
                da.Dispose();
                throw e;
            }
        }
    }
    #endregion


    #region 充值查询类。

    public class FundQueryClass : Query_BaseForNET
    {
        public string ICESQL = "";
        public string ICETYPE = "";
        public string HISSQL = "";

        public FundQueryClass(string u_ID, string u_QueryType, int fcurtype, DateTime u_BeginTime, DateTime u_EndTime, int fstate,
            float fnum, float fnumMax, string banktype, string sorttype, bool isHistory)
        {
            string strGroup = "";
            string strWhere = "";

            //furion 20090515 这个地方的合单查询,改为union 而且以区间来查询.
            string strsource = "";
            string strUnite = "";

            string strsource_ice = "";
            string strunite_ice = "";

            string uniteFlag = ConfigurationManager.AppSettings["UniteFlag"];
            if (string.IsNullOrEmpty(u_ID))
            {
                throw new Exception("ID 不能为空");
            }
            else if (u_QueryType.ToLower() == "qq")  //按照QQ号查询，注意使用内部uid
            {
                string uid = PublicRes.ConvertToFuid(u_ID);
                strWhere += " where fauid='" + uid + "' ";
                ICESQL = "auid=" + uid;
            }
            else if (u_QueryType.ToLower() == "tobank")  //给银行的订单号
            {
                strsource = " where Fbank_list='" + u_ID + "' ";
                strUnite = " where Fbank_list in ('" + uniteFlag + "01" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "02" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "03" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "04" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "05" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "06" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "07" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "08" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "09" + u_ID + "') ";
                strWhere += strsource;
                strsource_ice = "bank_list_or=" + u_ID;
                strunite_ice = "bank_list_likestr=" + u_ID;
            }
            else if (u_QueryType.ToLower() == "bankback") //银行返回u
            {
                strsource = " where Fbank_acc='" + u_ID + "' ";
                strUnite = " where Fbank_acc in ('" + uniteFlag + "01" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "02" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "03" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "04" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "05" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "06" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "07" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "08" + u_ID + "'";
                strUnite += ",'" + uniteFlag + "09" + u_ID + "') ";
                strWhere += strsource;
                strsource_ice = "bank_acc_or=" + u_ID;
                strunite_ice = "bank_acc_likestr=" + u_ID;
            }
            else if (u_QueryType.ToLower() == "czd")
            {
                strWhere += " where listid='" + u_ID + "' ";
                ICESQL = "listid=" + u_ID;
            }
            else
            {
                throw new Exception("queryType 不正确");
            }

            if (fstate != 0)
            {
                if (strWhere != "")
                {
                    ICESQL += "&sign=" + fstate;
                    strWhere += " and FSign=" + fstate.ToString() + " ";
                }
                else
                {
                    ICESQL = "sign=" + fstate;
                    strWhere = " where FSign=" + fstate.ToString() + " ";
                }
            }

            long num = (long)Math.Round(fnum * 100, 0);
            long numMax = (long)Math.Round(fnumMax * 100, 0);

            if (strWhere != "")
            {
                ICESQL += "&num_start=" + num;
                ICESQL += "&num_end=" + numMax;
                strWhere += " and Fnum>" + num.ToString() + " and Fnum<" + numMax.ToString() + " ";
            }
            else
            {
                ICESQL = "num_start=" + num;
                ICESQL += "&num_end=" + numMax;
                strWhere = " where Fnum>" + num.ToString() + " and Fnum<" + numMax.ToString() + " ";
            }

            if (banktype != "0000")
            {
                strWhere += " and Fbank_type=" + banktype + " ";
                ICESQL += "&bank_type=" + banktype;
            }

            string test = u_BeginTime.ToString("yyyy-MM-dd");

            if (test != "1940-01-01")
            {
                if (strWhere != "")
                {

                    strWhere += " and Fpay_front_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                        + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "'  and Fcurtype=" + fcurtype + " ";
                    ICESQL += "&fronttime_start=" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss");
                    ICESQL += "&fronttime_end=" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                    ICESQL += "&curtype=" + fcurtype; //rowena 20100722增加基金项目

                }
                else
                {
                    strWhere += " where Fpay_front_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                        + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "'  and Fcurtype=" + fcurtype + " ";
                    ICESQL = "fronttime_start=" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss");
                    ICESQL += "&fronttime_end=" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                    ICESQL += "&curtype=" + fcurtype; //rowena 20100722增加基金项目
                }
            }

            ICETYPE = CommQuery.QUERY_TCBANKROLL;
            if (test != "1940-01-01")
            {
                //从效率考虑,只取开始时间和结束时间所在月的历史记录,其它不取.
                //DateTime tmpDate = u_BeginTime.AddMonths(-1);
                DateTime tmpDate = u_BeginTime.AddMonths(-1);//先前查询一个月吧
                DateTime date_End = u_EndTime;
                if (DateTime.Now.CompareTo(u_BeginTime.AddMonths(2)) > 0)//默认最多查寻4个月的历史记录吧
                {
                    date_End = u_BeginTime.AddMonths(2);
                }

                while (tmpDate <= date_End)
                {
                    //string TableName = "c2c_db_receive.t_tcbankroll_list_" + tmpDate.ToString("yyyyMM");
                    string TableName = "c2c_db_history.t_tcbankroll_list_" + tmpDate.ToString("yyyyMM");
                    strGroup = strGroup + " select * from " + TableName + strWhere + " union all";
                    tmpDate = tmpDate.AddMonths(1);
                    string strTmp = tmpDate.ToString("yyyy-MM-");
                    tmpDate = DateTime.Parse(strTmp + "01 00:00:01");
                }
                if (isHistory)
                {
                    ICETYPE = CommQuery.QUERY_TCBANKROLL_HISTORY;
                }
                else
                {
                    ICETYPE = CommQuery.QUERY_TCBANKROLL;
                }
            }
            ICESQL += "&start_time=" + test; //增加时间参数 andrew 20110322
            string TableName1 = "c2c_db.t_tcbankroll_list";
            strGroup = strGroup + " select * from " + TableName1 + strWhere + " ";
            //furion 20090515 这个地方的合单查询,改为union 而且以区间来查询.
            if (strsource != "" && strUnite != "")
            {
                strGroup += " union all " + strGroup.Replace(strsource, strUnite);
                ICESQL += "&" + strsource_ice;
                ICESQL += "&" + strunite_ice;
            }

            string strorder = "";
            if (sorttype != null && sorttype.Trim() != "")
            {
                if (sorttype.Trim() == "1")
                {
                    if (u_QueryType == "total")
                    {
                        strorder = " order by Fpay_time_acc asc ";
                    }
                    else
                    {
                        strorder = " order by Fpay_front_time asc ";
                    }
                }
                else if (sorttype.Trim() == "2")
                {
                    if (u_QueryType == "total")
                    {
                        strorder = " order by Fpay_time_acc desc ";
                    }
                    else
                    {
                        strorder = " order by Fpay_front_time desc ";
                    }
                }
                else if (sorttype.Trim() == "3")
                {
                    strorder = " order by Fnum asc ";
                }
                else if (sorttype.Trim() == "4")
                {
                    strorder = " order by Fnum desc ";
                }
            }
            fstrSql = strGroup + strorder;
            fstrSql_count = "select 100000";
            HISSQL = strWhere + strorder;
        }

        public FundQueryClass(string tdeid, string listid, DateTime u_BeginTime, DateTime u_EndTime, bool oldflag, bool isHistory)
        {
            if (string.IsNullOrEmpty(tdeid) && string.IsNullOrEmpty(listid))
            {
                throw new Exception("查询条件不具备");
            }
            string strGroup = "";
            string strWhere = "";

            if (tdeid != null && tdeid.Trim() != "")
            {
                ICESQL = "tde_id=" + tdeid;
                strWhere += " where Ftde_id=" + tdeid + "  ";
            }
            else
            {
                strWhere += "";
            }

            if (listid != null && listid.Trim() != "")
            {
                ICESQL += "&listid=" + listid;
            }

            string test = u_BeginTime.ToString("yyyy-MM-dd");
            if (test != "1940-01-01")
            {
                if (strWhere != "")
                {
                    ICESQL += "&fronttime_start=" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss");
                    ICESQL += "&fronttime_end=" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss");

                    strWhere += " and Fpay_front_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                        + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                }
                else
                {
                    ICESQL += "fronttime_start=" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss");
                    ICESQL += "&fronttime_end=" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss");

                    strWhere = " where Fpay_front_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                        + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                }
            }

            ICETYPE = CommQuery.QUERY_TCBANKROLL;

            if (test != "1940-01-01" && isHistory)
            {
                //从效率考虑,只取开始时间和结束时间所在月的历史记录,其它不取.
                DateTime tmpDate = u_BeginTime.AddMonths(-1);

                DateTime date_End = u_EndTime;
                if (DateTime.Now.CompareTo(u_BeginTime.AddMonths(2)) > 0)//默认最多查寻4个月的历史记录吧
                {
                    date_End = u_BeginTime.AddMonths(2);
                }
                while (tmpDate <= date_End)
                {
                    string TableName = "c2c_db_history.t_tcbankroll_list_" + tmpDate.ToString("yyyyMM");

                    strGroup = strGroup + " select * from " + TableName + strWhere + " union all";

                    tmpDate = tmpDate.AddMonths(1);

                    string strTmp = tmpDate.ToString("yyyy-MM-");
                    tmpDate = DateTime.Parse(strTmp + "01 00:00:01");
                }
                if (isHistory)
                {
                    ICETYPE = CommQuery.QUERY_TCBANKROLL_HISTORY;
                }
                else
                {
                    ICETYPE = CommQuery.QUERY_TCBANKROLL;
                }
            }

            ICESQL += "&start_time=" + u_BeginTime.ToString("yyyy-MM-dd");  //增加时间参数 andrew 20110322
            string TableName1 = "c2c_db.t_tcbankroll_list";
            strGroup = strGroup + " select * from " + TableName1 + strWhere + " ";

            fstrSql = strGroup;
            fstrSql_count = "select 10000";//" select count(*) from ( " + strGroup + ")  a ";
        }
    }

    #endregion

    #region 同步记录查询

    public class SynRecordClass : Query_BaseForNET
    {
        public static string GetNowSynTableName()
        {
            int idate = DateTime.Now.Day;
            string nowtablename = DateTime.Now.ToString("yyyyMM");

            if (idate < 11)
            {
                nowtablename += "01";
            }
            else if (idate < 21)
            {
                nowtablename += "02";
            }
            else
                nowtablename += "03";

            return nowtablename;

        }

        public static string GetSynTableNameFromDate(DateTime dt)
        {
            int idate = dt.Day;
            string nowtablename = dt.ToString("yyyyMM");

            if (idate < 11)
            {
                nowtablename += "01";
            }
            else if (idate < 21)
            {
                nowtablename += "02";
            }
            else
                nowtablename += "03";

            return nowtablename;

        }
        public static string GetPriorSynTableNameFromDate(DateTime dt)
        {
            string nowtablename = GetSynTableNameFromDate(dt);

            if (nowtablename.EndsWith("01"))
            {
                nowtablename = DateTime.Now.AddMonths(-1).ToString("yyyyMM") + "03";
            }
            else
            {
                int inowindex = Int32.Parse(nowtablename.Substring(6, 2));
                inowindex--;
                nowtablename = nowtablename.Substring(0, 6) + "0" + inowindex.ToString();
            }

            return nowtablename;
        }

        public static string GetPriorSynTableName()
        {
            string nowtablename = GetNowSynTableName();

            if (nowtablename.EndsWith("01"))
            {
                nowtablename = DateTime.Now.AddMonths(-1).ToString("yyyyMM") + "03";
            }
            else
            {
                int inowindex = Int32.Parse(nowtablename.Substring(6, 2));
                inowindex--;
                nowtablename = nowtablename.Substring(0, 6) + "0" + inowindex.ToString();
            }

            return nowtablename;
        }

        /*
        public static string GetPriorSynTableName()
        {
            string nowtablename = GetNowSynTableName();

            if(nowtablename.EndsWith("01"))
            {
                nowtablename = DateTime.Now.AddMonths(-1).ToString("yyyyMM") + "01";
            }
            else
            {
                int inowindex = Int32.Parse(nowtablename.Substring(6,2));
                inowindex --;
                nowtablename = nowtablename.Substring(0,6) + "0" + inowindex.ToString();
            }

            return nowtablename;
        }
        */

        public string ICESQL = "";
        public static void ResetSynRecordState(string transid, string createtime, int inum)
        {
            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("SYNINFO"));
            try
            {
                //furion 得到当前时间值.
                //long timestamp = long.Parse(Common.CommLib.commRes.GetTickFromTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                /*
                string strSql = "update cft_syn_record.t_syn_record set Fsyn_count=" + inum 
                    + ",Fsyn_status=2,Flast_modify_time="+timestamp+",Fsyn_status=1 where Ftransaction_id='" + transid + "'";

                if(inum > 0)
                {
                    strSql = "update cft_syn_record.t_syn_record set Fsyn_count=" + inum 
                        + ",Fsyn_status=2,Flast_modify_time="+timestamp+" where Ftransaction_id='" + transid + "'";
                }

                da.OpenConn();

                da.ExecSql(strSql);
                */

                string Msg = "";
                string sql = "transaction_id=" + transid + "&create_time=" + SynRecordClass.GetNowSynTableName();
                string pay_type = CommQuery.GetOneResultFromICE(sql, CommQuery.QUERY_SYNREC_ID, "Fpay_type", out Msg);

                if (pay_type == null || pay_type.Trim() == "")
                {
                    sql = "transaction_id=" + transid + "&create_time=" + SynRecordClass.GetPriorSynTableName();
                    pay_type = TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.GetOneResultFromICE(sql, CommQuery.QUERY_SYNREC_ID, "Fpay_type", out Msg);

                    if (pay_type == null || pay_type.Trim() == "")
                    {
                        //找不到记录，即使调用接口也是失败。
                        Msg = "调用接口syn_update4KZ_service前，查询记录失败" + transid;
                        TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.sendLog4Log("TradeSynch.noc2cSynch", Msg);
                        return;
                    }
                }

                string inmsg1 = "&transaction_id=" + transid;
                inmsg1 += "&update_type=2";
                inmsg1 += "&pay_type=" + pay_type;
                //inmsg1 += "&call_source=KZ";				
                inmsg1 += "&modify_time=" + TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTickFromTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                string reply = "";
                short result;
                string msg = "";


                if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("syn_update4KZ_service", inmsg1, true, out reply, out result, out msg))
                {
                    if (result != 0)
                    {
                        Msg = "调用接口syn_update4KZ_service未能成功 result=" + result + "，msg=" + msg;
                        TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.sendLog4Log("TradeSynch.noc2cSynch", Msg);
                    }
                    else
                    {
                        if (reply.IndexOf("result=0") > -1)
                        {
                            return;
                        }
                        else
                        {
                            Msg = "调用接口syn_update4KZ_service未能成功" + reply;
                            TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.sendLog4Log("TradeSynch.noc2cSynch", Msg);
                        }
                    }
                }
                else
                {
                    Msg = "调用接口syn_update4KZ_service未能成功" + inmsg1;
                    TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.sendLog4Log("TradeSynch.noc2cSynch", Msg);
                }
            }
            catch
            { }
            finally
            {
                //da.Dispose();
            }
        }


        public SynRecordClass(string transid, string createdate, int flag)
        {
            if (flag == 0)
            {
                fstrSql = "select * from cft_syn_record.t_syn_record where Ftransaction_id='" + transid + "'";
                fstrSql_count = "select count(*) from cft_syn_record.t_syn_record where Ftransaction_id='" + transid + "'";
            }
            else
            {
                string tablename = "cft_syn_record_history.t_syn_record_" + DateTime.Parse(createdate).ToString("yyyyMM");
                fstrSql = "select * from " + tablename + " where Ftransaction_id='" + transid + "'";
                fstrSql_count = "select count(*) from cft_syn_record.t_syn_record where Ftransaction_id='" + transid + "'";
            }
        }

        public SynRecordClass(string transid)
        {
            fstrSql = "select *,0 as flag from cft_syn_record.t_syn_record where Ftransaction_id='" + transid + "'";
            fstrSql_count = "select 10000";
        }

        //同步支付状态
        public static bool SynPayState(string transid, string createtime,out string strMsg)
        {

            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("SYNINFO"));
            //MySqlAccess dayw = new MySqlAccess(PublicRes.GetConnString("YW"));
            strMsg = "开始";
            try
            {
                //da.OpenConn();
                //dayw.OpenConn();

                long timestamp = long.Parse(TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTickFromTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                /*
                string strSql="select Fpay_time from "+ PublicRes.GetTName("t_tran_list",transid) +" where flistid='"+transid+"' and Fstate=2";

                string pay_time=dayw.GetOneResult(strSql);
                */

                string msg = "";
                string strSql = "listid=" + transid + "&state=2"; //配置中增加了state参数
                string pay_time = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_ORDER, "Fpay_time", out msg);
                strMsg += string.Format("strSql={0} pay_time={1}", strSql, pay_time);
                if (pay_time == null || pay_time == "")
                {
                    strMsg += string.Format("出错原因：{0}",msg);
                    return false;
                }

                string pay_type = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_ORDER, "Fpay_type", out msg);
                /*
                string strUpdate  = "update cft_syn_record.t_syn_record set Fsyn_count= 0" 
                    + ",Fsyn_status=1,Flast_modify_time="+timestamp+",fpay_status=2,Fpay_time='"+pay_time+"' where Ftransaction_id='" + transid + "' and Fpay_status=1";

								
                return da.ExecSql(strUpdate);
                */

                string inmsg1 = "&transaction_id=" + transid;
                inmsg1 += "&update_type=1";
                inmsg1 += "&pay_type=" + pay_type;
                //inmsg1 += "&call_source=KZ";	
                inmsg1 += "&pay_status=2";
                inmsg1 += "&pay_time=" + TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTickFromTime(pay_time);
                inmsg1 += "&modify_time=" + timestamp;
                
                strMsg += string.Format("inmsg1={0}", inmsg1);
                string reply = "";
                short result;

                string Msg = "";

                if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("syn_update4KZ_service", inmsg1, true, out reply, out result, out msg))
                {
                    if (result != 0)
                    {
                        Msg = "调用接口syn_update4KZ_service未能成功 result=" + result + "，msg=" + msg;

                        strMsg += string.Format("Msg={0}", Msg);
                        TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.sendLog4Log("TradeSynch.noc2cSynch", Msg);
                        return false;
                    }
                    else
                    {
                        if (reply.IndexOf("result=0") > -1)
                        {
                            return true;
                        }
                        else
                        {
                            Msg = "调用接口syn_update4KZ_service未能成功" + reply;
                            strMsg += string.Format("Msg={0}", Msg);
                            TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.sendLog4Log("TradeSynch.noc2cSynch", Msg);
                            return false;
                        }
                    }
                }
                else
                {
                    Msg = "调用接口syn_update4KZ_service未能成功" + inmsg1;
                    strMsg += string.Format("Msg={0}", Msg);
                    TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.sendLog4Log("TradeSynch.noc2cSynch", Msg);
                    return false;
                }
            }
            catch (Exception ex)
            {
                strMsg += string.Format("Exception ex={0}", ex.Message);
                throw new Exception(ex.Message);
                return false;
            }
            finally
            {
                //da.Dispose();
                //dayw.Dispose();
            }
        }

        /*
        public SynRecordClass(string transid, string createdate, int flag)
        {
            if(flag == 0)
            {
                fstrSql = "transaction_id=" + transid + "&create_time=" + createdate;
                ICESQL =fstrSql;
            }
            else
            {
                /*
                string tablename = "cft_syn_record_history.t_syn_record_" + DateTime.Parse(createdate).ToString("yyyyMM");
                fstrSql = "select * from " + tablename + " where Ftransaction_id='" + transid + "'";
                fstrSql_count = "select count(*) from cft_syn_record.t_syn_record where Ftransaction_id='" + transid + "'";
				
                fstrSql = "transaction_id=" + transid + "&create_time=" + createdate;
                ICESQL =fstrSql;
            }
        }
        */



        public SynRecordClass(string transid, string begintime, string endtime, int paystatus,
            int synstatus, int syntype, int paytype, string spid, string spbillno, string purchaser, string bargainor, int flag, int synresult)
        {
            if (transid.Trim() == "" && spid.Trim() == "")
            {
                throw new LogicException("商户ID和交易单ID必输入其中一个.");
            }

            string strGroup = "";
            string strWhere = "";

            string ibegin = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTickFromTime(begintime);
            string iend = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTickFromTime(endtime);

            strWhere = "create_time=" + GetSynTableNameFromDate(DateTime.Parse(endtime));
            strWhere += "&start_time=" + ibegin + "&end_time=" + iend;


            if (spid.Trim() != "")
            {
                strWhere += "&sp_id=" + spid;
            }

            if (paystatus != 9)
            {
                strWhere += "&pay_status=" + paystatus;
            }

            if (synstatus != 9)
            {
                strWhere += "&syn_status=" + synstatus;
            }

            if (syntype != 9)
            {
                strWhere += "&syn_type=" + syntype;
            }

            if (paytype != 9)
            {
                strWhere += "&pay_type=" + paytype;
            }

            if (synresult != 9)
            {
                strWhere += "&syn_result=" + synresult;
            }

            if (spbillno.Trim() != "")
            {
                strWhere += "&sp_billno=" + spbillno;
            }

            if (purchaser.Trim() != "")
            {
                strWhere += "&purchaser_uin=" + purchaser;
            }

            if (bargainor.Trim() != "")
            {
                strWhere += "&bargainor_uin=" + bargainor;
            }

            if (transid.Trim() != "")
            {
                //furion 20090514 如果指定交易单，不再使用时间条件
                strWhere = "create_time=" + GetSynTableNameFromDate(DateTime.Parse(endtime));
                strWhere += "&transaction_id=" + transid;
            }

            ICESQL = strWhere;

            /*
            if(flag == 1)
            {
                DateTime tmpDate = DateTime.Parse(begintime);

                while(tmpDate <= DateTime.Parse(endtime))
                {
                    string TableName = "cft_syn_record_history.t_syn_record_" + tmpDate.ToString("yyyyMM");

                    strGroup = strGroup + " select *,1 as flag from " + TableName + strWhere + " union all";

                    tmpDate = tmpDate.AddMonths(1);

                    string strTmp = tmpDate.ToString("yyyy-MM-");
                    tmpDate = DateTime.Parse(strTmp + "01 00:00:00");
                }
            }
			
            string TableName1 = "cft_syn_record.t_syn_record";
            strGroup = strGroup + " select *,0 as flag from " + TableName1 + strWhere + " ";


            fstrSql = strGroup;
            fstrSql_count  = "select 10000";//" select count(*) from ( " + strGroup + ")  a ";
            */
        }
    }

    #endregion


    #region 订单查询类

    public class OrderQueryClass : Query_BaseForNET
    {
        public OrderQueryClass(string listid)
        {
            fstrSql = "select * from " + PublicRes.GetTName("t_order", listid) + " where Flistid='" + listid + "'";
            fstrSql_count = "select count(*) from " + PublicRes.GetTName("t_order", listid) + " where Flistid='" + listid + "'";
        }

        public OrderQueryClass(DateTime u_BeginTime, DateTime u_EndTime, string buyqq, string saleqq,
            string u_QueryType, string queryvalue, int fstate)
        {
            if (u_QueryType != "FlistID" || queryvalue.Trim() == "")
            {
                if (buyqq.Trim() == "" && saleqq.Trim() == "")
                {
                    throw new Exception("买家帐号,卖家帐号,交易单ID至少输入一个");
                }

                string tablename = "";
                string strWhere = " where FCreate_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                    + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                if (buyqq.Trim() != "")
                {
                    string buyid = PublicRes.ConvertToFuid(buyqq);

                    tablename = PublicRes.GetTName("t_user_order", buyid);
                    strWhere += " and FBuy_uid=" + buyid;
                }

                if (saleqq.Trim() != "")
                {
                    string saleid = PublicRes.ConvertToFuid(saleqq);

                    tablename = PublicRes.GetTName("t_user_order", saleid);
                    strWhere += " and FSale_uid=" + saleid;
                }

                if (queryvalue.Trim() != "")
                    strWhere += " and " + u_QueryType + "='" + queryvalue + "' ";

                if (fstate != 99)
                    strWhere += " and FTrade_State=" + fstate;

                fstrSql = "select * from " + tablename + strWhere;
                fstrSql_count = "select count(*) from " + tablename + strWhere;
            }
            else
            {
                fstrSql = "select * from " + PublicRes.GetTName("t_order", queryvalue) + " where Flistid='" + queryvalue + "'";
                fstrSql_count = "select count(*) from " + PublicRes.GetTName("t_order", queryvalue) + " where Flistid='" + queryvalue + "'";
            }
        }
    }

    #endregion
    #region 订单查询类

    public class OrderQueryClassZJ : Query_BaseForNET
    {
        public string ICESQL = "";

        public OrderQueryClassZJ(string listid)
        {
            ICESQL = "listid=" + listid;
            fstrSql = "select * from " + PublicRes.GetTName("t_order", listid) + " where Flistid='" + listid + "'";
            fstrSql_count = "select count(*) from " + PublicRes.GetTName("t_order", listid) + " where Flistid='" + listid + "'";
        }

        public OrderQueryClassZJ(DateTime u_BeginTime, DateTime u_EndTime, string buyqq, string saleqq,string buyqqInnerID, string saleqqInnerID,string u_QueryType, string queryvalue, int fstate, int fcurtype)
        {
            if (u_QueryType != "FlistID" || queryvalue.Trim() == "")
            {
                if (buyqq.Trim() == "" && saleqq.Trim() == "" && buyqqInnerID == "" && saleqqInnerID=="")
                {
                    throw new Exception("买家帐号,卖家帐号,买家帐号内部ID,卖家帐号内部ID，交易单ID至少输入一个");
                }

                string tablename = "";
                string strWhere = " where FCreate_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                    + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' and Fcurtype=" + fcurtype + " ";

                if (buyqqInnerID.Trim() != "")
                {
                    tablename = PublicRes.GetTName("t_user_order", buyqqInnerID);
                    strWhere += " and FBuy_uid=" + buyqqInnerID;
                }

                if (saleqqInnerID.Trim() != "")
                {
                    tablename = PublicRes.GetTName("t_user_order", saleqqInnerID);
                    strWhere += " and FSale_uid=" + saleqqInnerID;
                }

                if (buyqq.Trim() != "")
                {
                    string buyid = PublicRes.ConvertToFuid(buyqq);

                    if (buyid == null || buyid == "" || buyid == "0")
                    {
                        throw new LogicException("买家未注册");
                    }

                    tablename = PublicRes.GetTName("t_user_order", buyid);
                    strWhere += " and FBuy_uid=" + buyid;
                }

                if (saleqq.Trim() != "")
                {
                    string saleid = PublicRes.ConvertToFuid(saleqq);

                    if (saleid == null || saleid == "" || saleid == "0")
                    {
                        throw new LogicException("卖家未注册");
                    }

                    tablename = PublicRes.GetTName("t_user_order", saleid);
                    strWhere += " and FSale_uid=" + saleid;
                }

                if (queryvalue.Trim() != "")
                    strWhere += " and " + u_QueryType + "='" + queryvalue + "' ";

                if (fstate != 99)
                    strWhere += " and FTrade_State=" + fstate;

                fstrSql = "select * from " + tablename + strWhere;
                fstrSql_count = "select 10000";//"select count(*) from " + tablename + strWhere;					
            }
            else
            {
                ICESQL = "listid=" + queryvalue;
                fstrSql = "select * from " + PublicRes.GetTName("t_order", queryvalue) + " where Flistid='" + queryvalue + "' and Fcurtype=" + fcurtype + "";
                fstrSql_count = "select count(*) from " + PublicRes.GetTName("t_order", queryvalue) + " where Flistid='" + queryvalue + "' and Fcurtype=" + fcurtype + "";
            }
        }
    }

    #endregion

    #region 订单实时查询类
    public class RealTimeOrderQueryClass : Query_BaseForNET
    {

        public RealTimeOrderQueryClass(string u_ID, DateTime u_BeginTime, DateTime u_EndTime, int fstate, float fnum, string banktype, string sorttype)
        {
            string strGroup = "";
            string strWhere = "";

            if (u_ID != null && u_ID.Trim() != "")
            {
                strWhere += " where Freq_orderno='" + u_ID + "' ";
            }

            if (fstate != 9)
            {
                if (strWhere == "")
                {
                    strWhere = " where FSign=" + fstate.ToString() + " ";
                }
                else
                    strWhere += " and FSign=" + fstate.ToString() + " ";
            }

            if (banktype != "0000")
            {
                if (strWhere == "")
                {
                    strWhere = " where Fbank_type=" + banktype.ToString() + " ";
                }
                else
                    strWhere += " and Fbank_type=" + banktype.ToString() + " ";
            }

            long num = (long)Math.Round(fnum * 100, 0);

            if (strWhere == "")
            {
                strWhere = " where Famount>=" + num.ToString() + " ";
            }
            else
                strWhere += " and Famount>=" + num.ToString() + " ";

            string test = u_BeginTime.ToString("yyyy-MM-dd");
            if (test != "1940-01-01")
            {
                strWhere += " and Fbanktran_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                    + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                DateTime tmpDate = u_BeginTime.AddDays(-1);

                while (tmpDate <= u_EndTime.AddDays(1))
                {
                    string TableName = "cft_bankorder_" + tmpDate.ToString("yyyyMM") + " .t_bankorder_info_" + tmpDate.ToString("dd");

                    strGroup = strGroup + " select * from " + TableName + strWhere + " union all";

                    tmpDate = tmpDate.AddDays(1);

                    string strTmp = tmpDate.ToString("yyyy-MM-dd");
                    tmpDate = DateTime.Parse(strTmp + " 00:00:01");
                }

                strGroup = strGroup.Substring(0, strGroup.Length - 10);
            }

            string strorder = "";
            if (sorttype != null && sorttype.Trim() != "")
            {
                if (sorttype.Trim() == "1")
                {
                    strorder = " order by Fcreate_time asc ";
                }
                else if (sorttype.Trim() == "2")
                {
                    strorder = " order by Fcreate_time desc ";
                }
                else if (sorttype.Trim() == "3")
                {
                    strorder = " order by FAmount asc ";
                }
                else if (sorttype.Trim() == "4")
                {
                    strorder = " order by FAmount desc ";
                }
            }

            fstrSql = strGroup + strorder;

            fstrSql_count = "select 10000";

        }


    }

    #endregion

    #region 投诉单查询类

    public class AppealQueryClass : Query_BaseForNET
    {
        public string ICESQL;
        //			select * from c2c_db_appeal.t_appeal where 1=1 [:and Fappealid='$sppealid$':] [:and Flistid='$listid$':] [:and Fstate=$state$:] [:and Fuid=$uid$:]
        //			[:and Fappeal_time between '$time_start$' and '$time_end$':] [:and Fvs_uid=$vs_uid$:];

        public AppealQueryClass(string appealid)
        {
            fstrSql = "select * from c2c_db_appeal.t_appeal where Fappealid='" + appealid + "'";
            fstrSql_count = "select count(*) from c2c_db_appeal.t_appeal where Fappealid='" + appealid + "'";

            //ICESQL = "appealid=" + appealid ;
            ICESQL = "sppealid=" + appealid;
        }

        public AppealQueryClass(DateTime u_BeginTime, DateTime u_EndTime, string buyqq, string saleqq,
            string queryvalue, int fstate)
        {

            string tablename = "c2c_db_appeal.t_appeal";
            string strWhere = " where Fappeal_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

            ICESQL = "time_start=" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss");
            ICESQL += "&time_end=" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss");

            if (buyqq.Trim() != "")
            {
                string buyid = PublicRes.ConvertToFuid(buyqq);

                strWhere += " and Fuid=" + buyid;
                ICESQL += "&uid=" + buyid;
            }

            if (saleqq.Trim() != "")
            {
                string saleid = PublicRes.ConvertToFuid(saleqq);

                strWhere += " and Fvs_uid=" + saleid;
                ICESQL += "&vs_uid=" + saleid;
            }

            if (queryvalue.Trim() != "")
            {
                strWhere += " and Flistid='" + queryvalue + "' ";
                ICESQL += "&listid=" + queryvalue;
            }

            if (fstate != 99)
            {
                strWhere += " and FState=" + fstate;
                ICESQL += "&state=" + fstate;
            }

            fstrSql = "select * from " + tablename + strWhere;
            fstrSql_count = "select 10000";//"select count(*) from " + tablename + strWhere;	
        }
    }

    #endregion

    #region 提现查询类
    public class PickQueryClass : Query_BaseForNET
    {
        public PickQueryClass(string u_ID)
        {

            //**furion提现单改造20120216
            fstrSql = "select " + GetTcPayListOldFields() + " from c2c_db.t_tcpay_list where  Flistid='" + u_ID + "'";

            string currtable = "";
            string othertable = "";
            GetPayListTableFromID(u_ID, out currtable, out othertable);

            fstrSql += " union all select " + GetTcPayListNewFields() + " from " + currtable + " where  Flistid='" + u_ID + "'";
            fstrSql += " union all select " + GetTcPayListNewFields() + " from " + othertable + " where  Flistid='" + u_ID + "'";

            //fstrSql_count = "select count(*) from c2c_db.t_tcpay_list where Flistid='" + u_ID + "'";
            fstrSql_count = "select 1";
        }

        // 2012/5/29 添加是否解密银行卡参数
        public PickQueryClass(string u_ID, DateTime u_BeginTime, DateTime u_EndTime, int fstate, float fnum, string banktype, string sorttype, int idtype, string cashtype, bool isSecret)
        {

            string strGroup = "";
            string strWhere = "";

            if (u_ID != null && u_ID.Trim() != "")
            {
                if (idtype == 0)
                {
                    //furion 20051101 以后查询全以内部ID开始.
                    string uid = PublicRes.ConvertToFuid(u_ID);

                    //strWhere += " where faid='" + u_ID + "' and Fsubject=14 ";
                    //strWhere += " where Fuid=" + uid + " and Fsubject=14 ";
                    strWhere += " where Fuid=" + uid + " ";
                }
                else if (idtype == 1)
                {
                    if (isSecret)
                    {
                        string bankID = BankLib.BankIOX.GetCreditEncode(u_ID, BankLib.BankIOX.fxykconn);
                        strWhere += " where Fabankid='" + bankID + "' ";
                    }
                    else
                    {
                        strWhere += " where Fabankid='" + u_ID + "' ";
                    }
                }
                else if (idtype == 2)
                {
                    strWhere += "  where Flistid='" + u_ID + "' ";
                }
            }

            if (fstate != 0)
            {
                if (strWhere != "")
                {
                    strWhere += " and FSign=" + fstate.ToString() + " ";
                }
                else
                {
                    strWhere += " where FSign=" + fstate.ToString() + " ";
                }
            }

            long num = (long)Math.Round(fnum * 100, 0);
            if (strWhere != "")
            {
                strWhere += " and Fnum>=" + num.ToString() + " ";
            }
            else
            {
                strWhere += " where Fnum>=" + num.ToString() + " ";
            }

            if (banktype != "0000")
            {
                strWhere += " and Fabank_type=" + banktype + " ";
            }
            if (cashtype != "0000")
            {
                strWhere += " and Fbankid=" + cashtype + " ";
            }

            string test = u_BeginTime.ToString("yyyy-MM-dd");
            if (test != "1940-01-01")
            {
                strWhere += " and Fpay_front_time_acc between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                    + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                //从效率考虑,只取开始时间和结束时间所在月的历史记录,其它不取.
                //DateTime tmpDate = u_BeginTime.AddMonths(-1);
                DateTime tmpDate = u_BeginTime;
                //while(tmpDate <= u_EndTime.AddMonths(1))
                while (tmpDate <= u_EndTime)
                {
                    //string TableName = "c2c_db_tcpay.t_tcpay_list_" + tmpDate.ToString("yyyyMM");
                    string TableName = "c2c_db.t_tcpay_list_" + tmpDate.ToString("yyyyMM");

                    strGroup = strGroup + " select " + GetTcPayListNewFields() + " from " + TableName + strWhere + " union all";

                    tmpDate = tmpDate.AddMonths(1);

                    string strTmp = tmpDate.ToString("yyyy-MM-");
                    tmpDate = DateTime.Parse(strTmp + "01 00:00:01");
                }

            }
            //**furion提现单改造20120216
            string TableName1 = "c2c_db.t_tcpay_list";
            strGroup = strGroup + " select " + GetTcPayListOldFields() + " from " + TableName1 + strWhere + " ";

            string strorder = "";
            if (sorttype != null && sorttype.Trim() != "")
            {
                if (sorttype.Trim() == "1")
                {
                    strorder = " order by Fpay_front_time_acc asc ";
                }
                else if (sorttype.Trim() == "2")
                {
                    strorder = " order by Fpay_front_time_acc desc ";
                }
                else if (sorttype.Trim() == "3")
                {
                    strorder = " order by Fnum asc ";
                }
                else if (sorttype.Trim() == "4")
                {
                    strorder = " order by Fnum desc ";
                }
            }

            fstrSql = strGroup + strorder;
            fstrSql_count = "select 10000";//" select count(*) from ( " + strGroup + ") a ";

        }

        public PickQueryClass(string listid, DateTime u_BeginTime, DateTime u_EndTime, bool oldflag)
        {
            string strGroup = "";
            string strWhere = "";

            string test = u_BeginTime.ToString("yyyy-MM-dd");

            if (listid != null && listid.Trim() != "")
            {
                strWhere += " where Flistid='" + listid + "' ";

                string currtable = "";
                string othertable = "";
                GetPayListTableFromID(listid, out currtable, out othertable);

                strGroup += "  select " + GetTcPayListNewFields() + " from " + currtable + " where  Flistid='" + listid + "' union all ";
                strGroup += "  select " + GetTcPayListNewFields() + " from " + othertable + " where Flistid='" + listid + "' union all ";
                test = "1940-01-01"; //不再使用里面的循环体构造strgroup
            }
            else
            {
                //strWhere += " where Fsubject=14 ";
            }

            if (test != "1940-01-01")
            {
                if (strWhere == "")
                {
                    strWhere = " where Fpay_front_time_acc between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                        + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                }
                else
                {
                    strWhere += " and Fpay_front_time_acc between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                        + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                }

                //从效率考虑,只取开始时间和结束时间所在月的历史记录,其它不取.
                //DateTime tmpDate = u_BeginTime.AddMonths(-1);
                DateTime tmpDate = u_BeginTime;
                //while(tmpDate <= u_EndTime.AddMonths(1))
                while (tmpDate <= u_EndTime)
                {
                    //string TableName = "c2c_db_tcpay.t_tcpay_list_" + tmpDate.ToString("yyyyMM");
                    string TableName = "c2c_db.t_tcpay_list_" + tmpDate.ToString("yyyyMM");

                    strGroup = strGroup + " select " + GetTcPayListNewFields() + " from " + TableName + strWhere + " union all";

                    tmpDate = tmpDate.AddMonths(1);

                    string strTmp = tmpDate.ToString("yyyy-MM-");
                    tmpDate = DateTime.Parse(strTmp + "01 00:00:01");
                }

            }
            //**furion提现单改造20120216
            string TableName1 = "c2c_db.t_tcpay_list";
            strGroup = strGroup + " select " + GetTcPayListOldFields() + " from " + TableName1 + strWhere + " ";


            fstrSql = strGroup;
            fstrSql_count = "select 10000";//" select count(*) from ( " + strGroup + ")  a ";
        }

        public static string GetTcPayListNewFields()
        {
            return " Ftde_id,Flistid,Fdraw_id,Fsp_batch,Fsp_serial,Fsp_operator,Fspid,Fbankid,Fproduct,Fbusiness_type,Ftype,Fsubject,Fnum,"
                + "Fcharge,Fcharge_payer,Fcharge_recv_uid,Fstate,Fsign,Fbank_list,Fbank_acc,Fbank_type,Fcurtype,Fuid,Faid,Faname,Fabank_type,"
                + "Fabankid,Fprove,Fip,Fmemo,Fbank_memo,Fresult,Frefund_ticket_flag,Frefund_ticket_list,Fpay_front_time,Fpay_front_time_acc,"
                + "Fpay_time,Fpay_time_acc,Fmodify_time,Fbank_name,Farea,Fcity,Facc_name,Fuser_type,Fstandby1,Fstandby2,Fstandby3,Fstandby4,Fstandby5,Fstandby6 ";
        }

        public static string GetTcPayListOldFields()
        {

            return " Ftde_id,Flistid,'' as Fdraw_id,'' as Fsp_batch,'' as Fsp_serial,'' as Fsp_operator,Fspid,Fbankid,0 as Fproduct,0 as Fbusiness_type,Ftype,Fsubject,Fnum,"
                + "0 as Fcharge,0 as Fcharge_payer,0 as Fcharge_recv_uid,Fstate,Fsign,Fbank_list,Fbank_acc,Fbank_type,Fcurtype,Fuid,Faid,Faname,Fabank_type,"
                + "Fabankid,Fprove,Fip,Fmemo,'' as Fbank_mem,'' as Fresult,1 as Frefund_ticket_flag,'' as Frefund_ticket_list,Fpay_front_time,Fpay_front_time_acc,"
                + "Fpay_time,Fpay_time_acc,Fmodify_time,Fbank_name,Farea,Fcity,Facc_name,Fuser_type,0 as Fstandby1,0 as Fstandby2,'' as Fstandby3,'' as Fstandby4,'' as Fstandby5,'' as Fstandby6 ";
        }

        public static void GetPayListTableFromTime(DateTime datetime, out string currtable, out string othertable)
        {
            currtable = "c2c_db.t_tcpay_list_" + datetime.ToString("yyyyMM");

            if (datetime.Day > 15)
            {
                othertable = "c2c_db.t_tcpay_list_" + datetime.AddMonths(1).ToString("yyyyMM");
            }
            else
            {
                othertable = "c2c_db.t_tcpay_list_" + datetime.AddMonths(-1).ToString("yyyyMM");
            }
        }

        public static void GetPayListTableFromID(string listid, out string currtable, out string othertable)
        {
            //1、3位系统ID+YYYYMMDD+10位流水号；
            //2、3位系统ID+10位商户号+YYYYMMDD+7位序列号

            string strdate = "";
            if (listid.Length == 21)
            {
                strdate = listid.Substring(3, 4) + "-" + listid.Substring(7, 2) + "-" + listid.Substring(9, 2);
            }
            else if (listid.Length == 28)
            {
                strdate = listid.Substring(13, 4) + "-" + listid.Substring(17, 2) + "-" + listid.Substring(19, 2);
            }

            DateTime dt = DateTime.Now;
            try
            {
                dt = DateTime.Parse(strdate);
            }
            catch
            {
                dt = DateTime.Now;
            }

            GetPayListTableFromTime(dt, out currtable, out othertable);
        }
    }

    #endregion

    #region 投诉商户查询类
    public class ComplainBussClass : Query_BaseForNET
    {
        public ComplainBussClass()
        {
           
        }

        public ComplainBussClass(string bussid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ht"));

            try
            {
                da.OpenConn();
                string strSql = "  select " + GetComplainBussListFields() + " from  c2c_fmdb.t_complain_buss_list where Fbuss_id='" + bussid+"'";
                DataTable dt = da.GetTable(strSql);
                if (dt == null || dt.Rows.Count != 1)
                {
                    throw new LogicException("读取数据有误！");
                }
                FBussId = bussid;
                object obj = dt.Rows[0]["Fbuss_name"];
                if (obj != null) FBussName = obj.ToString().Trim();
                obj = dt.Rows[0]["Fbuss_email"];
                if (obj != null) FBussEmail = obj.ToString().Trim();
                obj = dt.Rows[0]["Fcreate_time"];
                if (obj != null) FCreateTime = obj.ToString().Trim();
                obj = dt.Rows[0]["Fmodify_time"];
                if (obj != null) FModifyTime = obj.ToString().Trim();
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
            finally 
            {
                da.Dispose();
            }
        }

        public ComplainBussClass(string bussid, DateTime u_BeginTime, DateTime u_EndTime)
        {
            string strGroup = "  select " + GetComplainBussListFields() + " from  c2c_fmdb.t_complain_buss_list where 1=1 ";
            string strWhere = "";

            if (bussid != null && bussid.Trim() != "")
            {
                strWhere += " and Fbuss_id='" + bussid + "'";
            }
            else {
                strWhere += " and (Fcreate_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                    + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' )";
            }

            fstrSql = strGroup + strWhere;
            fstrSql_count = "select count(1) from c2c_fmdb.t_complain_buss_list where 1=1 " + strWhere;
        }

        public string FBussId = "";
        public string FBussName = "";
        public string FBussEmail = "";
        public string FCreateTime = "";
        public string FModifyTime = "";

        public bool addComplainBuss(out string msg)
        {
            msg = "";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ht"));

            try 
            {
                da.OpenConn();
                string strSql = "select " + GetComplainBussListFields() + " from c2c_fmdb.t_complain_buss_list where Fbuss_id='" + FBussId+"'";
                DataTable dt = da.GetTable(strSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    msg = "添加失败:商户号码已存在" + FBussId;
                    return false;
                }

                strSql = "insert into c2c_fmdb.t_complain_buss_list(Fbuss_id,Fbuss_name,Fbuss_email,Fcreate_time,Fmodify_time) values('{0}','{1}','{2}',now(),now())";
                strSql = String.Format(strSql, FBussId, FBussName, FBussEmail);

                if (da.ExecSqlNum(strSql) == 1)
                {
                    return true;
                }
                else {
                    msg = "添加失败";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        public bool changeComplainBuss(out string msg)
        {
            msg = "";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ht"));

            try
            {
                da.OpenConn();
                string strSql = "select " + GetComplainBussListFields() + " from c2c_fmdb.t_complain_buss_list where Fbuss_id='" + FBussId+"'";
                DataTable dt = da.GetTable(strSql);
                if (dt != null && dt.Rows.Count < 0)
                {
                    msg = "修改失败:商户号码不存在" + FBussId;
                    return false;
                }

                strSql = "update c2c_fmdb.t_complain_buss_list set Fbuss_name='{0}',Fbuss_email='{1}',Fmodify_time=now() where Fbuss_id='{2}' ";
                strSql = String.Format(strSql, FBussName, FBussEmail, FBussId);

                if (da.ExecSqlNum(strSql) == 1)
                {
                    return true;
                }
                else
                {
                    msg = "修改失败";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        public static string GetComplainBussListFields()
        {
            return " Fbuss_id,Fbuss_name,Fbuss_email,Fcreate_time,Fmodify_time ";
        }
    }
    #endregion

    #region 用户投诉类
    public class UserComplainClass : Query_BaseForNET
    {
        public UserComplainClass()
        {

        }
        public UserComplainClass(string bussid, string cft_orderid,int comptype, int compstatus, DateTime u_BeginTime, DateTime u_EndTime)
        {
            string strGroup = "  select " + GetComplainUserListFields() + " from  c2c_fmdb.t_complain_user_list where 1=1 ";
            string strWhere = "";
            if (bussid != null && bussid.Trim() != "")
            {
                strWhere += " and Fbuss_id='" + bussid + "'";
            }
            if (cft_orderid != null && cft_orderid.Trim() != "")
            {
                strWhere += " and Forder_id='" + cft_orderid + "'";
            }
            if (comptype != null && comptype != 0) 
            {
                strWhere += " and Fcomp_type=" + comptype;
            }
            if (compstatus != null && compstatus != 0)
            {
                strWhere += " and Fstatus=" + compstatus;
            }
            if (u_BeginTime != null && u_EndTime != null)
            {
                strWhere += " and (Fcreate_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                    + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' )";
            }

            fstrSql = strGroup + strWhere;
            fstrSql_count = "select count(1) from c2c_fmdb.t_complain_user_list where 1=1 " +strWhere;
        }

        public UserComplainClass(int listid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ht"));

            try
            {
                da.OpenConn();
                string strSql = "  select " + GetComplainUserListFields() + " from  c2c_fmdb.t_complain_user_list where Flistid=" + listid;
                DataTable dt = da.GetTable(strSql);
                if (dt == null || dt.Rows.Count != 1)
                {
                    throw new LogicException("读取数据有误！");
                }
                FListId = listid;
                object obj = dt.Rows[0]["Fbuss_id"];
                if (obj != null) FBussId = obj.ToString().Trim();
                obj = dt.Rows[0]["Fbuss_name"];
                if (obj != null) FBussName = obj.ToString().Trim();
                obj = dt.Rows[0]["Forder_id"];
                if (obj != null) FCftOrderId = obj.ToString().Trim();
                obj = dt.Rows[0]["Forder_fee"];
                if (obj != null) FOrderFee = obj.ToString().Trim();
                obj = dt.Rows[0]["Fcomp_type"];
                if (obj != null) FCompType = Convert.ToInt16(obj.ToString().Trim());
                obj = dt.Rows[0]["Fstatus"];
                if (obj != null) FStatus = Convert.ToInt16(obj.ToString().Trim());
                obj = dt.Rows[0]["Fcontact"];
                if (obj != null) FContact = obj.ToString().Trim();
                obj = dt.Rows[0]["Freply_type"];
                if (obj != null) FReplyType = Convert.ToInt16(obj.ToString().Trim());
                obj = dt.Rows[0]["Fnotice_time"];
                if (obj != null) FNoticeTime = obj.ToString().Trim();
                obj = dt.Rows[0]["Fremind_time"];
                if (obj != null) FRemindTime = obj.ToString().Trim();
                obj = dt.Rows[0]["Fcreate_time"];
                if (obj != null) FCreateTime = obj.ToString().Trim();
                obj = dt.Rows[0]["Fmodify_time"];
                if (obj != null) FModifyTime = obj.ToString().Trim();
                obj = dt.Rows[0]["Fbuss_order_id"];
                if (obj != null) FBussOrderId = obj.ToString().Trim();
                obj = dt.Rows[0]["Fmemo"];
                if (obj != null) FMemo = obj.ToString().Trim();
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }


        public int FListId = 0;
        public string FBussId = "";
        public string FBussName = "";
        public string FCftOrderId = "";
        public string FOrderFee = "";
        public int FCompType = 1;
        public int FStatus = 1;
        public int FReplyType = 1;
        public string FContact = "";
        public string FNoticeTime = "";
        public string FRemindTime = "";
        public string FCreateTime = "";
        public string FModifyTime = "";
        //add by yinhuang 2013/8/9
        public string FBussOrderId = "";
        public string FMemo = "";

        public string addUserComplain(out string msg)
        {
            msg = "";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ht"));

            try
            {
                da.OpenConn();
                string strSql = "select " + GetComplainUserListFields() + " from c2c_fmdb.t_complain_user_list where Forder_id='" + FCftOrderId + "'";
                DataTable dt = da.GetTable(strSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    msg = "添加失败:财付通订单号已存在" + FCftOrderId;
                    return "";
                }
                strSql = "select Fbuss_name from c2c_fmdb.t_complain_buss_list where Fbuss_id='" + FBussId + "'";
                dt = da.GetTable(strSql);
                if (dt == null || dt.Rows.Count != 1)
                {
                    msg = "添加失败:商户号码不存在" + FBussId;
                    return "";
                }
                FBussName = dt.Rows[0]["Fbuss_name"].ToString().Trim();

                string striceWhere = "listid=" + FCftOrderId;
                string QUERY_ORDER = "query_order_service";
                DataTable dt_ice = CommQuery.GetTableFromICE(striceWhere, QUERY_ORDER, out msg);
                if (dt_ice != null && dt_ice.Rows.Count > 0)
                {
                    object obj = dt_ice.Rows[0]["Fpaynum"];
                    if (obj != null) FOrderFee = obj.ToString().Trim();
                }
                else 
                {
                    return "";
                }

                strSql = "insert into c2c_fmdb.t_complain_user_list(Fbuss_id,Fbuss_name,Forder_id,Forder_fee,Fcomp_type,Fstatus,Fcontact,Freply_type,Fbuss_order_id,Fmemo,Fnotice_time,Fcreate_time,Fmodify_time) values('{0}','{1}','{2}',{3},{4},{5},'{6}',{7},'{8}','{9}',now(),now(),now())";
                strSql = String.Format(strSql, FBussId, FBussName, FCftOrderId, FOrderFee, FCompType, FStatus, FContact, FReplyType, FBussOrderId, FMemo);

                if (da.ExecSqlNum(strSql) == 1)
                {
                    strSql = "select Flistid from c2c_fmdb.t_complain_user_list where Forder_id='" + FCftOrderId + "'";
                    dt = da.GetTable(strSql);
                    if (dt != null && dt.Rows.Count > 0) {
                        return dt.Rows[0]["Flistid"].ToString().Trim();
                    }
                    return "";
                }
                else
                {
                    msg = "添加失败";
                    return "";
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return "";
            }
            finally
            {
                da.Dispose();
            }
        }

        public bool changeUserComplain(out string msg)
        {
            msg = "";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ht"));

            try
            {
                da.OpenConn();
                string strSql = "select " + GetComplainUserListFields() + " from c2c_fmdb.t_complain_user_list where Flistid="+FListId;
                DataTable dt = da.GetTable(strSql);
                if (dt != null && dt.Rows.Count < 0)
                {
                    msg = "修改失败:ID不存在" + FListId;
                    return false;
                }
                strSql = "select Fbuss_name from c2c_fmdb.t_complain_buss_list where Fbuss_id='" + FBussId + "'";
                dt = da.GetTable(strSql);
                if (dt == null || dt.Rows.Count != 1)
                {
                    msg = "修改失败:商户号码不存在" + FBussId;
                    return false;
                }
                FBussName = dt.Rows[0]["Fbuss_name"].ToString().Trim();

                string striceWhere = "listid=" + FCftOrderId;
                string QUERY_ORDER = "query_order_service";
                DataTable dt_ice = CommQuery.GetTableFromICE(striceWhere, QUERY_ORDER, out msg);
                if (dt_ice != null && dt_ice.Rows.Count > 0)
                {
                    object obj = dt_ice.Rows[0]["Fpaynum"];
                    if (obj != null) FOrderFee = obj.ToString().Trim();
                }
                else
                {
                    return false;
                }

                strSql = "update c2c_fmdb.t_complain_user_list set Fbuss_id='{0}',Fbuss_name='{1}',Forder_id='{2}',Forder_fee='{3}',Fcomp_type={4},Fstatus={5},Fcontact='{6}',Freply_type={7},Fbuss_order_id='{8}',Fmemo='{9}',Fmodify_time=now() where Flistid='{10}' ";
                strSql = String.Format(strSql, FBussId, FBussName, FCftOrderId, FOrderFee, FCompType, FStatus, FContact, FReplyType,FBussOrderId, FMemo, FListId);

                if (da.ExecSqlNum(strSql) == 1)
                {
                    return true;
                }
                else
                {
                    msg = "修改失败";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        public bool remindUserComplain(out string msg)
        {
            msg = "";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ht"));

            try
            {
                da.OpenConn();
                string strSql = "select " + GetComplainUserListFields() + " from c2c_fmdb.t_complain_user_list where Flistid=" + FListId;
                DataTable dt = da.GetTable(strSql);
                if (dt != null && dt.Rows.Count < 0)
                {
                    msg = "催办失败:ID不存在" + FListId;
                    return false;
                }

                strSql = "update c2c_fmdb.t_complain_user_list set Fstatus=2, Fremind_time=now() where Flistid='{0}' ";
                strSql = String.Format(strSql, FListId);

                if (da.ExecSqlNum(strSql) == 1)
                {
                    return true;
                }
                else
                {
                    msg = "催办失败";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        public static string GetComplainUserListFields()
        {
            return " Flistid,Fbuss_id,Fbuss_name,Forder_id,Forder_fee,Fcomp_type,Fstatus,Fcontact,Freply_type,Fbuss_order_id,Fmemo,Fnotice_time,Fremind_time,Fcreate_time,Fmodify_time ";
        }
    }
    #endregion

    #region 退款登记类
    public class RefundInfoClass : Query_BaseForNET 
    {
        public RefundInfoClass() { }
        public RefundInfoClass(string coding, string orderId, string stime, string etime, int refundType, int refundState, string tradeState) 
        {
            string strGroup = "  select " + GeRefundInfoFields() + " from  c2c_fmdb.t_refund_info where 1=1 ";
            string strWhere = "";
            if (!string.IsNullOrEmpty(coding)) 
            {
                strWhere += " and Fcoding='" + coding + "'";
            }
            if (!string.IsNullOrEmpty(orderId))
            {
                strWhere += " and Forder_id='" + orderId + "'";
            }
            if (!string.IsNullOrEmpty(stime))
            {
                strWhere += " and Fcreate_time>='" + stime + "'";
            }
            if (!string.IsNullOrEmpty(etime))
            {
                strWhere += " and Fcreate_time<='" + etime + "'";
            }
            if (refundType != null && refundType != 0)
            {
                if (refundType == 10)
                {
                    strWhere += " and Frefund_type IN(1,2,3,4,5,10) ";
                }
                else {
                    strWhere += " and Frefund_type=" + refundType;
                }
                
            }
            if (refundState != null && refundState != 0)
            {
                //strWhere += " and Frefund_state=" + refundState;
                strWhere += " and Fsubmit_refund=" + refundState;
            }
            if (!string.IsNullOrEmpty(tradeState) && tradeState != "0") 
            {
                strWhere += " and Ftrade_state='" + tradeState + "'";
            }

            fstrSql = strGroup + strWhere;
            fstrSql_count = "select count(1) from c2c_fmdb.t_refund_info where 1=1 " + strWhere;
        }

        public RefundInfoClass(string fid) 
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ht"));

            try
            {
                da.OpenConn();
                string strSql = "  select " + GeRefundInfoFields() + " from  c2c_fmdb.t_refund_info where Fid=" + fid;
                DataTable dt = da.GetTable(strSql);
                if (dt == null || dt.Rows.Count != 1)
                {
                    throw new LogicException("数据不存在！");
                }
                FId = int.Parse(fid);
                object obj = dt.Rows[0]["Forder_id"];
                if (obj != null) FOrderId = obj.ToString().Trim();
                obj = dt.Rows[0]["Fcoding"];
                if (obj != null) FCoding = obj.ToString().Trim();
                obj = dt.Rows[0]["Famount"];
                if (obj != null) FAmount = obj.ToString().Trim();
                obj = dt.Rows[0]["Ftrade_state"];
                if (obj != null) FTrade_state = Convert.ToInt16(obj.ToString().Trim());
                obj = dt.Rows[0]["Fbuy_acc"];
                if (obj != null) FBuy_acc = obj.ToString().Trim();
                obj = dt.Rows[0]["Ftrade_desc"];
                if (obj != null) FTrade_desc = obj.ToString().Trim();
                obj = dt.Rows[0]["Frefund_type"];
                if (obj != null) FRefund_type =  Convert.ToInt16(obj.ToString().Trim());
                obj = dt.Rows[0]["Frefund_state"];
                if (obj != null) FRefund_state = Convert.ToInt16(obj.ToString().Trim());
                obj = dt.Rows[0]["Fcreate_time"];
                if (obj != null) FCreateTime = obj.ToString().Trim();
                obj = dt.Rows[0]["Fmodify_time"];
                if (obj != null) FModifyTime = obj.ToString().Trim();
                obj = dt.Rows[0]["Fmemo"];
                if (obj != null) FMemo = obj.ToString().Trim();
                obj = dt.Rows[0]["Fsubmit_user"];
                if (obj != null) FSubmit_user = obj.ToString().Trim();
                obj = dt.Rows[0]["Frecycle_user"];
                if (obj != null) FRecycle_user = obj.ToString().Trim();
                obj = dt.Rows[0]["Fsam_no"];
                if (obj != null) FSam_no = obj.ToString().Trim();
                obj = dt.Rows[0]["Fsubmit_refund"];
                if (obj != null) FSubmit_refund = Convert.ToInt16(obj.ToString().Trim());
                obj = dt.Rows[0]["Frefund_amount"];
                if (obj != null) FRefund_amount = obj.ToString().Trim();
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }

        public int FId = 0;
        public string FOrderId = ""; //财付通订单号
        public string FCoding = "";
        public string FAmount = ""; //交易金额
        public int FTrade_state = 2;
        public string FBuy_acc = "";
        public string FTrade_desc = "";//交易说明
        public int FRefund_type = 1; //退款类型
        public int FRefund_state = 10; //退款状态
        public string FCreateTime = "";
        public string FModifyTime = "";
        public string FMemo = "";//备注
        public string FSubmit_user = "";//登记人
        public string FRecycle_user = "";//回收人
        public string FSam_no = "";//SAM工单号
        public int FSubmit_refund = 3; //提交退款状态
        public string FRefund_amount="0"; //退款金额

        public void addRefundInfo(out string msg) 
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ht"));

            try
            {
                da.OpenConn();
                string strSql = "select " + GeRefundInfoFields() + " from c2c_fmdb.t_refund_info where Forder_id='" + FOrderId + "'";
                DataTable dt = da.GetTable(strSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    //msg = "添加失败:财付通订单号已存在" + FOrderId;
                    throw new LogicException("财付通订单号已存在:" + FOrderId);
                }

                string striceWhere = "listid=" + FOrderId;
                DataTable dt_ice = CommQuery.GetTableFromICE(striceWhere, "query_order_service", out msg);
                if (dt_ice != null && dt_ice.Rows.Count > 0)
                {
                    object obj = dt_ice.Rows[0]["Fcoding"];
                    if (obj != null) FCoding = obj.ToString().Trim();
                    obj = dt_ice.Rows[0]["Fpaynum"];
                    if (obj != null) FAmount = obj.ToString().Trim();
                    obj = dt_ice.Rows[0]["Ftrade_state"];
                    if (obj != null) { 
                        FTrade_state = int.Parse(obj.ToString().Trim());
                        if (FTrade_state == 2) {
                            FSubmit_refund = 2;
                        }
                    }
                    obj = dt_ice.Rows[0]["Fbuyid"];
                    if (obj != null) FBuy_acc = obj.ToString().Trim();
                    obj = dt_ice.Rows[0]["Fmemo"];
                    if (obj != null) FTrade_desc = obj.ToString().Trim();
                }
                else
                {
                    //msg = "通用查询订单号不存在：" + FOrderId;
                    throw new LogicException("订单号不存在：" + FOrderId + msg);
                }
                //判断退款金额<=订单金额
                int oAmount = Convert.ToInt32(FAmount);
                int rAmount = 0;
                if (!string.IsNullOrEmpty(FRefund_amount)) 
                {
                    rAmount = Convert.ToInt32(FRefund_amount);
                }
                if (rAmount <= oAmount)
                {
                    //退款金额<=订单金额
                    strSql = "insert into c2c_fmdb.t_refund_info(Forder_id,Fcoding,Famount,Ftrade_state,Fbuy_acc,Ftrade_desc,Frefund_type,Frefund_state,Fmemo,Fsubmit_user,Frecycle_user,Fsam_no,Fsubmit_refund,Frefund_amount,Fcreate_time,Fmodify_time) values('{0}','{1}','{2}',{3},'{4}','{5}',{6},{7},'{8}','{9}','{10}','{11}',{12},{13},now(),now())";
                    strSql = String.Format(strSql, FOrderId, FCoding, FAmount, FTrade_state, FBuy_acc, FTrade_desc, FRefund_type, FRefund_state, FMemo, FSubmit_user, FRecycle_user, FSam_no, FSubmit_refund, FRefund_amount);

                    da.ExecSqlNum(strSql);
                }
                else 
                {
                    throw new LogicException("退款金额" + rAmount + "大于订单金额" + oAmount);
                }
            }
            catch (LogicException err) 
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }

        public void changeRefundInfo(out string msg) 
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ht"));

            try
            {
                da.OpenConn();
                string strSql = "select " + GeRefundInfoFields() + " from c2c_fmdb.t_refund_info where Fid="+FId+" AND Forder_id<>'" + FOrderId + "'";
                DataTable dt = da.GetTable(strSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    throw new LogicException("修改失败:财付通订单号已存在" + FOrderId);
                }

                string striceWhere = "listid=" + FOrderId;
                string QUERY_ORDER = "query_order_service";
                DataTable dt_ice = CommQuery.GetTableFromICE(striceWhere, QUERY_ORDER, out msg);
                if (dt_ice != null && dt_ice.Rows.Count > 0)
                {
                    object obj = dt_ice.Rows[0]["Fcoding"];
                    if (obj != null) FCoding = obj.ToString().Trim();
                    obj = dt_ice.Rows[0]["Fpaynum"];
                    if (obj != null) FAmount = obj.ToString().Trim();
                    obj = dt_ice.Rows[0]["Ftrade_state"];
                    if (obj != null) FTrade_state = int.Parse(obj.ToString().Trim());
                    obj = dt_ice.Rows[0]["Fbuyid"];
                    if (obj != null) FBuy_acc = obj.ToString().Trim();
                    obj = dt_ice.Rows[0]["Fmemo"];
                    if (obj != null) FTrade_desc = obj.ToString().Trim();
                    obj = dt_ice.Rows[0]["Ftrade_state"];
                    if (obj != null)
                    {
                        FTrade_state = int.Parse(obj.ToString().Trim());
                        if (FTrade_state == 2)
                        {
                            FSubmit_refund = 2;
                        }
                    }
                }
                else
                {
                    throw new LogicException("通用查询订单号不存在：" + FOrderId);
                }

                //判断退款金额<=订单金额
                int oAmount = Convert.ToInt32(FAmount);
                int rAmount = 0;
                if (!string.IsNullOrEmpty(FRefund_amount))
                {
                    rAmount = Convert.ToInt32(FRefund_amount);
                }
                if (rAmount <= oAmount)
                {
                    //退款金额<=订单金额
                    strSql = "update c2c_fmdb.t_refund_info set Forder_id='{0}',Fcoding='{1}',Famount='{2}',Ftrade_state={3},Fbuy_acc='{4}',Ftrade_desc='{5}',Frefund_type={6},Frefund_state={7},Fmemo='{8}',Fsubmit_user='{9}',Frecycle_user='{10}',Fsam_no='{11}',Fsubmit_refund={12},Frefund_amount={13},Fmodify_time=now() where Fid={14} ";
                    strSql = String.Format(strSql, FOrderId, FCoding, FAmount, FTrade_state, FBuy_acc, FTrade_desc, FRefund_type, FRefund_state, FMemo, FSubmit_user, FRecycle_user, FSam_no, FSubmit_refund, FRefund_amount, FId);

                    da.ExecSqlNum(strSql);
                }
                else
                {
                    throw new LogicException("退款金额" + rAmount + "大于订单金额" + oAmount);
                }
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }
        
        public static string GeRefundInfoFields()
        {
            return " Fid,Forder_id,Fcoding,Famount,Ftrade_state,Fbuy_acc,Ftrade_desc,Frefund_type,Frefund_state,Fmemo,Fcreate_time,Fmodify_time,Fsubmit_user,Frecycle_user,Fsam_no,Fsubmit_refund,Frefund_amount ";
        }
    }
    #endregion

    #region 外汇汇率查询类
    public class ExchangeRateQueryClass : Query_BaseForNET 
    {
        public ExchangeRateQueryClass() 
        {
        }

        public ExchangeRateQueryClass(string foreType, string issueBank, string beginTime, string endTime) 
        {
            string strGroup = "  select Fcurrency_type,Fexchg_flag,Feffect_flag,Frate_time,Fbank_type,Fcurrency_sell,Fcurrency_sell_tuned,Fmodify_time from  fcpay_db.t_rate_log where 1=1 ";
            string strWhere = "";
            if (foreType != null && foreType.Trim() != "" && foreType != "0")
            {
                strWhere += " and Fcurrency_type='" + foreType + "'";
            }
            if (issueBank != null && issueBank.Trim() != "" && issueBank != "0")
            {
                strWhere += " and Fbank_type='" + issueBank + "'";
            }
            if (beginTime != null && endTime != null)
            {
                strWhere += " and (Frate_time between '" + beginTime
                    + "' and '" + endTime + "' )";
            }

            fstrSql = strGroup + strWhere;
            fstrSql_count = "select count(1) from fcpay_db.t_rate_log where 1=1 " + strWhere;
        }
    }

    #endregion

    #region 快速交易查询类

    public class QuickTradeQueryClass : Query_BaseForNET
    {
        //暂时改造成商户流水查询中的了。 furion 20050817
        public QuickTradeQueryClass(string listid)
        {
            string tmpspid = listid.Substring(0, 10);

            /*
            string strSql = "select Fspecial,FName from c2c_db.t_merchant_info where Fspid='" + tmpspid + "'";
            DataTable dt = PublicRes.returnDataTable(strSql,"ZL");
            */

            string errMsg = "";
            string strSql = "spid=" + tmpspid;
            DataTable dt = CommQuery.GetTableFromICE(strSql, CommQuery.QUERY_MERCHANTINFO, out errMsg);

            string FSaleID = "";
            string FSale_Name = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                /*
                FSaleID = dt.Rows[0][0].ToString();
                FSale_Name = dt.Rows[0][1].ToString();
                */

                FSaleID = dt.Rows[0]["Fspecial"].ToString();
                FSale_Name = dt.Rows[0]["Fname"].ToString();
            }


            //再调整一下字段，让外面网页可以判断退款的按钮是否可用就行了。furion 20050817
            /*
            //string tablename1 = "c2c_db.t_tcbankroll_list B ";
            //string tablename2 = PublicRes.GetTName("t_tran_list",listid) + " A ";
            string tablename2 = PublicRes.GetTName("t_order",listid) + " A ";
            //string tablename3 = "c2c_db.t_middle_user C ";

            //string strWhere = " where B.FListID=A.FListID and A.FListID='" + listid + "' and  A.Ftrade_type=3 and A.FSpID=C.FSpID";
            //string strWhere = " where  A.FListID='" + listid + "' and A.FSpID=C.FSpID";
            string strWhere = " where  A.FListID='" + listid + "' ";

            //fstrSql = "select A.FCoding,A.Flistid,A.Fbank_listid,C.Fqqid as FSaleID,C.Ftruename as FSale_Name,A.Fstate as FState1,A.Flstate,A.Fcreate_time_c2c,A.Fmemo,A.FTrade_Type " 
            fstrSql = "select A.FCoding,A.Flistid,A.Fbank_listid,'" + FSaleID + "' as FSaleID," + FSale_Name + " as FSale_Name,A.FTrade_state as FState1,A.Flstate,A.Fcreate_time_c2c,A.Fmemo,A.FTrade_Type " 
                + ",A.FPayNum as Fnum,A.Fbuyid,A.Fbuy_name,A.FSpid,A.Fbuy_uid  from " 
                //+ tablename2 + "," + tablename3 + strWhere;
                + tablename2  + strWhere;
            //fstrSql_count = "select count(*) from " + tablename2 + "," + tablename3 + strWhere;
            */

            strSql = "listid=" + listid;
            DataTable dtlst = CommQuery.GetTableFromICE(strSql, CommQuery.QUERY_ORDER, out errMsg);

            DataRow dr = dtlst.Rows[0];

            fstrSql = "select '" + QueryInfo.GetString(dr["Fcoding"]) + "' as FCoding,'"
                + listid + "' as Flistid,'"
                + QueryInfo.GetString(dr["Fbank_listid"]) + "' as Fbank_listid,'"
                + FSaleID + "' as FSaleID," + FSale_Name + " as FSale_Name,"
                + dr["FTrade_state"].ToString() + " as FState1,"
                + dr["Flstate"].ToString() + " as Flstate,'"
                + QueryInfo.GetString(dr["Fcreate_time_c2c"]) + "' as Fcreate_time_c2c,'"
                + QueryInfo.GetString(dr["Fmemo"]) + "' as Fmemo,"
                + dr["Ftrade_type"].ToString() + " as FTrade_Type,"
                + dr["fpaynum"].ToString() + " as Fnum,'"
                + dr["fbuyid"].ToString() + "' as Fbuyid,'"
                + QueryInfo.GetString(dr["Fbuy_name"]) + "' as Fbuy_name,'"
                + dr["Fspid"].ToString() + "' as FSpid,"
                + dr["Fbuy_uid"].ToString() + " as Fbuy_uid";

            fstrSql_count = "select 1";
        }

        //对合适的记录进行退款处理。
        public static bool Refund(string listid, Finance_Header fh)
        {
            //furion 20061201 不允许退单
            return false;

            //			if(listid == null || listid.Trim() == "") throw new LogicException("交易单ID不能为空！");
            //
            //			
            //
            //			MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("YW"));
            //			try
            //			{
            //				da.OpenConn();
            //				da.StartTran(); //新加入的以事务执行所有的函数，待测试，20050818 furion
            //
            //				string strSql = "select * from " + PublicRes.GetTName("t_tran_list",listid) + " where FlistID='" + listid + "' for update";
            //				DataSet ds = da.dsGetTotalData(strSql);
            //
            //				if(ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count != 1)
            //				{
            //					throw new LogicException("查找不到指定的交易单信息！");
            //				}
            //
            //				DataRow dr = ds.Tables[0].Rows[0];
            //
            //				if(dr["FTrade_Type"] == null || dr["FTrade_Type"].ToString() != "2")
            //				{
            //					//只退B2C的款。
            //					throw new LogicException("指定的交易单不能退款！");
            //				}
            //
            //				if(dr["Flstate"] == null || dr["Flstate"].ToString() != "2")
            //				{
            //					throw new LogicException("指定的交易单处于锁定或作废状态！");
            //				}
            //
            //				if(dr["Fstate"] == null || dr["Fstate"].ToString() != "2")
            //				{
            //					throw new LogicException("指定的交易单状态不对，不能退款！");
            //				}
            //
            //				int paynum = Int32.Parse(dr["fpaynum"].ToString());
            //				string saleid = dr["Fsale_uid"].ToString();
            //				
            //
            //				strSql = "select * from c2c_db.t_middle_user where Fuid='" + saleid + "' for update";
            //				DataSet ds1 = da.dsGetTotalData(strSql);
            //
            //				if(ds1 == null || ds1.Tables.Count == 0 || ds1.Tables[0].Rows.Count != 1)
            //				{
            //					throw new LogicException("查找不到指定的用户信息！");
            //				}
            //
            //				DataRow dr1 = ds1.Tables[0].Rows[0];
            //
            //				if(dr1["Fstate"] == null || dr1["Fstate"].ToString() == "2")
            //				{
            //					throw new LogicException("用户帐户被冻结了！");
            //				}
            //
            //				int num = Int32.Parse(dr1["Fbalance"].ToString()) - Int32.Parse(dr1["Fcon"].ToString());
            //
            //				if(num < paynum)
            //				{
            //					throw new LogicException("用户可用余额不足，无法退款！");
            //				}
            //
            //				strSql = "select * from " + PublicRes.GetTName("t_user",dr["fbuy_uid"].ToString()) + " where Fuid='" + dr["fbuy_uid"].ToString()+ "' for update";
            //				DataSet ds2 = da.dsGetTotalData(strSql);
            //
            //				if(ds2 == null || ds2.Tables.Count == 0 || ds2.Tables[0].Rows.Count != 1)
            //				{
            //					throw new LogicException("查找不到指定的用户信息！");
            //				}
            //
            //				DataRow dr2 = ds2.Tables[0].Rows[0];
            //				//开始退款。
            //				//模仿C2C的退款流程。
            //				//修改交易单状态为转入退款。
            //				//生成退款单，状态：等待支付通打款。
            //				//商户帐户流水表出。
            //				//买家帐户流水表入。
            //				//更新双方余额。
            //				//记录交易流水表 商户到买家。
            //
            //			
            //				//修改交易单状态为转入退款
            //				strSql = "update " + PublicRes.GetTName("t_tran_list",listid) + " set Fstate=7 where flistid='" + listid + "'";				
            //				if(!da.ExecSql(strSql)) throw new LogicException("修改交易单时出错！");
            //
            //				strSql = "update " + PublicRes.GetTName("t_pay_list",dr["fsale_uid"].ToString()) + " set Fstate=7 where flistid='" + listid + "'";				
            //				if(!da.ExecSql(strSql)) throw new LogicException("修改交易单时出错！");
            //
            //				strSql = "update " + PublicRes.GetTName("t_pay_list",dr["fbuy_uid"].ToString()) + " set Fstate=7 where flistid='" + listid + "'";				
            //				if(!da.ExecSql(strSql)) throw new LogicException("修改交易单时出错！");
            //
            //				int paysale = 0;
            //
            //				//生成退款单 dr交易单，dr1买家 dr2卖家
            //				strSql = "Insert c2c_db.t_refund_list(Flistid,Fspid,Fbuy_uid,Fbuyid,Fsale_uid,Fsaleid,Fstate,Flstate,Fpaybuy,Fpaysale,Fbargain_time,Fcreate_time,Fok_time,Fok_time_acc,Fip,Fmodify_time,Ftrade_type,Fbuy_name,Fsale_name)"
            //					+ " values('{0}','{1}','{2}','{3}','{4}','{5}',2,2,{6},{7},now(),now(),now(),now(),'{8}',now(),2,'{9}','{10}')";
            //				strSql = String.Format(strSql,listid,dr["fspid"],dr["fbuy_uid"],dr["fbuyid"],dr["fsale_uid"],dr["fsaleid"],paynum,paysale,fh.UserIP,dr["FBuy_Name"],dr["FSale_Name"]);
            //				if(!da.ExecSql(strSql)) throw new LogicException("生成退款单时出错！");
            //
            //				
            //				//商户帐户流水出。
            //				string strdate = DateTime.Now.ToString("yyyyMM");
            //				string tablename = "c2c_db_medi_user."  + "t_bankroll_list_" + strdate; 
            //				int newbalance = Int32.Parse(dr1["Fbalance"].ToString()) - paynum;
            //
            //				strSql = "insert " + tablename + "(Flistid,Fspid,Fuid,Fqqid,Ftype,Fsubject,Ffrom_uid,Ffromid,fbalance,Fpaynum,Fip,Faction_type,Fmodify_time_acc,Fmodify_time,Fvs_qqid,Flist_sign,FCurType,Ftrue_name,Ffrom_name,Fexplain)"
            //					+ " values('{0}','{1}','{2}','{3}',2,5,'{4}','{5}',{6},{7},'{8}',16,now(),now(),'{9}',0,{10},'{11}','{12}','{13}')";
            //				strSql = String.Format(strSql,listid,dr["fspid"],dr["fsale_uid"],dr["fsaleid"],dr["fbuy_uid"],dr["fbuyid"],newbalance,paynum,fh.UserIP,dr["fbuyid"],dr["FCurType"],dr["FBuy_Name"],dr["FSale_Name"],dr["Fexplain"]);
            //				if(!da.ExecSql(strSql)) throw new LogicException("增加商户帐户流水时出错！");
            //
            //				//买家帐户流水入。
            //				tablename = PublicRes.GetTName("t_bankroll_list",dr["fbuy_uid"].ToString()); 
            //				int newbalance1 = Int32.Parse(dr2["Fbalance"].ToString()) + paynum;
            //
            //				strSql = "insert " + tablename + "(Flistid,Fspid,Fuid,Fqqid,Ftype,Fsubject,Ffrom_uid,Ffromid,fbalance,Fpaynum,Fip,Faction_type,Fmodify_time_acc,Fmodify_time,Fvs_qqid,Flist_sign,FCurType,Ftrue_name,Ffrom_name,Fexplain)"
            //					+ " values('{0}','{1}','{2}','{3}',1,5,'{4}','{5}',{6},{7},'{8}',16,now(),now(),'{9}',0,{10},'{11}','{12}','{13}')";
            //				strSql = String.Format(strSql,listid,dr["fspid"],dr["fbuy_uid"],dr["fbuyid"],dr["fsale_uid"],dr["fsaleid"],newbalance1,paynum,fh.UserIP,dr["fsaleid"],dr["FCurType"],dr["FBuy_Name"],dr["FSale_Name"],dr["Fexplain"]);
            //				if(!da.ExecSql(strSql)) throw new LogicException("增加买家帐户流水时出错！");
            //
            //				//记录交易流水表。
            //				tablename = PublicRes.GetTName("t_userpay_list",dr["fsale_uid"].ToString()); //furion 20050829 交易流水要记录到商家了。
            //				strSql = "insert " + tablename + "(Flistid,Fspid,Fuid,Fqqid,Ftype,Fsubject,Ffrom_uid,Ffromid,Fbalance,Ffrom_balance,Fpaynum,Fcreate_time,Fip,Fmodify_time,Flist_sign,Fold_state,Fnew_state,FCurType,Ftrue_name,Ffrom_name,Fcoding)"
            //					+ " values('{0}','{1}','{2}','{3}',2,5,'{4}','{5}',{6},{7},{8},now(),'{9}',now(),0,2,7,{10},'{11}','{12}','{13}')";
            //				strSql = String.Format(strSql,listid,dr["fspid"],dr["fsale_uid"],dr["fsaleid"],dr["fbuy_uid"],dr["fbuyid"],newbalance,newbalance1,paynum,fh.UserIP,dr["FCurType"],dr["FSale_name"],dr["FBuy_Name"],dr["FCoding"]);
            //				if(!da.ExecSql(strSql)) throw new LogicException("增加交易流水时出错！");
            //
            //				//更新双方余额。
            //				strSql = "update " + PublicRes.GetTName("t_user",dr["fbuy_uid"].ToString()) + " set Fbalance=Fbalance+" + paynum.ToString() 
            //					+ " where Fuid='" + dr["fbuy_uid"].ToString() + "'";
            //				if(!da.ExecSql(strSql)) throw new LogicException("更新买家余额时出错！");
            //
            //				strSql = "update c2c_db.t_middle_user set Fbalance=Fbalance-" + paynum.ToString() 
            //					+ " where Fuid='" + dr["fsale_uid"].ToString() + "'";
            //				if(!da.ExecSql(strSql)) throw new LogicException("更新卖家余额时出错！");			
            //				
            //				//记录交易日志表
            //				strSql = "select now()";
            //				string tmp = da.GetOneResult(strSql);
            //				tmp = DateTime.Parse(tmp).ToString("yyyyMMdd");
            //				tablename = "c2c_db_paylog.t_paylog_" + tmp;
            //				strSql = "Insert " + tablename + "(Flistid,Fspid,Fuid,Fqqid,Fauid,Fvs_qqid,Fcurtype,Fnum,Fpaybuy,Frequest_type,Fcreate_time_c2c,Fmodify_time,Fip,Flist_sign,Ftrade_type)"
            //					+ " values('{0}','{1}','{2}','{3}','{4}','{5}',{6},{7},{8},12,now(),now(),'{9}',0,2)";
            //				strSql = String.Format(strSql,dr["FListID"],dr["FSpid"],dr["Fbuy_uid"],dr["Fbuyid"],dr["Fsale_uid"],dr["FSaleID"],dr["Fcurtype"],dr["Fpaynum"],dr["Fpaynum"],fh.UserIP);
            //				if(!da.ExecSql(strSql)) throw new LogicException("记录交易日志表时出错！");
            //
            //				da.Commit();
            //				da.CloseConn();
            //				return true;
            //			}
            //			catch(LogicException err)
            //			{
            //				da.RollBack();
            //				da.CloseConn();
            //				throw err;
            //				return false;
            //			}
            //			catch(Exception err)
            //			{
            //				da.RollBack();
            //				da.CloseConn();
            //				throw new LogicException("在执行事务时出错了错误！");
            //				return false;
            //			}
            //			finally
            //			{
            //				da.Dispose();
            //			}           

        }
    }

    #endregion




    #region 商户流水查询类 furion 20050817 因为中介改成商户的话，可以从买卖家交易单表中查了。

    //其实查的是商户做为卖家的交易。 furion 20050817

    public class MediListQueryClass : Query_BaseForNET
    {
        public MediListQueryClass(string u_ID, string Fcode, DateTime u_BeginTime, DateTime u_EndTime)
            : this(u_ID, Fcode, u_BeginTime, u_EndTime, "", "Flistid") { }

        public MediListQueryClass(string u_ID, string Fcode, DateTime u_BeginTime, DateTime u_EndTime, string u_UserFilter, string u_OrderBy)
        {
            if (u_ID != null && u_ID.Trim() != "")
            {
                /*
                //furion 20050817 先从商户帐户表中取出UID出来。
                //string strSql = "select fuid from c2c_db.t_middle_user where fqqid='" + u_ID + "'";
                string strSql = "select fuidMiddle from c2c_db.t_merchant_info where fspid='" + u_ID + "'";

                //string fuid = PublicRes.ExecuteOne(strSql,"YW_30");
                //string fuid = PublicRes.ExecuteOne(strSql,"YWB_30");
                string fuid = PublicRes.ExecuteOne(strSql,"ZL");
                //string tablename = "c2c_db_medi_user.t_bankroll_list_" + u_BeginTime.ToString("yyyyMM");
                */

                string strSql = "spid=" + u_ID;
                string errMsg = "";
                string fuid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MERCHANTINFO, "FuidMiddle", out errMsg);

                if (fuid == null || fuid.Trim() == "")
                {
                    throw new LogicException("查找不到指定的商户！" + errMsg);
                }

                //string tablename = PublicRes.GetTName("t_pay_list", fuid);
                string tablename = PublicRes.GetTName("t_user_order", fuid);

                fstrSql = " select *,Ftrade_state as Fstate from  " + tablename;
                fstrSql_count = " select count(*) from " + tablename;


                fstrSql += " where Fsale_uid='" + fuid + "'  ";
                fstrSql_count += " where Fsale_uid='" + fuid + "'  ";


                fstrSql += " and Fcreate_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                    + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                fstrSql_count += " and Fcreate_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                    + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                if (Fcode != "")
                {
                    fstrSql += "and Fcoding = '" + Fcode + "' ";
                    fstrSql_count += "and Fcoding = '" + Fcode + "' ";
                }

                //增加了自定义查询条件 edwardzheng 20051110
                if (u_UserFilter != "")
                {
                    fstrSql += " AND (" + u_UserFilter + ")";
                    fstrSql_count += " AND (" + u_UserFilter + ")";
                }
                //增加了过滤重复清单的处理 edwardzheng 20051110
               // fstrSql += " GROUP BY Flistid";
                fstrSql_count += " GROUP BY Flistid";

                //测试是否由于ORDER BY引起的性能问题 yonghua
               // if (u_OrderBy == "") u_OrderBy = "Flistid";
               // fstrSql += " ORDER BY " + u_OrderBy;

                fstrSql_count = "select 10000";
            }
            else
            {
                throw new LogicException("商户帐号不能为空");
            }
        }



        public MediListQueryClass(string u_ID, string Fcode, string strBeginTime, string strEndTime, string u_UserFilter
            , string u_OrderBy, int limStart, int limCount)
        {
            if (u_ID != null && u_ID.Trim() != "")
            {
                string strSql = "spid=" + u_ID;
                string errMsg = "";
                string fuid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MERCHANTINFO, "FuidMiddle", out errMsg);

                if (fuid == null || fuid.Trim() == "")
                {
                    throw new LogicException("查找不到指定的商户！" + errMsg);
                }

                //string tablename = PublicRes.GetTName("t_pay_list", fuid);
                string tablename = PublicRes.GetTName("t_user_order", fuid);

                fstrSql = " select *,Ftrade_state as Fstate from  " + tablename;
                //fstrSql_count  = " select count(*) from " + tablename;


                fstrSql += " where Fsale_uid='" + fuid + "'  ";
                //fstrSql_count += " where Fsale_uid='" + fuid + "'  ";


                if (strBeginTime.Trim() != "")
                {
                    fstrSql += " and Fcreate_time >='" + strBeginTime + "'";
                    //fstrSql_count += " and Fcreate_time >='" + strBeginTime + "'";
                }

                if (strEndTime.Trim() != "")
                {
                    fstrSql += " and Fcreate_time <='" + strEndTime + "'";
                    //fstrSql_count += " and Fcreate_time <='" + strEndTime + "'";
                }

                /*
                fstrSql += " and Fcreate_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss") 
                    + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                fstrSql_count += " and Fcreate_time between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss") 
                    + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                    */

                if (Fcode != "")
                {
                    fstrSql += "and Fcoding = '" + Fcode + "' ";
                    //fstrSql_count += "and Fcoding = '" + Fcode + "' ";
                }

                //增加了自定义查询条件 edwardzheng 20051110
                if (u_UserFilter != "")
                {
                    fstrSql += " AND (" + u_UserFilter + ")";
                    //fstrSql_count += " AND (" + u_UserFilter + ")";
                }

                //fstrSql_count += " limit " + limStart + "," + limCount;

                //增加了过滤重复清单的处理 edwardzheng 20051110
               // fstrSql += " GROUP BY Flistid";
                //fstrSql_count += " GROUP BY Flistid";

               // if (u_OrderBy == "") u_OrderBy = "Flistid";
               // fstrSql += " ORDER BY " + u_OrderBy;

                fstrSql += " limit " + limStart + "," + limCount;

                fstrSql_count = "select 10000";
            }
            else
            {
                throw new LogicException("商户帐号不能为空");
            }
        }

    }


    #endregion

    #region 冻结查询类。

    public class FreezeQueryClass : Query_BaseForNET
    {
        public FreezeQueryClass(string fid)
        {
            fstrSql = "select * from c2c_fmdb.t_freeze_list  where  Fid='" + fid + "'";
            fstrSql_count = "select count(*) from c2c_fmdb.t_freeze_list where  Fid='" + fid + "'";
        }

        public FreezeQueryClass(string qqid, int handleFinish)
        {
            fstrSql = "select * from c2c_fmdb.t_freeze_list  where  FFreezeID='" + qqid + "' and FHandleFinish=1";
            fstrSql_count = "select count(*) from c2c_fmdb.t_freeze_list where  FFreezeID='" + qqid + "' and FHandleFinish=1";
        }

        public FreezeQueryClass(DateTime u_BeginTime, DateTime u_EndTime, string freezeuser, string username, int handletype, int statetype, string qqid)
        {
            string strGroup = "";
            string strWhere = "";

            strWhere += " where FFreezeTime between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss")
                + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

            if (statetype != 0)
            {
                strWhere += " and FHandleFinish=" + statetype.ToString() + " ";
            }

            if (handletype != 0)
            {
                strWhere += " and FFreezeType=" + handletype.ToString() + " ";
            }

            if (freezeuser != null && freezeuser.Trim() != "")
            {
                strWhere += " and FFreezeuserID='" + freezeuser + "' ";
            }

            if (username != null && username.Trim() != "")
            {
                strWhere += " and FUserName like '%" + username + "%' ";
            }

            if (qqid != null && qqid.Trim() != "")
            {
                strWhere += " and FFreezeID = '" + qqid + "' ";
            }

            string TableName1 = "c2c_fmdb.t_freeze_list";
            strGroup = strGroup + " select FID,FFreezeUserID,FFreezeTime,FFreezeType,FContact,FFreezeReason,FHandleUserID,FHandleResult,"
                + "(case FFreezeType when 1 then '冻结帐户' when 2 then '锁定交易单' else '' end) FFreezeTypeName,FUserName,"
                + "FHandleFinish,(case FHandleFinish when 1 then '处理中' when 9 then '处理完成' else '' end ) FHandleFinishName,FFreezeID from "
                + TableName1 + strWhere + " ";

            fstrSql = strGroup;
            fstrSql_count = " select count(*) from ( " + strGroup + ") a ";

        }

    }


    public class FreezeFinQueryClass : Query_BaseForNET
    {
        // 目前页面提供的冻结金额只有一个限额，所以添加一个上限。
        private const double m_FreezeFinMax = 10000;

        public FreezeFinQueryClass(string strBeginDate, string strEndDate, string fpayAccount, double freezeFin, string flistID, int limStart, int limMax)
        {
            if (flistID == null || flistID.Trim() == "")
            {
                if (fpayAccount == null)
                {
                    throw new Exception("查询账户不能为空！");
                }

                this.ICEcommand = "QUERY_USER_BANKROLL_FULL";

                string strUID = PublicRes.ConvertToFuid(fpayAccount);

                this.ICESql += "uid=" + strUID;
                //测试用的UID
                //this.ICESql += "uid=295191000";
                this.ICESql += "&start_time=" + strBeginDate + "&end_time=" + strEndDate + "&type=3";

                if (freezeFin != 0)
                {
                    this.ICESql += "&con_low=" + (freezeFin * 100) + "&con_high=" + (m_FreezeFinMax * 100);
                }

                this.ICESql += "&lim_start=" + limStart;
                this.ICESql += "&lim_count=" + limMax;
            }
            else
            {
                // 如果有listid查询，则使用QUERY_BANKROLL_LISTID_2，而这个接口只接受2个参数
                if (fpayAccount == null)
                {
                    throw new Exception("查询账户不能为空！");
                }

                this.ICEcommand = "QUERY_BANKROLL_LISTID_2";
                string strUID = PublicRes.ConvertToFuid(fpayAccount);
                this.ICESql += "uid=" + strUID;
                // 测试用的UID
                //this.ICESql += "uid=295191000";
                this.ICESql += "&listid=" + flistID;
            }
        }
    }





    public class FreezeQueryClass_2 : Query_BaseForNET
    {
        public FreezeQueryClass_2(string fid)
        {
            fstrSql = "select * from c2c_fmdb.t_freeze_list  where  Fid='" + fid + "'";
            fstrSql_count = "select count(*) from c2c_fmdb.t_freeze_list where  Fid='" + fid + "'";
        }


        /*
        public FreezeQueryClass_2(string strBeginDate,string strEndDate)
        {
            CFTUserAppealClass cuser = new CFTUserAppealClass("",strBeginDate,strEndDate,99,8,"");
            DataSet ds = cuser.GetResultX(iPageStart,iPageMax,"CFT");		
	
            if(ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                CFTUserAppealClass.HandleParameter_ForControledFreeze(ds,true);
            }

            return ds;
        }
        */

        public FreezeQueryClass_2(string szQQID, string szBeginDate, string szEndDate, string szStatue, string szListID,
            string szFreezeUser, string szFreezeReason)
        {
            string strGroup = "";
            string strWhere = " where (1=1) ";

            if (szBeginDate != null && szBeginDate != "")
            {
                strWhere += " and a.FFreezeTime>= '" + szBeginDate + "' ";
            }

            if (szEndDate != null && szEndDate != "")
            {
                strWhere += " and a.FFreezeTime<= '" + szEndDate + "' ";
            }

            if (szStatue != "99")
            {
                if (szStatue == "0")
                {
                    strWhere += " and b.FField3 is null";
                }
                else if (szStatue == "10")
                {
                    szStatue = "0";

                    strWhere += " and b.fsourceType=" + szStatue + " and b.FField3=''";
                }
                else
                {
                    strWhere += " and b.fsourceType=" + szStatue + " and b.FField3 != '' ";
                }
            }

            if (szFreezeUser != null && szFreezeUser != "")
            {
                strWhere += " and b.FField3 like '%" + szFreezeUser + "%' ";
            }

            if (szQQID != null && szQQID != "")
            {
                strWhere += " and a.FFreezeID = '" + szQQID + "' ";
            }

            if (szListID != null && szListID != "")
            {
                strWhere += " and a.FID= '" + szListID + "'";
            }

            if (szFreezeReason != "")
            {
                strWhere += " and a.FFreezeReason like '%" + szFreezeReason + "%' ";
            }

            // 只将未解冻的结果查出来
            //strWhere += " and a.FHandleFinish=1 and a.FID=b.ffreezeListID group by a.fid order by b.FCreateDate DESC ";
            strWhere += " and a.FHandleFinish=1 group by a.fid order by a.FFreezeTime DESC ";

            /*
                strWhere += " where FFreezeTime between '" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss") 
                    + "' and '" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                if(statetype != 0)
                {
                    strWhere += " and FHandleFinish=" + statetype.ToString() + " ";
                }
 
                if(handletype != 0)
                {
                    strWhere += " and FFreezeType=" + handletype.ToString() + " ";
                }
			
                if(freezeuser !=null && freezeuser.Trim() != "")
                {
                    strWhere += " and FFreezeuserID='" + freezeuser + "' ";
                }

                if(username !=null && username.Trim() != "")
                {
                    strWhere += " and FUserName like '%" + username + "%' ";
                }

                if(qqid != null && qqid.Trim() != "")
                {
                    strWhere += " and FFreezeID = '" + qqid + "' ";
                }
                */

            string TableName = "c2c_fmdb.t_freeze_list a ";
            //string joinSql = " left outer join (select ffreezelistid,ffield3,fsourcetype from c2c_fmdb.t_Freeze_Detail) b on a.fid = b.FFreezeListID ";
            string joinSql = " left outer join c2c_fmdb.t_Freeze_Detail b on a.fid = b.FFreezeListID ";

            /*
                strGroup += " select distinct a.FID,FFreezeUserID,FFreezeTime,FFreezeType,"
                    + "(case FFreezeType when 1 then '冻结帐户' when 2 then '锁定交易单' else '' end) FFreezeTypeName,FUserName,"
                    + "FHandleFinish,(case FHandleFinish when 1 then '处理中' when 9 then '处理完成' else '' end ) FHandleFinishName,FFreezeID,FFreezeReason from " 
                    + TableName + joinSql + strWhere + " ";
                    */

            strGroup += " select * from " + TableName + joinSql + strWhere;

            fstrSql = strGroup;
            fstrSql_count = " select count(*) from ( " + strGroup + ") a ";
        }
    }


    #endregion


    #region	 受控资金信息查询

    public class QeuryUserControledFinInfoClass : Query_BaseForNET
    {
        public QeuryUserControledFinInfoClass(string fuid, string beginDateStr, string endDateStr, string cur_type, int iNumStart, int iNumMax)
        {
            this.ICEcommand = "FINANCE_QUERY_USER_CONTROLED";

            //string fuid = PublicRes.ConvertToFuid(qqid);
            //// fuid = "295191000";测试
            //if (fuid == null || fuid.Trim() == "")
            //    throw new Exception("帐号不存在！");

            this.ICESql = "uid=" + fuid;
            //this.ICESql = "uid=298684980";

            if (beginDateStr != null && beginDateStr.Trim() != "" && endDateStr != null && endDateStr.Trim() != "")
            {
                this.ICESql += "&createTime_begin=" + beginDateStr + "&createTime_end=" + endDateStr;
            }

            if (cur_type != null && cur_type.Trim() != "" )
            {
                this.ICESql += "&cur_type=" + cur_type ;
            }

            this.ICESql += "&strlimit=limit " + iNumStart + "," + iNumMax;
        }
    }


    #endregion

    #region 财付券查询类
    public class GwqQueryClass : Query_BaseForNET
    {
        public GwqQueryClass(string u_ID)
            : this(u_ID, "")
        {
        }

        public GwqQueryClass(string u_ID, int is_tdeID)
            : this(u_ID, is_tdeID, "")
        {
        }

        public GwqQueryClass(string u_ID, string filter)
        {
            if (u_ID != null && u_ID.Trim() != "")
            {
                string tablename = "";
                string fuid = PublicRes.ConvertToFuid(u_ID);
                try
                {
                    //tablename = PublicRes.GetTableName("t_gwq",u_ID);
                    tablename = PublicRes.GetTName("t_gwq", fuid);
                }
                catch
                {
                    throw new LogicException("号码无效或不存在");
                }

                fstrSql = " select * from " + tablename;
                fstrSql_count = " select count(1) from " + tablename;

                //fstrSql += " where (Fuser_id='" + u_ID + "') ";
                fstrSql += " where (Fuser_uid=" + fuid + ") ";
                //fstrSql_count += " where (Fuser_id='" + u_ID + "') ";
                fstrSql_count += " where (Fuser_uid=" + fuid + ") ";

                if (filter != "")
                {
                    fstrSql += " AND (" + filter + ")";
                    fstrSql_count += " AND (" + filter + ")";
                }
            }
            else
            {
                throw new LogicException("号码不能为空");
            }
        }

        public GwqQueryClass(string u_ID,int is_tdeID,string filter)
        {
            if (u_ID != null && u_ID.Trim() != "")
            {
                string tablename = "";
                try
                {
                    if (is_tdeID != 1)
                        tablename = PublicRes.GetTableName("t_gwq", u_ID);
                    else
                    {
                        CoinPubQueryClass pq = new CoinPubQueryClass(u_ID, 1);
                        T_COIN_PUB pub = pq.GetResult();
                        tablename = PublicRes.GetTName("t_gwq", pub.fuid);
                    }
                }
                catch
                {
                    throw new LogicException("号码无效或不存在");
                }

                fstrSql = " select * from " + tablename;
                fstrSql_count = " select count(1) from " + tablename;

                if (is_tdeID == 1)
                {
                    fstrSql += " where (Ftde_id=" + u_ID + ") ";
                    fstrSql_count += " where (Ftde_id=" + u_ID + ") ";
                }
                else
                {
                    string uid = PublicRes.ConvertToFuid(u_ID);
                    fstrSql += " where (Fuser_uid='" + uid + "') ";
                    fstrSql_count += " where (Fuser_uid='" + uid + "') ";
                }

                if (filter != "")
                {
                    fstrSql += " AND (" + filter + ")";
                    fstrSql_count += " AND (" + filter + ")";
                }

                //打印
                //PublicRes.WriteFile(fstrSql);
                //PublicRes.WriteFile(fstrSql_count);
                //PublicRes.CloseFile();
            }
            else
            {
                throw new LogicException("号码不能为空");
            }
        }
        public T_GWQ GetResult(string ticket_id)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("GWQ"));
            T_GWQ result = new T_GWQ();
            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql + " AND (Fticket_id='" + ticket_id + "')");
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    result.GetInfoFromDB(dr);
                }
                else
                {
                    throw new LogicException("没有查找到相应的记录");
                }
                return result;
            }
            finally
            {
                da.CloseConn();
                da.Dispose();
            }
        }
    }
    #endregion

    #region 财付券发行单查询类
    public class CoinPubQueryClass : Query_BaseForNET
    {
        public CoinPubQueryClass(string u_ID, int is_tdeID)
        {
            if (u_ID != null && u_ID.Trim() != "")
            {
                string tablename = "c2c_db_gwq.t_gwq_pub";

                fstrSql = " select * from " + tablename;
                fstrSql_count = " select count(1) from " + tablename;

                if (is_tdeID == 1)
                {
                    fstrSql += " where (Ftde_id=" + u_ID + ") ";
                    fstrSql_count += " where (Ftde_id=" + u_ID + ") ";
                }
                else
                {
                    fstrSql += " where (FSpid='" + u_ID + "') ";
                    fstrSql_count += " where (FSpid='" + u_ID + "') ";
                }
            }
            else
            {
                throw new LogicException("发行者号码不能为空");
            }
        }

        public CoinPubQueryClass(string filter)
        {
            filter = filter == "" ? "" : (" WHERE " + filter);

            string tablename = "c2c_db_gwq.t_gwq_pub";
            fstrSql = " select * from " + tablename + filter;
            fstrSql_count = " select count(1) from " + tablename + filter;
        }

        public T_COIN_PUB GetResult()
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("GWQ"));
            T_COIN_PUB result = new T_COIN_PUB();
            try
            {
                da.OpenConn();
                DataTable dt = new DataTable();
                dt = da.GetTable(fstrSql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    result.LoadFromDB(dr, true);
                }
                else
                {
                    throw new LogicException("没有查找到相应的记录");
                }
                return result;
            }
            finally
            {
                da.CloseConn();
                da.Dispose();
            }
        }

    }
    #endregion

    #region 财付券操作日志查询类
    public class GwqRollQueryClass : Query_BaseForNET
    {
        public GwqRollQueryClass(string u_ID, string ticket_id)
        {
            if (u_ID != null && u_ID.Trim() != "")
            {
                string tablename = "";
                try
                {
                    tablename = PublicRes.GetTableName("t_operate_roll", u_ID);
                }
                catch
                {
                    throw new LogicException("查询号码解析时发生错误");
                }

                string fuid = PublicRes.ConvertToFuid(u_ID);

                fstrSql = " select * from " + tablename;
                fstrSql_count = " select count(1) from " + tablename;

                //fstrSql += " where (Fuser_id='" + u_ID + "' AND Fticket_id='"+ticket_id+"') ";
                fstrSql += " where (Fuser_uid='" + fuid + "' AND Fticket_id='" + ticket_id + "') ";
                //fstrSql_count += " where (Fuser_id='" + u_ID + "' AND Fticket_id='"+ticket_id+"') ";
                fstrSql_count += " where (Fuser_uid='" + fuid + "' AND Fticket_id='" + ticket_id + "') ";
            }
            else
            {
                throw new LogicException("号码不能为空");
            }
        }
    }
    #endregion

    #region 财付券发行记录类
    public class T_COIN_PUB : T_CLASS_BASIC
    {
        public bool IsNew = false; //是否为新增的中介帐户

        public string ftde_id;
        public string fspid;
        public string fatt_name;
        public string fqqid;
        public string fuid;
        public string fmer_id;
        public string ftype;
        public string fpub_type;
        public string fstate;
        public string flist_state;
        public string fstime;
        public string fetime;
        public string fpub_time;
        public string fpub_num;
        public string ffact_num;
        public string fdonate_type;
        public string ffee;
        public string fpre_fee;
        public string ffact_fee;
        public string fuse_pro;
        public string fmin_fee;
        public string fmax_num;
        public string furl;
        public string fpub_user;
        public string fex_user1;
        public string fex_user2;
        public string fuser_ip;
        public string fpub_ip;
        public string fstandby1;
        public string fstandby2;
        public string fstandby3;
        public string fstandby4;
        public string fstandby5;
        public string fstandby6;
        public string fstandby7;
        public string fstandby8;
        public string fmemo;
        public string fmodify_time;
        public string fpub_name;
        public string fac_stime;
        public string fac_etime;
        public string fac_flag;
        public string fac_num;
        public string fac_uin;
        public string fac_uid;
        public string returnUrl;

        public string Limits;
        public const string LIMIT_ALL = "00000";

        public void LoadFromDB(DataRow dr, bool GetLimits)
        {
            IsNew = false;
            base.LoadFromDB(dr);

            Limits = "";
            if (GetLimits)
            {
                System.Data.DataSet ds = null;
                try
                {
                    ds = PublicRes.returnDSAll("SELECT fspid,fpub_id,fmer_id FROM c2c_db_gwq.t_gwq_limit WHERE ftde_id=" + ftde_id, "GWQ");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0) Limits += ";";
                        Limits += ds.Tables[0].Rows[i]["fspid"].ToString() + "," +
                            ds.Tables[0].Rows[i]["fpub_id"].ToString() + "," +
                            ds.Tables[0].Rows[i]["fmer_id"].ToString();
                    }
                }
                catch
                {
                }
                if (ds != null) ds.Dispose();
            }
        }

        public bool CheckRight()
        {
            if (fspid == null || fspid == "")
                throw new LogicException("发行者号码不能为空！");

            // TODO: 1客户信息资料外移
            //根据SPID获取
            /*
            string strSql = "select fspecial,fname,fuid,fuidmiddle from c2c_db.t_merchant_info where fspid='" + fspid + "'";
            */
            string strSql = "spid=" + fspid;

            System.Data.DataSet dsTemp = null;
            string spid_fuid, spid_fqqid, spid_fpub_name, spid_fuidmiddle;
            try
            {
                /*
                //dsTemp = PublicRes.returnDSAll(strSql,"YW_30");
                dsTemp = PublicRes.returnDSAll(strSql,"ZL");
                */
                string errMsg = "";
                dsTemp = CommQuery.GetDataSetFromICE(strSql, CommQuery.QUERY_MERCHANTINFO, out errMsg);

                if (dsTemp == null || dsTemp.Tables.Count == 0 || dsTemp.Tables[0].Rows.Count == 0)
                    throw new LogicException("商户号和QQ对应关系在业务系统中尚未登记");

                spid_fuid = dsTemp.Tables[0].Rows[0]["fuid"].ToString();
                spid_fuidmiddle = dsTemp.Tables[0].Rows[0]["fuidmiddle"].ToString();
                if (spid_fuid == null || spid_fuid == "") spid_fuid = "0";
                spid_fqqid = dsTemp.Tables[0].Rows[0]["fspecial"].ToString();
                spid_fpub_name = dsTemp.Tables[0].Rows[0]["fname"].ToString();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (dsTemp != null) dsTemp.Dispose();
            }
            //自动获取QQ号码、Fuid和名称
            if (fqqid != null && fqqid != "" && fqqid != spid_fqqid)
            {
                //当指定了fqqid并且和spid绑定的qqid不同时，根据QQ号获取
                fuid = PublicRes.ConvertToFuid(fqqid);
                if (fuid == null || fuid == "")
                    throw new LogicException("QQ号在业务系统尚未登记");

                /*
                //strSql = "Select ftruename from " + PublicRes.GetTableName("t_user",fqqid) + " where fuid="+fuid;
                strSql = "Select ftruename from " + PublicRes.GetTableName("t_user_info",fqqid) + " where fuid="+fuid;
                //fpub_name = PublicRes.ExecuteOne(strSql,"YW_30");
                fpub_name = PublicRes.ExecuteOne(strSql,"ZL");
                */

                strSql = "uid=" + fuid;
                string Msg = "";
                fpub_name = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Ftruename", out Msg);
            }
            else
            {
                //根据SPID获取
                fuid = spid_fuid;
                fqqid = spid_fqqid;
                fpub_name = spid_fpub_name;
            }
            //检查是否有权限
            bool HasRight = false;
            //string ssign = PublicRes.ExecuteOne("Select FSign1 FROM c2c_db.t_user_att where fuid="+spid_fuidmiddle,"YW_30");
            string ssign = PublicRes.ExecuteOne("Select FSign1 FROM c2c_db.t_user_att where fuid=" + spid_fuidmiddle, "AU");
            if (ssign == null)
                HasRight = true;
            else
            {
                int isign = Convert.ToInt32(ssign);
                if ((isign & 1073741824) == 0)
                    HasRight = false;
                else
                    HasRight = true;
            }
            if (!HasRight)
                throw new LogicException("商户账号“" + fspid + "”没有财付券发行权限。");
            if (fuid != spid_fuid)
            {
                //ssign = PublicRes.ExecuteOne("Select FSign1 FROM c2c_db.t_user_att where fuid="+fuid,"YW_30");
                ssign = PublicRes.ExecuteOne("Select FSign1 FROM c2c_db.t_user_att where fuid=" + fuid, "AU");
                if (ssign == null)
                    HasRight = true;
                else
                {
                    int isign = Convert.ToInt32(ssign);
                    if ((isign & 1073741824) == 0)
                        HasRight = false;
                    else
                        HasRight = true;
                }
                if (!HasRight)
                    throw new LogicException("个人账号“" + fqqid + "”没有财付券发行权限。");
            }

            //初始化其他数据
            if (fstate == "" || fstate == null) fstate = "2";
            if (flist_state == "" || flist_state == null) flist_state = "1";
            fmodify_time = PublicRes.strNowTime.Replace("'", "");
            if (fpub_time == "" || fpub_time == null) fpub_time = Convert.ToDateTime(fmodify_time).ToString("yyyy-MM-dd");
            if (ffact_num == "" || ffact_num == null) ffact_num = "0";
            if (ftype == "" || ftype == null) ftype = "1";
            if (ftype == "1")
                fpre_fee = Convert.ToString(Convert.ToInt64(ffee) * Convert.ToInt64(fpub_num));
            else
                fpre_fee = "0";
            if (ffact_fee == "" || ffact_fee == null) ffact_fee = "0";
            if (fex_user1 == null) fex_user1 = "";
            if (fex_user2 == null) fex_user2 = "";
            if (fstandby1 == "" || fstandby1 == null) fstandby1 = "0";
            if (fstandby2 == "" || fstandby2 == null) fstandby2 = "0";
            if (fstandby3 == "" || fstandby3 == null) fstandby3 = "";
            if (fstandby4 == "" || fstandby4 == null) fstandby4 = "";
            if (fstandby5 == "" || fstandby5 == null) fstandby5 = "0";
            if (fstandby6 == "" || fstandby6 == null) fstandby6 = "0";
            if (fstandby7 == "" || fstandby7 == null) fstandby7 = "";
            if (fstandby8 == "" || fstandby8 == null) fstandby8 = "";
            if (Convert.ToDateTime(fstime) >= Convert.ToDateTime(fetime))
                throw new LogicException("生效日期必须小于结束日期");
            if (ftype == "2")
            {
                int iFee = Convert.ToInt32(ffee);
                if (iFee <= 0 || iFee >= 10000)
                    throw new LogicException("比例折扣券的面值必须在1－9999之间");
            }
            //初始化激活相关的字段
            if (fac_flag == "" || fac_flag == null) fac_flag = "2";
            if (fac_num == "" || fac_num == null) fac_num = "1";
            if (fpub_type == "1")
            {
                if (Convert.ToDateTime(fetime) <= Convert.ToDateTime(fac_etime) ||
                    Convert.ToDateTime(fac_stime) >= Convert.ToDateTime(fac_etime))
                    throw new LogicException("开始领用时间必须小于结束领用时间，并且都必须小于结束日期");
                fac_uid = PublicRes.ConvertToFuid(fac_uin);
                if (fac_uid == null || fac_uid == "")
                    throw new LogicException("被领用者账号在业务系统尚未登记");
            }
            else
            {
                fac_uin = "";
                fac_uid = "0";
            }
            if (fpub_type == "1")
            {
                //				if (fdonate_type!="1")
                //					throw new LogicException("发行类型为激活时，必须允许赠送");
            }
            return true;
        }

        public bool Update(Finance_Header fh)
        {
            ArrayList al = new ArrayList();
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("GWQ"));
            try
            {
                CheckRight();

                //开始更新
                da.OpenConn();
                if (IsNew)
                {
                    ftde_id = da.GetOneResult("SELECT MAX(ftde_id)+1 FROM c2c_db_gwq.t_gwq_pub");
                    if (ftde_id == null || ftde_id == "")
                        ftde_id = "1";
                    string strSql = "Insert c2c_db_gwq.t_gwq_pub "
                        + " (ftde_id,fspid,fatt_name,fqqid,fuid, fmer_id,ftype,fpub_type,fstate,flist_state,"
                        + " fstime,fetime,fpub_time,fpub_num,ffact_num,fdonate_type,"
                        + " ffee,fpre_fee,ffact_fee,fuse_pro,fmin_fee,fmax_num,furl,fpub_user,fex_user1,"
                        + " fex_user2,fuser_ip,fpub_ip,fstandby1,fstandby2,fstandby3,fstandby4,fmemo,fmodify_time,"
                        + " fpub_name,fac_stime,fac_etime,fac_flag,fac_num,fac_uin,fac_uid,fstandby5,fstandby6,fstandby7,fstandby8)"
                        + " values "
                        + " ({34},'{0}','{1}','{2}',{3},'{4}',{5},{6},{7},{8},"
                        + " '{9}','{10}','{11}',{12},{13},{14},"
                        + " {15},{16},{17},{18},{19},{20},'{21}','{22}','{23}',"
                        + " '{24}','{25}','{26}',{27},{28},'{29}','{30}','{31}','{32}',"
                        + " '{33}','{35}','{36}',{37},{38},'{39}',{40},{41},{42},'{43}','{44}')";
                    strSql = String.Format(strSql,
                        fspid, fatt_name, fqqid, fuid, fmer_id, ftype, fpub_type, fstate, flist_state,
                        fstime, fetime, fpub_time, fpub_num, ffact_num, fdonate_type,
                        ffee, fpre_fee, ffact_fee, fuse_pro, fmin_fee, fmax_num, furl, fpub_user, fex_user1,
                        fex_user2, fuser_ip, fpub_ip, fstandby1, fstandby2, fstandby3, fstandby4, fmemo, fmodify_time, fpub_name,
                        ftde_id, fac_stime, fac_etime, fac_flag, fac_num, fac_uin, fac_uid, fstandby5, fstandby6, fstandby7, fstandby8);
                    al.Add(strSql);
                }
                else
                {
                    string strSql = "UPDATE c2c_db_gwq.t_gwq_pub SET "
                        + " fspid='{0}',fatt_name='{1}',fqqid='{2}',fuid={3}, fmer_id='{4}',ftype={5},fpub_type={6},fstate={7},flist_state={8},"
                        + " fstime='{9}',fetime='{10}',fpub_time='{11}',fpub_num={12},ffact_num={13},fdonate_type={14},"
                        + " ffee={15},fpre_fee={16},ffact_fee={17},fuse_pro={18},fmin_fee={19},fmax_num={20},furl='{21}',fpub_user='{22}',fex_user1='{23}',"
                        + " fex_user2='{24}',fuser_ip='{25}',fpub_ip='{26}',fstandby1={27},fstandby2={28},fstandby3='{29}',fstandby4='{30}',fmemo='{31}',fmodify_time='{32}',"
                        + " fpub_name='{33}',fac_stime='{35}',fac_etime='{36}',fac_flag={37},fac_num={38},fac_uin='{39}',fac_uid={40},fstandby5={41},fstandby6={42},fstandby7='{43}',fstandby8='{44}'"
                        + " WHERE Ftde_id={34}";
                    strSql = String.Format(strSql,
                        fspid, fatt_name, fqqid, fuid, fmer_id, ftype, fpub_type, fstate, flist_state,
                        fstime, fetime, fpub_time, fpub_num, ffact_num, fdonate_type,
                        ffee, fpre_fee, ffact_fee, fuse_pro, fmin_fee, fmax_num, furl, fpub_user, fex_user1,
                        fex_user2, fuser_ip, fpub_ip, fstandby1, fstandby2, fstandby3, fstandby4, fmemo, fmodify_time,
                        fpub_name,
                        ftde_id,
                        fac_stime, fac_etime, fac_flag, fac_num, fac_uin, fac_uid, fstandby5, fstandby6, fstandby7, fstandby8);
                    al.Add(strSql);
                }
                //如果是发行加领用，则自动初始化in表
                if (fpub_type == "1")
                {
                    al.Add("DELETE FROM c2c_db_gwq.t_gwq_in WHERE Ftde_id=" + ftde_id);
                    string strSql = "INSERT c2c_db_gwq.t_gwq_in (Ftde_id,Fson_id,Fqqid,FNum,FImportDate,FReleaseDate,FState,FModify_Time)" +
                        " VALUES(" + ftde_id + ",0,'" + fac_uin + "'," + fpub_num + ",'" + fmodify_time + "','0000-00-00 00:00:00',1,'" + fmodify_time + "')";
                    al.Add(strSql);
                }
                UpdateLimit(fh, al);
                return da.ExecSqls_Trans(al);
            }
            finally
            {
                da.Dispose();
            }
        }

        public bool UpdateLimit(Finance_Header fh, ArrayList al)
        {
            try
            {
                Limits = Limits.Replace(" ", "").Replace("　", "").Replace("；", ";").Replace("，", ",");
                if (Limits == "")
                {
                    Limits = LIMIT_ALL + "," + LIMIT_ALL + "," + LIMIT_ALL;
                }
                string[] limits = Limits.Split(";".ToCharArray());

                string sql = "DELETE FROM c2c_db_gwq.t_gwq_limit WHERE ftde_id = " + ftde_id;
                al.Add(sql);

                sql = "INSERT c2c_db_gwq.t_gwq_limit (Ftde_id,Fpub_id,Fpub_uid,Fspid,Fmer_id," +
                    " Fstandby1,Fstandby2,Fstandby3,Fstandby4,Flist_state,Fcreate_time,Fmodify_time)" +
                    " VALUES ( " + ftde_id + ",'{2}',{3},'{0}','{1}',0,0,'','',1,'" + fmodify_time + "','" + fmodify_time + "')";

                string temp_pub_uid;
                for (int i = 0; i < limits.Length; i++)
                {
                    if (limits[i] == "") continue;
                    //如果重复，则跳过
                    bool IsExist = false;
                    for (int j = i - 1; j >= 0; j--)
                    {
                        if (limits[i] == limits[j])
                        {
                            IsExist = true;
                            break;
                        }
                    }
                    if (IsExist) continue;
                    //开始加入
                    string[] a = limits[i].Split(",".ToCharArray());
                    if (a == null || a.Length != 3)
                        throw new LogicException("使用范围中“" + limits[i] + "”格式不正确");
                    if (a[0] != LIMIT_ALL)
                    {
                        if (!SPIDIsExists(a[0]))
                            throw new LogicException("使用范围中商户号“" + a[0] + "”不存在");
                    }
                    if (a[1] != LIMIT_ALL)
                    {
                        temp_pub_uid = PublicRes.ConvertToFuid(a[1]);
                        if (temp_pub_uid == null || temp_pub_uid == "")
                            throw new LogicException("使用范围中卖家帐号“" + a[1] + "”不存在");
                    }
                    else
                        temp_pub_uid = "0";
                    al.Add(String.Format(sql, a[0], a[2], a[1], temp_pub_uid));
                }
                return true;
            }
            finally
            {
            }
        }
    }
    #endregion

    #region 财付券类
    public class T_GWQ : T_CLASS_BASIC
    {
        public bool IsNew = false; //是否为新增的中介帐户

        public string fauto_id;
        public string fticket_id;
        public string ftde_id;
        public string fson_id;
        public string fuser_id;
        public string fuser_uid;
        public string fpub_id;
        public string fpub_uid;
        public string flistid;
        public string fspid;
        public string fatt_name;
        public string fmer_id;
        public string fdonate_type;
        public string fdonate_num;
        public string ftype;
        public string fpub_type;
        public string fstate;
        public string flist_state;
        public string fadjust_flag;
        public string ffee;
        public string fuse_pro;
        public string fmin_fee;
        public string fmax_num;
        public string ffact_fee;
        public string fstime;
        public string fetime;
        public string fpub_time;
        public string fuse_time;
        public string furl;
        public string fpub_user;
        public string fuser_ip;
        public string fpub_ip;
        public string fstandby1;
        public string fstandby2;
        public string fstandby3;
        public string fstandby4;
        public string fmemo;
        public string fmodify_time;
        public string fpub_name;
        public string fdonate_id;
        public string fdonate_uid;
        public string fdonate_time;
        public string fuse_listid;
        public string fac_stime;
        public string fac_etime;
        public string fac_flag;
        public string fac_num;
        public string fac_uin;
        public string fac_uid;

        public void GetInfoFromDB(DataRow dr)
        {
            IsNew = false;
            base.LoadFromDB(dr);
        }
    }
    #endregion

    #region 自助申诉处理类

    public class CFTUserAppealClass : Query_BaseForNET
    {
        //未领单申诉查找的有效期 furion 20080508
        public static int AppealValidDay = Int32.Parse(ConfigurationManager.AppSettings["AppealValidDay"]);
        /*
                public static bool GetAppealUserSumInfo(string user, DateTime begintime, DateTime endtime, out string msg)
                {
                    msg = "";

                    MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
                    try
                    {				
                        da.OpenConn();

                        string validdaysql = DateTime.Now.AddDays(-AppealValidDay).ToString("yyyy-MM-dd 00:00:00");
                        validdaysql = " FsubmitTime >='" + validdaysql + "' ";

                        string strSql = "select count(*) from t_tenpay_appeal_trans where " + validdaysql + " and Fstate=0 and  FType in (1,2,3,5) "; //所有未处理数
					

                        int itmp = Int32.Parse(da.GetOneResult(strSql));
                        msg = "所有未处理申诉：[" + itmp + "];";

                        strSql = "select count(*) from t_tenpay_appeal_trans where " + validdaysql + " and Fstate=8 and Fpickuser='" + user + "'"; //领单数				
                        itmp = Int32.Parse(da.GetOneResult(strSql));
                        msg += "本人领单数：[" + itmp + "];";

        //				strSql = "select count(*) from t_tenpay_appeal_trans where Fpickuser='" + user 
        //					//+ "' and Fstate<>8 and FPickTime between '" + begintime.ToString("yyyy-MM-dd HH:mm:ss")
        //					+ "' and Fstate<>8 and FCheckTime between '" + begintime.ToString("yyyy-MM-dd HH:mm:ss")
        //					+ "' and '" + endtime.ToString("yyyy-MM-dd HH:mm:ss") + "'"; //指定时间内已处理数
        //				itmp = Int32.Parse(da.GetOneResult(strSql));
                        strSql = "select SuccessNum,FailNum,OtherNum from t_tenpay_appeal_kf_total where User='" + user + "' and OperationDay='" + begintime.ToString("yyyy-MM-dd") + "'";
                        DataSet ds= da.dsGetTotalData(strSql);
                        if(ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                        {
                            itmp = Int32.Parse(ds.Tables[0].Rows[0][0].ToString()) + Int32.Parse(ds.Tables[0].Rows[0][1].ToString()) + Int32.Parse(ds.Tables[0].Rows[0][2].ToString());
                        }
                        else
                            itmp = 0;

                        msg += "本人" + begintime.ToString("yyyy-MM-dd") + "处理数：[" + itmp + "];";
                        return true;
                    }
                    catch(Exception err)
                    {
                        msg = PublicRes.replaceMStr(err.Message);
                        return false;
                    }
                    finally
                    {
                        da.Dispose();
                    }
			
                }
        */
        private static bool SendAppealMail(string email, int ftype, bool issucc, string param1,
            string param2, string param3, string param4, string Reason, string OtherReason, string fuin, out string msg)
        {
            msg = "";

            if (email == null || email.Trim() == "")
            {
                //furion 20060902 是否支持不发邮件取决于这里.要么返回真,要么返回假
                return true;
            }

            string filename = ConfigurationManager.AppSettings["ServicePath"].Trim();
            if (!filename.EndsWith("\\"))
                filename += "\\";

            string title = "";
            string actiontype = "";
            switch (ftype)
            {
                case 0:
                    {
                        title = "财付通申诉解绑财付盾通知！";
                        if (issucc)
                            filename += "UKeyYes.htm";
                        else
                            filename += "UKeyNo.htm";
                        break;
                    }
                case 1:
                    {
                        title = "财付通取回支付密码通知！";
                        if (issucc)
                        {
                            filename += "GetPwdYes.htm";
                            actiontype = "2031";
                        }
                        else
                        { 
                            filename += "GetPwdNo.htm";
                            actiontype = "2032";
                        }
                        break;
                    }
                case 2:
                case 3:
                    {
                        title = "财付通修改真实姓名通知！";
                        if (issucc)
                        {
                            filename += "ChangeNameYes.htm";
                            actiontype = "2035";
                        }
                        else
                        {
                            filename += "ChangeNameNo.htm";
                            actiontype = "2036";
                        }
                        break;
                    }
                case 4:
                    {
                        title = "注销财付通帐户的通知！";
                        if (issucc)
                        {
                            filename += "DelUserYes.htm";
                            actiontype = "2034";
                        }
                        else
                        { 
                            filename += "DelUserNo.htm";
                            actiontype = "2043";
                        }
                        break;
                    }
                case 5:
                case 6://简化注册用户换手机 andrew 20110419
                    {
                        title = "财付通关联手机更换申诉通知！";
                        if (issucc)
                        { 
                            filename += "ChangeMobileYes.htm";
                            actiontype = "2027";
                        }
                        else
                        { 
                            filename += "ChangeMobileNo.htm";
                            actiontype = "2037";
                        }
                        break;
                    }
                case 7://更换身份证号
                    {
                        title = "财付通提醒您！";
                        if (issucc)
                        { 
                            filename += "ChangeCreidYes.htm";
                            actiontype = "2041";
                        }
                        else
                        { 
                            filename += "ChangeCreidNo.htm";
                            actiontype = "2042";
                        }
                        break;
                    }
                case 9://手机令牌
                    {
                        title = "财付通提醒您！";
                        if (issucc)
                        { 
                            filename += "MobileCommandYes.htm";
                            actiontype = "2029";
                        }
                        else
                        { 
                            filename += "MobileCommandNo.htm";
                            actiontype = "2030";
                        }
                        break;
                    }
                case 10://第三方令牌
                    {
                        title = "财付通提醒您！";
                        if (issucc)
                        { 
                            filename += "MobileCommandYes.htm";
                            actiontype = "2029";
                        }
                        else
                        { 
                            filename += "MobileCommandNo.htm";
                            actiontype = "2030";
                        }
                        break;
                    }
                default:
                    {
                        msg = "给出类型不对";
                        return false;
                    }
            }

            try
            {
                if (!issucc)
                {
                    if (Reason == "")
                    {
                        Reason = "- " + OtherReason;
                    }
                    else
                    {
                        Reason = Reason.Substring(0, Reason.Length - 1);
                        string[] ReasonDetail = Reason.Split('&');
                        Reason = "";
                        // 2012/4/18 添加申诉原因
                        for (int i = 0; i < ReasonDetail.Length; i++)
                        {
                            if (ReasonDetail[i] == "特殊申诉找回地址")
                            {
                                string specialAppealFindBack = @"请您重新提交申述表时，上传账户资金来源截图。请参考“<a href='http://help.tenpay.com/cgi-bin/helpcenter/help_center.cgi?id=2232'> 特殊申诉找回</a>”指引";
                                
                                Reason += "- " + specialAppealFindBack + "<br>";
                            }
                            else
                            {
                                Reason += "- " + ReasonDetail[i].ToString() + "<br>";
                            }
                        }
                        if (OtherReason != "")
                        {
                            Reason += "- " + OtherReason + "<br>";
                        }
                    }
                }

                if (PublicRes.IgnoreLimitCheck)
                    return true;
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            
            if (ftype == 0)
            {
                //使用老的邮件发送方式
                StreamReader sr = new StreamReader(filename, System.Text.Encoding.GetEncoding("GB2312"));
                try {
                    string content = sr.ReadToEnd();
                    content = String.Format(content, param1, fuin, param2, param3, param4, Reason);
                    TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend newMail = new TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend();
                    newMail.SendMail(email, "", title, content, true, null);
                    return true;
                }
                catch (Exception err)
                {
                    msg = err.Message;
                    return false;
                }
                finally
                {
                    sr.Close();
                }
                

            }
            else { 
                //yinhuang 2031/8/7
                string str_params = "p_name=" + param1 + "&p_parm1=" + param2 + "&p_parm2=" + param3 + "&p_parm3=" + param4 + "&p_parm4=" + Reason;
                TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsg(email, actiontype, str_params);

                return true;
            }
        }

        //申诉新库表发邮件，只有1、5、6
        private static bool SendAppealMailNew(string email, int ftype, bool issucc, string param1,
           string param2, string param3, string param4, string Reason, string OtherReason, string fuin, out string msg)
        {
            msg = "";

            if (email == null || email.Trim() == "")
            {
                //furion 20060902 是否支持不发邮件取决于这里.要么返回真,要么返回假
                return true;
            }

            string title = "";
            string actiontype = "";
            switch (ftype)
            {
                
                case 1:
                    {
                        title = "财付通取回支付密码通知！";
                        if (issucc)
                        {
                            actiontype = "2217";
                        }
                        else
                        {
                            actiontype = "2032";
                        }
                        break;
                    }
              
                case 5:
                case 6://简化注册用户换手机 
                    {
                        title = "财付通关联手机更换申诉通知！";
                        if (issucc)
                        {
                            actiontype = "2223";
                        }
                        else
                        {
                            actiontype = "2037";
                        }
                        break;
                    }
                default:
                    {
                        msg = "给出类型不对";
                        return false;
                    }
            }
            string str_params = "p_name=" + param1 + "&p_parm1=" + param2 + "&p_parm2=" + param3 + "&p_parm3=" + param4 + "&p_parm4=" + Reason;
                TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsg(email, actiontype, str_params);
                return true;

        }

        //申诉新库表发QQTips
        private static bool SendAppealQQTips(string qqid, int ftype, string url, bool issucc, out string msg)
        {
            msg = "";

            if (qqid == null || qqid.Trim() == "")
            {
                // 是否支持不发QQtips取决于这里.要么返回真,要么返回假
                return true;
            }

            string actiontype = "";
            switch (ftype)
            {

                case 1:
                    {
                        if (issucc)
                        {
                            actiontype = "2217";
                        }
                        else
                        {
                            actiontype = "";
                        }
                        break;
                    }
                default:
                    {
                        return false;
                    }
            }
            string str_params = "url=" + url ;
            TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsgQQTips(qqid, actiontype, str_params);
            return true;

        }

        //申诉新库表发短信
        private static bool SendAppealMessage(string mobile,int ftype, string url, bool issucc,string reason, out string msg)
        {
            msg = "";

            if (mobile == null || mobile.Trim() == "")
            {
                // 是否支持不发短信取决于这里.要么返回真,要么返回假
                return false;
            }
            string noticeMobile = "";//短信中手机号，屏蔽中间6位
            string actiontype = "";
            switch (ftype)
            {

                case 1:
                    {
                        if (issucc)
                        {
                            actiontype = "2217";
                        }
                        else
                        {
                            actiontype = "2222";
                        }
                        break;
                    }
                case 5:
                case 6://简化注册用户换手机 
                    {
                        noticeMobile = mobile.Substring(0, 3) + "******" + mobile.Substring(9, 2);
                        if (issucc)
                        {
                            actiontype = "2223";
                        }
                        else
                        {
                            actiontype = "2224";
                        }
                        break;
                    }
                default:
                    {
                        msg = "给出类型不对";
                        return false;
                    }
            }
            try
            {
                if (!issucc)
                {
                    reason = reason.Substring(0, reason.Length - 1);
                    string[] ReasonDetail = reason.Split('&');
                    reason = "";
                    for (int i = 0; i < ReasonDetail.Length; i++)
                    {
                        if (i == 0)
                            reason += ReasonDetail[i].ToString();
                        else
                        {
                            if (ReasonDetail[i].ToString() != "")
                                reason += "；" + ReasonDetail[i].ToString();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
   
            string str_params = "url=" + url + "&reason=" + reason + "&phoneNUM=" + noticeMobile;
            TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMessage(mobile, actiontype, str_params);
            return true;

        }

        /*
                public static string makePwd()
                {
                    string str1 = "ABCDEFGHIJKLMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz";   //数据字典大写ABCDEF + 小写ghijklmnopqrstuvwxyz
                    int iStr1 = str1.Length;
			
                    string str2 = "2345678998765432";                   //数据字典特殊字符 + 数字逆序
                    int iStr2 = str2.Length;

                    string str3 = "abcdefghjkmnpqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ";   //数据字典小写abcdef + 大写GHIJKLMNOPQRSTUVWXYZ
                    int iStr3 = str3.Length;

                    string str4 = "2345678998765432";   //数据字典小写abcdef + 大写GHIJKLMNOPQRSTUVWXYZ
                    int iStr4 = str4.Length;

                    string s= null;

                    ArrayList al = new ArrayList();
                    al.Add(str1);
                    al.Add(str2);
                    al.Add(str3);
                    al.Add(str4);
			
                    for (int i = 0;i<4; i++)
                    {
                        s += pwd(al[i].ToString());  //从每个数据字典中随机取出2位，组成12位随机密码
                    }

                    return s;

                }

                private static string pwd(string str)
                {
                    int ln = str.Length;
			
                    System.Random rd = new Random();
			
                    string pwd = null;
			
                    for (int i = 0; i< 2; i++)
                    {
                        //furion 这个地方一定需要延时吗？
                        System.Threading.Thread.Sleep(1);
                        int j = rd.Next(str.Length);
                        pwd += str[j];
                    }

                    return pwd;
                }
        */

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

        private static string pwd(string str)
        {
            int ln = str.Length;

            System.Random rd = new Random();

            string pwd = null;

            for (int i = 0; i < 2; i++)
            {
                //furion 这个地方一定需要延时吗？
                System.Threading.Thread.Sleep(1);
                int j = rd.Next(str.Length);
                pwd += str[j];
            }

            return pwd;
        }

        private static bool GetNewPwd(string qqid, int clear_pps, string userip, string nowtime, string question1, string answer1, bool IsNew, out string msg)
        {
            msg = "";

            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("YW_30"));
            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("ZL"));
            try
            {
                //da.OpenConn();
                string pwd = makePwd();
                string pwdmd5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, "md5").ToLower();

                string uid = PublicRes.ConvertToFuid(qqid);

                // TODO: 1客户信息资料外移
                if (uid != null && uid.Trim() != "")
                {

                    /*
                    //furion 20061128 email登录修改，下面的地方需要修改。
                    string strSql = "update " + PublicRes.GetTName("t_user_info",uid) + " SET Fpasswd = '" + pwdmd5 
                        + "',Fip='" + userip + "',Fmodify_time='" + nowtime + "' where fuid =" + uid ;

                    if(clear_pps == 1)
                    {
                        strSql = "update " + PublicRes.GetTName("t_user_info",uid) + " SET Fpasswd = '" + pwdmd5 
                            + "',Fquestion1 = null,Fanswer1 = null,Fquestion2 = null,Fanswer2 = null" 
                            + ",Fip='" + userip + "',Fmodify_time='" + nowtime + "' where fuid =" + uid ;
                    }				
			

                    if(da.ExecSqlNum(strSql) == 1)
                    */

                    string strSql = "uid=" + uid;
                    strSql += "&modify_time=" + PublicRes.strNowTimeStander;
                    strSql += "&passwd=" + pwdmd5;
                    strSql += "&pass_flag=0";
                    strSql += "&ip=" + userip;

                    if (clear_pps == 1)
                    {
                        if (IsNew)
                        {
                            strSql += "&question1=" + question1;
                            strSql += "&answer1=" + answer1;
                            strSql += "&question2=";
                            strSql += "&answer2=";

                            if (question1 == "" || answer1 == "")
                            {
                                msg = "密保问题或答案为空,操作失败";
                                return false;
                            }
                        }
                        else
                        {
                            strSql += "&question1=" + question1;
                            strSql += "&answer1=" + answer1;
                            strSql += "&question2=";
                            strSql += "&answer2=";
                        }
                    }

                    int iresult = CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_USERINFO, out msg);

                    if (iresult == 1)//测试加iresult == -5
                    {
                        msg = pwd;
                        //发送邮件吧.
                        return true;
                    }
                    else
                    {
                        msg = "执行更新密码操作时失败";
                        return false;
                    }
                }
                else
                {
                    msg = "获取内部ID失败.";
                    return false;
                }

            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                //da.Dispose();
            }
        }

        private static bool CheckQuestion(string qqid, string userip, string nowtime, string question1, string answer1, bool IsNew, out string msg)
        {
            msg = "";
            try
            {
                string uid = PublicRes.ConvertToFuid(qqid);

                // TODO: 1客户信息资料外移
                if (uid != null && uid.Trim() != "")
                {


                    string strSql = "uid=" + uid;
                    strSql += "&modify_time=" + PublicRes.strNowTimeStander;
                    strSql += "&ip=" + userip;


                    if (IsNew)
                    {
                        strSql += "&question1=" + question1;
                        strSql += "&answer1=" + answer1;
                        strSql += "&question2=";
                        strSql += "&answer2=";

                        if (question1 == "" || answer1 == "")
                        {
                            msg = "密保问题或答案为空,操作失败";
                            return false;
                        }
                    }
                    else
                    {
                        strSql += "&question1=" + question1;
                        strSql += "&answer1=" + answer1;
                        strSql += "&question2=";
                        strSql += "&answer2=";
                    }


                    int iresult = CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_USERINFO, out msg);

                    if (iresult == 1)
                    {
                        msg = "密保问题和答案替换成功";
                        //发送邮件吧.
                        return true;
                    }
                    else
                    {
                        msg = "密保问题和答案替换失败";
                        return false;
                    }
                }
                else
                {
                    msg = "获取内部ID失败.";
                    return false;
                }

            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                //da.Dispose();
            }
        }
        private static bool DelUser(string qqid, string email, string reason, string user, string userIP, string nowtime, out string msg)
        {
            msg = "";

            try
            {
                Finance_Manage ms = new Finance_Manage();
                if (!ms.logOnUser(qqid, reason, user, userIP, out msg))
                {
                    msg += "销户操作失败！";
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
        }

        private static bool UpdateUserName(string qqid, string new_name, bool iscompany, string userip, string nowtime, out string msg)
        {
            msg = "";

            if (new_name == null || new_name.Trim() == "")
            {
                msg = "新名称不能为空";
                return false;
            }

            new_name = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.replaceSqlStr(new_name);

            //已更改  furion V30_FURION核心查询需改动 修改调用核心
            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("YW"));
            //MySqlAccess da_zl = new MySqlAccess(PublicRes.GetConnString("ZL"));
            try
            {
                string uid = PublicRes.ConvertToFuid(qqid);

                // TODO: 1客户信息资料外移
                if (uid != null && uid.Trim() != "")
                {
                    /*
                    string strCmd1 = "update " + PublicRes.GetTName("t_user_info",uid) + " SET Ftruename = '" + new_name  
                        + "',Fip='" + userip + "',Fmodify_time='" + nowtime + "' where fuid =" + uid ;
                    string strCmd2 = "update " + PublicRes.GetTName("t_user",uid)      + " SET Ftruename = '" + new_name  
                        + "',Flogin_ip='" + userip + "',Fmodify_time='" + nowtime + "' where fuid =" + uid ;
                    string strCmd3 = "update " + PublicRes.GetTName("t_bank_user",uid) + " SET Ftruename = '" + new_name  
                        + "',Flogin_ip='" + userip + "',Fmodify_time='" + nowtime + "' where fuid =" + uid ;
			
                    if(iscompany)
                    {
                        strCmd1 = "update " + PublicRes.GetTName("t_user_info",uid) + " SET Fcompany_name='" + new_name  
                            + "',Fip='" + userip + "',Fmodify_time='" + nowtime + "' where fuid =" + uid ;
                        strCmd2 = "update " + PublicRes.GetTName("t_user",uid)      + " SET Fcompany_name='" + new_name  
                            + "',Flogin_ip='" + userip + "',Fmodify_time='" + nowtime + "' where fuid =" + uid ;
                        strCmd3 = "update " + PublicRes.GetTName("t_bank_user",uid) + " SET Fcompany_name='" + new_name  
                            + "',Flogin_ip='" + userip + "',Fmodify_time='" + nowtime + "' where fuid =" + uid ; 
                    }

                    ArrayList al = new ArrayList();
			
                    al.Add(strCmd1);
                    //al.Add(strCmd2);
                    al.Add(strCmd3);

                    //da.OpenConn();
                    da_zl.OpenConn();

                    //if(da.ExecSqls_Trans(al))
                    //if(da.ExecSqlNum(strCmd2) == 1 && da_zl.ExecSqls_Trans(al))
                    if( da_zl.ExecSqls_Trans(al))
                    {
                    */

                    string strsetname = "&truename=" + new_name;
                    if (iscompany)
                        strsetname = "&company_name=" + new_name;

                    string systemtime = PublicRes.strNowTimeStander;

                    string strSql = "uid=" + uid;
                    strSql += "&modify_time=" + systemtime;
                    strSql += "&ip=" + userip;
                    strSql += strsetname;

                    int iresult = CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_USERINFO, out msg);

                    if (iresult != 1)
                        return false;


                    ICEAccess ice = new ICEAccess(PublicRes.ICEServerIP, PublicRes.ICEPort);
                    try
                    {
                        ice.OpenConn();
                        string strwhere = "where=" + ICEAccess.URLEncode("fuid=" + uid + "&");
                        strwhere += ICEAccess.URLEncode("fcurtype=" + 1 + "&");

                        string strUpdate = "data=" + ICEAccess.URLEncode("fip=" + userip);
                        if (iscompany)
                        {
                            strUpdate += ICEAccess.URLEncode("&fcompany_name=" + ICEAccess.URLEncode(new_name));
                        }
                        else
                        {
                            strUpdate += ICEAccess.URLEncode("&ftruename=" + ICEAccess.URLEncode(new_name));
                        }

                        strUpdate += ICEAccess.URLEncode("&fmodify_time=" + PublicRes.strNowTimeStander);

                        string strResp = "";
                        //3.0接口测试需要 furion 20090708
                        if (ice.InvokeQuery_Exec(YWSourceType.用户资源, YWCommandCode.修改用户信息, uid, strwhere + "&" + strUpdate, out strResp) != 0)
                        {
                            throw new Exception("修改用户信息时出错！" + strResp);
                        }

                        return true;
                    }
                    catch (Exception err)
                    {
                        msg = err.Message;
                        return false;
                    }
                    finally
                    {
                        ice.Dispose();
                    }
                }
                else
                {
                    msg = "获取内部ID失败.";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                //da.Dispose();
                //da_zl.Dispose();
            }
        }

        private static bool UpdateCreid(string qqid, string creid, string userip, string nowtime, out string msg)
        {
            msg = "";

            if (creid == null || creid.Trim() == "")
            {
                msg = "新身份证号不能为空";
                return false;
            }

            creid = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.replaceSqlStr(creid);
            try
            {
                string uid = PublicRes.ConvertToFuid(qqid);

                // TODO: 1客户信息资料外移
                if (uid != null && uid.Trim() != "")
                {
                    string systemtime = PublicRes.strNowTimeStander;

                    string strSql = "uid=" + uid;
                    strSql += "&modify_time=" + systemtime;
                    strSql += "&ip=" + userip;
                    strSql += "&creid=" + creid;

                    int iresult = CommQuery.ExecSqlFromICE(strSql, CommQuery.UPDATE_USERINFO, out msg);

                    if (iresult == 1)
                        return true;
                    else
                        return false;
                }
                else
                {
                    msg = "获取内部ID失败.";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
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

        private static string GetOneParameter(string fid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            try
            {
                da.OpenConn();
                string strSql = "select FParameter from t_tenpay_appeal_trans where Fid=" + fid;
                return da.GetOneResult(strSql);
            }
            catch (Exception err)
            {
                return "";
            }
            finally
            {
                da.Dispose();
            }
        }

        public static bool HandleParameter(DataSet ds, bool haveimg)
        {
            //加入一个证书库连接.
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CRT"));
            try
            {
                ds.Tables[0].Columns.Add("FTypeName", typeof(String));
                ds.Tables[0].Columns.Add("FStateName", typeof(String));

                ds.Tables[0].Columns.Add("cre_id", typeof(string)); //证件号码
                ds.Tables[0].Columns.Add("cre_type", typeof(string)); //证件类型
                ds.Tables[0].Columns.Add("cre_image", typeof(string)); //图片
                ds.Tables[0].Columns.Add("clear_pps", typeof(string)); //1为清除密保
                ds.Tables[0].Columns.Add("reason", typeof(string)); //原因
                ds.Tables[0].Columns.Add("old_name", typeof(string));
                ds.Tables[0].Columns.Add("new_name", typeof(string));
                ds.Tables[0].Columns.Add("old_company", typeof(string));
                ds.Tables[0].Columns.Add("new_company", typeof(string));

                //更换手机暂时加入一个字段,原请求证件地址
                ds.Tables[0].Columns.Add("cre_image2", typeof(string)); //图片
                ds.Tables[0].Columns.Add("question1", typeof(string)); //密保1
                ds.Tables[0].Columns.Add("answer1", typeof(string)); //答案1
                ds.Tables[0].Columns.Add("cont_type", typeof(string)); // 1: 手机 2: email//cont_type=1时,mobile_no有效 cont_type=2时,femail有效
                ds.Tables[0].Columns.Add("mobile_no", typeof(string));
                ds.Tables[0].Columns.Add("labIsAnswer", typeof(string)); //是否答对密保
                ds.Tables[0].Columns.Add("client_ip", typeof(string)); //申诉人IP
                ds.Tables[0].Columns.Add("score", typeof(string)); //实际得分
                ds.Tables[0].Columns.Add("standard_score", typeof(string)); //免审核标准分
                ds.Tables[0].Columns.Add("detail_score", typeof(string)); //得分明细
                ds.Tables[0].Columns.Add("IsPass", typeof(string)); //结果
                ds.Tables[0].Columns.Add("new_cre_id", typeof(string)); //新身份证
                ds.Tables[0].Columns.Add("risk_result", typeof(string)); //风控标记
                ds.Tables[0].Columns.Add("from", typeof(string)); //申诉渠道标记（手机）

                // 风控冻结要求加入字段如下：
                /*
                ds.Tables[0].Columns.Add("email",typeof(string));		// 邮箱
                ds.Tables[0].Columns.Add("pkey",typeof(string));		// 验证邮件连接时传的PKEY
                ds.Tables[0].Columns.Add("contact_no",typeof(string));	// 用户联系电话
                ds.Tables[0].Columns.Add("otherImage1",typeof(string));	// 其他图片1
                ds.Tables[0].Columns.Add("otherImage2",typeof(string));	// 其他图片2
                ds.Tables[0].Columns.Add("otherImage3",typeof(string));	// 其他图片3
                ds.Tables[0].Columns.Add("otherImage4",typeof(string));	// 其他图片4
                ds.Tables[0].Columns.Add("otherImage5",typeof(string));	// 其他图片5
                ds.Tables[0].Columns.Add("rec_cftadd",typeof(string));	// 最近使用财付通的地址
                ds.Tables[0].Columns.Add("prove_banlance_image",typeof(string));	// 余额资金来源证明
                ds.Tables[0].Columns.Add("DC_timeaddress",typeof(string));	// 最近安装数字证书的时间和地址
                ds.Tables[0].Columns.Add("mobile",typeof(string));		// 绑定的手机号
                */


                da.OpenConn();

                // 2012/4/28 修改，因为目前如果其中一个元素查找失败(可能是由于该帐号已经注销)，则会导致后面的dataRow查询失败！
                // 所以增加一个try catch模块，忽略查询失败的记录
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    try
                    {
                        dr["cre_id"] = "";
                        dr["cre_type"] = "";
                        dr["cre_image"] = "";
                        dr["clear_pps"] = "0";
                        dr["reason"] = "";
                        dr["old_name"] = "";
                        dr["new_name"] = "";
                        dr["old_company"] = "";
                        dr["new_company"] = "";
                        dr["question1"] = "";
                        dr["answer1"] = "";
                        dr["cont_type"] = "";
                        dr["mobile_no"] = "";
                        dr["labIsAnswer"] = "";
                        dr["client_ip"] = "";
                        dr["score"] = "";
                        dr["standard_score"] = "";
                        dr["detail_score"] = "";
                        dr["IsPass"] = "";
                        dr["new_cre_id"] = "";
                        dr["risk_result"] = "";
                        dr["from"] = "";

                        string fuin = dr["FUin"].ToString();

                        string fuid = PublicRes.ConvertToFuid(fuin);
                        if (fuid == null || fuid.Trim() == "")
                        {
                            // 2012/4/28 修改，用户注销之后，应该是查不到相应的UID的
                            continue;
                            //return false;
                        }

                        string strSql = "select Fattach from " + PublicRes.GetTName("cft_digit_crt", "t_user_attr", fuid)
                            + " where Fuid=" + fuid + " and Fattr=1";

                        string strtmp = QueryInfo.GetString(da.GetOneResult(strSql));
                        dr["cre_image2"] = strtmp;

                        //string parameter = dr["FParameter"].ToString();
                        if (haveimg)
                        {
                            string parameter = GetOneParameter(dr["Fid"].ToString());

                            string[] paramlist = parameter.Split('&');

                            foreach (string param in paramlist)
                            {
                                if (param.StartsWith("cre_id="))
                                {
                                    dr["cre_id"] = getCgiString(param.Replace("cre_id=", ""));
                                }
                                else if (param.StartsWith("cre_type="))
                                {
                                    dr["cre_type"] = getCgiString(param.Replace("cre_type=", ""));
                                }
                                else if (param.StartsWith("cre_image="))
                                {
                                    dr["cre_image"] = getCgiString(param.Replace("cre_image=", ""));
                                }
                                else if (param.StartsWith("clear_pps="))
                                {
                                    dr["clear_pps"] = getCgiString(param.Replace("clear_pps=", ""));
                                }
                                //						else if(param.StartsWith("email"))
                                //						{
                                //							dr["email"] = getCgiString(param.Replace("email=",""));
                                //						}
                                else if (param.StartsWith("reason="))
                                {
                                    dr["reason"] = getCgiString(param.Replace("reason=", ""));
                                }
                                else if (param.StartsWith("old_name="))
                                {
                                    dr["old_name"] = getCgiString(param.Replace("old_name=", ""));
                                }
                                else if (param.StartsWith("new_name="))
                                {
                                    dr["new_name"] = getCgiString(param.Replace("new_name=", ""));
                                }
                                else if (param.StartsWith("old_company="))
                                {
                                    dr["old_company"] = getCgiString(param.Replace("old_company=", ""));
                                }
                                else if (param.StartsWith("new_company="))
                                {
                                    dr["new_company"] = getCgiString(param.Replace("new_company=", ""));
                                }
                                else if (param.StartsWith("question1="))
                                {
                                    dr["question1"] = getCgiString(param.Replace("question1=", ""));
                                }
                                else if (param.StartsWith("answer1="))
                                {
                                    dr["answer1"] = getCgiString(param.Replace("answer1=", ""));
                                }
                                else if (param.StartsWith("cont_type="))
                                {
                                    dr["cont_type"] = getCgiString(param.Replace("cont_type=", ""));
                                }
                                else if (param.StartsWith("mobile_no="))
                                {
                                    dr["mobile_no"] = getCgiString(param.Replace("mobile_no=", ""));
                                }
                                else if (param.StartsWith("ENV_ClientIp="))
                                {
                                    dr["client_ip"] = getCgiString(param.Replace("ENV_ClientIp=", ""));
                                }
                                else if (param.StartsWith("score="))
                                {
                                    dr["score"] = getCgiString(param.Replace("score=", ""));
                                }
                                else if (param.StartsWith("standard_score="))
                                {
                                    dr["standard_score"] = getCgiString(param.Replace("standard_score=", ""));
                                }
                                else if (param.StartsWith("detail_score="))
                                {
                                    dr["detail_score"] = getCgiString(param.Replace("detail_score=", ""));
                                }
                                else if (param.StartsWith("new_cre_id="))
                                {
                                    dr["new_cre_id"] = getCgiString(param.Replace("new_cre_id=", ""));
                                }
                                else if (param.StartsWith("RISK_RESULT="))
                                {
                                    string risk_result = getCgiString(param.Replace("RISK_RESULT=", ""));
                                    if (risk_result == "0")
                                        dr["risk_result"] = "";
                                    else if (risk_result == "1")
                                        dr["risk_result"] = "风控异常单无需人工回访用户";
                                    else if (risk_result == "2")
                                        dr["risk_result"] = "风控异常单需人工回访用户";
                                    else
                                        dr["risk_result"] = risk_result;
                                }
                                else if (param.StartsWith("from="))
                                {
                                    dr["from"] = getCgiString(param.Replace("from=", ""));
                                }

                            }
                        }

                        if (dr["detail_score"].ToString() != "")
                        {
                            try
                            {
                                string detail_score = System.Web.HttpUtility.UrlDecode(dr["detail_score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));

                                if (detail_score.IndexOf("PwdProtection") > -1)
                                {
                                    detail_score = detail_score.Replace("PwdProtection", "密保校验得分");
                                }
                                if (detail_score.IndexOf("CertifiedId") > -1)
                                {
                                    detail_score = detail_score.Replace("CertifiedId", "证件号校验得分");
                                }
                                if (detail_score.IndexOf("bind_email") > -1)
                                {
                                    detail_score = detail_score.Replace("bind_email", "绑定邮箱校验得分");
                                }
                                if (detail_score.IndexOf("bind_mobile") > -1)
                                {
                                    detail_score = detail_score.Replace("bind_mobile", "绑定手机校验得分");
                                }
                                if (detail_score.IndexOf("QQReceipt") > -1)
                                {
                                    detail_score = detail_score.Replace("QQReceipt", "QQ申诉回执号得分");
                                }
                                if (detail_score.IndexOf("CertifiedBankCard") > -1)
                                {
                                    detail_score = detail_score.Replace("CertifiedBankCard", "实名认证银行卡号校验得分");
                                }
                                if (detail_score.IndexOf("CreditCardPayHist") > -1)
                                {
                                    detail_score = detail_score.Replace("CreditCardPayHist", "信用卡还款信息校验得分");
                                }
                                if (detail_score.IndexOf("WithdrawHist") > -1) //andrew 20110419
                                {
                                    detail_score = detail_score.Replace("WithdrawHist", "提现记录得分");
                                }

                                dr["detail_score"] = detail_score;

                            }
                            catch
                            { }
                        }

                        string tmp = dr["FType"].ToString();
                        if (tmp == "1")    //目前只开放找回支付密码得分超标准分免审核，以后会慢慢开放的
                        {
                            dr["FTypeName"] = "找回密码";

                            if (dr["clear_pps"].ToString() == "0") //不清空
                            {
                                string Sql = "uid=" + fuid;
                                string errMsg = "";
                                string answer1 = CommQuery.GetOneResultFromICE(Sql, CommQuery.QUERY_USERINFO, "Fanswer1", out errMsg);
                                if (answer1 != null && answer1.Trim() != "")
                                {
                                    if (dr["answer1"].ToString().Trim() == answer1.ToString().Trim())
                                        dr["labIsAnswer"] = "答对了";
                                    else
                                        dr["labIsAnswer"] = "答错了";
                                }
                            }

                            try
                            {
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "2")
                        {
                            dr["FTypeName"] = "修改姓名";
                            try
                            {
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "3")
                        {
                            dr["FTypeName"] = "修改公司名";
                        }
                        else if (tmp == "4")
                        {
                            dr["FTypeName"] = "注销帐号";
                        }
                        else if (tmp == "5")// andrew 超标准分免审 20110419
                        {
                            dr["FTypeName"] = "完整注册用户更换关联手机";

                            try
                            {
                                //IVR外呼专用furion  因为有高分单不会被领单,所以这里不用改动.
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "6")//andrew 20110419
                        {
                            dr["FTypeName"] = "简化注册用户更换绑定手机";
                            try
                            {
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "7")
                        {
                            dr["FTypeName"] = "更换证件号";
                            try
                            {
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "9")
                        {
                            dr["FTypeName"] = "手机令牌";
                        }
                        else if (tmp == "10")
                        {
                            dr["FTypeName"] = "第三方令牌";
                        }
                        else if (tmp == "0")
                        {
                            dr["FTypeName"] = "财付盾解绑";
                        }

                        tmp = dr["FState"].ToString();
                        if (tmp == "0")
                        {
                            dr["FStateName"] = "未处理";
                        }
                        else if (tmp == "1")
                        {
                            dr["FStateName"] = "申诉成功";
                        }
                        else if (tmp == "2")
                        {
                            dr["FStateName"] = "申诉失败";
                        }
                        else if (tmp == "3")
                        {
                            dr["FStateName"] = "大额待复核";
                        }
                        else if (tmp == "4")
                        {
                            dr["FStateName"] = "直接转后台";
                        }
                        else if (tmp == "5")
                        {
                            dr["FStateName"] = "异常转后台";
                        }
                        else if (tmp == "6")
                        {
                            dr["FStateName"] = "发邮件失败";
                        }
                        else if (tmp == "7")
                        {
                            dr["FStateName"] = "已删除";
                        }
                        else if (tmp == "8")
                        {
                            dr["FStateName"] = "已领单";
                        }
                        else if (tmp == "9")
                        {
                            dr["FStateName"] = "短信撤销申诉";
                        }
                    }
                    catch (System.Exception ex)
                    {

                    }
                }

                return true;
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }
        
        //20131107 lxl
        //分库表的处理方法，处理结果后的结果与旧表一样
        public static bool HandleParameterByDBTB(DataSet ds, bool haveimg)
        {
            //加入一个证书库连接.
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CRT"));
            try
            {
                ds.Tables[0].Columns.Add("FTypeName", typeof(String));
                ds.Tables[0].Columns.Add("FStateName", typeof(String));

                ds.Tables[0].Columns.Add("cre_id", typeof(string)); //证件号码
                ds.Tables[0].Columns.Add("cre_type", typeof(string)); //证件类型
                ds.Tables[0].Columns.Add("cre_image", typeof(string)); //图片
                ds.Tables[0].Columns.Add("clear_pps", typeof(string)); //1为清除密保
                ds.Tables[0].Columns.Add("reason", typeof(string)); //原因
                ds.Tables[0].Columns.Add("old_name", typeof(string));
                ds.Tables[0].Columns.Add("new_name", typeof(string));
                ds.Tables[0].Columns.Add("old_company", typeof(string));
                ds.Tables[0].Columns.Add("new_company", typeof(string));

                //更换手机暂时加入一个字段,原请求证件地址
                ds.Tables[0].Columns.Add("cre_image2", typeof(string)); //图片
                ds.Tables[0].Columns.Add("question1", typeof(string)); //密保1
                ds.Tables[0].Columns.Add("answer1", typeof(string)); //答案1
                ds.Tables[0].Columns.Add("cont_type", typeof(string)); // 1: 手机 2: email//cont_type=1时,mobile_no有效 cont_type=2时,femail有效
                ds.Tables[0].Columns.Add("mobile_no", typeof(string));
                ds.Tables[0].Columns.Add("labIsAnswer", typeof(string)); //是否答对密保
                ds.Tables[0].Columns.Add("client_ip", typeof(string)); //申诉人IP
                ds.Tables[0].Columns.Add("score", typeof(string)); //实际得分
                ds.Tables[0].Columns.Add("standard_score", typeof(string)); //免审核标准分
                ds.Tables[0].Columns.Add("detail_score", typeof(string)); //得分明细
                ds.Tables[0].Columns.Add("IsPass", typeof(string)); //结果
                ds.Tables[0].Columns.Add("new_cre_id", typeof(string)); //新身份证
                ds.Tables[0].Columns.Add("risk_result", typeof(string)); //风控标记
                ds.Tables[0].Columns.Add("from", typeof(string)); //申诉渠道标记（手机）

                // 风控冻结要求加入字段如下：
                /*
                ds.Tables[0].Columns.Add("email",typeof(string));		// 邮箱
                ds.Tables[0].Columns.Add("pkey",typeof(string));		// 验证邮件连接时传的PKEY
                ds.Tables[0].Columns.Add("contact_no",typeof(string));	// 用户联系电话
                ds.Tables[0].Columns.Add("otherImage1",typeof(string));	// 其他图片1
                ds.Tables[0].Columns.Add("otherImage2",typeof(string));	// 其他图片2
                ds.Tables[0].Columns.Add("otherImage3",typeof(string));	// 其他图片3
                ds.Tables[0].Columns.Add("otherImage4",typeof(string));	// 其他图片4
                ds.Tables[0].Columns.Add("otherImage5",typeof(string));	// 其他图片5
                ds.Tables[0].Columns.Add("rec_cftadd",typeof(string));	// 最近使用财付通的地址
                ds.Tables[0].Columns.Add("prove_banlance_image",typeof(string));	// 余额资金来源证明
                ds.Tables[0].Columns.Add("DC_timeaddress",typeof(string));	// 最近安装数字证书的时间和地址
                ds.Tables[0].Columns.Add("mobile",typeof(string));		// 绑定的手机号
                */


                da.OpenConn();

                // 2012/4/28 修改，因为目前如果其中一个元素查找失败(可能是由于该帐号已经注销)，则会导致后面的dataRow查询失败！
                // 所以增加一个try catch模块，忽略查询失败的记录
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    try
                    {
                        dr["cre_id"] = "";
                        dr["cre_type"] = "";
                        dr["cre_image"] = "";
                        dr["clear_pps"] = "0";
                        dr["reason"] = "";
                        dr["old_name"] = "";
                        dr["new_name"] = "";
                        dr["old_company"] = "";
                        dr["new_company"] = "";
                        dr["question1"] = "";
                        dr["answer1"] = "";
                        dr["cont_type"] = "";
                        dr["mobile_no"] = "";
                        dr["labIsAnswer"] = "";
                        dr["client_ip"] = "";
                        dr["score"] = "";
                        dr["standard_score"] = "";
                        dr["detail_score"] = "";
                        dr["IsPass"] = "";
                        dr["new_cre_id"] = "";
                        dr["risk_result"] = "";
                        dr["from"] = "";

                        string fuin = dr["FUin"].ToString();

                        string fuid = PublicRes.ConvertToFuid(fuin);
                        if (fuid == null || fuid.Trim() == "")
                        {
                            // 2012/4/28 修改，用户注销之后，应该是查不到相应的UID的
                            continue;
                            //return false;
                        }

                        string strSql = "select Fattach from " + PublicRes.GetTName("cft_digit_crt", "t_user_attr", fuid)
                            + " where Fuid=" + fuid + " and Fattr=1";

                        string strtmp = QueryInfo.GetString(da.GetOneResult(strSql));
                        dr["cre_image2"] = strtmp;

                        //string parameter = dr["FParameter"].ToString();
                        if (haveimg)
                        {
                                    dr["cre_id"] = getCgiString(dr["FCreId"].ToString());

                                    dr["cre_type"] = getCgiString(dr["FCreType"].ToString());

                                    //dr["cre_image"] = getCgiString(dr["FCreImg1"].ToString()) + "|" + getCgiString(dr["FCreImg2"].ToString());
                                   //20141020 echo 将身份证反面改为资金来源
                                    dr["cre_image"] = getCgiString(dr["FCreImg1"].ToString()) + "|" + getCgiString(dr["FProveBanlanceImage"].ToString());

                                    dr["clear_pps"] = getCgiString(dr["FClearPps"].ToString());

                                    dr["reason"] = getCgiString(dr["FAppealReason"].ToString());

                                    dr["old_name"] = getCgiString(dr["FOldName"].ToString());

                                    dr["new_name"] = getCgiString(dr["FNewName"].ToString());

                                    dr["old_company"] = getCgiString(dr["FOldCompanyName"].ToString());

                                    dr["new_company"] = getCgiString(dr["FNewCompanyName"].ToString());

                                    dr["question1"] = getCgiString(dr["FMbQuestion"].ToString());

                                    dr["answer1"] = getCgiString(dr["FMbAnswer"].ToString());

                                    dr["cont_type"] = getCgiString(dr["FContType"].ToString());

                                    dr["mobile_no"] = getCgiString(dr["FReservedMobile"].ToString());

                                    dr["client_ip"] = getCgiString(dr["FIp"].ToString());

                                    dr["score"] = getCgiString(dr["FAppealScore"].ToString()); ;

                                    dr["standard_score"] = getCgiString(dr["FStandardScore"].ToString());

                                    dr["detail_score"] = getCgiString(dr["FDetailScore"].ToString());

                                    dr["new_cre_id"] = getCgiString(dr["FNewCreId"].ToString());

                                    string risk_result = getCgiString(dr["FRiskState"].ToString()); 
                                    if (risk_result == "0")
                                        dr["risk_result"] = "";
                                    else if (risk_result == "1")
                                        dr["risk_result"] = "风控异常单无需人工回访用户";
                                    else if (risk_result == "2")
                                        dr["risk_result"] = "风控异常单需人工回访用户";
                                    else
                                        dr["risk_result"] = risk_result;

                                    dr["from"] = getCgiString(dr["FChanel"].ToString());
                          
                        }

                        if (dr["detail_score"].ToString() != "")
                        {
                            try
                            {
                                string detail_score = System.Web.HttpUtility.UrlDecode(dr["detail_score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));

                                if (detail_score.IndexOf("PwdProtection") > -1)
                                {
                                    detail_score = detail_score.Replace("PwdProtection", "密保校验得分");
                                }
                                if (detail_score.IndexOf("CertifiedId") > -1)
                                {
                                    detail_score = detail_score.Replace("CertifiedId", "证件号校验得分");
                                }
                                if (detail_score.IndexOf("bind_email") > -1)
                                {
                                    detail_score = detail_score.Replace("bind_email", "绑定邮箱校验得分");
                                }
                                if (detail_score.IndexOf("bind_mobile") > -1)
                                {
                                    detail_score = detail_score.Replace("bind_mobile", "绑定手机校验得分");
                                }
                                if (detail_score.IndexOf("QQReceipt") > -1)
                                {
                                    detail_score = detail_score.Replace("QQReceipt", "QQ申诉回执号得分");
                                }
                                if (detail_score.IndexOf("CertifiedBankCard") > -1)
                                {
                                    detail_score = detail_score.Replace("CertifiedBankCard", "实名认证银行卡号校验得分");
                                }
                                if (detail_score.IndexOf("CreditCardPayHist") > -1)
                                {
                                    detail_score = detail_score.Replace("CreditCardPayHist", "信用卡还款信息校验得分");
                                }
                                if (detail_score.IndexOf("WithdrawHist") > -1) //andrew 20110419
                                {
                                    detail_score = detail_score.Replace("WithdrawHist", "提现记录得分");
                                }

                                dr["detail_score"] = detail_score;

                            }
                            catch
                            { }
                        }

                        string tmp = dr["FType"].ToString();
                        if (tmp == "1")    //目前只开放找回支付密码得分超标准分免审核，以后会慢慢开放的
                        {
                            dr["FTypeName"] = "找回密码";

                            if (dr["clear_pps"].ToString() == "0") //不清空
                            {
                                string Sql = "uid=" + fuid;
                                string errMsg = "";
                                string answer1 = CommQuery.GetOneResultFromICE(Sql, CommQuery.QUERY_USERINFO, "Fanswer1", out errMsg);
                                if (answer1 != null && answer1.Trim() != "")
                                {
                                    if (dr["answer1"].ToString().Trim() == answer1.ToString().Trim())
                                        dr["labIsAnswer"] = "答对了";//是否答对密保
                                    else
                                        dr["labIsAnswer"] = "答错了";
                                }
                            }

                            try
                            {
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "2")
                        {
                            dr["FTypeName"] = "修改姓名";
                            try
                            {
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "3")
                        {
                            dr["FTypeName"] = "修改公司名";
                        }
                        else if (tmp == "4")
                        {
                            dr["FTypeName"] = "注销帐号";
                        }
                        else if (tmp == "5")// andrew 超标准分免审 20110419
                        {
                            dr["FTypeName"] = "完整注册用户更换关联手机";

                            try
                            {
                                //IVR外呼专用furion  因为有高分单不会被领单,所以这里不用改动.
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "6")//andrew 20110419
                        {
                            dr["FTypeName"] = "简化注册用户更换绑定手机";
                            try
                            {
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "7")
                        {
                            dr["FTypeName"] = "更换证件号";
                            try
                            {
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "9")
                        {
                            dr["FTypeName"] = "手机令牌";
                        }
                        else if (tmp == "10")
                        {
                            dr["FTypeName"] = "第三方令牌";
                        }
                        else if (tmp == "0")
                        {
                            dr["FTypeName"] = "财付盾解绑";
                        }

                        tmp = dr["FState"].ToString();
                        if (tmp == "0")
                        {
                            dr["FStateName"] = "未处理";
                        }
                        else if (tmp == "1")
                        {
                            dr["FStateName"] = "申诉成功";
                        }
                        else if (tmp == "2")
                        {
                            dr["FStateName"] = "申诉失败";
                        }
                        else if (tmp == "3")
                        {
                            dr["FStateName"] = "大额待复核";
                        }
                        else if (tmp == "4")
                        {
                            dr["FStateName"] = "直接转后台";
                        }
                        else if (tmp == "5")
                        {
                            dr["FStateName"] = "异常转后台";
                        }
                        else if (tmp == "6")
                        {
                            dr["FStateName"] = "发邮件失败";
                        }
                        else if (tmp == "7")
                        {
                            dr["FStateName"] = "已删除";
                        }
                        else if (tmp == "8")
                        {
                            dr["FStateName"] = "已领单";
                        }
                        else if (tmp == "9")
                        {
                            dr["FStateName"] = "短信撤销申诉";
                        }
                    }
                    catch (System.Exception ex)
                    {

                    }
                }

                return true;
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }

        //20131107 lxl
        //分库表的处理方法，处理结果后的结果与旧表一样
        public static bool HandleParameterByDBTBList(DataSet ds, bool haveimg)
        {
            //加入一个证书库连接.
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CRT"));
            MySqlAccess da2 = new MySqlAccess(PublicRes.GetConnString("CFTNEW"));
        
            try
            {
                ds.Tables[0].Columns.Add("FTypeName", typeof(String));
                ds.Tables[0].Columns.Add("FStateName", typeof(String));

                ds.Tables[0].Columns.Add("cre_id", typeof(string)); //证件号码
                ds.Tables[0].Columns.Add("cre_type", typeof(string)); //证件类型
                ds.Tables[0].Columns.Add("cre_image", typeof(string)); //图片
                ds.Tables[0].Columns.Add("clear_pps", typeof(string)); //1为清除密保
                ds.Tables[0].Columns.Add("reason", typeof(string)); //原因
                ds.Tables[0].Columns.Add("old_name", typeof(string));
                ds.Tables[0].Columns.Add("new_name", typeof(string));
                ds.Tables[0].Columns.Add("old_company", typeof(string));
                ds.Tables[0].Columns.Add("new_company", typeof(string));

                //更换手机暂时加入一个字段,原请求证件地址
                ds.Tables[0].Columns.Add("cre_image2", typeof(string)); //图片
                ds.Tables[0].Columns.Add("question1", typeof(string)); //密保1
                ds.Tables[0].Columns.Add("answer1", typeof(string)); //答案1
                ds.Tables[0].Columns.Add("cont_type", typeof(string)); // 1: 手机 2: email//cont_type=1时,mobile_no有效 cont_type=2时,femail有效
                ds.Tables[0].Columns.Add("mobile_no", typeof(string));
                ds.Tables[0].Columns.Add("labIsAnswer", typeof(string)); //是否答对密保
                ds.Tables[0].Columns.Add("client_ip", typeof(string)); //申诉人IP
                ds.Tables[0].Columns.Add("score", typeof(string)); //实际得分
                ds.Tables[0].Columns.Add("standard_score", typeof(string)); //免审核标准分
                ds.Tables[0].Columns.Add("detail_score", typeof(string)); //得分明细
                ds.Tables[0].Columns.Add("IsPass", typeof(string)); //结果
                ds.Tables[0].Columns.Add("new_cre_id", typeof(string)); //新身份证
                ds.Tables[0].Columns.Add("risk_result", typeof(string)); //风控标记
                ds.Tables[0].Columns.Add("from", typeof(string)); //申诉渠道标记（手机）

                // 风控冻结要求加入字段如下：
                /*
                ds.Tables[0].Columns.Add("email",typeof(string));		// 邮箱
                ds.Tables[0].Columns.Add("pkey",typeof(string));		// 验证邮件连接时传的PKEY
                ds.Tables[0].Columns.Add("contact_no",typeof(string));	// 用户联系电话
                ds.Tables[0].Columns.Add("otherImage1",typeof(string));	// 其他图片1
                ds.Tables[0].Columns.Add("otherImage2",typeof(string));	// 其他图片2
                ds.Tables[0].Columns.Add("otherImage3",typeof(string));	// 其他图片3
                ds.Tables[0].Columns.Add("otherImage4",typeof(string));	// 其他图片4
                ds.Tables[0].Columns.Add("otherImage5",typeof(string));	// 其他图片5
                ds.Tables[0].Columns.Add("rec_cftadd",typeof(string));	// 最近使用财付通的地址
                ds.Tables[0].Columns.Add("prove_banlance_image",typeof(string));	// 余额资金来源证明
                ds.Tables[0].Columns.Add("DC_timeaddress",typeof(string));	// 最近安装数字证书的时间和地址
                ds.Tables[0].Columns.Add("mobile",typeof(string));		// 绑定的手机号
                */


                da.OpenConn();

                // 2012/4/28 修改，因为目前如果其中一个元素查找失败(可能是由于该帐号已经注销)，则会导致后面的dataRow查询失败！
                // 所以增加一个try catch模块，忽略查询失败的记录
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    try
                    {
                        dr["cre_id"] = "";
                        dr["cre_type"] = "";
                        dr["cre_image"] = "";
                        dr["clear_pps"] = "0";
                        dr["reason"] = "";
                        dr["old_name"] = "";
                        dr["new_name"] = "";
                        dr["old_company"] = "";
                        dr["new_company"] = "";
                        dr["question1"] = "";
                        dr["answer1"] = "";
                        dr["cont_type"] = "";
                        dr["mobile_no"] = "";
                        dr["labIsAnswer"] = "";
                        dr["client_ip"] = "";
                        dr["score"] = "";
                        dr["standard_score"] = "";
                        dr["detail_score"] = "";
                        dr["IsPass"] = "";
                        dr["new_cre_id"] = "";
                        dr["risk_result"] = "";
                        dr["from"] = "";

                        string fuin = dr["FUin"].ToString();

                        string fuid = PublicRes.ConvertToFuid(fuin);
                        if (fuid == null || fuid.Trim() == "")
                        {
                            // 2012/4/28 修改，用户注销之后，应该是查不到相应的UID的
                            continue;
                            //return false;
                        }

                        string strSql = "select Fattach from " + PublicRes.GetTName("cft_digit_crt", "t_user_attr", fuid)
                            + " where Fuid=" + fuid + " and Fattr=1";

                        string strtmp = QueryInfo.GetString(da.GetOneResult(strSql));
                        dr["cre_image2"] = strtmp;

                        //查询出FCreId等
                        da2.OpenConn();
                        strSql = "select * from " + dr["DBName"].ToString() + "." + dr["tableName"].ToString() + " where FID='" + dr["Fid"]+"'";
                        DataTable dtByFid = da2.GetTable(strSql);
                       
                        if (dtByFid != null && dtByFid.Rows.Count > 0)
                        {
                            DataRow row = dtByFid.Rows[0];
                            if (haveimg)
                            {
                                dr["cre_id"] = getCgiString(row["FCreId"].ToString());

                                dr["cre_type"] = getCgiString(row["FCreType"].ToString());

                                if (getCgiString(row["FCreImg2"].ToString()) != "")
                                {
                                    dr["cre_image"] = getCgiString(row["FCreImg1"].ToString()) + "|" + getCgiString(row["FCreImg2"].ToString());
                                }
                                else
                                {
                                    dr["cre_image"] = getCgiString(row["FCreImg1"].ToString());
                                }

                                dr["clear_pps"] = getCgiString(row["FClearPps"].ToString());

                                dr["reason"] = getCgiString(row["FAppealReason"].ToString());

                                dr["old_name"] = getCgiString(row["FOldName"].ToString());

                                dr["new_name"] = getCgiString(row["FNewName"].ToString());

                                dr["old_company"] = getCgiString(row["FOldCompanyName"].ToString());

                                dr["new_company"] = getCgiString(row["FNewCompanyName"].ToString());

                                dr["question1"] = getCgiString(row["FMbQuestion"].ToString());

                                dr["answer1"] = getCgiString(row["FMbAnswer"].ToString());

                                dr["cont_type"] = getCgiString(row["FContType"].ToString());

                                dr["mobile_no"] = getCgiString(row["FMobile"].ToString());

                                dr["client_ip"] = getCgiString(row["FIp"].ToString());

                                dr["score"] = getCgiString(row["FAppealScore"].ToString()); ;

                                dr["standard_score"] = getCgiString(row["FStandardScore"].ToString());

                                dr["detail_score"] = getCgiString(row["FDetailScore"].ToString());

                                dr["new_cre_id"] = getCgiString(row["FNewCreId"].ToString());

                                string risk_result = getCgiString(row["FRiskState"].ToString());
                                if (risk_result == "0")
                                    dr["risk_result"] = "";
                                else if (risk_result == "1")
                                    dr["risk_result"] = "风控异常单无需人工回访用户";
                                else if (risk_result == "2")
                                    dr["risk_result"] = "风控异常单需人工回访用户";
                                else
                                    dr["risk_result"] = risk_result;

                                dr["from"] = getCgiString(row["FChanel"].ToString());

                            }
                        }

                        if (dr["detail_score"].ToString() != "")
                        {
                            try
                            {
                                string detail_score = System.Web.HttpUtility.UrlDecode(dr["detail_score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));

                                if (detail_score.IndexOf("PwdProtection") > -1)
                                {
                                    detail_score = detail_score.Replace("PwdProtection", "密保校验得分");
                                }
                                if (detail_score.IndexOf("CertifiedId") > -1)
                                {
                                    detail_score = detail_score.Replace("CertifiedId", "证件号校验得分");
                                }
                                if (detail_score.IndexOf("bind_email") > -1)
                                {
                                    detail_score = detail_score.Replace("bind_email", "绑定邮箱校验得分");
                                }
                                if (detail_score.IndexOf("bind_mobile") > -1)
                                {
                                    detail_score = detail_score.Replace("bind_mobile", "绑定手机校验得分");
                                }
                                if (detail_score.IndexOf("QQReceipt") > -1)
                                {
                                    detail_score = detail_score.Replace("QQReceipt", "QQ申诉回执号得分");
                                }
                                if (detail_score.IndexOf("CertifiedBankCard") > -1)
                                {
                                    detail_score = detail_score.Replace("CertifiedBankCard", "实名认证银行卡号校验得分");
                                }
                                if (detail_score.IndexOf("CreditCardPayHist") > -1)
                                {
                                    detail_score = detail_score.Replace("CreditCardPayHist", "信用卡还款信息校验得分");
                                }
                                if (detail_score.IndexOf("WithdrawHist") > -1) //andrew 20110419
                                {
                                    detail_score = detail_score.Replace("WithdrawHist", "提现记录得分");
                                }

                                dr["detail_score"] = detail_score;

                            }
                            catch
                            { }
                        }

                        string tmp = dr["FType"].ToString();
                        if (tmp == "1")    //目前只开放找回支付密码得分超标准分免审核，以后会慢慢开放的
                        {
                            dr["FTypeName"] = "找回密码";

                            if (dr["clear_pps"].ToString() == "0") //不清空
                            {
                                string Sql = "uid=" + fuid;
                                string errMsg = "";
                                string answer1 = CommQuery.GetOneResultFromICE(Sql, CommQuery.QUERY_USERINFO, "Fanswer1", out errMsg);
                                if (answer1 != null && answer1.Trim() != "")
                                {
                                    if (dr["answer1"].ToString().Trim() == answer1.ToString().Trim())
                                        dr["labIsAnswer"] = "答对了";//是否答对密保
                                    else
                                        dr["labIsAnswer"] = "答错了";
                                }
                            }

                            try
                            {
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "2")
                        {
                            dr["FTypeName"] = "修改姓名";
                            try
                            {
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "3")
                        {
                            dr["FTypeName"] = "修改公司名";
                        }
                        else if (tmp == "4")
                        {
                            dr["FTypeName"] = "注销帐号";
                        }
                        else if (tmp == "5")// andrew 超标准分免审 20110419
                        {
                            dr["FTypeName"] = "完整注册用户更换关联手机";

                            try
                            {
                                //IVR外呼专用furion  因为有高分单不会被领单,所以这里不用改动.
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "6")//andrew 20110419
                        {
                            dr["FTypeName"] = "简化注册用户更换绑定手机";
                            try
                            {
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "7")
                        {
                            dr["FTypeName"] = "更换证件号";
                            try
                            {
                                if (Convert.ToInt32(dr["score"].ToString()) >= Convert.ToInt32(dr["standard_score"].ToString()))
                                    dr["IsPass"] = "Y";
                                else
                                    dr["IsPass"] = "N";
                            }
                            catch
                            { }
                        }
                        else if (tmp == "9")
                        {
                            dr["FTypeName"] = "手机令牌";
                        }
                        else if (tmp == "10")
                        {
                            dr["FTypeName"] = "第三方令牌";
                        }
                        else if (tmp == "0")
                        {
                            dr["FTypeName"] = "财付盾解绑";
                        }

                        tmp = dr["FState"].ToString();
                        if (tmp == "0")
                        {
                            dr["FStateName"] = "未处理";
                        }
                        else if (tmp == "1")
                        {
                            dr["FStateName"] = "申诉成功";
                        }
                        else if (tmp == "2")
                        {
                            dr["FStateName"] = "申诉失败";
                        }
                        else if (tmp == "3")
                        {
                            dr["FStateName"] = "大额待复核";
                        }
                        else if (tmp == "4")
                        {
                            dr["FStateName"] = "直接转后台";
                        }
                        else if (tmp == "5")
                        {
                            dr["FStateName"] = "异常转后台";
                        }
                        else if (tmp == "6")
                        {
                            dr["FStateName"] = "发邮件失败";
                        }
                        else if (tmp == "7")
                        {
                            dr["FStateName"] = "已删除";
                        }
                        else if (tmp == "8")
                        {
                            dr["FStateName"] = "已领单";
                        }
                        else if (tmp == "9")
                        {
                            dr["FStateName"] = "短信撤销申诉";
                        }
                    }
                    catch (System.Exception ex)
                    {

                    }
                }

                return true;
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
                da2.Dispose();
            }
        }

        public static bool HandleParameter_ForControledFreeze(DataSet ds, bool haveimg)
        {
            //加入一个证书库连接.
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CRT"));
            try
            {
                ds.Tables[0].Columns.Add("FTypeName", typeof(String));
                ds.Tables[0].Columns.Add("FStateName", typeof(String));

                ds.Tables[0].Columns.Add("clear_pps", typeof(string));
                ds.Tables[0].Columns.Add("uin", typeof(string));
                ds.Tables[0].Columns.Add("uid", typeof(string));
                ds.Tables[0].Columns.Add("cre_id", typeof(string)); //证件号码
                ds.Tables[0].Columns.Add("cre_type", typeof(string)); //证件类型
                ds.Tables[0].Columns.Add("cre_image", typeof(string)); //图片
                ds.Tables[0].Columns.Add("reason", typeof(string)); //原因

                //更换手机暂时加入一个字段,原请求证件地址
                ds.Tables[0].Columns.Add("client_ip", typeof(string)); //申诉人IP
                ds.Tables[0].Columns.Add("risk_result", typeof(string)); //风控标记

                // 风控冻结要求加入字段如下：
                ds.Tables[0].Columns.Add("email", typeof(string));		// 邮箱
                ds.Tables[0].Columns.Add("pkey", typeof(string));		// 验证邮件连接时传的PKEY
                ds.Tables[0].Columns.Add("contact_no", typeof(string));	// 用户联系电话
                ds.Tables[0].Columns.Add("otherImage1", typeof(string));	// 其他图片1
                ds.Tables[0].Columns.Add("otherImage2", typeof(string));	// 其他图片2
                ds.Tables[0].Columns.Add("otherImage3", typeof(string));	// 其他图片3
                ds.Tables[0].Columns.Add("otherImage4", typeof(string));	// 其他图片4
                ds.Tables[0].Columns.Add("otherImage5", typeof(string));	// 其他图片5
                ds.Tables[0].Columns.Add("rec_cftadd", typeof(string));	// 最近使用财付通的地址
                ds.Tables[0].Columns.Add("prove_banlance_image", typeof(string));	// 余额资金来源证明
                ds.Tables[0].Columns.Add("DC_timeaddress", typeof(string));	// 最近安装数字证书的时间和地址
                ds.Tables[0].Columns.Add("mobile", typeof(string));		// 绑定的手机号


                da.OpenConn();

                // 2012/4/28 修改，因为目前如果其中一个元素查找失败(可能是由于该帐号已经注销)，则会导致后面的dataRow查询失败！
                // 所以增加一个try catch模块，忽略查询失败的记录
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    try
                    {
                        dr["uin"] = "";
                        dr["uid"] = "";
                        dr["cre_id"] = "";
                        dr["cre_type"] = "";
                        dr["cre_image"] = "";
                        dr["clear_pps"] = "0";
                        dr["reason"] = "";

                        dr["email"] = "";
                        dr["pkey"] = "";
                        dr["contact_no"] = "";
                        dr["otherImage1"] = "";
                        dr["otherImage2"] = "";
                        dr["otherImage3"] = "";
                        dr["otherImage4"] = "";
                        dr["otherImage5"] = "";
                        dr["rec_cftadd"] = "";
                        dr["prove_banlance_image"] = "";
                        dr["DC_timeaddress"] = "";
                        dr["mobile"] = "";

                        string fuin = dr["FUin"].ToString();

                        string fuid = PublicRes.ConvertToFuid(fuin);
                        if (fuid == null || fuid.Trim() == "")
                        {
                            // 2012/4/28 修改，用户注销之后，应该是查不到相应的UID的
                            continue;
                            //return false;
                        }

                        //string parameter = dr["FParameter"].ToString();
                        if (haveimg)
                        {
                            string parameter = GetOneParameter(dr["Fid"].ToString());

                            string[] paramlist = parameter.Split('&');

                            foreach (string param in paramlist)
                            {
                                if (param.StartsWith("uin="))
                                {
                                    dr["uin"] = getCgiString(param.Replace("uin=", ""));
                                }
                                else if (param.StartsWith("uid="))
                                {
                                    dr["uid"] = getCgiString(param.Replace("uid=", ""));
                                }
                                if (param.StartsWith("cre_id="))
                                {
                                    dr["cre_id"] = getCgiString(param.Replace("cre_id=", ""));
                                }
                                else if (param.StartsWith("cre_type="))
                                {
                                    dr["cre_type"] = getCgiString(param.Replace("cre_type=", ""));
                                }
                                else if (param.StartsWith("cre_image="))
                                {
                                    dr["cre_image"] = getCgiString(param.Replace("cre_image=", ""));
                                }
                                else if (param.StartsWith("clear_pps="))
                                {
                                    dr["clear_pps"] = getCgiString(param.Replace("clear_pps=", ""));
                                }
                                else if (param.StartsWith("email"))
                                {
                                    dr["email"] = getCgiString(param.Replace("email=", ""));
                                }
                                else if (param.StartsWith("reason="))
                                {
                                    dr["reason"] = getCgiString(param.Replace("reason=", ""));
                                }

                                else if (param.StartsWith("pkey="))
                                {
                                    dr["pkey"] = getCgiString(param.Replace("pkey=", ""));
                                }
                                else if (param.StartsWith("mobile="))
                                {
                                    dr["mobile"] = getCgiString(param.Replace("mobile=", ""));
                                }
                                else if (param.StartsWith("contact_no="))
                                {
                                    dr["contact_no"] = getCgiString(param.Replace("contact_no=", ""));
                                }
                                else if (param.StartsWith("rec_cftadd="))
                                {
                                    dr["rec_cftadd"] = getCgiString(param.Replace("rec_cftadd=", ""));
                                }
                                else if (param.StartsWith("prove_banlance_image="))
                                {
                                    dr["prove_banlance_image"] = getCgiString(param.Replace("prove_banlance_image=", ""));
                                }
                                else if (param.StartsWith("other1_image="))
                                {
                                    dr["otherImage1"] = getCgiString(param.Replace("other1_image=", ""));
                                }
                                else if (param.StartsWith("other2_image="))
                                {
                                    dr["otherImage2"] = getCgiString(param.Replace("other2_image=", ""));
                                }
                                else if (param.StartsWith("other3_image="))
                                {
                                    dr["otherImage3"] = getCgiString(param.Replace("other3_image=", ""));
                                }
                                else if (param.StartsWith("other4_image="))
                                {
                                    dr["otherImage4"] = getCgiString(param.Replace("other4_image=", ""));
                                }
                                else if (param.StartsWith("other5_image="))
                                {
                                    dr["otherImage5"] = getCgiString(param.Replace("other5_image=", ""));
                                }
                                else if (param.StartsWith("DC_timeaddress="))
                                {
                                    dr["DC_timeaddress"] = getCgiString(param.Replace("DC_timeaddress=", ""));
                                }
                            }
                        }

                        string tmp = dr["FState"].ToString();
                        if (tmp == "0")
                        {
                            dr["FStateName"] = "未处理";
                        }
                        else if (tmp == "1")
                        {
                            dr["FStateName"] = "申诉成功";
                        }
                        else if (tmp == "2")
                        {
                            dr["FStateName"] = "申诉失败";
                        }
                        else if (tmp == "3")
                        {
                            dr["FStateName"] = "大额待复核";
                        }
                        else if (tmp == "4")
                        {
                            dr["FStateName"] = "直接转后台";
                        }
                        else if (tmp == "5")
                        {
                            dr["FStateName"] = "异常转后台";
                        }
                        else if (tmp == "6")
                        {
                            dr["FStateName"] = "发邮件失败";
                        }
                        else if (tmp == "7")
                        {
                            dr["FStateName"] = "已删除";
                        }
                        else if (tmp == "8")
                        {
                            dr["FStateName"] = "已领单";
                        }
                        else if (tmp == "9")
                        {
                            dr["FStateName"] = "短信撤销申诉";
                        }
                    }
                    catch (Exception ex)
                    {
                        string str = ex.Message;
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                string str = ex.Message;
                return false;
            }

            return true;
        }

        public static void InputAppealNumber(string User, string Type, string OperationType)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            try
            {
                da.OpenConn();
                string sql = "select count(1) from t_tenpay_appeal_kf_total where User='" + User + "' and OperationDay='" + DateTime.Today.ToString("yyyy-MM-dd") + "' and OperationType='" + OperationType + "'";
                if (da.GetOneResult(sql) == "1")
                {
                    if (Type == "Success")
                        sql = "update t_tenpay_appeal_kf_total set SuccessNum = SuccessNum + 1 where User='" + User + "' and OperationDay='" + DateTime.Today.ToString("yyyy-MM-dd") + "' and OperationType='" + OperationType + "'";
                    else if (Type == "Fail")
                        sql = "update t_tenpay_appeal_kf_total set FailNum = FailNum + 1 where User='" + User + "' and OperationDay='" + DateTime.Today.ToString("yyyy-MM-dd") + "' and OperationType='" + OperationType + "'";
                    else if (Type == "Delete")
                        sql = "update t_tenpay_appeal_kf_total set DeleteNum = DeleteNum + 1 where User='" + User + "' and OperationDay='" + DateTime.Today.ToString("yyyy-MM-dd") + "' and OperationType='" + OperationType + "'";
                    else
                        sql = "update t_tenpay_appeal_kf_total set OtherNum = OtherNum + 1 where User='" + User + "' and OperationDay='" + DateTime.Today.ToString("yyyy-MM-dd") + "' and OperationType='" + OperationType + "'";
                }
                else
                {
                    if (Type == "Success")
                        sql = "insert into t_tenpay_appeal_kf_total(User,OperationDay,SuccessNum,OperationType) values('" + User + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "',1,'" + OperationType + "')";
                    else if (Type == "Fail")
                        sql = "insert into t_tenpay_appeal_kf_total(User,OperationDay,FailNum,OperationType) values('" + User + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "',1,'" + OperationType + "')";
                    else if (Type == "Delete")
                        sql = "insert into t_tenpay_appeal_kf_total(User,OperationDay,DeleteNum,OperationType) values('" + User + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "',1,'" + OperationType + "')";
                    else
                        sql = "insert into t_tenpay_appeal_kf_total(User,OperationDay,OtherNum,OperationType) values('" + User + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "',1,'" + OperationType + "')";
                }

                da.ExecSqlNum(sql);
            }
            catch
            {
                throw new Exception("记录处理统计失败！");
            }
            finally
            {
                da.Dispose();
            }
        }

        public CFTUserAppealClass(string fuin, string u_BeginTime, string u_EndTime, int fstate, int ftype, string QQType, string dotype, int SortType)
        {
            string strWhere = " where 1=1 ";

            if (fuin != null && fuin.Trim() != "")
            {
                strWhere += " and Fuin='" + fuin.Trim() + "' ";
            }

            if (u_BeginTime != null && u_BeginTime != "")
            {

                strWhere += " and FSubmitTime between '" + u_BeginTime + "' and '" + u_EndTime + "' ";
            }

            if (fstate != 99)
            {
                if (fstate == 10)   //直接申诉成功，即审核成功，且FCheckUser为system
                    strWhere += " and FState='1' and FCheckUser='system' ";
                else
                    strWhere += " and FState='" + fstate + "'  ";
            }


            if (ftype != 99)
            {
                strWhere += " and FType=" + ftype + " ";
            }
            else
            {
                // 8号申诉是风控冻结，有另外的处理入口，申诉处理暂不包含改类型的申诉
                strWhere += " and FType!=8 and FType!=19 ";
            }

            if (QQType == "0")    //"" 所有类型; "0" 非会员; "1" 普通会员; "2" VIP会员
            {
                strWhere += " and (FParameter not like '%vip_flag=1%') and (FParameter not like '%vip_flag=2%') ";
            }
            else if (QQType == "1")
            {
                strWhere += " and FParameter like '%vip_flag=1%' ";
            }
            else if (QQType == "2")
            {
                strWhere += " and FParameter like '%vip_flag=2%' ";
            }

            //增加高分单低分单查询
            if (dotype == "1")
            {
                //高分
                strWhere += " and FParameter like '%&AUTO_APPEAL=1%' ";
            }
            else if (dotype == "0")
            {
                //低分
                strWhere += " and FParameter not like '%&AUTO_APPEAL=1%' ";
            }
            
            if (SortType != 99)
            {
                if (SortType == 0)   //排序：时间小到大
                    strWhere += " order by FSubmitTime asc ";
                if (SortType == 1)   //排序：时间大到小
                    strWhere += " order by FSubmitTime desc ";
            }

            //lxl 20131116加两列tableName、DBName
            fstrSql = "select '' as DBName, '' as tableName,Fid,FType,Fuin,FSubmitTime,FState,FCheckTime,FReCheckTime,Fpicktime,FReCheckUser,FCheckInfo,FCheckUser,FComment,Femail from t_tenpay_appeal_trans "
                + strWhere;
            fstrSql_count = "select count(1) from t_tenpay_appeal_trans " + strWhere;
        }

        public CFTUserAppealClass(string fuin, string u_BeginTime, string u_EndTime, int fstate, int ftype, string QQType, string dotype, int SortType, bool mark)
        {
            string strWhere = " where 1=1 ";

            if (fuin != null && fuin.Trim() != "")
            {
                strWhere += " and Fuin='" + fuin.Trim() + "' ";
            }

            if (u_BeginTime != null && u_BeginTime != "")
            {

                strWhere += " and FSubmitTime between '" + u_BeginTime + "' and '" + u_EndTime + "' ";
            }

            if (fstate != 99)
            {
                if (fstate == 10)   //直接申诉成功，即审核成功，且FCheckUser为system
                    strWhere += " and FState='1' and FCheckUser='system' ";
                else
                    strWhere += " and FState='" + fstate + "'  ";
            }


            if (ftype != 99)
            {
                strWhere += " and FType=" + ftype + " ";
            }
            else
            {
                // 8号申诉是风控冻结，有另外的处理入口，申诉处理暂不包含改类型的申诉
                strWhere += " and FType!=8 ";
            }

            if (QQType == "0")    //"" 所有类型; 0 非会员; 1 普通会员; 2 VIP会员
            {
                strWhere += " and FVipFlag<>1 and FVipFlag<>2 ";
            }
            else if (QQType == "1")
            {
                strWhere += " and FVipFlag=1 ";
            }
            else if (QQType == "2")
            {
                strWhere += " and FVipFlag=2 ";
            }

            //增加高分单低分单查询
            if (dotype == "1")
            {
                //高分
                strWhere += " and FAutoAppeal=1 ";
            }
            else if (dotype == "0")
            {
                //低分
                strWhere += " and FAutoAppeal<>1 ";
            }
            fstrSql = "";
            fstrSql_count = "";
            string str = " Fid,FType,Fuin,FSubmitTime,FState,FCheckTime,FReCheckTime,Fpicktime,FReCheckUser,FCheckInfo,FCheckUser,FComment,Femail from db_appeal_";

            int yearBegin = DateTime.Parse(u_BeginTime).Year;
            int monBegin = DateTime.Parse(u_BeginTime).Month;
            int yearEnd = DateTime.Parse(u_EndTime).Year;
            int monEnd = DateTime.Parse(u_EndTime).Month;

            while (yearBegin <= yearEnd)
            {
                if (yearBegin < 2014)
                {
                    yearBegin++;
                    continue;
                }
                for (int i = 1; i <= 12; i++)//月份
                {
                    if (yearBegin == yearEnd && i > monEnd)//查到结束日期
                        break;
                    if (i < monBegin)//小于开始月份的数据不查
                        continue;

                    if (fstrSql != "")
                        fstrSql += "union all ";
                    if (i < 10)
                        fstrSql += " select 'db_appeal_" + yearBegin + "' as DBName,'t_tenpay_appeal_trans_" + "0" + i + "' as tableName," + str + yearBegin + ".t_tenpay_appeal_trans_" + "0" + i + strWhere;
                    else
                        fstrSql += " select 'db_appeal_" + yearBegin + "' as DBName,'t_tenpay_appeal_trans_" + i + "' as tableName," + str + yearBegin + ".t_tenpay_appeal_trans_" + i + strWhere;

                }
                yearBegin++;
                monBegin = 0;//换年份后，将初始月份至为0
            }
            fstrSql_count = "select count(1) from ( " + fstrSql + " ) as total";
        }

        public CFTUserAppealClass(string fuin, string u_BeginTime, string u_EndTime, int fstate, int ftype, string QQType, string dotype, int SortType, bool mark,string db,string tb)
        {
            string strWhere = " where 1=1 ";

            if (fuin != null && fuin.Trim() != "")
            {
                strWhere += " and Fuin='" + fuin.Trim() + "' ";
            }

            if (u_BeginTime != null && u_BeginTime != "")
            {

                strWhere += " and FSubmitTime between '" + u_BeginTime + "' and '" + u_EndTime + "' ";
            }

            if (fstate != 99)
            {
                if (fstate == 10)   //直接申诉成功，即审核成功，且FCheckUser为system
                    strWhere += " and FState='1' and FCheckUser='system' ";
                else
                    strWhere += " and FState='" + fstate + "'  ";
            }


            if (ftype != 99)
            {
                strWhere += " and FType=" + ftype + " ";
            }
            else
            {
                // 8号申诉是风控冻结，有另外的处理入口，申诉处理暂不包含改类型的申诉
                strWhere += " and FType!=8  and FType!=19 ";
            }

            if (QQType == "0")    //"" 所有类型; 0 非会员; 1 普通会员; 2 VIP会员
            {
                strWhere += " and FVipFlag<>1 and FVipFlag<>2 ";
            }
            else if (QQType == "1")
            {
                strWhere += " and FVipFlag=1 ";
            }
            else if (QQType == "2")
            {
                strWhere += " and FVipFlag=2 ";
            }

            //增加高分单低分单查询
            if (dotype == "1")
            {
                //高分
                strWhere += " and FAutoAppeal=1 ";
            }
            else if (dotype == "0")
            {
                //低分
                strWhere += " and FAutoAppeal<>1 ";
            }
            fstrSql = "";
            fstrSql_count = "";
            string str = " Fid,FType,Fuin,FSubmitTime,FState,FCheckTime,FReCheckTime,Fpicktime,FReCheckUser,FCheckInfo,FCheckUser,FComment,Femail from db_appeal_";

            if (int.Parse(tb) < 10)
                fstrSql += " select 'db_appeal_" + db + "' as DBName,'t_tenpay_appeal_trans_" + "0" + tb + "' as tableName," + str + db + ".t_tenpay_appeal_trans_" + "0" + tb + strWhere;
            else
                fstrSql += " select 'db_appeal_" + db + "' as DBName,'t_tenpay_appeal_trans_" + tb + "' as tableName," + str + db + ".t_tenpay_appeal_trans_" + tb + strWhere;
            fstrSql_count = "select count(1) from ( " + fstrSql + " ) as total";
        }

        public CFTUserAppealClass(string fuin, string u_BeginTime, string u_EndTime, int fstate, int ftype, string QQType,
            string pickUser, string fid, string szReason, string orderType)
        {
            string strWhere = " where 1=1 ";

            if (fuin != null && fuin.Trim() != "")
            {
                strWhere += " and Fuin='" + fuin.Trim() + "' ";
            }

            if (u_BeginTime != null && u_BeginTime != "")
            {

                strWhere += " and FSubmitTime between '" + u_BeginTime + "' and '" + u_EndTime + "' ";
            }

            if (fstate != 99)
            {
                if (fstate == 10)   //直接申诉成功，即审核成功，且FCheckUser为system
                    strWhere += " and FState='1' and FCheckUser='system' ";
                else
                    strWhere += " and FState='" + fstate + "'  ";
            }

            if (ftype != 99)
            {
                strWhere += " and FType=" + ftype + " ";
            }
            else
            {
                // 8号申诉是风控冻结，有另外的处理入口，申诉处理暂不包含改类型的申诉
                strWhere += " and FType!=8 ";
            }

            if (QQType == "0")    //"" 所有类型; "0" 非会员; "1" 普通会员; "2" VIP会员
            {
                strWhere += " and (FParameter not like '%vip_flag=1%') and (FParameter not like '%vip_flag=2%') ";
            }
            else if (QQType == "1")
            {
                strWhere += " and FParameter like '%vip_flag=1%' ";
            }
            else if (QQType == "2")
            {
                strWhere += " and FParameter like '%vip_flag=2%' ";
            }

            if (pickUser != "")
            {
                strWhere += " and fpickuser like '%" + pickUser + "%' ";
            }

            if (fid != "")
            {
                strWhere += " and Fid=" + fid;
            }

            if (szReason != "")
            {
                strWhere += " and FComment like '%" + szReason + "%' ";
            }

            // 默认按照提交时间最早来排
            //string orderStr = " order by date_format(FSubmitTime,'%Y%m%d'),FUin asc";
            string orderStr = " order by date_format(FSubmitTime,'%Y%m%d') asc";

            if (orderType == "2")
            {
                orderStr = "order by date_format(FSubmitTime,'%Y%m%d') desc";
            }

            fstrSql = "select Fid,FType,Fuin,FSubmitTime,FState,FCheckTime,Fpicktime,FCheckInfo,FCheckUser,FComment,Femail,FPickUser from t_tenpay_appeal_trans "
                + strWhere + orderStr;

            fstrSql_count = "select count(1) from t_tenpay_appeal_trans " + strWhere;
        }

        //yinhuang 2014/02/13 分库分表
        public CFTUserAppealClass(string fuin, string u_BeginTime, string u_EndTime, int fstate, int ftype, string QQType,
            string pickUser, string fid, string szReason, string orderType, string table)
        {
            string strWhere = " where 1=1 ";

            if (fuin != null && fuin.Trim() != "")
            {
                strWhere += " and Fuin='" + fuin.Trim() + "' ";
            }

            if (u_BeginTime != null && u_BeginTime != "")
            {

                strWhere += " and FSubmitTime between '" + u_BeginTime + "' and '" + u_EndTime + "' ";
            }

            if (fstate != 99)
            {
                if (fstate == 10)   //直接申诉成功，即审核成功，且FCheckUser为system
                {
                    if (ftype == 8)
                    {
                        strWhere += " and FState='" + fstate + "'  ";
                    }
                    else 
                    {
                        strWhere += " and FState='1' and FCheckUser='system' ";
                    } 
                }
                else
                {
                    strWhere += " and FState='" + fstate + "'  ";
                }
            }

            if (ftype != 99)
            {
                //if (ftype == 8)
                //{
                //    strWhere += " and (FType=8 or FType=19) ";
                //}
                //else 
                //{
                    strWhere += " and FType=" + ftype + " ";
                //} 
            }

            if (QQType == "0")    //"" 所有类型; "0" 非会员; "1" 普通会员; "2" VIP会员
            {
                strWhere += " and (FParameter not like '%vip_flag=1%') and (FParameter not like '%vip_flag=2%') ";
            }
            else if (QQType == "1")
            {
                strWhere += " and FParameter like '%vip_flag=1%' ";
            }
            else if (QQType == "2")
            {
                strWhere += " and FParameter like '%vip_flag=2%' ";
            }

            if (pickUser != "")
            {
                strWhere += " and fpickuser like '%" + pickUser + "%' ";
            }

            if (fid != "")
            {
                strWhere += " and Fid='" + fid+"'";
            }

            if (szReason != "")
            {
                strWhere += " and FComment like '%" + szReason + "%' ";
            }

            // 默认按照提交时间最早来排
            //string orderStr = " order by date_format(FSubmitTime,'%Y%m%d'),FUin asc";
            string orderStr = " order by date_format(FSubmitTime,'%Y%m%d') asc";

            if (orderType == "2")
            {
                orderStr = "order by date_format(FSubmitTime,'%Y%m%d') desc";
            }

            fstrSql = "select Fid,FType,Fuin,FSubmitTime,FState,FCheckTime,Fpicktime,FCheckInfo,FCheckUser,FComment,Femail,FPickUser from " + table+" "
                + strWhere + orderStr;

            fstrSql_count = "select count(1) from "+ table+" " + strWhere;
        }

        //直接通过的构造函数
        public CFTUserAppealClass(string u_BeginTime, string u_EndTime, string ftypes)
        {
            string strWhere = " where FSubmitTime between '" + u_BeginTime + "' and '" + u_EndTime + "' and FState='0' and FType in(" + ftypes + ")";

            fstrSql = "select Fid,FType,Fuin,FSubmitTime,FState,FCheckTime,Fpicktime,FCheckInfo,FCheckUser,FComment,Femail from t_tenpay_appeal_trans " + strWhere;
            fstrSql_count = "select count(1) from t_tenpay_appeal_trans " + strWhere;
        }
        public CFTUserAppealClass(int fid)
        {
            //fstrSql = "select Fid,FType,Fuin,FSubmitTime,FState,Fpicktime,FCheckTime,FCheckInfo,FCheckUser,FComment,Femail from t_tenpay_appeal_trans where FID=" + fid ;
            fstrSql = "select * from t_tenpay_appeal_trans where FID=" + fid;
            fstrSql_count = "select count(1) from t_tenpay_appeal_trans where FID=" + fid;
        }

        //yinhuang 2014/02/12 解冻申诉单分库分表
        public CFTUserAppealClass(string fid, string table)
        {
            //fstrSql = "select Fid,FType,Fuin,FSubmitTime,FState,Fpicktime,FCheckTime,FCheckInfo,FCheckUser,FComment,Femail from t_tenpay_appeal_trans where FID=" + fid ;
            fstrSql = "select * from "+table+" where FID='" + fid+"'";
            fstrSql_count = "select count(1) from "+table+" where FID='" + fid+"'";
        }

        //20131107 lxl 查询分库表
        public CFTUserAppealClass(string fid, string db, string tb,string mark)
        {
            //fstrSql = "select Fid,FType,Fuin,FSubmitTime,FState,Fpicktime,FCheckTime,FCheckInfo,FCheckUser,FComment,Femail from t_tenpay_appeal_trans where FID=" + fid ;
            fstrSql = "select * from "+db+"."+tb+ " where FID='" + fid+"'";
            fstrSql_count = "select count(1) from " + db + "." + tb + " where FID='" + fid + "'";
        }

        public static DataSet GetLockList(DateTime BeginDate, DateTime EndDate, string fstate, string ftype, string QQType, string username, int Count,int SortType)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            DataSet ds = new DataSet();

            try
            {
                da.OpenConn();

                string strSql = "select * from t_tenpay_appeal_trans where FsubmitTime >= '" + BeginDate.ToString("yyyy-MM-dd 00:00:00") + "' and FsubmitTime <= '" + EndDate.ToString("yyyy-MM-dd 23:59:59") + "'";

                if (username == null || username == "")
                {
                    throw new Exception("批量领单人不允许为空!");
                }

                if (fstate == "99")
                {
                    strSql += " and Fstate in(0,3,4,5,6,8) ";		// 2012/4/25  因为新增8也批量了，所以这里应该添加吧？
                }
                else if (fstate == "0" || fstate == "3" || fstate == "4" || fstate == "5" || fstate == "6" || fstate == "8")  //新增8也批量了
                {
                    strSql += " and Fstate = " + fstate;
                }
                else
                {
                    throw new Exception("改申诉状态不允许批量领单!");
                }
                if (ftype == "99")
                {
                    strSql += " and FType in(1,2,3,4,5,6,7,0,9,10)";//20130923 lxl 将“财付通解绑、手机令牌、第三方令牌”加入批量领单功能中
                }
                // 2012/4/2 新添加ftype=“7”，允许批量领“更换证件号码”单
                else if (ftype == "1" || ftype == "2" || ftype == "3" || ftype == "4" || ftype == "5" || ftype == "6" || ftype == "7" || ftype == "0" || ftype == "9" || ftype == "10")
                {
                    strSql += " and FType = " + ftype;
                }
                else
                {
                    throw new Exception("该申诉类型不允许批量领单!");
                }

                if (QQType == "0")    //"" 所有类型; "0" 非会员; "1" 普通会员; "2" VIP会员
                {
                    strSql += " and (FParameter not like '%vip_flag=1%') and (FParameter not like '%vip_flag=2%') ";
                }
                else if (QQType == "1")
                {
                    strSql += " and FParameter like '%vip_flag=1%' ";
                }
                else if (QQType == "2")
                {
                    strSql += " and FParameter like '%vip_flag=2%' ";
                }

                if (SortType != 99)
                {
                    if (SortType == 0)   //排序：时间小到大
                        strSql += " order by FSubmitTime asc ";
                    if (SortType == 1)   //排序：时间大到小
                        strSql += " order by FSubmitTime desc ";
                }

                strSql += " limit " + Count;

                ds = da.dsGetTotalData(strSql);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    HandleParameter(ds, true);

                    string WhereStr = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["IsPass"].ToString() == "Y")
                        {
                            dr.Delete();
                        }
                        else
                            WhereStr += "'" + dr["Fid"].ToString().Trim() + "',";
                    }
                    ds.AcceptChanges();

                    WhereStr = WhereStr.Substring(0, WhereStr.Length - 1);

                    strSql = " update t_tenpay_appeal_trans set FPickUser='" + username + "',FPickTime=now(),Fstate=8 where Fid in(" + WhereStr + ")";

                    da.ExecSql(strSql);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                da.Dispose();
            }

            return ds;
        }

        //批量领单三种特殊类型旧表构造器
        public CFTUserAppealClass(DateTime BeginDate, DateTime EndDate, string fstate, string ftype, string QQType, string username, int Count, int SortType)
        {
            fstrSql = "select '' as DBName, '' as tableName, Fid,FType,Fuin,FSubmitTime,FState,FCheckTime,FReCheckTime,Fpicktime,FReCheckUser,FCheckInfo,FCheckUser,FComment,Femail from t_tenpay_appeal_trans where FsubmitTime >= '" + BeginDate.ToString("yyyy-MM-dd 00:00:00") + "' and FsubmitTime <= '" + EndDate.ToString("yyyy-MM-dd 23:59:59") + "'";

            if (username == null || username == "")
            {
                throw new Exception("批量领单人不允许为空!");
            }

            if (fstate == "99")
            {
                fstrSql += " and Fstate in(0,3,4,5,6,8) ";		// 2012/4/25  因为新增8也批量了，所以这里应该添加吧？
            }
            else if (fstate == "0" || fstate == "3" || fstate == "4" || fstate == "5" || fstate == "6" || fstate == "8")  //新增8也批量了
            {
                fstrSql += " and Fstate = " + fstate;
            }
            else
            {
                throw new Exception("改申诉状态不允许批量领单!");
            }
            if (ftype == "99")
            {
                fstrSql += " and FType in(1,2,3,4,5,6,7,0,9,10)";//20130923 lxl 将“财付通解绑、手机令牌、第三方令牌”加入批量领单功能中
            }
            // 2012/4/2 新添加ftype=“7”，允许批量领“更换证件号码”单
            else if (ftype == "1" || ftype == "2" || ftype == "3" || ftype == "4" || ftype == "5" || ftype == "6" || ftype == "7" || ftype == "0" || ftype == "9" || ftype == "10")
            {
                fstrSql += " and FType = " + ftype;
            }
            else
            {
                throw new Exception("该申诉类型不允许批量领单!");
            }

            if (QQType == "0")    //"" 所有类型; "0" 非会员; "1" 普通会员; "2" VIP会员
            {
                fstrSql += " and (FParameter not like '%vip_flag=1%') and (FParameter not like '%vip_flag=2%') ";
            }
            else if (QQType == "1")
            {
                fstrSql += " and FParameter like '%vip_flag=1%' ";
            }
            else if (QQType == "2")
            {
                fstrSql += " and FParameter like '%vip_flag=2%' ";
            }

            if (SortType != 99)
            {
                if (SortType == 0)   //排序：时间小到大
                    fstrSql += " order by FSubmitTime asc ";
                if (SortType == 1)   //排序：时间大到小
                    fstrSql += " order by FSubmitTime desc ";
            }

            //    strSql += " limit " + Count;

            //ds = da.dsGetTotalData(strSql);

            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    HandleParameter(ds, true);

            //    string WhereStr = "";
            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        if (dr["IsPass"].ToString() == "Y")
            //        {
            //            dr.Delete();
            //        }
            //        else
            //            WhereStr += "'" + dr["Fid"].ToString().Trim() + "',";
            //    }
            //    ds.AcceptChanges();

            //    WhereStr = WhereStr.Substring(0, WhereStr.Length - 1);

            //    strSql = " update t_tenpay_appeal_trans set FPickUser='" + username + "',FPickTime=now(),Fstate=8 where Fid in(" + WhereStr + ")";

            //    da.ExecSql(strSql);

            //    fstrSql = "";
            //    fstrSql_count = "";
            //}


            // return ds;
        }

        //批量领单三种特殊类型分库表构造器
        public CFTUserAppealClass(DateTime BeginDate, DateTime EndDate, string fstate, string ftype, string QQType, string username, int Count, int SortType, bool mark)
        {
            string strWhere = " where FsubmitTime >= '" + BeginDate.ToString("yyyy-MM-dd 00:00:00") + "' and FsubmitTime <= '" + EndDate.ToString("yyyy-MM-dd 23:59:59") + "'";
            if (username == null || username == "")
            {
                throw new Exception("批量领单人不允许为空!");
            }

            if (fstate == "99")
            {
                strWhere += " and Fstate in(0,3,4,5,6,8) ";		// 2012/4/25  因为新增8也批量了，所以这里应该添加吧？
            }
            else if (fstate == "0" || fstate == "3" || fstate == "4" || fstate == "5" || fstate == "6" || fstate == "8")  //新增8也批量了
            {
                strWhere += " and Fstate = " + fstate;
            }
            else
            {
                throw new Exception("改申诉状态不允许批量领单!");
            }
            if (ftype == "99")
            {
                strWhere += " and FType in(1,2,3,4,5,6,7,0,9,10)";//20130923 lxl 将“财付通解绑、手机令牌、第三方令牌”加入批量领单功能中
            }
            // 2012/4/2 新添加ftype=“7”，允许批量领“更换证件号码”单
            else if (ftype == "1" || ftype == "2" || ftype == "3" || ftype == "4" || ftype == "5" || ftype == "6" || ftype == "7" || ftype == "0" || ftype == "9" || ftype == "10")
            {
                strWhere += " and FType = " + ftype;
            }
            else
            {
                throw new Exception("该申诉类型不允许批量领单!");
            }

            if (QQType == "0")    //"" 所有类型; "0" 非会员; "1" 普通会员; "2" VIP会员
            {
                strWhere += " and FVipFlag<>1 and FVipFlag<>2 ";
            }
            else if (QQType == "1")
            {
                strWhere += " and FVipFlag=1 ";
            }
            else if (QQType == "2")
            {
                strWhere += " and FVipFlag=2 ";
            }

            fstrSql = "";
            fstrSql_count = "";
            string str = " Fid,FType,Fuin,FSubmitTime,FState,FCheckTime,FReCheckTime,Fpicktime,FReCheckUser,FCheckInfo,FCheckUser,FComment,Femail from db_appeal_";
            
            int yearBegin = BeginDate.Year;
            int monBegin = BeginDate.Month;
            int yearEnd = EndDate.Year;
            int monEnd = EndDate.Month;
            if (yearEnd < 2014)//数据库无数据
            {
                fstrSql = "";
                fstrSql_count = "";
            }
            else
            {
                while (yearBegin <= yearEnd)
                {
                    if (yearBegin < 2014)
                    {
                        yearBegin++;
                        continue;
                    }
                    for (int i = 1; i <= 12; i++)//月份
                    {
                        if (yearBegin == yearEnd && i > monEnd)//查到结束日期
                            break;
                        if (i < monBegin)//小于开始月份的数据不查
                            continue;
                        if (fstrSql != "")
                            fstrSql += " union all ";
                        if (i < 10)
                            fstrSql += " select 'db_appeal_" + yearBegin + "' as DBName,'t_tenpay_appeal_trans_" + "0" + i + "' as tableName," + str + yearBegin + ".t_tenpay_appeal_trans_" + "0" + i + strWhere;
                        else
                            fstrSql += " select 'db_appeal_" + yearBegin + "' as DBName,'t_tenpay_appeal_trans_" + i + "' as tableName," + str + yearBegin + ".t_tenpay_appeal_trans_" + i + strWhere;

                    }
                    yearBegin++;
                    monBegin = 0;//换年份后，将初始月份至为0
                }
            }
        }

        //批量领单三种特殊类型分库表构造器
        public CFTUserAppealClass(DateTime BeginDate, DateTime EndDate, string fstate, string ftype, string QQType, string username, int Count, int SortType, bool mark,string db,string tb)
        {
            string strWhere = " where FsubmitTime >= '" + BeginDate.ToString("yyyy-MM-dd 00:00:00") + "' and FsubmitTime <= '" + EndDate.ToString("yyyy-MM-dd 23:59:59") + "'";
            if (username == null || username == "")
            {
                throw new Exception("批量领单人不允许为空!");
            }

            if (fstate == "99")
            {
                strWhere += " and Fstate in(0,3,4,5,6,8) ";		// 2012/4/25  因为新增8也批量了，所以这里应该添加吧？
            }
            else if (fstate == "0" || fstate == "3" || fstate == "4" || fstate == "5" || fstate == "6" || fstate == "8")  //新增8也批量了
            {
                strWhere += " and Fstate = " + fstate;
            }
            else
            {
                throw new Exception("改申诉状态不允许批量领单!");
            }
            if (ftype == "99")
            {
                strWhere += " and FType in(1,2,3,4,5,6,7,0,9,10)";//20130923 lxl 将“财付通解绑、手机令牌、第三方令牌”加入批量领单功能中
            }
            // 2012/4/2 新添加ftype=“7”，允许批量领“更换证件号码”单
            else if (ftype == "1" || ftype == "2" || ftype == "3" || ftype == "4" || ftype == "5" || ftype == "6" || ftype == "7" || ftype == "0" || ftype == "9" || ftype == "10")
            {
                strWhere += " and FType = " + ftype;
            }
            else
            {
                throw new Exception("该申诉类型不允许批量领单!");
            }

            if (QQType == "0")    //"" 所有类型; "0" 非会员; "1" 普通会员; "2" VIP会员
            {
                strWhere += " and FVipFlag<>1 and FVipFlag<>2 ";
            }
            else if (QQType == "1")
            {
                strWhere += " and FVipFlag=1 ";
            }
            else if (QQType == "2")
            {
                strWhere += " and FVipFlag=2 ";
            }

            fstrSql = "";
            fstrSql_count = "";
            string str = " Fid,FType,Fuin,FSubmitTime,FState,FCheckTime,FReCheckTime,Fpicktime,FReCheckUser,FCheckInfo,FCheckUser,FComment,Femail from db_appeal_";
            if (int.Parse(tb)< 10)
                fstrSql += " select 'db_appeal_" + db + "' as DBName,'t_tenpay_appeal_trans_" + "0" + tb + "' as tableName," + str + db + ".t_tenpay_appeal_trans_" + "0" + tb + strWhere;
            else
                fstrSql += " select 'db_appeal_" + db + "' as DBName,'t_tenpay_appeal_trans_" + tb + "' as tableName," + str + db + ".t_tenpay_appeal_trans_" + tb + strWhere;
        }


        public DataSet GetLockListDBTB(DateTime BeginDate, DateTime EndDate, string fstate, string ftype, string QQType, string username, int Count, int SortType)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            DataSet ds = new DataSet();

            try
            {
                da.OpenConn();

                string strSql = "select * from t_tenpay_appeal_trans where FsubmitTime >= '" + BeginDate.ToString("yyyy-MM-dd 00:00:00") + "' and FsubmitTime <= '" + EndDate.ToString("yyyy-MM-dd 23:59:59") + "'";

                if (username == null || username == "")
                {
                    throw new Exception("批量领单人不允许为空!");
                }

                if (fstate == "99")
                {
                    strSql += " and Fstate in(0,3,4,5,6,8) ";		// 2012/4/25  因为新增8也批量了，所以这里应该添加吧？
                }
                else if (fstate == "0" || fstate == "3" || fstate == "4" || fstate == "5" || fstate == "6" || fstate == "8")  //新增8也批量了
                {
                    strSql += " and Fstate = " + fstate;
                }
                else
                {
                    throw new Exception("改申诉状态不允许批量领单!");
                }
                if (ftype == "99")
                {
                    strSql += " and FType in(1,2,3,4,5,6,7,0,9,10)";//20130923 lxl 将“财付通解绑、手机令牌、第三方令牌”加入批量领单功能中
                }
                // 2012/4/2 新添加ftype=“7”，允许批量领“更换证件号码”单
                else if (ftype == "1" || ftype == "2" || ftype == "3" || ftype == "4" || ftype == "5" || ftype == "6" || ftype == "7" || ftype == "0" || ftype == "9" || ftype == "10")
                {
                    strSql += " and FType = " + ftype;
                }
                else
                {
                    throw new Exception("该申诉类型不允许批量领单!");
                }

                if (QQType == "0")    //"" 所有类型; "0" 非会员; "1" 普通会员; "2" VIP会员
                {
                    strSql += " and (FParameter not like '%vip_flag=1%') and (FParameter not like '%vip_flag=2%') ";
                }
                else if (QQType == "1")
                {
                    strSql += " and FParameter like '%vip_flag=1%' ";
                }
                else if (QQType == "2")
                {
                    strSql += " and FParameter like '%vip_flag=2%' ";
                }

                if (SortType != 99)
                {
                    if (SortType == 0)   //排序：时间小到大
                        strSql += " order by FSubmitTime asc ";
                    if (SortType == 1)   //排序：时间大到小
                        strSql += " order by FSubmitTime desc ";
                }

                strSql += " limit " + Count;

                ds = da.dsGetTotalData(strSql);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    HandleParameter(ds, true);

                    string WhereStr = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["IsPass"].ToString() == "Y")
                        {
                            dr.Delete();
                        }
                        else
                            WhereStr += "'" + dr["Fid"].ToString().Trim() + "',";
                    }
                    ds.AcceptChanges();

                    WhereStr = WhereStr.Substring(0, WhereStr.Length - 1);

                    strSql = " update t_tenpay_appeal_trans set FPickUser='" + username + "',FPickTime=now(),Fstate=8 where Fid in(" + WhereStr + ")";

                    da.ExecSql(strSql);

                    fstrSql = "";
                    fstrSql_count = ds.Tables[0].Rows.Count.ToString();//计算出记录行数
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                da.Dispose();
            }

            return ds;
        }

        //新增表t_tenpay_appeal_kf_total
        /*人     日期      处理成功  处理失败   其它
        User OperationDay SuccessNum  FailNum  OtherNum
        处理成功:发成功邮件
        其它:包括很多(转后台或错误等)
        */
        public static bool ConfirmAppeal(int fid, string Fcomment, string user, string userIP, out string msg)
        {
            msg = "";

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            try
            {
                da.OpenConn();

                if (Fcomment != null && Fcomment.Trim() != "")
                {
                    Fcomment = PublicRes.replaceMStr(Fcomment);
                }

                //先取出一些需要的信息 当前状态,修改类别, QQ号,email
                //修改姓名需要新姓名. 取回支付密码需要clear_pps, 注销不需要多余东西
                string strSql = "select * from t_tenpay_appeal_trans where Fid=" + fid;
                DataSet ds = da.dsGetTotalData(strSql);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    HandleParameter(ds, true);

                    DataRow dr = ds.Tables[0].Rows[0];
                    int fstate = Int32.Parse(dr["FState"].ToString());
                    int ftype = Int32.Parse(dr["FType"].ToString());
                    string fuin = dr["FUin"].ToString();
                    string email = dr["Femail"].ToString();
                    string question1 = dr["question1"].ToString();
                    string answer1 = dr["answer1"].ToString();
                    string cont_type = dr["cont_type"].ToString();
                    string mobile_no = dr["mobile_no"].ToString();
                    string client_ip = dr["client_ip"].ToString();
                    string certno = dr["cre_id"].ToString();

                    if (fstate == 1 || fstate == 7)
                    {
                        msg = "原记录的状态不正确";
                        return false;
                    }

                    string nowtime = PublicRes.strNowTimeStander;
                    string new_name = "";
                    int clear_pps = 0;

                    string submittime = DateTime.Parse(dr["FSubmitTime"].ToString()).ToString("yyyy年MM月dd日");

                    //读取现在的用户名？furion 20060902
                    string username = PublicRes.GetNameFromQQ(fuin);
                    //简化注册用户这里处理下
                    if ((username == null || username.Trim() == "") && ftype == 6)
                    {
                        username = "亲爱的财付通客户";
                    }
                    if (username == null || username.Trim() == "")
                    {
                        msg = "取原有用户名时出错";
                        //return false;
                    }

                    string p3 = "";
                    string p4 = "";

                    //					if (!PublicRes.ReleaseCache(fuin,"qqid"))
                    //					{
                    //						Common.CommLib.commRes.sendLog4Log("QUeryInfo.ConfirmAppeal","通过自助申诉时，预清除用户"+ fuin + "cache失败！请检查！");
                    //						msg = "通过自助申诉时，预清除用户"+ fuin + "cache失败！请检查！";
                    //						return false;
                    //					}
                    //进行处理.	 //发送邮件
                    if (ftype == 0)
                    {
                        string Fuid = PublicRes.ConvertToFuid(fuin);
                        if (Fuid.Length < 3)
                        {
                            msg = "取内部ID失败";
                            return false;
                        }

                        /*
                        string inmsg = "uid=" + Fuid;
                        inmsg += "&optype=3";
                        inmsg += "&time_stamp=" + TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTickFromTime(nowtime);
                        inmsg += "&server_ip=" + userIP;
                        inmsg += "&client_ip=" + client_ip;
                        inmsg += "&sign=" + System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Fuid + "3" + TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTickFromTime(nowtime) + "cft_uk_key","md5").ToLower();
                        */

                        //md5( uid + optype + time_stamp + watch_word)
                        string watchWord = System.Configuration.ConfigurationManager.AppSettings["watchword"].ToLower();
                        string inmsg = "watchword=" + watchWord;
                        inmsg += "&uid=" + Fuid;
                        inmsg += "&optype=3";
                        inmsg += "&time_stamp=" + TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTickFromTime(nowtime);
                        inmsg += "&server_ip=" + userIP;
                        inmsg += "&client_ip=" + client_ip;
                        inmsg += "&sign=" + System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Fuid + "3" + TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTickFromTime(nowtime) + watchWord, "md5").ToLower();


                        string reply;
                        short sresult;

                        if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("ra_apeal_service", inmsg, true, out reply, out sresult, out msg))
                        {
                            if (sresult != 0)
                            {
                                msg = "ra_apeal_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                                return false;
                            }
                            else
                            {
                                if (reply.IndexOf("result=0") > -1)   //&0=298751028&res_info=ok&result=0 正常值
                                {
                                }
                                else
                                {
                                    msg = "ra_apeal_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            msg = "ra_apeal_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                            return false;
                        }
                    }
                    else if (ftype == 1)
                    {
                        //找回密码
                        clear_pps = Int32.Parse(dr["clear_pps"].ToString());

                        bool IsNew; //是否新流程
                        if (cont_type != "") //cont_type为新加字段，如果为空就说明走老流程
                        {
                            IsNew = true;
                            bool IsMobile = false;
                            bool IsMail = false;
                            //cont_type=1时,mobile_no有效 cont_type=2时,femail有效

                            if (cont_type == "1")
                                IsMobile = true;
                            else if (cont_type == "2")
                                IsMail = true;

                            if (cont_type == "1" || cont_type == "2")//无需绑定
                            {
                                //如果是邮箱型用户，不需要绑定邮箱（默认绑定邮箱就是该帐号）
                                if (IsMail && fuin.IndexOf("@") > -1)
                                {
                                }
                                else
                                {
                                    string BindMail = "";
                                    Query_Service qs = new Query_Service();
                                    if (!qs.BindMsgNotify(fuin, IsMobile, mobile_no, IsMail, email, client_ip,certno, out BindMail, out msg)) return false;

                                    email = BindMail;
                                }
                            }
                        }
                        else
                        {
                            IsNew = false;
                        }

                        if (!GetNewPwd(fuin, clear_pps, userIP, nowtime, question1, answer1, IsNew, out msg)) return false;

                        string warmTips = @"<br/><br/>温馨提示：<br/>如果您的账号还未绑定关联手机，建议您现在立即绑定。绑定成功后，您既可根据关联手机接收验证码快速实时的修改您的支付密码。";

                        p3 = msg;
                        if (clear_pps == 1)
                            //p4 = "您的密码保护问题已被清空，请登陆重新设置您的密码保护问题！"; 
                            p4 = "您的密保资料已经更新成功，请使用该新的密保答案操作您的财付通账户！" + warmTips;
                        else
                            p4 = warmTips;
                    }
                    else if (ftype == 2)
                    {
                        //修改用户姓名
                        new_name = QueryInfo.GetString(dr["new_name"]);
                        /*旧接口
                        if(!UpdateUserName(fuin,new_name,false,userIP,nowtime, out msg))
                            return false;
                        */
                        //新接口
                        string Fuid = PublicRes.ConvertToFuid(fuin);
                        if (Fuid.Length < 3)
                        {
                            msg = "取内部ID失败";
                            return false;
                        }

                        //遇中文MD5加密有问题哦，因为编码规则不一样
                        System.Text.Encoding GB2312 = System.Text.Encoding.GetEncoding("GB2312");
                        byte[] outbuff = GB2312.GetBytes(Fuid + "|" + fuin + "|" + new_name + "||5416d7cd6ef195a0f7622a9c56b55e84");
                        System.Security.Cryptography.MD5CryptoServiceProvider get_md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                        byte[] hash_byte = get_md5.ComputeHash(outbuff);

                        string inmsg = "uin=" + fuin;
                        inmsg += "&true_name=" + new_name;
                        inmsg += "&vali_type=100";
                        inmsg += "&token=" + System.BitConverter.ToString(hash_byte).Replace("-", "").ToLower();

                        string reply;
                        short sresult;

                        if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("ui_modify_usernameid_service", inmsg, true, out reply, out sresult, out msg))
                        {
                            if (sresult != 0)
                            {
                                msg = "ui_modify_usernameid_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                                return false;
                            }
                            else
                            {
                                if (reply.StartsWith("result=0"))
                                {
                                }
                                else
                                {
                                    msg = "ui_modify_usernameid_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            msg = "ui_modify_usernameid_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                            return false;
                        }

                        p3 = new_name;
                    }
                    else if (ftype == 3)
                    {
                        //修改公司名
                        new_name = QueryInfo.GetString(dr["new_company"]);

                        if (!UpdateUserName(fuin, new_name, true, userIP, nowtime, out msg))
                            return false;

                        p3 = new_name;
                    }
                    else if (ftype == 4)
                    {
                        //注销帐号
                        if (!DelUser(fuin, email, Fcomment, user, userIP, nowtime, out msg))
                            return false;

                        //因为现在暂时不支持注销绑定关系 furion 20060902
                        //p3 = "，（已同时注销了该帐户原银行卡号和身份证号码的绑定关系）谢谢";
                        p3 = "";
                    }
                    else if (ftype == 5)//完整帐号的申述找回手机的，审核通过后客服系统就增加一步绑定邮箱。 andrew 20110419
                    {

                        //找回密码
                        clear_pps = Int32.Parse(dr["clear_pps"].ToString());

                        bool IsNew; //是否新流程
                        if (cont_type != "") //cont_type为新加字段，如果为空就说明走老流程
                        {
                            IsNew = true;
                            bool IsMobile = false;
                            bool IsMail = false;
                            //cont_type=1时,mobile_no有效 cont_type=2时,femail有效

                            if (cont_type == "1")
                                IsMobile = true;
                            else if (cont_type == "2")
                                IsMail = true;

                            if (cont_type == "1" || cont_type == "2")
                            {
                                //如果是邮箱型用户，不需要绑定邮箱（默认绑定邮箱就是该帐号）
                                if (IsMail && fuin.IndexOf("@") > -1)
                                {
                                }
                                else
                                {
                                    string BindMail = "";
                                    Query_Service qs = new Query_Service();
                                    if (!qs.BindMsgNotify(fuin, IsMobile, mobile_no, IsMail, email, client_ip,certno, out BindMail, out msg)) return false;

                                    email = BindMail;
                                }
                            }
                        }
                        else
                        {
                            IsNew = false;
                        }

                        if (clear_pps == 1)
                        {
                            //只替换密保问题和答案
                            if (!CheckQuestion(fuin, userIP, nowtime, question1, answer1, IsNew, out msg)) return false;
                        }

                        if (dr["from"].ToString() == "TENPAY_MOBILE")
                        {
                            string Fuid = PublicRes.ConvertToFuid(fuin);
                            string ret = "";
                            string appealtime = TimeTransfer.GetTickFromTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                            System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("GB2312");
                            string cgi = System.Configuration.ConfigurationManager.AppSettings["MobileCgi"].ToString();
                            cgi += "?uin=" + fuin;
                            cgi += "&bargainor_id=1000000000";
                            cgi += "&uid=" + Fuid;
                            cgi += "&appealtime=" + appealtime;
                            cgi += "&status=0";
                            cgi += "&cmd=update";
                            cgi += "&newmobile=" + mobile_no;
                            cgi += "&chv=9";
                            cgi += "&sign=" + System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile("appealtime=" + appealtime + "&bargainor_id=1000000000" +
                                "&chv=9" + "&cmd=update" + "&newmobile=" + mobile_no + "&status=0" + "&uid=" + Fuid + "&uin=" + fuin + "&key=Adf^#KK12D", "md5").ToLower();

                            SunLibrary.LoggerFactory.Get("KF_Service QueryInfo").Info("MobileCgi send req:" + cgi);
                            
                            System.Net.HttpWebRequest webrequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(cgi);
                            System.Net.HttpWebResponse webresponse = (System.Net.HttpWebResponse)webrequest.GetResponse();
                            Stream stream = webresponse.GetResponseStream();
                            StreamReader streamReader = new StreamReader(stream, encoding);
                            ret = streamReader.ReadToEnd();

                            SunLibrary.LoggerFactory.Get("KF_Service QueryInfo").Info("MobileCgi return:" + ret);

                            webresponse.Close();
                            streamReader.Close();
                        }

                        //更换关联手机,不用做什么操作,直接发出邮件就可以了.
                        string url = ConfigurationManager.AppSettings["ChangeMobileUrl"].Trim();

                        // 更换关联手机的话，需要添加WarmTips
                        string warmTips = "<br/> 温馨提示：<br/>此链接地址有效期为72小时，请您在有效期内完成新手机帐号的绑定，如果您无法正常打开链接地址，请您使用IE浏览器登录<a href='www.tenpay.com'>财付通主站</a>后，再重新点击邮件链接地址，或者请您直接将以上完整链接地址复制到浏览器地址栏中打开操作即可,谢谢！";

                        p3 = url + fid;

                        if (clear_pps == 1)
                            p4 = p3 + "<br/>您的密保资料已经更新成功，请使用您申诉时成功修改的密保答案即可！" + warmTips;
                        else
                            p4 = p3 + warmTips;
                    }
                    else if (ftype == 6) //andrew 20110419
                    {
                        //更换关联手机,不用做什么操作,直接发出邮件就可以了.
                        string url = ConfigurationManager.AppSettings["ChangeMobileUrl"].Trim();

                        // 更换关联手机的话，需要添加WarmTips
                        string warmTips = "<br/> 温馨提示：<br/>此链接地址有效期为72小时，请您在有效期内完成新手机帐号的绑定，如果您无法正常打开链接地址，请您使用IE浏览器登录<a href='www.tenpay.com'>财付通主站</a>后，再重新点击邮件链接地址，或者请您直接将以上完整链接地址复制到浏览器地址栏中打开操作即可,谢谢！";

                        p3 = url + fid;
                        p4 = p3 + warmTips;
                    }
                    else if (ftype == 7)
                    {
                        string new_cre_id = dr["new_cre_id"].ToString();
                        //修改身份证号
                        /*旧接口
                        if(!UpdateCreid(fuin,new_cre_id,userIP,nowtime, out msg))
                            return false;
                        */
                        //新接口
                        string Fuid = PublicRes.ConvertToFuid(fuin);
                        if (Fuid.Length < 3)
                        {
                            msg = "取内部ID失败";
                            return false;
                        }
                        string inmsg = "uin=" + fuin;
                        inmsg += "&cre_type=" + "1";
                        inmsg += "&cre_id=" + new_cre_id;
                        inmsg += "&vali_type=100";
                        inmsg += "&token=" + System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Fuid + "|" + fuin + "||" + new_cre_id + "|5416d7cd6ef195a0f7622a9c56b55e84", "md5").ToLower();

                        string reply;
                        short sresult;

                        if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("ui_modify_usernameid_service", inmsg, true, out reply, out sresult, out msg))
                        {
                            if (sresult != 0)
                            {
                                msg = "ui_modify_usernameid_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                                return false;
                            }
                            else
                            {
                                if (reply.StartsWith("result=0"))
                                {
                                }
                                else
                                {
                                    msg = "ui_modify_usernameid_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            msg = "ui_modify_usernameid_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                            return false;
                        }
                        p3 = new_cre_id;
                    }
                    else if (ftype == 9) //bruceliao 20121106
                    {
                        string Key = ConfigurationManager.AppSettings["Mbtoken_Unbind_Key"].Trim();
                        string Fuid = PublicRes.ConvertToFuid(fuin);
                        string time_stamp = TimeTransfer.GetTickFromTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        string inmsg = "uin=" + fuin;
                        inmsg += "&optype=" + "1";
                        inmsg += "&uid=" + Fuid;
                        inmsg += "&time_stamp=" + time_stamp;
                        inmsg += "&sign=" + System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Fuid + "1" + time_stamp + Key, "md5").ToLower();

                        string reply;
                        short sresult;

                        if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("mbtoken_unbind_service", inmsg, true, out reply, out sresult, out msg))
                        {
                            if (sresult != 0)
                            {
                                msg = "mbtoken_unbind_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                                return false;
                            }
                            else
                            {
                                if (reply.IndexOf("result=0") > -1)
                                {
                                }
                                else
                                {
                                    msg = "mbtoken_unbind_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            msg = "mbtoken_unbind_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                            return false;
                        }
                    }
                    else if (ftype == 10) //申诉类型第三方令牌
                    {
                        string ip = ConfigurationManager.AppSettings["TCPMobileTokenIP"].Trim();
                        int portNumber;
                        int.TryParse(ConfigurationManager.AppSettings["TCPMobileTokenPORT"].Trim(),out portNumber);//字符串形式port转换成int形式
                        string Key = ConfigurationManager.AppSettings["Mbtoken_Unbind_Key"].Trim();
                        string Fuid = PublicRes.ConvertToFuid(fuin);
                        string time_stamp = TimeTransfer.GetTickFromTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        string request_type = "8161";
                        string inmsg = "request_type=" + request_type;
                        inmsg += "&optype=" + "1";
                        inmsg += "&uid=" + Fuid;
                        inmsg += "&ver=" + "1";
                        inmsg += "&head_u=" + Fuid;
                        inmsg += "&time_stamp=" + time_stamp;
                        inmsg += "&sign=" + System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Fuid + "1" + time_stamp + Key, "md5").ToLower();
                        var parameters = System.Text.Encoding.Default.GetBytes(inmsg);
                        var length = new byte[4];
                        length = BitConverter.GetBytes(parameters.Length);
                        List<byte> bufferIn = new List<byte>();
                        bufferIn.AddRange(length);
                        bufferIn.AddRange(parameters);


                        string token_seq;
                        string sresult;

                        TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetTCPReply(inmsg,bufferIn.ToArray(), ip, portNumber, out msg, "sdyb_mbtoken_itg_srv", out sresult, out token_seq);
                        if (!("0".Equals(sresult)))//sresult="0"表示解绑成功
                        {
                            msg = "sdyb_mbtoken_itg_srv接口返回失败应答：result=" + sresult + "，msg=" + msg + "，token_seq=" + token_seq;
                            return false;
                        }
                    }
                    else
                    {
                        msg = "请求类型不正确";
                        return false;
                    }

                    if (!SendAppealMail(email, ftype, true, username, submittime, p3, p4, "", "", fuin, out msg))
                    {
                        msg = "发送邮件失败：" + msg;
                        return false;
                    }
                    else
                    {

                        if (fstate == 0)
                        {
                            strSql = "update t_tenpay_appeal_trans set FState=1,"
                                + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid=" + fid + " and Fstate=0";
                        }
                        else if (fstate == 2)
                        {
                            strSql = "update t_tenpay_appeal_trans set FState=1,"
                                + " Fcomment='二次审核,拒绝转审核通过." + Fcomment + "', FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid=" + fid + " and Fstate=2";
                        }
                        else if (fstate == 3)
                        {
                            strSql = "update t_tenpay_appeal_trans set FState=1,"
                                + " Fcomment='" + Fcomment + "', FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid=" + fid + " and Fstate=3";
                        }
                        else if (fstate == 4)
                        {
                            strSql = " update t_tenpay_appeal_trans set FState=1,"
                                + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid=" + fid + " and Fstate=4";
                        }
                        else if (fstate == 5)
                        {
                            strSql = "update t_tenpay_appeal_trans set FState=1,"
                                + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid=" + fid + " and Fstate=5";
                        }
                        else if (fstate == 6)
                        {
                            strSql = " update t_tenpay_appeal_trans set FState=1,"
                                + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid=" + fid + " and Fstate=6";
                        }
                        else if (fstate == 8)
                        {
                            strSql = " update t_tenpay_appeal_trans set FState=1,"
                                + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid=" + fid + " and Fstate=8";
                        }
                        // 2012/4/25 添加允许通过短信撤销状态的申诉！，并添加相应的风控通知！
                        else if (fstate == 9)
                        {
                            strSql = " update t_tenpay_appeal_trans set FState=1,"
                                + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid=" + fid + " and Fstate=9";
                        }
                        else
                        {
                            msg = "更新原有记录出错,此记录原始状态不正确";
                            return false;
                        }
                        if (da.ExecSqlNum(strSql) == 1)
                        {
                            InputAppealNumber(user, "Success", "appeal");

                            string IsSendAppeal = ConfigurationManager.AppSettings["IsSendAppeal"].ToString();
                            if (IsSendAppeal == "Yes")
                            {
                                try
                                {
                                    if (ftype == 1 || ftype == 5 || ftype == 6)//发送数据给风控目前只要密码（1）和更换关联手机（5） 2012/4/28 新添fstate=9，ftype=6通知风控2次通过“短信撤销状态”的申诉
                                    {
                                        fuin = System.Web.HttpUtility.UrlEncode(fuin, System.Text.Encoding.GetEncoding("GB2312"));
                                        string ftypestr;
                                        //2014/07/11 xiuling有type=6未发风控才修改
                                        //if (fstate == 9)
                                        //    ftypestr = System.Web.HttpUtility.UrlEncode("6", System.Text.Encoding.GetEncoding("GB2312"));
                                        //else
                                            ftypestr = System.Web.HttpUtility.UrlEncode(ftype.ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        //string fstatestr = System.Web.HttpUtility.UrlEncode(fstate.ToString(),System.Text.Encoding.GetEncoding("GB2312"));
                                        string cre_id = System.Web.HttpUtility.UrlEncode(dr["cre_id"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string clear_ppsstr = System.Web.HttpUtility.UrlEncode(dr["clear_pps"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string is_answer = System.Web.HttpUtility.UrlEncode(dr["labIsAnswer"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        mobile_no = System.Web.HttpUtility.UrlEncode(mobile_no, System.Text.Encoding.GetEncoding("GB2312"));
                                        email = System.Web.HttpUtility.UrlEncode(email, System.Text.Encoding.GetEncoding("GB2312"));
                                        string score = System.Web.HttpUtility.UrlEncode(dr["score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string is_pass = System.Web.HttpUtility.UrlEncode(dr["IsPass"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        client_ip = System.Web.HttpUtility.UrlEncode(client_ip, System.Text.Encoding.GetEncoding("GB2312"));
                                        string reason = System.Web.HttpUtility.UrlEncode(dr["reason"].ToString().Trim(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string fcomment = System.Web.HttpUtility.UrlEncode(Fcomment, System.Text.Encoding.GetEncoding("GB2312")); //备注
                                        string fcheck_info = System.Web.HttpUtility.UrlEncode("", System.Text.Encoding.GetEncoding("GB2312"));   //拒绝原因
                                        string fsubmittime = System.Web.HttpUtility.UrlEncode(dr["FSubmitTime"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string fchecktime = System.Web.HttpUtility.UrlEncode(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Text.Encoding.GetEncoding("GB2312"));

                                        string Data = "fuin=" + fuin + "&ftype=" + ftypestr + "&fstate=1&cre_id=" + cre_id + "&clear_pps=" + clear_ppsstr +
                                            "&is_answer=" + is_answer + "&mobile_no=" + mobile_no + "&email=" + email + "&score=" + score + "&is_pass=" + is_pass +
                                            "&client_ip=" + client_ip + "&fsubmittime=" + fsubmittime + "&fchecktime=" + fchecktime + "&reason=" + reason + "&fcomment=" + fcomment + "&fcheck_info=" + fcheck_info;
                                        Data = System.Web.HttpUtility.UrlEncode(Data, System.Text.Encoding.GetEncoding("GB2312"));
                                        Data = "protocol=user_appeal_notify&version=1.0&data=" + Data;

                                        SunLibrary.LoggerFactory.Get("KF_Service QueryInfo").Info("user_appeal_notify send:" + Data);


                                        System.Text.Encoding GB2312 = System.Text.Encoding.GetEncoding("GB2312");
                                        byte[] sendBytes = GB2312.GetBytes(Data);

                                        string IP = ConfigurationManager.AppSettings["FK_UDP_IP"].ToString();
                                        string PORT = ConfigurationManager.AppSettings["FK_UDP_PORT"].ToString();
                                        TENCENT.OSS.CFT.KF.Common.UDP.GetUDPReplyNoReturnNotReturn(sendBytes, IP, Int32.Parse(PORT));
                                    }
                                }
                                catch
                                {
                                }
                            }
                            return true;
                        }
                        else
                        {
                            msg = "更新原有记录出错,此记录不存在或原始状态不正确";
                            return false;
                        }
                    }
                }
                else
                {
                    msg = "查找记录失败.";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        public static bool ConfirmAppealDBTB(string fid, string db,string tb,string Fcomment, string user, string userIP, out string msg)
        {
            msg = "";

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFTNEW"));
            try
            {
                da.OpenConn();

                if (Fcomment != null && Fcomment.Trim() != "")
                {
                    Fcomment = PublicRes.replaceMStr(Fcomment);
                }

                //先取出一些需要的信息 当前状态,修改类别, QQ号,email
                //修改姓名需要新姓名. 取回支付密码需要clear_pps, 注销不需要多余东西
                string strSql = "select * from " + db + "." + tb + " where FID='" + fid+"'";
                DataSet ds = da.dsGetTotalData(strSql);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    #region
                    HandleParameterByDBTB(ds, true);

                    DataRow dr = ds.Tables[0].Rows[0];
                    int fstate = Int32.Parse(dr["FState"].ToString());
                    int ftype = Int32.Parse(dr["FType"].ToString());
                    string fuin = dr["FUin"].ToString();
                    string email = dr["Femail"].ToString();
                    string question1 = dr["question1"].ToString();
                    string answer1 = dr["answer1"].ToString();
                    string cont_type = dr["cont_type"].ToString();
                    string mobile_no = dr["mobile_no"].ToString();
                    string client_ip = dr["client_ip"].ToString();
                  //  string mobile_bind = dr["FMobile"].ToString();//绑定手机号,取到传给风控
                    string certno = dr["cre_id"].ToString();

                    if (fstate == 1 || fstate == 7)
                    {
                        msg = "原记录的状态不正确";
                        return false;
                    }

                    string nowtime = PublicRes.strNowTimeStander;
                    string new_name = "";
                    int clear_pps = 0;

                    string submittime = DateTime.Parse(dr["FSubmitTime"].ToString()).ToString("yyyy年MM月dd日");

                    //读取现在的用户名？furion 20060902
                    string username = PublicRes.GetNameFromQQ(fuin);
                    //简化注册用户这里处理下
                    if ((username == null || username.Trim() == "") && ftype == 6)
                    {
                        username = "亲爱的财付通客户";
                    }
                    if (username == null || username.Trim() == "")
                    {
                        msg = "取原有用户名时出错";
                        //return false;
                    }

                    string p3 = "";
                    string p4 = "";

                    //					if (!PublicRes.ReleaseCache(fuin,"qqid"))
                    //					{
                    //						Common.CommLib.commRes.sendLog4Log("QUeryInfo.ConfirmAppeal","通过自助申诉时，预清除用户"+ fuin + "cache失败！请检查！");
                    //						msg = "通过自助申诉时，预清除用户"+ fuin + "cache失败！请检查！";
                    //						return false;
                    //					}
                    //进行处理.	 //发送邮件
                    if (ftype == 1)
                    {
                        //找回密码
                        clear_pps = Int32.Parse(dr["clear_pps"].ToString());

                        bool IsNew; //是否新流程
                        if (cont_type != "") //cont_type为新加字段，如果为空就说明走老流程
                        {
                            IsNew = true;

                            if (cont_type == "1")//前端会传回1或者3,3的时候不需要客服绑定
                            {
                                string Fuid = PublicRes.ConvertToFuid(fuin);
                                string client_id = ConfigurationManager.AppSettings["client_id"].ToString();
                                string singed = PublicRes.SingedByService("client_id=" + client_id + "&cmd=update_mobile&mobile=" + mobile_no + "&uid=" + Fuid);

                                //1.找回密码、完整及简化版更换手机都要发 user_appeal_notify 通知
                                //2.涉及到绑定、更换手机，要发验证、通知
                               string old_mobile = "";
                               Query_Service qs = new Query_Service();
                               if (!qs.BindOrChangeMobile(Fuid, fuin, old_mobile, mobile_no, client_ip, certno,singed, out msg))
                                      return false;
                                
                            }
                        }
                        else
                        {
                            IsNew = false;
                        }

                        if (clear_pps == 1)
                        {
                            //替换密保问题和答案
                            if (!CheckQuestion(fuin, userIP, nowtime, question1, answer1, true, out msg)) return false;
                        }

                        string url = ConfigurationManager.AppSettings["GetPassWKeyEmailUrl"].ToString();
                        string urlQQtips = ConfigurationManager.AppSettings["GetPassWKeyTipsUrl"].ToString();
                        string urlMessage = ConfigurationManager.AppSettings["GetPassWKeyMesUrl"].ToString();
                        //发邮件、短信、QQTips (string qqid, int ftype, string url, bool issucc, out string msg)
                        if (!SendAppealMailNew(email, ftype, true, username, url, url, url, "", "", fuin, out msg) )
                        {
                            msg = "发送邮件失败：" + msg;
                            return false;
                        }
                        if (!SendAppealQQTips(fuin, ftype, urlQQtips, true, out msg))
                        {
                            msg = "发送QQTips失败：" + msg;
                            return false;
                        }
                        if (!SendAppealMessage(mobile_no, ftype, urlMessage, true, "", out msg))
                        {
                            msg = "发送短信失败：" + msg;
                            return false;
                        }
                    }

                    else if (ftype == 5 || ftype == 6)
                    {
                        //修改密保
                        clear_pps = Int32.Parse(dr["clear_pps"].ToString());
                      
                            //ftype=5,固定cont_type=3
                            if (cont_type == "3")
                            {
                                string Fuid = PublicRes.ConvertToFuid(fuin);
                                string client_id = ConfigurationManager.AppSettings["client_id"].ToString();
                                string singed = PublicRes.SingedByService("client_id=" + client_id + "&cmd=update_mobile&mobile=" + mobile_no + "&uid=" + Fuid);
                             
                                //1.找回密码、完整及简化版更换手机都要发 user_appeal_notify 通知
                                //2.涉及到绑定、更换手机，要发验证、通知

                                Query_Service qs = new Query_Service();
                                string old_mobile = qs.GetOldBindMobile(Fuid,out msg);
                                if (msg!="")
                                    return false;
                                if (string.IsNullOrEmpty(old_mobile.Trim())){
                                    msg = "old_mobile为空";
                                    return false;
                                }
                                if (!qs.BindOrChangeMobile(Fuid, fuin, old_mobile, mobile_no, client_ip, certno, singed, out msg))
                                    return false;
                            }
                            else
                            {
                                msg = "cont_type不为3";
                                return false;
                            }

                        if (clear_pps == 1)
                        {
                            //替换密保问题和答案  lxl 20131226 只有完整版和修改支付密码有改密保
                            if (!CheckQuestion(fuin, userIP, nowtime, question1, answer1, true, out msg)) return false;
                        }

                        //发邮件、短信(string qqid, int ftype, string url, bool issucc, out string msg) 
                        if (mobile_no == "" || mobile_no.Length != 11)
                        {
                            msg = "mobile不符合规范，mobile=" + mobile_no;
                            return false;
                        }
                        string noticeMobile = mobile_no.Substring(0, 3) + "******" + mobile_no.Substring(9,2);
                        if (!SendAppealMailNew(email, ftype, true, username, noticeMobile, "", "", "", "", fuin, out msg))
                        {
                            msg = "发送邮件失败：" + msg;
                            return false;
                        }
                        if ( !SendAppealMessage(mobile_no, ftype, "", true, "", out msg))
                        {
                            msg = "发送短信失败：" + msg;
                            return false;
                        }
                    }
                    else
                    {
                        msg = "请求类型不正确";
                        return false;
                    }

                    #region
                    if (fstate == 0)
                    {
                        strSql = "update " + db + "." + tb + " set FState=1,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=0";
                    }
                    else if (fstate == 2)
                    {
                        strSql = "update " + db + "." + tb + " set FState=1,"
                            + " Fcomment='二次审核,拒绝转审核通过." + Fcomment + "', FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=2";
                    }
                    else if (fstate == 3)
                    {
                        strSql = "update " + db + "." + tb + " set FState=1,"
                            + " Fcomment='" + Fcomment + "', FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=3";
                    }
                    else if (fstate == 4)
                    {
                        strSql = " update " + db + "." + tb + " set FState=1,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=4";
                    }
                    else if (fstate == 5)
                    {
                        strSql = "update " + db + "." + tb + " set FState=1,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=5";
                    }
                    else if (fstate == 6)
                    {
                        strSql = " update " + db + "." + tb + " set FState=1,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=6";
                    }
                    else if (fstate == 8)
                    {
                        strSql = " update " + db + "." + tb + " set FState=1,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=8";
                    }
                    // 2012/4/25 添加允许通过短信撤销状态的申诉！，并添加相应的风控通知！
                    else if (fstate == 9)
                    {
                        strSql = " update " + db + "." + tb + " set FState=1,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=9";
                    }
                    else
                    {
                        msg = "更新原有记录出错,此记录原始状态不正确";
                        return false;
                    }
                    if (da.ExecSqlNum(strSql) == 1)
                    {
                        InputAppealNumber(user, "Success", "appeal");

                        string IsSendAppeal = ConfigurationManager.AppSettings["IsSendAppeal"].ToString();
                        if (IsSendAppeal == "Yes")
                        {
                            try
                            {
                                if (ftype == 1 || ftype == 5 || ftype == 6)//发送数据给风控目前只要密码（1）和更换关联手机（5） 2012/4/28 新添fstate=9，ftype=6通知风控2次通过“短信撤销状态”的申诉
                                {
                                    fuin = System.Web.HttpUtility.UrlEncode(fuin, System.Text.Encoding.GetEncoding("GB2312"));
                                    string ftypestr;
                                    //if (fstate == 9)
                                    //    ftypestr = System.Web.HttpUtility.UrlEncode("6", System.Text.Encoding.GetEncoding("GB2312"));
                                    //else
                                        ftypestr = System.Web.HttpUtility.UrlEncode(ftype.ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    //string fstatestr = System.Web.HttpUtility.UrlEncode(fstate.ToString(),System.Text.Encoding.GetEncoding("GB2312"));
                                    string cre_id = System.Web.HttpUtility.UrlEncode(dr["cre_id"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string clear_ppsstr = System.Web.HttpUtility.UrlEncode(dr["clear_pps"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string is_answer = System.Web.HttpUtility.UrlEncode(dr["labIsAnswer"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    mobile_no = System.Web.HttpUtility.UrlEncode(mobile_no, System.Text.Encoding.GetEncoding("GB2312"));
                                    email = System.Web.HttpUtility.UrlEncode(email, System.Text.Encoding.GetEncoding("GB2312"));
                                    string score = System.Web.HttpUtility.UrlEncode(dr["score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string is_pass = System.Web.HttpUtility.UrlEncode(dr["IsPass"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    client_ip = System.Web.HttpUtility.UrlEncode(client_ip, System.Text.Encoding.GetEncoding("GB2312"));
                                    string reason = System.Web.HttpUtility.UrlEncode(dr["reason"].ToString().Trim(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string fcomment = System.Web.HttpUtility.UrlEncode(Fcomment, System.Text.Encoding.GetEncoding("GB2312")); //备注
                                    string fcheck_info = System.Web.HttpUtility.UrlEncode("", System.Text.Encoding.GetEncoding("GB2312"));   //拒绝原因
                                    string fsubmittime = System.Web.HttpUtility.UrlEncode(dr["FSubmitTime"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string fchecktime = System.Web.HttpUtility.UrlEncode(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Text.Encoding.GetEncoding("GB2312"));

                                    string Data = "fuin=" + fuin + "&ftype=" + ftypestr + "&fstate=1&cre_id=" + cre_id + "&clear_pps=" + clear_ppsstr +
                                        "&is_answer=" + is_answer + "&mobile_no=" + mobile_no + "&email=" + email + "&score=" + score + "&is_pass=" + is_pass +
                                        "&client_ip=" + client_ip + "&fsubmittime=" + fsubmittime + "&fchecktime=" + fchecktime + "&reason=" + reason + "&fcomment=" + fcomment + "&fcheck_info=" + fcheck_info;
                                    Data = System.Web.HttpUtility.UrlEncode(Data, System.Text.Encoding.GetEncoding("GB2312"));
                                    Data = "protocol=user_appeal_notify&version=1.0&data=" + Data;

                                    SunLibrary.LoggerFactory.Get("KF_Service QueryInfo").Info("user_appeal_notify send:" + Data);

                                    System.Text.Encoding GB2312 = System.Text.Encoding.GetEncoding("GB2312");
                                    byte[] sendBytes = GB2312.GetBytes(Data);

                                    string IP = ConfigurationManager.AppSettings["FK_UDP_IP"].ToString();
                                    string PORT = ConfigurationManager.AppSettings["FK_UDP_PORT"].ToString();
                                    TENCENT.OSS.CFT.KF.Common.UDP.GetUDPReplyNoReturnNotReturn(sendBytes, IP, Int32.Parse(PORT));
                                }
                            }
                            catch
                            {
                            }
                        }
                        return true;
                    }
                    else
                    {
                        msg = "更新原有记录出错,此记录不存在或原始状态不正确";
                        return false;
                    }
                    #endregion

                    #endregion
                }
                else
                {
                    msg = "查找记录失败.";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

      

        public static bool CancelAppeal(int fid, string reason, string OtherReason, string Fcomment, string user, string userIP, out string msg)
        {
            if (reason != null && reason.Trim() != "")
            {
                reason = PublicRes.replaceMStr(reason);
            }


            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            try
            {
                da.OpenConn();
                if (OtherReason != null && OtherReason.Trim() != "")
                {
                    OtherReason = PublicRes.replaceMStr(OtherReason);
                }
                if (Fcomment != null && Fcomment.Trim() != "")
                {
                    Fcomment = PublicRes.replaceMStr(Fcomment);
                }

                //修改姓名需要新姓名. 取回支付密码需要clear_pps, 注销不需要多余东西
                string strSql = "select * from t_tenpay_appeal_trans where Fid=" + fid;
                DataSet ds = da.dsGetTotalData(strSql);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    HandleParameter(ds, true);

                    DataRow dr = ds.Tables[0].Rows[0];
                    int fstate = Int32.Parse(dr["FState"].ToString());
                    int ftype = Int32.Parse(dr["FType"].ToString());
                    string fuin = dr["FUin"].ToString();
                    string email = dr["Femail"].ToString();
                    string submittime = DateTime.Parse(dr["FSubmitTime"].ToString()).ToString("yyyy年MM月dd日");

                    //读取现在的用户名？furion 20060902
                    string username = PublicRes.GetNameFromQQ(fuin);
                    //简化注册用户这里处理下
                    if ((username == null || username.Trim() == "") && ftype == 6)
                    {
                        username = "亲爱的财付通客户";
                    }

                    if (username == null || username.Trim() == "")
                    {
                        msg = "取原有用户名时出错";
                        username = fuin;
                        return false;
                    }
                    string EmailReason = reason;

                    if (ftype == 1)   //新流程中，对于绑定手机的用户就发给他原来绑定的邮箱，对于绑定邮箱的用户就发给录入的邮箱
                    {
                        //找回密码
                        string cont_type = dr["cont_type"].ToString();

                        if (cont_type == "1") //cont_type为新加字段，如果不为空就说明走新流程,cont_type=1时,mobile_no有效 cont_type=2时,femail有效
                        {
                            string Fuid = PublicRes.ConvertToFuid(fuin);

                            if (Fuid == null || Fuid == "")
                            {
                                msg = "该帐号的内部ID为空,查询绑定邮箱失败!";
                                return false;
                            }

                            string sql = " select * from msgnotify_" + Fuid.Substring(Fuid.Length - 3, 2) + ".t_msgnotify_user_" + Fuid.Substring(Fuid.Length - 1, 1) + " where Fuid = '" + Fuid + "'";
                            DataSet emailds = da.dsGetTotalData(sql);
                            if (emailds == null || emailds.Tables.Count == 0 || emailds.Tables[0].Rows.Count == 0)
                            {
                                msg = "该帐号没有绑定邮箱!";
                                return false;
                            }

                            email = emailds.Tables[0].Rows[0]["Femail"].ToString();
                        }

                        EmailReason = reason + "<br/> 温馨提示：<br/> 如果您的账号已经绑定了手机号码，并可有效接收验证码，您可以直接进入<a href='https://www.tenpay.com/v2.0/main/getPwdByMobile_index.shtml'>手机绑定找回支付密码地址</a>通过绑定手机快速找回您的支付密码。&";
                    }
                    //发送失败邮件 失败的一律没有三四参数
                    if (!SendAppealMail(email, ftype, false, username, submittime, "", "", EmailReason, OtherReason, fuin, out msg))
                    {
                        msg = "发送邮件失败：" + msg;
                        return false;
                    }
                    else
                    {
                        if (fstate == 0)
                        {
                            strSql = "update t_tenpay_appeal_trans set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid=" + fid + " and Fstate=0";
                        }
                        else if (fstate == 3)
                        {
                            strSql = "update t_tenpay_appeal_trans set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid=" + fid + " and Fstate=3";
                        }
                        else if (fstate == 4)
                        {
                            strSql = " update t_tenpay_appeal_trans set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid=" + fid + " and Fstate=4";
                        }
                        else if (fstate == 5)
                        {
                            strSql = "update t_tenpay_appeal_trans set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid=" + fid + " and Fstate=5";
                        }
                        else if (fstate == 6)
                        {
                            strSql = " update t_tenpay_appeal_trans set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid=" + fid + " and Fstate=6";
                        }
                        else if (fstate == 8)
                        {
                            strSql = " update t_tenpay_appeal_trans set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid=" + fid + " and Fstate=8";
                        }
                        else
                        {
                            msg = "更新原有记录出错,此记录原始状态不正确";
                            return false;
                        }
                        if (da.ExecSqlNum(strSql) == 1)
                        {
                            InputAppealNumber(user, "Fail", "appeal");
                            string IsSendAppeal = ConfigurationManager.AppSettings["IsSendAppeal"].ToString();
                            if (IsSendAppeal == "Yes")
                            {
                                try
                                {
                                    if (ftype == 1 || ftype == 5)//发送数据给风控目前只要密码（1）和更换关联手机（5）
                                    {
                                        fuin = System.Web.HttpUtility.UrlEncode(fuin, System.Text.Encoding.GetEncoding("GB2312"));
                                        string ftypestr = System.Web.HttpUtility.UrlEncode(ftype.ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        //string fstatestr = System.Web.HttpUtility.UrlEncode(fstate.ToString(),System.Text.Encoding.GetEncoding("GB2312"));
                                        string cre_id = System.Web.HttpUtility.UrlEncode(dr["cre_id"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string clear_ppsstr = System.Web.HttpUtility.UrlEncode(dr["clear_pps"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string is_answer = System.Web.HttpUtility.UrlEncode(dr["labIsAnswer"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string mobile_no = System.Web.HttpUtility.UrlEncode(dr["mobile_no"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        email = System.Web.HttpUtility.UrlEncode(email, System.Text.Encoding.GetEncoding("GB2312"));
                                        string score = System.Web.HttpUtility.UrlEncode(dr["score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string is_pass = System.Web.HttpUtility.UrlEncode(dr["IsPass"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string client_ip = System.Web.HttpUtility.UrlEncode(dr["client_ip"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string reasonstr = System.Web.HttpUtility.UrlEncode(dr["reason"].ToString().Trim(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string fcomment = System.Web.HttpUtility.UrlEncode(Fcomment, System.Text.Encoding.GetEncoding("GB2312")); //备注
                                        string fcheck_info = System.Web.HttpUtility.UrlEncode(reason + OtherReason, System.Text.Encoding.GetEncoding("GB2312"));   //拒绝原因
                                        string fsubmittime = System.Web.HttpUtility.UrlEncode(dr["FSubmitTime"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string fchecktime = System.Web.HttpUtility.UrlEncode(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Text.Encoding.GetEncoding("GB2312"));

                                        string Data = "fuin=" + fuin + "&ftype=" + ftypestr + "&fstate=2&cre_id=" + cre_id + "&clear_pps=" + clear_ppsstr +
                                            "&is_answer=" + is_answer + "&mobile_no=" + mobile_no + "&email=" + email + "&score=" + score + "&is_pass=" + is_pass +
                                            "&client_ip=" + client_ip + "&fsubmittime=" + fsubmittime + "&fchecktime=" + fchecktime + "&reason=" + reasonstr + "&fcomment=" + fcomment + "&fcheck_info=" + fcheck_info;
                                        Data = System.Web.HttpUtility.UrlEncode(Data, System.Text.Encoding.GetEncoding("GB2312"));
                                        Data = "protocol=user_appeal_notify&version=1.0&data=" + Data;

                                        SunLibrary.LoggerFactory.Get("KF_Service QueryInfo").Info("user_appeal_notify send:" + Data);

                                        System.Text.Encoding GB2312 = System.Text.Encoding.GetEncoding("GB2312");
                                        byte[] sendBytes = GB2312.GetBytes(Data);

                                        string IP = ConfigurationManager.AppSettings["FK_UDP_IP"].ToString();
                                        string PORT = ConfigurationManager.AppSettings["FK_UDP_PORT"].ToString();
                                        TENCENT.OSS.CFT.KF.Common.UDP.GetUDPReplyNoReturnNotReturn(sendBytes, IP, Int32.Parse(PORT));
                                    }
                                }
                                catch
                                {
                                }
                            }
                            return true;
                        }
                        else
                        {
                            msg = "更新原有记录出错,此记录不存在或原始状态不正确";
                            return false;
                        }
                    }
                }
                else
                {
                    msg = "查找记录失败.";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        public static bool CancelAppealDBTB(string fid, string db, string tb, string reason, string OtherReason, string Fcomment, string user, string userIP, out string msg)
        {
            if (reason != null && reason.Trim() != "")
            {
                reason = PublicRes.replaceMStr(reason);
            }

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFTNEW"));
            MySqlAccess da2 = new MySqlAccess(PublicRes.GetConnString("CFT"));
            try
            {
                da.OpenConn();
                da2.OpenConn();
                if (OtherReason != null && OtherReason.Trim() != "")
                {
                    OtherReason = PublicRes.replaceMStr(OtherReason);
                }
                if (Fcomment != null && Fcomment.Trim() != "")
                {
                    Fcomment = PublicRes.replaceMStr(Fcomment);
                }

                //修改姓名需要新姓名. 取回支付密码需要clear_pps, 注销不需要多余东西
                string strSql = "select * from " + db + "." + tb + " where FID='" + fid+"'";
                DataSet ds = da.dsGetTotalData(strSql);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    HandleParameterByDBTB(ds, true);

                    DataRow dr = ds.Tables[0].Rows[0];
                    int fstate = Int32.Parse(dr["FState"].ToString());
                    int ftype = Int32.Parse(dr["FType"].ToString());
                    string fuin = dr["FUin"].ToString();
                    string email = dr["Femail"].ToString();
                    string submittime = DateTime.Parse(dr["FSubmitTime"].ToString()).ToString("yyyy年MM月dd日");
                    string mobile = dr["mobile_no"].ToString();
                    string mobile_bind = dr["FMobile"].ToString();//绑定手机号,取到传给风控

                    //读取现在的用户名？furion 20060902
                    string username = PublicRes.GetNameFromQQ(fuin);
                    //简化注册用户这里处理下
                    if ((username == null || username.Trim() == "") && ftype == 6)
                    {
                        username = "亲爱的财付通客户";
                    }

                    if (username == null || username.Trim() == "")
                    {
                        msg = "取原有用户名时出错";
                        username = fuin;
                        return false;
                    }
                    string EmailReason = reason;
                    string mesReason = "";
                    mesReason = reason + "&" + OtherReason;
                    if (ftype == 1)
                    {
                        EmailReason = reason + "<br>温馨提示：</br>如果您的账号已经绑定了手机号码，并可有效接收验证码，您可以直接进入<a href='https://www.tenpay.com/v2.0/main/getPwdByMobile_index.shtml'>手机绑定找回支付密码地址</a>通过绑定手机快速找回您的支付密码。<br>";
                    }

                    if (!SendAppealMessage(mobile, ftype, "", false, mesReason, out msg))
                    {
                        msg = "发送短信失败：" + msg;
                        return false;
                    }
                    //发送失败邮件 失败的一律没有三四参数
                     if (!SendAppealMail(email, ftype, false, username, submittime, "", "", EmailReason, OtherReason, fuin, out msg))
                    {
                        msg = "发送邮件失败：" + msg;
                        return false;
                    }
                    else
                    {
                        if (fstate == 0)
                        {
                            strSql = "update " + db + "." + tb + " set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=0";
                        }
                        else if (fstate == 3)
                        {
                            strSql = "update " + db + "." + tb + " set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=3";
                        }
                        else if (fstate == 4)
                        {
                            strSql = " update " + db + "." + tb + " set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=4";
                        }
                        else if (fstate == 5)
                        {
                            strSql = "update " + db + "." + tb + " set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=5";
                        }
                        else if (fstate == 6)
                        {
                            strSql = " update " + db + "." + tb + " set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=6";
                        }
                        else if (fstate == 8)
                        {
                            strSql = " update " + db + "." + tb + " set FState=2,"
                                + " FCheckInfo='" + reason + OtherReason + "',Fcomment = '" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                                + " FPickTime=now(),FPickUser='" + user + "',"
                                + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                                + " where Fid='" + fid + "' and Fstate=8";
                        }
                        else
                        {
                            msg = "更新原有记录出错,此记录原始状态不正确";
                            return false;
                        }
                        if (da.ExecSqlNum(strSql) == 1)
                        {
                            InputAppealNumber(user, "Fail", "appeal");
                            string IsSendAppeal = ConfigurationManager.AppSettings["IsSendAppeal"].ToString();
                            if (IsSendAppeal == "Yes")
                            {
                                try
                                {
                                    if (ftype == 1 || ftype == 5)//发送数据给风控目前只要密码（1）和更换关联手机（5）
                                    {
                                        fuin = System.Web.HttpUtility.UrlEncode(fuin, System.Text.Encoding.GetEncoding("GB2312"));
                                        string ftypestr = System.Web.HttpUtility.UrlEncode(ftype.ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        //string fstatestr = System.Web.HttpUtility.UrlEncode(fstate.ToString(),System.Text.Encoding.GetEncoding("GB2312"));
                                        string cre_id = System.Web.HttpUtility.UrlEncode(dr["cre_id"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string clear_ppsstr = System.Web.HttpUtility.UrlEncode(dr["clear_pps"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string is_answer = System.Web.HttpUtility.UrlEncode(dr["labIsAnswer"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        mobile_bind = System.Web.HttpUtility.UrlEncode(mobile_bind, System.Text.Encoding.GetEncoding("GB2312"));
                                        email = System.Web.HttpUtility.UrlEncode(email, System.Text.Encoding.GetEncoding("GB2312"));
                                        string score = System.Web.HttpUtility.UrlEncode(dr["score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string is_pass = System.Web.HttpUtility.UrlEncode(dr["IsPass"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string client_ip = System.Web.HttpUtility.UrlEncode(dr["client_ip"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string reasonstr = System.Web.HttpUtility.UrlEncode(dr["reason"].ToString().Trim(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string fcomment = System.Web.HttpUtility.UrlEncode(Fcomment, System.Text.Encoding.GetEncoding("GB2312")); //备注
                                        string fcheck_info = System.Web.HttpUtility.UrlEncode(reason + OtherReason, System.Text.Encoding.GetEncoding("GB2312"));   //拒绝原因
                                        string fsubmittime = System.Web.HttpUtility.UrlEncode(dr["FSubmitTime"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                        string fchecktime = System.Web.HttpUtility.UrlEncode(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Text.Encoding.GetEncoding("GB2312"));

                                        string Data = "fuin=" + fuin + "&ftype=" + ftypestr + "&fstate=2&cre_id=" + cre_id + "&clear_pps=" + clear_ppsstr +
                                            "&is_answer=" + is_answer + "&mobile_no=" + mobile_bind + "&email=" + email + "&score=" + score + "&is_pass=" + is_pass +
                                            "&client_ip=" + client_ip + "&fsubmittime=" + fsubmittime + "&fchecktime=" + fchecktime + "&reason=" + reasonstr + "&fcomment=" + fcomment + "&fcheck_info=" + fcheck_info;
                                        Data = System.Web.HttpUtility.UrlEncode(Data, System.Text.Encoding.GetEncoding("GB2312"));
                                        Data = "protocol=user_appeal_notify&version=1.0&data=" + Data;

                                        SunLibrary.LoggerFactory.Get("KF_Service QueryInfo").Info("user_appeal_notify send:" + Data);

                                        System.Text.Encoding GB2312 = System.Text.Encoding.GetEncoding("GB2312");
                                        byte[] sendBytes = GB2312.GetBytes(Data);

                                        string IP = ConfigurationManager.AppSettings["FK_UDP_IP"].ToString();
                                        string PORT = ConfigurationManager.AppSettings["FK_UDP_PORT"].ToString();
                                        TENCENT.OSS.CFT.KF.Common.UDP.GetUDPReplyNoReturnNotReturn(sendBytes, IP, Int32.Parse(PORT));
                                    }
                                }
                                catch
                                {
                                }
                            }
                            return true;
                        }
                        else
                        {
                            msg = "更新原有记录出错,此记录不存在或原始状态不正确";
                            return false;
                        }
                    }
                }
                else
                {
                    msg = "查找记录失败.";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
                da2.Dispose();
            }
        }

        public static bool DelAppeal(int fid, string Fcomment, string user, string userIP, out string msg)
        {
            msg = "";

            if (Fcomment != null && Fcomment.Trim() != "")
            {
                Fcomment = PublicRes.replaceMStr(Fcomment);
            }

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            try
            {
                da.OpenConn();

                string strSql = " select * from t_tenpay_appeal_trans where Fid=" + fid;
                DataSet ds = da.dsGetTotalData(strSql);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    HandleParameter(ds, true);

                    DataRow dr = ds.Tables[0].Rows[0];
                    int fstate = Int32.Parse(dr["FState"].ToString());
                    int ftype = Int32.Parse(dr["Ftype"].ToString());

                    if (fstate == 0)
                    {
                        strSql = "update t_tenpay_appeal_trans set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid=" + fid + " and Fstate=0";
                    }
                    else if (fstate == 3)
                    {
                        strSql = " update t_tenpay_appeal_trans set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid=" + fid + " and Fstate=3";
                    }
                    else if (fstate == 4)
                    {
                        strSql = " update t_tenpay_appeal_trans set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid=" + fid + " and Fstate=4";
                    }
                    else if (fstate == 5)
                    {
                        strSql = "update t_tenpay_appeal_trans set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid=" + fid + " and Fstate=5";
                    }
                    else if (fstate == 6)
                    {
                        strSql = " update t_tenpay_appeal_trans set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid=" + fid + " and Fstate=6";
                    }
                    else if (fstate == 8)
                    {
                        strSql = " update t_tenpay_appeal_trans set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid=" + fid + " and Fstate=8";
                    }
                    else
                    {
                        msg = "更新原有记录出错,此记录原始状态不正确";
                        return false;
                    }

                    if (da.ExecSqlNum(strSql) == 1)
                    {
                        InputAppealNumber(user, "Delete", "appeal");
                        string IsSendAppeal = ConfigurationManager.AppSettings["IsSendAppeal"].ToString();
                        if (IsSendAppeal == "Yes")
                        {
                            try
                            {
                                if (ftype == 1 || ftype == 5)//发送数据给风控目前只要密码（1）和更换关联手机（5）
                                {
                                    string fuin = System.Web.HttpUtility.UrlEncode(dr["fuin"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string ftypestr = System.Web.HttpUtility.UrlEncode(ftype.ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string fstatestr = System.Web.HttpUtility.UrlEncode(fstate.ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string cre_id = System.Web.HttpUtility.UrlEncode(dr["cre_id"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string clear_ppsstr = System.Web.HttpUtility.UrlEncode(dr["clear_pps"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string is_answer = System.Web.HttpUtility.UrlEncode(dr["labIsAnswer"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string mobile_no = System.Web.HttpUtility.UrlEncode(dr["mobile_no"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string email = System.Web.HttpUtility.UrlEncode(dr["email"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string score = System.Web.HttpUtility.UrlEncode(dr["score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string is_pass = System.Web.HttpUtility.UrlEncode(dr["IsPass"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string client_ip = System.Web.HttpUtility.UrlEncode(dr["client_ip"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string reason = System.Web.HttpUtility.UrlEncode(dr["reason"].ToString().Trim(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string fcomment = System.Web.HttpUtility.UrlEncode(Fcomment, System.Text.Encoding.GetEncoding("GB2312")); //备注
                                    string fcheck_info = System.Web.HttpUtility.UrlEncode("", System.Text.Encoding.GetEncoding("GB2312"));   //拒绝原因
                                    string fsubmittime = System.Web.HttpUtility.UrlEncode(dr["FSubmitTime"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string fchecktime = System.Web.HttpUtility.UrlEncode(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Text.Encoding.GetEncoding("GB2312"));

                                    string Data = "fuin=" + fuin + "&ftype=" + ftypestr + "&fstate=3&cre_id=" + cre_id + "&clear_pps=" + clear_ppsstr +
                                        "&is_answer=" + is_answer + "&mobile_no=" + mobile_no + "&email=" + email + "&score=" + score + "&is_pass=" + is_pass +
                                        "&client_ip=" + client_ip + "&fsubmittime=" + fsubmittime + "&fchecktime=" + fchecktime + "&reason=" + reason + "&fcomment=" + fcomment + "&fcheck_info=" + fcheck_info;
                                    Data = System.Web.HttpUtility.UrlEncode(Data, System.Text.Encoding.GetEncoding("GB2312"));
                                    Data = "protocol=user_appeal_notify&version=1.0&data=" + Data;

                                    SunLibrary.LoggerFactory.Get("KF_Service QueryInfo").Info("user_appeal_notify send:" + Data);

                                    System.Text.Encoding GB2312 = System.Text.Encoding.GetEncoding("GB2312");
                                    byte[] sendBytes = GB2312.GetBytes(Data);

                                    string IP = ConfigurationManager.AppSettings["FK_UDP_IP"].ToString();
                                    string PORT = ConfigurationManager.AppSettings["FK_UDP_PORT"].ToString();
                                    TENCENT.OSS.CFT.KF.Common.UDP.GetUDPReplyNoReturnNotReturn(sendBytes, IP, Int32.Parse(PORT));
                                }
                            }
                            catch
                            {
                            }
                        }
                        return true;
                    }
                    else
                    {
                        msg = "更新原有记录出错,此记录不存在或原始状态不正确";
                        return false;
                    }
                }
                else
                {
                    msg = "查找记录失败.";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        public static bool DelAppealDBTB(string fid, string db, string tb, string Fcomment, string user, string userIP, out string msg)
        {
            msg = "";

            if (Fcomment != null && Fcomment.Trim() != "")
            {
                Fcomment = PublicRes.replaceMStr(Fcomment);
            }

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFTNEW"));
            try
            {
                da.OpenConn();

                string strSql = "select * from " + db + "." + tb + " where FID='" + fid+"'";
                DataSet ds = da.dsGetTotalData(strSql);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                   HandleParameterByDBTB(ds, true);

                    DataRow dr = ds.Tables[0].Rows[0];
                    int fstate = Int32.Parse(dr["FState"].ToString());
                    int ftype = Int32.Parse(dr["Ftype"].ToString());

                    if (fstate == 0)
                    {
                        strSql = "update " + db + "." + tb + " set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=0";
                    }
                    else if (fstate == 3)
                    {
                        strSql = " update " + db + "." + tb + " set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=3";
                    }
                    else if (fstate == 4)
                    {
                        strSql = " update " + db + "." + tb + " set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=4";
                    }
                    else if (fstate == 5)
                    {
                        strSql = "update " + db + "." + tb + " set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=5";
                    }
                    else if (fstate == 6)
                    {
                        strSql = " update " + db + "." + tb + " set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=6";
                    }
                    else if (fstate == 8)
                    {
                        strSql = " update " + db + "." + tb + " set FState=7,"
                            + " Fcomment='" + Fcomment + "',FCheckUser='" + user + "',FCheckTime=Now(),FModifyTime=Now(),"
                            + " FPickTime=now(),FPickUser='" + user + "',"
                            + " FReCheckTime=now(),FRecheckUser='" + user + "'"
                            + " where Fid='" + fid + "' and Fstate=8";
                    }
                    else
                    {
                        msg = "更新原有记录出错,此记录原始状态不正确";
                        return false;
                    }

                    if (da.ExecSqlNum(strSql) == 1)
                    {
                        InputAppealNumber(user, "Delete", "appeal");
                        string IsSendAppeal = ConfigurationManager.AppSettings["IsSendAppeal"].ToString();
                        if (IsSendAppeal == "Yes")
                        {
                            try
                            {
                                if (ftype == 1 || ftype == 5)//发送数据给风控目前只要密码（1）和更换关联手机（5）
                                {
                                    string fuin = System.Web.HttpUtility.UrlEncode(dr["fuin"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string ftypestr = System.Web.HttpUtility.UrlEncode(ftype.ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string fstatestr = System.Web.HttpUtility.UrlEncode(fstate.ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string cre_id = System.Web.HttpUtility.UrlEncode(dr["cre_id"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string clear_ppsstr = System.Web.HttpUtility.UrlEncode(dr["clear_pps"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string is_answer = System.Web.HttpUtility.UrlEncode(dr["labIsAnswer"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string mobile_no = System.Web.HttpUtility.UrlEncode(dr["mobile_no"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string email = System.Web.HttpUtility.UrlEncode(dr["email"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string score = System.Web.HttpUtility.UrlEncode(dr["score"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string is_pass = System.Web.HttpUtility.UrlEncode(dr["IsPass"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string client_ip = System.Web.HttpUtility.UrlEncode(dr["client_ip"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string reason = System.Web.HttpUtility.UrlEncode(dr["reason"].ToString().Trim(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string fcomment = System.Web.HttpUtility.UrlEncode(Fcomment, System.Text.Encoding.GetEncoding("GB2312")); //备注
                                    string fcheck_info = System.Web.HttpUtility.UrlEncode("", System.Text.Encoding.GetEncoding("GB2312"));   //拒绝原因
                                    string fsubmittime = System.Web.HttpUtility.UrlEncode(dr["FSubmitTime"].ToString(), System.Text.Encoding.GetEncoding("GB2312"));
                                    string fchecktime = System.Web.HttpUtility.UrlEncode(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Text.Encoding.GetEncoding("GB2312"));

                                    string Data = "fuin=" + fuin + "&ftype=" + ftypestr + "&fstate=3&cre_id=" + cre_id + "&clear_pps=" + clear_ppsstr +
                                        "&is_answer=" + is_answer + "&mobile_no=" + mobile_no + "&email=" + email + "&score=" + score + "&is_pass=" + is_pass +
                                        "&client_ip=" + client_ip + "&fsubmittime=" + fsubmittime + "&fchecktime=" + fchecktime + "&reason=" + reason + "&fcomment=" + fcomment + "&fcheck_info=" + fcheck_info;
                                    Data = System.Web.HttpUtility.UrlEncode(Data, System.Text.Encoding.GetEncoding("GB2312"));
                                    Data = "protocol=user_appeal_notify&version=1.0&data=" + Data;

                                    SunLibrary.LoggerFactory.Get("KF_Service QueryInfo").Info("user_appeal_notify send:" + Data);

                                    System.Text.Encoding GB2312 = System.Text.Encoding.GetEncoding("GB2312");
                                    byte[] sendBytes = GB2312.GetBytes(Data);

                                    string IP = ConfigurationManager.AppSettings["FK_UDP_IP"].ToString();
                                    string PORT = ConfigurationManager.AppSettings["FK_UDP_PORT"].ToString();
                                    TENCENT.OSS.CFT.KF.Common.UDP.GetUDPReplyNoReturnNotReturn(sendBytes, IP, Int32.Parse(PORT));
                                }
                            }
                            catch
                            {
                            }
                        }
                        return true;
                    }
                    else
                    {
                        msg = "更新原有记录出错,此记录不存在或原始状态不正确";
                        return false;
                    }
                }
                else
                {
                    msg = "查找记录失败.";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

    }



    #endregion

    #region 修改QQ查询类

    public class ChangeQQQueryClass : Query_BaseForNET
    {
        public ChangeQQQueryClass(string userid, string qq)
        {
            string strWhere = " where 1=1 ";

            if (userid != null && userid.Trim() != "")
            {
                strWhere += " and FUserID='" + userid + "' ";
            }

            if (qq != null && qq.Trim() != "")
            {
                strWhere += " and (FOldQQ='" + qq + "' or FNewQQ='" + qq + "') ";
            }

            fstrSql = " select * from c2c_fmdb.t_changeqq_list " + strWhere + " order by FActionTime desc";

            fstrSql_count = " select count(*) from  c2c_fmdb.t_changeqq_list " + strWhere;

        }

    }

    #endregion

    #region 手机充值卡记录查询
    public class FundCardQueryClass : Query_BaseForNET
    {
        public FundCardQueryClass(string u_ID, string fsupplylist, string Fcard_id)
        {
            string strwhere = " where 1=1 ";
            if (u_ID != "" && u_ID != null)
            {
                strwhere += "and Flistid='" + u_ID + "'";
            }

            if (fsupplylist != "" && fsupplylist != null)
            {
                strwhere += "  and  Fsupply_list='" + fsupplylist + "'";
            }

            if (Fcard_id != "" && Fcard_id != null)
            {
                strwhere += "  and  Fcard_id='" + Fcard_id + "'";
            }

            strwhere += "  order by  Fmodify_time  desc";

            fstrSql = "select * from charge_card_db.t_card_list " + strwhere;
            fstrSql_count = "select count(1) from charge_card_db.t_card_list  " + strwhere;
        }

    }
    #endregion

    #region 实名认证处理类

    public class UserClassClass : Query_BaseForNET
    {/*
		public static bool GetUserClassSumInfo(string user, DateTime begintime, DateTime endtime, out string msg)
		{
			msg = "";

			MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("RU"));
			try
			{				
				da.OpenConn();

				string strSql = "select count(*) from authen_process_db.t_authening_info "
					+ " where Fstat=1 and Fcre_stat=2 and Fauthen_type=1 and Fcard_stat=1 " 
					//状态为正常，认证状态为待认证，认证途径为客服认证，银行卡状态为已认证
					+ " and Fpickstate=0 "; //所有未领单

				int itmp = Int32.Parse(da.GetOneResult(strSql));
				msg = "所有未处理申诉：[" + itmp + "];";

				strSql = "select count(*) from authen_process_db.t_authening_info where Fpickstate=1 and Fpickuser='" + user + "'"; //领单数				
				itmp = Int32.Parse(da.GetOneResult(strSql));
				msg += "本人领单数：[" + itmp + "];";

				strSql = "select count(*) from authen_process_db.t_authening_info where Fpickuser='" + user 
					+ "' and Fpickstate>1 and Fpicktime between '" + begintime.ToString("yyyy-MM-dd HH:mm:ss")
					+ "' and '" + endtime.ToString("yyyy-MM-dd HH:mm:ss") + "'"; //指定时间内已处理数

				itmp = Int32.Parse(da.GetOneResult(strSql));

				msg += "本人" + begintime.ToString("yyyy-MM-dd") + "处理数：[" + itmp + "];";
				return true;
			}
			catch(Exception err)
			{
				msg = PublicRes.replaceMStr(err.Message);
				return false;
			}
			finally
			{
				da.Dispose();
			}
			
		}*/

        public static bool HandleParameterX(DataSet ds)
        {
            try
            {
                ds.Tables[0].Columns.Add("Fcre_typeName", typeof(String));
                ds.Tables[0].Columns.Add("FpickstateName", typeof(String));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    string tmp = dr["Fcre_type"].ToString();
                    if (tmp == "1")
                    {
                        dr["Fcre_typeName"] = "身份证";
                    }
                    else if (tmp == "2")
                    {
                        dr["Fcre_typeName"] = "护照";
                    }
                    else if (tmp == "3")
                    {
                        dr["Fcre_typeName"] = "军官证";
                    }
                    else if (tmp == "100")
                    {
                        dr["Fcre_typeName"] = "对公鉴权";
                    }
                    else
                    {
                        dr["Fcre_typeName"] = "未定义";
                    }

                    tmp = dr["Fpickstate"].ToString();
                    if (tmp == "0")
                    {
                        dr["FpickstateName"] = "未处理";
                    }
                    else if (tmp == "1")
                    {
                        dr["FpickstateName"] = "已领单";
                    }
                    else if (tmp == "2")
                    {
                        dr["FpickstateName"] = "认证成功";
                    }
                    else if (tmp == "3")
                    {
                        dr["FpickstateName"] = "认证失败";
                    }
                    else
                    {
                        dr["FpickstateName"] = "未定义" + tmp;
                    }

                }
                return true;
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
        }

        private static bool SendAppealMail(string email, bool issucc, string param1, out string msg)
        {
            msg = "";

            if (PublicRes.IgnoreLimitCheck)
                return true;

            if (email == null || email.Trim() == "")
            {
                //furion 20060902 是否支持不发邮件取决于这里.要么返回真,要么返回假
                return true;
            }

            string filename = ConfigurationManager.AppSettings["ServicePath"].Trim();
            if (!filename.EndsWith("\\"))
                filename += "\\";

            string title = ""; if (issucc) filename += "UserClassYes.htm"; else filename += "UserClassNo.htm";

            StreamReader sr = new StreamReader(filename, System.Text.Encoding.GetEncoding("GB2312"));
            try
            {
                string content = sr.ReadToEnd();

                content = String.Format(content, param1);

                //					MailMessage mail = new MailMessage();
                //					mail.From = "service@tenpay.com";        //发件人
                //					mail.To   = email;          //收件人;
                //					mail.BodyFormat = MailFormat.Html;
                //					mail.Body = content; //邮件内容
                //					mail.Subject  = title;           //邮件主题
                //	
                //					SmtpMail.SmtpServer = ConfigurationManager.AppSettings["OutSmtpServer"].ToString();
                //					SmtpMail.Send(mail);
                //					SmtpMail.SmtpServer = null;
                TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend newMail = new TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend();
                newMail.SendMail(email, "", title, content, true, null);

                return true;
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                sr.Close();
            }
        }



        private static string getCgiString(string instr)
        {
            if (instr == null || instr.Trim() == "")
                return "";

            //System.Text.Encoding enc = System.Text.Encoding.GetEncoding("GB2312");
            return System.Web.HttpContext.Current.Server.UrlDecode(instr).Replace("\r\n", "").Trim()
                .Replace("%3d", "=").Replace("%20", " ").Replace("%26", "&");
        }

        public static bool CommitAppeal(Finance_Header fh, string[] result, string dbstr, out string msg)
        {
            msg = "";
            bool flag = true;

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString(dbstr));
            try
            {
                if (result.Length == 0)
                {
                    return true;
                }

                da.OpenConn();
                string strSql = "";
                foreach (string strresult in result)
                {
                    //成功，失败，都需要发邮件，异常，转后台时不用发邮件。
                    //另外，失败时发邮件失败
                    string[] split = strresult.Split(';');
                    if (split.Length != 3)
                        continue;

                    long id = long.Parse(split[0].Trim());
                    int index = Int32.Parse(split[1].Trim());
                    string idcard = split[2].Trim().ToUpper();

                    //if(index == 0 && idcard.Length != 5)
                    if (index == -1 && idcard.Length != 5)
                        continue;

                    //新加判断
                    if (idcard.Length == 5)
                        index = 0;
                    else
                        index += 1;

                    int emailflag = 0; //0不用发邮件，1发成功邮件，2发失败邮件。
                    string memo = "";

                    strSql = " select Fuid from authen_process_db.t_authening_info where Flist_id=" + id;
                    string fuin = da.GetOneResult(strSql);

                    if (fuin == null || fuin.Trim() == "")
                    {
                        msg += "记录{" + id + "}的帐号读取有错;";
                        flag = false;
                        continue;
                    }

                    //三条分支，0判断后五位是否一样，1直接拒绝，2转后台处理。
                    if (index == 1)
                    {
                        strSql = " update authen_process_db.t_authening_info set Fpicktime=now(),Fmemo='提交证件不合格。',Fpickstate=3"
                            + " where Flist_id=" + id + " and Fpickstate=1";
                        emailflag = 2;
                        memo = "提交证件不合格。";
                    }
                    else if (index == 0)
                    {

                        // TODO: 1客户信息资料外移
                        /*
                        strSql = " select Fcreid from " +  PublicRes.GetTName("t_user_info",fuin) + " where fuid=" + fuin;
                        //string allidcard = PublicRes.ExecuteOne(strSql,"YW");
                        string allidcard = PublicRes.ExecuteOne(strSql,"ZL").ToUpper();
                        */

                        string Msg = "";
                        strSql = "uid=" + fuin;
                        string allidcard = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Fcreid", out Msg);

                        if (allidcard == null || allidcard.Trim() == "")
                        {
                            msg += "记录{" + id + "}的帐号读取有错;" + Msg;
                            flag = false;
                            continue;
                        }

                        if (allidcard.EndsWith(idcard))
                        {

                            //这里才会成功
                            strSql = " update authen_process_db.t_authening_info set Fpicktime=now(),Fmemo='证件合格。',Fpickstate=2"
                                + " where Flist_id=" + id + " and Fpickstate=1";


                            emailflag = 1;
                            memo = "证件合格。";

                        }
                        else //furion 20071106 新加需求，如果是输入帐号有错这种，变成异常转后台。
                        {
                            //失败。
                            strSql = " update authen_process_db.t_authening_info set Fpicktime=now(),Fmemo='提交证件不合格。"
                                + idcard + "',Fpickstate=3"
                                + " where Flist_id=" + id + " and Fpickstate=1";

                            emailflag = 2;

                        }//if allidcard
                    }//if index		

                    //调用接口,如果失败跳过,不发邮件.
                    string strInfoSql = "select Fqqid,Fpath,Fstandby3 from authen_process_db.t_authening_info where Flist_id=" + id;
                    DataTable dtinfo = da.GetTable(strInfoSql);

                    if (dtinfo == null || dtinfo.Rows.Count != 1)
                    {
                        msg += "记录{" + id + "}的信息读取有错;";
                        flag = false;
                        continue;
                    }

                    if (dtinfo.Rows[0]["Fstandby3"].ToString() != "")  //走mandy的新流程   opr_state：0未定义，1确认，2驳回
                    {
                        string inmsg = "uid=" + fuin;
                        inmsg += "&opr_state=" + emailflag;
                        inmsg += "&memo=" + memo;
                        inmsg += "&des_path=" + PublicRes.ICEEncode(dtinfo.Rows[0]["Fpath"].ToString().Trim());
                        inmsg += "&operator=" + fh.UserName;

                        string reply;
                        short sresult;

                        if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("au_sure_cre_check_service", inmsg, true, out reply, out sresult, out msg))
                        {
                            if (sresult != 0)
                            {
                                msg = "au_sure_cre_check_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                                flag = false;
                                continue;
                            }
                            else
                            {
                                if (reply.StartsWith("result=0"))
                                {

                                }
                                else
                                {
                                    msg = "au_sure_cre_check_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                                    flag = false;
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            msg = "au_sure_cre_check_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                            flag = false;
                            continue;
                        }
                    }
                    else   //走老流程
                    {
                        string inmsg = "uin=" + dtinfo.Rows[0]["Fqqid"].ToString().Trim();
                        inmsg += "&opr_state=" + emailflag;
                        inmsg += "&opr_type=1";
                        inmsg += "&des_path=" + PublicRes.ICEEncode(dtinfo.Rows[0]["Fpath"].ToString().Trim());
                        inmsg += "&operator=" + fh.UserName;

                        string reply;
                        short sresult;

                        if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("au_sure_creinfo_service", inmsg, true, out reply, out sresult, out msg))
                        {
                            if (sresult != 0)
                            {
                                msg = "au_sure_creinfo_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                                flag = false;
                                continue;
                            }
                            else
                            {
                                if (reply.StartsWith("result=0"))
                                {

                                }
                                else
                                {
                                    msg = "au_sure_creinfo_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                                    flag = false;
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            msg = "au_sure_creinfo_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                            flag = false;
                            continue;
                        }
                    }

                    int iresult = da.ExecSqlNum(strSql);
                    if (iresult != 1)
                    {
                        msg += "更新记录{" + id + "}时未成功;";
                        flag = false;
                        continue;
                    }

                    //发送邮件根据成功或失败，发送邮件，如果发送失败，就转后台处理。
                    //if(emailflag > 0)
                    if (emailflag < 0) //不再发邮件
                    {
                        //取得此审批的各需要信息.
                        strSql = " select Fuid,Ftruename from authen_process_db.t_authening_info where Flist_id=" + id;
                        DataSet ds = da.dsGetTotalData(strSql);

                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                        {
                            string username = ds.Tables[0].Rows[0]["Ftruename"].ToString();
                            //查询出来email

                            /*
                            strSql = "select Femail from " + PublicRes.GetTName("t_user_info",fuin) + " where Fuid=" + fuin;
                            string email = PublicRes.ExecuteOne(strSql,"ZL");
                            */

                            string Msg = "";
                            strSql = "uid=" + fuin;
                            string email = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Femail", out Msg);

                            if (email == null || email.Trim() == "")
                                continue;

                            bool resultfalg = false;
                            if (emailflag == 1)
                                resultfalg = true;

                            string tmpmsg = "";

                            if (!SendAppealMail(email, resultfalg, username, out tmpmsg))
                            {
                                msg += "发送邮件失败：" + tmpmsg;
                                flag = false;
                                continue;
                            }
                        }// if ds.rows.count > 0
                    } //if emailflag > 0

                }//foreach

                return true;

            }//try
            finally
            {
                da.Dispose();
            }
        }

        public static void InputUserClasslNumber(string User, string Type, string OperationType)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("CFT"));
            try
            {
                da.OpenConn();
                string sql = "select count(1) from t_tenpay_appeal_kf_total where User='" + User + "' and OperationDay='" + DateTime.Today.ToString("yyyy-MM-dd") + "' and OperationType = '" + OperationType + "'";
                if (da.GetOneResult(sql) == "1")
                {
                    if (Type == "Success")
                        sql = "update t_tenpay_appeal_kf_total set UserClassSuccessNum = UserClassSuccessNum + 1 where User='" + User + "' and OperationDay='" + DateTime.Today.ToString("yyyy-MM-dd") + "' and OperationType = '" + OperationType + "'";
                    else if (Type == "Fail")
                        sql = "update t_tenpay_appeal_kf_total set UserClassFailNum = UserClassFailNum + 1 where User='" + User + "' and OperationDay='" + DateTime.Today.ToString("yyyy-MM-dd") + "' and OperationType = '" + OperationType + "'";
                    else if (Type == "Other")
                        sql = "update t_tenpay_appeal_kf_total set UserClassOtherNum = UserClassOtherNum + 1 where User='" + User + "' and OperationDay='" + DateTime.Today.ToString("yyyy-MM-dd") + "' and OperationType = '" + OperationType + "'";
                    else
                    {
                        throw new Exception("没有这种类型!");
                    }
                }
                else
                {
                    if (Type == "Success")
                        sql = "insert into t_tenpay_appeal_kf_total(User,OperationDay,UserClassSuccessNum,OperationType) values('" + User + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "',1,'" + OperationType + "')";
                    else if (Type == "Fail")
                        sql = "insert into t_tenpay_appeal_kf_total(User,OperationDay,UserClassFailNum,OperationType) values('" + User + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "',1,'" + OperationType + "')";
                    else if (Type == "Other")
                        sql = "insert into t_tenpay_appeal_kf_total(User,OperationDay,UserClassOtherNum,OperationType) values('" + User + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "',1,'" + OperationType + "')";
                    else
                    {
                        throw new Exception("没有这种类型!");
                    }
                }

                da.ExecSqlNum(sql);
            }
            catch
            {
                throw new Exception("记录处理统计失败！");
            }
            finally
            {
                da.Dispose();
            }
        }

        public static bool UserClassConfirm(int flist_id, string dbstr, string user, out string msg)
        {
            msg = "";
            int emailflag = 1; //0不用发邮件，1发成功邮件，2发失败邮件。

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString(dbstr));
            try
            {
                // 2012/4/18 改动sql，允许对认证失败的记录进行二次实名认证。
                da.OpenConn();
                //string strSql = " update authen_process_db.t_authening_info set Fpicktime=now(),Fmemo='证件合格。',Fpickstate=2"
                //+ " where Flist_id=" + flist_id.ToString() + " and (Fpickstate=0 or Fpickstate=1)";

                string strSql = " update authen_process_db.t_authening_info set Fpicktime=now(),Fmemo='证件合格。',Fpickstate=2"
                    + " where Flist_id=" + flist_id.ToString() + " and (Fpickstate=0 or Fpickstate=1 or Fpickstate=3)";

                //调用接口,如果失败跳过,不发邮件.
                string strInfoSql = "select Fuid,Fqqid,Fpath,Fstandby3 from authen_process_db.t_authening_info where Flist_id=" + flist_id.ToString();
                DataTable dtinfo = da.GetTable(strInfoSql);

                if (dtinfo == null || dtinfo.Rows.Count != 1)
                {
                    msg += "记录{" + flist_id.ToString() + "}的信息读取有错;";
                    return false;
                }

                if (dtinfo.Rows[0]["Fstandby3"].ToString() != "")  //走mandy的新流程   opr_state：0未定义，1确认，2驳回
                {
                    string inmsg = "uid=" + dtinfo.Rows[0]["Fuid"].ToString();
                    inmsg += "&opr_state=" + emailflag;
                    inmsg += "&memo=证件合格。";
                    inmsg += "&des_path=" + PublicRes.ICEEncode(dtinfo.Rows[0]["Fpath"].ToString().Trim());
                    inmsg += "&operator=" + user;

                    string reply;
                    short sresult;

                    if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("au_sure_cre_check_service", inmsg, true, out reply, out sresult, out msg))
                    {
                        if (sresult != 0)
                        {
                            msg = "au_sure_cre_check_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                            return false;
                        }
                        else
                        {
                            if (reply.StartsWith("result=0"))
                            {

                            }
                            else
                            {
                                msg = "au_sure_cre_check_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                                return false;
                            }
                        }
                    }
                    else
                    {
                        msg = "au_sure_cre_check_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                        return false;
                    }
                }
                else
                {
                    string inmsg = "uin=" + dtinfo.Rows[0]["Fqqid"].ToString().Trim();
                    inmsg += "&opr_state=" + emailflag;
                    inmsg += "&opr_type=1";
                    inmsg += "&des_path=" + PublicRes.ICEEncode(dtinfo.Rows[0]["Fpath"].ToString().Trim());
                    inmsg += "&operator=" + user;

                    string reply;
                    short sresult;

                    if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("au_sure_creinfo_service", inmsg, true, out reply, out sresult, out msg))
                    {
                        if (sresult != 0)
                        {
                            msg = "au_sure_creinfo_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                            return false;
                        }
                        else
                        {
                            if (reply.StartsWith("result=0"))
                            {

                            }
                            else
                            {
                                msg = "au_sure_creinfo_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                                InputUserClasslNumber(user, "Other", "appeal");
                                return false;
                            }
                        }
                    }
                    else
                    {
                        msg = "au_sure_creinfo_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                        return false;
                    }
                }

                int iresult = da.ExecSqlNum(strSql);
                if (iresult != 1)
                {
                    msg += "更新记录{" + flist_id.ToString() + "}时未成功;";
                    return false;
                }

                InputUserClasslNumber(user, "Success", "appeal");
                return true;
            }
            finally
            {
                da.Dispose();
            }
        }


        public static bool UserClassCancel(int flist_id, string reason, string OtherReason, string dbstr, string user, out string msg)
        {
            msg = "";
            int emailflag = 2; //0不用发邮件，1发成功邮件，2发失败邮件。

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString(dbstr));
            try
            {
                da.OpenConn();
                string strSql = " update authen_process_db.t_authening_info set Fpicktime=now(),Fmemo='" + reason + OtherReason + "',Fpickstate=3"
                    + " where Flist_id=" + flist_id.ToString() + " and (Fpickstate=0 or Fpickstate=1)";

                //调用接口,如果失败跳过,不发邮件.
                string strInfoSql = "select Fuid,Fqqid,Fpath,Fstandby3 from authen_process_db.t_authening_info where Flist_id=" + flist_id.ToString();
                DataTable dtinfo = da.GetTable(strInfoSql);

                if (dtinfo == null || dtinfo.Rows.Count != 1)
                {
                    msg += "记录{" + flist_id.ToString() + "}的信息读取有错;";
                    return false;
                }

                if (dtinfo.Rows[0]["Fstandby3"].ToString() != "")  //走mandy的新流程   opr_state：0未定义，1确认，2驳回
                {
                    string inmsg = "uid=" + dtinfo.Rows[0]["Fuid"].ToString();
                    inmsg += "&opr_state=" + emailflag;
                    inmsg += "&memo=" + reason + OtherReason;
                    inmsg += "&des_path=" + PublicRes.ICEEncode(dtinfo.Rows[0]["Fpath"].ToString().Trim());
                    inmsg += "&operator=" + user;

                    string reply;
                    short sresult;

                    if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("au_sure_cre_check_service", inmsg, true, out reply, out sresult, out msg))
                    {
                        if (sresult != 0)
                        {
                            msg = "au_sure_cre_check_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                            return false;
                        }
                        else
                        {
                            if (reply.StartsWith("result=0"))
                            {

                            }
                            else
                            {
                                msg = "au_sure_cre_check_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                                return false;
                            }
                        }
                    }
                    else
                    {
                        msg = "au_sure_cre_check_service接口失败：result=" + sresult + "，msg=" + msg + "，reply=" + reply;
                        return false;
                    }
                }
                else
                {
                    string inmsg = "uin=" + dtinfo.Rows[0]["Fqqid"].ToString().Trim();
                    inmsg += "&opr_state=" + emailflag;
                    inmsg += "&opr_type=1";
                    inmsg += "&des_path=" + PublicRes.ICEEncode(dtinfo.Rows[0]["Fpath"].ToString().Trim());
                    inmsg += "&operator=" + user;

                    string reply;
                    short sresult;

                    if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("au_sure_creinfo_service", inmsg, true, out reply, out sresult, out msg))
                    {
                        if (sresult != 0)
                        {
                            msg = "au_sure_creinfo_service接口失败：result=" + sresult + "，msg=" + msg;
                            return false;
                        }
                        else
                        {
                            if (reply.StartsWith("result=0"))
                            {

                            }
                            else
                            {
                                msg = "au_sure_creinfo_service接口失败：result=" + sresult + "，msg=" + msg;
                                return false;
                            }
                        }
                    }
                    else
                    {
                        msg = "au_sure_creinfo_service接口失败：result=" + sresult + "，msg=" + msg;
                        return false;
                    }
                }

                int iresult = da.ExecSqlNum(strSql);
                if (iresult != 1)
                {
                    msg += "更新记录{" + flist_id.ToString() + "}时未成功;";
                    return false;
                }

                InputUserClasslNumber(user, "Fail", "appeal");

                return true;
            }
            finally
            {
                da.Dispose();
            }
        }

        /*
                public static DataSet UserClassClassPickTJ(Finance_Header fh, string dbstr,DateTime begindate, DateTime enddate, out string msg)
                {
                    msg = "";
                    MySqlAccess da = new MySqlAccess(PublicRes.GetConnString(dbstr));
                    try
                    {
                        da.OpenConn();

                        string strSql = " select Fpickuser,Fpickstate,count(*) from authen_process_db.t_authening_info "
                            + " where Fpicktime>='" + begindate.ToString("yyyy-MM-dd HH:mm:ss")
                            //+ "' and FSubmitTime<='" + enddate.ToString("yyyy-MM-dd HH:mm:ss") + "' and FState >0 "
                            + "' and Fpicktime<='" + enddate.ToString("yyyy-MM-dd HH:mm:ss") + "' and FpickState>1 "
                            + " group by Fpickuser,Fpickstate ";

                        DataSet ds = new DataSet();
                        DataTable dtresult = new DataTable();
                        ds.Tables.Add(dtresult);
                        dtresult.Columns.Add("Fpickuser",typeof(System.String));
                        dtresult.Columns.Add("FCommitCount",typeof(System.String));
                        dtresult.Columns.Add("FCancelCount",typeof(System.String));
                        dtresult.Columns.Add("FTotalCount",typeof(System.String));

                        DataTable dt = da.GetTable(strSql);
                        if(dt == null || dt.Rows.Count == 0)
                            return ds;

				
                        int commitcount = 0;
                        int cancelcount = 0;
                        int totalcount = 0;

                        int rowindex = 0;
                        Hashtable ht = new Hashtable();
                        foreach(DataRow dr in dt.Rows)
                        {
                            string struser = "";
                            if(dr["Fpickuser"] != null)
                            {
                                struser = dr["Fpickuser"].ToString().Trim();
                            }
                            else
                                continue;

                            int state = Int32.Parse(dr["Fpickstate"].ToString());
                            int count = Int32.Parse(dr[2].ToString());

                            if(ht.ContainsKey(struser))
                            {
                                //此用户的记录已存在
                                int currrowindex = Int32.Parse(ht[struser].ToString());
                                dtresult.Rows[currrowindex]["FTotalCount"] = Int32.Parse(dtresult.Rows[currrowindex]["FTotalCount"].ToString()) + count;
						
                                switch(state)
                                {
                                    case 2:
                                        dtresult.Rows[currrowindex]["FCommitCount"] = count;	
                                        commitcount += count;
                                        break;
                                    case 3:
                                        dtresult.Rows[currrowindex]["FCancelCount"] = count;	
                                        cancelcount += count;
                                        break;
                                    default:
                                        break;
                                }
                            }//if containskey
                            else
                            {
                                //此用户记录不存在
                                //加入用户
                                DataRow drresult = dtresult.NewRow();
                                drresult["Fpickuser"] = struser;
                                drresult["FTotalCount"] = count;
                                drresult["FCommitCount"] = 0;
                                drresult["FCancelCount"] = 0;

                                switch(state)
                                {
                                    case 2:
                                        drresult["FCommitCount"] = count;	
                                        commitcount += count;
                                        break;
                                    case 3:
                                        drresult["FCancelCount"] = count;			
                                        cancelcount += count;
                                        break;
                                    default:
                                        break;
                                }

                                dtresult.Rows.Add(drresult);

                                ht.Add(struser,rowindex);
                                rowindex ++;
                            }//if containskey else
                        } //for each

                        totalcount = commitcount + cancelcount ;
                        DataRow drtotal = dtresult.NewRow();
                        drtotal["Fpickuser"] = "合计";
                        drtotal["FCommitCount"] = commitcount;
                        drtotal["FCancelCount"] = cancelcount;	
                        drtotal["FTotalCount"] = totalcount;

                        dtresult.Rows.Add(drtotal);

                        return ds;
                    }
                    catch(Exception err)
                    {
                        msg = err.Message;
                        return null;
                    }
                    finally
                    {
                        da.Dispose();
                    }
			
                }

                public static DataSet GetPickList(string fuin, int imax, string dbstr, int flag)
                {
                    DataSet ds = new DataSet();
                    MySqlAccess da = new MySqlAccess(PublicRes.GetConnString(dbstr));
                    try
                    {
                        fuin = fuin.Replace("\\","\\\\").Replace("'","\\'").Trim();
                        da.OpenConn();
                        //取记录,打领用标记. 不处理销户申批.
                        string strSql = "";
                        string begindate = DateTime.Today.AddDays(-3).ToString("yyyy-MM-dd 00:00:00");
                        string enddate = DateTime.Today.ToString("yyyy-MM-dd 23:59:59");

                        if(flag == 0)
                        {
                            strSql = " select count(*) from authen_process_db.t_authening_info where Fpickuser='" + fuin + "' and Fpickstate=1 "
                                + " and  Fstat=1 and Fcre_stat=2 and Fauthen_type=1 and Fcard_stat=1 and Fmodify_time between '" + begindate + "' and '" + enddate + "'";

                            int icurr = Int32.Parse(da.GetOneResult(strSql));
                            if(icurr < imax)
                            {
                                icurr = imax - icurr;
                                strSql = " update authen_process_db.t_authening_info set FPickUser='" + fuin + "',Fpickstate=1 "
                                    + " where Fstat=1 and Fcre_stat=2 and Fauthen_type=1 and Fcard_stat=1 " 
                                    //状态为正常，认证状态为待认证，认证途径为客服认证，银行卡状态为已认证
                                    + " and Fpickstate=0 and Fmodify_time between '" + begindate + "' and '" + enddate + "' limit " + icurr;
                                da.ExecSql(strSql);
                            }

                            strSql = " select Flist_id,Fcre_type,Fpath,FPickState from authen_process_db.t_authening_info "
                                + " where FPickUser='" + fuin + "' and Fpickstate=1  "
                                + " and  Fstat=1 and Fcre_stat=2 and Fauthen_type=1 and Fcard_stat=1 and Fmodify_time between '" + begindate + "' and '" + enddate + "' limit " + imax;

                            ds = da.dsGetTotalData(strSql);
                        }
                        else if(flag == 1)
                        {
                            strSql = " select Flist_id,Fcre_type,Fpath,FPickState from authen_process_db.t_authening_info "
                                + " where Fstat=1 and Fcre_stat=2 and Fauthen_type=1 and Fcard_stat=1 " 
                                //状态为正常，认证状态为待认证，认证途径为客服认证，银行卡状态为已认证
                                + " and Fpickstate=0  and Fmodify_time between '" + begindate + "' and '" + enddate + "' limit " + imax;
                            //where FState=0 and FPickUser is null and  FType<4  limit " + imax;

                            ds = da.dsGetTotalData(strSql);

                            if(ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                            {
                                foreach(DataRow dr in ds.Tables[0].Rows)
                                {
                                    string tmpfid = dr["Flist_id"].ToString().Trim();

                                    strSql = " update authen_process_db.t_authening_info set FPickUser='" + fuin + "',Fpickstate=1 "
                                        + " where Flist_id='" + tmpfid + "' and Fpickstate=0 ";

                                    da.ExecSql(strSql);
                                }
                            }
					
                        }
				
                    }
                    catch(Exception err)
                    {
                        throw new LogicException(err.Message);
                    }
                    finally
                    {
                        da.Dispose();
                    }

                    return ds;
                }

                */

        public UserClassClass(string u_BeginTime, string u_EndTime, string fuin, int fstate, string QQType, int SortType)
        {

            string strWhere = " where Fstat=1 and Fcre_stat=2 and Fauthen_type=1 and Fcard_stat=1 ";

            if (fuin != null && fuin != "")
            {
                strWhere += " and Fqqid = '" + fuin + "' ";
            }

            if (u_BeginTime != null && u_BeginTime != "")
            {
                strWhere += " and Fmodify_time between '" + u_BeginTime + "' and '" + u_EndTime + "' ";
            }

            if (fstate != 99)
            {
                strWhere += " and Fpickstate=" + fstate + "  ";
            }

            //"" 所有类型; "0" 非会员; "1" 普通会员; "2" VIP会员
            //用户等级及vip标识(Fstandby1)
            //非cft会员及等级： 0-6
            //cft会员及等级： 100-106
            //vip会员及等级： 200-206
            //连续1个月不做任务的普通会员（同非会员）：400-406
            //0是默认值
            if (QQType == "0")
            {
                strWhere += " and (Fstandby1 < 100 or Fstandby1 >= 300) ";
            }
            else if (QQType == "1")
            {
                strWhere += " and Fstandby1>=100 and Fstandby1<200 ";
            }
            else if (QQType == "2")
            {
                strWhere += " and Fstandby1>=200 and Fstandby1<300 ";
            }

            if (SortType != 99)
            {
                if (SortType == 0)   //排序：时间小到大
                    strWhere += " order by Fcreate_time asc ";
                if (SortType == 1)   //排序：时间大到小
                    strWhere += " order by Fcreate_time desc ";
            }

            fstrSql = "select Fpickstate,Fpickuser,Fpicktime,Fpath,Fcre_type,Fqqid,Fmemo,Flist_id,Fcreate_time from authen_process_db.t_authening_info "
                + strWhere;
            fstrSql_count = "select count(1) from authen_process_db.t_authening_info " + strWhere;
        }

        public UserClassClass(string fuin, string Flag)
        {
            fstrSql = "select Fpickstate,Fpickuser,Fcard_stat,Fpicktime,Fpath,Fcre_type,Fqqid,Fmemo,Flist_id,Fcreate_time,Fcre_stat from authen_process_db.t_authening_info where Fqqid = '" + fuin + "' and Fauthen_type=1 " +
                "order by Fmodify_time ";
            fstrSql_count = "select count(1) from authen_process_db.t_authening_info where Fqqid = '" + fuin + "' and Fauthen_type=1 ";
        }

        public UserClassClass(int flist_id)
        {
            fstrSql = "select Fpickstate,Fpickuser,Fcard_stat,Fpicktime,Fpath,Fcre_type,Fqqid,Fmemo,Flist_id,Fcreate_time from authen_process_db.t_authening_info where flist_id=" + flist_id.ToString();
            fstrSql_count = "select count(1) from authen_process_db.t_authening_info where flist_id=" + flist_id.ToString();
        }

        public static DataSet GetLockList(DateTime BeginDate, DateTime EndDate, int fstate, string username, int Count)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("RU"));
            da.OpenConn();

            DataSet ds = new DataSet();

            try
            {
                string strWhere = "where Fstat=1 and Fcre_stat=2 and Fauthen_type=1 and Fcard_stat=1 " +
                    "and Fmodify_time between '" + BeginDate.ToString("yyyy-MM-dd 00:00:00") + "' and '" + EndDate.ToString("yyyy-MM-dd 23:59:59") + "' ";

                if (fstate == 99)
                    fstate = 0;

                if (fstate != 0 && fstate != 1)
                {
                    throw new Exception("改状态不允许批量领单!");
                }
                strWhere += " and Fpickstate=" + fstate + "  ";

                if (username == null || username == "")
                {
                    throw new Exception("批量领单人不允许为空!");
                }

                string strSql = "select Fuid,Flist_id,Fpickstate,Fpickuser,Fcard_stat,Fpicktime,Fpath,Fcre_type,Fqqid,Fmemo,Fcreate_time from authen_process_db.t_authening_info "
                    + strWhere + "  order by Fmodify_time limit " + Count;

                ds = da.dsGetTotalData(strSql);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string WhereStr = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        WhereStr += "'" + dr["Flist_id"].ToString().Trim() + "',";
                    }
                    WhereStr = WhereStr.Substring(0, WhereStr.Length - 1);

                    strSql = " update authen_process_db.t_authening_info set FPickUser='" + username + "',FPickTime=now(),Fpickstate=1 where Flist_id in(" + WhereStr + ")";

                    da.ExecSql(strSql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                da.Dispose();
            }

            return ds;

        }



        public static DataSet GetDeleteList(string Fqqid)
        {
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("RU"));
            da.OpenConn();

            try
            {
                string strSql = "select Fauthen_operator,Fqqid,Fmemo,Fmodify_time from authen_process_db.t_authening_info " +
                    "where Fqqid='" + Fqqid + "' and Fmemo='客服删除' order by Fmodify_time desc limit 0,100";

                return da.dsGetTotalData(strSql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                da.Dispose();
            }

        }

    }


    #endregion

    #region  系统公告类
    public class SysBulletinClass
    {
        public static bool GoPrior(string fid, string userip, out string msg)
        {
            msg = "";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
            try
            {
                da.OpenConn();
                string strSql = "select FOrder from c2c_db_inc.t_bulletin_info where FID=" + fid
                    + " and Fstate=1 and Flist_state=1";
                int noworder = Int32.Parse(da.GetOneResult(strSql));

                strSql = "select FOrder from c2c_db_inc.t_bulletin_info where FSysID=(select FSysID "
                    + " from c2c_db_inc.t_bulletin_info where FID=" + fid + ") and FOrder>"
                    + noworder + " and Fstate=1 and Flist_state=1 order by FOrder limit 1";

                string obj = da.GetOneResult(strSql);

                if (obj != null)
                {
                    int maxorder = Int32.Parse(obj);

                    strSql = "update c2c_db_inc.t_bulletin_info set FOrder=" + noworder
                        + ",FModify_time=now(),FIP='" + userip + "' where FOrder=" + maxorder
                        + " and Fstate=1 and Flist_state=1";
                    if (da.ExecSqlNum(strSql) == 1)
                    {
                        strSql = "update c2c_db_inc.t_bulletin_info set FOrder=" + maxorder
                            + ",FModify_time=now(),FIP='" + userip + "' where FID=" + fid
                            + " and Fstate=1 and Flist_state=1";
                        if (da.ExecSqlNum(strSql) == 1)
                            return true;
                        else
                        {
                            msg = "有一条记录更新出错";

                            strSql = "update c2c_db_inc.t_bulletin_info set FOrder=" + maxorder
                                + ",FModify_time=now(),FIP='" + userip
                                + "' where FOrder=" + noworder + " and FID<>" + fid
                                + " and Fstate=1 and Flist_state=1";
                            da.ExecSql(strSql);

                            return false;
                        }
                    }
                    else
                        return true;
                }
                else
                    return true;
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }

        }

        public static bool GoNext(string fid, string userip, out string msg)
        {
            msg = "";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
            try
            {
                da.OpenConn();
                string strSql = "select FOrder from c2c_db_inc.t_bulletin_info where FID="
                    + fid + " and Fstate=1 and Flist_state=1";

                int noworder = Int32.Parse(da.GetOneResult(strSql));

                strSql = "select FOrder from c2c_db_inc.t_bulletin_info where FSysID=(select FSysID "
                    + " from c2c_db_inc.t_bulletin_info where FID=" + fid + ") and FOrder<"
                    + noworder + " and Fstate=1 and Flist_state=1 order by FOrder desc limit 1";

                string obj = da.GetOneResult(strSql);

                if (obj != null)
                {
                    int minorder = Int32.Parse(obj);

                    strSql = "update c2c_db_inc.t_bulletin_info set FOrder=" + noworder
                        + ",FModify_time=now(),FIP='" + userip + "' where FOrder=" + minorder
                        + " and Fstate=1 and Flist_state=1";
                    if (da.ExecSqlNum(strSql) == 1)
                    {
                        strSql = "update c2c_db_inc.t_bulletin_info set FOrder=" + minorder
                            + ",FModify_time=now(),FIP='" + userip + "' where FID=" + fid
                            + " and Fstate=1 and Flist_state=1";

                        if (da.ExecSqlNum(strSql) == 1)
                            return true;
                        else
                        {
                            msg = "有一条记录更新出错";

                            strSql = "update c2c_db_inc.t_bulletin_info set FOrder=" + minorder
                                + ",FModify_time=now(),FIP='" + userip
                                + "' where FOrder=" + noworder + " and FID<>" + fid + " and Fstate=1 and Flist_state=1";
                            da.ExecSql(strSql);

                            return false;
                        }
                    }
                    else
                        return true;
                }
                else
                    return true;
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        public static bool GoHistory(string fid, string userip, out string msg)
        {
            msg = "";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
            try
            {
                da.OpenConn();
                string strSql = "select FSysID from c2c_db_inc.t_bulletin_info where FID=" + fid
                    + " and Flist_state=1 and Fstate=1";

                string obj = da.GetOneResult(strSql);

                if (obj != null)
                {
                    int sysid = Int32.Parse(obj);
                    if (sysid != 2)
                    {
                        msg = "只有财付通系统首页公告才能转移到历史";
                        return false;
                    }
                    else
                    {
                        strSql = "update c2c_db_inc.t_bulletin_info set FSysID=3,FModify_time=now(),FIP='" + userip + "' where FID=" + fid;
                        if (da.ExecSqlNum(strSql) == 1)
                        {
                            return true;
                        }
                        else
                        {
                            msg = "更新记录出错";
                            return false;
                        }
                    }
                }
                else
                {
                    msg = "不存在正确的记录";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        public static bool Del(string fid, string userip, out string msg)
        {
            msg = "";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
            try
            {
                da.OpenConn();
                string strSql = "update c2c_db_inc.t_bulletin_info set Flist_state=2,FModify_time=now(),FIP='" + userip + "' where FID=" + fid;

                da.ExecSql(strSql);
                return true;
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        public static bool Issue(string sysid, out string msg)
        {
            msg = "";
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
            try
            {
                da.OpenConn();

                string strSql = "delete from c2c_db_inc.t_bulletin_info  where FSysID="
                    + sysid + " and Fstate=2";
                da.ExecSql(strSql);

                strSql = "insert c2c_db_inc.t_bulletin_info(FSysID,FOrder,FIsNew,FTitle,FUrl,FissueTime,FLastTime,FuserId,FModify_time,Fstate,Flist_state,Fip,FStandBy1)"
                    + " select FSysID,FOrder,FIsNew,FTitle,FUrl,FissueTime,FLastTime,FuserId,now(),2,1,Fip,FStandBy1"
                    + " from c2c_db_inc.t_bulletin_info where FSysID=" + sysid + " and Fstate=1 and Flist_state=1";

                da.ExecSql(strSql);

                return true;
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        public int FID = 0;
        public int FSysID = 1;
        public int FIsNew = 1;
        public string FTitle = "";
        public string FUrl = "";
        public string FissueTime = "";
        public string FLastTime = "";
        public string FuserId = "";

        public int FIsRed = 1;

        private bool ValidParam(out string msg)
        {
            msg = "";
            //校验开始
            //			if(FSysID <1 || FSysID > 4)
            //			{
            //				msg = "系统ID不正确";
            //				return false;
            //			}

            if (FIsNew < 1 || FIsNew > 2)
            {
                msg = "是否为最新字段有误";
                return false;
            }

            if (FIsRed < 0 || FIsRed > 1)
            {
                msg = "是否为红字字段有误";
                return false;
            }

            if (FTitle == null || FTitle.Trim() == "")
            {
                msg = "请给出公告的标题。";
                return false;
            }

            try
            {
                FissueTime = DateTime.Parse(FissueTime.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                if (FLastTime != null && FLastTime.Trim() != "")
                {
                    FLastTime = DateTime.Parse(FLastTime.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            catch
            {
                msg = "请给出正确的时间";
                return false;
            }

            if (FUrl == null || FUrl.Trim() == "")
            {
                msg = "请给出链接";
                return false;
            }

            if (FuserId == null || FuserId.Trim() == "")
            {
                msg = "请给出发布人";
                return false;
            }
            return true;
        }

        public bool Change(string userip, out string msg)
        {
            msg = "";
            if (!ValidParam(out msg))
            {
                return false;
            }

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
            try
            {
                da.OpenConn();
                string strSql = "select count(*) from c2c_db_inc.t_bulletin_info where FID=" + FID
                    + "  and Fstate=1 and Flist_state=1";

                if (da.GetOneResult(strSql) != "1")
                {
                    msg = "找不到原有记录";
                    return false;
                }

                if (FLastTime == null || FLastTime.Trim() == "")
                {
                    strSql = "update c2c_db_inc.t_bulletin_info set FIsNew={0},FTitle='{1}',FUrl='{2}',FissueTime='{3}',"
                        + "FuserId='{4}',Fip='{5}',FModify_time=now(),FStandBy1={6} where FID=" + FID;

                    strSql = String.Format(strSql, FIsNew, FTitle, FUrl, FissueTime, FuserId, userip, FIsRed);
                }
                else
                {
                    strSql = "update c2c_db_inc.t_bulletin_info set FIsNew={0},FTitle='{1}',FUrl='{2}',FissueTime='{3}',FLastTime='{4}',"
                        + "FuserId='{5}',Fip='{6}',FModify_time=now(),FStandBy1={7} where FID=" + FID;

                    strSql = String.Format(strSql, FIsNew, FTitle, FUrl, FissueTime, FLastTime, FuserId, userip, FIsRed);
                }

                if (da.ExecSqlNum(strSql) == 1)
                    return true;
                else
                {
                    msg = "更新记录失败";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        public bool CreateNew(string userip, out string msg)
        {
            msg = "";
            if (!ValidParam(out msg))
            {
                return false;
            }

            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
            try
            {
                da.OpenConn();
                string strSql = "select ifnull(max(Forder),0) from c2c_db_inc.t_bulletin_info where"
                    + "   Fstate=1 and Flist_state=1";

                int maxorder = Int32.Parse(da.GetOneResult(strSql)) + 1;

                if (FLastTime == null || FLastTime.Trim() == "")
                {
                    strSql = "Insert c2c_db_inc.t_bulletin_info(FSysID,FOrder,FIsNew,FTitle,FUrl,FissueTime"
                        + ",FuserId,FModify_time,Fstate,Flist_state,Fip,FStandBy1) "
                        + " values({0},{1},{2},'{3}','{4}','{5}','{6}',now(),1,1,'{7}',{8})";

                    strSql = String.Format(strSql, FSysID, maxorder, FIsNew, FTitle, FUrl, FissueTime, FuserId, userip, FIsRed);
                }
                else
                {
                    strSql = "Insert c2c_db_inc.t_bulletin_info(FSysID,FOrder,FIsNew,FTitle,FUrl,FissueTime,FLastTime"
                        + ",FuserId,FModify_time,Fstate,Flist_state,Fip,FStandBy1) "
                        + " values({0},{1},{2},'{3}','{4}','{5}','{6}','{7}',now(),1,1,'{8}',{9})";

                    strSql = String.Format(strSql, FSysID, maxorder, FIsNew, FTitle, FUrl, FissueTime, FLastTime, FuserId, userip, FIsRed);
                }

                if (da.ExecSqlNum(strSql) == 1)
                    return true;
                else
                {
                    msg = "创建记录失败";
                    return false;
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
                return false;
            }
            finally
            {
                da.Dispose();
            }
        }

        public SysBulletinClass()
        { }

        public SysBulletinClass(int fid)
        {
            //读取信息。
            MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("INC"));
            try
            {
                da.OpenConn();
                string strSql = "select * from c2c_db_inc.t_bulletin_info where FID=" + fid
                    + " and Fstate=1 and Flist_state=1";
                DataTable dt = da.GetTable(strSql);

                if (dt == null || dt.Rows.Count != 1)
                {
                    throw new LogicException("读取数据有误！");
                }

                FID = fid;
                FSysID = Int32.Parse(dt.Rows[0]["FSysID"].ToString());
                FIsNew = Int32.Parse(dt.Rows[0]["FIsNew"].ToString());
                FIsRed = Int32.Parse(dt.Rows[0]["FStandBy1"].ToString());

                object obj = dt.Rows[0]["FTitle"];
                if (obj != null) FTitle = obj.ToString().Trim();
                obj = dt.Rows[0]["FUrl"];
                if (obj != null) FUrl = obj.ToString().Trim();
                obj = dt.Rows[0]["FIssueTime"];
                if (obj != null) FissueTime = obj.ToString().Trim();
                obj = dt.Rows[0]["FLastTime"];
                if (obj != null) FLastTime = obj.ToString().Trim();
                obj = dt.Rows[0]["FUserID"];
                if (obj != null) FuserId = obj.ToString().Trim();
            }
            catch (Exception err)
            {
                throw new LogicException(err.Message);
            }
            finally
            {
                da.Dispose();
            }
        }

    }
    #endregion


    #region 银行卡号查询类

    public class BankDataClass : Query_BaseForNET
    {
        public BankDataClass(string fpay_acc, string Date)
        {
            fstrSql = "select fbank_order,fpay_acc,Fbank_date,Famt from c2c_zwdb_" + Date + ".t_bankdata_list where fpay_acc='" + fpay_acc + "' order by date_format(Fbank_date,'%Y%m%d') ";
            fstrSql_count = "select count(1) from c2c_zwdb_" + Date + ".t_bankdata_list where fpay_acc='" + fpay_acc + "'";
        }
    }

    #endregion



    #region 邮储汇款查询类

    public class RemitQueryClass : Query_BaseForNET
    {
        public RemitQueryClass(string batchid, string tranType, string dataType, string remitType, string tranState, string spid, string remitRec, string listID)
        {
            string strWhere = " where 1=1 ";

            if (batchid != null && batchid != "")
            {
                strWhere += " and Fbatchid='" + batchid + "'";
            }

            if (spid != null && spid != "")
            {
                strWhere += " and Fspid='" + spid + "'";
            }

            if (tranType != "99")
            {
                strWhere += " and Ftran_type=" + tranType;
            }

            if (dataType != "99")
            {
                strWhere += " and Fdata_type=" + dataType;
            }

            if (remitType != "99")
            {
                strWhere += " and Fremit_type=" + remitType;
            }

            if (tranState != "99")
            {
                strWhere += " and Ftran_state='" + tranState + "'";
            }

            if (listID != null && listID.Trim() != "")
            {
                strWhere += " and Flistid='" + listID.Trim() + "' ";
            }

            if (remitRec != null && remitRec.Trim() != "")
            {
                strWhere += " and Fremit_rec='" + remitRec.Trim() + "' ";
            }

            fstrSql = "select * from c2c_zwdb.t_remit_order " + strWhere;
            fstrSql_count = "select count(*) from c2c_zwdb.t_remit_order " + strWhere;
        }
    }



    public class QueryRemitStateInfo : Query_BaseForNET
    {
        public QueryRemitStateInfo(string Ford_date, string Ford_ssn)
        {
            string strWhere = " where Ford_date='" + Ford_date + "' and Ford_ssn='" + Ford_ssn + "'";

            fstrSql = "select * from c2c_db_remit.t_remit_list " + strWhere;
        }
    }

    #endregion

    public class ZWDicClass
    {
        //获取账务配置置
        public static string GetZWDicValue(string dicKey, string strConn)
			{
				MySqlAccess dazw = new MySqlAccess(strConn);
				try
				{
					if(dicKey==null||dicKey=="")
						return "";
			
					string strSql="select FDicValue from c2c_zwdb.t_zwdic_info where FDicKey='"+dicKey+"'";
					dazw.OpenConn();
				
					string fvalue= dazw.GetOneResult(strSql);
					if(fvalue==null||fvalue=="")
					{
						throw new LogicException("未查询到"+dicKey+"对应的配置的值！");
					}
					else
					{
						return fvalue;
					}


				}
				catch(Exception ex)
				{
					log4net.ILog log = log4net.LogManager.GetLogger("获取账务配置置失败");
					if(log.IsErrorEnabled) log.Error(ex.Message);
					return "";
				}
				finally
				{
					dazw.Dispose();
				
				}
			}

    }

}