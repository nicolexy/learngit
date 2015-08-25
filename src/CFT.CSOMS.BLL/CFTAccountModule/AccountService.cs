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
        public  string Uid2QQ(string uin)
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

        public DataTable QueryVipInfo(string uin)
        {
            try
            {
                DataSet ds = new AccountData().QueryVipInfo(uin);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add("vipflag_str", typeof(String));
                    ds.Tables[0].Columns.Add("subid_str", typeof(String));
                    DataRow dr = ds.Tables[0].Rows[0];
                    dr.BeginEdit();
                    string vipflag = dr["vipflag"].ToString();
                    if (vipflag == "0")
                        dr["vipflag_str"] = "非会员";
                    else if (vipflag == "1")
                        dr["vipflag_str"] = "普通会员";
                    else if (vipflag == "2")
                        dr["vipflag_str"] = "VIP会员";
                    else if (vipflag == "4")
                        dr["vipflag_str"] = "连续1个月不做任务的普通会员（同非会员）";
                    else
                        dr["vipflag_str"] = "Unknown";

                    string subid = dr["subid"].ToString();
                    if (subid == "0")
                        dr["subid_str"] = "无支付方式";
                    else if (subid == "1")
                        dr["subid_str"] = "手机支付";
                    else if (subid == "2")
                        dr["subid_str"] = "个人帐户支付";
                    else if (subid == "3")
                        dr["subid_str"] = "Vnet支付";
                    else if (subid == "4")
                        dr["subid_str"] = "PHS小灵通支付";
                    else if (subid == "5")
                        dr["subid_str"] = "银行支付";
                    else if (subid == "100")
                        dr["subid_str"] = "Q币卡支付";
                    else if (subid == "101")
                        dr["subid_str"] = "声讯支付";
                    else if (subid == "102")
                        dr["subid_str"] = "ESALES支付";
                    else if (subid == "103")
                        dr["subid_str"] = "他人赠送";
                    else if (subid == "104")
                        dr["subid_str"] = "索要";
                    else if (subid == "105")
                        dr["subid_str"] = "公司活动赠送";
                    else if (subid == "106")
                        dr["subid_str"] = "免费（用于公免）";
                    else if (subid == "107")
                        dr["subid_str"] = "电信卡";
                    else if (subid == "108")
                        dr["subid_str"] = "缴费卡支付";
                    else if (subid == "109")
                        dr["subid_str"] = "积分";
                    else if (subid == "110")
                        dr["subid_str"] = "广州收费易";
                    else if (subid == "111")
                        dr["subid_str"] = "Q点";
                    else if (subid == "112")
                        dr["subid_str"] = "EPAY";
                    else if (subid == "113")
                        dr["subid_str"] = "MPAY";
                    else if (subid == "114")
                        dr["subid_str"] = "声讯预付费（活动接口）";
                    else if (subid == "115")
                        dr["subid_str"] = "PPW_PAIPAI 拍拍渠道";
                    else if (subid == "116")
                        dr["subid_str"] = "赠送索回";
                    else if (subid == "117")
                        dr["subid_str"] = "声讯预付费  2006.10,移动联通";
                    else if (subid == "118")
                        dr["subid_str"] = "Q币不足Q点支付";
                    else if (subid == "119")
                        dr["subid_str"] = "Q点不足Q币支付";
                    else if (subid == "120")
                        dr["subid_str"] = "移动do分离";
                    else if (subid == "121")
                        dr["subid_str"] = "tenpay";
                    else if (subid == "122")
                        dr["subid_str"] = "手机声讯渠道";
                    else if (subid == "123")
                        dr["subid_str"] = "统一帐户渠道";
                    else
                        dr["subid_str"] = "Unknown";
                    dr.EndEdit();

                    return ds.Tables[0];
                }
                return null;
            }
            catch (Exception ex)
            {
                //throw new Exception(string.Format("查询会员信息异常:{0}", ex.Message));
                log4net.LogManager.GetLogger("查询会员信息异常: " + ex.Message);
                return null;
            }
        }
                              
        public bool AddChangeUserInfoLog(string qqid, string cre_type, string cre_type_old, string user_type, string user_type_old, string attid, string attid_old, string commet, string commet_old, string submit_user)
        {
            if (string.IsNullOrEmpty(qqid))
            {
                throw new ArgumentNullException("qqid");
            }

            return new AccountData().AddChangeUserInfoLog(qqid, cre_type, cre_type_old, user_type, user_type_old, attid, attid_old, commet, commet_old, submit_user);
        }

        public DataTable QueryChangeUserInfoLog(string qqid, int offset, int limit)
        {
            if (string.IsNullOrEmpty(qqid))
            {
                throw new ArgumentNullException("qqid");
            }
            return new AccountData().QueryChangeUserInfoLog(qqid, offset, limit);
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
            
        public DataTable GetFetchListIntercept(string fetchListid)
        {
            if (string.IsNullOrEmpty(fetchListid))
            {
                throw new ArgumentNullException("fetchListid");
            }
            return (new AccountData()).GetFetchListIntercept(fetchListid);
        }

        public bool AddFetchListIntercept(string fetchListid, string opera)
        {
            if (string.IsNullOrEmpty(fetchListid))
            {
                throw new ArgumentNullException("fetchListid");
            }

            var dt = new AccountService().GetFetchListIntercept(fetchListid);
            if (dt != null && dt.Rows.Count > 0)
                throw new Exception("该体现单号已拦截！");

            return (new AccountData()).AddFetchListIntercept(fetchListid, opera);
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

            DataTable dt =new LCTBalanceService().QuerySubAccountInfo(query_id, 1);    //主帐户余额
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
                            dr["Fpro_att"] = CheckBasicInfo(tempAtt);
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
      
        private string CheckBasicInfo(int nAttid)
        {
            //从数据字典中读取数据，绑定到web页
            DataSet ds = PermitPara.QueryDicAccName();
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return "";
            }
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                if (nAttid == int.Parse(dr["Value"].ToString().Trim()))
                {
                    return dr["Text"].ToString().Trim();
                }
            }
            return "";
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
