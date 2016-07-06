using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CFT.CSOMS.DAL.PNRModule;
using System.Collections;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CFT.CSOMS.COMMLIB;
using CFT.CSOMS.DAL.TradeModule;
using System.Configuration;
using CFT.CSOMS.DAL.Infrastructure;
using CFT.CSOMS.BLL.WechatPay;
using System.Xml;
using CFT.CSOMS.DAL.FundModule;

namespace CFT.CSOMS.BLL.TradeModule
{

    public class TradeService
    {
        public DataSet BeforeCancelTradeQuery(string uid)
        {
            if (uid.Trim().Length < 3)
            {
                throw new Exception("内部id有误");
            }

            return new TradeData().BeforeCancelTradeQuery(uid);

        }

               //注销前历史库交易查询
        public DataSet BeforeCancelTradeHistoryQuery(string uid, DateTime start_time, DateTime end_time)
        {
            return new TradeData().BeforeCancelTradeHistoryQuery(uid,start_time,end_time);
        }

        public int ControledFinRemoveLogInsert(string qqid, string FbalanceStr, string FtypeText, string curtype, DateTime FmodifyTime, string FupdateUser)
        {
            return new ControlFundData().RemoveControledFinLogInsert(qqid, FbalanceStr, FtypeText, curtype, FmodifyTime, FupdateUser);
        }
        public void RemoveControledFinLogInsertAll(string qqid, string uid, DataTable dt)
        {
            foreach (DataRow item in dt.Rows)
            {
                new ControlFundData().RemoveControledFinLogInsert(qqid, item["FbalanceStr"].ToString(), item["FtypeText"].ToString(), item["cur_type"].ToString(), DateTime.Now, uid);
            }
        }
       
        public DataSet QueryWxBuyOrderByUid(Int64 uid, DateTime startTime, DateTime endTime)
        {
            return (new TradeData()).QueryWxBuyOrderByUid(uid, startTime, endTime);
        }
        #region 交易记录查询

        /// <summary>
        /// 交易记录查询
        /// </summary>
        /// <param name="tradeid"></param>
        /// <param name="typeid">0,买家交易单;9卖家交易单;10,中介交易单;-1,买家未完成交易单;-2,卖家未完成交易单;1,银行返回定单号;4,订单号</param>
        /// <param name="listTime">查询银行返回定单的时间</param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="istr"></param>
        /// <param name="imax"></param>
        /// <returns></returns>
        public DataSet GetTradeList(string tradeid, int typeid, DateTime listTime, DateTime beginTime, DateTime endTime, int istr, int imax,string uid)
        {
            try
            {
                if (typeid == 4 || typeid == 1)
                {
                    if (typeid.ToString() == "1") //根据银行返回定单号查询
                    {
                        tradeid = new TradeData().ConvertToListID(tradeid, listTime);
                    }
                    istr = 1;
                    imax = 2;
                    beginTime = DateTime.Parse(ConfigurationManager.AppSettings["sBeginTime"].ToString());
                    endTime = DateTime.Parse(ConfigurationManager.AppSettings["sEndTime"].ToString());
                }

                //TODO:有个makelog的方法不是主需求
                DataSet ds = new DataSet();
                if (typeid >= 0)
                {
                    ds = new TradeData().GetTradeList(tradeid, typeid, beginTime, endTime, istr, imax);
                }
                else
                {
                    ds = new TradeService().GetListidFromUserOrder(tradeid, uid, 0, 1, typeid);
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    #region 字段处理

                    ds.Tables[0].Columns.Add("Fpay_type_str");  //支付类型
                    ds.Tables[0].Columns.Add("Fpaybuy_str"); //退买家金额
                    ds.Tables[0].Columns.Add("Fpaysale_str"); //退卖家金额
                    ds.Tables[0].Columns.Add("Fappeal_sign_str"); //申诉标志
                    ds.Tables[0].Columns.Add("Fmedi_sign_str"); //中介标志
                    ds.Tables[0].Columns.Add("Fchannel_id_str"); //渠道编号
                    ds.Tables[0].Columns.Add("CloseReason", typeof(string));
                    ds.Tables[0].Columns.Add("FbuyidCFT", typeof(string));//买家财付通账号

                    if (!ds.Tables[0].Columns.Contains("Fexplain"))
                    { ds.Tables[0].Columns.Add("Fexplain"); }//备注
                    if (!ds.Tables[0].Columns.Contains("Frefund_typeName"))
                    { ds.Tables[0].Columns.Add("Frefund_typeName"); }//退款类型
                    if (!ds.Tables[0].Columns.Contains("FsaleidCFT"))
                    { ds.Tables[0].Columns.Add("FsaleidCFT"); }//卖家财付通账号
                    if (!ds.Tables[0].Columns.Contains("Ftrade_stateName"))
                    { ds.Tables[0].Columns.Add("Ftrade_stateName"); }//交易状态

                    TransferMeaning.Transfer.GetColumnValueFromDic(ds.Tables[0], "Fpay_type", "Fpay_type_str", "PAY_TYPE");//支付类型

                    #region Fappeal_sign、Fmedi_sign、Fchannel_id
                    string strtmp = ds.Tables[0].Rows[0]["Fappeal_sign"].ToString();
                    if (strtmp == "1")
                    {
                        ds.Tables[0].Rows[0]["Fappeal_sign_str"] = "正常";
                    }
                    else if (strtmp == "2")
                    {
                        ds.Tables[0].Rows[0]["Fappeal_sign_str"] = "已转申诉";
                    }
                    else
                    {
                        ds.Tables[0].Rows[0]["Fappeal_sign_str"] = "未知类型" + strtmp;
                    }


                    strtmp = ds.Tables[0].Rows[0]["Fmedi_sign"].ToString();
                    if (strtmp == "1")
                    {
                        ds.Tables[0].Rows[0]["Fmedi_sign_str"] = "是中介交易";
                    }
                    else if (strtmp == "2")
                    {
                        ds.Tables[0].Rows[0]["Fmedi_sign_str"] = "非中介交易";
                    }
                    else if (strtmp == "0")
                    {
                        ds.Tables[0].Rows[0]["Fmedi_sign_str"] = "非中介交易(2.0)";
                    }
                    else
                    {
                        ds.Tables[0].Rows[0]["Fmedi_sign_str"] = "未知类型" + strtmp;
                    }


                    strtmp = ds.Tables[0].Rows[0]["Fchannel_id"].ToString();
                    if (strtmp == "1")
                    {
                        ds.Tables[0].Rows[0]["Fchannel_id_str"] = "财付通";
                    }
                    else if (strtmp == "2")
                    {
                        ds.Tables[0].Rows[0]["Fchannel_id_str"] = "拍拍网";
                    }
                    else if (strtmp == "3")
                    {
                        ds.Tables[0].Rows[0]["Fchannel_id_str"] = "客户端小钱包";
                    }
                    else if (strtmp == "4")
                    {
                        ds.Tables[0].Rows[0]["Fchannel_id_str"] = "手机支付";
                    }
                    else if (strtmp == "5")
                    {
                        ds.Tables[0].Rows[0]["Fchannel_id_str"] = "第三方";
                    }
                    else if (strtmp == "6")
                    {
                        ds.Tables[0].Rows[0]["Fchannel_id_str"] = "ivr";
                    }
                    else if (strtmp == "7")
                    {
                        ds.Tables[0].Rows[0]["Fchannel_id_str"] = "平台专用账户";
                    }
                    else if (strtmp == "8")
                    {
                        ds.Tables[0].Rows[0]["Fchannel_id_str"] = "基金基础代扣";
                    }
                    else if (strtmp == "9")
                    {
                        ds.Tables[0].Rows[0]["Fchannel_id_str"] = "微支付";
                    }
                    else if (strtmp == "100")
                    {
                        ds.Tables[0].Rows[0]["Fchannel_id_str"] = "手机客户端（手机充值卡充值财付通）";
                    }
                    else if (strtmp == "101")
                    {
                        ds.Tables[0].Rows[0]["Fchannel_id_str"] = "手机财付通HTMl5支付中心（手机充值卡充值财付通）";
                    }
                    else if (strtmp == "102")
                    {
                        ds.Tables[0].Rows[0]["Fchannel_id_str"] = "手机qq";
                    }
                    else if (strtmp == "103")
                    {
                        ds.Tables[0].Rows[0]["Fchannel_id_str"] = "Pos收单";
                    }
                    else if (strtmp == "104")
                    {
                        ds.Tables[0].Rows[0]["Fchannel_id_str"] = "微生活";
                    }
                    else if (strtmp == "105")
                    {
                        ds.Tables[0].Rows[0]["Fchannel_id_str"] = "微信Web扫码支付";
                    }
                    else if (strtmp == "106")
                    {
                        ds.Tables[0].Rows[0]["Fchannel_id_str"] = "微信app跳转支付";
                    }
                    else if (strtmp == "107")
                    {
                        ds.Tables[0].Rows[0]["Fchannel_id_str"] = "微信公众帐号内支付";
                    }
                    else if (strtmp == "108")
                    {
                        ds.Tables[0].Rows[0]["Fchannel_id_str"] = "手机支付-wap";
                    }
                    else if (strtmp == "109")
                    {
                        ds.Tables[0].Rows[0]["Fchannel_id_str"] = "手机支付-HTML5";
                    }
                    else if (strtmp == "110")
                    {
                        ds.Tables[0].Rows[0]["Fchannel_id_str"] = "手机支付-客户端";
                    }
                    else if (strtmp == "111")
                    {
                        ds.Tables[0].Rows[0]["Fchannel_id_str"] = "手q支付";
                    }
                    else if (strtmp == "112")
                    {
                        ds.Tables[0].Rows[0]["Fchannel_id_str"] = "数平SDK";
                    }
                    #endregion

                    if (!ds.Tables[0].Columns.Contains("Fbuy_bankid"))//支付绑定序列号
                    {
                        ds.Tables[0].Columns.Add("Fbuy_bankid");
                        ds.Tables[0].Rows[0]["Fbuy_bankid"] = "";
                    }

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Fstandby8"].ToString()))
                    {
                        string s_close_reason = ds.Tables[0].Rows[0]["Fstandby8"].ToString();
                        if (s_close_reason == "1")
                        {
                            ds.Tables[0].Rows[0]["CloseReason"] = "风控关闭订单";
                        }
                        else if (s_close_reason == "2")
                        {
                            ds.Tables[0].Rows[0]["CloseReason"] = "微信线下支付商户关闭订单";
                        }
                        else if (s_close_reason == "3")
                        {
                            ds.Tables[0].Rows[0]["CloseReason"] = "购物券回收关闭订单";
                        }
                        else if (s_close_reason == "4")
                        {
                            ds.Tables[0].Rows[0]["CloseReason"] = "拍拍关闭订单";
                        }
                        else if (s_close_reason == "5")
                        {
                            ds.Tables[0].Rows[0]["CloseReason"] = "赔付调帐订单";
                        }
                    }

                    bool isC2C = false;
                    int type = 0;
                    if (int.TryParse(ds.Tables[0].Rows[0]["Ftrade_type"].ToString(), out type))
                    {
                        if (type == 1)
                        {
                            isC2C = true;
                        }
                    }

                    ds.Tables[0].Rows[0]["Fbuy_bank_type"] = TransferMeaning.Transfer.convertbankType(ds.Tables[0].Rows[0]["Fbuy_bank_type"].ToString());
                    ds.Tables[0].Rows[0]["Fcurtype"] = TransferMeaning.Transfer.convertMoney_type(ds.Tables[0].Rows[0]["Fcurtype"].ToString());
                    ds.Tables[0].Rows[0]["Flstate"] = TransferMeaning.Transfer.convertTradeState(ds.Tables[0].Rows[0]["Flstate"].ToString());
                    ds.Tables[0].Rows[0]["Fsale_bank_type"] = TransferMeaning.Transfer.convertbankType(ds.Tables[0].Rows[0]["Fsale_bank_type"].ToString());
                    ds.Tables[0].Rows[0]["Fadjust_flag"] = TransferMeaning.Transfer.convertAdjustSign(ds.Tables[0].Rows[0]["Fadjust_flag"].ToString());
                    string tradeType = TransferMeaning.Transfer.convertPayType(ds.Tables[0].Rows[0]["Ftrade_type"].ToString());
                    ds.Tables[0].Rows[0]["Ftrade_type"] = TransferMeaning.Transfer.convertPayType(ds.Tables[0].Rows[0]["Ftrade_type"].ToString());

                    try
                    {
                        //退款类型
                        string bargain_time = ds.Tables[0].Rows[0]["Fbargain_time"].ToString();
                        if (!string.IsNullOrEmpty(bargain_time))
                        {
                            DateTime begindate = DateTime.Parse(bargain_time.Trim());
                            string strBeginTime = begindate.AddDays(-7).ToString("yyyy-MM-dd 00:00:00");
                            string strEndTime = begindate.AddDays(8).ToString("yyyy-MM-dd 23:59:59");

                            DataSet tmpDS = GetB2cReturnList("", strBeginTime, strEndTime, 99, 99, tradeid, "", "0000", 99, "", 1, 1, 10);

                            if (tmpDS != null && tmpDS.Tables.Count > 0)
                            {
                                ds.Tables[0].Rows[0]["Frefund_typeName"] = tmpDS.Tables[0].Rows[0]["Frefund_typeName"].ToString();
                            }
                            else
                            {
                                ds.Tables[0].Rows[0]["Frefund_typeName"] = "";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log4net.ILog tmpLog = log4net.LogManager.GetLogger("查询退款类型");
                        tmpLog.ErrorFormat("查询退款类型：出错：{0} ", ex);
                    }


                    DataTable wx_dt = null;
                    try
                    {
                        wx_dt = new WechatPayService().QueryWxTrans(ds.Tables[0].Rows[0]["Flistid"].ToString()); //查询微信转账业务
                    }
                    catch (Exception ex)
                    {
                        log4net.ILog tmpLog = log4net.LogManager.GetLogger("查询微信转账业务");
                        tmpLog.ErrorFormat("查询微信转账业务：出错：{0} ", ex);
                    }
                    if (wx_dt != null && wx_dt.Rows.Count > 0)
                    {
                        ds.Tables[0].Rows[0]["Fcoding"] = PublicRes.objectToString(wx_dt, "wx_trade_id");//子账户关联订单号
                        string scene = PublicRes.objectToString(wx_dt, "scene");//区分微信转账，面对面付款
                        if (scene == "0")
                        {
                            ds.Tables[0].Rows[0]["Fexplain"] = "微信转账";
                        }
                        else
                        {
                            ds.Tables[0].Rows[0]["Fexplain"] = "面对面付款";
                        }
                        //通过卖家交易单反查付款方
                        ds.Tables[0].Rows[0]["Fbuyid"] = PublicRes.objectToString(wx_dt, "pay_openid");
                    }
                    else
                    {
                        ds.Tables[0].Rows[0]["Fexplain"] = ds.Tables[0].Rows[0]["Fmemo"].ToString();
                    }

                    //查询卖家财付通账号
                    string fcoding = ds.Tables[0].Rows[0]["Fcoding"].ToString();
                    FastPayService fpService = new FastPayService();//fcoding订单编码
                    if (fcoding != "" && ds.Tables[0].Rows[0]["Fsale_name"].ToString().Trim() == "微信转账业务中转账户")//该笔为微信转账
                    {
                        DataSet dsCoinWalPay = fpService.CoinWalletsPaymentQuery(fcoding);
                        if (dsCoinWalPay != null & dsCoinWalPay.Tables.Count > 0 & dsCoinWalPay.Tables[0].Rows.Count > 0)
                        {
                            ds.Tables[0].Rows[0]["FsaleidCFT"] = dsCoinWalPay.Tables[0].Rows[0]["rcv_openid"].ToString();
                        }
                    }

                    //交易状态
                    if (ds.Tables[0].Rows[0]["Flistid"].ToString() != "")
                    {
                        var listID = ds.Tables[0].Rows[0]["Flistid"].ToString();
                        DataSet dsState = new TradeData().GetQueryListDetail(listID);

                        if (dsState != null && dsState.Tables.Count > 0 && dsState.Tables[0].Rows.Count > 0)
                        {
                            dsState.Tables[0].Columns.Add("Ftrade_stateName");
                            TransferMeaning.Transfer.GetColumnValueFromDic(dsState.Tables[0], "Ftrade_state", "Ftrade_stateName", "PAY_STATE");
                            ds.Tables[0].Rows[0]["Ftrade_stateName"] = dsState.Tables[0].Rows[0]["Ftrade_stateName"].ToString();
                            if (isC2C)
                            {
                                int PersonInfoDayCount = Int32.Parse(ConfigurationManager.AppSettings["PersonInfoDayCount"].Trim());

                                var dsList = new TradeData().GetBankRollList_withID(DateTime.Now.AddDays(-PersonInfoDayCount), DateTime.Now.AddDays(1), listID, 1, 50);
                                bool isRefund = false;
                                bool isCompelete = false;
                                if (dsList != null && dsList.Tables.Count > 0 && dsList.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow row in dsList.Tables[0].Rows)
                                    {
                                        var state = row["Fsubject"].ToString();
                                        int stateNum = 0;
                                        if (int.TryParse(state, out stateNum))
                                        {
                                            if (stateNum == 5 || stateNum == 6)
                                            {
                                                isRefund = true;
                                            }
                                            else if (stateNum == 3 || stateNum == 4 || stateNum == 8)
                                            {
                                                isCompelete = true;
                                            }
                                        }
                                    }

                                }

                                if (isRefund)
                                {
                                    ds.Tables[0].Rows[0]["Ftrade_stateName"] = "转入退款";
                                }
                                else if (isCompelete)
                                {
                                    ds.Tables[0].Rows[0]["Ftrade_stateName"] = "交易完成";
                                }
                            }
                        }
                    }


                    try
                    {
                        string listid = tradeid;
                        string qry_type = "";
                        bool isOK = false;
                        if (tradeType.Contains("B2C"))
                        {
                            qry_type = "1";
                            isOK = true;
                        }
                        else if (tradeType.Contains("商户结算"))
                        {
                            qry_type = "2";
                            string IsReconfig = System.Configuration.ConfigurationManager.AppSettings["HandQ_Payment_IsReconfig"].ToString();
                            if (IsReconfig == "1")
                            {
                                listid = ds.Tables[0].Rows[0]["Fcoding"].ToString();
                            }
                            isOK = true;
                        }

                        if (isOK)
                        {
                            DataSet ds2 = new TradeService().QueryPaymentParty(listid, "", qry_type, "");
                            //DataSet ds = new TradeService().QueryPaymentParty("1301278501201412090010900888", "", "2", "");
                            //qry_type = "2";
                            if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                            {
                                if (ds2.Tables[0].Rows[0]["result"].ToString() == "0")
                                {
                                    if (qry_type == "1")
                                    {
                                        ds.Tables[0].Rows[0]["FsaleidCFT"] = ds2.Tables[0].Rows[0]["seller_uin"].ToString();
                                    }
                                    else if (qry_type == "2")
                                    {
                                        ds.Tables[0].Rows[0]["FbuyidCFT"] = ds2.Tables[0].Rows[0]["payer_uin"].ToString();
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log4net.ILog tmpLog = log4net.LogManager.GetLogger("调用接口：查询用户转账单记录失败！");
                        tmpLog.ErrorFormat("查询用户转账单记录失败：出错：{0} ", ex);
                    }

                    return ds;

                    #endregion
                }

                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                log4net.ILog tmpLog = log4net.LogManager.GetLogger("查询交易记录");
                tmpLog.ErrorFormat("查询交易记录：出错：{0} ", ex);
                return null;
            }

        }

        /// <summary>
        /// 获取B2C退款查询列表函数
        /// </summary>
        /// <param name="spid"></param>
        /// <param name="begintime"></param>
        /// <param name="endtime"></param>
        /// <param name="refundtype"></param>
        /// <param name="status"></param>
        /// <param name="tranid"></param>
        /// <param name="buyqq"></param>
        /// <param name="banktype"></param>
        /// <param name="sumtype"></param>
        /// <param name="drawid"></param>
        /// <param name="queryTable"></param>
        /// <param name="istart"></param>
        /// <param name="imax"></param>
        /// <returns></returns>
        public DataSet GetB2cReturnList(string spid, string begintime, string endtime, int refundtype, int status,
                   string tranid, string buyqq, string banktype, int sumtype, string drawid, int queryTable, int istart, int imax)
        {
            try
            {
                DataSet ds = null;
                if (queryTable == 3)//查询快照表 andrew 20120712
                {

                    B2cReturnClass cuser = new B2cReturnClass(spid, begintime, endtime, refundtype, status, tranid, buyqq, banktype, sumtype, drawid);
                    ds = cuser.GetResultX(istart, imax, "ZWTK");

                }
                else if (queryTable == 1)//商户退款申请表
                {
                    if (spid.Trim() == "" && tranid.Trim() == "" && drawid.Trim() == "")
                    {
                        throw new Exception("商户号、交易单号、退单号不能全部为空！");
                    }

                    if (spid.Trim() == "" && tranid.Trim() == "" && drawid.Trim() != "")//只有drawid
                    {
                        //drawid 不为空，先通过drawId查询listid
                        string comSql = "draw_id=" + drawid;
                        string msg = "";
                        tranid = CommQuery.GetOneResultFromICE(comSql, CommQuery.QUERY_REFUND_RELATION, "Ftransaction_id", out msg);

                        if (tranid == null || tranid.Trim() == "")
                        {
                            throw new Exception("通过退款单号：" + drawid + "未查询到对应的交易单号！");
                        }
                    }


                    if (tranid.Trim() != "")//交易单不为空
                    {
                        B2cReturnClass cuser = new B2cReturnClass(spid, begintime, endtime, refundtype, status, tranid, buyqq, banktype, sumtype, drawid);

                        int start = istart - 1;
                        if (start < 0) start = 0;

                        string strSql = cuser.ICESQL + "&strlimit=limit " + start + "," + imax;
                        string errMsg = "";
                        ds = CommQuery.GetDataSetFromICE(strSql, CommQuery.QUERY_MCH_REFUND, out errMsg);
                    }
                    else
                    {
                        B2cReturnClass cuser = new B2cReturnClass(spid, begintime, endtime, refundtype, status, tranid, buyqq, banktype, sumtype, drawid);
                        int start = istart - 1;
                        if (start < 0) start = 0;

                        string strSql = cuser.ICESQL + "&strlimit=limit " + start + "," + imax;
                        string errMsg = "";
                        ds = CommQuery.GetDataSetFromICE(strSql, CommQuery.QUERY_USER_REFUND, out errMsg);
                    }
                }
                else if (queryTable == 2)//拍拍退款申请表
                {
                    B2cReturnClass cuser = new B2cReturnClass(spid, begintime, endtime, refundtype, status, tranid, buyqq, banktype, sumtype, drawid);

                    int start = istart - 1;
                    if (start < 0) start = 0;

                    string strSql = cuser.ICESQL + "&strlimit=limit " + start + "," + imax;
                    string errMsg = "";
                    ds = CommQuery.GetDataSetFromICE(strSql, CommQuery.QUERY_PAIPAI_REFUND, out errMsg);
                }


                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
                {

                    #region 转化操作

                    DataTable dt = ds.Tables[0];

                    GetRefundType(dt);

                    #endregion

                    return ds;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                throw;
                //return null;
            }
        }

        private void GetRefundType(DataTable dt)
        {
            dt.Columns.Add("Frefund_typeName", typeof(String));

            foreach (DataRow dr in dt.Rows)
            {
                string tmp = dr["Frefund_type"].ToString();
                if (tmp == "1")
                {
                    dr["Frefund_typeName"] = "B2C退款";
                }
                else if (tmp == "2")
                {
                    dr["Frefund_typeName"] = "我要付款";
                }
                else if (tmp == "3")
                {
                    dr["Frefund_typeName"] = "银行直接退款";
                }
                else if (tmp == "4")
                {
                    dr["Frefund_typeName"] = "提现退款";
                }
                else if (tmp == "5")
                {
                    dr["Frefund_typeName"] = "信用卡退款";
                }
                else if (tmp == "6")
                {
                    dr["Frefund_typeName"] = "分帐退款";
                }
                else if (tmp == "9")
                {
                    dr["Frefund_typeName"] = "充值单退款";
                }
                else if (tmp == "11")
                {
                    dr["Frefund_typeName"] = "拍拍退单";
                }
                else
                {
                    dr["Frefund_typeName"] = "未知类型" + tmp;
                }

            }
        }
        /// <summary>
        /// 查询用户转账单记录
        /// </summary>
        /// <param name="listid">单号</param>
        /// <param name="state">付款单状态列表，以逗号分隔（当为空时，则不过滤状态）具体状态含义如下:1下单,2支付成功,
        ///                     3 付款成功,4 退款申请中,5 已退款,12 充值成功,6,7 非终结状态注：针对用户注销来说，传入1,2,12,4,6,7</param>
        /// <param name="qry_type">查询类型:1 支付单号,2 B2C转账单号（注意：重构前传入为核心转账单号，
        ///                        重构后传入为转账商户订单号）,3 按uin+state查询（查询最近一单符合要求的付款单，最多查三个月内的）</param>
        /// <param name="uin"></param>
        /// <returns></returns>
        public DataSet QueryPaymentParty(string listid, string state, string qry_type, string uin)
        {
            return (new TradeData()).QueryPaymentParty(listid, state, qry_type, uin);
        }
        /// <summary>
        /// 查询 转账是否可以注销  
        /// </summary>
        /// <param name="qqid"></param>
        /// <returns>true 可以注销</returns>
        public bool QueryWXUnfinishedTrade(string qqid)
        {
            return (new TradeData()).QueryWXUnfinishedTrade(qqid);
        }

        /// <summary>
        /// 查询 红包是否可以注销
        /// </summary>
        /// <param name="qqid"></param>
        /// <returns>true 可以注销</returns>
        public bool QueryWXUnfinishedHB(string qqid)
        {
            return (new TradeData()).QueryWXUnfinishedHB(qqid);
        }

        /// <summary>
        /// 查询  是否有微信还款的未完成交易 
        /// </summary>
        /// <param name="qqid"></param>
        /// <returns>true 可以注销</returns>
        public bool QueryWXFCancelAsRepayMent(string qqid, out string resultCodeString)
        {
             return (new TradeData()).QueryWXFCancelAsRepayMent(qqid,out resultCodeString);
        }

        ///// <summary>
        ///// 指定时间内用户是否存在未完成的红包
        ///// </summary>
        ///// <param name="WeChatName">用户的微信红包Openid</param>
        ///// <param name="beginTime">开始时间</param>
        ///// <param name="endTime">结束</param>
        ///// <returns>true 存在  false 不存在</returns>
        //public bool QueryWXHasUnfinishedHB(string openid, DateTime beginTime, DateTime endTime)
        //{
        //    //  15天以内 用户发送了红包并且红包状态在 2:PAYOK ,3:PARTRECEIVE,7:REFUNDING  状态下 不能注销
        //    var dal = new WechatPayService();
        //    const int max = 100;  //最多查询次数
        //    const int size = 20; //页大小
        //    for (int i = 0; i < max; i++)
        //    {
        //        var start = i * size;
        //        var ds = dal.QueryUserSendList(openid, beginTime, endTime, start, size);
        //        if (ds == null || ds.Tables.Count == 0)
        //        {
        //            return false;
        //        }
        //        var arr = ds.Tables[0].Select("State in('PAYOK','PARTRECEIVE','REFUNDING')");  //如果发现State 值等于 2:PAYOK ,3:PARTRECEIVE,7:REFUNDING 立即返回真
        //        if (arr.Length != 0)
        //        {
        //            return true;
        //        }
        //        if (ds.Tables[0].Rows.Count < size)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        public DataSet GetUnfinishedMobileQHB(string uin)
        {
            return (new TradeData()).GetUnfinishedMobileQHB(uin);
        }

        public DataSet GetUnfinishedMobileQTransfer(string uin)
        {
            return (new TradeData()).GetUnfinishedMobileQTransfer(uin);
        }
        #endregion

        /// <summary>
        /// 判断是否存在未完成交易
        /// </summary>
        /// <param name="u_QQID"></param>
        /// <param name="Fcurtype"></param>
        /// <returns></returns>
        public bool LogOnUsercheckOrder(string u_QQID, string Fcurtype)
        {
            return new TradeData().LogOnUsercheckOrder(u_QQID, Fcurtype);
        }

        /// <summary>
        /// 查询腾讯收款记录表
        /// </summary>
        public DataSet GetTCBankRollList(string u_ID, int u_IDType, DateTime u_BeginTime, DateTime u_EndTime, bool isHistory, int istr, int imax)
        {
            try
            {
                DataSet ds = new DataSet();

                float fnum = float.Parse("0");
                float fnummax = float.Parse("20000000.00");

                if (u_IDType == 0)
                {
                    ds = new TradeData().GetBankRollListByListId(u_ID, "qq", 1, u_BeginTime, u_EndTime, 0, fnum, fnummax, "0000", "0", istr, imax);
                }
                else
                {
                    ds = new TradeData().GetBankRollListByListId(u_ID, "czd", 1, u_BeginTime, u_EndTime, 0, fnum, fnummax, "0000", "0", istr, imax);
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add("total");
                    ds.Tables[0].Rows[0]["total"] = ds.Tables[0].Rows.Count;
                }

                if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
                {
                    return null;
                }
                else
                {
                    ds.Tables[0].Columns.Add("Fcurtype_str", typeof(string));
                    ds.Tables[0].Columns.Add("Fstate_str", typeof(string));
                    ds.Tables[0].Columns.Add("Ftype_str", typeof(string));
                    ds.Tables[0].Columns.Add("Fsubject_str", typeof(string));
                    ds.Tables[0].Columns.Add("Fsign_str", typeof(string));
                    ds.Tables[0].Columns.Add("Fbank_type_str", typeof(string));
                    ds.Tables[0].Columns.Add("Fnum_str", typeof(string));

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr["Fcurtype_str"] = TransferMeaning.Transfer.convertMoney_type(dr["Fcurtype"].ToString());
                        dr["Fstate_str"] = TransferMeaning.Transfer.convertTCState(dr["Fstate"].ToString());
                        dr["Ftype_str"] = TransferMeaning.Transfer.convertTradeType(dr["Ftype"].ToString());
                        dr["Fsubject_str"] = TransferMeaning.Transfer.convertSubject(dr["Fsubject"].ToString());
                        dr["Fsign_str"] = TransferMeaning.Transfer.convertTradeSign(dr["Fsign"].ToString());
                        dr["Fbank_type_str"] = TransferMeaning.Transfer.convertbankType(dr["Fbank_type"].ToString());

                        dr["Fnum_str"] = MoneyTransfer.FenToYuan(dr["Fnum"].ToString());
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("查询腾讯收款记录表失败!: " + ex);
                return null;
            }
        }
        /// <summary>
        /// 新的充值记录查询
        /// </summary>
        /// <param name="u_ID"></param>
        /// <param name="u_IDType"></param>
        /// <param name="u_BeginTime"></param>
        /// <param name="u_EndTime"></param>
        /// <param name="istr"></param>
        /// <param name="imax"></param>
        /// <returns></returns>
        public DataSet GetBankRollListByListId(string u_ID, string u_QueryType, int? fcurtype, DateTime u_BeginTime, DateTime u_EndTime, int fstate,
              float? fnum, float? fnumMax, string banktype, int iPageStart, int iPageMax,string uid="")
        {
            DataSet ds = new TradeData().GetBankRollListByListId_New(u_ID, u_QueryType, fcurtype, u_BeginTime, u_EndTime,
                            fstate, fnum, fnumMax, banktype, iPageStart, iPageMax, uid);

            if ((u_QueryType == "toBank" || u_QueryType == "BankBack") && !u_ID.ToUpper().StartsWith("CFT"))
            {
                for (int i = 1; i < 9; i++)
                {
                    string newUID = "CFT0" + i.ToString() + u_ID;
                    DataSet tmpDS = new TradeData().GetBankRollListByListId_New(newUID, u_QueryType, fcurtype, u_BeginTime, u_EndTime,
                                        fstate, fnum, fnumMax, banktype, iPageStart, iPageMax, uid);

                    if (tmpDS != null && tmpDS.Tables.Count > 0 && tmpDS.Tables[0].Rows.Count > 0)
                    {
                        ds = PublicRes.ToOneDataset(ds, tmpDS);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return ds;
        }

        /// <summary>
        /// 查询用户帐户流水表
        /// </summary>       
        public DataSet GetUserRollList(string u_QQID, int type, int fcurtype, DateTime u_BeginTime, DateTime u_EndTime, int istr, int imax)
        {
            string u_type = "";
            string ref_param="";
            try
            {
                if (type == 0)
                    u_type = "qqid";
                if (type == 1)
                    u_type = "listid";

                DataSet ds = new DataSet();
                if (u_type == "qqid")
                {
                    ds = GetChildrenBankRollList(u_QQID, u_BeginTime, u_EndTime, fcurtype.ToString(), istr, imax);

                    if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
                    {

                        ds = GetBankRollList(u_QQID, "", u_BeginTime, u_EndTime,"", istr, imax, ref ref_param);

                        if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (ds.Tables[0].Rows[i]["Fpaynum"].ToString().Trim() == "0")
                                {
                                    ds.Tables[0].Rows[i].Delete();
                                    i--;
                                }
                            }
                        }

                    }
                }

                if (u_type == "listid")
                {
                    ds = GetBankRollList_withID(u_BeginTime, u_EndTime, u_QQID, istr, imax);
                }

                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                {
                    ds.Tables[0].Columns.Add("Faction_type_str", typeof(string));
                    ds.Tables[0].Columns.Add("Fcurtype_str", typeof(string));
                    ds.Tables[0].Columns.Add("Ftype_str", typeof(string));  //交易类型
                    ds.Tables[0].Columns.Add("Fsubject_str", typeof(string));
                    ds.Tables[0].Columns.Add("Fpaynum_str", typeof(string));
                    ds.Tables[0].Columns.Add("Fbalance_str", typeof(string));
                    if (u_type == "qqid")
                        ds.Tables[0].Columns.Add("Fcon_str", typeof(string));

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr["Faction_type_str"] = TransferMeaning.Transfer.convertActionType(dr["Faction_type"].ToString());
                        dr["Fcurtype_str"] = TransferMeaning.Transfer.convertMoney_type(dr["Fcurtype"].ToString());
                        dr["Ftype_str"] = TransferMeaning.Transfer.convertTradeType(dr["Ftype"].ToString());
                        dr["Fsubject_str"] = TransferMeaning.Transfer.convertSubject(dr["Fsubject"].ToString());
                        dr["Fpaynum_str"] = MoneyTransfer.FenToYuan(dr["Fpaynum"].ToString());
                        dr["Fbalance_str"] = MoneyTransfer.FenToYuan(dr["Fbalance"].ToString());
                        if (u_type == "qqid")
                            dr["Fcon_str"] = MoneyTransfer.FenToYuan(dr["Fcon"].ToString());
                    }

                    #region 权限问题

                    //string[] CoverPickFuid = System.Configuration.ConfigurationManager.AppSettings["CoverPickFuid"].ToString().Split('|');

                    //foreach (DataRow dr in ds.Tables[0].Rows)
                    //{
                    //    try
                    //    {
                    //        string Fpaynum = MoneyTransfer.FenToYuan(dr["Fpaynum"].ToString());
                    //        string Fbalance = MoneyTransfer.FenToYuan(dr["Fbalance"].ToString());
                    //        dr["FpaynumStr"] = Fpaynum;
                    //        dr["FbalanceStr"] = Fbalance;
                    //        for (int i = 0; i < CoverPickFuid.Length; i++)
                    //        {
                    //            if (CoverPickFuid[i].ToString() == dr["Fuid"].ToString())
                    //            {
                    //                try
                    //                {
                    //                    int PointIndex = Fpaynum.IndexOf(".");
                    //                    dr["FpaynumStr"] = "******" + Fpaynum.Substring(PointIndex - 1, Fpaynum.Length - PointIndex + 1);
                    //                    PointIndex = Fbalance.IndexOf(".");
                    //                    dr["FbalanceStr"] = "******" + Fbalance.Substring(PointIndex - 1, Fbalance.Length - PointIndex + 1);
                    //                }
                    //                catch
                    //                {
                    //                    dr["FpaynumStr"] = "******";
                    //                    dr["FbalanceStr"] = "******";
                    //                }
                    //            }
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        throw new Exception("金额Fpaynum：" + dr["Fnum"].ToString() + "金额Fbalance：" + dr["Fbalance"].ToString() + "转换有问题" + ex.Message);
                    //    }
                    //}

                    #endregion
                }


                return ds;
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("查询用户帐户流水失败!: " + ex);
                return null;
            }
        }
      
        /// <summary>
        /// 查询退款单表
        /// </summary>
        public DataSet GetRefund(string u_ID, int u_IDType, DateTime u_BeginTime, DateTime u_EndTime, int istr, int imax)
        {
            try
            {
                DataSet ds = new TradeData().GetRefund(u_ID, u_IDType, u_BeginTime, u_EndTime, istr, imax);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Columns.Add("Fpay_type_str", typeof(string));
                    ds.Tables[0].Columns.Add("Fbuy_bank_type_str", typeof(string));
                    ds.Tables[0].Columns.Add("Fsale_bank_type_str", typeof(string));
                    ds.Tables[0].Columns.Add("Fsale_bankid_str", typeof(string));
                    ds.Tables[0].Columns.Add("Fstate_str", typeof(string));
                    ds.Tables[0].Columns.Add("Flstate_str", typeof(string));
                    ds.Tables[0].Columns.Add("Fpaybuy_str", typeof(string));
                    ds.Tables[0].Columns.Add("Fpaysale_str", typeof(string));
                    ds.Tables[0].Columns.Add("Fprocedure_str", typeof(string));

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr["Fpay_type_str"] = TransferMeaning.Transfer.cPay_type(dr["Fpay_type"].ToString());
                        dr["Fbuy_bank_type_str"] = TransferMeaning.Transfer.convertbankType(dr["Fbuy_bank_type"].ToString());
                        dr["Fsale_bank_type_str"] = TransferMeaning.Transfer.convertbankType(dr["Fsale_bank_type"].ToString());
                        dr["Fsale_bankid_str"] = TransferMeaning.Transfer.convertbankType(dr["Fsale_bankid"].ToString());
                        dr["Fstate_str"] = TransferMeaning.Transfer.cRefundState(dr["Fstate"].ToString());
                        dr["Flstate_str"] = TransferMeaning.Transfer.cRlistState(dr["Flstate"].ToString());
                        dr["Fpaybuy_str"] = MoneyTransfer.FenToYuan(dr["Fpaybuy"].ToString());
                        dr["Fpaysale_str"] = MoneyTransfer.FenToYuan(dr["Fpaysale"].ToString());
                        dr["Fprocedure_str"] = MoneyTransfer.FenToYuan(dr["Fprocedure"].ToString());
                    }
                }

                return ds;
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("查询退款单失败: " + ex);
                return null;
            }
        }
        
        /// <summary>
        /// 查询用户帐户流水表_WithListID
        /// </summary>    
        public DataSet GetBankRollList_withID(DateTime u_BeginTime, DateTime u_EndTime, string ListID, int istr, int imax)
        {
            return new TradeData().GetBankRollList_withID(u_BeginTime, u_EndTime, ListID, istr, imax);
        }

        /// <summary>
        /// 子帐户资金流水查询函数
        /// </summary>      
        public DataSet GetChildrenBankRollList(string u_QQID, DateTime u_BeginTime, DateTime u_EndTime, string Fcurtype, int istr, int imax)
        {
            try
            {
                string reply = "";
                DataSet ds = new DataSet();
                string fuid = PublicRes.ConvertToFuid(u_QQID);

                bool isOk = new TradeData().GetChildrenBankRollList(u_QQID, u_BeginTime, u_EndTime, Fcurtype, istr, imax, 0, string.Empty, out reply);

                if (isOk && reply.StartsWith("result=0&res_info=ok"))
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Flistid", typeof(string));
                    dt.Columns.Add("Ftype", typeof(string));
                    dt.Columns.Add("Fspid", typeof(string));
                    dt.Columns.Add("Fbalance", typeof(string));
                    dt.Columns.Add("Fpaynum", typeof(string));
                    dt.Columns.Add("Fbank_type", typeof(string));
                    dt.Columns.Add("Fsubject", typeof(string));
                    dt.Columns.Add("Faction_type", typeof(string));
                    dt.Columns.Add("Fmemo", typeof(string));
                    dt.Columns.Add("Fcreate_time", typeof(string));
                    dt.Columns.Add("Ffromid", typeof(string));
                    dt.Columns.Add("Fvs_qqid", typeof(string));
                    dt.Columns.Add("Ffrom_name", typeof(string));
                    dt.Columns.Add("Fpaynum1", typeof(string));
                    dt.Columns.Add("Fpaynum2", typeof(string));
                    dt.Columns.Add("FbalanceNum", typeof(string));

                    dt.Columns.Add("total", typeof(string));
                    dt.Columns.Add("Fcurtype", typeof(string));
                    dt.Columns.Add("Fexplain", typeof(string));
                    dt.Columns.Add("FBKid", typeof(string));
                    dt.Columns.Add("Fuid", typeof(string));
                    dt.Columns.Add("Fqqid", typeof(string));
                    dt.Columns.Add("Ftrue_name", typeof(string));
                    dt.Columns.Add("Ffrom_uid", typeof(string));
                    dt.Columns.Add("Fprove", typeof(string));
                    dt.Columns.Add("Fapplyid", typeof(string));
                    dt.Columns.Add("Fip", typeof(string));
                    dt.Columns.Add("Fmodify_time_acc", typeof(string));
                    dt.Columns.Add("Fmodify_time", typeof(string));
                    dt.Columns.Add("Fcon", typeof(string));

                    XmlDocument param = new XmlDocument();
                    param.LoadXml(reply.Replace("result=0&res_info=ok&rec_info=", ""));

                    XmlNodeList tmpElement = param.GetElementsByTagName("record");
                    if (tmpElement != null && tmpElement.Count > 0)
                    {
                        for (int i = 0; i < tmpElement.Count; i++)
                        {
                            DataRow dr = dt.NewRow();

                            foreach (XmlNode node in tmpElement[i].ChildNodes)
                            {
                                string strvalue = node.InnerText.Trim();
                                if (node.Name.Trim() == "Flistid")
                                {
                                    dr["Flistid"] = strvalue;
                                }
                                else if (node.Name.Trim() == "Ffromid")
                                {
                                    dr["Ffromid"] = strvalue;
                                }
                                else if (node.Name.Trim() == "Ftype")
                                {
                                    dr["Ftype"] = strvalue;
                                }
                                else if (node.Name.Trim() == "Fspid")
                                {
                                    dr["Fspid"] = strvalue;
                                }
                                else if (node.Name.Trim() == "Fbalance")
                                {
                                    dr["Fbalance"] = strvalue;
                                }
                                else if (node.Name.Trim() == "Fpaynum")
                                {
                                    dr["Fpaynum"] = strvalue;
                                }
                                else if (node.Name.Trim() == "Fbank_type")
                                {
                                    dr["Fbank_type"] = strvalue;
                                }
                                else if (node.Name.Trim() == "Fsubject")
                                {
                                    dr["Fsubject"] = strvalue;
                                }
                                else if (node.Name.Trim() == "Faction_type")
                                {
                                    dr["Faction_type"] = strvalue;
                                }
                                else if (node.Name.Trim() == "Fmemo")
                                {
                                    dr["Fmemo"] = strvalue;
                                }
                                else if (node.Name.Trim() == "Fcreate_time")
                                {
                                    dr["Fcreate_time"] = strvalue;
                                }
                                else if (node.Name.Trim() == "Ffrom_name")
                                {
                                    dr["Ffrom_name"] = strvalue;
                                }
                                else if (node.Name.Trim() == "Fcon")
                                {
                                    dr["Fcon"] = strvalue;
                                }
                            }

                            if (dr["Ftype"].ToString() == "1")
                            {
                                dr["Fpaynum1"] = MoneyTransfer.FenToYuan(dr["Fpaynum"].ToString());
                                dr["FbalanceNum"] = MoneyTransfer.FenToYuan(dr["Fbalance"].ToString());

                                dr["Fvs_qqid"] = "";
                            }
                            else
                            {
                                dr["Fpaynum2"] = MoneyTransfer.FenToYuan(dr["Fpaynum"].ToString());
                                dr["FbalanceNum"] = MoneyTransfer.FenToYuan(dr["Fbalance"].ToString());

                                dr["Fspid"] = "";
                            }
                            dr["total"] = "1000";
                            dr["FBKid"] = "";
                            dr["Fuid"] = fuid;
                            dr["Fqqid"] = u_QQID;
                            dr["Ftrue_name"] = "";
                            dr["Ffrom_uid"] = "";
                            dr["Fprove"] = "";
                            dr["Fapplyid"] = "";
                            dr["Fip"] = "";
                            dr["Fcurtype"] = Fcurtype;
                            dr["Fexplain"] = "";
                            dr["Fvs_qqid"] = "";
                            dr["Fmodify_time_acc"] = dr["Fcreate_time"];
                            dr["Fmodify_time_acc"] = dr["Fcreate_time"];
                            dt.Rows.Add(dr);
                        }
                    }
                    ds.Tables.Add(dt);
                }

                return ds;
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("子帐户资金流水查询失败: " + ex);
                return null;
            }
        }

        public DataSet GetListidFromUserOrder(string qqid, string uid, int start, int max,int type)
        {
            return (new TradeData()).GetListidFromUserOrder(qqid, uid, start, max,type);
        }
        public DataSet GetQueryList(DateTime u_BeginTime, DateTime u_EndTime, string buyqq, string saleqq, string buyqqInnerID, string saleqqInnerID,
          string u_QueryType, string queryvalue, int fstate, int fcurtype, int start, int max)
        {
            return (new TradeData()).GetQueryList(u_BeginTime, u_EndTime, buyqq, saleqq, buyqqInnerID, saleqqInnerID,
            u_QueryType, queryvalue, fstate, fcurtype, start, max);
        }
        public DataSet GetManJianUsingList(string u_ID, int u_IDType, DateTime u_BeginTime, DateTime u_EndTime, string banktype, int istr, int imax)
        {
            return (new TradeData()).GetManJianUsingList(u_ID, u_IDType, u_BeginTime, u_EndTime, banktype, istr, imax);
        }
        public DataSet Q_PAY_LIST(string strID, int iIDType, DateTime dtBegin, DateTime dtEnd, int istr, int imax,string fuid="")
        {
            return (new TradeData()).Q_PAY_LIST(strID, iIDType, dtBegin, dtEnd, istr, imax, fuid);
        }
        public DataSet MediListQueryClass(string u_ID, string Fcode, string strBeginTime, string strEndTime, string u_UserFilter
          , string u_OrderBy, int limStart, int limCount)
        {
            return (new TradeData()).MediListQueryClass(u_ID, Fcode, strBeginTime, strEndTime, u_UserFilter, u_OrderBy, limStart, limCount);
        }
        public DataSet QueryBusCardPrepaid(string beginDate, string endDate, int PageSize, string uin, string listid, string cardid, out string errMsg)
        {
            return (new TradeData()).QueryBusCardPrepaid(beginDate, endDate, PageSize, uin, listid, cardid, out errMsg);
        }
        public DataSet GetBankRollList(string u_QQID, string fuid, DateTime u_BeginTime, DateTime u_EndTime,string ftype, int istr, int imax, ref  string ref_param)
        {
            return (new TradeData()).GetBankRollList(u_QQID, fuid, u_BeginTime, u_EndTime ,ftype, istr, imax, ref ref_param);
        }

        public DataSet GetFundCardListDetail(string flistid, string fsupplylist, string fcarrdid, int offset, int limit)
        {
            DataSet ds = new TradeData().GetFundCardListDetail(flistid, fsupplylist, fcarrdid, offset, limit);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("FStateName", typeof(System.String));
                ds.Tables[0].Columns.Add("FSignName", typeof(System.String));
                ds.Tables[0].Columns.Add("FNumYuan", typeof(System.String));
                ds.Tables[0].Columns.Add("FCardtypeName", typeof(System.String));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr["FNumYuan"] = !string.IsNullOrEmpty(dr["FNumYuan"].ToString()) ? MoneyTransfer.FenToYuan(dr["Fnum"].ToString()) : "0";
                    string tmp = dr["Fstate"].ToString();
                    if (tmp == "1")
                    {
                        dr["FStateName"] = "付款前";
                    }   
                    if (tmp == "2")
                    {
                        dr["FStateName"] = "付款后";
                    }

                    tmp = dr["Fsign"].ToString();
                    if (tmp == "1")
                    {
                        dr["FSignName"] = "销卡成功";
                    }
                    if (tmp == "2")
                    {
                        dr["FSignName"] = "销卡失败";
                    }
                    if (tmp == "3")
                    {
                        dr["FSignName"] = "初始化";
                    }

                    tmp = dr["Fcard_type"].ToString();
                    if (tmp == "1")
                    {
                        dr["FCardtypeName"] = "移动卡";
                    }
                    if (tmp == "2")
                    {
                        dr["FCardtypeName"] = "联通卡";
                    }
                }
            }

            return ds;
        }

        /// <summary>
        /// 查询 转账单
        /// </summary>
        /// <param name="uin">单号</param>    
        /// <param name="queryType">查询类型 财付通订单号: 1,  商户订单号:2 </param>
        /// <returns></returns>
        public DataSet TransferQuery(string uin, int queryType)
        {
            Dictionary<string, string> dic_listType = new Dictionary<string, string>()
            {
                {"1", "微信红包"},
                {"2", "微信AA收款"},
                {"3", "微信转账"},
                {"4", "手Q红包"},
                {"5", "手Q AA收款"},
                {"6", "手Q转账"},
            };
           var ds=new TradeData().TransferQuery(uin, queryType);
           if (ds != null && ds.Tables.Count > 0)
           {
               var dt = ds.Tables[0];
               dt.Columns.Add("Flist_type_str", typeof(string));
               dt.Columns.Add("Ftotalnum_str", typeof(string));
               foreach (DataRow row in dt.Rows)
               {
                   string fts = row["Flist_type"] as string;
                   row["Flist_type_str"] = dic_listType.ContainsKey(fts) ? dic_listType[fts] : "未知";
                   row["Ftotalnum_str"] = MoneyTransfer.FenToYuan((string)row["Ftotalnum"]);
               }
           }
           return ds;
        }

        /// <summary>
        /// 微粒贷查询-是否有未还清的欠款
        /// </summary>
        /// <param name="uin">QQ号</param>
        /// <returns>true 存在, false 不存在</returns>
        public bool HasUnfinishedWeiLibDai(string uin)
        {
            int state = new TradeData().WeiLibDaiQuery(uin);
            return state != 0;  //0:无未还清欠款, 1:有未还清欠款
        }
     
        /// <summary>
        /// 查询交易单表
        /// </summary>
        /// <param name="listid"></param>
        /// <returns></returns>
        public DataSet GetPayByListid(string listid)
        {
            return new TradeData().GetPayByListid(listid);
        }

        //解冻拍拍保证金
        public bool IsIceOutPPSecurtyMoney(string uin, string transactionId, out string msg)
        {
            DataSet ds = new TradeData().IsIceOutPPSecurtyMoney(uin, transactionId);
            bool ret = false;
            msg = "";
            try
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["result"].ToString()) && int.Parse(ds.Tables[0].Rows[0]["result"].ToString()) == 0)
                    {
                        ret = true;
                    }
                    msg = ds.Tables[0].Rows[0]["res_info"].ToString();
                }
            }
            catch (Exception e)
            {
                throw new Exception("解冻拍拍保证金IsIceOutPPSecurtyMoney：" + e.Message);
            }
            return ret;        
        }

    }
}
