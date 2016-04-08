using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CFT.CSOMS.COMMLIB;
using System.Collections;
using CFT.CSOMS.DAL.Infrastructure;
using TENCENT.OSS.C2C.Finance.BankLib;
using commLib.Entity;

namespace CFT.CSOMS.BLL.ForeignCurrencyModule
{
    public class FCXGWallet
    {
        CFT.CSOMS.DAL.ForeignCurrencModule.FCXGWallet dal = new DAL.ForeignCurrencModule.FCXGWallet();

        //用户物理状态:  DicLstate
        //用户类型:     DicUserType
        //交易类型:     DicTradeType
        //交易状态:     DicTradeState
        //交易卡种:     DicCardType
        //交易单状态:   ListState
        //绑卡状态:     DicBindStatus
        //用户注册状态： DicUserRegState 
        //渠道：        DicChannel
        //币种:        CurType
        Dictionary<string, string> DicLstate, DicTradeType, DicUserType, DicTradeState, DicCardType, DicAppealSign, DicBindStatus, DicUserRegState, DicChannel, CurType;

        public FCXGWallet()
        {
            DicLstate = new Dictionary<string, string>()
            {
                {"1","正常"},
                {"2","冻结"},
                {"3","作废"}
            };

            DicUserType = new Dictionary<string, string>()
            {
                {"1","普通用户"},
                {"2","微信用户"}
            };

            DicTradeType = new Dictionary<string, string>() 
            { 
                { "2", "B2C" },
                { "3", "Fastpay" },
                { "20", "DCC交易" }
            };

            DicTradeState = new Dictionary<string, string>() 
            {
                { "1", "等待买家支付" },
                { "2", "支付成功" },
                { "7", "转入退款" },
                { "99", "交易关闭" }
            };

            DicCardType = new Dictionary<string, string>() 
            {
                { "1", "MasterCard" },
                { "2", "Visa" },
                { "3", "JCB" },
                { "4", "AE" },
                { "5", "DinnerCard" }
            };

            DicAppealSign = new Dictionary<string, string>()
            {
                {"1","正常"},
                {"2","已转申诉"}
            };


            DicBindStatus = new Dictionary<string, string>()
            {
                { "0" ,"未定义"},
                { "1" ,"初始状态"},
                { "2" ,"开启"},
                { "3" ,"关闭"},//（支付关闭，可以提现）
                { "4" ,"解除"},//【最终状态】
            };

            DicUserRegState = new Dictionary<string, string>() 
            {
                 {"1",  "初始状态"},
                 {"2",  "简化注册完成"},//；【没有密码】
                 {"3",  "注册完成"},//【有密码
            };

            CurType = new Dictionary<string, string>() 
            {
                { "344", "HKD" },
                { "156", "CUY" },
                { "392", "JPY" },
                { "840", "USD" }
            };

            DicChannel = new Dictionary<string, string>()
            {
                 {"1",  "风控冻结"},
                 {"2",  "拍拍冻结"},
                 {"3",  "用户冻结"},
                 {"4",  "商户冻结"},
                 {"5",  "BG接口冻结"},
                 {"6",  "涉嫌可疑交易冻结"},
                 {"7",  "ivr自助冻结"},
                 {"8",  "公众号自助冻结"},
                 {"9",  "微信安全"},
            };
        }

        #region 一、外币帐号查询

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="uid">用户香港钱包账号</param>
        /// <returns></returns>
        public DataTable QueryUserInfo(string uid, string client_ip)
        {
            var dt = dal.QueryUserInfo(uid, client_ip);
            if (dt != null && dt.Rows.Count > 0)
            {
                var columns = new string[] { "lstate_str", "user_type_str", "state_str" };
                dt.Columns.AddRange(columns.Select(u => new DataColumn(u)).ToArray());
                foreach (DataRow row in dt.Rows)
                {
                    row["lstate_str"] = GetDictionaryTryValue(DicLstate, row["lstate"], "其他");
                    row["user_type_str"] = GetDictionaryTryValue(DicUserType, row["user_type"], "其他");
                    row["state_str"] = GetDictionaryTryValue(DicUserRegState, row["state"], "其他");
                }
            }
            return dt;
        }

        /// <summary>
        /// 获取用户 内部id
        /// </summary>
        /// <param name="uin">用户香港钱包账号</param>
        /// <returns></returns>
        public string QueryUserId(string uin)
        {
            var dt = dal.QueryUserId(uin);
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0]["uid"] as string;
            }
            return string.Empty;
        }

        /// <summary>
        /// 冻结/解冻 用户
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
            return dal.LockUser(uin, lock_status, channel, op_name, true_name, phone, reason, memo, HandlerIp(client_ip));
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
            return dal.ResetPassWord(uin, op_name, true_name, phone, reason, memo, HandlerIp(client_ip));
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
            var dt = dal.QueryBindCardInfo(HandlerIp(client_ip), uin, card_id);
            if (dt != null)
            {
                var columns = new string[] { "card_type_str", "bill_user_name", "bind_status_str" };
                dt.Columns.AddRange(columns.Select(u => new DataColumn(u)).ToArray());
                foreach (DataRow row in dt.Rows)
                {
                    row["card_type_str"] = GetDictionaryTryValue(DicCardType, row["card_type"], "其他");
                    row["bind_status_str"] = GetDictionaryTryValue(DicBindStatus, row["bind_status"], "其他");
                    row["bill_user_name"] = (string)row["bill_first_name"] + (string)row["bill_last_name"];
                }
            }
            return dt;
        }

        /// <summary>
        /// 解绑卡
        /// </summary>
        /// <param name="uin">用户uin</param>
        /// <param name="bind_serialno">绑卡序列号</param>
        /// <param name="op_name">操作员</param>
        /// <param name="true_name">用户姓名</param>
        /// <param name="phone">用户联系方式</param>
        /// <param name="reason">冻结原因</param>
        /// <param name="memo">备注</param>
        /// <param name="client_ip">客户端ip</param>
        /// <returns></returns>
        public bool FreeBindCard(string uin, string bind_serialno, string op_name, string true_name, string phone, string reason, string memo, string client_ip)
        {
            return dal.FreeBindCard(uin, bind_serialno, op_name, true_name, phone, reason, memo, HandlerIp(client_ip));
        }

        #endregion

        #region 三、订单查询（新）

        /// <summary>
        /// 查询订单信息 By MD订单号
        /// </summary>
        /// <param name="listid"></param>
        /// <returns></returns>
        public DataTable QueryOrderByMDList(string listid)
        {
            var dt = dal.QueryOrderByMDList(listid);
            if (dt != null && dt.Rows.Count > 0)
            {
                string[] addColumns = { "paynum_str", "trade_type_str", "trade_state_str", "card_curtype_str", "refund_state_str","price_str",
                                          "IsRefund", "IsRefuse", "appeal_sign_str", "total_payout_str", "bank_paynum_str", "bank_curtype_str", "MDList","price_curtype_str" };
                dt.Columns.AddRange(addColumns.Select(u => new DataColumn(u, typeof(string))).ToArray());

                var row = dt.Rows[0];
                row["price_str"] = MoneyTransfer.FenToYuan((string)row["price"]);
                row["paynum_str"] = MoneyTransfer.FenToYuan((string)row["paynum"]);
                //row["total_payout_str"] = MoneyTransfer.FenToYuan((string)row["total_payout"]);
                row["bank_paynum_str"] = MoneyTransfer.FenToYuan((string)row["bank_paynum"]);
                row["MDList"] = listid;
                row["bank_curtype_str"] = GetDictionaryTryValue(CurType, row["bank_curtype"], "其他");
                row["price_curtype_str"] = GetDictionaryTryValue(CurType, row["price_curtype"], "其他");
                row["trade_type_str"] = GetDictionaryTryValue(DicTradeType, row["trade_type"], "其他");
                row["trade_state_str"] = GetDictionaryTryValue(DicTradeState, row["trade_state"], "其他");
                row["card_curtype_str"] = GetDictionaryTryValue(DicCardType, row["card_curtype"], "其他");
                // v_yqyqguo 2016-1-21 更改 退款状态为0初始状态的时候即为未退款，页面退款状态请展示为空.
                //row["refund_state_str"] = (string)row["refund_state"] == "0" ? "退款成功":"退款中"; 
                row["refund_state_str"] = (string)row["refund_state"] == "0" ? "" : "退款成功";
                row["total_payout_str"] = (string)row["refund_state"] == "0" ? "" : MoneyTransfer.FenToYuan((string)row["total_payout"]); 

                row["appeal_sign_str"] = GetDictionaryTryValue(DicAppealSign, row["appeal_sign"], "其他");
                row["IsRefund"] = ((string)row["trade_state"]) == "7" ? "是" : "否";
                row["IsRefuse"] = ((string)row["appeal_sign"]) == "2" ? "是" : "否";
            }
            return dt;
        }

        /// <summary>
        /// 查询订单信息 By 商户号 and 商户订单号 
        /// </summary>
        /// <param name="listid"></param>
        /// <returns></returns>
        public DataTable QueryOrderBySp(string spid, string sp_billno)
        {
            var dt = dal.QueryOrderBySp(spid, sp_billno);
            if (dt != null && dt.Rows.Count > 0)
            {
                var transaction_id = dt.Rows[0]["transaction_id"] as string;
                return QueryOrderByMDList(transaction_id);
            }
            return null;
        }

        /// <summary>
        /// 通过银行卡号
        /// </summary>
        /// <param name="card_no">卡号</param>
        /// <param name="trans_date">交易日期</param>
        /// <param name="client_ip">IP</param>
        public DataTable QueryOrderByCardNo(string card_no, DateTime trans_date, string client_ip)
        {
            var cardOrders = dal.QueryOrderByCardNo(card_no, trans_date, HandlerIp(client_ip));
            if (cardOrders != null && cardOrders.Rows.Count > 0)
            {
                DataTable resultdt = null;
                foreach (DataRow item in cardOrders.Rows)
                {
                    var orderid = item["transaction_id"] as string;
                    if (orderid != null)
                    {
                        var tempdt = QueryOrderByMDList(orderid);
                        resultdt = MergeDataTable(resultdt, tempdt);
                    }
                }
                return resultdt;
            }
            return null;
        }

        /// <summary>
        ///  通过银行订单号查询卡信息
        /// </summary>
        /// <param name="bank_type">银行类型</param>
        /// <param name="bill_no">银行订单号</param>
        /// <returns></returns>
        public DataTable QueryCardType(string bank_type, string bill_no)
        {
            return dal.QueryCardType(bank_type, bill_no);
        }
        #endregion

        #region 四、账户资金和流水查询（新增）

        /// <summary>
        /// 交易单-查询
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="offset"></param>       
        /// <param name="limit"></param>
        /// <param name="client_ip">IP</param>
        /// <returns></returns>
        public DataTable QueryTradeInfo(string uid,string stime,string etime, int offset, int limit, string client_ip)
        {
            var preTable = dal.QueryTradeInfo(uid, "101", stime, etime, offset, limit, HandlerIp(client_ip));
            if (preTable != null && preTable.Rows.Count > 0)
            {
                DataTable resultdt = null;
                foreach (DataRow item in preTable.Rows)
                {
                    var orderid = item["listid"] as string;
                    if (orderid != null)
                    {
                        var tempdt = QueryOrderByMDList(orderid);
                        resultdt = MergeDataTable(resultdt, tempdt);
                    }
                }
                return resultdt;
            }
            return null;
        }

        /// <summary>
        /// 退款单-查询
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="offset"></param>       
        /// <param name="limit"></param>
        /// <param name="client_ip">IP</param>
        /// <returns></returns>
        public DataTable QueryRefundInfo(string uid,string stime,string etime, int offset, int limit, string client_ip)
        {
            var preTable = dal.QueryTradeInfo(uid, "102", stime, etime, offset, limit, HandlerIp(client_ip));
            if (preTable != null && preTable.Rows.Count > 0)
            {
                DataTable resultdt = null;
                foreach (DataRow item in preTable.Rows)
                {
                    var orderid = item["coding"] as string;
                    if (orderid != null)
                    {
                        var tempdt = QueryOrderByMDList(orderid);
                        if (tempdt != null && tempdt.Rows.Count > 0)
                        {
                            tempdt.Columns.Add("refund_list");
                            foreach (DataRow row in tempdt.Rows)
                            {
                                row["refund_list"] = item["listid"];  //退款单号
                            }
                        }
                        resultdt = MergeDataTable(resultdt, tempdt);
                    }
                }
                return resultdt;
            }
            return null;
        }

        /// <summary>
        /// 体现单查询
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="client_ip"></param>
        /// <returns></returns>
        public DataTable QueryFetchInfo(string uid, string stime, string etime, int offset, int limit, string client_ip, string coding = "")
        {
            DataTable dt = dal.QueryFetchInfo(uid, stime, etime, offset, limit, client_ip, coding);
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string fetch_type = row["fetch_type"].ToString();
                    row["fetch_type"] = fetch_type == "1" ? "结算入账提现" :
                                        fetch_type == "2" ? "商户提现" :
                                        fetch_type == "3" ? "用户提现" :
                                        fetch_type == "4" ? "退款提现" :
                                        fetch_type == "50" ? "拒付提现" : "未知:" + fetch_type;

                    string fetch_state = row["fetch_state"].ToString();
                    row["fetch_state"] = fetch_state == "1" ? "初始状态(未冻结)" :
                                         fetch_state == "2" ? "等待付款(未汇总)" :
                                         fetch_state == "3" ? "审核中" :
                                         fetch_state == "4" ? "付款中" :
                                         fetch_state == "5" ? "付款成功" :
                                         fetch_state == "6" ? "付款失败" :
                                         fetch_state == "7" ? "付款作废" :
                                         fetch_state == "8" ? "已退票" :
                                         "未知:" + fetch_state;

                    string subject = row["subject"].ToString();
                    row["subject"] = subject == "1" ? "充值支付（中介收货款）" :
                                        subject == "2" ? "充值支付" :
                                        subject == "3" ? "买家确认" :
                                        subject == "4" ? "买家确认（自动提现）" :
                                        subject == "5" ? "退款" :
                                        subject == "6" ? "退款（退卖家货款）" :
                                        subject == "7" ? "充值支付（余额支付）" :
                                        subject == "8" ? "买家确认（卖家收货款）" :
                                        subject == "9" ? "快速交易" :
                                        subject == "10" ? "余额支付" :
                                        subject == "11" ? "充值" :
                                        subject == "12" ? "充值转帐" :
                                        subject == "13" ? "转帐" :
                                        subject == "14" ? "提现" :
                                        subject == "15" ? "回导" :
                                        "未知:" + subject;

                    row["num"] = MoneyTransfer.FenToYuan(row["num"].ToString(), "HKD");
                    row["charge"] = MoneyTransfer.FenToYuan(row["charge"].ToString(), "HKD");
                }
            }
            return dt;
        }

        public DataTable QueryBankrollList(string uid, string stime, string etime, int offset, int limit, string client_ip)
        {
            DataTable dt = dal.QueryBankrollList(uid, stime, etime, offset, limit, client_ip);

            if (dt != null) 
            {
                foreach (DataRow row in dt.Rows) 
                {
                    string type = row["type"].ToString();
                    row["type"] = type == "1" ? "入" :
                                    type == "2" ? "出" :
                                    type == "3" ? "冻结" :
                                    type == "4" ? "解冻" : "未知:" + type;

                    string subject = row["subject"].ToString();
                    row["subject"] = subject == "1" ? "充值支付（中介收货款）" :
                                        subject == "2" ? "充值支付" :
                                        subject == "3" ? "买家确认" :
                                        subject == "4" ? "买家确认（自动提现）" :
                                        subject == "5" ? "退款" :
                                        subject == "6" ? "退款（退卖家货款）" :
                                        subject == "7" ? "充值支付（余额支付）" :
                                        subject == "8" ? "买家确认（卖家收货款）" :
                                        subject == "9" ? "快速交易" :
                                        subject == "10" ? "余额支付" :
                                        subject == "11" ? "充值" :
                                        subject == "12" ? "充值转帐" :
                                        subject == "13" ? "转帐" :
                                        subject == "14" ? "提现" :
                                        subject == "15" ? "回导" :
                                        "未知:" + subject;

                    row["paynum"] = MoneyTransfer.FenToYuan(row["paynum"].ToString(), "HKD");
                    row["balance"] = MoneyTransfer.FenToYuan(row["balance"].ToString(), "HKD");
                    row["con"] = MoneyTransfer.FenToYuan(row["con"].ToString(), "HKD");
                    row["connum"] = MoneyTransfer.FenToYuan(row["connum"].ToString(), "HKD");
                }
            }
            return dt;
        }

        /// <summary>
        /// HK钱包支付---收红包记录查询
        /// </summary>
        public List<HKWalletReceivePackageModel> QueryReceivePackageList(string uid, string stime, string etime, int offset, int limit, string client_ip)
        {
            return dal.QueryReceivePackageList(uid, stime, etime, offset, limit, client_ip);
        }

        /// <summary>
        /// HK钱包支付---发红包记录查询
        /// </summary>
        public List<HKWalletSendPackageModel> QuerySendPackageList(string uid, string stime, string etime, int offset, int limit, string client_ip)
        {
            return dal.QuerySendPackageList(uid, stime, etime, offset, limit, client_ip);
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
            return dal.QueryHKPackageDetail(typeid,  listid,  qry_time,  client_ip);
        }

        #endregion

        #region 六、外币商户查询(新增)

        /// <summary>
        /// 查询外币商户查询-通过商户号spid 
        /// </summary>
        /// <param name="client_ip">Ip</param>
        /// <param name="spid">商户号</param>
        /// <returns></returns>
        public DataTable QueryMerinfoBySpid(string client_ip, string spid)
        {
            return dal.QueryMerinfo(HandlerIp(client_ip), null, spid);
        }

        /// <summary>
        /// 查询外币商户查询- 通过商户户id
        /// </summary>
        /// <param name="client_ip">Ip</param>
        /// <param name="uid">商户户id</param>
        /// <returns></returns>
        public DataTable QueryMerinfoByUid(string client_ip, string uid)
        {
            return dal.QueryMerinfo((client_ip), uid, null);
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
            return dal.QueryResetPassWordLog(parameters, offset, limit);
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
            var dt = dal.QueryFreezeLog(parameters, offset, limit);
            if (dt != null)
            {
                dt.Columns.Add("lock_status_str");
                dt.Columns.Add("channel_str");
                foreach (DataRow row in dt.Rows)
                {
                    row["channel_str"] = GetDictionaryTryValue(DicChannel, row["fchannel"], "其他");
                    row["lock_status_str"] = (string)row["flock_status"] == "0" ? "冻结" : "解冻";
                }
            }
            return dt;
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
            var dt = dal.QueryBindCardLog(uid, bind_serialno, offset, limit);
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Add("unbind_type_str");
                foreach (DataRow row in dt.Rows)
                {
                    var ftrue_name = row["ftrue_name"] as string;
                    var fphone = row["fphone"] as string;
                    if (string.IsNullOrEmpty(ftrue_name) && string.IsNullOrEmpty(fphone))
                    {
                        row["unbind_type_str"] = "自助解绑";
                        continue;
                    }
                    row["unbind_type_str"] = "客服解绑";
                }
            }
            return dt;
        }
        #endregion

        #region 公共方法

        /// <summary>
        /// 获取字典中的值
        /// </summary>
        /// <param name="dic">字典对象</param>
        /// <param name="key">key</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        private string GetDictionaryTryValue(IDictionary<string, string> dic, object key, string defaultValue)
        {
            string k = key as string;
            if (dic.ContainsKey(k))
            {
                return dic[k].ToString();
            }
            return defaultValue + ":(" + key + ")";
        }

        /// <summary>
        /// 合并两个表格
        /// </summary>
        /// <param name="source">要合并的表格</param>
        /// <param name="dt2">被合并的表格</param>
        protected DataTable MergeDataTable(DataTable source, DataTable dt2)
        {
            if (source == null)
            {
                source = dt2;
            }
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                source.Merge(dt2);
            }
            return source;
        }

        //ip处理
        public string HandlerIp(string ip)
        {
            if (ip == "::1")
            {
                ip = "127.0.0.1";
            }
            return ip;
        }
        #endregion
    }
}
