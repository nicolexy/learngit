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
            string errMsg = "";
            try
            {
                string strWhere = "start_time=" + s_time;
                strWhere += "&end_time=" + e_time;
                strWhere += "&uid=" + fuid;
                strWhere += "&curtype=" + fcurtype;
                strWhere += "&lim_start=" + offset;
                strWhere += "&lim_count=" + limit;

                DataSet ds = new DataSet();
                if (!CommQuery.GetDataFromICE(strWhere, CommQuery.个人资金流水, out errMsg, out ds))
                {
                    throw new Exception(errMsg);
                }
                if (ds == null)
                    return null;
                return ds;
            }
            catch (Exception err)
            {
                throw new Exception("查询外卡商户（C账户）流水失败！" + err.Message);
            }
        }

        public DataSet QueryForeignCardInfoByOrder(string fields)
        {
            return new PublicRes().QueryCommRelay("3904", fields,0,1);
        }
    }
}
