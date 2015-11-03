using CFT.CSOMS.DAL.CFTAccount;
using CFT.CSOMS.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using TENCENT.OSS.CFT.KF.DataAccess;

namespace CFT.CSOMS.DAL.TradeModule
{
    public class SettleData
    {
        public string getAmount(string qqid, string UinOrUid)
        {
            // "select * from app_platform.t_account_freeze where Fuin = '" + u_QQID + "'";
            if (UinOrUid == "uin")
            {
                qqid = AccountData.ConvertToFuid(qqid);
            }

            string Amount = "";
            string ip = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("SettleRateIP", "10.123.6.29");
            int port = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("SettleRatePort", 443);

            string requestText = "reqid=170&flag=1&fields=uid:" + qqid;
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "1407", ip, port);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Amount = ds.Tables[0].Rows[0]["Famount"].ToString();
            }
            return Amount;
        }

        //分账规则查询函数
        public DataTable QuerySettleRuleList(string Fspid, int offset, int limit)
        {
            //string sql = "select * from app_platform.t_settle_rule where Fspid='" + spid + "'";

            string ip = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("SettleRateIP", "10.123.6.29");
            int port = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("SettleRatePort", 443);
            DataTable dt = null;
  
            string requestText = "reqid=174&flag=2&offset={0}&limit={1}&fields=spid:{2}|time:0";
            requestText = string.Format(requestText, offset, limit, Fspid);
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "1407", ip, port);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }
        //商户权限查询函数
        public DataTable QuerySpControl(string spid)
        {
            //MySqlAccess da = new MySqlAccess("Driver={MySQL ODBC 5.2a Driver};Database=mysql;Server=10.6.206.73;UID=zft_dev_all;PWD=zft_dev_all;charset=latin1;option=3;");
            //da.OpenConn();
            //string sql = "select * from app_platform.t_sp_control where Fspid='" + spid + "'";
            //DataTable dtd = da.dsGetTotalData(sql).Tables[0];
            //MySqlAccess da = new MySqlAccess("Driver={MySQL ODBC 5.2a Driver};Database=mysql;Server=10.6.206.73;UID=zft_dev_all;PWD=zft_dev_all;charset=latin1;option=3;");
            //da.OpenConn();
            //string sql = "SELECT Fspid,Frule,Fcontroll_method,Fcontroll_close_type, Fcontroll_args,Fcreate_time,Fmodify_time,Foperator,Fexplain FROM app_platform.t_sp_control WHERE Fspid='20000000501'";
            //return  da.dsGetTotalData(sql).Tables[0];

            DataTable dt = null;
            string ip = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("SettleRateIP", "10.123.6.29");
            int port = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("SettleRatePort", 443);

            string requestText = "reqid=175&flag=1&fields=spid:" + spid;
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "1407", ip, port);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }


       //补差关系查询函数
        public DataTable QueryRelationOrderList(string szListid, string subListid)
        {
            //string sql = "select * from app_platform.t_relation_order where 1=1 ";
            //sql += " AND Ftransaction_id='" + szListid + "'";
            //sql += " AND Fsub_listid='" + subListid + "'";
            //DataSet ds = da.dsGetTotalData(sql);
            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    ds.Tables[0].Columns.Add("Fuin", typeof(string));
            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        sql = "select * from app_platform.t_trust_limit where Fspid='" + dr["Fspid"].ToString() + "'";
            //        DataSet ds2 = da.dsGetTotalData(sql);
            //        if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            //        {
            //            DataRow dr2 = ds2.Tables[0].Rows[0];
            //            dr["Fuin"] = dr2["Fuin"];
            //        }
            //    }
            //}
            string ip = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("SettleRateIP", "10.123.6.29");
            int port = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("SettleRatePort", 443);
            DataTable dt = null;

            if (string.IsNullOrEmpty(szListid) )
            {
               return null;
            }
            else if (!string.IsNullOrEmpty(szListid))
            {
                string requestText = "reqid=176&flag=2&offset={0}&limit={1}&fields=time:0|transaction_id:{2}";
                requestText = string.Format(requestText, 0, 50, szListid);//"2201195901201210188118681566"

                if (subListid != null && !string.IsNullOrEmpty(subListid.Trim()))
                {
                    requestText += "|subListid:" + subListid;
                }

                DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "1407", ip, port);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];
                    dt.Columns.Add("Fuin", typeof(string));

                    foreach (DataRow dr in dt.Rows)
                    {
                        DataTable dt2 = QueryTrueLimtList(dr["Fspid"].ToString(), null, null);
                        if (dt2 != null && dt.Rows.Count > 0) 
                        {
                            dr["Fuin"] = dt2.Rows[0]["Fuin"].ToString();
                        }
                    }
                }
            }

            return dt;
        }


        //调帐查询函数
        public DataTable QueryAdjustList(string szListid, string spno, string spid, string adjustTime)
        {
            //MySqlAccess da = new MySqlAccess("Driver={MySQL ODBC 5.2a Driver};Database=mysql;Server=10.6.206.73;UID=zft_dev_all;PWD=zft_dev_all;charset=latin1;option=3;");
            //try
            //{
            //    int t = 0;
            //    string sql = "select Ftransaction_id,Fsp_bill_no,Fspid,Fnum,Ftype,Fstatus,Fmodify_time,Fadjust_time from app_platform.t_adjust where 1=1 ";
            //    if (szListid != null && !string.IsNullOrEmpty(szListid.Trim()))
            //    {
            //        t++;
            //        szListid = szListid.Trim();
            //        sql += " AND  Ftransaction_id='" + szListid + "'";
            //    }
            //    if (spno != null && !string.IsNullOrEmpty(spno.Trim()))
            //    {
            //        t++;
            //        spno = spno.Trim();
            //        sql += " AND  Fsp_bill_no='" + spno + "'";
            //    }
            //    if (spid != null && !string.IsNullOrEmpty(spid.Trim()))
            //    {
            //        if (adjustTime == null || string.IsNullOrEmpty(adjustTime.Trim()))
            //        {
            //            throw new Exception("调帐时间不能为空！");
            //        }
            //        t++;
            //        spid = spid.Trim();
            //        sql += " AND  Fspid='" + spid + "' AND DATE_FORMAT(Fadjust_time,'%Y-%m-%d')='" + adjustTime + "'";
            //    }
            //    if (t < 1)
            //    {
            //        throw new Exception("请输入查询条件！");
            //    }

            //    da.OpenConn();

            //    return da.dsGetTotalData(sql).Tables[0];
            //}
            //catch (Exception err)
            //{
            //    throw err;
            //}
            //finally
            //{
            //    da.Dispose();
            //}

            string ip = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("SettleRateIP", "10.123.6.29");
            int port = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("SettleRatePort", 443);
            DataTable dt = null;

            string requestText = "reqid=180&flag=2&offset={0}&limit={1}&fields=time:0";
            requestText = string.Format(requestText, 0, 10);
            if (szListid != null && !string.IsNullOrEmpty(szListid.Trim()))
            {
                requestText += "|transaction_id:" + szListid.Trim();
            }
            if (spno != null && !string.IsNullOrEmpty(spno.Trim()))
            {
                requestText += "|sp_bill_no:" + spno.Trim();
            }
            if (spid != null && !string.IsNullOrEmpty(spid.Trim()))
            {
                if (adjustTime == null || string.IsNullOrEmpty(adjustTime.Trim()))
                {
                    throw new Exception("调帐时间不能为空！");
                }
                requestText += "|spid:" + spno.Trim() + "|adjusttime:" + adjustTime.Trim();

            }
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "1407", ip, port);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }

        //调账订单查询
        public DataTable  GetAirAdjustList(int iType, string Flistid, string szSpid, string szBeginDate, string szEndDate, int iPageStart, int iPageMax)
        {
            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("AP"));
            //iPageStart = iPageStart - 1;
            //if (iPageStart < 0) iPageStart = 0;

            //try
            //{
            //    da.OpenConn();
            //    string sql = "select * from app_platform.t_adjust where ";

            //    if (iType == 1)
            //    {
            //        sql += " Ftransaction_id='" + Flistid + "'";
            //    }
            //    else if (iType == 2)
            //    {
            //        sql += " Fsp_bill_no='" + Flistid + "'";
            //    }
            //    else
            //    {
            //        sql += " Fspid='" + szSpid + "' and Fmodify_time>='" + szBeginDate + "' and Fmodify_time<='" + szEndDate + "' order by Fmodify_time desc";
            //    }

            //    sql = sql + " limit " + iPageStart + "," + iPageMax;

            //    return da.dsGetTotalData(sql);
            //}
            //catch (Exception err)
            //{
            //    return null;
            //}
            //finally
            //{
            //    da.Dispose();
            //}


            string ip = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("SettleRateIP", "10.123.6.29");
            int port = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("SettleRatePort", 443);
            DataTable dt = null;

            string requestText = "reqid=180&flag=2&offset={0}&limit={1}&fields=time:0";
            requestText = string.Format(requestText, iPageStart, iPageMax);

            if (iType == 1)
            {
                requestText += "|transaction_id:" + Flistid.Trim();
            }
            else if (iType == 2)
            {
                requestText += "|sp_bill_no:" + Flistid.Trim();
            }
            else
            {
                requestText += "|spid:" + szSpid + "|stime:" + szBeginDate + "|etime:" + szEndDate;
                
            }
             
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "1407", ip, port);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }


       //分账请求明细函数
        public DataTable  GetSettleReqInfo(string szListid)
        {
            //MySqlAccess da = new MySqlAccess("Driver={MySQL ODBC 5.2a Driver};Database=mysql;Server=10.6.206.73;UID=zft_dev_all;PWD=zft_dev_all;charset=latin1;option=3;");
            //try
            //{
            //    if (szListid == null || string.IsNullOrEmpty(szListid.Trim()))
            //    {
            //        throw new Exception("财付通订单号不能为空！");
            //    }
            //    szListid = szListid.Trim();
            //    da.OpenConn();
            //    string sql = "select Fsettle_list_id,Flistid,Fcoding,Fuin,Fstate,Fsettle_num,Fmodify_time from app_platform_" + szListid.Substring(26, 2) + ".t_settle_list_" + szListid.Substring(25, 1) +
            //        " where Flistid='" + szListid + "'";

            //    return da.dsGetTotalData(sql).Tables[0];
            //}
            //catch (Exception err)
            //{
            //    throw err;
            //}
            //finally
            //{
            //    da.Dispose();
            //}

            string ip = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("SettleRateIP", "10.123.6.29");
            int port = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("SettleRatePort", 443);
            DataTable dt = null;

            string requestText = "reqid=179&flag=2&offset={0}&limit={1}&fields=listid:{2}";
            requestText = string.Format(requestText, 0, 10, szListid);
            
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "1407", ip, port);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;

        }



       //分账请求查询
        public DataTable  GetSettleReqList(string szListid, string reqid)
        {
            string ip = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("SettleRateIP", "10.123.6.29");
            int port = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("SettleRatePort", 443);
            DataTable dt = null;

            string requestText = "reqid=178&flag=2&offset={0}&limit={1}&fields=listid:{2}";
            requestText = string.Format(requestText, 0, 10, szListid);

            if (!string.IsNullOrEmpty(reqid))
            {
                requestText += "|request_id:" + reqid;
            }
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "1407", ip, port);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;


            //MySqlAccess da = new MySqlAccess("Driver={MySQL ODBC 5.2a Driver};Database=mysql;Server=10.6.206.73;UID=zft_dev_all;PWD=zft_dev_all;charset=latin1;option=3;");
            //try
            //{
            //    if (szListid == null || string.IsNullOrEmpty(szListid.Trim()))
            //    {
            //        throw new Exception("财付通订单号不能为空！");
            //    }
            //    szListid = szListid.Trim();

            //    da.OpenConn();
            //    string sql = "select  Fsettle_request_id,Fsettle_list_id,Fcoding,Flistid,Fpnr,"
            //           + "Fcontact,Fpri_spid,Fflight_info,Fphone,Fticket_num,Fcurtype,Fsettle_fee,"
            //           + "Ftotal_fee,Fbus_type,Fbus_args,Fbus_desc,Fsp_bankurl,Fstate,Flstate,"
            //           + "Fcreate_time,Fsettle_time,Fmodify_time,Fagentid from app_platform_" + szListid.Substring(26, 2) + ".t_settle_request_" + szListid.Substring(25, 1) 
            //           + " where Flistid='" + szListid + "'";

            //    if (!string.IsNullOrEmpty(reqid.Trim()))
            //    {
            //        reqid = reqid.Trim();
            //        sql += " AND Fsettle_request_id='" + reqid + "'";
            //    }

            //    return da.dsGetTotalData(sql).Tables[0];
            //}
          
            //finally
            //{
            //    da.Dispose();
            //}
        }



        //补差账户查询函数
        public DataTable QueryTrueLimtList(string spid, string qqid, string trust_rule)
        {
            string ip = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("SettleRateIP", "10.123.6.29");
            int port = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("SettleRatePort", 443);
            DataTable dt = null;

            string requestText = "reqid=181&flag=2&offset={0}&limit={1}&fields=spid:{2}|time:0";
            requestText = string.Format(requestText, 0,10, spid);

            if (!string.IsNullOrEmpty(qqid))
            {
                requestText += "|uin:" + qqid;
            }
            if (!string.IsNullOrEmpty(trust_rule))
            {
                requestText += "|trust_rule:" + trust_rule;
            }
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "1407", ip, port);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }

            return dt;
        }


        //获取委托退款函数
        public DataTable GetTrustLimitList(string qqid, string Fspid)
        {
            //string uid = PublicRes.ConvertToFuid(qqid);
            //if (uid == null || uid.Length < 3)
            //{
            //    return null;
            //}
            //string sql = "select * from app_platform.t_trust_limit where Fuid = '" + uid + "' and Fspid = '" + Fspid + "' and Flstate = 1";
            DataTable dt = null;
            string ip = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("SettleRateIP", "10.123.6.29");
            int port = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("SettleRatePort", 443);
            string requestText = "reqid=155&flag=1&fields=spid:{0}|uin:{1}";
            requestText = string.Format(requestText, Fspid, qqid);

            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "1407", ip, port);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }

        //补差明细查询函数
        public DataTable  QuerySubOrderList(string mergeListid, string listid,int offset, int limit)
        {
            //string sql = "select * from app_platform_" + mergeListid.Substring(26, 2) + ".t_sub_order_" + mergeListid.Substring(25, 1) +
            //    " where Fmerge_listid='" + mergeListid + "'";
            //if (listid != null && !string.IsNullOrEmpty(listid.Trim()))
            //{
            //    listid = listid.Trim();
            //    sql += " AND Flistid='" + listid + "'";
            //}

            if (string.IsNullOrEmpty(mergeListid))
            {
                throw new Exception("原交易单号不能为空");
            }

            DataTable dt = new DataTable();
            string ip = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("SettleRateIP", "10.123.6.29");
            int port = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("SettleRatePort", 443);

            string requestText = "reqid=173&flag=2&offset={0}&limit={1}&fields=merge_listid:{2}";//2201195901201210191158345001
            requestText = string.Format(requestText, offset, limit, mergeListid );

            if (!string.IsNullOrEmpty(listid))
            {
                requestText += "|listid:" + listid;
            }
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "1407", ip, port);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }

        //冻结解冻
        public DataTable GetAirFreeze(string Spid, string QQid, string startDate, string EndDate, int offset, int limit) 
        {
            //    string sql = "select * from app_platform.t_freeze_roll where Fcreate_time>='" + szBeginDate + "' and Fcreate_time<='" + szEndDate + "'";
            //    sql += " and Fspid='" + szSpid + "'";
            //    sql += " and Fqqid='" + szQQid + "'";
            //    sql += " and Frole!=3  order by Fcreate_time desc ";
            //    sql = sql + " limit " + iPageStart + "," + iPageMax;
         
            DataTable dt = new DataTable();
            string ip = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("SettleRateIP", "10.123.6.29");
            int port = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("SettleRatePort", 443);

            string requestText = "reqid=140&flag=2&offset={0}&limit={1}&fields=stime:{2}|etime:{3}";
            requestText = string.Format(requestText, offset, limit, startDate, EndDate);
            if (!string.IsNullOrEmpty(Spid)) 
            {
                requestText += "|spid:" + Spid;
            }
            if (!string.IsNullOrEmpty(QQid))
            {
                requestText += "|uin:" + QQid;
            }
            
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "1407", ip, port);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }
      //分账附加信息查询
        public DataTable GetSettleListAppend(string szListid)
        {
            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("AP"));
            //try
            //{
            //    da.OpenConn();
            //    string sql = "select * from app_platform_" + szListid.Substring(26, 2) + ".t_air_order_append_" + szListid.Substring(25, 1) +
            //        " where Flistid='" + szListid + "'";

            //    return da.dsGetTotalData(sql);
            //}
            //catch (Exception err)
            //{
            //    throw new LogicException(err.Message);
            //}
            //finally
            //{
            //    da.Dispose();
            //}

            DataTable dt = null;
            string ip = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("SettleRateIP", "10.123.6.29");
            int port = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("SettleRatePort", 443);
            string requestText = "reqid=177&flag=1&fields=listid:{0}";
            requestText = string.Format(requestText, szListid);

            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "1407", ip, port);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }


      //分账明细信息查询
        public DataTable  GetSettleInfoListDetail(string szListid)
        {
            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("AP"));
            //try
            //{
            //    da.OpenConn();
            //    string sql = "select * from app_platform_" + szListid.Substring(26, 2) + ".t_settle_stat_" + szListid.Substring(25, 1) +
            //        " where Flistid='" + szListid + "' and Frole!=3";

            //    return da.dsGetTotalData(sql);
            //}
            //catch (Exception err)
            //{
            //    return null;
            //}
            //finally
            //{
            //    da.Dispose();
            //}

            string ip = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("SettleRateIP", "10.123.6.29");
            int port = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("SettleRatePort", 443);
            DataTable dt = null;

            string requestText = "reqid=182&flag=2&offset={0}&limit={1}&fields=listid:{2}";
            requestText = string.Format(requestText, 0, 10, szListid);
 
            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "1407", ip, port);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }

        //分账退款明细查询
        public DataTable  GetSettleRefundListDetail(string szRefundId, string szListid)
        {
            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("AP"));
            //try
            //{
            //    da.OpenConn();
            //    string sql = "select * from app_platform_" + szListid.Substring(26, 2) + ".t_settle_refund_" + szListid.Substring(25, 1) +
            //                    " where Flistid='" + szListid + "' and Fdrawid='" + szRefundId + "'order by Fmodify_time,Foper_type";

            //    return da.dsGetTotalData(sql);
            //}
            //catch (Exception err)
            //{
            //    return null;
            //}
            //finally
            //{
            //    da.Dispose();
            //}


            string ip = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("SettleRateIP", "10.123.6.29");
            int port = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("SettleRatePort", 443);
            DataTable dt = null;

            string requestText = "reqid=183&flag=2&offset={0}&limit={1}&fields=listid:{2}|drawid:{3}";
            requestText = string.Format(requestText, 0, 10, szListid, szRefundId);

            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "1407", ip, port);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }


       //分账退款查询
        public DataTable GetSettleRefundList(string Flistid, int iQueryType, int iPageStart, int iPageMax)
        {
            //MySqlAccess da = new MySqlAccess(PublicRes.GetConnString("AP"));
            //iPageStart = iPageStart - 1;
            //if (iPageStart < 0) iPageStart = 0;

            //try
            //{
            //    da.OpenConn();
            //    string sql = "select * from app_platform.t_spm_refund where ";

            //    if (iQueryType == 1)
            //    {
            //        sql += " Ftransaction_id='" + +;
            //    }
            //    else
            //    {
            //        sql += " Fdraw_id='" + Flistid;
            //    }

            //    sql = sql + "' limit " + iPageStart + "," + iPageMax;

            //    return da.dsGetTotalData(sql);
            //}
            //catch (Exception err)
            //{
            //    return null;
            //}
            //finally
            //{
            //    da.Dispose();
            //}

            string ip = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("SettleRateIP", "10.123.6.29");
            int port = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("SettleRatePort", 443);
            DataTable dt = null;

            string requestText = "reqid=184&flag=2&offset={0}&limit={1}&fields=time:0";
            requestText = string.Format(requestText, iPageStart, iPageMax);
            if (iQueryType == 1)
            {
                requestText += "|tran_id:" + Flistid;
            }
            else
            {
                requestText += "|draw_id:" + Flistid;
            }
            

            DataSet ds = RelayAccessFactory.GetDSFromRelayFromXML(requestText, "1407", ip, port);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;

        }

    }
}
