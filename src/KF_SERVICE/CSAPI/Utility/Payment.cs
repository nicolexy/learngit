using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace CFT.CSOMS.Service.CSAPI.PayMent
{
    public class Payment
    {
        public Payment()
        {

        }
        #region 理财通

        public class UserProfit
        {
            [XmlElement("day")]
            public string Fday { get; set; }
            [XmlElement("spname")]
            public string Fspname { get; set; }
            [XmlElement("pur_type_name")]
            public string Fpur_typeName { get; set; }
            [XmlElement("profit_per_ten_thousand")]
            public string Fprofit_per_ten_thousand { get; set; }
            [XmlElement("f7day_profit_rate_str")]
            public string F7day_profit_rate_str { get; set; }
            [XmlElement("valid_money_str")]
            public string Fvalid_money_str { get; set; }
            [XmlElement("profit_str")]
            public string Fprofit_str { get; set; }

        }

        public class PayCardInfo
        {
            [XmlElement("bank_type")]
            public string Fbank_type { get; set; }
            [XmlElement("card_tail")]
            public string Fcard_tail { get; set; }
            [XmlElement("bank_type_name")]
            public string bank_type_name { get; set; }
        }

        public class UserTradePosInfo
        {
            [XmlElement("listid")]
            public string Flistid { get; set; }
            [XmlElement("sub_trans_id_str")]
            public string Fsub_trans_id_str { get; set; }
            [XmlElement("fetchid")]//客服系统增加提现单展示，客服部未添加
            public string Ffetchid { get; set; }
            [XmlElement("type_str")]
            public string FtypeText { get; set; }
            [XmlElement("total_fee_str")]
            public string Ftotal_fee_str { get; set; }
            [XmlElement("loading_type_str")]
            public string Floading_type_str { get; set; }
            [XmlElement("state_str")]
            public string Fstate_str { get; set; }
        }

        public class UserFundSummary
        {
            [XmlElement("spid")]
            public string Fspid { get; set; }
            [XmlElement("curtype")]
            public string Fcurtype { get; set; }
            [XmlElement("fund_code")]
            public string fund_code { get; set; }
            [XmlElement("close_flag")]
            public string close_flag { get; set; }
            [XmlElement("fund_name")]
            public string fundName { get; set; }
            [XmlElement("transfer_flag")]
            public string transfer_flag { get; set; }
            [XmlElement("buy_valid")]
            public string buy_valid { get; set; }
            [XmlElement("total_profit")]
            public string Ftotal_profit { get; set; }
            [XmlElement("balance")]
            public string balance { get; set; }
            [XmlElement("con")]
            public string con { get; set; }
        }

        public class CloseFundRoll
        {
            [XmlElement("seqno")]
            public string Fseqno { get; set; }
            [XmlElement("trade_id")]
            public string Ftrade_id { get; set; }
            [XmlElement("start_total_fee_str")]
            public string Fstart_total_fee_str { get; set; }
            [XmlElement("current_total_fee_str")]
            public string Fcurrent_total_fee_str { get; set; }
            [XmlElement("end_tail_fee_str")]
            public string Fend_tail_fee_str { get; set; }
            [XmlElement("trans_date")]
            public string Ftrans_date { get; set; }
            [XmlElement("start_date")]
            public string Fstart_date { get; set; }
            [XmlElement("end_date")]
            public string Fend_date { get; set; }
            [XmlElement("state_str")]
            public string Fstate_str { get; set; }
            [XmlElement("user_end_type_str")]
            public string Fuser_end_type_str { get; set; }
            [XmlElement("pay_type_str")]
            public string Fpay_type_str { get; set; }
            [XmlElement("channel_id_str")]
            public string Fchannel_id_str { get; set; }
        }

        public class UserFundAccountInfo
        {
            [XmlElement("true_name")]
            public string Ftrue_name { get; set; }
            [XmlElement("state_ame")]
            public string FstateName { get; set; }
            [XmlElement("mobile")]
            public string Fmobile { get; set; }
            [XmlElement("create_time")]
            public string Fcreate_time { get; set; }
        }



        public class ChildrenBankRoll
        {
            [XmlElement("create_time")]
            public string Fcreate_time { get; set; }
            [XmlElement("listid")]
            public string Flistid { get; set; }
            [XmlElement("type_text")]
            public string FtypeText { get; set; }
            [XmlElement("paynum_text")]
            public string FpaynumText { get; set; }
            [XmlElement("balance_text")]
            public string FbalanceText { get; set; }
            [XmlElement("con_str")]
            public string FconStr { get; set; }
            [XmlElement("memo_text")]
            public string FmemoText { get; set; }
        }

        public class BankCard
        {
            [XmlElement("pay_acc")]
            public string fpay_acc { get; set; }
            [XmlElement("bank_order")]
            public string fbank_order { get; set; }
            [XmlElement("amt_str")]
            public string FamtStr { get; set; }
            [XmlElement("biz_type_str")]
            public string Fbiz_type_str { get; set; }
        }

        public class BankCode
        {
            [XmlElement("Fno")]
            public string Fno { get; set; }
            [XmlElement("FType")]
            public string FType { get; set; }
            [XmlElement("Fvalue")]
            public string Fvalue { get; set; }
            [XmlElement("Fmemo")]
            public string Fmemo { get; set; }
            [XmlElement("Fsymbol")]
            public string Fsymbol { get; set; }
        }

        public class OpResult
        {
            [XmlElement("result")]
            public string result { get; set; }
        }
        #endregion

    }
}