using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using CFT.CSOMS.DAL.ForeignCurrencModule;
using CFT.CSOMS.DAL.Infrastructure;
using CFT.CSOMS.COMMLIB;

namespace CFT.CSOMS.BLL.ForeignCurrencyModule
{
    public class ForeignCurrencyService
    {
        //商户资料接口查询
        public DataSet MerInfoQuery(string spid, string uid, string ip)
        {
            DataSet ds= new MerchantData().MerInfoQuery(spid,uid,ip);
            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    DataTable dt = ds.Tables[0];
            //    string Fbalance_state = dt.Rows[0]["Fbalance_state"].ToString();
            //    if (Fbalance_state == "1")//1 不可使用余额支付 。2可以使用余额支付 
            //        ;
            //    else if (Fbalance_state == "2")
            //        ;
            //}
            //else
            //    throw new Exception("Service处理失败！判断是否可用余额支付出错");
            return ds;
        }

        //根据商户查询订单
        public DataSet QueryOrderBySpid(string sp_uid, string coding, string start_time, string end_time, string trade_state, int offset,int limit)
        {
            return new PayManageData().QueryOrderBySpid(sp_uid, coding, start_time, end_time, trade_state, offset, limit);
        }


        //根据订单号查询订单
        public DataSet QueryOrderByListId(string listid)
        {
            return new PayManageData().QueryOrderByListId(listid);
        }

        //根据订单号查询订单
        public DataSet BankRollQuery(string bill_no, string bank_type, string biz_type, string begin_datetime, string end_datetime)
        {
            return new PayManageData().BankRollQuery(bill_no, bank_type, biz_type, begin_datetime, end_datetime);
        }

        //外币银行卡解密
        public string BankDecodingQuery(string user_card)
        {
            DataSet ds = new PayManageData().BankDecodingQuery(user_card);
            string cardNo = "";
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                cardNo = ds.Tables[0].Rows[0]["user_card"].ToString();
            return CommUtil.BankIDEncode_ForBankCardUnbind(cardNo);
        }

        //根据交易单号或者订单号查询拒付单
        public DataSet QueryRefuseOrder(string spid, string listid, string create_time, string start_time, string end_time, string coding, string check_state, string sp_process_state, int offset, int limit)
        {
            return new PayManageData().QueryRefuseOrder(spid, listid, create_time, start_time, end_time, coding, check_state, sp_process_state, offset, limit);
        }

        //根据商户查询退款单
        public DataSet QueryRefundBySpid(string sp_uid, string sp_billno, string start_time, string end_time, string refund_state, int offset, int limit)
        {
            return new PayManageData().QueryRefundBySpid(sp_uid, sp_billno, start_time, end_time, refund_state, offset, limit);
        }

        //根据交易单号查询退款单
        public DataSet QueryRefundByTransId(string transaction_id, string start_time, string end_time, string refund_state, int offset, int limit)
        {
            return new PayManageData().QueryRefundByTransId(transaction_id, start_time, end_time, refund_state, offset, limit);
        }

        //查询商户acno
        public DataSet AcnoQuery(string spid, string acc_type, string cur_type)
        {
            return new MerchantData().AcnoQuery(spid, acc_type, cur_type);
        }

        //查询商户流水
        public DataSet QueryMerchantRoll(string uid, string sp_billno, string start_time, string end_time, string acno, int offset, int limit)
        {
            return new PayManageData().QueryMerchantRoll(uid, sp_billno, start_time, end_time, acno, offset, limit);
        }

        //商户结算查询
        public DataSet QueryMerSettlement(string spid, string s_time, string e_time, int offset, int limit)
        {
            return new PayManageData().QueryMerSettlement(spid, s_time, e_time, offset, limit);
        }
        //商户划款查询
        public DataSet QueryMerTransfer(string spid, string s_time, string e_time, int offset, int limit)
        {
            return new PayManageData().QueryMerTransfer(spid, s_time, e_time, offset, limit);
        }
    }
}
