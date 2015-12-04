using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TENCENT.OSS.C2C.Finance.DataAccess;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CFT.CSOMS.DAL.Infrastructure;
using System.Configuration;
using System.IO;

namespace CFT.CSOMS.DAL.ForeignCardModule
{
    public class PayManageData
    {
        /// <summary>
        /// 查询外卡订单  
        /// </summary>
        /// <param name="sp_uid">商户id</param>
        /// <param name="spid">商户编号</param>
        /// <param name="sp_bill_no">商家订单号</param>
        /// <param name="transaction_id">财付通订单号</param>
        /// <param name="email">买方邮箱</param>
        /// <param name="s_time"></param>
        /// <param name="e_time"></param>
        /// <param name="trade_state">交易状态</param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public DataSet QueryForeignCardOrderList(string sp_uid, string spid, string sp_bill_no, string transaction_id, string email, string s_time, string e_time, string trade_state, int offset, int limit)
        {
            string msg = "";
            string reqid = "3906";

            if (string.IsNullOrEmpty(spid) && string.IsNullOrEmpty(transaction_id))
            {
                throw new Exception("spid与transaction_id不能都为空！");
            }
            try
            {
                DataSet ds = null;
                string req = "fields=sp_uid:" + sp_uid;
                if (!string.IsNullOrEmpty(spid))
                req += "|spid:" + spid;
                if (!string.IsNullOrEmpty(sp_bill_no))
                req += "|sp_bill_no:" + sp_bill_no;
                if (!string.IsNullOrEmpty(transaction_id))
                req += "|transaction_id:" + transaction_id;
                if (!string.IsNullOrEmpty(email))
                req += "|email:" + email;
                if (!string.IsNullOrEmpty(s_time))
                req += "|s_time:" + s_time;
                if (!string.IsNullOrEmpty(e_time))
                req += "|e_time:" + e_time;
                if (!string.IsNullOrEmpty(trade_state))
                req += "|trade_state:" + trade_state;
                req += "&offset=" + offset;  //从0开始的
                req += "&limit=" + limit;
                req += "&flag=2&reqid=" + reqid;
                ds = CommQuery.GetXmlToDataSetFromICEConvertUTF8(req, "", "common_query_service", out msg);
                if (msg != "")
                    throw new Exception(msg);
                return ds;
            }
            catch (Exception err)
            {
                throw new Exception("查询外卡订单失败！msg:" + msg+"   err:"+err.Message);
            }
        }

        /// <summary>
        /// 外卡拒付列表查询 
        /// </summary>
        /// <param name="query_time"></param>
        /// <param name="spid">商户编号</param>
        /// <param name="coding">商家订单号</param>
        /// <param name="list_id">财付通订单号</param>
        /// <param name="check_state">拒付状态</param>
        /// <param name="sp_process_state">商户处理状态</param>
        /// <param name="s_time">开始日期</param>
        /// <param name="e_time">结束日期</param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public DataSet QueryForeignCardRefusePayList(string query_time, string spid, string coding, string list_id, string check_state, string sp_process_state, string s_time, string e_time, int offset, int limit)
        {
            string msg = "";
            string reqid = "3917";

            if (string.IsNullOrEmpty(query_time))
            {
                throw new Exception("query_time不能为空！");
            }
            try
            {
                DataSet ds = null;
                string req = "fields=query_time:" + query_time;
                if (!string.IsNullOrEmpty(spid))
                    req += "|spid:" + spid;
                if (!string.IsNullOrEmpty(coding))
                    req += "|coding:" + coding;
                if (!string.IsNullOrEmpty(list_id))
                    req += "|list_id:" + list_id;
                if (!string.IsNullOrEmpty(check_state))
                    req += "|check_state:" + check_state;
                if (!string.IsNullOrEmpty(s_time))
                    req += "|s_time:" + s_time;
                if (!string.IsNullOrEmpty(e_time))
                    req += "|e_time:" + e_time;
                if (!string.IsNullOrEmpty(sp_process_state))
                    req += "|sp_process_state:" + sp_process_state;
                req += "&offset=" + offset;  //从0开始的
                req += "&limit=" + limit;
                req += "&flag=2&reqid=" + reqid;
                ds = CommQuery.GetXmlToDataSetFromICEConvertUTF8(req, "", "common_query_service", out msg);
                if (msg != "")
                    throw new Exception(msg);
                return ds;
            }
            catch (Exception err)
            {
                throw new Exception("查询外卡拒付列表失败！msg:" + msg + "   err:" + err.Message);
            }
        }

        /// <summary>
        /// 查询外卡商户流水 只查C账户
        /// </summary>
        /// <param name="fuid">商户C账户内部ID</param>
        /// <param name="spid">商户编号</param>
        /// <param name="s_time">开始日期</param>
        /// <param name="e_time">结束日期</param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public DataSet GetForeignCardRollList(string fuid, int fcurtype, string s_time, string e_time, int offset, int limit)
        {
            DataSet ds = null;
            string errMsg = "";
            try
            {
                string service_name = "qry_bankroll_list_service";
                string req = "MSG_NO=" + DateTime.Now.ToString("yyyyMMddHHmmss") + PublicRes.NewStaticNoManage();
                req += "&sp_id=2000000000";
                req += "&uid=" + fuid;
                req += "&curtype=" + fcurtype;
                req += "&s_time=" + s_time;
                req += "&e_time=" + e_time;
                req += "&offset=" + offset;
                req += "&limit=" + limit;

                ds = CommQuery.GetDataSetFromICE(req, CommQuery.个人资金流水, false, service_name, out errMsg);

                if (!string.IsNullOrEmpty(errMsg))
                    throw new Exception(errMsg);
            }
            catch (Exception err)
            {
                throw new Exception("查询外卡商户（C账户）流水失败！" + err.Message);
            }
            return ds;
        }

        /// <summary>
        //财付通外卡支付查询按订单号查询
        /// </summary>
        /// <param name="fields">relay接口特性参数串</param>
        /// <returns></returns>
        public DataSet QueryForeignCardInfoByOrder(string fields)
        {
            return new PublicRes().QueryCommRelay8020("3904", fields);
        }
     
        /// <summary>
        /// 财付通外卡支付查询，按商户号
        /// </summary>
        /// <param name="spid">商户号</param>
        /// <param name="fields">relay接口特性参数串</param>
        /// <returns></returns>
        public DataSet QueryForeignCardInfoByMerchant(string spid, string fields, int offset, int limit)
        {

            string strSql = "spid=" + spid;
            string errMsg = "";
            string f_strID = CommQuery.GetOneResultFromICE(strSql, CommQuery.QUERY_MERCHANTINFO, "FuidMiddle", out errMsg);

            if (f_strID == null || f_strID.Trim() == "")
            {
                return null;
            }
            fields += "|sp_uid:" + f_strID;

            return new PublicRes().QueryCommRelay8020("3953", fields, offset, limit);
        }
       
        /// <summary>
        ///外卡交易流水查询
        /// </summary>
        /// <param name="fields">relay接口特性参数串</param>
        /// <returns></returns>
        public DataSet QueryForeignCardRoll(string fields, int offset, int limit)
        {
            return new PublicRes().QueryCommRelay8020("3954", fields, offset, limit);
        }

        /// <summary>
        /// 财付通外卡支付查询，按银行订单号
        /// </summary>
        /// <param name="fields">relay接口特性参数串</param>
        /// <returns></returns>
        public DataSet QueryForeignCardInfoByBankOrder(string fields, int offset, int limit)
        {
            string order = "";
            try
            {
                DataSet ds = QueryForeignCardRoll(fields,0,1);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;
                order = ds.Tables[0].Rows[0]["Ftransaction_id"].ToString();
            }
            catch (Exception e)
            {
                throw new Exception("按银行订单号查订单号异常！" + e.Message);
            }

            return QueryForeignCardInfoByOrder("listid:" + order + "");
        }

        /// <summary>
        /// 查询外汇汇率列表函数
        /// </summary>
        /// <param name="foreType"></param>
        /// <param name="issueBank"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="iPageStart"></param>
        /// <param name="iPageMax"></param>
        /// <returns></returns>
        public DataSet GetExchangeRateList(string foreType, string issueBank, string beginTime, string endTime, int start, int max)
        {
            if (string.IsNullOrEmpty(beginTime) || string.IsNullOrEmpty(endTime))
            {
                throw new Exception("时间不能为空！");
            }

            string fields = "";
            if (foreType != null && foreType.Trim() != "" && foreType != "0")
            {
                fields += "|currency_type:" + foreType;
            }
            if (issueBank != null && issueBank.Trim() != "" && issueBank != "0")
            {
                fields += "|bank_type:" + issueBank;
            }
            if (beginTime != null && endTime != null)
            {
                fields += "|stime:" + beginTime;
                fields += "|etime:" + endTime;
            }

            fields = fields.Substring(1, fields.Length - 1);

            return new PublicRes().QueryCommRelay8020("3952", fields, start, max);

        }
    }
}
