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

namespace CFT.CSOMS.BLL.TradeModule
{
   
    public class TradeService
    {
        public DataSet BeforeCancelTradeQuery(string uid)
        {
            if (string.IsNullOrEmpty(uid.Trim()) && uid.Trim().Length<3)
            {
                throw new Exception("内部id有误");
            }

            return new TradeData().BeforeCancelTradeQuery(uid);

        }
        public int ControledFinRemoveLogInsert(string qqid, string FbalanceStr, string FtypeText, string curtype, DateTime FmodifyTime, string FupdateUser)
        {
            return new TradeData().RemoveControledFinLogInsert(qqid, FbalanceStr, FtypeText, curtype, FmodifyTime, FupdateUser);
        }
        public void RemoveControledFinLogInsertAll(string qqid, string uid, DataTable dt) 
        {
            foreach (DataRow item in dt.Rows)
            {
                new TradeData().RemoveControledFinLogInsert(qqid, item["FbalanceStr"].ToString(), item["FtypeText"].ToString(), item["cur_type"].ToString(), DateTime.Now, uid);
            }
        }
        public DataSet RemoveControledFinLogQuery(string qqid) 
        {
            return new TradeData().RemoveControledFinLogQuery(qqid);
        }

        public DataSet QueryWxBuyOrderByUid(int uid, DateTime startTime, DateTime endTime)
        {
            return (new TradeData()).QueryWxBuyOrderByUid(uid, startTime, endTime);
        }
        #region 交易记录查询

        public DataSet GetTradeList(string tradeid, int typeid, DateTime time)
        {
            try
            {
                if (typeid.ToString() == "1") //根据银行返回定单号查询
                {
                    tradeid = new TradeData().ConvertToListID(tradeid, time);
                }
                int istr = 1;
                int imax = 2;
                DateTime beginTime = DateTime.Parse(ConfigurationManager.AppSettings["sBeginTime"].ToString());
                DateTime endTime = DateTime.Parse(ConfigurationManager.AppSettings["sEndTime"].ToString());
                //TODO:有个makelog的方法不是主需求
                DataSet ds = new TradeData().GetTradeList(tradeid, typeid, beginTime, endTime, istr, imax);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
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

                    PublicService.PublicService.GetColumnValueFromDic(ds.Tables[0], "Fpay_type", "Fpay_type_str", "PAY_TYPE");//支付类型

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

                    ds.Tables[0].Rows[0]["Fbuy_bank_type"] = PublicService.PublicService.convertbankType(ds.Tables[0].Rows[0]["Fbuy_bank_type"].ToString());
                    ds.Tables[0].Rows[0]["Fcurtype"] = PublicService.PublicService.convertMoney_type(ds.Tables[0].Rows[0]["Fcurtype"].ToString());
                    ds.Tables[0].Rows[0]["Flstate"] = PublicService.PublicService.convertTradeState(ds.Tables[0].Rows[0]["Flstate"].ToString());
                    ds.Tables[0].Rows[0]["Fsale_bank_type"] = PublicService.PublicService.convertbankType(ds.Tables[0].Rows[0]["Fsale_bank_type"].ToString());
                    ds.Tables[0].Rows[0]["Fadjust_flag"] = PublicService.PublicService.convertAdjustSign(ds.Tables[0].Rows[0]["Fadjust_flag"].ToString());
                    string tradeType = PublicService.PublicService.convertPayType(ds.Tables[0].Rows[0]["Ftrade_type"].ToString());
                    ds.Tables[0].Rows[0]["Ftrade_type"] = PublicService.PublicService.convertPayType(ds.Tables[0].Rows[0]["Ftrade_type"].ToString());

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
                        tmpLog.ErrorFormat("查询退款类型：出错：{0} ", ex.Message);
                    }


                    DataTable wx_dt = null;
                    try
                    {
                        wx_dt = new WechatPayService().QueryWxTrans(ds.Tables[0].Rows[0]["Flistid"].ToString()); //查询微信转账业务
                    }
                    catch (Exception ex)
                    {
                        log4net.ILog tmpLog = log4net.LogManager.GetLogger("查询微信转账业务");
                        tmpLog.ErrorFormat("查询微信转账业务：出错：{0} ", ex.Message);
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
                            PublicService.PublicService.GetColumnValueFromDic(dsState.Tables[0], "Ftrade_state", "Ftrade_stateName", "PAY_STATE");
                            ds.Tables[0].Rows[0]["Ftrade_stateName"] = dsState.Tables[0].Rows[0]["Ftrade_stateName"].ToString();
                            if (isC2C)
                            {
                                var dsList = new TradeData().GetBankRollList_withID(DateTime.Now.AddDays(-300), DateTime.Now.AddDays(1), listID, 1, 50);
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
                        tmpLog.ErrorFormat("查询用户转账单记录失败：出错：{0} ", ex.Message);
                    }

                    return ds;
                }

                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                log4net.ILog tmpLog = log4net.LogManager.GetLogger("查询交易记录");
                tmpLog.ErrorFormat("查询交易记录：出错：{0} ", ex.Message);
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
                throw new Exception(e.Message);
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
        ///                     3 付款成功,4 退款申请中,5 已退款,12 充值成功:注：针对用户注销来说，传入1,2,12,4</param>
        /// <param name="qry_type">查询类型:1 支付单号,2 B2C转账单号（注意：重构前传入为核心转账单号，
        ///                        重构后传入为转账商户订单号）,3 按uin+state查询（查询最近一单符合要求的付款单，最多查三个月内的）</param>
        /// <param name="uin"></param>
        /// <returns></returns>
        public DataSet QueryPaymentParty(string listid, string state, string qry_type, string uin)
        {
            return (new TradeData()).QueryPaymentParty(listid, state, qry_type, uin);
        }
        public int QueryWXUnfinishedTrade(string open_id)
        {
            return (new TradeData()).QueryWXUnfinishedTrade(open_id);
        }
        public int QueryWXUnfinishedHB(string WeChatName)
        {
            return (new TradeData()).QueryWXUnfinishedHB(WeChatName);
        }
        public DataSet GetUnfinishedMobileQHB(string uin)
        {
            return (new TradeData()).GetUnfinishedMobileQHB(uin);
        }
        #endregion

        public DataSet GetListidFromUserOrder(string qqid, string uid, int start, int max)
        {
            return (new TradeData()).GetListidFromUserOrder(qqid, uid, start, max);
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
        public DataSet Q_PAY_LIST(string strID, int iIDType, DateTime dtBegin, DateTime dtEnd, int istr, int imax)
        {
            return (new TradeData()).Q_PAY_LIST(strID, iIDType, dtBegin, dtEnd, istr, imax);
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
    }
}
