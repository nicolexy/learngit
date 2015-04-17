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

        #region 特殊申诉

        public class AppealLog
        {
            [XmlElement("diary_id")]
            public string FID { get; set; }
            [XmlElement("handle_result")]
            public string handleResult { get; set; }
        }

        public class SpecialAppealList
        {
            [XmlElement("fid")]
            public string Fid { get; set; }
            [XmlElement("uin")]
            public string Fuin { get; set; }
            [XmlElement("submit_time")]
            public string FSubmitTime { get; set; }
            [XmlElement("order_state")]
            public string FState { get; set; }
            [XmlElement("order_state_str")]
            public string handleStateName { get; set; }
            [XmlElement("handler")]
            public string FCheckUser { get; set; }
        }

        public class SpecialAppealDetail
        {
            [XmlElement("uin")]
            public string FUin { get; set; }
            [XmlElement("email")]
            public string FEmail { get; set; }
            [XmlElement("sub_creid")]
            public string FCreId { get; set; }
            [XmlElement("phone_no")]
            public string FReservedMobile { get; set; }
            [XmlElement("freeze_reason")]
            public string FreezeReason { get; set; }
            [XmlElement("sub_username")]
            public string FOldName { get; set; }
            [XmlElement("standard_score")]
            public string FStandardScore { get; set; }
            [XmlElement("risk_result")]
            public string risk_result { get; set; }
            [XmlElement("clear_pps")]
            public string ClearPPS { get; set; }
            [XmlElement("appeal_score")]
            public string FAppealScore { get; set; }
            [XmlElement("detail_score")]
            public string detail_score { get; set; }
            [XmlElement("appeal_reason")]
            public string FAppealReason { get; set; }

            [XmlElement("cre_image1")]//身份证正面
            public string FCreImg1Str { get; set; }
            [XmlElement("cre_image2")]//身份证反面
            public string FCreImg2Str { get; set; }
            [XmlElement("bank_image")]//银行卡照片
            public string FOtherImage1Str { get; set; }
            [XmlElement("balance_image")]//资金来源截图
            public string FProveBanlanceImageStr { get; set; }
            [XmlElement("other_image1")]//补充其他证件照片
            public string FOtherImage2Str { get; set; }
            [XmlElement("other_image2")]//补充的手持身份证半身照
            public string FOtherImage3Str { get; set; }
            [XmlElement("other_image3")]//补充户籍证明照片
            public string FOtherImage4Str { get; set; }
            [XmlElement("other_image4")]//补充资料截图
            public string FOtherImage5Str { get; set; }

            [XmlElement("zdy_title1")]//自定义标题1
            public string Fsup_desc1Str { get; set; }
            [XmlElement("zdy_title2")]
            public string Fsup_desc2Str { get; set; }
            [XmlElement("zdy_title3")]
            public string Fsup_desc3Str { get; set; }
            [XmlElement("zdy_title4")]
            public string Fsup_desc4Str { get; set; }
            [XmlElement("zdy_info1")]//自定义内容1
            public string Fsup_tips1Str { get; set; }
            [XmlElement("zdy_info2")]
            public string Fsup_tips2Str { get; set; }
            [XmlElement("zdy_info3")]
            public string Fsup_tips3Str { get; set; }
            [XmlElement("zdy_info4")]
            public string Fsup_tips4Str { get; set; }
        }
        #endregion

        #region 证件号码清理

        public class CreidInfoBasic
        {
            [XmlElement("creid")]
            public string FCreid { get; set; }
            [XmlElement("create_time")]
            public string FCreate_time { get; set; }
            [XmlElement("user_type")]
            public string FUser_type { get; set; }
            [XmlElement("oper")]
            public string FUid { get; set; }
        }
        #endregion

        public class UserControledFund
        {
            [XmlElement("cur_typeName")]
            public string Fcur_typeName { get; set; }
            [XmlElement("balance")]
            public string balance { get; set; }
            [XmlElement("stateName")]
            public string FstateName { get; set; }
            [XmlElement("create_time")]
            public string Fcreate_time { get; set; }
            [XmlElement("typeText")]
            public string FtypeText { get; set; }
            [XmlElement("cur_type")]
            public string cur_type { get; set; }
            [XmlElement("modify_time")]
            public string Fmodify_time { get; set; }
        }
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