using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CFT.CSOMS.DAL.WechatPay;
using CFT.CSOMS.COMMLIB;
using System.Collections;
using CFT.CSOMS.DAL.CFTAccount;
using CFT.CSOMS.DAL.Infrastructure;
using TENCENT.OSS.CFT.KF.DataAccess;
using BankLib;
using System.Web;

namespace CFT.CSOMS.BLL.WechatPay
{
    public class WechatPayService
    {
        /// <summary>
        /// 获取微信信用卡还款记录
        /// </summary>
        /// <param name="wxNo">微信号</param>
        /// <param name="bankNo">银行账号</param>
        /// <param name="refundNo">还款单号</param>
        /// <param name="stime">开始日期</param>
        /// <param name="etime">结束日期</param>
        /// <returns></returns>
        public DataSet QueryCreditCardRefund(string wxNo, string bankNo, string refundNo, string uin, string stime, string etime, int start, int count)
        {
            DataSet ds = null;
            try
            {
                if (string.IsNullOrEmpty(stime) || string.IsNullOrEmpty(etime))
                {
                    throw new ArgumentNullException("起始日期不能为空");
                }

                if (!string.IsNullOrEmpty(wxNo))
                {
                    //通过微信号查openid
                    string openid = WeChatHelper.GetXYKHKOpenIdFromWeChatName(wxNo);
                    //通过openid查询还款uid
                    string uid = new CreditCardRefund().QueryUidFromCreditCardOpenid(openid);

                    ds = new CreditCardRefund().QueryCreditCardRefundWX(uid, stime, etime, start, count);
                }
                else if (!string.IsNullOrEmpty(bankNo))
                {
                    //如果是银行卡号查询，需要先调接口加密
                    ds = new CreditCardRefund().QueryCreditCardRefundWX(bankNo, refundNo, stime, etime, start, count);
                }
                else if (!string.IsNullOrEmpty(refundNo))
                {
                    ds = new CreditCardRefund().QueryCreditCardRefundWX(bankNo, refundNo, stime, etime, start, count);
                }
                //else if (!string.IsNullOrEmpty(uin))
                //{
                //    string uid = AccountData.ConvertToFuid(uin);
                //  //  string uid = "299708827";
                //    ds = new CreditCardRefund().QueryCreditCardRefundWX(uid, stime, etime, start, count);
                //}
                else
                {
                    throw new ArgumentNullException("查询条件不能为空");
                }
            }
            catch (Exception ee)
            {
                throw new Exception(ee.Message);
            }

            return ds;
        }

        /// <summary>
        /// 获取微信信用卡还款详情
        /// </summary>
        /// <param name="wxNo">财付通账号</param>
        /// <param name="wxFetchNo">还款单号</param>
        /// <returns></returns>
        public DataTable QueryCreditCardRefundDetail(string uid, string wxFetchNo)
        {
            DataTable dt = null;
            if (!string.IsNullOrEmpty(uid))
            {
                dt = new CreditCardRefund().QueryCreditCardRefundDetail(uid, wxFetchNo);
            }
            return dt;
        }

        /// <summary>
        /// 微信订单查询接口
        /// </summary>
        /// <param name="type">类型 1 --- 根据微信订单号查询2 ---  根据商户号和商户订单号查询</param>
        /// <param name="wx_trans_id">微信订单号， 当type=1时 必传</param>
        /// <param name="mch_code">商户号， 当type=2时必传</param>
        /// <param name="out_trade_no">商户订单号， 当type=2时必传</param>
        /// <returns></returns>
        public DataTable QueryTradeOrder(int type, string wx_trans_id, string mch_code, string out_trade_no)
        {
            return new CreditCardRefund().QueryTradeOrder(type, wx_trans_id, mch_code, out_trade_no);
        }

        /// <summary>
        /// 加密信用卡卡号
        /// </summary>
        /// <param name="creditId">卡号ID，多个卡号用|分割</param>
        /// <returns>加密后的卡号</returns>
        public string EncodeCreidtId(string creditId)
        {
            return new CreditCardRefund().CreditEncode(creditId);
        }

        /// <summary>
        /// 查询用户增值券
        /// </summary>
        /// <param name="accType">账号类型：1财付通账号，2微信号</param>
        /// <param name="uin">微信号或财付通账号</param>
        /// <param name="state">业务状态：0全部，1未使用，3已使用，4已过期</param>
        /// <param name="couponId">增值券编号</param>
        /// <param name="spId">商户号</param>
        /// <param name="start">分页起始位</param>
        /// <param name="count">记录条数</param>
        /// <returns>增值券列表</returns>
        public DataSet QueryAddedValueTicket(int accType, string uin, string state, string couponId, string spId, int start, int count)
        {
            DataSet ds = null;
            try
            {
                ds = new CreditCardRefund().QueryAddedValueTicket(accType, uin, couponId, spId, state, start, count);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add("stateStr", typeof(String));//使用状态
                    ds.Tables[0].Columns.Add("thresholdStr", typeof(String));//用券门槛，最低申购金额(分)
                    ds.Tables[0].Columns.Add("valueStr", typeof(String));//增值券面额大小,单位分，只有非固定面额批次才有效

                    Hashtable ht1 = new Hashtable();
                    ht1.Add("1", "未使用");
                    ht1.Add("3", "已使用");
                    ht1.Add("4", "已过期");

                    COMMLIB.CommUtil.DbtypeToPageContent(ds.Tables[0], "state", "stateStr", ht1);
                    COMMLIB.CommUtil.FenToYuan_Table(ds.Tables[0], "threshold", "thresholdStr");
                    COMMLIB.CommUtil.FenToYuan_Table(ds.Tables[0], "value", "valueStr");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("QueryAddedValueTicket Error：" + ex.Message);
            }

            return ds;
        }

        /// <summary>
        /// 根据批次号查增值券详情
        /// </summary>
        /// <param name="batchId">批次号</param>
        /// <param name="pwd">密码</param>
        /// <returns>增值券详情</returns>
        public DataTable QueryBatchTicketDetail(string batchId, string pwd)
        {
            DataTable dt = null;
            try
            {
                dt = new CreditCardRefund().QueryBatchTicketDetail(batchId, pwd);
            }
            catch (Exception ex)
            {
                throw new Exception("QueryBatchTicketDetail Error：" + ex.Message);
            }

            return dt;
        }

        public DataTable QueryWxTrans(string prime_trans_id)
        {
            return new TradePayData().QueryWxTrans(prime_trans_id);
        }

        /// <summary>
        /// 发送的红包
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public DataSet QueryUserSendList(string openid, DateTime start_time, DateTime end_time, int offset, int limit)
        {
            var parameter = new string[][]
            {
                new string[]  {"openid",openid }, //"onqOjjkmc6mFyMkG67lri4qPlpBg"
                new string[]  {"start_time",start_time.ToString("yyyy-MM-dd HH:mm:ss")},
                new string[]  {"end_time",end_time.ToString("yyyy-MM-dd HH:mm:ss")},
                new string[]  {"limit",limit.ToString()},              
                new string[]  {"offset",offset.ToString()}
            };

            DataSet ds = new TradePayData().QueryWechatHB(parameter, "QueryUserSendList");

            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("Summary", typeof(string));
                ds.Tables[0].Columns.Add("State_text", typeof(string));
                ds.Tables[0].Columns.Add("Refund", typeof(string));
                ds.Tables[0].Columns.Add("TotalAmount_text", typeof(string));

                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    item["Summary"] = string.Format("{0}/{1},总计{2}元",
                                                    item["ReceivedNum"].ToString(),
                                                    item["TotalNum"].ToString(),
                                                    MoneyTransfer.FenToYuan(item["ReceivedAmount"].ToString()));

                    double refundAmount = double.Parse(item["TotalAmount"].ToString()) - double.Parse(item["ReceivedAmount"].ToString());

                    item["Refund"] = MoneyTransfer.FenToYuan(refundAmount).ToString() + "元";
                    item["TotalAmount_text"] = MoneyTransfer.FenToYuan(item["TotalAmount"].ToString());

                    switch (item["State"].ToString().Trim())
                    {
                        case "PREPAY": item["State_text"] = "等待支付"; break;              //1
                        case "PAYOK": item["State_text"] = "支付完成"; break;               //2
                        case "PARTRECEIVE": item["State_text"] = "部分领取"; break;         //3
                        case "ALLRECEIVE": item["State_text"] = "全部领取"; break;          //4
                        case "OVERTIMEREFUNDED": item["State_text"] = "过期退回"; break;    //5
                        case "EXCEPTIONREFUNDED": item["State_text"] = "异常退款"; break;   //6
                        case "REFUNDING": item["State_text"] = "退款中"; break;             //7
                        default: item["State_text"] = "未知" + item["State"].ToString(); break;
                    }
                }
            }

            return ds;
        }

        /// <summary>
        /// 收到的红包
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public DataSet QueryUserReceiveList(string openid, DateTime start_time, DateTime end_time, int offset, int limit)
        {
            var parameter = new string[][]
            {
                new string[]  {"openid",openid},  //"onqOjjhRRpnwjceoYnAY2CrJd2JU"
                new string[]  {"start_time",start_time.ToString("yyyy-MM-dd HH:mm:ss")},
                new string[]  {"end_time",end_time.ToString("yyyy-MM-dd HH:mm:ss")},              
                new string[]  {"limit",limit.ToString()}, 
                new string[]  {"offset",offset.ToString()}
            };

            DataSet ds = new TradePayData().QueryWechatHB(parameter, "QueryUserReceiveList");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("Amount_text", typeof(string));
                ds.Tables[0].Columns.Add("Title", typeof(string));

                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    item["Amount_text"] = MoneyTransfer.FenToYuan(item["Amount"].ToString());
                    item["Title"] = string.Format("{0}发的红包", item["SendName"].ToString());
                }
            }

            return ds;
        }

        /// <summary>
        /// 发送的红包,的 接受者红包信息
        /// </summary>
        /// <param name="send_id"></param>
        /// <param name="qry_type">0 普通模式  完全字段, 1 简单模式</param>
        /// <returns></returns>
        public DataSet QueryDetail(string send_id, int qry_type)
        {
            var parameter = new string[][]
            {
                new string[]  {"send_id",send_id},
                new string[]  {"qry_type",qry_type.ToString()}
            };
            return new TradePayData().QueryWechatHB(parameter, "QueryDetail");  //QuerySendById:发送的红包本身的信息  ,QueryDetailReq: 发送的红包,的 接受者红包信息
        }

        /// <summary>
        /// 收到的红包详情
        /// </summary>
        /// <param name="receive_id"></param>
        /// <param name="send_id"></param>
        /// <param name="qry_type">0 普通模式  完全字段, 1 简单模式</param>
        /// <returns></returns>
        public DataSet QueryReceiveHBInfoById(string receive_id, string send_id, int qry_type)
        {
            var parameter = new string[][]
            {
                new string[]  {"receive_id",receive_id},
                new string[]  {"send_id",send_id},
                new string[]  {"qry_type",qry_type.ToString()}
            };
            return new TradePayData().QueryWechatHB(parameter, "QueryReceiveById");
        }


        /// <summary>
        /// 查询实时还款详情
        /// </summary>
        /// <param name="transaction_id">还款提现单号</param>
        /// <param name="acc_date">还款日期</param>
        /// <param name="status">交易状态1：请求；2：成功</param>
        /// <returns></returns>
        public DataTable QueryRealtimeRepayment(string transaction_id, DateTime acc_date, int status)
        {
            var dt = new TradePayData().QueryRealtimeRepayment(transaction_id, acc_date, status);
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Add("Fstatus_str");
                Dictionary<string, string> dicFstatus = new Dictionary<string, string>()
                {
                    {"0","无状态消息"},
                    {"1","请求还款"},
                    {"2","还款成功"},
                    {"3","还款失败"},
                    {"4","还款结果未知"},
                };
                var row = dt.Rows[0];
                row["Fret_code"] = CommUtil.URLDecode(row["Fret_code"] as string);
                CFT.CSOMS.COMMLIB.CommUtil.DbtypeToPageContent(dt, "Fstatus", "Fstatus_str", dicFstatus);

            }
            return dt;
        }

        /// <summary>
        /// 获取微信用户的参与的AA交易记录
        /// </summary>
        /// <param name="aaUIN"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public DataSet GetAATradeList(string aaUIN, int startIndex, int count)
        {
            DataSet dsAACollections = new TradePayData().GetAATradeList(aaUIN, startIndex, count);

            if (dsAACollections != null && dsAACollections.Tables.Count > 0 && dsAACollections.Tables[0].Rows.Count > 0)
            {
                dsAACollections.Tables[0].Columns.Add("Ftotal_paid_amount_text", typeof(string));
                dsAACollections.Tables[0].Columns.Add("Fstatus_text", typeof(string));

                foreach (DataRow item in dsAACollections.Tables[0].Rows)
                {
                    var totalPaidAmountText = MoneyTransfer.FenToYuan(item["Ftotal_paid_amount"].ToString());
                    switch (item["Ftype"].ToString())
                    {
                        case "1":
                            item["Ftotal_paid_amount_text"] = string.Format("+{0}", totalPaidAmountText);
                            break;
                        case "2":
                            item["Ftotal_paid_amount_text"] = string.Format("-{0}", totalPaidAmountText);
                            break;
                        default:
                            item["Ftotal_paid_amount_text"] = string.Format("未知{0}", totalPaidAmountText);
                            break;
                    }

                    switch (item["Flstate"].ToString())
                    {

                        case "1":
                            item["Fstatus_text"] = "正常";
                            break;
                        case "2":
                            item["Fstatus_text"] = "作废";
                            break;
                        case "3":
                            item["Fstatus_text"] = "关闭";
                            break;
                        default:
                            item["Fstatus_text"] = "未知" + item["Flstate"].ToString();
                            break;
                    }
                }
            }

            return dsAACollections;
        }

        /// <summary>
        /// 获取指定的AA收单分单记录明细
        /// </summary>
        public DataSet GetAATradeDetailsSingleYear(string aaCollectionNo, DateTime createTime, int startIndex, int count)
        {
            return new TradePayData().GetAATradeDetailsSingleYear(aaCollectionNo, createTime, startIndex, count);
        }

        /// <summary>
        /// 获取指定的AA收款总单信息
        /// </summary>
        /// <param name="aaCollectionNo"></param>
        /// <returns></returns>
        public DataSet QueryAATotalTradeInfo(string aaCollectionNo)
        {
            return new TradePayData().QueryAATotalTradeInfo(aaCollectionNo);
        }
        /// <summary>
        /// 查询申报流水
        /// </summary>
        /// <param name="partner">商户号</param>
        /// <param name="customs">海关编号</param>
        /// <param name="transaction_id">支付单号</param>
        /// <param name="out_trade_no">商户订单号</param>
        /// <param name="sub_order_no">子商户订单号</param>
        /// <param name="sub_order_id">子支付单号</param>
        /// <returns></returns>
        public DataSet QueryDeclareDogInfo(string partner, string transaction_id, string out_trade_no, string sub_order_no, string sub_order_id)
        {
            if (string.IsNullOrEmpty(partner.Trim()) && string.IsNullOrEmpty(transaction_id.Trim())) 
            {
                throw new Exception("商户号和支付单号不能同时为空！");
            }
            else if (string.IsNullOrEmpty(transaction_id.Trim()) && !string.IsNullOrEmpty(partner.Trim()))
            {
                if (string.IsNullOrEmpty(out_trade_no.Trim()) &&
                string.IsNullOrEmpty(sub_order_no.Trim()) &&
                string.IsNullOrEmpty(sub_order_id.Trim()))
                {
                    throw new Exception("根据商户号查询时：商户订单号,子商户订单号,子支付单号不能同时为空！");
                }
            }
            

            DataSet ds = new TradePayData().QueryDeclareDogInfo(partner, transaction_id, out_trade_no, sub_order_no, sub_order_id);
            if (ds != null && ds.Tables.Count != 0)
            {
                DataTable dtF = ds.Tables[0];
                DataTable dtS = ds.Tables[1];

                dtS.Columns.Add("number", typeof(string));
                dtS.Columns.Add("state_str", typeof(string));
                dtS.Columns.Add("modify_time_str", typeof(string));
                dtS.Columns.Add("customs_str", typeof(string));
                dtS.Columns.Add("order_fee_str", typeof(string));
                dtS.Columns.Add("product_fee_str", typeof(string));
                dtS.Columns.Add("transport_fee_str", typeof(string));
                dtS.Columns.Add("duty_str", typeof(string));

                int i = 1;
                foreach (DataRow dr in dtS.Rows)
                {
                    dr["number"] = i.ToString();
                    i++;
                    string state = dr["state"].ToString();
                    dr["state_str"] = state == "1" ? "待申报" :
                                      state == "3" ? "申报中" :
                                      state == "4" ? "申报成功" :
                                      state == "5" ? "申报失败" : state;
                    dr["modify_time_str"] = string.Format("{0:0000-00-00 00:00:00}", Convert.ToInt64(dr["modify_time"].ToString()));
                    dr["customs_str"] = Transferred(dr["customs"].ToString().Trim(), "海关编码");
                    dr["order_fee_str"] = MoneyTransfer.FenToYuan(dr["order_fee"].ToString().Trim());
                    dr["product_fee_str"] = MoneyTransfer.FenToYuan(dr["product_fee"].ToString().Trim());
                    dr["transport_fee_str"] = MoneyTransfer.FenToYuan(dr["transport_fee"].ToString().Trim());
                    dr["duty_str"] = MoneyTransfer.FenToYuan(dr["duty"].ToString().Trim());
                }
            }
            return ds;
        }
        public DataSet QueryMerchantCustom(string partner)
        {
            if (string.IsNullOrEmpty(partner.Trim()))
            {
                throw new Exception("商户号不能为空！");
            }
            DataSet ds = new TradePayData().QueryMerchantCustom(partner);
            if (ds != null && ds.Tables.Count != 0&&ds.Tables[0].Rows.Count>0)
            {
                ds.Tables[0].Columns.Add("merchant_type_str", typeof(string));
                ds.Tables[0].Rows[0]["contact_email"] = BankIOX.DecryptNoPadding(HttpUtility.UrlDecode(ds.Tables[0].Rows[0]["contact_email"].ToString(), Encoding.GetEncoding("gbk")));
                ds.Tables[0].Rows[0]["contact_name"] = BankIOX.DecryptNoPadding(HttpUtility.UrlDecode(ds.Tables[0].Rows[0]["contact_name"].ToString(), Encoding.GetEncoding("gbk")));
                ds.Tables[0].Rows[0]["contact_phone"] = BankIOX.DecryptNoPadding(HttpUtility.UrlDecode(ds.Tables[0].Rows[0]["contact_phone"].ToString(), Encoding.GetEncoding("gbk")));
                ds.Tables[0].Rows[0]["merchant_type_str"] = Transferred(ds.Tables[0].Rows[0]["merchant_type"].ToString().Trim(), "商户类型");

                DataTable dtS = ds.Tables[1];
                dtS.Columns.Add("custom_id_str", typeof(string));
                foreach (DataRow dr in dtS.Rows)
                {
                    dr["custom_id_str"] = Transferred(dr["custom_id"].ToString().Trim(), "海关编码");
                }
            }
            return ds;
        }
        public DataTable CustomsRedeclare(string requesttext)
        {
            return new TradePayData().CustomsRedeclare(requesttext);
        }
        /// <summary>
        /// 海关编码和商户类型转义
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private string Transferred(string id, string type)
        {
            if (type == "海关编码")
            {
                return id == "1" ? "广州" :
                 id == "2" ? "杭州" :
                 id == "3" ? "宁波" :
                 id == "4" ? "深圳" :
                 id == "5" ? "郑州（保税物流中心）" :
                 id == "6" ? "重庆" :
                 id == "7" ? "西安" :
                 id == "8" ? "上海" :
                 id == "9" ? "郑州（综保区）" :
                   "未知:" + id;

            }
            else if (type == "商户类型")
            {
                return id == "1" ? "境内商户" :
                        id == "2" ? "境外商户" :
                        id == "4" ? "微信支付商户" :
                        "未知:" + id;
            }
            else
            {
                return "";
            }
        }

        public DataTable CrossBorderRemittances(string WeChatId, string s_phone_no, string wx_pay_id, string wx_pay_state,
                                               string remit_type, string start_date, string end_date, int offset, int limit)
        {
            #region  数据验证
            string s_openid = "";
            if (string.IsNullOrEmpty(WeChatId) && string.IsNullOrEmpty(s_phone_no) && string.IsNullOrEmpty(wx_pay_id))
            {
                throw new Exception("微信号，手机号码，支付单号不能同时为空！");
            }
            if (!string.IsNullOrEmpty(WeChatId) )
            {
                if ((string.IsNullOrEmpty(start_date) || string.IsNullOrEmpty(end_date)))
                {
                    throw new Exception("根据微信号查询时，起始时间和结束时间不能为空！");
                }
                try
                {
//#if DEBUG
//                    s_openid = WeChatId;
//#endif
                    s_openid = WeChatHelper.GetHBOpenIdFromWeChatName(WeChatId);
                }
                catch (Exception ex)
                {
                    throw new Exception("微信号转openid失败:" + ex.Message);
                }
            }
            if (!string.IsNullOrEmpty(s_phone_no) && (string.IsNullOrEmpty(start_date) || string.IsNullOrEmpty(end_date)))
            {
                throw new Exception("根据手机号码查询时，起始时间和结束时间不能为空！");
            }

            if (!string.IsNullOrEmpty(start_date) && !string.IsNullOrEmpty(end_date))
            {
                DateTime stime = Convert.ToDateTime(start_date);
                DateTime etime = Convert.ToDateTime(end_date);

                if (stime.AddDays(30) < etime)
                {
                    throw new Exception("时间返回不能超过一个月!");
                }

                start_date = stime.ToString("yyyy-MM-dd");
                end_date = etime.ToString("yyyy-MM-dd");
            }
            #endregion

            DataTable dt = new TradePayData().CrossBorderRemittances(s_openid, s_phone_no, wx_pay_id, wx_pay_state,
                                               remit_type, start_date, end_date, offset, limit);

            //dt.Columns.Add("remit_type", typeof(string));
            //dt.Columns.Add("wx_pay_state", typeof(string));
            //dt.Columns.Add("r_bank_type", typeof(string));
            //dt.Columns.Add("", typeof(string));

            return dt;
        }
    }
}
