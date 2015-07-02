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

            [XmlElement("fund_value")]
            public string fund_value { get; set; }
            [XmlElement("sday_profit_rate_str")]
            public string Sday_profit_rate_str { get; set; }
            [XmlElement("fund_balance")]
            public string fund_balance { get; set; }
            [XmlElement("mark_value")]
            public string mark_value { get; set; }
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

            [XmlElement("charge_fee_str")]
            public string charge_fee_str { get; set; }
            [XmlElement("fund_balance")]
            public string fund_balance { get; set; }
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
            [XmlElement("mark_value")]
            public string markValue { get; set; }
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

        public class LCTBalance
        {
            [XmlElement("balance")]
            public string Fbalance { get; set; }
        }
        #endregion

        #region 邮政汇款
        public class RemitList
        {
            [XmlElement("list_id")]
            public string Flistid { get; set; }
            [XmlElement("order_date")] //汇款平台日期
            public string Ford_date { get; set; }
            [XmlElement("order_ssn")] //汇款平台流水
            public string Ford_ssn { get; set; }
            [XmlElement("remit_fee")] //汇款金额
            public string FremitfeeName { get; set; }
            [XmlElement("remitpay_fee")] //汇费
            public string FremitpayfeeName { get; set; }

            [XmlElement("procedure_fee")] //汇款手续费
            public string FprocedureName { get; set; }
            [XmlElement("other_procedure_fee")] //退/改汇手续费
            public string FotherprocedureName { get; set; }
            [XmlElement("remit_rec")] //汇票号
            public string Fremit_rec { get; set; }
            [XmlElement("dest_name")] //收款人
            public string Fdest_name { get; set; }
            [XmlElement("dest_card")] //银行卡
            public string Fdest_card { get; set; }

            [XmlElement("trade_type")]
            public string FtrantypeName { get; set; }
            [XmlElement("trade_state")]
            public string FtranstateName { get; set; }
            [XmlElement("remit_type")]
            public string FremittypeName { get; set; }
            [XmlElement("data_type")]
            public string FdatatypeName { get; set; }
            [XmlElement("state")] //兑汇状态
            public string Fstate { get; set; }
        }

        public class RemitSpid
        {
            [XmlElement("spid")]
            public string spid { get; set; }
        }

        #endregion

        #region 交易记录查询

        public class TradePayList
        {
            [XmlElement("pay_type_str")]
            public string Fpay_type_str { get; set; }
            [XmlElement("paybuy")]
            public string fpaybuy { get; set; }
            [XmlElement("paysale")]
            public string fpaysale { get; set; }
            [XmlElement("appeal_sign_str")]
            public string Fappeal_sign_str { get; set; }
            [XmlElement("medi_sign_str")]
            public string Fmedi_sign_str { get; set; }

            [XmlElement("channel_id_str")]
            public string Fchannel_id_str { get; set; }
            [XmlElement("buy_bankid")]
            public string fbuy_bankid { get; set; }
            [XmlElement("closereason")]
            public string CloseReason { get; set; }
            [XmlElement("bank_backid")]
            public string fbank_backid { get; set; }
            [XmlElement("bank_listid")]
            public string fbank_listid { get; set; }

            [XmlElement("bargain_time")]
            public string fbargain_time { get; set; }
            [XmlElement("buy_bank_type")]
            public string fbuy_bank_type { get; set; }
            [XmlElement("buy_name")]
            public string fbuy_name { get; set; }
            [XmlElement("buy_uid")]
            public string fbuy_uid { get; set; }
            [XmlElement("carriage")]
            public string fcarriage { get; set; }

            [XmlElement("cash")]
            public string fcash { get; set; }
            [XmlElement("coding")]
            public string fcoding { get; set; }
            [XmlElement("create_time")]
            public string fcreate_time { get; set; }
            [XmlElement("create_time_c2c")]
            public string fcreate_time_c2c { get; set; }
            [XmlElement("curtype")]
            public string fcurtype { get; set; }

            [XmlElement("fact")]
            public string ffact { get; set; }
            [XmlElement("ip")]
            public string fip { get; set; }
            [XmlElement("listid")]
            public string flistid { get; set; }
            [XmlElement("lstate")]
            public string flstate { get; set; }
            [XmlElement("modify_time")]
            public string fmodify_time { get; set; }

            [XmlElement("pay_time")]
            public string fpay_time { get; set; }
            [XmlElement("paynum")]
            public string fpaynum { get; set; }
            [XmlElement("price")]
            public string fprice { get; set; }
            [XmlElement("procedure")]
            public string fprocedure { get; set; }
            [XmlElement("receive_time")]
            public string freceive_time { get; set; }

            [XmlElement("receive_time_c2c")]
            public string freceive_time_c2c { get; set; }
            [XmlElement("sale_bank_type")]
            public string fsale_bank_type { get; set; }
            [XmlElement("sale_bankid")]
            public string fsale_bankid { get; set; }
            [XmlElement("sale_name")]
            public string fsale_name { get; set; }
            [XmlElement("sale_uid")]
            public string fsale_uid { get; set; }

            [XmlElement("saleid")]
            public string fsaleid { get; set; }
            [XmlElement("service")]
            public string fservice { get; set; }
            [XmlElement("spid")]
            public string fspid { get; set; }
            [XmlElement("adjust_flag")]
            public string fadjust_flag { get; set; }
            [XmlElement("refund_typeName")]
            public string Frefund_typeName { get; set; }

            [XmlElement("req_refund_time")]
            public string freq_refund_time { get; set; }
            [XmlElement("ok_time")]
            public string fok_time { get; set; }
            [XmlElement("ok_time_acc")]
            public string fok_time_acc { get; set; }
            [XmlElement("memo")]
            public string fmemo { get; set; }
            [XmlElement("explain")]
            public string fexplain { get; set; }

            [XmlElement("buyid")]
            public string fbuyid { get; set; }
            [XmlElement("trade_type")]
            public string ftrade_type { get; set; }
            [XmlElement("saleidCFT")]
            public string FsaleidCFT { get; set; }
            [XmlElement("trade_stateName")]
            public string Ftrade_stateName { get; set; }
            [XmlElement("buyidCFT")]
            public string FbuyidCFT { get; set; }
        }

        #endregion

        /// <summary>
        /// 充值记录
        /// </summary>
        public class TCBankRollList
        {
            [XmlElement("trade_id")]
            public string Ftde_id { get; set; }
            [XmlElement("listid")]
            public string Flistid { get; set; }
            [XmlElement("curtype")]
            public string Fcurtype { get; set; }
            [XmlElement("curtype_str")]
            public string Fcurtype_str { get; set; }
            [XmlElement("state")]   //当前状态
            public string Fstate { get; set; }
            [XmlElement("state_str")]
            public string Fstate_str { get; set; }
            [XmlElement("type")]    //交易类型
            public string Ftype { get; set; }
            [XmlElement("type_str")]
            public string Ftype_str { get; set; }

            [XmlElement("subject")] //科目
            public string Fsubject { get; set; }
            [XmlElement("subject_str")]
            public string Fsubject_str { get; set; }
            [XmlElement("num")] //交易金额
            public string Fnum_str { get; set; }
            [XmlElement("sign")]    //交易标记
            public string Fsign { get; set; }
            [XmlElement("sign_str")]
            public string Fsign_str { get; set; }
            [XmlElement("bank_type")]
            public string Fbank_type { get; set; }
            [XmlElement("bank_type_str")]
            public string Fbank_type_str { get; set; }

            [XmlElement("auid")]    //对方内部账户ID
            public string Fauid { get; set; }
            [XmlElement("aid")]    //对方的ID
            public string Faid { get; set; }
            [XmlElement("aname")]    //对方的名称
            public string Faname { get; set; }
            [XmlElement("bank_list")]
            public string Fbank_list { get; set; }
            [XmlElement("bank_acc")]
            public string Fbank_acc { get; set; }
            [XmlElement("pay_time_acc")]
            public string Fpay_time_acc { get; set; }
            [XmlElement("pay_front_time")]
            public string Fpay_front_time { get; set; }
            [XmlElement("pay_time")]
            public string Fpay_time { get; set; }

            [XmlElement("ip")]
            public string Fip { get; set; }
            [XmlElement("memo")]
            public string Fmemo { get; set; }
            [XmlElement("prove")]
            public string Fprove { get; set; }
            [XmlElement("tc_bankid")]
            public string Ftc_bankid { get; set; }
        }
    }
}