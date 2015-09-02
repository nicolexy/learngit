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

        #region 销户操作

        /// <summary>
        /// 销户日志查询
        /// </summary>
        /// <returns></returns>
        public DataSet GetCanncelAccountLog(string qqid, string opera, DateTime begin_time, DateTime end_time, int offset, int limit, out string msg)
        {
            return new AccountData().logOnUserHistory(qqid, opera, begin_time, end_time, offset, limit, out msg);
        }

        /// <summary>
        /// 申请销户
        /// </summary>
        /// <param name="query_id"></param>
        /// <param name="query_type">0，财付通，手Q账号；1微信账号</param>
        /// <param name="reason"></param>
        /// <param name="is_Send">是否发送邮件</param>
        /// <param name="opera"></param>
        /// <param name="ip"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool LogOnUserDeleteUser(string query_id, int query_type, string reason, bool is_Send, string email_Addr, string opera, out string ret_msg, out bool ret_continue)
        {
            bool ret_value = false;
            ret_msg = "";
            ret_continue = false;

            if (reason.Length > 255)
            {
                ret_msg = "备注字数不得超过255个字符";
                return false;
            }
            if (is_Send && TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.CheckEmail(email_Addr))
            {
                ret_msg = "请输入正确格式的用户邮箱地址";
                return false;
            }
            string wx_id = "";
            //微信账号
            if (query_type == 1)
            {
                wx_id = query_id;
                query_id = WeChatHelper.GetUINFromWeChatName(query_id);
            }

            string memo = "[注销QQ号码:" + query_id + "]注销原因:" + reason;
            string Msg = "";
            if (!new AccountData().checkUserReg(query_id, out Msg))
            {
                ret_msg = "帐号非法或者未注册！";
                return false;
            }

            //手Q用户转账中、退款中、未完成的订单禁止注销和批量注销
            if (Regex.IsMatch(query_id, @"^[1-9]\d*$"))
            {
                DataSet dsHandQ = new TradeService().QueryPaymentParty("", "1,2,12,4", "3", query_id);
                if (dsHandQ != null && dsHandQ.Tables.Count > 0 && dsHandQ.Tables[0].Rows.Count > 0 && dsHandQ.Tables[0].Rows[0]["result"].ToString() != "97420006")
                {
                    ret_msg = "手Q用户转账中、退款中、未完成的订单禁止注销和批量注销";
                    return false;
                }
                try
                {
                    DataSet dsMobileQHB = new TradeService().GetUnfinishedMobileQHB(query_id);
                    if (dsMobileQHB.Tables[0].Columns.Contains("row_num"))
                    {
                        if (dsMobileQHB != null && dsMobileQHB.Tables.Count > 0 && dsMobileQHB.Tables[0].Rows.Count > 0 && int.Parse(dsMobileQHB.Tables[0].Rows[0]["row_num"].ToString()) > 0)
                        {
                            ret_msg = "该账户存在未完成的手Q红包交易，禁止注销和批量注销";
                            return false;
                        }
                    }
                    else
                    {
                        ret_msg = "查询是否有未完成手Q红包交易失败!";
                        return false;
                    }
                }
                catch (System.Exception ex)
                {
                    LogHelper.LogError("查询手Q红包未完成交易失败" + ex.Message, "LogOnUser");
                    ret_msg = "查询手Q红包未完成交易失败" + ex.Message;
                    return false;
                }
            }

            //有微信支付、转账、红包未完成的禁止注销和批量注销
            if (query_type == 1)
            {
                try
                {
                    int WXUnfinishedTrade = (new TradeService()).QueryWXUnfinishedTrade(wx_id);
                    if (WXUnfinishedTrade > 0)
                    {
                        LogHelper.LogInfo("此账号有未完成微信支付转账，禁止注销!");
                        ret_msg = "此账号有未完成微信支付转账，禁止注销!";
                        return false;
                    }
                }
                catch (System.Exception ex)
                {
                    LogHelper.LogError("销户操作查询微信支付在途条目出错" + ex.Message);
                    ret_msg = "销户操作查询微信支付在途条目出错" + ex.Message;
                    return false;
                }
            }

            //是否有未完成的交易单
            if (new TradeService().LogOnUsercheckOrder(query_id, "1"))
            {
                ret_msg = "有未完成的交易单";
                return false;
            }
            //是否开通一点通
            if (new BankCardBindService().LogOnUserCheckYDT(query_id, "1"))
            {
                ret_msg = "开通了一点通";
                return false;
            }

            //金额大于阀值，提起一个销户申请
            //金额小于阀值,插入logonhistory表,调用接口注销,如果有邮箱,向邮箱发送邮件,反馈信息给一线人员.

            long balance = 0;

            DataTable dt = new LCTBalanceService().QuerySubAccountInfo(query_id, 1);    //主帐户余额
            if (dt != null && dt.Rows.Count > 0)
            {
                balance += long.Parse(dt.Rows[0]["Fbalance"].ToString().Trim());
            }
            dt = new LCTBalanceService().QuerySubAccountInfo(query_id, 80);     //游戏子帐户
            if (dt != null && dt.Rows.Count > 0)
            {
                balance += long.Parse(dt.Rows[0]["Fbalance"].ToString().Trim());
            }
            dt = new LCTBalanceService().QuerySubAccountInfo(query_id, 82);     //直通车子帐户
            if (dt != null && dt.Rows.Count > 0)
            {
                balance += long.Parse(dt.Rows[0]["Fbalance"].ToString().Trim());
            }

            if (balance < 5000) //系统自动销户
            {
                if (new AccountData().LogOnUserDeleteUser(query_id, reason, opera, "", out Msg))
                {
                    throw new Exception(Msg);

                }
                //系统自动注销成功给用户发邮件
                if (is_Send)
                {
                    SendEmail(email_Addr, query_id, "系统自动销户", out Msg);
                }
                ret_msg = "系统自动销户成功！";
                return true;
            }
            else //提起销户申请
            {
                //TODO:提起销户申请
                ret_continue = true;
            }

            return ret_value;
        }

        private bool SendEmail(string email, string qqid, string subject, out string Msg)
        {
            Msg = "";
            try
            {
                string str_params = "p_name=" + qqid + "&p_parm1=" + DateTime.Now + "&p_parm2=" + "" + "&p_parm3=" + "" + "&p_parm4=" + "系统自动销户";
                TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsg(email, "2034", str_params);
                return true;
            }
            catch (Exception err)
            {
                Msg = "给用户发邮件出错：" + err.Message;
                return false;
            }
        }

        #endregion

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
