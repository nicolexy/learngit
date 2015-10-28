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
            public string ftde_id { get; set; }
            [XmlElement("listid")]
            public string flistid { get; set; }
            [XmlElement("curtype")]
            public string fcurtype { get; set; }
            [XmlElement("curtype_str")]
            public string Fcurtype_str { get; set; }
            [XmlElement("state")]   //当前状态
            public string fstate { get; set; }
            [XmlElement("state_str")]
            public string Fstate_str { get; set; }
            [XmlElement("type")]    //交易类型
            public string ftype { get; set; }
            [XmlElement("type_str")]
            public string Ftype_str { get; set; }

            [XmlElement("subject")] //科目
            public string fsubject { get; set; }
            [XmlElement("subject_str")]
            public string Fsubject_str { get; set; }
            [XmlElement("num")] //交易金额
            public string Fnum_str { get; set; }
            [XmlElement("sign")]    //交易标记
            public string fsign { get; set; }
            [XmlElement("sign_str")]
            public string Fsign_str { get; set; }
            [XmlElement("bank_type")]
            public string fbank_type { get; set; }
            [XmlElement("bank_type_str")]
            public string Fbank_type_str { get; set; }

            [XmlElement("auid")]    //对方内部账户ID
            public string fauid { get; set; }
            [XmlElement("aid")]    //对方的ID
            public string faid { get; set; }
            [XmlElement("aname")]    //对方的名称
            public string faname { get; set; }
            [XmlElement("bank_list")]
            public string fbank_list { get; set; }
            [XmlElement("bank_acc")]
            public string fbank_acc { get; set; }
            [XmlElement("pay_time_acc")]
            public string fpay_time_acc { get; set; }
            [XmlElement("pay_front_time")]
            public string fpay_front_time { get; set; }
            [XmlElement("pay_time")]
            public string fpay_time { get; set; }

            [XmlElement("ip")]
            public string fip { get; set; }
            [XmlElement("memo")]
            public string fmemo { get; set; }
            [XmlElement("prove")]
            public string fprove { get; set; }
            [XmlElement("tc_bankid")]
            public string ftc_bankid { get; set; }
        }

        #region 手Q支付

        public class RefundHandQList
        {
            [XmlElement("card_name")]
            public string card_name { get; set; }
            [XmlElement("num_str")]
            public string num_str { get; set; }
            [XmlElement("bank_type_str")]
            public string bank_type_str { get; set; }
            [XmlElement("card_id")]
            public string card_id { get; set; }
            [XmlElement("create_time")]
            public string create_time { get; set; }
            [XmlElement("state_str")]
            public string state_str { get; set; }
            [XmlElement("isTP")]
            public string isTP { get; set; }
            [XmlElement("wx_fetch_no")]
            public string wx_fetch_no { get; set; }
        }

        public class RefundHandQDetail
        {
            [XmlElement("openid")]
            public string openid { get; set; }
            [XmlElement("card_id")]
            public string card_id { get; set; }
            [XmlElement("wx_fetch_no")]
            public string wx_fetch_no { get; set; }
            [XmlElement("card_name")]
            public string card_name { get; set; }
            [XmlElement("num_str")]
            public string num_str { get; set; }
            [XmlElement("bank_type_str")]
            public string bank_name { get; set; }
            [XmlElement("create_time")]
            public string create_time { get; set; }
            [XmlElement("state_str")]
            public string state_str { get; set; }
            [XmlElement("modify_time")]
            public string modify_time { get; set; }
            [XmlElement("cft_fetch_no")]
            public string cft_fetch_no { get; set; }
            [XmlElement("trade_no")]
            public string trade_no { get; set; }
        }

        public class SendRedPacketInfo
        {
            [XmlElement("list_id")]
            public string send_listid { get; set; }
            [XmlElement("create_time")]
            public string create_time { get; set; }
            [XmlElement("title")]
            public string Title { get; set; }
            [XmlElement("send_listid")]
            public string send_listidex { get; set; }
            [XmlElement("amount_text")]
            public string amount_text { get; set; }
            [XmlElement("state_text")]
            public string state_text { get; set; }
            [XmlElement("summary")]
            public string summary { get; set; }
            [XmlElement("refund")]
            public string refund { get; set; }
            [XmlElement("channel_text")]
            public string channel_text { get; set; }
            [XmlElement("wishing")]
            public string wishing { get; set; }
        }

        public class RecvRedPacketInfo
        {
            [XmlElement("list_id")]
            public string send_listid { get; set; }
            [XmlElement("create_time")]
            public string create_time { get; set; }
            [XmlElement("amount_text")]
            public string amount_text { get; set; }
            [XmlElement("wishing")]
            public string wishing { get; set; }
            [XmlElement("title")]
            public string Title { get; set; }
            [XmlElement("recv_listid")]
            public string recv_listid { get; set; }
            [XmlElement("channel_text")]
            public string channel_text { get; set; }
        }

        public class RedPacketDetail
        {
            [XmlElement("recv_listid")]
            public string recv_listid { get; set; }
            [XmlElement("create_time")]
            public string create_time { get; set; }
            [XmlElement("title")]
            public string Title { get; set; }
            [XmlElement("send_uin")]
            public string send_uin { get; set; }
            [XmlElement("send_name")]
            public string send_name { get; set; }
            [XmlElement("recv_uin")]
            public string recv_uin { get; set; }
            [XmlElement("recv_name")]
            public string recv_name { get; set; }
            [XmlElement("amount_text")]
            public string amount_text { get; set; }
            [XmlElement("channel_text")]
            public string channel_text { get; set; }
        }

        #endregion

        #region 姓名生僻字

        public class RareNameList
        {
            [XmlElement("user_type")]
            public string Fuser_type { get; set; }
            [XmlElement("user_type_str")]
            public string Fuser_type_str { get; set; }
            [XmlElement("record_type_str")]
            public string record_type_str { get; set; }
            [XmlElement("updateuser")]
            public string updateuser { get; set; }
            [XmlElement("card_state")]
            public string Fcard_state { get; set; }
            [XmlElement("card_state_str")]
            public string card_state_str { get; set; }
            [XmlElement("modify_type")]
            public string modify_type { get; set; }
            [XmlElement("modify_time")]
            public string Fmodify_time { get; set; }
            [XmlElement("card_no")]
            public string Fcard_no { get; set; }
            [XmlElement("account_name")]
            public string Faccount_name { get; set; }
        }

        #endregion
    }

    public class WechatApi
    {
        public WechatApi()
        { 
        
        }

        public class WechatReceviceHB
        {
            //查询详情时需要Listid，SendListId
            [XmlElement("create_time")]
            public string CreateTime { get; set; }
            [XmlElement("title")]
            public string Title { get; set; }
            [XmlElement("amount_text")]
            public string Amount_text { get; set; }
            [XmlElement("listid")]
            public string Listid { get; set; }
            [XmlElement("send_listid")]
            public string SendListId { get; set; }
        }

        public class WechatSendHB
        {
            //查询详情时需要Listid
            [XmlElement("createTime")]
            public string CreateTime { get; set; }
            [XmlElement("totalAmount_text")]
            public string TotalAmount_text { get; set; }
            [XmlElement("state")]
            public string State { get; set; }
            [XmlElement("state_text")]
            public string State_text { get; set; }
            [XmlElement("summary")]
            public string Summary { get; set; }
            [XmlElement("refund")]
            public string Refund { get; set; }
            [XmlElement("listid")]
            public string Listid { get; set; }
        }

        public class WechatHBDetail
        {
            [XmlElement("send_listid")]
            public string SendListId { get; set; }
            [XmlElement("pay_listid")]
            public string PayListid { get; set; }
            [XmlElement("create_time")]
            public string CreateTime { get; set; }
            [XmlElement("receive_name")]
            public string ReceiveName { get; set; }
            [XmlElement("sendOpenid_text")]
            public string SendOpenid_text { get; set; }
            [XmlElement("receiveOpenid_text")]
            public string ReceiveOpenid_text { get; set; }
            [XmlElement("amount_text")]
            public string Amount_text { get; set; }
            [XmlElement("wishing")]
            public string Wishing { get; set; }
        }

        public class WechatAAAcount
        {
            [XmlElement("aaUin")]
            public string aaUin { get; set; }
            [XmlElement("accUin")]
            public string accUin { get; set; }
            [XmlElement("balance")]
            public string balance { get; set; }
            [XmlElement("create_time")]
            public string Fcreate_time { get; set; }
            [XmlElement("reason")]
            public string Freason { get; set; }
            [XmlElement("total_paid_num")]
            public string Ftotal_paid_num { get; set; }
            [XmlElement("plan_paid_num")]
            public string Fplan_paid_num { get; set; }
            [XmlElement("total_paid_amount_text")]
            public string Ftotal_paid_amount_text { get; set; }
            [XmlElement("state")]
            public string Flstate { get;set; }
            [XmlElement("status_text")]
            public string Fstatus_text { get; set; }
            [XmlElement("aa_collection_no")]
            public string Faa_collection_no { get; set; }
        }

        public class WechatAADetail
        {
            [XmlElement("memo")]
            public string Fmemo { get; set; }
            [XmlElement("num_text")]
            public string Fnum_text { get; set; }
            [XmlElement("pay_nickname")]
            public string Fpay_nickname { get; set; }
            [XmlElement("receive_name")]
            public string receive_name { get; set; }
            [XmlElement("pay_openid")]
            public string Fpay_openid { get; set; }
            [XmlElement("pay_aaopenid")]
            public string Fpay_aaopenid { get; set; }
            [XmlElement("receive_openid")]
            public string Freceive_openid { get; set; }
            [XmlElement("receive_aaopenid")]
            public string receive_aaopenid { get; set; }
            [XmlElement("state")]
            public string Fstate { get; set; }
            [XmlElement("state_text")]
            public string Fstate_text { get; set; }
            [XmlElement("pay_memo")]
            public string Fpay_memo { get; set; }
        }

    }
}