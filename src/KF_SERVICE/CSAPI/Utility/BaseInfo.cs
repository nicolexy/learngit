using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace CFT.CSOMS.Service.CSAPI.BaseInfo
{
    public class BaseInfoC
    {
        public BaseInfoC()
        {

        }

        #region 自助申诉

        public class UserInfoBasic
        {
            [XmlElement("balance_str")]
            public string FBalanceStr { get; set; }
            [XmlElement("con_str")]
            public string FconStr { get; set; }
            [XmlElement("true_name")]
            public string Ftruename { get; set; }
            [XmlElement("company_name")]
            public string Fcompany_name { get; set; }
            [XmlElement("qqid")]
            public string Fqqid { get; set; }
            [XmlElement("email")]
            public string Femail { get; set; }
            [XmlElement("cre_type_str")]
            public string Fcre_type_str { get; set; }
            [XmlElement("creid")]
            public string Fcreid { get; set; }
            [XmlElement("bankid")]
            public string Fbankid { get; set; }
        }

        public class UserAppealList
        {
            [XmlElement("uin")]
            public string Fuin { get; set; }
            [XmlElement("email")]
            public string Femail { get; set; }
            [XmlElement("type_name")]
            public string FTypeName { get; set; }
            [XmlElement("state_name")]
            public string FStateName { get; set; }
            [XmlElement("submit_time")]
            public string FSubmitTime { get; set; }
            [XmlElement("pick_time")]
            public string Fpicktime { get; set; }
            [XmlElement("check_info")]
            public string FCheckInfo { get; set; }
            [XmlElement("uin_color")]
            public string Fuincolor { get; set; }
            [XmlElement("db")]
            public string DBName { get; set; }
            [XmlElement("tb")]
            public string tableName { get; set; }
            [XmlElement("fid")]
            public string Fid { get; set; }
            [XmlElement("balance")]
            public string balance { get; set; }
        }

        public class UserAppealDetail
        {
            [XmlElement("type_name")]
            public string FTypeName { get; set; }
            [XmlElement("cre_id")]
            public string cre_id { get; set; }
            [XmlElement("new_cre_id")]
            public string new_cre_id { get; set; }
            [XmlElement("cre_type_str")]
            public string cre_type_str { get; set; }
            [XmlElement("email")]
            public string Femail { get; set; }
            [XmlElement("state_name")]
            public string FStateName { get; set; }
            [XmlElement("type")]
            public string FType { get; set; }
            [XmlElement("clear_pps_str")]
            public string clear_pps_str { get; set; }
            [XmlElement("reason")]
            public string reason { get; set; }
            [XmlElement("comment")]
            public string Fcomment { get; set; }
            [XmlElement("old_name")]
            public string old_name { get; set; }
            [XmlElement("new_name")]
            public string new_name { get; set; }
            [XmlElement("check_info")]
            public string FCheckInfo { get; set; }
            [XmlElement("old_company")]
            public string old_company { get; set; }
            [XmlElement("new_company")]
            public string new_company { get; set; }
            [XmlElement("answer")]
            public string labIsAnswer { get; set; }
            [XmlElement("mobile_no")]
            public string mobile_no { get; set; }
            [XmlElement("standard_score")]
            public string standard_score { get; set; }
            [XmlElement("score")]
            public string score { get; set; }
            [XmlElement("detail_score")]
            public string detail_score { get; set; }
            [XmlElement("risk_result")]
            public string risk_result { get; set; }
            [XmlElement("ivr_result")]
            public string FIVRResult { get; set; }
            [XmlElement("image1")]
            public string Image1 { get; set; }
            [XmlElement("image2")]
            public string Image2 { get; set; }
        }
       
        #endregion
    }

    public class TradeInfo
    {
        public TradeInfo()
        {

        }

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
            public string Fstandby8 { get; set; }
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
            public string Fexplain { get; set; }

            [XmlElement("buyid")]
            public string fbuyid { get; set; }
            [XmlElement("trade_type")]
            public string ftrade_type { get; set; }
            [XmlElement("saleidCFT")]
            public string FsaleidCFT { get; set; }
            [XmlElement("trade_stateName")]
            public string Ftrade_stateName { get; set; }         
        }
    }
}