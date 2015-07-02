using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TENCENT.OSS.C2C.Finance.DataAccess;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CFT.CSOMS.DAL.Infrastructure;
using CFT.CSOMS.DAL.CFTAccount;
using CommLib;
using System.Configuration;

using CFT.Apollo.Logging;
using CFT.CSOMS.COMMLIB;
using System.Xml;

namespace CFT.CSOMS.DAL.TradeModule
{
    public class TradeData
    {
        //注销前交易查询
        public DataSet BeforeCancelTradeQuery(string uid)
        {
            string fields = "uid:" + uid;
            return QueryUserOrder("413", fields, 0, 1);

            long uidL = long.Parse(uid);
            int uidEnd = int.Parse(uid.Substring(uid.Length - 2));
            string conString = "";
            //标记分机器的方式：uidSize 大于1.8亿一个库，小于另一个库； uidEnd 尾号 00-49 在一个库 50-99 在另外一个库
            string mark = "uidSize";
            try
            {
                mark = System.Configuration.ConfigurationManager.AppSettings["BankRollList_mark"].ToString();
            }
            catch
            { }

            //原来的用户数据库分成二个，uid<1.8亿的在DB1，uid>=1.8亿的在DB2。
            //现在，用户数据库要分成四个，uid后二位 < 25 的在 DB1， 
            //25 <= uid后二位 < 50 的在 DB2,
            //50 <= uid后二位 < 75 的在 DB3,
            //75 <= uid后二位 <= 99 的在 DB4,
            if (mark == "uidSize")
            {
                if (uidL < 180000000)
                    conString = "BankRollList1";
                else
                    conString = "BankRollList2";
            }
            if (mark == "uidEnd")
            {
                if (uidEnd >= 0 && uidEnd <= 3)
                    conString = "zw1";
                else if (uidEnd >= 4 && uidEnd <= 7)
                    conString = "zw2";
                else if (uidEnd >= 8 && uidEnd <= 11)
                    conString = "zw3";
                else if (uidEnd >= 12 && uidEnd <= 15)
                    conString = "zw4";
                else if (uidEnd >= 16 && uidEnd <= 19)
                    conString = "zw5";
                else if (uidEnd >= 20 && uidEnd <= 23)
                    conString = "zw6";
                else if (uidEnd >= 24 && uidEnd <= 27)
                    conString = "zw7";
                else if (uidEnd >= 28 && uidEnd <= 31)
                    conString = "zw8";
                else if (uidEnd >= 32 && uidEnd <= 35)
                    conString = "zw9";
                else if (uidEnd >= 36 && uidEnd <= 39)
                    conString = "zw10";
                else if (uidEnd >= 40 && uidEnd <= 43)
                    conString = "zw11";
                else if (uidEnd >= 44 && uidEnd <= 47)
                    conString = "zw12";
                else if (uidEnd >= 48 && uidEnd <= 51)
                    conString = "zw13";
                else if (uidEnd >= 52 && uidEnd <= 55)
                    conString = "zw14";
                else if (uidEnd >= 56 && uidEnd <= 59)
                    conString = "zw15";
                else if (uidEnd >= 60 && uidEnd <= 63)
                    conString = "zw16";
                else if (uidEnd >= 64 && uidEnd <= 67)
                    conString = "zw17";
                else if (uidEnd >= 68 && uidEnd <= 71)
                    conString = "zw18";
                else if (uidEnd >= 72 && uidEnd <= 75)
                    conString = "zw19";
                else if (uidEnd >= 76 && uidEnd <= 79)
                    conString = "zw20";
                else if (uidEnd >= 80 && uidEnd <= 83)
                    conString = "zw21";
                else if (uidEnd >= 84 && uidEnd <= 87)
                    conString = "zw22";
                else if (uidEnd >= 88 && uidEnd <= 91)
                    conString = "zw23";
                else if (uidEnd >= 92 && uidEnd <= 95)
                    conString = "zw24";
                else if (uidEnd >= 96 && uidEnd <= 99)
                    conString = "zw25";
            }
            using (var da = MySQLAccessFactory.GetMySQLAccess(conString))
            {
                da.OpenConn();
                string tableStr = PublicRes.GetTableNameUid("t_bankroll_list", uid);
                string Sql = "Select * from  " + tableStr + " where Fuid='"+uid+"' Order by Fmodify_time DESC limit 1";
                DataSet ds = da.dsGetTotalData(Sql);
                return ds;
            }
        }

        public int RemoveControledFinLogInsert(string qqid, string FbalanceStr, string FtypeText, string cur_type, DateTime ApplyTime, string ApplyUser)
        {
            using (var da = CommLib.DbConnectionString.Instance.GetConnection("DataSource_ht"))
            {
                da.OpenConn();

                string Sql = "insert into  c2c_fmdb.t_log_ConFinRemove (Fuin ,FbalanceStr,FtypeText,FcurType ,FmodifyTime ,FupdateUser) " +
                        "values('" + qqid + "','" + FbalanceStr + "','" + FtypeText + "','" + cur_type + "','" + ApplyTime.ToString() + "','" + ApplyUser + "')";
               return da.ExecSqlNum(Sql);
            }
        }
        /// <summary>
        /// 微信买家纬度用户订单查询
        /// </summary>
        /// <returns></returns>
        public DataSet QueryWxBuyOrderByUid(int uid, DateTime startTime, DateTime endTime)
        {
            //ver=1&head_u=&sp_id=2000000501&request_type=100878&uid=123456&s_time=2015-01-01&e_time=2015-03-01&offset=0&limit=10&icard_flag=0
            string reqString = "uid=" + uid.ToString();
            reqString += "&s_time=" + startTime.ToString("yyyy-MM-dd 00:00:00");
            reqString += "&e_time=" + endTime.ToString("yyyy-MM-dd 23:59:59");
            reqString += "&offset=0";
            reqString += "&limit=10";
            reqString += "&icard_flag=0";
            reqString += "&MSG_NO=100878" + DateTime.Now.Ticks.ToString();

            var serverIp = System.Configuration.ConfigurationManager.AppSettings["WX_Order_RelayIP"].ToString();
            var serverPort = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["WX_Order_RelayPort"].ToString());
            DataSet ds= RelayAccessFactory.GetDSFromRelayRowNumStartWithZero(reqString, "100878", serverIp, serverPort);

            if (ds != null && ds.Tables.Count > 0) 
            {
                if (!ds.Tables[0].Columns.Contains("Fbank_listid")) 
                {
                    ds.Tables[0].Columns.Add("Fbank_listid", typeof(System.String));
                }
                if (!ds.Tables[0].Columns.Contains("Fbank_backid"))
                {
                    ds.Tables[0].Columns.Add("Fbank_backid", typeof(System.String));
                }
                if (!ds.Tables[0].Columns.Contains("Fstate"))
                {
                    ds.Tables[0].Columns.Add("Fstate", typeof(System.String));
                }
                if (!ds.Tables[0].Columns.Contains("Fcreate_time_c2c"))
                {
                    ds.Tables[0].Columns.Add("Fcreate_time_c2c", typeof(System.String));
                }
                if (!ds.Tables[0].Columns.Contains("Fip"))
                {
                    ds.Tables[0].Columns.Add("Fip", typeof(System.String));
                }
                if (!ds.Tables[0].Columns.Contains("Fbargain_time"))
                {
                    ds.Tables[0].Columns.Add("Fbargain_time", typeof(System.String));
                }
                if (!ds.Tables[0].Columns.Contains("Freceive_time_c2c"))
                {
                    ds.Tables[0].Columns.Add("Freceive_time_c2c", typeof(System.String));
                }
                if (!ds.Tables[0].Columns.Contains("Freceive_time"))
                {
                    ds.Tables[0].Columns.Add("Freceive_time", typeof(System.String));
                }
                if (!ds.Tables[0].Columns.Contains("total"))
                {
                    ds.Tables[0].Columns.Add("total", typeof(System.String));
                    foreach (DataRow item in ds.Tables[0].Rows) 
                    {
                        item["total"] = "10000";
                    }
                }
            }
            return ds;
        }

        public DataSet RemoveControledFinLogQuery(string qqid)
        {
            using (var da = CommLib.DbConnectionString.Instance.GetConnection("DataSource_ht"))
            {
                da.OpenConn();

                string Sql = "Select * from  c2c_fmdb.t_log_ConFinRemove where Fuin='" + qqid + "' Order by FmodifyTime DESC";
                DataSet ds = da.dsGetTotalData(Sql);
                return ds;
            }
        }

        #region 交易记录查询

        public DataSet GetTradeList(string u_ID, int u_IDType, DateTime u_BeginTime, DateTime u_EndTime, int istr, int imax)
        {

            string ServerIP = ConfigurationManager.AppSettings["ICEServerIP"].Trim();
            int Port = Int32.Parse( ConfigurationManager.AppSettings["ICEPort"].Trim());
            ICEAccess ice = new ICEAccess(ServerIP,Port);
            try
            {
                //已修改 furion V30_FURION核心查询需改动 type=1和2时,由t_tran_list改为查询t_order
                //0买家交易单，9卖家交易单，1，通过交易单查询，2通过给银行订单号查询，4，通过订单查询
                Q_PAY_LIST cuser = new Q_PAY_LIST(u_ID, u_IDType, u_BeginTime, u_EndTime, istr, imax);

                if (cuser.ICESQL == "")
                {
                    if (u_IDType == 0 || u_IDType == 9 || u_IDType == 10 || u_IDType == 13)
                    {
                        string fuid = PublicRes.ConvertToFuid(u_ID);
                        string connstr = PublicRes.GetConnString("t_user_order_bsb", fuid.Substring(fuid.Length - 2));
                        return cuser.GetResultX_Conn(connstr);
                    }
                    else
                        return cuser.GetResultX("BSB");
                    //现在要查核心交易单，而不是用订单替代。
                }
                else if (u_IDType == 1)
                {
                    ice.OpenConn();
                    string strwhere = "where=" + ICEAccess.URLEncode("flistid=" + u_ID.Trim() + "&");
                    strwhere += ICEAccess.URLEncode("fcurtype=1&");

                    string strResp = "";

                    //3.0接口测试需要 furion 20090708
                    DataTable dt = ice.InvokeQuery_GetDataTable(YWSourceType.交易单资源, YWCommandCode.查询交易单信息, u_ID.Trim(), strwhere, out strResp);

                    ice.CloseConn();

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return null;
                        //throw new LogicException("调用ICE查询T_tran_list无记录" + strResp);
                    }

                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);
                    return ds;
                }
                else
                {
                    string errMsg = "";
                    DataSet ds = CommQuery.GetDataSetFromICE(cuser.ICESQL, CommQuery.QUERY_ORDER, out errMsg);

                    return ds;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                ice.Dispose();
            }

        }

        //将银行返回定单号转换成交易单号
        public string ConvertToListID(string sFbankAcc, DateTime sDateTime)
        {
            try
            {
                string errMsg = "";

                string strCmd = "bank_acc=" + sFbankAcc + "&query_day=" + sDateTime.ToString("yyyyMMdd");  //增加时间参数 andrew 20110322;
                string listID = CommQuery.GetOneResultFromICE(strCmd, CommQuery.QUERY_TCBANKROLL_DAY, "Flistid", out errMsg);
                return listID;
            }
            catch
            {
                throw new Exception("根据银行订单号和日期查询交易单失败！");
            }
        }

        public DataSet GetQueryListDetail(string listid)
        {
            try
            {
                OrderQueryClassZJ cuser = new OrderQueryClassZJ(listid);
                string errMsg = "";
                DataSet ds = CommQuery.GetDataSetFromICE(cuser.ICESQL, CommQuery.QUERY_ORDER, out errMsg);
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
                return null;
            }
        }

        /// <summary>
        /// 查询用户转账单记录
        /// </summary>
        /// <param name="listid">单号</param>
        /// <param name="state">付款单状态列表，以逗号分隔（当为空时，则不过滤状态）具体状态含义如下:1下单,2支付成功,
        /// 3 付款成功,4 退款申请中,5 已退款,12 充值成功:注：针对用户注销来说，传入1,2,12,4</param>
        /// <param name="qry_type">查询类型:1 支付单号,2 B2C转账单号（注意：重构前传入为核心转账单号，
        /// 重构后传入为转账商户订单号）,3 按uin+state查询（查询最近一单符合要求的付款单，最多查三个月内的）</param>
        /// <param name="uin"></param>
        /// <returns></returns>
        public DataSet QueryPaymentParty(string listid, string state, string qry_type, string uin)
        {
            //qpayment_party_query_service
            string reqString = "uin=" + uin;
            reqString += "&listid=" + listid;
            reqString += "&state=" + state;
            reqString += "&qry_type=" + qry_type;

            var serverIp = System.Configuration.ConfigurationManager.AppSettings["HandQ_Payment_RelayIP"].ToString();
            var serverPort = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["HandQ_Payment_RelayPort"].ToString());

            string answer = RelayAccessFactory.RelayInvoke(reqString, "100910", false, false, serverIp, serverPort, "");
            answer = System.Web.HttpUtility.UrlDecode(answer, System.Text.Encoding.GetEncoding("GB2312"));
            string Msg;

            DataSet ds = CommQuery.ParseRelayStr(answer, out Msg, true);
            if (Msg != "")
            {
                throw new Exception(Msg);
            }
            return ds;

            //  return RelayAccessFactory.GetDSFromRelay(reqString, "100910", serverIp, serverPort);
        }
        #endregion

        //按充值单号查询
        public DataSet GetBankRollListByListId(string u_ID, string u_QueryType, int fcurtype, DateTime u_BeginTime, DateTime u_EndTime, int fstate,
           float fnum, float fnumMax, string banktype, string sorttype, int iPageStart, int iPageMax)
        {
            string msg = "";
            try
            {
                DataSet ds = null;
                DataSet dsqq = new DataSet();
                DataTable dsdt = new DataTable();
                string srtSql = "";
                if (u_ID != null && u_ID.Trim() != "" && u_QueryType.ToLower() == "qq")  //按照QQ号查询，注意使用内部uid
                {
                    string uid = PublicRes.ConvertToFuid(u_ID);
                    srtSql = "auid=" + uid;
                }
                else if (u_ID != null && u_ID.Trim() != "" && u_QueryType.ToLower() == "tobank")  //给银行的订单号
                {
                    srtSql = "bank_list=" + u_ID.Trim() + "";
                }
                else if (u_ID != null && u_ID.Trim() != "" && u_QueryType.ToLower() == "bankback") //银行返回u
                {
                    srtSql = "bank_acc=" + u_ID.Trim();
                }
                else if (u_ID != null && u_ID.Trim() != "" && u_QueryType.ToLower() == "czd") //充值单查询
                {
                    srtSql = "listid=" + u_ID.Trim();
                }
                if (fstate != 0)
                {
                    srtSql += "&sign=" + fstate;
                }
                long num = (long)Math.Round(fnum * 100, 0);
                long numMax = (long)Math.Round(fnumMax * 100, 0);

                srtSql += "&num_start=" + num + "&num_end=" + numMax;

                if (banktype != "0000" && banktype != "")
                {
                    srtSql += "&bank_type=" + banktype;
                }

                if (u_QueryType.ToLower() != "tobank")
                {
                    TimeSpan ts = u_EndTime - u_BeginTime;
                    int sub = ts.Days;
                    bool iscone = false;
                    for (int i = 0; i <= sub; i++)
                    {
                        string truetime = u_EndTime.AddDays(-i).ToString("yyyyMMdd");
                        string querstr = srtSql + "&query_day=" + truetime + "&curtype=" + fcurtype + "&strlimit=limit " + iPageStart + "," + iPageMax;
                        ds = CommQuery.GetDataSetFromICE(querstr, CommQuery.QUERY_TCBANKROLL_DAY, out msg);
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            if (u_QueryType.ToLower() == "qq" || u_QueryType.ToLower() == "czd")
                            {
                                if (!iscone)
                                {
                                    dsdt = ds.Tables[0].Clone();
                                    iscone = true;
                                }
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    dsdt.ImportRow(dr);
                                }
                            }
                            else
                            {
                                break;
                            }

                        }
                    }
                }
                else
                {
                    srtSql += "&offset=0&limit=10";
                    ds = CommQuery.GetDataSetFromICE_QueryServer(srtSql, CommQuery.QUERY_TCBANKROLL_S, out msg);
                }

                if (u_QueryType.ToLower() == "qq" || u_QueryType.ToLower() == "czd")
                {
                    dsqq.Tables.Add(dsdt);
                    return dsqq;
                }

                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message + msg);
            }
        }

        // 查询用户帐户流水表
        public DataSet GetBankRollList(string u_QQID, int fcurtype, DateTime u_BeginTime, DateTime u_EndTime, int istr, int imax)
        {
            try
            {
                //idtype 0买家，1卖家
                string fuid = PublicRes.ConvertToFuid(u_QQID);
                DataSet ds;

                int start = istr - 1;
                if (start < 0) start = 0;

                string strWhere = "start_time=" + ICEAccess.ICEEncode(u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss"));
                strWhere += "&end_time=" + ICEAccess.ICEEncode(u_EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                strWhere += "&uid=" + fuid;
                strWhere += "&curtype=" + fcurtype;
                strWhere += "&lim_start=" + start;
                strWhere += "&lim_count=" + imax;

                string errMsg;

                if (!CommQuery.GetDataFromICE(strWhere, CommQuery.个人资金流水, out errMsg, out ds))
                {
                    return null;
                }

                if (ds == null)
                    return null;

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //查询腾讯付款记录表
        public DataSet GetTCBankPAYList(string u_ID, int u_IDType, DateTime u_BeginTime, DateTime u_EndTime, int istr, int imax)
        {
            try
            {
                Q_TCBANKPAY_LIST cuser = new Q_TCBANKPAY_LIST(u_ID, u_IDType, u_BeginTime, u_EndTime);
                return cuser.GetResultX(istr, imax);
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
            }
        }

        //查询退款单表
        public DataSet GetRefund(string u_ID, int u_IDType, DateTime u_BeginTime, DateTime u_EndTime, int istr, int imax)
        {
            try
            {
                Q_REFUND cuser = new Q_REFUND(u_ID, u_IDType, u_BeginTime, u_EndTime);
                return cuser.GetResultX(istr, imax);
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！");
            }
        }

        //子帐户资金流水查询函数
        public bool GetChildrenBankRollList(string u_QQID, DateTime u_BeginTime, DateTime u_EndTime, string Fcurtype, int istr, int imax, int Ftype, string Fmemo,out string reply)
        {
            try
            {
                reply = "";
                string fuid = PublicRes.ConvertToFuid(u_QQID);
                if (fuid == null || fuid == "" || fuid == "0")
                    return false;

                //reqid=124 是uid(180000000 - 1999999999)
                //reqid=117 是uid < 180000000的

                string reqid = "124";
                if (Int64.Parse(fuid) < 180000000)
                {
                    reqid = "117";
                }

                string inmsg = "fields=begin_time:" + PublicRes.ICEEncode(u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss"));
                inmsg += "|end_time:" + PublicRes.ICEEncode(u_EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                inmsg += "|uid:" + fuid;
                inmsg += "|cur_type:" + Fcurtype;
                if (Ftype != 0)
                {
                    inmsg += "|type:" + Ftype;
                }
                if (!string.IsNullOrEmpty(Fmemo))
                {
                    inmsg += "|memo:" + Fmemo;
                }
                inmsg += "&flag=2";
                int start = istr - 1;
                inmsg += "&offset=" + start.ToString();  //从0开始的
                inmsg += "&limit=" + imax;
                inmsg += "&reqid=" + reqid;


                short sresult;
                string msg;

                if (TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.middleInvoke("common_simquery_service", inmsg, false, out reply, out sresult, out msg))
                {
                    if (sresult != 0)
                    {
                        throw new Exception("common_simquery_service接口失败：result=" + sresult + "，msg=" + "&reply=" + reply);
                    }
                    else if (reply.StartsWith("result=0&res_info=ok"))
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                    throw new Exception("common_simquery_service接口失败：result=" + sresult + "，msg=" + msg + "&reply=" + reply);
                }

            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }


        //查询用户帐户流水表_WithListID
        public DataSet GetBankRollList_withID(DateTime u_BeginTime, DateTime u_EndTime, string ListID, int istr, int imax)
        {
         
            try
            {
                //furion V30_FURION核心查询需改动 等待通用查询接口.  //V30_20090525 资金流水用listid查询 这个要增加多个查询，然后组合结果
                Q_BANKROLL_LIST cuser = new Q_BANKROLL_LIST(u_BeginTime, u_EndTime, ListID);

                if (cuser.alTables == null || cuser.alTables.Count == 0)
                    return null;

                string onerow = cuser.alTables[0].ToString().Replace("UID=", "");
                string strWhere = "uid=" + onerow;
                strWhere += "&listid=" + ListID;

                DataSet ds;
                string errMsg;

                if (!CommQuery.GetDataFromICE(strWhere, CommQuery.交易单资金流水_个人, out errMsg, out ds))
                {
                    //throw new LogicException(errMsg);
                }

                bool havefirstds = true;
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0] == null || ds.Tables[0].Columns.Count == 0)
                {
                    havefirstds = false;
                }

                //先取出买卖家的一个数据表
                GetBuyerRollList(ListID, cuser, ref onerow, ref strWhere, ref ds, ref errMsg, ref havefirstds);

                //furion 增加银行资金流水表。 20090813
                GetBankRollList(ListID, cuser, ref onerow, ref strWhere, ref ds, ref errMsg, ref havefirstds);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0] == null || ds.Tables[0].Columns.Count == 0)
                {
                    return null;
                }

                ds.Tables[0].DefaultView.Sort = "Faction_Type DESC";
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("service发生错误,请联系管理员！" + e.Message);
                return null;
            }
            finally
            {
                
            }
        }

        /// <summary>
        /// 银行资金流水表
        /// </summary>
        public static void GetBankRollList(string ListID, Q_BANKROLL_LIST cuser, ref string onerow, ref string strWhere, ref DataSet ds, ref string errMsg, ref bool havefirstds)
        {
            for (int i = 1; i < cuser.alTables.Count; i++)
            {
                DataSet newds;
                onerow = cuser.alTables[i].ToString();
                if (onerow.StartsWith("UID="))
                {
                    continue;
                }
                else
                {
                    strWhere = "start_time=" + onerow.Replace("TME=", "").Substring(0, 10);
                    strWhere += "&listid=" + ListID;

                    if (!CommQuery.GetDataFromICE(strWhere, CommQuery.交易单资金流水_银行, out errMsg, out newds))
                    {
                        //throw new LogicException(errMsg);
                    }
                }

                //把这次得到的表内容合并入ds中。
                if (newds == null || newds.Tables.Count == 0 || newds.Tables[0] == null || newds.Tables[0].Rows.Count == 0)
                {
                    continue;
                }

                if (havefirstds)
                {
                    foreach (DataRow dr in newds.Tables[0].Rows)
                    {
                        ds.Tables[0].Rows.Add(dr.ItemArray);
                    }
                }
                else
                {
                    havefirstds = true;
                    ds = newds;
                }
            }
        }

        /// <summary>
        /// 买卖家的资金流水表
        /// </summary>
        public static void GetBuyerRollList(string ListID, Q_BANKROLL_LIST cuser, ref string onerow, ref string strWhere, ref DataSet ds, ref string errMsg, ref bool havefirstds)
        {
            for (int i = 1; i < cuser.alTables.Count; i++)
            {
                DataSet newds;
                onerow = cuser.alTables[i].ToString();
                if (onerow.StartsWith("UID="))
                {
                    strWhere = "uid=" + onerow.Replace("UID=", "");
                    strWhere += "&listid=" + ListID;

                    if (!CommQuery.GetDataFromICE(strWhere, CommQuery.交易单资金流水_个人, out errMsg, out newds))
                    {
                        //throw new LogicException(errMsg);
                    }
                }
                else
                {
                    strWhere = "start_time=" + onerow.Replace("TME=", "").Substring(0, 10);
                    strWhere += "&listid=" + ListID;

                    if (!CommQuery.GetDataFromICE(strWhere, CommQuery.交易单资金流水_商户, out errMsg, out newds))
                    {
                        //throw new LogicException(errMsg);
                    }
                }

                //把这次得到的表内容合并入ds中。
                if (newds == null || newds.Tables.Count == 0 || newds.Tables[0] == null || newds.Tables[0].Rows.Count == 0)
                {
                    continue;
                }

                if (havefirstds)
                {
                    foreach (DataRow dr in newds.Tables[0].Rows)
                    {
                        ds.Tables[0].Rows.Add(dr.ItemArray);
                    }
                }
                else
                {
                    havefirstds = true;
                    ds = newds;
                }
            }
        }

        /// <summary>
        /// 查询微信支付转账和微信支付红包支付未完成交易数据
        /// </summary>
        /// <param name="pay_openid"></param>
        /// <returns></returns>
        public int QueryWXUnfinishedTrade(string WeChatName)
        {
            string wxUIN = WeChatHelper.GetUINFromWeChatName(WeChatName);
            string wxHBUIN = WeChatHelper.GetHBUINFromWeChatName(WeChatName);

            int num = 0;
            string relayIP = Apollo.Common.Configuration.AppSettings.Get<string>("Relay_IP", "172.27.31.177");
            int relayPort = Apollo.Common.Configuration.AppSettings.Get<int>("Relay_PORT", 22000);
            string RequestType = "100222";//转账的
            string open_id = wxUIN.Trim();
            string RequestText = "pay_openid="+open_id;
            try
            {
                string answer = RelayAccessFactory.RelayInvoke(RequestText, RequestType, true, false, relayIP, relayPort, "");
                answer = System.Web.HttpUtility.UrlDecode(answer, System.Text.Encoding.GetEncoding("GB2312"));
                string Msg;
                DataSet ds = CommQuery.ParseRelayStr(answer, out Msg, true);
                if (Msg != "")
                {
                    LogHelper.LogError(Msg);
                    throw new Exception(Msg);
                }
                if (ds.Tables[0].Columns.Contains("num"))
                {
                    num = int.Parse(ds.Tables[0].Rows[0]["num"].ToString());
                    return num;
                }
                else
                {
                    LogHelper.LogError("查询微信转账条目返回结果不正确!");
                    throw new Exception("查询微信转账条目返回结果不正确!");
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.LogError("查询微信转账条目异常:" + ex.Message);
                throw new Exception("查询微信转账条目异常:" + ex.Message);
            }
        }
        public int QueryWXUnfinishedHB(string WeChatName)
        {
            string wxHBOpenID = WeChatHelper.GetHBOpenIdFromWeChatName(WeChatName);
            int num = 0;
            string relayIP = Apollo.Common.Configuration.AppSettings.Get<string>("RelayWXHB_IP", "10.198.17.219");
            int relayPort = Apollo.Common.Configuration.AppSettings.Get<int>("RelayWXHB_Port", 22001);
            string RequestText = "uin=" + wxHBOpenID;
            string Msg = "";
            try
            {
                string stranswer = RelayAccessFactory.RelayInvoke(RequestText, "100038", false, false, relayIP, relayPort, "");
                stranswer= System.Web.HttpUtility.UrlDecode(stranswer, System.Text.Encoding.GetEncoding("GB2312"));
                DataSet ds= CommQuery.ParseRelayStr(stranswer, out Msg, true);
                if (Msg != "")
                {
                    LogHelper.LogError(Msg);
                    throw new Exception(Msg);
                }
                if (ds.Tables[0].Columns.Contains("num"))
                {
                    num = int.Parse(ds.Tables[0].Rows[0]["num"].ToString());
                }
                else
                {
                    LogHelper.LogError("微信红包未完成交易查询返回结果有误!");
                    throw new Exception("微信红包未完成交易查询返回结果有误!");
                }
                return num;
            }
            catch (System.Exception ex)
            {
                LogHelper.LogError("微信红包在途条目查询失败:" + ex.Message);
                throw new Exception("微信红包在途条目查询失败:"+ex.Message);
            }
            
        }

        public DataSet GetUnfinishedMobileQHB(string uin)
        {
            string RequestText = "uin=" + uin;
            RequestText += "&snd_state=1,3&offset=0&limit=1&type=1";

            var relayIP = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("HandQHBIP", "10.238.13.244");
            var relayPORT = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("HandQHBPort", 22000);
            string answer = RelayAccessFactory.RelayInvoke(RequestText, "100578", false, false, relayIP, relayPORT, "");
            answer = System.Web.HttpUtility.UrlDecode(answer, System.Text.Encoding.GetEncoding("GB2312"));
            string Msg = "";
            DataSet ds = CommQuery.ParseRelayStr(answer, out Msg, true);
            return ds;
        }
        /// <summary>
        /// 未完成交易单查询
        /// </summary>
        public DataSet GetListidFromUserOrder(string qqid, string uid, int start, int max)
        {
            //ver=1&head_u=&sp_id=2000000501&request_type=4613&reqid=2213&flag=2&fields=buy_uid:298686752&offset=0&limit=1&msgno=10002
            string fuid = "";
            if (!string.IsNullOrEmpty(qqid.Trim()))
                fuid = PublicRes.ConvertToFuid(qqid);
            if (!string.IsNullOrEmpty(uid.Trim()))
                fuid = uid;
            if (fuid == null || fuid.Length < 3)
            {
                throw new Exception(fuid + "账号不存在");
            }
            //查询买家
            DataSet dsbuy = QueryUserOrder("2213", "buy_uid:" + fuid, start, max);
            //查询卖家
            DataSet dssale = QueryUserOrder("2214", "sale_uid:" + fuid, start, max);
            dsbuy = PublicRes.ToOneDataset(dsbuy, dssale);
            return dsbuy;
        }
        /// <summary>
        /// 中介订单查询
        /// </summary>
        public DataSet GetQueryList(DateTime u_BeginTime, DateTime u_EndTime, string buyqq, string saleqq, string buyqqInnerID, string saleqqInnerID,
            string u_QueryType, string queryvalue, int fstate, int fcurtype, int start, int max)
        {
            //ver=1&head_u=&sp_id=2000000501&request_type=4613&reqid=2211&flag=2&fields=sp_uid:298731022&offset=0&limit=1&msgno=10002
            //ver=1&head_u=&sp_id=2000000501&request_type=4613&reqid=2212&flag=2&fields=buy_uid:298686752&offset=0&limit=1&msgno=10002
            try
            {
                if (u_QueryType != "FlistID" || queryvalue.Trim() == "")
                {
                    string fuid = "";

                    if (u_QueryType == "FSpid") u_QueryType = "spid";
                    if (u_QueryType == "FBank_listID") u_QueryType = "bank_listid";
                    if (u_QueryType == "FCoding") u_QueryType = "coding";

                    string fields = "|stime:" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss") +
                           "|etime:" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss") +
                           ((fstate != 99) ? "|trade_state:" + fstate.ToString() : "") +
                           ((queryvalue.Trim() != "" && u_QueryType != "") ? "|" + u_QueryType.Trim() + ":" + queryvalue.Trim() : "");

                    DataSet ds = new DataSet();
                   
                    if (!string.IsNullOrEmpty(buyqqInnerID))
                    {
                        fields = "buy_uid:" + buyqqInnerID + fields;
                        ds = QueryUserOrder("2212", fields, start, max);
                    }
                    else if (!string.IsNullOrEmpty(saleqqInnerID))
                    {
                        fields = "sp_uid:" + saleqqInnerID + fields;
                        ds = QueryUserOrder("2211", fields, start, max);
                    }
                    else if (!string.IsNullOrEmpty(buyqq))
                    {
                        fuid = PublicRes.ConvertToFuid(buyqq);
                        buyqqInnerID = fuid;
                        fields = "buy_uid:" + fuid + fields;
                        ds = QueryUserOrder("2212", fields, start, max);
                    }
                    else if (!string.IsNullOrEmpty(saleqq))
                    {
                        fuid = PublicRes.ConvertToFuid(saleqq);
                        fields = "sp_uid:" + fuid + fields;
                        ds = QueryUserOrder("2211", fields, start, max);
                    }

                    DataSet dsForWX = new DataSet();
                    if (!string.IsNullOrEmpty(buyqqInnerID.Trim()))
                    {
                        try
                        {
                            dsForWX = QueryWxBuyOrderByUid(int.Parse(buyqqInnerID.Trim()), u_BeginTime, u_EndTime);//微信买家纬度订单
                            //添加将微信订单数据
                            ds = PublicRes.ToOneDataset(ds, dsForWX);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("添加将微信订单数据失败：" + ex.Message);
                        }
                    }
                    return ds;
                }
                else
                {
                    string errMsg = "";
                    DataSet ds = CommQuery.GetDataSetFromICE("listid=" + queryvalue, CommQuery.QUERY_ORDER, out errMsg);
                    if (errMsg != "")
                    {
                        throw new Exception(errMsg);
                    }
                    return ds;
                }

            }
            catch (Exception err)
            {
                throw new Exception("查询出错！" + err.Message);
            }
        }
        /// <summary>
        /// 满减使用查询
        /// </summary>
        public DataSet GetManJianUsingList(string u_ID, int u_IDType, DateTime u_BeginTime, DateTime u_EndTime, string banktype, int istr, int imax)
        {
            string fuid = PublicRes.ConvertToFuid(u_ID);

            string fields = "buy_uid:" + fuid;
            //#if DEBUG
            //            fields = "buy_uid:298686752"  ;
            //#endif
            fields += "|stime:" + u_BeginTime.ToString("yyyy-MM-dd HH:mm:ss");
            fields += "|etime:" + u_EndTime.ToString("yyyy-MM-dd HH:mm:ss");
            if (!string.IsNullOrEmpty(banktype))
            {
                fields += "|bank_type:" + banktype;
            }

            DataSet ds = QueryUserOrder("2215", fields, istr, imax);
            return ds;
        }
        /// <summary>
        /// iIDType=0,9,10,13
        /// </summary>
        public DataSet Q_PAY_LIST(string strID, int iIDType, DateTime dtBegin, DateTime dtEnd, int istr, int imax)
        {
            DataSet ds = new DataSet();
            string fuid = PublicRes.ConvertToFuid(strID);
            //#if DEBUG
            //           fuid = strID;
            //#endif
            if (iIDType == 0)  //根据QQ号码查买家交易单
            {
                string fields = "buy_uid:" + fuid +
                         "|stime:" + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") +
                        "|etime:" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss");
                ds = QueryUserOrder("2216", fields, istr - 1, imax);
                try
                {
                    DataSet dsForWX = QueryWxBuyOrderByUid(int.Parse(fuid.Trim()), dtBegin, dtEnd);//微信买家纬度订单
                    //添加将微信订单数据
                    ds = PublicRes.ToOneDataset(ds, dsForWX);
                }
                catch (Exception ex)
                {
                    throw new Exception("添加将微信订单数据失败：" + ex.Message);
                }
            }
            else if (iIDType == 9)  //根据QQ号码查卖家交易单
            {
                string fields = "sale_uid:" + fuid +
                         "|stime:" + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") +
                        "|etime:" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss");
                ds = QueryUserOrder("2217", fields, (istr - 1), imax);
            }
            else if (iIDType == 10) // 2012/5/29 新添加根据QQ号查询中介交易
            {
                string fields = "sale_uid:" + fuid +
                         "|stime:" + dtBegin.ToString("yyyy-MM-dd HH:mm:ss") +
                        "|etime:" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss");
                ds = QueryUserOrder("2218", fields, (istr - 1), imax);
            }
            else if (iIDType == 13)  //yinhuang 小额刷卡交易查询
            {
                string fields = "buy_uid:" + fuid;
                ds = QueryUserOrder("2219", fields, istr, imax);
            }
            else
            {
                throw new Exception("交易单查询传入参数错误!iIDType=" + iIDType);
            }
            return ds;
        }
        /// <summary>
        /// 商户管理-商户交易清单查询/商户订单查询财付通订单
        /// </summary>
        public DataSet MediListQueryClass(string u_ID, string Fcode, string strBeginTime, string strEndTime, string u_UserFilter
            , string u_OrderBy, int limStart, int limCount)
        {
            if (u_ID != null && u_ID.Trim() != "")
            {
                string strSql = "spid=" + u_ID;
                string errMsg = "";
                string fuid = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MERCHANTINFO, "FuidMiddle", out errMsg);

                if (fuid == null || fuid.Trim() == "")
                {
                    throw new Exception("查找不到指定的商户！" + errMsg);
                }

                string fields = "sale_uid:" + fuid;
                //#if DEBUG
                // fields = "sale_uid:298686752";//测试
                // string tablename = PublicRes.GetTName("t_user_order", "298686752"); //c2c_db_52.t_user_order_7
                //#endif
                if (!string.IsNullOrEmpty(Fcode))
                {
                    fields += "|coding:" + Fcode;
                }
                if (!string.IsNullOrEmpty(u_UserFilter))
                {
                    fields += "|trade_state:" + u_UserFilter;
                }
                if (!string.IsNullOrEmpty(strBeginTime))
                {
                    fields += "|stime:" + strBeginTime;
                }
                if (!string.IsNullOrEmpty(strEndTime))
                {
                    fields += "|etime:" + strEndTime;
                }
                return QueryUserOrder("2220", fields, limStart, limCount);
            }
            else
            {
                throw new Exception("商户帐号不能为空");
            }
        }

        private DataSet QueryUserOrder(string reqid, string fields,  int offset, int limit)
        {
            string reqString = "";
            string msgno = System.DateTime.Now.ToString("yyyyMMddHHmmss") + PublicRes.NewStaticNoManage();
            var serverIp = System.Configuration.ConfigurationManager.AppSettings["UserOrderIP"].ToString();
            var serverPort = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["UserOrderPort"].ToString());
            string request_type = System.Configuration.ConfigurationManager.AppSettings["UserOrder_requesttype"].ToString();
            //查询卖家
            reqString = "reqid=" + reqid + "&flag=2" +
                  "&fields=" + fields +
                  "&offset=" + offset + "&limit=" + limit + "&msgno=" + msgno;
            return RelayAccessFactory.GetDSFromRelayFromXML(reqString, request_type, serverIp, serverPort);
        }
        public DataSet QueryBusCardPrepaid(string beginDate, string endDate, int pageSize, string uin, string listid, string cardid, out string errMsg)
        {
            DataSet ds = null;
            string relayIP = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("RelayBusCard_IP", "172.27.31.177");
            int relayPort = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("RelayBusCard_Port", 22000);
            string requestText = "yyyymmdd_from=" + beginDate + "&yyyymmdd_to=" + endDate + "&page_size=" + pageSize + "&uin=" + uin + "&tenpay_bill=" + listid + "&card_mark_number=" + cardid;
            try
            {
                string answer = RelayAccessFactory.RelayInvoke(requestText, "101012", true, false, relayIP, relayPort, "");
                ds = CommQuery.ParseRelayBusCardPrepaid(answer, out errMsg);
                return ds;
            }
            catch (System.Exception ex)
            {
                errMsg = ex.Message;
                return null;
            }
        }
    }
}
