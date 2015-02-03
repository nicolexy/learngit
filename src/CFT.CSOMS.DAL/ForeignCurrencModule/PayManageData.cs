using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TENCENT.OSS.C2C.Finance.DataAccess;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CFT.CSOMS.DAL.Infrastructure;
using System.Configuration;

namespace CFT.CSOMS.DAL.ForeignCurrencModule
{
    public class PayManageData
    {
        //根据商户查询订单
        public DataSet QueryOrderBySpid(string sp_uid, string coding, string start_time, string end_time, string trade_state,int offset,int limit)
        {
            string msg = "";
            string req = "";

            if (string.IsNullOrEmpty(sp_uid))
            {
                throw new Exception("sp_uid不能为空！");
            }
            try
            {
                DataSet ds = null;
                if (!string.IsNullOrEmpty(sp_uid))
                    req += "&sp_uid=" + sp_uid;
                if (!string.IsNullOrEmpty(coding))
                    req += "&coding=" + coding;
                if (!string.IsNullOrEmpty(start_time))
                    req += "&start_time=" + start_time;
                if (!string.IsNullOrEmpty(end_time))
                    req += "&end_time=" + end_time;
                if (!string.IsNullOrEmpty(trade_state))
                    req += "&trade_state=" + trade_state;
                req += "&offset=" + offset + "&limit=" + limit;
                ds = CommQuery.GetDataSetFromICEIA(req, "QUERY_MER_B2C_ORDER", out msg);
                if (msg != "")
                    throw new Exception("Service处理失败！" + msg);

                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("根据商户查询订单失败！" + msg);
            }
        }

        //根据订单号查询订单
        public DataSet QueryOrderByListId(string listid)
        {
            string msg = "";
            string req = "";

            if (string.IsNullOrEmpty(listid))
            {
                throw new Exception("listid不能为空！");
            }
            try
            {
                DataSet ds = null;
                if (!string.IsNullOrEmpty(listid))
                    req += "listid=" + listid;

                ds = CommQuery.GeOnetDataSetFromICEIA(req, "QUERY_B2C_ORDER", out msg);
                if (msg != "")
                    throw new Exception("Service处理失败！" + msg);

                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("根据订单号查询订单失败！" + msg);
            }
        }

        //银行流水查询
        public DataSet BankRollQuery(string bill_no, string bank_type, string biz_type, string begin_datetime, string end_datetime)
        {
            string msg = "";
            string service_name = "bank_pos_single_query_service";
            string req = "";

            if (string.IsNullOrEmpty(bill_no) && string.IsNullOrEmpty(bank_type) && string.IsNullOrEmpty(biz_type))
            {
                throw new Exception("bill_no和bank_type和biz_type不能为空！");
            }
            try
            {
                DataSet ds = null;
                if (!string.IsNullOrEmpty(bill_no))
                    req += "bill_no=" + bill_no;
                if (!string.IsNullOrEmpty(bank_type))
                    req += "&bank_type=" + bank_type;
                if (!string.IsNullOrEmpty(biz_type))
                req += "&biz_type=" + biz_type;
                if (!string.IsNullOrEmpty(begin_datetime))
                    req += "&begin_datetime=" + begin_datetime;
                if (!string.IsNullOrEmpty(end_datetime))
                    req += "&end_datetime=" + end_datetime;
                ds = CommQuery.AIGetOneTableFromICE(req, "", service_name, false, out msg);
                if (msg != "")
                    throw new Exception(msg);

                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("银行流水查询Service处理失败！" + msg);
            }
        }

        //外币银行卡解密
        public DataSet BankDecodingQuery(string user_card)
        {
            string msg = "";
            string service_name = "ia_card_encrpt_service";
            string req = "";

            if (string.IsNullOrEmpty(user_card))
            {
                throw new Exception("user_card不能为空！");
            }
            try
            {
                DataSet ds = null;
                if (!string.IsNullOrEmpty(user_card))
                    req += "user_card=" + user_card;
                req += "&type=2";
                req += "&watchcode=" + ConfigurationManager.AppSettings["IABankWatchcode"].Trim();

                ds = CommQuery.AIGetOneTableFromICE(req, "", service_name, true, out msg);
                if (msg != "")
                    throw new Exception(msg);

                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("外币银行卡解密Service处理失败！" + msg);
            }
        }

        //根据交易单号或者订单号查询拒付单
        public DataSet QueryRefuseOrder(string spid, string listid, string create_time, string start_time, string end_time, string coding, string check_state, string sp_process_state, int offset, int limit)
        {
            string msg = "";
            string req = "";

            if (string.IsNullOrEmpty(create_time))
            {
                throw new Exception("create_time不能为空！");
            }
            try
            {
                DataSet ds = null;
                if (!string.IsNullOrEmpty(create_time))
                    req += "&create_time=" + create_time;
                if (!string.IsNullOrEmpty(spid))
                    req += "&spid=" + spid;
                if (!string.IsNullOrEmpty(listid))
                    req += "&listid=" + listid;
                if (!string.IsNullOrEmpty(start_time))
                    req += "&start_time=" + start_time;
                if (!string.IsNullOrEmpty(end_time))
                    req += "&end_time=" + end_time;
                if (!string.IsNullOrEmpty(coding))
                    req += "&coding=" + coding;
                if (!string.IsNullOrEmpty(check_state))
                    req += "&check_state=" + check_state;
                if (!string.IsNullOrEmpty(sp_process_state))
                    req += "&sp_process_state=" + sp_process_state;
                req += "&offset=" + offset + "&limit=" + limit;
                ds = CommQuery.GetDataSetFromICEIA(req, "CB_LIST_BATCH_QUERY", out msg);
                if (msg != "")
                    throw new Exception("Service处理失败！" + msg);

                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("根据交易单号或者订单号查询拒付单失败！" + msg);
            }
        }

        //根据商户查询退款单
        public DataSet QueryRefundBySpid(string sp_uid, string sp_billno, string start_time, string end_time, string refund_state, int offset, int limit)
        {
            string msg = "";
            string req = "";

            if (string.IsNullOrEmpty(sp_uid))
            {
                throw new Exception("sp_uid不能为空！");
            }
            try
            {
                DataSet ds = null;
                if (!string.IsNullOrEmpty(sp_uid))
                    req += "&sp_uid=" + sp_uid;
                if (!string.IsNullOrEmpty(sp_billno))
                    req += "&sp_billno=" + sp_billno;
                if (!string.IsNullOrEmpty(start_time))
                    req += "&start_time=" + start_time;
                if (!string.IsNullOrEmpty(end_time))
                    req += "&end_time=" + end_time;
                if (!string.IsNullOrEmpty(refund_state))
                    req += "&refund_state=" + refund_state;
                req += "&offset=" + offset + "&limit=" + limit;
                ds = CommQuery.GetDataSetFromICEIA(req, "QUERY_MER_REFUND", out msg);
                if (msg != "")
                    throw new Exception("Service处理失败！" + msg);

                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("根据商户查询退款单失败！" + msg);
            }
        }

        //根据交易单号查询退款单
        public DataSet QueryRefundByTransId(string transaction_id, string start_time, string end_time, string refund_state, int offset, int limit)
        {
            string msg = "";
            string req = "";

            if (string.IsNullOrEmpty(transaction_id))
            {
                throw new Exception("transaction_id不能为空！");
            }
            try
            {
                DataSet ds = null;
                if (!string.IsNullOrEmpty(transaction_id))
                    req += "transaction_id=" + transaction_id;
                if (!string.IsNullOrEmpty(start_time))
                    req += "&start_time=" + start_time;
                if (!string.IsNullOrEmpty(end_time))
                    req += "&end_time=" + end_time;
                if (!string.IsNullOrEmpty(refund_state))
                    req += "&refund_state=" + refund_state;
                req += "&offset=" + offset + "&limit=" + limit;
                ds = CommQuery.GetDataSetFromICEIA(req, "QUERY_REFUND", out msg);
                if (msg != "")
                    throw new Exception("Service处理失败！" + msg);

                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("根据交易单号查询退款单失败！" + msg);
            }
        }

        //查询商户流水
        public DataSet QueryMerchantRoll(string uid, string sp_billno, string start_time, string end_time, string acno ,int offset, int limit)
        {
            string msg = "";
            string req = "";

            if (string.IsNullOrEmpty(uid))
            {
                throw new Exception("uid不能为空！");
            }
            try
            {
                DataSet ds = null;
                if (!string.IsNullOrEmpty(uid))
                    req += "uid=" + uid;
                if (!string.IsNullOrEmpty(sp_billno))
                    req += "&sp_billno=" + sp_billno;
                if (!string.IsNullOrEmpty(start_time))
                    req += "&start_time=" + start_time;
                if (!string.IsNullOrEmpty(end_time))
                    req += "&end_time=" + end_time;
                if (!string.IsNullOrEmpty(acno))
                    req += "&acno=" + acno;
                req += "&offset=" + offset + "&limit=" + limit;
                ds = CommQuery.GetDataSetFromICEIA(req, "QUERY_MER_BANKROLL_LIST", out msg);
                if (msg != "")
                    throw new Exception(msg);

                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("查询商户流水失败！" + msg);
            }
        }

        //商户结算查询
        public DataSet QueryMerSettlement(string spid, string begin_date, string end_date, int offset, int limit)
        {
            string msg = "";
            string req = "";

            if (string.IsNullOrEmpty(spid))
            {
                throw new Exception("spid不能为空！");
            }
            try
            {
                DataSet ds = null;
                if (!string.IsNullOrEmpty(spid))
                    req += "&spid=" + spid;
                if (!string.IsNullOrEmpty(begin_date))
                    req += "&begin_date=" + begin_date;
                if (!string.IsNullOrEmpty(end_date))
                    req += "&end_date=" + end_date;
                req += "&offset=" + offset + "&limit=" + limit;
                ds = CommQuery.GetDataSetFromICEIA(req, "SETTLE_MER_QUERY_IA_INCOME_DAY", out msg);
                if (msg != "")
                    throw new Exception(msg);

                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("商户结算查询失败！" + msg);
            }
        }


        //商户划款查询
        public DataSet QueryMerTransfer(string spid, string begin_date, string end_date, int offset, int limit)
        {
            string msg = "";
            string req = "";

            if (string.IsNullOrEmpty(spid))
            {
                throw new Exception("spid不能为空！");
            }
            try
            {
                DataSet ds = null;
                if (!string.IsNullOrEmpty(spid))
                    req += "&spid=" + spid;
                if (!string.IsNullOrEmpty(begin_date))
                    req += "&begin_date=" + begin_date;
                if (!string.IsNullOrEmpty(end_date))
                    req += "&end_date=" + end_date;
                req += "&offset=" + offset + "&limit=" + limit;
                ds = CommQuery.GetDataSetFromICEIA(req, "SETTLE_MER_QUERY_IA_DRAW_DAY", out msg);
                if (msg != "")
                    throw new Exception(msg);

                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("商户划款查询失败！" + msg);
            }
        }
    }
}
