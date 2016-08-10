using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
namespace CFT.CSOMS.Service.CSAPI.BaseInfo
{
    public class RealNameCertification
    {
        public RealNameCertification()
        {

        }
        #region 银行认证渠道信息
        public class BandMaskInfo
        {
            //四位卡尾号
            [XmlElement("card_tail")]
            public string card_tail { get; set; }
            //手机号
            [XmlElement("mobile")]
            public string mobile { get; set; }
            //认证时间
            [XmlElement("authen_time")]
            public string authen_time { get; set; }
            //银行名称
            [XmlElement("bank_name")]
            public string bank_name { get; set; }
            //证件类型
            [XmlElement("cre_type")]
            public string cre_type { get; set; }
            //证件号掩码
            [XmlElement("cre_id_mask")]
            public string cre_id_mask { get; set; }
            //姓名掩码
            [XmlElement("name_mask")]
            public string name_mask { get; set; }

        }
        #endregion
        #region 认证渠道查询信息（请求详细信息）
        public class AuthStatusInfoDetail
        {
            //账户类型
            [XmlElement("authen_account_type")]
            public string authen_account_type { get; set; }
            //认证状态集
            [XmlElement("authen_channel_state")]
            public string authen_channel_state { get; set; }
            //认证渠道个数
            [XmlElement("authen_channel_sum")]
            public string authen_channel_sum { get; set; }
            //记录当前版本号
            [XmlElement("modify_ver")]
            public string modify_ver { get; set; }
            //0:未通过了公安部认证1:通过了公安部认证
            [XmlElement("name_auth_status")]
            public string name_auth_status { get; set; }
            //0:未通过了公安部认证1:通过了公安部认证
            [XmlElement("pass_gov_auth")]
            public string pass_gov_auth { get; set; }
            //0:未通过了绑卡实名认证1:通过了绑卡实名认证
            [XmlElement("pass_bank_auth")]
            public string pass_bank_auth { get; set; }
            //0:未通过了影印件认证1:通过了影印件认证
            [XmlElement("pass_ocr_auth")]
            public string pass_ocr_auth { get; set; }
            //白名单过期日期
            [XmlElement("white_list_expire")]
            public string white_list_expire { get; set; }
            //公安部认证渠道详细信息
            [XmlElement("gov_authen_info")]
            public string gov_authen_info { get; set; }
            //影印件认证详细信息
            [XmlElement("ocr_authen_info")]
            public string ocr_authen_info { get; set; }
            //公安部认证失败字段1. 证件号码合法2. 库中无此号3. 证件信息不一致4. 重号不一致5. 证件号码不规则
            [XmlElement("gov_auth_fail_reason")]
            public string gov_auth_fail_reason { get; set; }
            //渠道中的银行字符编码列表
            [XmlElement("bind_bank")]
            public string bind_bank { get; set; }
            //运营商认证详细信息
            [XmlElement("mobile_authen_info")]
            public string mobile_authen_info { get; set; }
            //学历认证详细信息
            [XmlElement("edu_authen_info")]
            public string edu_authen_info { get; set; }

        }
        #endregion
        #region 认证渠道查询信息（不请求详细信息）
        public class AuthStatusInfo
        {
            //账户类型
            [XmlElement("authen_account_type")]
            public string authen_account_type { get; set; }
            //认证状态集
            [XmlElement("authen_channel_state")]
            public string authen_channel_state { get; set; }
            //认证渠道个数
            [XmlElement("authen_channel_sum")]
            public string authen_channel_sum { get; set; }
            //记录当前版本号
            [XmlElement("modify_ver")]
            public string modify_ver { get; set; }
            //0:未通过了公安部认证1:通过了公安部认证
            [XmlElement("name_auth_status")]
            public string name_auth_status { get; set; }
            //0:未通过了公安部认证1:通过了公安部认证
            [XmlElement("pass_gov_auth")]
            public string pass_gov_auth { get; set; }
            //0:未通过了绑卡实名认证1:通过了绑卡实名认证
            [XmlElement("pass_bank_auth")]
            public string pass_bank_auth { get; set; }
            //0:未通过了影印件认证1:通过了影印件认证
            [XmlElement("pass_ocr_auth")]
            public string pass_ocr_auth { get; set; }
            //白名单过期日期
            [XmlElement("white_list_expire")]
            public string white_list_expire { get; set; }
        }
        #endregion
        #region 账户信息查询
        public class UserInfoByUinAndQueryUser
        {
            //账户uid
            [XmlElement("uid")]
            public string uid { get; set; }
            //1：正常；2.作废(报错，不会返回)
            [XmlElement("flag")]
            public string flag { get; set; }
            //用户所在核心set
            [XmlElement("acc_set")]
            public string acc_set { get; set; }
            //用户登录的类型QQ、Email、手机
            [XmlElement("login_type")]
            public string login_type { get; set; }
            //用户类型1个人，2公司
            [XmlElement("user_type")]
            public string user_type { get; set; }
            //帐户状态1个人，2公司
            [XmlElement("state")]
            public string state { get; set; }
            //用户名称
            [XmlElement("user_true_name")]
            public string user_true_name { get; set; }
            //公司名称
            [XmlElement("com_name")]
            public string com_name { get; set; }
            //证件类型 1身份证，2学生证，3工作证
            [XmlElement("cre_type")]
            public string cre_type { get; set; }
            //3des加密后证件号
            [XmlElement("cre_id")]
            public string cre_id { get; set; }
            //注册类型 0 完整注册 1简化注册
            [XmlElement("reg_type")]
            public string reg_type { get; set; }
            //注册渠道
            [XmlElement("reg_channel")]
            public string reg_channel { get; set; }
            //特殊密码标识 默认0， 微信6位数字密码为2
            [XmlElement("pass_flag")]
            public string pass_flag { get; set; }
            //开户日期
            [XmlElement("create_time")]
            public string create_time { get; set; }
            //最后修改时间
            [XmlElement("modify_time")]
            public string modify_time { get; set; }
            //帐号认证类型
            [XmlElement("authen_account_type")]
            public string authen_account_type { get; set; }
            //客户余额连续10天超过5000标记
            [XmlElement("ban_static_state")]
            public string ban_static_state { get; set; }
            //认证渠道状态集 最低第2位bit为1表示实名路径有影印件
            [XmlElement("authen_channel_state")]
            public string authen_channel_state { get; set; }
            //账户
            [XmlElement("uin")]
            public string uin { get; set; }
        }
        public class UserInfoByUinAndQueryUserAtt
        {
            //账户uid
            [XmlElement("uid")]
            public string uid { get; set; }
            //1：正常；2.作废(报错，不会返回)
            [XmlElement("flag")]
            public string flag { get; set; }
            //用户所在核心set
            [XmlElement("acc_set")]
            public string acc_set { get; set; }
            //用户登录的类型QQ、Email、手机
            [XmlElement("login_type")]
            public string login_type { get; set; }
            //产品属性ID
            [XmlElement("attid")]
            public string attid { get; set; }
        }
        public class UserInfoByUinAndUserAttach
        {
            //账户uid
            [XmlElement("uid")]
            public string uid { get; set; }
            //1：正常；2.作废(报错，不会返回)
            [XmlElement("flag")]
            public string flag { get; set; }
            //用户所在核心set
            [XmlElement("acc_set")]
            public string acc_set { get; set; }
            //用户登录的类型QQ、Email、手机
            [XmlElement("login_type")]
            public string login_type { get; set; }
            //用户类型1个人，2公司
            [XmlElement("user_type")]
            public string user_type { get; set; }
            //帐户状态1个人，2公司
            [XmlElement("state")]
            public string state { get; set; }
            //用户名称
            [XmlElement("user_true_name")]
            public string user_true_name { get; set; }
            //公司名称
            [XmlElement("com_name")]
            public string com_name { get; set; }
            //证件类型 1身份证，2学生证，3工作证
            [XmlElement("cre_type")]
            public string cre_type { get; set; }
            //3des加密后证件号
            [XmlElement("cre_id")]
            public string cre_id { get; set; }
            //注册类型 0 完整注册 1简化注册
            [XmlElement("reg_type")]
            public string reg_type { get; set; }
            //注册渠道
            [XmlElement("reg_channel")]
            public string reg_channel { get; set; }
            //特殊密码标识 默认0， 微信6位数字密码为2
            [XmlElement("pass_flag")]
            public string pass_flag { get; set; }
            //开户日期
            [XmlElement("create_time")]
            public string create_time { get; set; }
            //最后修改时间
            [XmlElement("modify_time")]
            public string modify_time { get; set; }
            //帐号认证类型
            [XmlElement("authen_account_type")]
            public string authen_account_type { get; set; }
            //客户余额连续10天超过5000标记
            [XmlElement("ban_static_state")]
            public string ban_static_state { get; set; }
            //认证渠道状态集 最低第2位bit为1表示实名路径有影印件
            [XmlElement("authen_channel_state")]
            public string authen_channel_state { get; set; }
            //账户
            [XmlElement("uin")]
            public string uin { get; set; }
            //产品属性ID
            [XmlElement("attid")]
            public string attid { get; set; }
        }
        public class UserInfoByUidAndQueryUser
        {

            //用户类型1个人，2公司
            [XmlElement("user_type")]
            public string user_type { get; set; }
            //帐户状态1个人，2公司
            [XmlElement("state")]
            public string state { get; set; }
            //用户名称
            [XmlElement("user_true_name")]
            public string user_true_name { get; set; }
            //公司名称
            [XmlElement("com_name")]
            public string com_name { get; set; }
            //证件类型 1身份证，2学生证，3工作证
            [XmlElement("cre_type")]
            public string cre_type { get; set; }
            //3des加密后证件号
            [XmlElement("cre_id")]
            public string cre_id { get; set; }
            //注册类型 0 完整注册 1简化注册
            [XmlElement("reg_type")]
            public string reg_type { get; set; }
            //注册渠道
            [XmlElement("reg_channel")]
            public string reg_channel { get; set; }
            //特殊密码标识 默认0， 微信6位数字密码为2
            [XmlElement("pass_flag")]
            public string pass_flag { get; set; }
            //开户日期
            [XmlElement("create_time")]
            public string create_time { get; set; }
            //最后修改时间
            [XmlElement("modify_time")]
            public string modify_time { get; set; }
            //帐号认证类型
            [XmlElement("authen_account_type")]
            public string authen_account_type { get; set; }
            //客户余额连续10天超过5000标记
            [XmlElement("ban_static_state")]
            public string ban_static_state { get; set; }
            //认证渠道状态集 最低第2位bit为1表示实名路径有影印件
            [XmlElement("authen_channel_state")]
            public string authen_channel_state { get; set; }
            //账户
            [XmlElement("uin")]
            public string uin { get; set; }

        }
        public class UserInfoByUidAndQueryUserAtt
        {
            //产品属性ID
            [XmlElement("attid")]
            public string attid { get; set; }
        }
        public class UserInfoByUidAndUserAttach
        {
            //用户类型1个人，2公司
            [XmlElement("user_type")]
            public string user_type { get; set; }
            //帐户状态1个人，2公司
            [XmlElement("state")]
            public string state { get; set; }
            //用户名称
            [XmlElement("user_true_name")]
            public string user_true_name { get; set; }
            //公司名称
            [XmlElement("com_name")]
            public string com_name { get; set; }
            //证件类型 1身份证，2学生证，3工作证
            [XmlElement("cre_type")]
            public string cre_type { get; set; }
            //3des加密后证件号
            [XmlElement("cre_id")]
            public string cre_id { get; set; }
            //注册类型 0 完整注册 1简化注册
            [XmlElement("reg_type")]
            public string reg_type { get; set; }
            //注册渠道
            [XmlElement("reg_channel")]
            public string reg_channel { get; set; }
            //特殊密码标识 默认0， 微信6位数字密码为2
            [XmlElement("pass_flag")]
            public string pass_flag { get; set; }
            //开户日期
            [XmlElement("create_time")]
            public string create_time { get; set; }
            //最后修改时间
            [XmlElement("modify_time")]
            public string modify_time { get; set; }
            //帐号认证类型
            [XmlElement("authen_account_type")]
            public string authen_account_type { get; set; }
            //客户余额连续10天超过5000标记
            [XmlElement("ban_static_state")]
            public string ban_static_state { get; set; }
            //认证渠道状态集 最低第2位bit为1表示实名路径有影印件
            [XmlElement("authen_channel_state")]
            public string authen_channel_state { get; set; }
            //账户
            [XmlElement("uin")]
            public string uin { get; set; }
            //产品属性ID
            [XmlElement("attid")]
            public string attid { get; set; }
        }
        #endregion

        #region 证件下账户列表查询接口
        public class PQueryCreRelation
        {
            //账户数
            [XmlElement("uid_num")]
            public string uid_num { get; set; }
            //账户列表
            [XmlElement("uid_list")]
            public string uid_list { get; set; }
            //证件号余额连续10天超过5000标记
            [XmlElement("ban_static_state")]
            public string ban_static_state { get; set; }
        }
        #endregion


        #region 账户限额数据查询
        public class QuotaBanQueryC
        {
            //账户日支出金额（单位分，下同）
            [XmlElement("day_out_amount")]
            public string day_out_amount { get; set; }
            //账户日支付次数
            [XmlElement("day_out_count")]
            public string day_out_count { get; set; }
            //账户月支出金额
            [XmlElement("month_out_amount")]
            public string month_out_amount { get; set; }
            //账户月进出金额
            [XmlElement("month_outin_amount")]
            public string month_outin_amount { get; set; }
            //账户年支出金额
            [XmlElement("year_out_amount")]
            public string year_out_amount { get; set; }
            //账户终身支出金额
            [XmlElement("total_out_amount")]
            public string total_out_amount { get; set; }
            //对应证件号月进出金额
            [XmlElement("cre_month_outin_amount")]
            public string cre_month_outin_amount { get; set; }
            //对应证件号年支出金额
            [XmlElement("cre_year_out_amount")]
            public string cre_year_out_amount { get; set; }
            //账户终身剩余额度
            [XmlElement("rest_total_out_amount")]
            public string rest_total_out_amount { get; set; }
            //账户剩余月进出额度
            [XmlElement("rest_month_outin_amount")]
            public string rest_month_outin_amount { get; set; }
            //账户剩余年支付额度
            [XmlElement("rest_year_out_amount")]
            public string rest_year_out_amount { get; set; }
            //对应证件号微信账户体系年支出金额
            [XmlElement("cre_wx_year_out_amount")]
            public string cre_wx_year_out_amount { get; set; }
            //对应证件号qq账户体系年支出金额
            [XmlElement("cre_sqpc_year_out_amount")]
            public string cre_sqpc_year_out_amount { get; set; }
            //对应证件号日支付金额
            [XmlElement("cre_day_out_amount")]
            public string cre_day_out_amount { get; set; }
        #endregion

        }
    }
}