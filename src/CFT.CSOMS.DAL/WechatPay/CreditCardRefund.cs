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
using System.Xml;
using CFT.CSOMS.COMMLIB;

namespace CFT.CSOMS.DAL.WechatPay
{
    public class CreditCardRefund
    {
        /// <param name="wxNo">微信号</param>
        /// <param name="bankNo">银行账号</param>
        /// <param name="refundNo">还款单号</param>
        public DataTable QueryCreditCardRefund(string No, string query_type, string stime, string etime, int offset, int limit)
        {
            //head_u=&sp_id=2000000000&request_type=110226&ver=1&sys_flag=1&module=zft_kf_server&type=3&business_type=1&repay_no=1000019701201512080356023736&start_day=2015-12-01&end_day=2015-12-31&limit=10&offset=0
            var ip = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("Relay_IP", "10.123.12.34");//172.27.31.177
            var port = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("Relay_PORT", 22000);
            //#if DEBUG
            //            stime = "2015-12-01";
            //            etime = "2015-12-31";
            //#endif

            string requestText = "sys_flag=1&module=zft_kf_server&business_type=1&start_day=" + stime + "&end_day=" + etime
                                    + "&offset=" + offset + "&limit=" + limit;
            //微信号
            if (query_type == "1")
            {
                string openid = WeChatHelper.GetXYKHKOpenIdFromWeChatName(No);
                //#if DEBUG
                //                openid = "o6BHkjiOuEkDSXPBaDbrelfpvg7k";
                //#endif

                requestText += "&type=1&openid=" + openid;

            }
            //银行账号
            else if (query_type == "2")
            {
                //#if DEBUG
                //                No = "6222000000000001";
                //#endif
                requestText += "&type=2&bank_no=" + No;
            }
            //还款单号
            else if (query_type == "3")
            {
                //#if DEBUG
                //                No = "1000019701201512080356023736";
                //#endif
                requestText += "&type=3&repay_no=" + No;
            }
            //微信支付账户
            else if (query_type == "4")
            {
                //#if DEBUG
                //                No = "085e9858eb5129227ba04f552@wx.tenpay.com";
                //#endif
                requestText += "&type=4&acctid=" + No;
            }
            else
            {
                throw new Exception("账号类型错误！");
            }

            DataSet ds = RelayAccessFactory.GetDSFromRelayMethod1(requestText, "110226", ip, port);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        //public DataSet QueryCreditCardRefundWX(string wxUid,string stime,string etime,int start,int count) 
        //{
        //    if (string.IsNullOrEmpty(wxUid)) 
        //    {
        //        throw new ArgumentNullException("必填参数：账号为空！");
        //    }
        //    wxUid = wxUid.Trim();
        //    string tableName = "";
        //    if (wxUid.Length == 1)
        //    {
        //        tableName = "wx_public_platform_0" + wxUid + ".t_wx_fetch_list_0";
        //    }
        //    else if (wxUid.Length == 2)
        //    {
        //        tableName = "wx_public_platform_" + wxUid + ".t_wx_fetch_list_0";

        //    }
        //    else
        //    {
        //        tableName = "wx_public_platform_" + wxUid.Substring(wxUid.Length - 2) + ".t_wx_fetch_list_" + wxUid.Substring(wxUid.Length - 3, 1);
        //    }

        //    StringBuilder Sql = new StringBuilder("select * from "+tableName);
        //    Sql.Append(" where Fuid=" + wxUid);
        //    if (!string.IsNullOrEmpty(stime)) 
        //    {
        //        Sql.Append(" AND Fcreate_time >='"+stime+"'");
        //    }
        //    if (!string.IsNullOrEmpty(etime))
        //    {
        //        Sql.Append(" AND Fcreate_time <='" + etime+"'");
        //    }
        //    Sql.Append(" limit "+start+","+count);

        //    using (var da = MySQLAccessFactory.GetMySQLAccess("WxCreditCard"))
        //    {
        //        da.OpenConn();
                
        //        DataSet ds = da.dsGetTotalData(Sql.ToString());

        //        return ds;
        //    }
        //}

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

        /// <summary>
        /// 微信订单查询接口
        /// </summary>
        /// <param name="type">类型 1 --- 根据微信订单号查询2 ---  根据商户号和商户订单号查询</param>
        /// <param name="wx_trans_id">微信订单号， 当type=1时 必传</param>
        /// <param name="mch_code">商户号， 当type=2时必传</param>
        /// <param name="out_trade_no">商户订单号， 当type=2时必传</param>
        /// <returns></returns>
        public DataTable QueryTradeOrder(int type, string wx_trans_id, string mch_code, string out_trade_no) 
        {
            var ip = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("WeChatAppId_Ip", "10.198.132.188");   //微信转发机VIP
            var port = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("WeChatAppId_Port", 22000);
            string parameterString;
     
            switch (type)
            {
                case 1:
                    parameterString = string.Format("<root><type>1</type><wx_trans_id>{0}</wx_trans_id></root>", wx_trans_id);
                    break;
                case 2:
                    parameterString = string.Format("<root><type>2</type><mch_code>{0}</mch_code><out_trade_no>{1}</out_trade_no></root>", mch_code, out_trade_no);
                    break;
                default: throw new Exception("type参数 错误");
            }
            var req = "wechat_xml_text=" + parameterString;
            var relay_result = RelayAccessFactory.RelayInvoke(req, "101090", false, false, ip, port);
            var relay_dic = SunLibraryEX.StringEx.ToDictionary(relay_result);
            if (relay_dic["result"] != "0")
            {
                throw new Exception("relay转发l5,异常 [" + relay_result + "]");
            }
            var result = System.Web.HttpUtility.UrlDecode(relay_dic["res_info"]);

            var resultXml = new XmlDocument();
            resultXml.LoadXml(result);
            var responseNode = resultXml.SelectSingleNode("Response");
            var errorCode = responseNode.SelectSingleNode("error").SelectSingleNode("code").InnerText;
            var errorMessage = responseNode.SelectSingleNode("error").SelectSingleNode("message").InnerText;
            if (errorCode != "0")
            {
                throw new Exception("接口返回异常:" + errorMessage);
            }

            var x_result = responseNode.SelectSingleNode("result");
            var dt = new DataTable();
            dt.Columns.Add("wx_trans_id");
            dt.Columns.Add("trade_no");
            dt.Columns.Add("pay_time");
            dt.Columns.Add("payment_amount");
            dt.Columns.Add("mask");
            dt.Columns.Add("portfolio_status");
            dt.Columns.Add("out_trade_no");
           
            var r_wx_trans_id = x_result.SelectSingleNode("wx_trans_id").FirstChild.InnerText;
            var r_out_trade_no = x_result.SelectSingleNode("out_trade_no").FirstChild.InnerText;
            var portfolio = x_result.SelectSingleNode("payment_portfolio");
            var items = portfolio.SelectNodes("item");

            foreach (XmlNode item in items)
            {
                var row = dt.NewRow();
                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == "wx_trans_id")
                    {
                        row[column.ColumnName] = r_wx_trans_id;
                    }
                    else if (column.ColumnName == "trade_no" && item.FirstChild != null)
                    {
                        row[column.ColumnName] = item.FirstChild.InnerText;
                    }
                    else if (column.ColumnName == "out_trade_no")
                    {
                        row[column.ColumnName] = r_out_trade_no;
                    }
                    else
                    {
                        var item_x = item.SelectSingleNode(column.ColumnName);
                        if (item_x != null)
                        {
                            row[column.ColumnName] = item_x.InnerText;
                        }
                    }
                }
                dt.Rows.Add(row);
            }
            return dt;
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
                dt.Columns.Add("value");//增值券面额大小,单位分，只有非固定面额批次才有效
                dt.Columns.Add("state");
                dt.Columns.Add("expire_date");
                dt.Columns.Add("sp_ids");
                dt.Columns.Add("threshold");//用券门槛，最低申购金额(分)

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
                    drfield["threshold"] = ht["threshold_" + i].ToString();
                    drfield["value"] = ht["value_" + i].ToString();

                    drfield.EndEdit();
                    dt.Rows.Add(drfield);
                }
            }
            return dsresult;
        }
    }
}
