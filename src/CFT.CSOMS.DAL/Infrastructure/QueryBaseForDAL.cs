using System;
using System.Configuration;
using TENCENT.OSS.CFT.KF.DataAccess;
using System.Data;
using System.Collections;
using System.IO;

using CFT.CSOMS.DAL.Infrastructure;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CommLib;
using CFT.Apollo.Logging;

namespace CFT.CSOMS.DAL.Infrastructure
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
            DataSet ds = new DataSet();
                using (var da = MySQLAccessFactory.GetMySQLAccess("YWB"))
                {
                    da.OpenConn();
                    //				result = da.GetTable(strSql);
                    ds = da.dsGetTableByRange(strSql, iPageStart, iPageMax);

                    return ds;
                }
        }

        public static DataSet GetTable(string strSql, int iPageStart, int iPageMax, string dbStr)
        {
            DataSet ds = new DataSet();
            using (var da = MySQLAccessFactory.GetMySQLAccess(dbStr))
                {
                    da.OpenConn();
                    ds = da.dsGetTableByRange(strSql, iPageStart, iPageMax);
                   // da.Dispose();
                    return ds;
                }
        }

        public static DataSet GetTable_Conn(string strSql, int iPageStart, int iPageMax, string connstr)
        {
            MySqlAccess da = new MySqlAccess(connstr);
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

        //public DataSet GetResultX()
        //{
        //    return PublicRes.returnDSAll(fstrSql, "ywb");
        //}

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
        //该方法未被调用 2015-8-11 v_yqyqguo
        //public int GetCount()
        //{
        //    return Int32.Parse(PublicRes.ExecuteOne(fstrSql_count, "ywb"));
        //}

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
                try
                {
                   
                    string fuid = PublicRes.ConvertToFuid(strID);

                    string tPayList = PublicRes.GetTName("c2c_db", "t_user_order", fuid);             //交易单的表

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

                    int iCount = 10000;

                    fstrSql = "Select *," + iCount + " as total,Ftrade_state as Fstate from " + tPayList + sWhereStr1
                        //+ "UNION Select *," + iCount + " as total,Ftrade_state as Fstate from " + tPayList + sWhereStr2  
                        + " ORDER By Fcreate_time DESC limit " + (istr - 1) + "," + imax;
                }
                catch (Exception e)
                {               
                    throw;
                }
                finally
                {
         
                }
            }
            else if (iIDType == 9)  //根据QQ号码查卖家交易单
            {
               
                try
                {
 
                    string fuid = PublicRes.ConvertToFuid(strID);
                    string tPayList = PublicRes.GetTName("c2c_db", "t_user_order", fuid);             //交易单的表

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

                    int iCount = 10000;

                    fstrSql = " Select *," + iCount + " as total,Ftrade_state as Fstate from " + tPayList + sWhereStr2
                        + " ORDER By Fcreate_time DESC limit " + (istr - 1) + "," + imax;
                }
                catch (Exception e)
                {
                    throw;
                }
                finally
                {

                }
            }
            // 2012/5/29 新添加根据QQ号查询中介交易
            else if (iIDType == 10)
            {
                try
                {
                    string fuid = PublicRes.ConvertToFuid(strID);

                    string tPayList = PublicRes.GetTName("c2c_db", "t_user_order", fuid);             //交易单的表

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

                    int iCount = 10000;

                    fstrSql = " Select *," + iCount + " as total,Ftrade_state as Fstate from " + tPayList + sWhereStr2
                        + " ORDER By Fcreate_time DESC limit " + (istr - 1) + "," + imax;
                }
                catch (Exception e)
                {
                    throw;
                }
                finally
                {

                }
            }
            else if (iIDType == 13)  //yinhuang 小额刷卡交易查询
            {
                try
                {
                    //查询内部UID
                    string fuid = PublicRes.ConvertToFuid(strID);

                    string tPayList = PublicRes.GetTName("c2c_db", "t_user_order", fuid);             //交易单的表

                    string sWhereStr1 = " where fbuy_uid='" + fuid + "' and fcurtype=1 and fchannel_id=113";

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

                }
            }
            else if (iIDType == 1)
            {
                ICESQL = "listid=" + f_strID;
                fstrSql = "Select *,Ftrade_state as Fstate from " + PublicRes.GetTName("c2c_db", "t_order", strID) + " where flistid='" + f_strID + "'";
            }
            else if (iIDType == 2) //furion 20090805 2的情况应该没有.
            {
                ICESQL = "listid=" + f_strID;
                fstrSql = "Select *,Ftrade_state as Fstate from " + PublicRes.GetTName("c2c_db", "t_order", strID) + " where Fbank_backid ='" + f_strID + "'";
            }
            else if (iIDType == 4)
            {
                ICESQL = "listid=" + f_strID;
                fstrSql = "Select * from " + PublicRes.GetTName("c2c_db", "t_order", strID) + " where flistid='" + f_strID + "'";
            }
            else
            {
                throw new Exception("交易单查询传入参数错误！");
            }

        }

        public Q_PAY_LIST(string strID)
        {
            f_strID = strID;
            string tPayList = PublicRes.GetTName("c2c_db", "t_order", f_strID);
            fstrSql = "Select *,Ftrade_state as Fstate from " + tPayList + " where flistid='" + f_strID + "'";

            ICESQL = "listid=" + f_strID;
        }

        /// <summary>
        /// 提供给其它语言调用的函数，以固定的类返回值。
        /// </summary>
        /// <returns></returns>
        /* 该方法未被调用 2015-8-11 v_yqyqguo
        public T_PAY_LIST GetResult()
        {
            MySqlAccess da = new MySqlAccess(DbConnectionString.Instance.GetConnectionString("YWB"));
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
        */
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

    #region
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
                        //throw new LogicException("买家未注册");
                    }

                    tablename = PublicRes.GetTName("t_user_order", buyid);
                    strWhere += " and FBuy_uid=" + buyid;
                }

                if (saleqq.Trim() != "")
                {
                    string saleid = PublicRes.ConvertToFuid(saleqq);

                    if (saleid == null || saleid == "" || saleid == "0")
                    {
                        //throw new LogicException("卖家未注册");
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

        public string spUid;
        public int bankType;

      
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

            spUid = ar[2];  //spid
            try
            {
                bankType = Convert.ToInt32(ar[8]);  //fbuy_bank_type
            }
            catch (Exception ex)
            {
                bankType = -1;
                LogHelper.LogInfo(ex.Message + ex.StackTrace);
            }
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
                    string strSql = "transaction_id=" + listID + "&statusno=5";
                    string errMsg = "";
                    DataTable dt = CommQuery.GetTableFromICE(strSql, CommQuery.QUERY_MCH_REFUND, out errMsg);

  
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
     
            }
            else
            {
                timeStr = ar[9].ToString().Trim();   //默认时间（订单创建时间）
                timeStr2 = null;

                alTables.Add("TME=" + timeStr);
            }

        }
     #endregion
    }


}