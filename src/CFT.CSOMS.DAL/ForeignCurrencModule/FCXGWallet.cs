﻿using CFT.CSOMS.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SunLibraryEX;

namespace CFT.CSOMS.DAL.ForeignCurrencModule
{
    /// <summary>
    /// 香港钱包相关
    /// </summary>
    public class FCXGWallet
    {
        string ip = Apollo.Common.Configuration.AppSettings.Get<string>("FCXGWallet_IP", "10.12.62.107");//10.12.189.88 新:10.12.62.107  线上10.192.100.115
        int port = Apollo.Common.Configuration.AppSettings.Get<int>("FCXGWallet_Port", 22000);

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
            //result=0&address=&area=&city=&company_name=&country=&cre_id=&cre_type=0&create_time=2015-10-22 17:50:38&email=&gender=0&lstate=2&memo=&mobile=&modify_time=2016-01-04 14:07:32&openid=&phone=&postal_code=&state=3&true_name=&uid=600000347&uin=o5PXlsm0qtXfIA2bLkoMoeXHT51c@wx.hkg&user_type=2&MSG_NO=10014519827780000000042
            DataTable dt = ParseRelayOneRow(result, "查询账户基本信息");

            //查询账户余额
            string reqText = "uid={0}&client_ip={1}&cur_type=344&acc_type=2&MSG_NO={2}";
            reqText = string.Format(reqText, uid, client_ip, "107831" + DateTime.Now.ToString("yyyyMMddHHmmss"));
            result = RelayAccessFactory.RelayInvoke(reqText, "107831", false, false, ip, port, "utf-8");
            //result=0&res_info=ok&uid=600000347&cur_type=344&balance=997700&MSG_NO=10783120160104190031

            DataTable dt_userbalance = ParseRelayOneRow(result, "查询账户余额");

            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Add("balance", typeof(string));//用户余额
                string balance = (dt_userbalance != null && dt_userbalance.Rows.Count > 0) ? dt_userbalance.Rows[0]["balance"].ToString() : "0";
                //港币分转元
                balance = MoneyTransfer.FenToYuan(balance, "HKD");
                dt.Rows[0]["balance"] = balance;
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
            var relay_result = RelayAccessFactory.RelayInvoke(StringTransCode(req), "101404", true, false, ip, port, "utf-8");
            var dic = relay_result.ToDictionary();
            if (dic["result"] == "0")
            {
                return true;
            }
            else
            {
                throw new Exception("重置密码失败:" + relay_result);
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
        public DataTable QueryTradeInfo(string uid, string list_type,string stime,string etime, int offset, int limit, string client_ip)
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
        public DataTable QueryFetchInfo(string uid, string stime, string etime, int offset, int limit, string client_ip)
        {
            try
            {
                string reqText = "begin_time={0}&end_time={1}&client_ip={2}&limit={3}&offset={4}&uid={5}&MSG_NO={6}";
               //reqText = "begin_time=2015-06-01&client_ip=127.0.0.1&end_time=2015-12-10&limit=2&offset=0&uid=600000212&MSG_NO=108197";
                reqText = string.Format(reqText, stime, etime, client_ip, limit, offset, uid, "108197" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                string result = RelayAccessFactory.RelayInvoke(reqText, "108197", true, false, ip, port);
//#if DEBUG
//                result = "result=0&res_info=ok&row_num=2&row_info=listid_0%3d1030033000000000201505130301789324%26fetch_type_0%3d3%26subject_0%3d14%26num_0%3d100%26charge_0%3d0%26bank_acno_0%3d%26fetch_state_0%3d2%26user_uin_0%3d085e9858ed8ed3aa9a95e4252@wx.tenpay.com%26memo_0%3d%26bank_name_0%3d????%26acc_name_0%3dtest test%26card_bankid_0%3d552037******2578%26pay_time_0%3d0000-00-00 00:00:00%26acc_time_0%3d2015-05-13 11:51:24%26create_time_0%3d2015-06-01 10:00:00%26modify_time_0%3d2015-06-02 10:00:00%26curtype_0%3d344%26listid_1%3d1030033000000000201505130301789325%26fetch_type_1%3d3%26subject_1%3d14%26num_1%3d100%26charge_1%3d0%26bank_acno_1%3d%26fetch_state_1%3d2%26user_uin_1%3d085e9858ed8ed3aa9a95e4252@wx.tenpay.com%26memo_1%3d%26bank_name_1%3d????%26acc_name_1%3dtest test%26card_bankid_1%3d552037******2578%26pay_time_1%3d0000-00-00 00:00:00%26acc_time_1%3d2015-05-13 11:51:24%26create_time_1%3d2015-06-01 10:00:00%26modify_time_1%3d2015-06-02 10:00:00%26curtype_1%3d344";
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
                //reqText = "begin_time=2015-06-01&client_ip=127.0.0.1&end_time=2015-12-10&limit=2&offset=0&uin=o5PXlsm0qtXfIA2bLkoMoeXHT51c@wx.hkg&MSG_NO=101109123456";
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
            if (string.IsNullOrWhiteSpace(str))
                return str;

            var srcEncoding = srcCode == null ? System.Text.Encoding.Default : System.Text.Encoding.GetEncoding(srcCode);
            var toEncoding = System.Text.Encoding.GetEncoding(toCode);

            var buff = srcEncoding.GetBytes(str);
            var tobuff = System.Text.Encoding.Convert(srcEncoding, toEncoding, buff);
            return srcEncoding.GetString(tobuff);
        }
        #endregion
    }
}
