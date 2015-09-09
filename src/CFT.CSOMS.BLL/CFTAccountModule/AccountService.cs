using System;
using CFT.CSOMS.DAL.CFTAccount;
using System.Data;
using CFT.CSOMS.DAL.Infrastructure;
using TENCENT.OSS.C2C.Finance.BankLib;
using System.Collections;
using System.Reflection;
using CFT.CSOMS.COMMLIB;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CFT.Apollo.Logging;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CFT.CSOMS.BLL.TradeModule;
using CFT.CSOMS.BLL.BankCardBindModule;
using CFT.CSOMS.DAL.FundModule;
using CFT.CSOMS.BLL.FundModule;

namespace CFT.CSOMS.BLL.CFTAccountModule
{
    public class AccountService
    {
        public string Uid2QQ(string uin)
        {
            return AccountData.Uid2QQ(uin);
        }

        public string QQ2UidX(string qqid)
        {
            return AccountData.ConvertToFuidX(qqid);
        }

        public string QQ2Uid(string qqid)
        {
            return AccountData.ConvertToFuid(qqid);
        }

        public static string GetQQID(string queryType, string queryString)
        {
            return AccountData.getQQID(queryType, queryString);
        }

        public string SynUserName(string aaUin, string oldName, string newName, string wxUin)
        {
            return new AccountData().SynUserName(aaUin, oldName, newName, wxUin);
        }

        public DataSet GetUserAccount(string u_QQID, int fcurtype, int istr, int imax)
        {
            return new AccountData().GetUserAccount(u_QQID, fcurtype, istr, imax);
        }

        public DataSet GetUserTypeInfo(string cftNo, int product_type, int business_type, int sub_business_type, int cur_type, int userType)
        {
            return new AccountData().GetUserTypeInfo(cftNo, product_type, business_type, sub_business_type, cur_type, userType);
        }

        //查询用户资料表
        public DataSet GetUserInfo(string u_QQID, int istr, int imax)
        {
            DataSet ds = new AccountData().GetUserInfo(u_QQID, istr, imax);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("Fatt_id_str", typeof(String));
                ds.Tables[0].Columns.Add("Fsex_str", typeof(String));
                ds.Tables[0].Columns.Add("Fcre_type_str", typeof(String));
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr["Fsex_str"] = TransferMeaning.Transfer.convertSex(dr["Fsex"].ToString());
                    dr["Fcre_type_str"] = TransferMeaning.Transfer.convertFcre_type(dr["Fcre_type"].ToString());
                    int tmp = int.Parse(dr["Fatt_id"].ToString());
                    dr["Fatt_id_str"] = TransferMeaning.Transfer.convertProAttType(tmp);
                }
            }

            return ds;
        }

        public DataTable QueryChangeUserInfoLog(string qqid, int offset, int limit)
        {
            if (string.IsNullOrEmpty(qqid))
            {
                throw new ArgumentNullException("qqid");
            }
            DataTable dt = new AccountData().QueryChangeUserInfoLog(qqid, offset, limit);

            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Add("Fcre_type_str", typeof(String));
                dt.Columns.Add("Fcre_type_old_str", typeof(String));
                dt.Columns.Add("Fuser_type_str", typeof(String));
                dt.Columns.Add("Fuser_type_old_str", typeof(String));
                dt.Columns.Add("Fattid_str", typeof(String));
                dt.Columns.Add("Fattid_old_str", typeof(String));
                foreach (DataRow dr in dt.Rows)
                {
                    dr["Fcre_type_str"] = TransferMeaning.Transfer.convertFcre_type(dr["Fcre_type"].ToString());
                    dr["Fcre_type_old_str"] = TransferMeaning.Transfer.convertFcre_type(dr["Fcre_type_old"].ToString());
                    dr["Fuser_type_str"] = ConvertToUsertype(dr["Fuser_type"].ToString());
                    dr["Fuser_type_old_str"] = ConvertToUsertype(dr["Fuser_type_old"].ToString());
                    int tmp = int.Parse(dr["Fattid"].ToString());
                    dr["Fattid_str"] = TransferMeaning.Transfer.convertProAttType(tmp);
                    tmp = int.Parse(dr["Fattid_old"].ToString());
                    dr["Fattid_old_str"] = TransferMeaning.Transfer.convertProAttType(tmp);
                }
            }

            return dt;
        }
        //个人信息获取用户类型
        private string ConvertToUsertype(string itype)
        {
            if (itype == "0")
                return "未指定";
            else if (itype == "1")
                return "个人";
            else if (itype == "2")
                return "公司";
            else
                return "";
        }

        public bool GetUserType(string qqid, out string userType, out string userType_str, out string Msg)
        {
            userType_str = "";
            bool ret_value = new AccountData().getUserType(qqid, out userType, out Msg);
            if (ret_value)
            {
                userType_str = ConvertToUsertype((userType));
            }
            return ret_value;
        }

        public bool AddChangeUserInfoLog(string qqid, string cre_type, string cre_type_old, string user_type, string user_type_old, string attid, string attid_old, string commet, string commet_old, string submit_user)
        {
            if (string.IsNullOrEmpty(qqid))
            {
                throw new ArgumentNullException("qqid");
            }

            return new AccountData().AddChangeUserInfoLog(qqid, cre_type, cre_type_old, user_type, user_type_old, attid, attid_old, commet, commet_old, submit_user);
        }

        public Boolean LCTAccStateOperator(string uin, string cre_id, string cre_type, string name, string op_type, string caller_name, string client_ip)
        {
            if (string.IsNullOrEmpty(uin))
            {
                throw new ArgumentNullException("uin");
            }
            if (string.IsNullOrEmpty(cre_id))
            {
                throw new ArgumentNullException("cre_id");
            }
            if (string.IsNullOrEmpty(op_type))
            {
                throw new ArgumentNullException("op_type");
            }

            if (!(op_type == "1" || op_type == "2" || op_type == "3"))
            {
                throw new Exception("理财通账户状态操作类型不正确");
            }

            //查询状态
            Boolean state = new AccountData().LCTAccStateOperator(uin, cre_id, cre_type, name, "3", caller_name, client_ip);

            if (op_type == "3")//查询
                return state;
            else if (op_type == "1")//冻结
            {
                if (state)
                {
                    LogHelper.LogInfo("This account：" + uin + " is already be freezed");
                    return true;
                }
                else
                    return new AccountData().LCTAccStateOperator(uin, cre_id, cre_type, name, "1", caller_name, client_ip);
            }
            else  //(op_type == "2")解冻
            {
                if (!state)
                {
                    LogHelper.LogInfo("This account：" + uin + " is already be Unfreezed");
                    return true;
                }
                else
                    return new AccountData().LCTAccStateOperator(uin, cre_id, cre_type, name, "2", caller_name, client_ip);
            }
        }

        public Boolean LCTAccStateOperator(string uin, string op_type, string caller_name, string client_ip)
        {
            string fuid = AccountData.ConvertToFuid(uin);
            //  string fuid = "540444925";

            if (fuid == null || fuid == "")
            {
                throw new Exception("根据C帐号获取Fuid失败	uin:" + uin);
            }
            if (fuid == null || fuid.Length < 3)
            {
                throw new Exception("内部ID不正确！");
            }

            string errMsg = "";
            string sql = "uid=" + fuid;
            string cre_id = CommQuery.GetOneResultFromICE(sql, CommQuery.QUERY_USERINFO, "Fcreid", out errMsg);
            if (errMsg != "")
                throw new Exception(errMsg);
            string cre_type = CommQuery.GetOneResultFromICE(sql, CommQuery.QUERY_USERINFO, "Fcre_type", out errMsg);
            if (errMsg != "")
                throw new Exception(errMsg);
            return LCTAccStateOperator(uin, cre_id, cre_type, "", op_type, caller_name, client_ip);
        }

        #region 个人账户信息

        /// <summary>
        /// 查询个人信息
        /// </summary>
        /// <param name="qqid"></param>
        /// <param name="type">"Uin"-C账号,"Uid"-内部账号,"WeChatId"-微信帐号,"WeChatQQ"-微信绑定QQ,"WeChatMobile"-微信绑定手机,"WeChatEmail"-微信绑定邮箱,"WeChatUid"-微信内部ID,"WeChatCft"-微信财付通帐号</param>
        /// <returns></returns>
        public DataSet GetPersonalInfo(string qqid, string type)
        {
            try
            {
                string QQID = qqid;
                bool isWechat = false;
                bool cancelSign = false;
                bool isFastPayUser = false;

                if (type == "Uid")
                {
                    QQID = Uid2QQ(qqid);
                }
                DataSet ds = new DataSet();

                if (type == "Uid" && QQID == null)    //查询注销账户信息(可能是注销的账户）
                {
                    cancelSign = true;
                    ds = GetUserAccountCancel(qqid, 1, 1, 1);
                }
                else if (type == "WeChatId" || type == "WeChatQQ" || type == "WeChatMobile" || type == "WeChatEmail" || type == "WeChatUid" || type == "WeChatCft")     //微信用户
                {
                    isWechat = true;
                    QQID = GetQQID(type, qqid);
                    ds = GetUserAccountFromWechat(QQID, 1, 1);

                    if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
                    {
                        if (new AccountData().IsFastPayUser(QQID))
                        {
                            isFastPayUser = true;
                            ds.Tables[0].Rows[0]["Fname_str"] = "快速交易用户";
                            ds.Tables[0].Rows[0]["Fuser_type_str"] = "";
                        }
                        else
                            return null;
                    }
                }
                else
                {
                    if (QQID != null)
                    {
                        isWechat = false;
                        ds = new AccountData().GetUserAccount(QQID, 1, 1, 1);

                        if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
                        {
                            if (new AccountData().IsFastPayUser(QQID))
                            {
                                isFastPayUser = true;
                                ds.Tables[0].Rows[0]["Fname_str"] = "快速交易用户";
                                ds.Tables[0].Rows[0]["Fuser_type_str"] = "";
                            }
                            else
                                return null;
                        }
                    }
                }

                //如果数据不为空且不是快速交易用户
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && !isFastPayUser)
                {
                    ds.Tables[0].Columns.Add("Fcurtype_str", typeof(string));
                    ds.Tables[0].Columns.Add("Fbalance_str", typeof(string));      //单位元
                    ds.Tables[0].Columns.Add("Fquota_str", typeof(string));        //单位元
                    ds.Tables[0].Columns.Add("Fquota_pay_str", typeof(string));    //单位元
                    ds.Tables[0].Columns.Add("Fstate_str", typeof(string));       //账户状态
                    ds.Tables[0].Columns.Add("Fuser_type_str", typeof(string));   //账户类型
                    ds.Tables[0].Columns.Add("Fname_str", typeof(string));        //真实姓名
                    ds.Tables[0].Columns.Add("Ffreeze_fee", typeof(string));      //冻结金额
                    ds.Tables[0].Columns.Add("Fuseable_fee", typeof(string));     //可用余额
                    ds.Tables[0].Columns.Add("Fpro_att", typeof(string));         //产品属性
                    ds.Tables[0].Columns.Add("Ffetch_str", typeof(string));       //当日提现金额
                    ds.Tables[0].Columns.Add("Fsave_str", typeof(string));        //当日已充值金额
                    ds.Tables[0].Columns.Add("Fqqid_state", typeof(string));      //qq关联状态
                    ds.Tables[0].Columns.Add("Femial_state", typeof(string));     //邮箱关联状态
                    ds.Tables[0].Columns.Add("Fmobile_state", typeof(string));    //手机关联状态
                    ds.Tables[0].Columns.Add("Fbpay_state_str", typeof(string));  //余额支付状态

                    #region 转换字段
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr["Fcurtype_str"] = TransferMeaning.Transfer.convertMoney_type(PublicRes.objectToString(ds.Tables[0], "Fcurtype"));   //币种类型转换
                        dr["Fbpay_state_str"] = TransferMeaning.Transfer.convertBPAY(PublicRes.objectToString(ds.Tables[0], "Fbpay_state"));       //余额支付状态
                        dr["Fuser_type_str"] = TransferMeaning.Transfer.convertFuser_type(dr["Fuser_type"].ToString());
                        dr["Femail"] = PublicRes.GetString(PublicRes.objectToString(ds.Tables[0], "Femail"));
                        dr["Fmobile"] = PublicRes.GetString(PublicRes.objectToString(ds.Tables[0], "Fmobile"));
                        MoneyTransfer.FenToYuan_Table(ds.Tables[0], "Fbalance", "Fbalance_str");
                        MoneyTransfer.FenToYuan_Table(ds.Tables[0], "Fquota", "Fquota_str");
                        MoneyTransfer.FenToYuan_Table(ds.Tables[0], "Fquota_pay", "Fquota_pay_str");
                        MoneyTransfer.FenToYuan_Table(ds.Tables[0], "Ffetch", "Ffetch_str");
                        MoneyTransfer.FenToYuan_Table(ds.Tables[0], "Fsave", "Fsave_str");

                        string state = PublicRes.objectToString(ds.Tables[0], "Fstate");

                        if (isWechat && !cancelSign)  //是微信账号,但不是已经注销的账号
                        {
                            if (state == "1")
                            {
                                dr["Fstate_str"] = "正常";
                            }
                            else if (state == "2")
                            {
                                dr["Fstate_str"] = "冻结";
                            }
                            else
                            {
                                dr["Fstate_str"] = "";
                            }
                        }
                        else
                        {
                            dr["Fstate_str"] = TransferMeaning.Transfer.accountState(dr["Fstate"].ToString());
                        }

                        string s_fz_amt = PublicRes.objectToString(ds.Tables[0], "Ffz_amt"); //分账冻结金额
                        string s_balance = PublicRes.objectToString(ds.Tables[0], "Fbalance");
                        string s_cron = PublicRes.objectToString(ds.Tables[0], "Fcon");
                        long l_balance = 0, l_cron = 0, l_fzamt = 0;
                        if (s_balance != "")
                        {
                            l_balance = long.Parse(s_balance);
                        }
                        if (s_cron != "")
                        {
                            l_cron = long.Parse(s_cron);
                        }
                        if (s_fz_amt != "")
                        {
                            l_fzamt = long.Parse(s_fz_amt);
                        }

                        dr["Ffreeze_fee"] = MoneyTransfer.FenToYuan((l_fzamt + l_cron).ToString());    //冻结金额=分账冻结金额+冻结金额
                        dr["Fuseable_fee"] = MoneyTransfer.FenToYuan((l_balance - l_cron).ToString());   //可用余额=帐户余额减去冻结余额

                        int tempAtt = 0;
                        if (dr["Att_id"].ToString() != "")
                        {
                            tempAtt = int.Parse(dr["Att_id"].ToString());
                        }
                        if (tempAtt != 0)
                        {
                            dr["Fpro_att"] = TransferMeaning.Transfer.convertProAttType(tempAtt);
                        }
                        else
                        {
                            dr["Fpro_att"] = "";
                        }

                        string qq = dr["Fqqid"].ToString();
                        string email = dr["Femail"].ToString();
                        string mobile = dr["Fmobile"].ToString();
                        string fuid = PublicRes.objectToString(ds.Tables[0], "fuid");

                        if (qq != "")
                        {
                            string uid1 = QQ2Uid(qq);
                            if (uid1 != null)
                            {
                                dr["Fqqid_state"] = "注册未关联";
                                if (uid1.Trim() == fuid)
                                {
                                    //再判断是否是已激活
                                    string uid2 = QQ2UidX(qq);
                                    if (uid2 != null)
                                        dr["Fqqid_state"] = "已关联";
                                    else
                                        dr["Fqqid_state"] = "已关联未激活";
                                }
                            }
                            else
                                dr["Fqqid_state"] = "未注册";

                        }


                        if (email != "")
                        {
                            string uid1 = QQ2Uid(email);
                            if (uid1 != null)
                            {
                                dr["Femial_state"] = "注册未关联";
                                if (uid1.Trim() == fuid)
                                {
                                    string uid2 = QQ2UidX(email);
                                    if (uid2 != null)
                                        dr["Femial_state"] = "已关联";
                                    else
                                        dr["Femial_state"] = "已关联未激活";
                                }
                            }
                            else
                                dr["Femial_state"] = "未注册";
                        }


                        if (mobile != "")
                        {
                            string uid1 = QQ2Uid(mobile);
                            if (uid1 != null)
                            {
                                dr["Fmobile_state"] = "注册未关联";
                                if (uid1.Trim() == fuid)
                                {
                                    string uid2 = QQ2UidX(mobile);
                                    if (uid2 != null)
                                        dr["Fmobile_state"] = "已关联";
                                    else
                                        dr["Fmobile_state"] = "已关联未激活";
                                }
                            }
                            else
                                dr["Fmobile_state"] = "未注册";
                        }


                        try
                        {
                            string name = dr["UserRealName2"].ToString();
                            if (!string.IsNullOrEmpty(name))
                            {
                                dr["Fname_str"] = dr["UserRealName2"].ToString();
                            }
                        }
                        catch
                        {
                            dr["Fname_str"] = dr["Ftruename"].ToString();
                        }

                    }

                    #endregion
                }

                return ds;
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("查询个人账户信息出错: " + ex.Message);
                return null;
            }
        }

        public DataSet GetUserAccountFromWechat(string u_QQID, int istr, int imax)
        {
            return new AccountData().GetUserAccountFromWechat(u_QQID, istr, imax);
        }

        /// <summary>
        /// 查询注销用户帐户表
        /// </summary>
        /// <param name="fuid"></param>
        /// <param name="fcurtype"></param>
        /// <param name="istr"></param>
        /// <param name="imax"></param>
        /// <returns></returns>
        public DataSet GetUserAccountCancel(string qqid, int fcurtype, int istr, int imax)
        {
            return new AccountData().GetUserAccountCancel(qqid, fcurtype, istr, imax);
        }

        #endregion

    }

    #region 异常姓名类
    public class NameAbnormalClass
    {
        public string Fuin;
        public string Fname_old;
        public string Fcre_id_old;
        public string Ftruename;
        public string Fcre_id;
        public string Fcre_type;
        public string Fcre_version;
        public string Fcre_valid_day;
        public string Faddress;
        public string Fimage_cre1;
        public string Fimage_cre2;
        public string Fimage_evidence;
        public string Fimage_other;
        public string Fsubmit_time;
        public string Fsubmit_user;
        public string Fcheck_time;
        public string Fcheck_user;
        public string Fcheck_state;
        public string Frefuse_reason;
        public string Fcomment;
    }
    #endregion

    #region 实名认证置失效类
    public class AuthenStateDeleteClass
    {
        public string Fuid;
        public string Fname_old;
        public string Fcre_id;
        public string Fcre_type;
        public string Fimage_cre1;
        public string Fimage_cre2;
        public string Fimage_evidence;
        public string Fsubmit_time;
        public string Fsubmit_user;
    }
    #endregion
}
