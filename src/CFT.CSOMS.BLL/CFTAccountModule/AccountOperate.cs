using CFT.Apollo.Logging;
using CFT.CSOMS.BLL.BankCardBindModule;
using CFT.CSOMS.BLL.FundModule;
using CFT.CSOMS.BLL.TradeModule;
using CFT.CSOMS.COMMLIB;
using CFT.CSOMS.DAL.CFTAccount;
using CFT.CSOMS.DAL.CheckModoule;
using CFT.CSOMS.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TENCENT.OSS.C2C.Finance.BankLib;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace CFT.CSOMS.BLL.CFTAccountModule
{
    public class AccountOperate
    {
        #region 账户QQ修改

        //提出修改QQ申请
        public bool ChangeQQApply(string old_qqid, string new_qqid, string reason, string user, string ip, out string outMsg)
        {
            outMsg = "";
            if (string.IsNullOrEmpty(old_qqid))
            {
                outMsg = "请输入旧帐号！";
                return false;
            }
            if (string.IsNullOrEmpty(new_qqid))
            {
                outMsg = "请输入旧帐号！";
                return false;
            }

            //发起审批。
            //在这里变成了一个提起审批的流程，而不再是直接审批。
            Param[] myParams = new Param[3];
            myParams[0] = new Param();
            myParams[0].ParamName = "OldQQ";
            myParams[0].ParamValue = old_qqid.Trim();

            myParams[1] = new Param();
            myParams[1].ParamName = "NewQQ";
            myParams[1].ParamValue = new_qqid.Trim();

            myParams[2] = new Param();
            myParams[2].ParamName = "Memo";
            myParams[2].ParamValue = "修改帐号，原帐号" + old_qqid.Trim() + "，新帐号" + new_qqid.Trim() + "。理由：" + commRes.replaceSqlStr(reason.Trim());

            string strMemo = "修改帐号，原帐号" + old_qqid.Trim() + "，新帐号" + new_qqid.Trim() + "。理由：" + commRes.replaceSqlStr(reason.Trim());

            try
            {
                new CheckService().StartCheck(old_qqid, "ChangeQQ", strMemo, "0", user, ip, myParams);
                outMsg = "提请审批成功";
                return true;
            }
            catch (Exception err)
            {
                outMsg = "提请审批失败，错误原因：" + PublicRes.GetErrorMsg(err.Message) + "。";
                return false;
            }
        }

        //修改QQ查询函数-历史记录
        public DataSet GetChangeQQList(string userid, string qq, int iPageStart, int iPageMax)
        {
            try
            {
                ChangeQQQueryClass cuser = new ChangeQQQueryClass(userid, qq);
                return cuser.GetResultX(iPageStart, iPageMax, "HT");
            }
            catch (Exception err)
            {
                throw new Exception("Service处理失败！");
            }
        }

        //判断是否符合修改QQ条件
        public bool ChangeQQState(string old_qqid, string opUin, out string outMsg)
        {
            outMsg = "";
            if (isHasBalance(old_qqid.Trim()))
            {
                outMsg = "原帐号理财通账户余额和基金份额不为0时不允许修改转换！";
                return false;
            }

            #region 微粒贷
            try
            {
                if (new TradeService().HasUnfinishedWeiLibDai(old_qqid.Trim()))
                {
                    outMsg = "存在未完成的微粒贷欠款,禁止修改账号";
                    return false;
                }
            }
            catch (Exception)
            {
                outMsg = "微粒贷查询,出错";
                return false;
            }
            #endregion

            #region 判断是否有绑定确认状态的银行卡
            var ds = new BankCardBindService().GetBankCardBindList_New(old_qqid, "", "", "", "", "", "", "", "", 2, "", opUin, 0, "", 0, 1);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                outMsg = "绑定确认状态的银行卡时,不允许修改转换！";
                return false;
            }
            #endregion

            return true;
        }

        //原帐号理财通账户余额和基金份额不为0时不允许修改转换
        public bool isHasBalance(string qq)
        {
            string uin = qq; //"1563686969"
            try
            {
                double totalBalance = 0;
                DataTable summaryTable = new FundService().GetUserFundSummary(uin);
                foreach (DataRow item in summaryTable.Rows)
                {
                    totalBalance += Convert.ToDouble(item["balance"].ToString());
                }
                if (totalBalance > 0)
                {
                    return true;
                }
            }
            catch
            {
            }

            try
            {
                double LCTBalance = 0;
                //理财通余额，币种89,统计收益总和，和余额总和
                DataTable subAccountInfoTable = new LCTBalanceService().QuerySubAccountInfo(uin, 89);
                if (subAccountInfoTable != null && subAccountInfoTable.Rows.Count > 0)
                {
                    LCTBalance = Convert.ToDouble(subAccountInfoTable.Rows[0]["Fbalance"].ToString());
                }
                if (LCTBalance > 0)
                {
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        #endregion

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
        /// 获取销户信息
        /// </summary>
        /// <param name="query_id"></param>
        /// <param name="query_type">0，财付通，手Q账号；1微信账号</param>
        /// <param name="reason"></param>
        /// <param name="is_Send">是否发送邮件</param>
        /// <param name="opera"></param>
        /// <param name="ip"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool CloseingAccountInfo(string query_id, int query_type, string reason, bool is_Send, string email_Addr, string opera, string ip, out string ret_msg)
        {
            ret_msg = "";

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

            //Param[] myParams = new Param[4];
            //myParams[0] = new Param();
            //myParams[0].ParamName = "fqqid";
            //myParams[0].ParamValue = query_id;

            //myParams[1] = new Param();
            //myParams[1].ParamName = "Memo";
            //myParams[1].ParamValue = memo;

            //myParams[2] = new Param();
            //myParams[2].ParamName = "returnUrl";
            //myParams[2].ParamValue = "/BaseAccount/InfoCenter.aspx?id=" + query_id;

            //myParams[3] = new Param();
            //myParams[3].ParamName = "fuser";
            //myParams[3].ParamValue = opera;

            //string mainID = DateTime.Now.ToString("yyyyMMdd") + query_id;
            //string checkType = "logonUser";


            //手Q用户转账中、退款中、未完成的订单禁止注销和批量注销
            if (Regex.IsMatch(query_id, @"^[1-9]\d*$"))
            {
                #region 转账
                string mobileQTransferErrorMsg = "";
                DataSet dsMobileQTransfer = new TradeService().GetUnfinishedMobileQTransfer(query_id, out mobileQTransferErrorMsg);
                if (!string.IsNullOrEmpty(mobileQTransferErrorMsg))
                {
                    ret_msg = "查询手Q转账记录失败" + mobileQTransferErrorMsg;
                    return false;
                }
                if (dsMobileQTransfer != null && dsMobileQTransfer.Tables.Count > 0 && dsMobileQTransfer.Tables[0].Rows.Count > 0 && dsMobileQTransfer.Tables[0].Rows[0]["result"].ToString() != "192720108")
                {
                    ret_msg = "手Q用户转账中、退款中、未完成的订单禁止注销和批量注销";
                    return false;
                }
                #endregion             
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
                    var WXUnfinishedTrade = (new TradeService()).QueryWXUnfinishedTrade(wx_id);
                    if (!WXUnfinishedTrade)
                    {
                        LogHelper.LogInfo(wx_id + "此账号有未完成微信支付转账，禁止注销!");
                        ret_msg = "此账号有未完成微信支付转账，禁止注销!";
                        return false;
                    }

                    var HasUnfinishedHB = (new TradeService()).QueryWXUnfinishedHB(wx_id);
                    if (!HasUnfinishedHB)
                    {
                        LogHelper.LogInfo(wx_id + "此账号有未完成微信红包，禁止注销!");
                        ret_msg = "此账号有未完成微信红包，禁止注销!";
                        return false;
                    }

                    //int WXUnfinishedTrade = (new TradeService()).QueryWXUnfinishedTrade(wx_id);
                    //if (WXUnfinishedTrade > 0)
                    //{
                    //    LogHelper.LogInfo("此账号有未完成微信支付转账，禁止注销!");
                    //    ret_msg = "此账号有未完成微信支付转账，禁止注销!";
                    //    return false;
                    //}

                    //var endDate = DateTime.Today.AddDays(+1);
                    //var startDate = endDate.AddDays(-15);
                    //var openid = query_id.Replace("@hb.tenpay.com", "");
                    //if (!string.IsNullOrEmpty(openid))
                    //{
                    //    var HasUnfinishedHB = (new TradeService()).QueryWXHasUnfinishedHB(openid, startDate, endDate);
                    //    if (HasUnfinishedHB)
                    //    {
                    //        LogHelper.LogInfo("此账号有未完成微信红包，禁止注销!");
                    //        ret_msg = "此账号有未完成微信红包，禁止注销!";
                    //        return false;
                    //    }
                    //}

                }
                catch (System.Exception ex)
                {
                    LogHelper.LogError("销户操作查询微信支付在途条目出错" + ex.Message);
                    ret_msg = "销户操作查询微信支付在途条目出错" + ex.Message;
                    return false;
                }
            }

            #region 微粒贷
            try
            {
                if (new TradeService().HasUnfinishedWeiLibDai(query_id))
                {
                    ret_msg = "存在未完成的微粒贷欠款,禁止注销和批量注销";
                    return false;
                }
            }
            catch (Exception)
            {
                ret_msg = "微粒贷查询,出错";
                return false;
            }
            #endregion

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

            if (balance < 10 * 10 * 200) //系统自动销户
            {
                ret_msg = "账户余额小于200元，可以注销";
                return true;
                //if (!new AccountData().LogOnUserDeleteUser(query_id, reason, opera, ip, out Msg))
                //{
                //    throw new Exception(Msg);

                //}
                ////系统自动注销成功给用户发邮件
                //if (is_Send)
                //{
                //    SendEmail(email_Addr, query_id, "系统自动销户", out Msg);
                //}
                //ret_msg = "系统自动销户成功！";
                //return true;
            }
            else
            {
                ret_msg = "账户余额大于或等于200元，请提起审批流程";
                return false;
            }
            //else //提起销户申请
            //{
            //    
            //    try
            //    {
            //        new CheckService().StartCheck(mainID, checkType, memo, MoneyTransfer.FenToYuan(balance.ToString()), opera, ip, myParams);
            //        ret_msg = "销户申请提请成功！";
            //        return true;
            //    }
            //    catch (Exception ex)
            //    {
            //        ret_msg = "销户申请提请失败：" + ex.Message;
            //        return false;
            //    }
            //}

        }

        //注销账号
        public bool LogOnUserDeleteUser(string query_id, int query_type, string reason, bool is_Send, string email_Addr, string opera, string ip, out string Msg)
        {
            //0，财付通，手Q账号；1微信账号
            if (query_type == 1)
            {
                query_id = WeChatHelper.GetUINFromWeChatName(query_id);
            }
            if (!new AccountData().LogOnUserDeleteUser(query_id, reason, opera, ip, out  Msg))
            {
                Msg = "系统自动销户失败 ： " + Msg;
                return false;
            }
            if (is_Send)
            {
                SendEmail(email_Addr, query_id, "系统自动销户", out Msg);
            }
            Msg = "系统自动销户成功！";
            return true;
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
    }
}
