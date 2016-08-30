using CFT.CSOMS.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SunLibraryEX;
using CFT.CSOMS.DAL.WechatPay.Entity;
using commLib.Entity;
using commLib.Entity.HKWallet;
using CFT.Apollo.Logging;

namespace CFT.CSOMS.DAL.ForeignCurrencModule
{
    /// <summary>
    /// 香港钱包相关
    /// </summary>
    public class FCXGWallet
    {
        string ip = Apollo.Common.Configuration.AppSettings.Get<string>("FCXGWallet_IP", "10.12.62.107");//10.12.189.88 新:10.12.62.107  线上10.192.100.115
        int port = Apollo.Common.Configuration.AppSettings.Get<int>("FCXGWallet_Port", 22000);

        /// <summary>
        /// 香港红包Relay 新接口请求IP
        /// </summary>
        string IP_RedPacket = Apollo.Common.Configuration.AppSettings.Get<string>("FCXGWallet_IP_ForRedPacket", "10.12.62.107");//10.12.189.88 新:10.12.62.107  线上10.192.100.115
        /// <summary>
        /// 香港红包Relay 新接口请求端口
        /// </summary>
        int Port_RedPacket = Apollo.Common.Configuration.AppSettings.Get<int>("FCXGWallet_Port_ForRedPacket", 22000);
        /// <summary>
        /// 香港红包Relay 编码格式
        /// </summary>
        string RedPacket_Encode = Apollo.Common.Configuration.AppSettings.Get<string>("FCXGWallet_RedPacket_Encode", "gb2312");


        #region 一、外币帐号查询

        /// <summary>
        /// 查询账户基本信息
        /// </summary>
        /// <param name="uid">用户账号</param>
        /// <returns></returns>
        public DataTable QueryUserInfo(string uid, string client_ip)
        {
            //uid = "600000347";
            string req = "CMD=QUERY_USERINFO&uid=" + uid;
            string result = RelayAccessFactory.RelayInvoke(req, "8514", false, false, ip, port, "utf-8");
//#if DEBUG
//            result = "result=0&address=&area=&city=&company_name=&country=&cre_id=&cre_type=0&create_time=2015-10-22 17:50:38&email=&gender=0&lstate=2&memo=&mobile=&modify_time=2016-01-04 14:07:32&openid=&phone=&postal_code=&state=3&true_name=&uid=600000347&uin=o5PXlsm0qtXfIA2bLkoMoeXHT51c@wx.hkg&user_type=2&MSG_NO=10014519827780000000042";
//#endif           
            DataTable dt = ParseRelayOneRow(result, "查询账户基本信息");

            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Add("balance", typeof(string));//用户余额
                //查询账户余额
                try
                {
                    string reqText = "uid={0}&client_ip={1}&cur_type=344&acc_type=2&MSG_NO={2}";
                    reqText = string.Format(reqText, uid, client_ip, "101741" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                    result = RelayAccessFactory.RelayInvoke(reqText, "101741", false, false, ip, port, "utf-8");
                    //result=0&res_info=ok&uid=600000347&cur_type=344&balance=997700&MSG_NO=10174120160104190031

                    DataTable dt_userbalance = ParseRelayOneRow(result, "查询账户余额");
                    string balance = (dt_userbalance != null && dt_userbalance.Rows.Count > 0) ? dt_userbalance.Rows[0]["balance"].ToString() : "0";
                    //港币分转元
                    balance = MoneyTransfer.FenToYuan(balance, "HKD");
                    dt.Rows[0]["balance"] = balance;
                }
                catch 
                {
                    //异常吃掉，有问题看日志；
                    dt.Rows[0]["balance"] = "0";
                }
            }
            return dt;
        }

        /// <summary>
        /// 查询用户uid
        /// </summary>
        /// <param name="uin">用户账号</param>
        /// <returns></returns>
        public DataTable QueryUserId(string uin)
        {
            var req = "CMD=QUERY_UID_BY_UIN&uin=" + uin + "&";
            var result = RelayAccessFactory.RelayInvoke(req, "8514", false, false, ip, port, "utf-8");
            string msg = "";
            var ds = TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.GetDataSetFromReply(result, out msg);
            if (msg != "")
            {
                throw new Exception(msg);
            }
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }

        /// <summary>
        /// 冻结/解冻用户
        /// </summary>
        /// <param name="uin">用户uin</param>
        /// <param name="lock_status">0 表示锁定用户  1、表示对用户解锁</param>
        /// <param name="channel">冻结渠道</param>
        /// <param name="op_name">操作员</param>
        /// <param name="true_name">用户姓名</param>
        /// <param name="phone">用户联系方式</param>
        /// <param name="reason">冻结原因</param>
        /// <param name="memo">备注</param>
        /// <param name="client_ip">客户端ip</param>
        /// <returns></returns>
        public bool LockUser(string uin, int lock_status, string channel, string op_name, string true_name, string phone, string reason, string memo, string client_ip)
        {
            var key = Apollo.Common.Configuration.AppSettings.Get<string>("", "EC177C6FA14DFBFF6E288580D0AFA6C2");
            var req =
                "uin=" + uin +
                "&lock_status=" + lock_status.ToString() +
                "&channel=" + channel +
                "&op_name=" + op_name +
                "&key=" + key +
                "&true_name=" + true_name +
                "&phone=" + phone +
                "&reason=" + reason +
                "&memo=" + memo +
                "&client_ip=" + client_ip
                ;
            var relay_result = RelayAccessFactory.RelayInvoke(StringTransCode(req), "101403", true, false, ip, port, "utf-8");
            var dic = relay_result.ToDictionary();
            if (dic["result"] == "0")
            {
                return true;
            }
            else
            {
                throw new Exception("冻结/解冻失败:" + relay_result);
            }
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="uin">用户uin</param>
        /// <param name="op_name">操作员</param>
        /// <param name="true_name">用户姓名</param>
        /// <param name="phone">用户联系方式</param>
        /// <param name="reason">冻结原因</param>
        /// <param name="memo">备注</param>
        /// <param name="client_ip">客户端ip</param>
        /// <returns></returns>
        public bool ResetPassWord(string uin, string op_name, string true_name, string phone, string reason, string memo, string client_ip)
        {
            var key = Apollo.Common.Configuration.AppSettings.Get<string>("", "EC177C6FA14DFBFF6E288580D0AFA6C2"); ;
            var req =
                "uin=" + uin +
                "&op_name=" + op_name +
                "&key=" + key +
                "&true_name=" + true_name +
                "&phone=" + phone +
                "&reason=" + reason +
                "&memo=" + memo +
                "&client_ip=" + client_ip
                ;
            LogHelper.LogInfo(string.Format("101404;StringTransCode"));
            string strCode = StringTransCode(req);
            if (!string.IsNullOrEmpty(strCode))
            {
                LogHelper.LogInfo(string.Format("101404;StringTransCode转码成功{0}"), strCode);
                var relay_result = RelayAccessFactory.RelayInvoke(strCode, "101404", true, false, ip, port, "utf-8");
                var dic = relay_result.ToDictionary();
                if (dic["result"] == "0")
                {
                    LogHelper.LogInfo(string.Format("101404;接口调用成功"));
                    return true;
                }
                else
                {
                    throw new Exception("重置密码失败:" + relay_result);
                }
            }
            else
            {
                LogHelper.LogInfo(string.Format("101404;StringTransCode转码失败"));
                throw new Exception("重置密码失败:StringTransCode转码失败");
            }
           
        }

        #endregion

        #region 二、绑卡查询

        /// <summary>
        /// 绑卡查询
        /// </summary>
        /// <param name="client_ip">IP</param>
        /// <param name="uin">用户账号</param>
        /// <param name="card_id">卡号</param>
        /// <returns></returns>
        public DataTable QueryBindCardInfo(string client_ip, string uin = null, string card_id = null)
        {
            try
            {
                var req = "client_ip=" + client_ip;
                if (!string.IsNullOrEmpty(uin))
                {
                    req += "&uin=" + uin;
                }
                if (!string.IsNullOrEmpty(card_id))
                {
                    req += "&card_id=" + card_id;
                }
                var result = RelayAccessFactory.RelayInvoke(req, "101103", true, false, ip, port, "utf-8");
                return TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.ParseRelayStringToDataTable(result, "row_num", "row_info");
            }
            catch (Exception ex)
            {
                throw new Exception("绑卡查询:" + ex.Message);
            }
        }

        /// <summary>
        /// 解绑卡
        /// </summary>
        /// <param name="uin">用户uin</param>
        /// <param name="bind_serialno">绑卡序列号</param>
        /// <param name="op_name">操作员</param>
        /// <param name="key">操作员密钥</param>
        /// <param name="true_name">用户姓名</param>
        /// <param name="phone">用户联系方式</param>
        /// <param name="reason">冻结原因</param>
        /// <param name="memo">备注</param>
        /// <param name="client_ip">客户端ip</param>
        /// <returns></returns>
        public bool FreeBindCard(string uin, string bind_serialno, string op_name, string true_name, string phone, string reason, string memo, string client_ip)
        {
            var key = Apollo.Common.Configuration.AppSettings.Get<string>("", "EC177C6FA14DFBFF6E288580D0AFA6C2");
            try
            {
                var req =
                    "uin=" + uin +
                    "&bind_serialno=" + bind_serialno +
                    "&op_name=" + op_name +
                    "&key=" + key +
                    "&true_name=" + true_name +
                    "&phone=" + phone +
                    "&reason=" + reason +
                    "&memo=" + memo +
                    "&client_ip=" + client_ip
                    ;
                var result = RelayAccessFactory.RelayInvoke(StringTransCode(req), "101402", true, false, ip, port, "utf-8");
                var dic = result.ToDictionary();
                return dic["result"] == "0";
            }
            catch (Exception ex)
            {
                throw new Exception("解绑卡失败:" + ex.Message);
            }
        }

        #endregion

        #region 三、订单查询（新）

        /// <summary>
        /// 通过商户和商户订单号查找订单
        /// </summary>
        /// <param name="spid">商户号</param>
        /// <param name="sp_billno">商户订单号</param>
        public DataTable QueryOrderBySp(string spid, string sp_billno)
        {
            var req = "spid=" + spid + "&sp_billno=" + sp_billno;
            var result = RelayAccessFactory.RelayInvoke(req, "8508", false, false, ip, port, "utf-8");
            return ParseRelayOneRow(result, "通过商户和商户订单号查找订单");
        }

        /// <summary>
        /// 通过MD订单号查找
        /// </summary>
        /// <param name="listid">MD订单号</param>
        public DataTable QueryOrderByMDList(string listid)
        {
            var req = "listid=" + listid;
            var result = RelayAccessFactory.RelayInvoke(req, "8511", false, false, ip, port, "utf-8");
            return ParseRelayOneRow(result, "通过MD订单号查找");
        }

        /// <summary>
        /// 通过银行卡号
        /// </summary>
        /// <param name="card_no">卡号</param>
        /// <param name="trans_date">交易日期</param>
        /// <param name="client_ip">IP</param>
        public DataTable QueryOrderByCardNo(string card_no, DateTime trans_date, string client_ip)
        {
            try
            {
                var req = "card_no=" + card_no + "&trans_date=" + trans_date.ToString("yyyy-MM-dd") + "&client_ip=" + client_ip;
                var result = RelayAccessFactory.RelayInvoke(req, "101405", true, false, ip, port, "utf-8");
                return TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.ParseRelayStringToDataTable(result, "row_num", "row_info");
            }
            catch (Exception ex)
            {
                throw new Exception("通过银行卡号出错:" + ex.Message);
            }
        }



        /// <summary>
        ///  通过银行订单号查询卡信息
        /// </summary>
        /// <param name="bank_type">银行类型</param>
        /// <param name="bill_no">银行订单号</param>
        /// <returns></returns>
        public DataTable QueryCardType(string bank_type, string bill_no)
        {
            try
            {
                var req = "bank_type=" + bank_type + "&bill_no=" + bill_no + "&biz_type=10100";
                var result = RelayAccessFactory.RelayInvoke(req, "101610", true, false, ip, port, "utf-8");
                return ParseRelayOneRow(result, "通过银行订单号查询卡信息");
            }
            catch (Exception ex)
            {
                throw new Exception("通过银行卡号出错:" + ex.Message);
            }
        }
        #endregion

        #region 四、账户资金和流水查询（新增）


        /// <summary>
        /// 交易单和退款单查询
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="list_type">交易类型:[101：B2C订单、102：退款单、103：提现单、104：充值单、108：转账单]</param>
        /// <param name="offset"></param>       
        /// <param name="limit"></param>
        /// <param name="client_ip">IP</param>
        /// <returns></returns>
        public DataTable QueryTradeInfo(string uid, string list_type, string stime, string etime, int offset, int limit, string client_ip)
        {
            try
            {
                var req =
                    "uid=" + uid +
                    "&list_type=" + list_type +
                    "&limit=" + limit.ToString() +
                    "&begin_time=" + stime +
                    "&end_time=" + etime +
                    "&offset=" + offset.ToString() +
                    "&client_ip=" + client_ip
                    ;
                var result = RelayAccessFactory.RelayInvoke(req, "101101", true, false, ip, port, "utf-8");
                return TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.ParseRelayStringToDataTable(result, "row_num", "row_info");
            }
            catch (Exception ex)
            {
                throw new Exception("交易单和退款单查询:" + ex.Message);
            }
        }

        /// <summary>
        /// 提现
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="client_ip"></param>
        /// <returns></returns>
        public DataTable QueryFetchInfo(string uid, string stime, string etime, int offset, int limit, string client_ip,string coding="")
        {
            try
            {
                string reqText = "begin_time={0}&end_time={1}&client_ip={2}&limit={3}&offset={4}&uid={5}&MSG_NO={6}";
                //reqText = "begin_time=2015-06-01&client_ip=127.0.0.1&end_time=2015-12-10&limit=2&offset=0&uid=600000212&MSG_NO=101771";
                reqText = string.Format(reqText, stime, etime, client_ip, limit, offset, uid, "101771" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                string result = RelayAccessFactory.RelayInvoke(reqText, "101771", true, false, ip, port, coding);
//#if DEBUG
//                result = "result=0&res_info=ok&row_num=2&row_info=listid_0%3d1030033000000000201505130301789324%26fetch_type_0%3d3%26subject_0%3d14%26num_0%3d100%26charge_0%3d0%26bank_acno_0%3d%26fetch_state_0%3d2%26user_uin_0%3d085e9858ed8ed3aa9a95e4252@wx.tenpay.com%26memo_0%3d%26bank_name_0%3d????%26acc_name_0%3dtest test%26card_bankid_0%3d552037******2578%26pay_time_0%3d0000-00-00 00:00:00%26acc_time_0%3d2015-05-13 11:51:24%26create_time_0%3d2015-06-01 10:00:00%26modify_time_0%3d2015-06-02 10:00:00%26curtype_0%3d344%26listid_1%3d1030033000000000201505130301789325%26fetch_type_1%3d3%26subject_1%3d14%26num_1%3d100%26charge_1%3d0%26bank_acno_1%3d%26fetch_state_1%3d2%26user_uin_1%3d085e9858ed8ed3aa9a95e4252@wx.tenpay.com%26memo_1%3d%26bank_name_1%3d????%26acc_name_1%3dtest test%26card_bankid_1%3d552037******2578%26pay_time_1%3d0000-00-00 00:00:00%26acc_time_1%3d2015-05-13 11:51:24%26create_time_1%3d2015-06-01 10:00:00%26modify_time_1%3d2015-06-02 10:00:00%26curtype_1%3d344";
//                result = "result=0&res_info=ok&row_num=7&row_info=listid_0%3d1030033000000000201601190809756815%26fetch_type_0%3d3%26subject_0%3d14%26num_0%3d1%26charge_0%3d0%26bank_acno_0%3d%26fetch_state_0%3d4%26user_uin_0%3do5PXlsgKtAaiZF070QuOM7Iuq3WY%2540wx.hkg%26memo_0%3dwithdraw%26bank_name_0%3d%25E4%25B8%25AD%25E5%259C%258B%25E9%258A%2580%25E8%25A1%258C%25EF%25BC%2588%25E9%25A6%2599%25E6%25B8%25AF%25EF%25BC%2589%26acc_name_0%3dTAOLI+HONG%26card_bankid_0%3d897103*0172%26pay_time_0%3d0000-00-00+00%253A00%253A00%26acc_time_0%3d2016-01-19+15%253A32%253A28%26create_time_0%3d2016-01-19+15%253A32%253A28%26modify_time_0%3d2016-01-20+09%253A33%253A45%26curtype_0%3d344%26listid_1%3d1030033000000000201601130788328940%26fetch_type_1%3d3%26subject_1%3d14%26num_1%3d1%26charge_1%3d0%26bank_acno_1%3d%26fetch_state_1%3d4%26user_uin_1%3do5PXlsgKtAaiZF070QuOM7Iuq3WY%2540wx.hkg%26memo_1%3dwithdraw%26bank_name_1%3d%25E4%25B8%25AD%25E5%259C%258B%25E9%258A%2580%25E8%25A1%258C%25EF%25BC%2588%25E9%25A6%2599%25E6%25B8%25AF%25EF%25BC%2589%26acc_name_1%3dTAOLI+HONG%26card_bankid_1%3d897103*0172%26pay_time_1%3d0000-00-00+00%253A00%253A00%26acc_time_1%3d2016-01-13+15%253A06%253A32%26create_time_1%3d2016-01-13+15%253A06%253A32%26modify_time_1%3d2016-01-14+10%253A20%253A52%26curtype_1%3d344%26listid_2%3d1030023000000101201601110000591197%26fetch_type_2%3d4%26subject_2%3d14%26num_2%3d100%26charge_2%3d0%26bank_acno_2%3d000000004504%26fetch_state_2%3d5%26user_uin_2%3do5PXlsgKtAaiZF070QuOM7Iuq3WY%2540wx.hkg%26memo_2%3dMD+HongBao+Refund%26bank_name_2%3d%26acc_name_2%3d%26card_bankid_2%3d%26pay_time_2%3d2016-01-11+17%253A00%253A01%26acc_time_2%3d2016-01-11+17%253A00%253A01%26create_time_2%3d2016-01-11+17%253A00%253A01%26modify_time_2%3d2016-01-11+17%253A00%253A01%26curtype_2%3d344%26listid_3%3d1030033000000000201601110781654922%26fetch_type_3%3d3%26subject_3%3d14%26num_3%3d1%26charge_3%3d0%26bank_acno_3%3d000103444505%26fetch_state_3%3d6%26user_uin_3%3do5PXlsgKtAaiZF070QuOM7Iuq3WY%2540wx.hkg%26memo_3%3d%26bank_name_3%3d%25E4%25B8%25AD%25E5%259C%258B%25E9%258A%2580%25E8%25A1%258C%25EF%25BC%2588%25E9%25A6%2599%25E6%25B8%25AF%25EF%25BC%2589%26acc_name_3%3dTAOLI+HONG%26card_bankid_3%3d897103*0172%26pay_time_3%3d2016-01-14+10%253A20%253A52%26acc_time_3%3d2016-01-14+10%253A20%253A52%26create_time_3%3d2016-01-11+14%253A23%253A14%26modify_time_3%3d2016-01-14+10%253A20%253A52%26curtype_3%3d344%26listid_4%3d1030033000000000201601080774173416%26fetch_type_4%3d3%26subject_4%3d14%26num_4%3d100%26charge_4%3d0%26bank_acno_4%3d000103444504%26fetch_state_4%3d6%26user_uin_4%3do5PXlsgKtAaiZF070QuOM7Iuq3WY%2540wx.hkg%26memo_4%3d%26bank_name_4%3d%25E4%25B8%25AD%25E5%259C%258B%25E9%258A%2580%25E8%25A1%258C%25EF%25BC%2588%25E9%25A6%2599%25E6%25B8%25AF%25EF%25BC%2589%26acc_name_4%3dTAOLI+HONG%26card_bankid_4%3d897103*0172%26pay_time_4%3d2016-01-12+11%253A04%253A01%26acc_time_4%3d2016-01-12+11%253A04%253A01%26create_time_4%3d2016-01-08+20%253A06%253A28%26modify_time_4%3d2016-01-12+11%253A04%253A01%26curtype_4%3d344%26listid_5%3d1030033000000000201601080773798145%26fetch_type_5%3d3%26subject_5%3d14%26num_5%3d100%26charge_5%3d0%26bank_acno_5%3d000103444504%26fetch_state_5%3d6%26user_uin_5%3do5PXlsgKtAaiZF070QuOM7Iuq3WY%2540wx.hkg%26memo_5%3d%26bank_name_5%3d%25E4%25B8%25AD%25E5%259C%258B%25E9%258A%2580%25E8%25A1%258C%25EF%25BC%2588%25E9%25A6%2599%25E6%25B8%25AF%25EF%25BC%2589%26acc_name_5%3dTAOLI+HONG%26card_bankid_5%3d897103*0172%26pay_time_5%3d2016-01-12+11%253A04%253A01%26acc_time_5%3d2016-01-12+11%253A04%253A01%26create_time_5%3d2016-01-08+18%253A11%253A27%26modify_time_5%3d2016-01-12+11%253A04%253A01%26curtype_5%3d344%26listid_6%3d1030033000000000201601080773492441%26fetch_type_6%3d3%26subject_6%3d14%26num_6%3d100%26charge_6%3d0%26bank_acno_6%3d000103444504%26fetch_state_6%3d6%26user_uin_6%3do5PXlsgKtAaiZF070QuOM7Iuq3WY%2540wx.hkg%26memo_6%3d%26bank_name_6%3d%25E4%25B8%25AD%25E5%259C%258B%25E9%258A%2580%25E8%25A1%258C%25EF%25BC%2588%25E9%25A6%2599%25E6%25B8%25AF%25EF%25BC%2589%26acc_name_6%3dTAOLI+HONG%26card_bankid_6%3d897103*0172%26pay_time_6%3d2016-01-12+11%253A04%253A01%26acc_time_6%3d2016-01-12+11%253A04%253A01%26create_time_6%3d2016-01-08+17%253A14%253A38%26modify_time_6%3d2016-01-12+11%253A04%253A01%26curtype_6%3d344";
//#endif
                return TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.ParseRelayStringToDataTable1(result, "utf-8");//GB2312
            }
            catch (Exception ex)
            {
                throw new Exception("提现:" + ex.Message);
            }
        }
        /// <summary>
        /// 资金流水
        /// </summary>
        /// <param name="uin"></param>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="client_ip"></param>
        /// <returns></returns>

        public DataTable QueryBankrollList(string uid, string stime, string etime, int offset, int limit, string client_ip)
        {
            try
            {
                DataTable dtuser = QueryUserInfo(uid, client_ip);
                if (dtuser == null || dtuser.Rows.Count == 0)
                {
                    throw new Exception("uid转uin失败！");
                }
                string uin = dtuser.Rows[0]["uin"].ToString();

                string reqText = "begin_time={0}&end_time={1}&client_ip={2}&limit={3}&offset={4}&uin={5}&MSG_NO={6}";
//#if DEBUG
//                reqText = "begin_time=2015-06-01&client_ip=127.0.0.1&end_time=2015-12-10&limit=2&offset=0&uin=o5PXlsm0qtXfIA2bLkoMoeXHT51c@wx.hkg&MSG_NO=101109123456";
//#endif
                reqText = string.Format(reqText, stime, etime, client_ip, limit, offset, uin, "101109" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                string result = RelayAccessFactory.RelayInvoke(reqText, "101109", true, false, ip, port);
//#if DEBUG
//                result = "result=0&res_info=ok&row_num=3&row_info=bkid_0=12&listid_0=1010014000000021201512090000060882&trade_time_0=2015-12-09 20:34:13&type_0=2&subject_0=9&paynum_0=100&balance_0=999000&memo_0= &con_0=0&connum_0=0&vs_info_0=100000100002&modify_time_0=2015-12-09 20:34:13&bkid_1=11&listid_1=1010014000000021201512090000060879&trade_time_1=2015-12-09 20:31:45&type_1=2&subject_1=9&paynum_1=100&balance_1=999100&memo_1=&con_1=0&connum_1=0&vs_info_1=100000100002&modify_time_1=2015-12-09 20:31:45&bkid_2=10&listid_2=1010014000000021201512090000060874&trade_time_2=2015-12-09 20:28:50&type_2=2&subject_2=9&paynum_2=100&balance_2=999200&memo_2=&con_2=0&connum_2=0&vs_info_2=100000100002&modify_time_2=2015-12-09 20:28:50";
//#endif
                return TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.ParseRelayStringToDataTable1(result, "utf-8");//GB2312
            }
            catch (Exception ex)
            {
                throw new Exception("资金流水:" + ex.Message);
            }
        }

        /// <summary>
        /// HK钱包支付---收红包记录查询
        /// </summary>
        public List<HKWalletReceivePackageModel> QueryReceivePackageList(string uid, string stime, string etime, int offset, int limit, string client_ip)
        {
            try
            {
                var req =
                    "openid=" + uid +
                    "&begin_time=" + stime +
                    "&end_time=" + etime +
                    "&offset=" + offset.ToString() +
                    "&limit=" + limit.ToString();

                var result = RelayAccessFactory.RelayInvoke(req, "101788", true, false, IP_RedPacket, Port_RedPacket, RedPacket_Encode);

                var retDataList = new List<HKWalletReceivePackageModel>();

                var resultJson = TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.GetRelayParams(result, RedPacket_Encode, "res_info");

                var resultModel = Newtonsoft.Json.JsonConvert.DeserializeObject<HKWalletrecvModel<HKWalletReceivePackageModel>>(resultJson);
                if (resultModel != null && resultModel.ret_num> 0)
                {
                    var recvData = resultModel.recv_hb_list;
                    retDataList = (recvData != null && recvData.Count > 0) ? recvData : null;
                }

                return retDataList;
            }
            catch (Exception ex)
            {
                throw new Exception(" HK钱包支付,收红包记录查询,异常:" + ex);
            }
        }

        /// <summary>
        /// HK钱包支付---发红包记录查询
        /// </summary>
        public List<HKWalletSendPackageModel> QuerySendPackageList(string uid, string stime, string etime, int offset, int limit, string client_ip)
        {
            try
            {
                var req =
                    "openid=" + uid +
                    "&begin_time=" + stime +
                    "&end_time=" + etime +
                    "&offset=" + offset.ToString() +
                    "&limit=" + limit.ToString();

                var result = RelayAccessFactory.RelayInvoke(req, "101787", true, false, IP_RedPacket, Port_RedPacket, RedPacket_Encode);

                var retDataList = new List<HKWalletSendPackageModel>();

                var resultJson = TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.GetRelayParams(result, RedPacket_Encode, "res_info");

                var resultModel = Newtonsoft.Json.JsonConvert.DeserializeObject<HKWalletSendModel<HKWalletSendPackageModel>>(resultJson);
                if (resultModel != null && resultModel.ret_num > 0)
                {
                    var recvData = resultModel.send_hb_list;
                    retDataList = (recvData != null && recvData.Count > 0) ? recvData : null;
                }

                return retDataList;
            }
            catch (Exception ex)
            {
                throw new Exception("HK钱包支付,发红包记录查询,异常:" + ex);
            }
        }

        /// <summary>
        /// HK钱包支付---红包详情查询
        /// </summary>
        /// <param name="typeid">1,根据发红包单号来查询 2.根据收红包单号来查询 </param>
        /// <param name="listid">发红包单号/收红包单号</param>
        /// <param name="qry_time">收红包或发红包时的时间  以YYYY-MM-DD格式  Type=2时必传</param>
        /// <param name="client_ip"></param>
        /// <returns></returns>
        public HKWalletDetailItem QueryHKPackageDetail(int typeid, string listid, string qry_time, string client_ip)
        {
            try
            {
                var req =
                    "type=" + typeid +
                    "&listid=" + listid;
                if (typeid == 2) {
                    req += "&qry_time=" + qry_time;
                }

                var result = RelayAccessFactory.RelayInvoke(req, "101786", true, false, IP_RedPacket, Port_RedPacket, RedPacket_Encode);
                
                var retDataList = new HKWalletDetailItem();

                var resultJson = TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.GetRelayParams(result, RedPacket_Encode, "res_info");
                retDataList = Newtonsoft.Json.JsonConvert.DeserializeObject<HKWalletDetailItem>(resultJson);

                return retDataList;
            }
            catch (Exception ex)
            {
                throw new Exception("HK钱包支付---红包详情查询,异常:" + ex);
            }
        } 
        /// <summary>
        /// 香港钱包 转账流水
        /// </summary>
        public List<HKWalletTransRollList> QueryHKTransRollList(string client_ip, string query_openid, string trans_type, string start_time, string end_time, int offset, int limit)
        {

            //&client_ip=127.0.0.1&end_time=2016-06-30&limit=10&offset=0&openid=o5PXlsmW40pXioztjs1BvqNPXCdQ&start_time=2016-01-01&trans_type=1&abstract=XEqsYg_vvlzl4yJBLt7t5OQbSKQ%3d
            string requesttest = "client_ip=" + client_ip +
                                "&openid=" + query_openid +
                                "&trans_type=" + trans_type +
                                "&start_time=" + start_time +
                                "&end_time=" + end_time +
                                "&offset=" + offset +
                                "&limit=" + limit +
                                "&MSG_NO=102369" + DateTime.Now.ToString("yyyyMMddHHmmss");

            var result = RelayAccessFactory.RelayInvoke(requesttest, "102369", true, false, IP_RedPacket, Port_RedPacket, RedPacket_Encode);
            if (!result.Contains("result=0"))
            {
                throw new Exception("HK钱包支付---转账流水查询,异常:requesttest:" + requesttest + "result:" + result);
            }
            string resultJson = TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.GetRelayParams(result, RedPacket_Encode, "res_info");
            var DynamicObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(resultJson);
            //rownum不是总条数，没什么作用
            string rownum = Convert.ToString(DynamicObject.rownum);

            string rows = Convert.ToString(DynamicObject.rows);
            List<HKWalletTransRollList> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<HKWalletTransRollList>>(rows);
            return list;
        }

        #endregion

        #region 六、外币商户查询(新增)

        /// <summary>
        /// 外币商户查询
        /// </summary>
        /// <param name="client_ip">Ip</param>
        /// <param name="uid">商户id</param>
        /// <param name="spid">商户号</param>
        /// <returns></returns>
        public DataTable QueryMerinfo(string client_ip, string uid = null, string spid = null)
        {
            string req = "client_ip=" + client_ip;
            if (uid != null && spid != null)
            {
                throw new ArgumentException("uid/spid 两个参数只能二选一");
            }
            if (uid != null)
            {
                req += "&uid=" + uid;
            }
            if (spid != null)
            {
                req += "&spid=" + spid;
            }
            var result = RelayAccessFactory.RelayInvoke(req, "8515", false, false, ip, port, "utf-8");
            return ParseRelayOneRow(result, "外币商户查询");
        }
        #endregion

        #region 七、实名信息
        public DataTable QueryRealNameInfo(string uin, string client_ip)
        {

            string req = "uin=" + uin + "&client_ip=" + client_ip;

            var result = RelayAccessFactory.RelayInvoke(req, "102224", true, false, ip, port, "utf-8");

            if (!result.Contains("result=0&res_info=ok"))
            {
                throw new Exception("请求串：" + req + "返回串：" + result);
            }

            Dictionary<string, string> dic = result.ToDictionary('&', '=');

            DataTable dt = new DataTable();
            foreach (string key in dic.Keys)
            {
                dt.Columns.Add(key, typeof(string));
            }
            DataRow dr = dt.NewRow();
            foreach (string key in dic.Keys)
            {
                dr[key] = dic[key];
            }
            dt.Rows.Add(dr);
            return dt;

        }

        public DataTable QueryRealNameInfo2(string uin, string type, string state, DateTime? start_time, DateTime? end_time, string client_ip,int offset,int limit)
        {
            string req = "client_ip=" + client_ip + "&offset=" + offset + "&limit=" + limit;

            if (!string.IsNullOrEmpty(uin))
            {
                req += "&uin=" + uin;
            }
            if (!string.IsNullOrEmpty(type))
            {
                req += "&type=" + type;
            }
            if (!string.IsNullOrEmpty(state))
            {
                req += "&state=" + state;
            }
            if (start_time.HasValue)
            {
                string stime = start_time.Value.ToString("yyyy-MM-dd");
                req += "&start_time=" + stime;
            }
            if (end_time.HasValue)
            {
                string etime = end_time.Value.ToString("yyyy-MM-dd");
                req += "&end_time=" + etime;
            }
            var result = RelayAccessFactory.RelayInvoke(req, "102241", true, false, ip, port, "utf-8");
            #region
            if (!result.Contains("result=0&res_info=ok"))
            {
                throw new Exception("请求串：" + req + "返回串：" + result);
            }

            Dictionary<string, string> dic = result.ToDictionary('&', '=');
            int row_num = Convert.ToInt32(dic["row_num"]);
            if (row_num == 0)
            {
                throw new Exception("查询结果为空！");
            }
            DataTable dt = new DataTable();

            string row1 = System.Web.HttpUtility.UrlDecode(dic["row1"], System.Text.Encoding.GetEncoding("utf-8"));
            Dictionary<string, string> dic_Colums = row1.ToDictionary('&', '=');
            foreach (string key in dic_Colums.Keys)
            {
                dt.Columns.Add(key, typeof(string));
            }

            foreach (string key_row in dic.Keys)
            {
                if (key_row.StartsWith("row") && key_row != "row_num")
                {
                    DataRow dr = dt.NewRow();
                    string row_x = System.Web.HttpUtility.UrlDecode(dic[key_row], System.Text.Encoding.GetEncoding("utf-8"));
                    Dictionary<string, string> dic_row = row_x.ToDictionary('&', '=');
                    foreach (string key_col in dic_Colums.Keys)
                    {
                        try
                        {
                            dr[key_col] = dic_row[key_col];
                        }
                        catch(Exception ex)
                        {
                            throw new Exception(key_col + key_row);
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }
            #endregion
            return dt;

        }

        public string checkRealName(string Operator, string uin, string approval_id, string state, string memo, string client_ip)
        {
            string req = "client_ip=" + client_ip +
                "&uin=" + uin +
                "&op_name=" + Operator +
                "&approval_id=" + approval_id +
                "&state=" + state +
                "&memo=" + memo.Trim();


            string result = RelayAccessFactory.RelayInvoke(req, "102225", true, false, ip, port, "utf-8");
            if (result.Contains("result=0&res_info=ok"))
            {
                return "0";
            }
            else
            {
                throw new Exception("请求串：" + req + "返回串" + result);
            }
        }


        public List<HKWalletRealNameCheckModel> New_QueryRealNameInfo2(string uin, string type, string state, DateTime? start_time, DateTime? end_time, string client_ip, int offset, int limit)
        {
            string req = "client_ip=" + client_ip + "&offset=" + offset + "&limit=" + limit;

            if (!string.IsNullOrEmpty(uin))
            {
                req += "&uin=" + uin;
            }
            if (!string.IsNullOrEmpty(type))
            {
                req += "&type=" + type;
            }
            if (!string.IsNullOrEmpty(state))
            {
                req += "&state=" + state;
            }
            if (start_time.HasValue)
            {
                string stime = start_time.Value.ToString("yyyy-MM-dd");
                req += "&start_time=" + stime;
            }
            if (end_time.HasValue)
            {
                string etime = end_time.Value.ToString("yyyy-MM-dd");
                req += "&end_time=" + etime;
            }
            var result = RelayAccessFactory.RelayInvoke(req, "102241", true, false, ip, port, "utf-8");
            if (!result.Contains("result=0&res_info=ok"))
            {
                throw new Exception("请求串：" + req + "返回串：" + result);
            }
            List<HKWalletRealNameCheckModel> list = ToList<HKWalletRealNameCheckModel>(result);
            return list;
        }

        public List<T> ToList<T>(string result) where T : class
        {
            Dictionary<string, string> dic = result.ToDictionary('&', '=');
            int row_num = Convert.ToInt32(dic["row_num"]);
            if (row_num == 0)
            {
                throw new Exception("查询结果为空！");
            }
            List<T> list = new List<T>();
            Type type = typeof(T);

            foreach (string key_row in dic.Keys)
            {
                if (key_row.StartsWith("row") && key_row != "row_num")
                {
                    string rows = System.Web.HttpUtility.UrlDecode(dic[key_row], System.Text.Encoding.GetEncoding("utf-8"));
                    Dictionary<string, string> dic_rows = rows.ToDictionary('&', '=');
                    object obj = Activator.CreateInstance(type);
                    foreach (string key_col in dic_rows.Keys)
                    {
                        System.Reflection.PropertyInfo Property = type.GetProperties().Where(p => p.Name.ToLower() == key_col.ToLower()).FirstOrDefault();
                        if (Property != null)
                        {
                            Property.SetValue(obj, dic_rows[key_col], null);
                        }
                    }
                    list.Add(obj as T);
                }
            }

            return list;
        }

        #endregion

        #region 日记
        /// <summary>
        /// 日记查询 - 重置
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"> limit 只能是10   接口限定的</param>
        /// <param name="uin">用户uin</param>
        /// <param name="uid">用户uid</param>
        /// <param name="op_name">操作员</param>
        /// <param name="channel">冻结渠道</param>  
        /// <param name="start_time">起始时间</param>
        /// <param name="end_time">截至时间</param>
        /// <returns></returns>
        public DataTable QueryResetPassWordLog(Dictionary<string, string> parameters, int offset, int limit)
        {
            parameters.Add("CMD", "RESET_LOG_QUERY");
            parameters.Add("offset", offset.ToString());
            parameters.Add("limit", limit.ToString());
            return QueryLog(parameters);
        }

        /// <summary>
        /// 日记查询 - 冻结
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"> limit 只能是10   接口限定的</param>
        /// <param name="uin">用户uin</param>
        /// <param name="uid">用户uid</param>
        /// <param name="op_name">操作员</param>
        /// <param name="channel">冻结渠道</param>  
        /// <param name="start_time">起始时间</param>
        /// <param name="end_time">截至时间</param>
        /// <returns></returns>
        public DataTable QueryFreezeLog(Dictionary<string, string> parameters, int offset, int limit)
        {
            parameters.Add("CMD", "FREEZE_LOG_QUERY");
            parameters.Add("offset", offset.ToString());
            parameters.Add("limit", limit.ToString());
            return QueryLog(parameters);
        }

        /// <summary>
        /// 日记查询 - 解绑卡日志查询
        /// </summary>
        /// <param name="uid">用户uid</param>
        /// <param name="bind_serialno">绑卡序列号</param>
        /// <param name="offset"></param>
        /// <param name="limit"> limit 只能是10   接口限定的</param>
        /// <returns></returns>
        public DataTable QueryBindCardLog(string uid, string bind_serialno, int offset, int limit)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>() 
            {
                {"CMD", "UNBIND_LOG_QUERY"},
                {"offset", offset.ToString()},
                {"limit", limit.ToString()},
                {"uid",uid},
                {"bind_serialno",bind_serialno},
            };
            return QueryLog(parameters);
        }

        /// <summary>
        /// 日记查询  
        /// CMD=[解绑卡日志查询接口:UNBIND_LOG_QUERY | 冻结日志查询接口:FREEZE_LOG_QUERY | 重置日志查询接口:RESET_LOG_QUERY]
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected DataTable QueryLog(Dictionary<string, string> parameters)
        {
            try
            {
                var req = ParseParameterString(parameters);
                var result = RelayAccessFactory.RelayInvoke(req, "8514", false, false, ip, port, "utf-8");
                string msg = "";
                var ds = TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.GetDataSetFromReply(result, out msg);
                if (msg != "")
                {
                    throw new Exception(msg);
                }
                if (ds != null && ds.Tables.Count > 0)
                    return ds.Tables[0];
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("查询出错:" + ex.Message);
            }
        }
        #endregion

        #region 公共方法

        /// <summary>
        /// 单行返回值解析成Table
        /// </summary>
        /// <param name="str">返回值</param>
        /// <param name="memo">值说明</param>
        /// <returns></returns>
        protected DataTable ParseRelayOneRow(string result, string memo)
        {
            string msg = "";
            var ds = TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.ParseRelayStr(result, out msg, false);
            if (msg != "")
            {
                throw new Exception(memo + "出错:" + msg);
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }

        /// <summary>
        /// 把一个 key Value 集合转换成请求参数
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected string ParseParameterString(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            var length = parameters.Count();
            if (length == 0)
            {
                return string.Empty;
            }
            var buff = new System.Text.StringBuilder(length * 20);
            var Enumerator = parameters.GetEnumerator();
            if (Enumerator.MoveNext())
            {
                var cur = Enumerator.Current;
                buff.AppendFormat("{0}={1}", cur.Key, cur.Value);
            }
            while (Enumerator.MoveNext())
            {
                var cur = Enumerator.Current;
                buff.AppendFormat("&{0}={1}", cur.Key, cur.Value);
            }
            return buff.ToString();
        }


        /// <summary>
        /// 字符转换编码
        /// </summary>
        /// <param name="str">要转码的字符串</param>
        /// <param name="coding">本身编码</param>
        /// <param name="toCodding">转换后编码</param>
        /// <returns></returns>
        protected string StringTransCode(string str, string srcCode = null, string toCode = "utf-8")
        {
            string result = string.Empty;
            try
            {
                if (string.IsNullOrWhiteSpace(str))
                    return str;

                //var srcEncoding = srcCode == null ? System.Text.Encoding.Default : System.Text.Encoding.GetEncoding(srcCode); //此处编码和解码会出现乱码，编码和解码要对应
                var srcEncoding = srcCode == null ? System.Text.Encoding.GetEncoding(srcCode) : System.Text.Encoding.GetEncoding(srcCode);
                var toEncoding = System.Text.Encoding.GetEncoding(toCode);

                var buff = srcEncoding.GetBytes(str);
                var tobuff = System.Text.Encoding.Convert(srcEncoding, toEncoding, buff);
                result = srcEncoding.GetString(tobuff);
            }
            catch (Exception ex)
            {
                result = string.Empty;
                LogHelper.LogInfo(string.Format("101404;StringTransCode:{0}"), ex.Message.ToString());
            }
            return result;
            
        }
        #endregion

    }
}
