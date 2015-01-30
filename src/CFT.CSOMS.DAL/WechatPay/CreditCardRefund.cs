using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using TENCENT.OSS.C2C.Finance.DataAccess;
using CFT.CSOMS.DAL.Infrastructure;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using System.Collections;
using CFT.Apollo.Logging;

namespace CFT.CSOMS.DAL.WechatPay
{
    public class CreditCardRefund
    {
        public DataSet QueryCreditCardRefundWX(string wxUid,string stime,string etime,int start,int count) 
        {
            if (string.IsNullOrEmpty(wxUid)) 
            {
                throw new ArgumentNullException("必填参数：账号为空！");
            }
            wxUid = wxUid.Trim();
            string tableName = "";
            if (wxUid.Length == 1)
            {
                tableName = "wx_public_platform_0" + wxUid + ".t_wx_fetch_list_0";
            }
            else if (wxUid.Length == 2)
            {
                tableName = "wx_public_platform_" + wxUid + ".t_wx_fetch_list_0";

            }
            else 
            {
                tableName = "wx_public_platform_" + wxUid.Substring(wxUid.Length - 2) + ".t_wx_fetch_list_" + wxUid.Substring(wxUid.Length - 3, 1);
            }

            StringBuilder Sql = new StringBuilder("select * from "+tableName);
            Sql.Append(" where Fuid=" + wxUid);
            if (!string.IsNullOrEmpty(stime)) 
            {
                Sql.Append(" AND Fcreate_time >='"+stime+"'");
            }
            if (!string.IsNullOrEmpty(etime))
            {
                Sql.Append(" AND Fcreate_time <='" + etime+"'");
            }
            Sql.Append(" limit "+start+","+count);

            using (var da = MySQLAccessFactory.GetMySQLAccess("WxCreditCard"))
            {
                da.OpenConn();
                
                DataSet ds = da.dsGetTotalData(Sql.ToString());

                return ds;
            }
        }

        public string QueryUidFromCreditCardOpenid(string openid)
        {
            string ret = "";
            try
            {
                if (string.IsNullOrEmpty(openid))
                {
                    throw new ArgumentNullException("openid为空！");
                }

                string Sql = "select Fuid from wx_public_platform.t_wx_relation where Fopenid='" + openid + "'";


                using (var da = MySQLAccessFactory.GetMySQLAccess("WxCreditCard"))
                {
                    da.OpenConn();

                    DataSet ds = da.dsGetTotalData(Sql);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) 
                    {
                        ret = ds.Tables[0].Rows[0]["Fuid"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return ret;
        }

        public DataSet QueryCreditCardRefundWX(string bankNo, string refundNo, string stime, string etime, int start, int count)
        {
            string table1 = "";
            string table2 = "";
            if (!string.IsNullOrEmpty(stime))
            {
                DateTime d = DateTime.Parse(stime);
                string tt = "";
                if (d.Month < 10)
                {
                    tt = d.Year + "0" + d.Month;
                }
                else 
                {
                    tt = d.Year.ToString() + d.Month;
                }
                table1 = "wx_public_platform.t_wx_fetch_list_" + tt;
            }
            if (!string.IsNullOrEmpty(etime))
            {
                DateTime d = DateTime.Parse(etime);
                string tt = "";
                if (d.Month < 10)
                {
                    tt = d.Year + "0" + d.Month;
                }
                else
                {
                    tt = d.Year.ToString() + d.Month;
                }
                table2 = "wx_public_platform.t_wx_fetch_list_" + tt;
            }
            if (table1 == "" && table2 == "") 
            {
                throw new Exception("起始日期不能为空");
            }

            string Sql1 = "SELECT * FROM "+table1;
            string Sql2 = "SELECT * FROM " + table2;
            string strWhere = "";
            
            if (!string.IsNullOrEmpty(bankNo))
            {
                bankNo = CreditEncode(bankNo);
                strWhere = " WHERE Fcard_id='"+bankNo+"'"; //卡号是加密的，需要调接口
            }
            else if (!string.IsNullOrEmpty(refundNo))
            {
                strWhere = " WHERE Fwx_fetch_no='" + refundNo+"'"; 
            }
            else 
            {
                throw new ArgumentNullException("查询参数不能为空！");
            }
            
            if (!string.IsNullOrEmpty(stime))
            {
                strWhere += " AND Fcreate_time >='" + stime+"'";
            }
            if (!string.IsNullOrEmpty(etime))
            {
                strWhere += " AND Fcreate_time <='" + etime+"'";
            }
            strWhere += " limit " + start + "," + count;


            Sql1 += strWhere;
            Sql2 += strWhere;

            using (var da = MySQLAccessFactory.GetMySQLAccess("WxCreditCard"))
            {
                da.OpenConn();

                DataSet ds = null;
                DataSet ds2 = null;

                if (table1 != "") 
                {
                    ds = da.dsGetTotalData(Sql1);
                }
                if (table2 != "" && table2 != table1) 
                {
                    ds2 = da.dsGetTotalData(Sql2);
                }
                if (ds != null)
                {
                    if (ds.Tables.Count == 0) 
                    {
                        ds.Tables.Add(new DataTable());
                    }

                    if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0) 
                    {
                        foreach (DataRow dr in ds2.Tables[0].Rows) 
                        {
                            ds.Tables[0].ImportRow(dr);
                        }
                    }
                }
                else 
                {
                    ds = ds2;
                }

                return ds;
            }
        }

        public DataTable QueryCreditCardRefundDetail(string wxUid, string wxFetchNo) 
        {
            if (string.IsNullOrEmpty(wxUid))
            {
                throw new ArgumentNullException("必填参数：账号为空！");
            }

            wxUid = wxUid.Trim();
            string tableName = "";
            if (wxUid.Length == 1)
            {
                tableName = "wx_public_platform_0" + wxUid + ".t_wx_fetch_list_0";
            }
            else if (wxUid.Length == 2)
            {
                tableName = "wx_public_platform_" + wxUid + ".t_wx_fetch_list_0";

            }
            else
            {
                tableName = "wx_public_platform_" + wxUid.Substring(wxUid.Length - 2) + ".t_wx_fetch_list_" + wxUid.Substring(wxUid.Length - 3, 1);
            }

            DataTable dt = null;

            using (var da = MySQLAccessFactory.GetMySQLAccess("WxCreditCard"))
            {
                da.OpenConn();
                string Sql = " select * from " + tableName + " where Fwx_fetch_no='"+wxFetchNo+"'";
                DataSet ds = da.dsGetTotalData(Sql);

                
                if (ds != null && ds.Tables.Count > 0) 
                {
                    //ds.Tables[0].Columns.Add("Ftruename",typeof(string));
                    
                    dt = ds.Tables[0];
                    //查找真实姓名
                    //string errMsg = "";
                    //string strSql = "uid=" + dt.Rows[0]["Fuid"].ToString();
                    //string ftruename = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_USERINFO, "Ftruename", out errMsg);

                    //dt.Rows[0]["Ftruename"] = ftruename;
                }
                

                return dt;
            }
        }

        public string CreditEncode(string creditId) 
        {

            string ret = "";
            string msg = "";
            DataSet ds = CommQuery.GetOneTableFromICE("creditid_list="+creditId, "FINANCE_OD_CREDIT_SECMSG", "bind_credit_secmsg_service", out msg);
            if (msg != "") 
            {
                throw new Exception(msg);
            }
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ret = ds.Tables[0].Rows[0]["creditcode_list"].ToString();
            }
            return ret;
        }

        //增值券查询
        public DataSet QueryAddedValueTicket(int accType, string uin, string couponId, string spId, string state, int start, int count)
        {
            DataSet ds = null;
            string key = System.Configuration.ConfigurationManager.AppSettings["AddedValueTicketKey"].ToString();
            string msg = string.Empty;
            StringBuilder token = new StringBuilder();
            token.Append(accType);
            token.Append("|");
            token.Append(uin);
            token.Append("|");
            token.Append(couponId);
            token.Append("|");
            token.Append(spId);
            token.Append("|");
            token.Append(state);
            token.Append("|");
            token.Append(start);
            token.Append("|");
            token.Append(count);
            token.Append("|");
            token.Append(key);
            LogHelper.LogInfo("token:" + token.ToString());
            string md5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(token.ToString(), "md5").ToLower();
            string serviceName = "fundcou_get_coupon_service";
            StringBuilder reqBuilder = new StringBuilder()
            .AppendFormat("acct_type={0}", accType)
            .AppendFormat("&acct_id={0}", uin)
            .AppendFormat("&coupon_id={0}", couponId)
            .AppendFormat("&spid={0}", spId)
            .AppendFormat("&state={0}", state)
            .AppendFormat("&offset={0}", start)
            .AppendFormat("&limit={0}", count)
            .AppendFormat("&token={0}", md5);

            LogHelper.LogInfo(serviceName + " send req:" + reqBuilder.ToString());
            //TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.GetOneTableFromICE(reqBuilder.ToString(), "", serviceName, true, out msg);
            
            string sReply;
            short iResult;
            string sMsg;
            bool isRet = commRes.middleInvoke(serviceName, reqBuilder.ToString(), true, out sReply, out iResult, out sMsg);
            LogHelper.LogInfo(serviceName + " return:" + sReply);
            if (isRet)
            {
                if (iResult == 0)
                {
                    ds = ParseAddedValueTicket(sReply);
                }
                else 
                {
                    LogHelper.LogInfo(serviceName + " ERROR:" + sMsg);
                }
            }
            else 
            {
                LogHelper.LogError(serviceName + " ERROR:"+sMsg);
            }

            return ds;
        }

        //根据批次号查券详情
        public DataTable QueryBatchTicketDetail(string batchId, string pwd) 
        {
            string key = "";
            string msg = string.Empty;
            StringBuilder token = new StringBuilder(batchId);
            token.Append("|");
            token.Append(pwd);
            token.Append("|");
            token.Append(key);

            string md5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(token.ToString(), "md5").ToLower();
            string serviceName = "fundcou_get_batch_service";
            StringBuilder reqBuilder = new StringBuilder()
            .AppendFormat("batch_id={0}", batchId)
            .AppendFormat("&passwd={0}", pwd)
            .AppendFormat("&token={0}", md5);

            LogHelper.LogInfo(serviceName + " send req:" + reqBuilder.ToString());
            TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.GetOneTableFromICE(reqBuilder.ToString(), "", serviceName, true, out msg);
            LogHelper.LogInfo(serviceName + " return:" + msg);

            return null;
        }

        private DataSet ParseAddedValueTicket(string str) 
        {
            if (string.IsNullOrEmpty(str)) 
            {
                return null;
            }
            DataSet dsresult = null;
            Hashtable ht = null;
            string[] strlist1 = str.Split('&');
            ht = new Hashtable(strlist1.Length);

            foreach (string strtmp in strlist1)
            {
                string[] strlist2 = strtmp.Split('=');
                if (strlist2.Length != 2)
                {
                    continue;
                }

                ht.Add(strlist2[0].Trim(), strlist2[1].Trim());
            }
            if (!ht.Contains("result") || ht["result"].ToString().Trim() != "0")
            {
                return null;
            }
            int count = int.Parse(ht["count"].ToString());
            if (count > 0)
            {
                dsresult = new DataSet();
                DataTable dt = new DataTable();
                dsresult.Tables.Add(dt);

                dt.Columns.Add("batch_id");
                dt.Columns.Add("batch_code");
                dt.Columns.Add("coupon_id");
                dt.Columns.Add("send_id");
                dt.Columns.Add("activity_id");
                dt.Columns.Add("acct_type");
                dt.Columns.Add("acct_id");
                dt.Columns.Add("value");
                dt.Columns.Add("state");
                dt.Columns.Add("expire_date");
                dt.Columns.Add("sp_ids");

                for (int i = 0; i < count; i++)
                {
                    DataRow drfield = dt.NewRow();
                    drfield.BeginEdit();

                    drfield["batch_id"] = ht["batch_id_" + i].ToString();
                    drfield["batch_code"] = ht["batch_code_" + i].ToString();
                    drfield["coupon_id"] = ht["coupon_id_" + i].ToString();
                    drfield["send_id"] = ht["send_id_" + i].ToString();
                    drfield["activity_id"] = ht["activity_id_" + i].ToString();
                    drfield["acct_type"] = ht["acct_type_" + i].ToString();
                    drfield["acct_id"] = ht["acct_id_" + i].ToString();
                    drfield["value"] = ht["value_" + i].ToString();
                    drfield["state"] = ht["state_" + i].ToString();
                    drfield["expire_date"] = ht["expire_date_" + i].ToString();
                    drfield["sp_ids"] = ht["sp_ids_" + i].ToString();

                    drfield.EndEdit();
                    dt.Rows.Add(drfield);
                }
            }
            return dsresult;
        }
    }
}
