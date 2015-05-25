using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CFT.CSOMS.DAL.WechatPay;
using CFT.CSOMS.COMMLIB;
using System.Collections;
using CFT.CSOMS.DAL.CFTAccount;

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
        public DataSet QueryCreditCardRefund(string wxNo, string bankNo, string refundNo,string uin, string stime, string etime, int start, int count)
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
        public DataSet QueryAddedValueTicket(int accType, string uin, string state, string couponId,string spId, int start, int count) 
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

        public DataSet QueryWebchatHB(string sendData, int flag, string client_ip, int offset, int limit)
        {
            string paramName = "";
            if (flag == 1)
            {
                paramName = "send_openid";
            }
            else if (flag == 2)
            {
                paramName = "rec_openid";
            }
            else if (flag == 3)
            {
                paramName = "send_listid";
            }
            else
                throw new Exception("flag参数有误，没有对应接口");
            return new TradePayData().QueryWebchatHB(paramName, sendData, flag, client_ip, offset, limit);
        }
    }
}
