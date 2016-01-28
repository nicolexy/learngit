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

        #region 受控资金

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

        public class CtrlFundLog
        {
            [XmlElement("uin")]
            public string Fuin { get; set; }
            [XmlElement("balance")]
            public string FbalanceStr { get; set; }
            [XmlElement("typeText")]
            public string FtypeText { get; set; }
            [XmlElement("cur_Type")]
            public string FcurType { get; set; }
            [XmlElement("modify_time")]
            public string FmodifyTime { get; set; }
            [XmlElement("operator")]
            public string FupdateUser { get; set; }
        }

        #endregion

        #region 个人账户信息

        public class PersonalInfo
        {
            [XmlElement("att_id")]
            public string Att_id { get; set; }
            [XmlElement("pro_att")]
            public string Fpro_att { get; set; }   
            [XmlElement("aa_uin")]          //微信AA财付通帐号
            public string Faa_uin { get; set; }
            [XmlElement("acc_uin")]         //微信支付财付通帐号
            public string Facc_uin { get; set; }
            [XmlElement("qqid")]
            public string Fqqid { get; set; }
            [XmlElement("qqid_state")]
            public string Fqqid_state { get; set; }
            [XmlElement("email")]
            public string Femail { get; set; }
            [XmlElement("emial_state")]
            public string Femial_state { get; set; }
            [XmlElement("mobile")]
            public string Fmobile { get; set; }
            [XmlElement("mobile_state")]
            public string Fmobile_state { get; set; }

            [XmlElement("state_str")]
            public string Fstate_str { get; set; }
            [XmlElement("useable_fee")]
            public string Fuseable_fee { get; set; }
            [XmlElement("yday_balance")]   //昨日余额
            public string Fyday_balance { get; set; }
            [XmlElement("curtype")]
            public string Fcurtype { get; set; }
            [XmlElement("curtype_str")]
            public string Fcurtype_str { get; set; }
            [XmlElement("day_pay")]
            public string Fapay { get; set; }
            [XmlElement("quota_pay")]
            public string Fquota_pay_str { get; set; }
            [XmlElement("save_str")]
            public string Fsave_str { get; set; }

            [XmlElement("fetch_time")]  //最近提款日期
            public string Ffetch_time { get; set; }
            [XmlElement("modify_time")]
            public string Fmodify_time { get; set; }
            [XmlElement("name_str")]    //真实姓名
            public string Fname_str { get; set; }
            [XmlElement("uid")]
            public string Fuid { get; set; }
            [XmlElement("bpay_state")]
            public string Fbpay_state { get; set; }
            [XmlElement("bpay_state_str")]
            public string Fbpay_state_str { get; set; }
            [XmlElement("user_type")]
            public string Fuser_type { get; set; }
            [XmlElement("user_type_str")]
            public string Fuser_type_str { get; set; }

            [XmlElement("freeze_fee")]
            public string Ffreeze_fee { get; set; }
            [XmlElement("balance")]
            public string Fbalance_str { get; set; }
            [XmlElement("create_time")] //注册时间
            public string Fcreate_time { get; set; }
            [XmlElement("quota")]   //单笔交易限额
            public string Fquota_str { get; set; }
            [XmlElement("fetch")]   //当日提现金额
            public string Ffetch_str { get; set; }
            [XmlElement("save_time")]   //最近存款日期
            public string Fsave_time { get; set; }
            [XmlElement("login_ip")]
            public string Flogin_ip { get; set; }          
        }

        public class VIPInfo
        {
            [XmlElement("value")]
            public string value { get; set; }
            [XmlElement("vipflag_str")]
            public string vipflag_str { get; set; }
            [XmlElement("level")]
            public string level { get; set; }
            [XmlElement("subid_str")]
            public string subid_str { get; set; }
            [XmlElement("exp_date")]
            public string exp_date { get; set; }
        }

        public class DeleteCertLog
        {
            [XmlElement("qqid")]
            public string Fqqid { get; set; }
            [XmlElement("memo")]
            public string Fmemo { get; set; }
            [XmlElement("authen_operator")]
            public string Fauthen_operator { get; set; }
            [XmlElement("modify_time")]
            public string Fmodify_time { get; set; }
        }

        public class GetUserInfo
        {
            [XmlElement("qqid")]
            public string fqqid { get; set; }
            [XmlElement("truename")]
            public string ftruename { get; set; }
            [XmlElement("att_id")]  
            public string Fatt_id { get; set; }
            [XmlElement("att_id_str")]
            public string Fatt_id_str { get; set; }
            [XmlElement("sex")] 
            public string fsex { get; set; }
            [XmlElement("sex_str")]
            public string Fsex_str { get; set; }
            [XmlElement("company_name")]
            public string fcompany_name { get; set; }
            [XmlElement("age")]
            public string fage { get; set; }
            [XmlElement("phone")]
            public string fphone { get; set; }
            [XmlElement("mobile")]
            public string fmobile { get; set; }
            [XmlElement("email")]
            public string femail { get; set; }

            [XmlElement("address")]
            public string faddress { get; set; }
            [XmlElement("pcode")]
            public string fpcode { get; set; }
            [XmlElement("cre_type")]    
            public string fcre_type { get; set; }
            [XmlElement("cre_type_str")]
            public string Fcre_type_str { get; set; }
            [XmlElement("creid")]
            public string fcreid { get; set; }
            [XmlElement("memo")]
            public string fmemo { get; set; }
            [XmlElement("modify_time")]
            public string fmodify_time { get; set; }
            [XmlElement("area")]
            public string farea { get; set; }
            [XmlElement("city")]
            public string fcity { get; set; }
            [XmlElement("userType")]
            public string userType { get; set; }
            [XmlElement("userType_str")]
            public string userType_str { get; set; }
        }

        public class UserInfoLog
        {
            [XmlElement("cre_type")]
            public string Fcre_type { get; set; }
            [XmlElement("cre_type_str")]
            public string Fcre_type_str { get; set; }
            [XmlElement("cre_type_old")]
            public string Fcre_type_old { get; set; }
            [XmlElement("cre_type_old_str")]
            public string Fcre_type_old_str { get; set; }
            [XmlElement("user_type")]
            public string Fuser_type { get; set; }
            [XmlElement("user_type_str")]
            public string Fuser_type_str { get; set; }

            [XmlElement("user_type_old")]
            public string Fuser_type_old { get; set; }
            [XmlElement("user_type_old_str")]
            public string Fuser_type_old_str { get; set; }
            [XmlElement("attid")]
            public string Fattid { get; set; }
            [XmlElement("attid_str")]
            public string Fattid_str { get; set; }
            [XmlElement("attid_old")]
            public string Fattid_old { get; set; }
            [XmlElement("attid_old_str")]
            public string Fattid_old_str { get; set; }

            [XmlElement("qqid")]
            public string Fqqid { get; set; }
            [XmlElement("submit_user")]
            public string Fsubmit_user { get; set; }
            [XmlElement("submit_time")]
            public string Fsubmit_time { get; set; }
            [XmlElement("commet")]
            public string Fcommet { get; set; }
            [XmlElement("commet_old")]
            public string Fcommet_old { get; set; }
        }

        #endregion

        #region 销户
        public class CancelAccountLog
        {
            [XmlElement("id")]
            public string Fid { get; set; }
            [XmlElement("qqid")]
            public string Fqqid { get; set; }
            [XmlElement("uid")]
            public string Fquid { get; set; }
            [XmlElement("reason")]
            public string Freason { get; set; }
            [XmlElement("opera")]
            public string handid { get; set; }
            [XmlElement("ip")]
            public string handip { get; set; }
            [XmlElement("modify_time")]
            public string FlastModifyTime { get; set; }
        }
        #endregion

        #region 修改QQ账号

        public class ChangeQQList
        {
            [XmlElement("old_qqid")]
            public string FOldQQ { get; set; }
            [XmlElement("new_qqid")]
            public string FNewQQ { get; set; }
            [XmlElement("operator")]
            public string FUserID { get; set; }
            [XmlElement("action_time")]
            public string FActionTime { get; set; }
            [XmlElement("memo")]
            public string FMemo { get; set; }
        }

        #endregion

        #region 证书通知黑名单
        public class CertNoticeBlackList
        {
            [XmlElement("spid")]
            public string Fspid { get; set; }
            [XmlElement("companyName")]
            public string FcompanyName { get; set; }
            [XmlElement("memo")]
            public string Fmemo { get; set; }
        }
        #endregion

        #region 手机绑定查询

        public class MobileBindInfo
        {
            [XmlElement("uid")]
            public string Fuid { get; set; }
            [XmlElement("qqid")]
            public string Fqqid { get; set; }
            [XmlElement("email")]
            public string Femail { get; set; }
            [XmlElement("mobile")]
            public string Fmobile { get; set; }
            [XmlElement("mobile_state")]
            public string MobileState { get; set; }
            [XmlElement("msg_state")]
            public string MsgState { get; set; }
            [XmlElement("mobile_pay_state")]
            public string MobilePayState { get; set; }
            [XmlElement("unbind")]
            public string Unbind { get; set; }
        }
        #endregion

        #region 个人证书管理

        public class UserCrtList
        {
            [XmlElement("scene")]
            public string Fscene { get; set; }
            [XmlElement("state_name")]
            public string FstateName { get; set; }
            [XmlElement("lstate_name")]
            public string FlstateName { get; set; }
            [XmlElement("validity_begin")]
            public string Fvalidity_begin { get; set; }
            [XmlElement("validity_end")]
            public string Fvalidity_end { get; set; }
            [XmlElement("ip")]
            public string Fip { get; set; }
            [XmlElement("create_time")]
            public string Fcreate_time { get; set; }
            [XmlElement("modify_time")]
            public string Fmodify_time { get; set; }
            [XmlElement("delete_time")]
            public string Fmodify_time2 { get; set; }
        }

        public class DeleteCrtInfo
        {
            [XmlElement("modify_time")]
            public string Fmodify_time { get; set; }
            [XmlElement("ip")]
            public string DeleteIP { get; set; }
        }

        #endregion

        #region 实名认证一块的需求

        public class FreeFlowInfo
        {
            [XmlElement("isVip")]
            public string isVip { get; set; }
            [XmlElement("vip_exp_date")]
            public string vip_exp_date { get; set; }
            [XmlElement("authenState")]
            public string authenState { get; set; }
            [XmlElement("freeFlow")]
            public string freeFlow { get; set; }
            [XmlElement("isBig")]
            public string isBig { get; set; }
            [XmlElement("isTX")]
            public string isTX { get; set; }
        }

        public class AuthenDealList
        {
            [XmlElement("result")]
            public string Result { get; set; }
            [XmlElement("pickuser")]//处理人
            public string Fpickuser { get;set; }
            [XmlElement("picktime")]
            public string Fpicktime { get; set; }
            [XmlElement("memo")]
            public string Fmemo { get; set; }
        }

        /// <summary>
        /// 付款延迟异常数据
        /// </summary>
        public class PaymentAbnormal
        {
            /// <summary>
            /// 批次号
            /// </summary>
            [XmlElement("FBatchID")]
            public string FBatchID { get; set; }
            /// <summary>
            /// 业务单号
            /// </summary>
            [XmlElement("Flistid")]
            public string Flistid { get; set; }
            ///// <summary>
            ///// 业务类型
            ///// </summary>
            //[XmlElement("Ftype_str")]
            //public string Ftype_str { get; set; }
            ///// <summary>
            ///// 子业务类型
            ///// </summary>
            //[XmlElement("subType_str")]
            //public string subType_str { get; set; }
            ///// <summary>
            ///// 银行类型
            ///// </summary>
            //[XmlElement("Fbank_type_str")]
            //public string Fbank_type_str { get; set; }
            ///// <summary>
            ///// 错误类型
            ///// </summary>
            //[XmlElement("Ferror_type_str")]
            //public string Ferror_type_str { get; set; }
            ///// <summary>
            ///// 最新通知状态
            ///// </summary>
            //[XmlElement("Fnotify_status_str")]
            //public string Fnotify_status_str { get; set; }
            ///// <summary>
            ///// 异常时间
            ///// </summary>
            //[XmlElement("Fabnormal_time")]
            //public string Fabnormal_time { get; set; }
        }

        public class FreezeThaw
        {
            [XmlElement("flag")]
            public int flag { get; set; }
            [XmlElement("info")]
            public string info { get; set; }

        }

        #endregion
    }

    public class FreezeInfo
    {
        public FreezeInfo()
        { 
        
        }

        public class UserFreezeRecord
        {
            [XmlElement("uin")]
            public string Fqqid { get; set; }
            [XmlElement("freeze_reason")]
            public string strFreason { get; set; }
            [XmlElement("type")]
            public string strType { get; set; }
            [XmlElement("sign_name")]
            public string strSignName { get; set; }
            [XmlElement("connum")]
            public string Fconnum { get; set; }
            [XmlElement("modify_time")]
            public string Fmodify_time { get; set; }
            [XmlElement("list_id")]
            public string Flistid { get; set; }
            [XmlElement("bkid")]
            public string Fbkid { get; set; }
            [XmlElement("true_name")]
            public string Ftrue_name { get; set; }
            [XmlElement("paynum")]
            public string Fpaynum { get; set; }
            [XmlElement("balance")]
            public string Fbalance { get; set; }
            [XmlElement("con")]
            public string Fcon { get; set; }
            [XmlElement("curtype")]
            public string strFcurtype { get; set; }
            [XmlElement("bank_name")]
            public string strBankName { get; set; }
            [XmlElement("memo")]
            public string Fmemo { get; set; }
            [XmlElement("apply_id")]
            public string Fapplyid { get; set; }
        }

        public class FreezeList
        {
            [XmlElement("username")]
            public string FUserName { get; set; }
            [XmlElement("freeze_type")]
            public string FFreezeTypeName { get; set; }
            [XmlElement("handle_name")]
            public string FHandleFinishName { get; set; }
            [XmlElement("freeze_userID")]
            public string FFreezeUserID { get; set; }
            [XmlElement("freeze_time")]
            public string FFreezeTime { get; set; }
            [XmlElement("id")]
            public string FID { get; set; }
        }

        public class FreezeListDetail
        {
            [XmlElement("username")]
            public string FUserName { get; set; }
            [XmlElement("contact")]
            public string FContact { get; set; }
            [XmlElement("freeze_type")]
            public string strFFreezeType { get; set; }
            [XmlElement("freezeID")]
            public string FFreezeID { get; set; }
            [XmlElement("freeze_userID")]
            public string FFreezeUserID { get; set; }
            [XmlElement("freeze_time")]
            public string FFreezeTime { get; set; }
            [XmlElement("freeze_reason")]
            public string FFreezeReason { get; set; }
            [XmlElement("handle_userID")]
            public string FHandleUserID { get; set; }
            [XmlElement("handle_time")]
            public string FHandleTime { get; set; }
            [XmlElement("handle_result")]
            public string FHandleResult { get; set; }
            [XmlElement("freeze_channel")]
            public string strFfreeze_channel { get; set; }
        }
    }
}