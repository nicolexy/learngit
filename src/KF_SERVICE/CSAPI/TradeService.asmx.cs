using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
using CFT.CSOMS.Service.CSAPI.Utility;
using System.IO;
using CFT.CSOMS.Service.CSAPI.Language;
using CFT.CSOMS.Service.CSAPI.PayMent;
using System.Collections;
using CFT.CSOMS.Service.CSAPI.BaseInfo;
using System.Collections.Specialized;
using System.Data;
using CFT.CSOMS.Service.CSAPI.Trade;

namespace CSAPI
{
    /// <summary>
    /// Summary description for TradeService
    /// </summary>
    [WebService(Namespace = "")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class TradeService : System.Web.Services.WebService
    {
        #region 网银
        [WebMethod]
        public void AddRefundInfo()
        {
            try
            {
                string FOrderId = ""; //财付通订单号
                int FRefund_type = 1; //退款类型
                string FSam_no = "";//SAM工单号
                string FRecycle_user = "";//回收人
                string FSubmit_user = ""; //登记人
                string FRefund_amount = "0"; //退款金额
                string memo = "";//备注
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();

                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "FOrderId", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                if (paramsHt.Keys.Contains("FOrderId"))
                {
                    FOrderId = paramsHt["FOrderId"].ToString();
                }
                if (paramsHt.Keys.Contains("FRefund_type"))
                {
                    FRefund_type = int.Parse(paramsHt["FRefund_type"].ToString());
                }
                if (paramsHt.Keys.Contains("FSam_no"))
                {
                    FSam_no = paramsHt["FSam_no"].ToString();
                }
                if (paramsHt.Keys.Contains("FRecycle_user"))
                {
                    FRecycle_user = paramsHt["FRecycle_user"].ToString();
                }
                if (paramsHt.Keys.Contains("FRefund_amount"))
                {
                    FRefund_amount = paramsHt["FRefund_amount"].ToString();
                }
                if (paramsHt.Keys.Contains("memo"))
                {
                    memo = paramsHt["memo"].ToString();
                }
                //调用bll层方法
                bool infos = new CFT.CSOMS.BLL.InternetBank.InternetBankService().AddRefundInfo(FOrderId, FRefund_type, FSam_no, FRecycle_user, FSubmit_user, FRefund_amount, memo);

                //返回值
                Record record = new Record();
                record.RetValue = infos.ToString().ToLower();
                List<Record> list = new List<Record>();
                list.Add(record);

                APIUtil.Print<Record>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("AddRefundInfo").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("AddRefundInfo").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }
        #endregion

        #region 交易单,资金流水等

        /// <summary>
        /// 各类交易单 0,买家交易单;9卖家交易单;10,中介交易单;-1,买家未完成交易单;-2,卖家未完成交易单
        /// </summary>
        [WebMethod]
        public void GetTradeList()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "tradeid", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string tradeid = paramsHt.ContainsKey("tradeid") ? paramsHt["tradeid"].ToString() : "";

                if (paramsHt.ContainsKey("list_time"))
                {
                    //根据银行返回定单号查询交易记录
                    DateTime list_time = APIUtil.StrToDate(paramsHt["list_time"].ToString());
                    var infos = new CFT.CSOMS.BLL.TradeModule.TradeService().GetTradeList(tradeid, 1, list_time, DateTime.Now, DateTime.Now, 1, 2);
                    if (infos == null || infos.Tables.Count == 0 || infos.Tables[0].Rows.Count == 0)
                    {
                        throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                    }
                    List<Payment.TradePayList> list = APIUtil.ConvertTo<Payment.TradePayList>(infos.Tables[0]);
                    APIUtil.Print<Payment.TradePayList>(list);
                }
                else//根据订单号查询交易记录
                {
                    var infos = new CFT.CSOMS.BLL.TradeModule.TradeService().GetTradeList(tradeid, 4, DateTime.Now, DateTime.Now, DateTime.Now, 1, 2);
                    if (infos == null || infos.Tables.Count == 0 || infos.Tables[0].Rows.Count == 0)
                    {
                        throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                    }
                    List<Payment.TradePayList> list = APIUtil.ConvertTo<Payment.TradePayList>(infos.Tables[0]);
                    APIUtil.Print<Payment.TradePayList>(list);
                }
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetTradeList").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetTradeList").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 各类交易单 0,买家交易单;9卖家交易单;10,中介交易单;-1,买家未完成交易单;-2,卖家未完成交易单
        /// </summary>
        [WebMethod]
        public void GetTradeListByType()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "id", "type", "begin_time", "end_time", "offset", "limit", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string id = paramsHt.ContainsKey("id") ? paramsHt["id"].ToString() : "";
                int type = APIUtil.StringToInt(paramsHt["type"].ToString());
                DateTime begin_time = APIUtil.StrToDate(paramsHt["begin_time"].ToString());
                DateTime end_time = APIUtil.StrToDate(paramsHt["end_time"].ToString());
                int offset = APIUtil.StringToInt(paramsHt["offset"].ToString());
                int limit = APIUtil.StringToInt(paramsHt["limit"].ToString());

                int days = (end_time - begin_time).Days;
                if (days > 184)
                {
                    end_time = begin_time.AddDays(184);
                }
                end_time.AddDays(1);

                if (offset < 0)
                {
                    offset = 0;
                }
                if (limit < 0 || limit > 1000)
                {
                    limit = 20;
                }

                var infos = new CFT.CSOMS.BLL.TradeModule.TradeService().GetTradeList(id, type, DateTime.Now, begin_time, end_time, offset, limit);
                if (infos == null || infos.Tables.Count == 0 || infos.Tables[0].Rows.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<Payment.TradePayList> list = APIUtil.ConvertTo<Payment.TradePayList>(infos.Tables[0]);
                APIUtil.Print<Payment.TradePayList>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetTradeListByType").ErrorFormat("return_code:{0},msg{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetTradeListByType").ErrorFormat("return_code:{0},msg{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 充值记录查询 0,QQID;1,ListID
        /// </summary>
        [WebMethod]
        public void GetTCBankRollList()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "ID", "type", "begin_time", "end_time", "offset", "limit", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string ID = paramsHt.ContainsKey("ID") ? paramsHt["ID"].ToString() : "";
                int type = APIUtil.StringToInt(paramsHt["type"].ToString());
                DateTime begin_time = APIUtil.StrToDate(paramsHt["begin_time"].ToString());
                DateTime end_time = APIUtil.StrToDate(paramsHt["end_time"].ToString());
                //将查询时间跨度设置在3个月以内,最长3个月92天
                TimeSpan ts = end_time - begin_time;
                int days = ts.Days;
                if (days > 92)
                {
                    end_time = begin_time.AddDays(92);
                }
                end_time = end_time.AddDays(1);
                int offset = APIUtil.StringToInt(paramsHt["offset"].ToString());
                int limit = APIUtil.StringToInt(paramsHt["limit"].ToString());

                if (offset < 0)
                {
                    offset = 0;
                }
                if (limit < 0)
                {
                    limit = 20;
                }

                var infos = new CFT.CSOMS.BLL.TradeModule.TradeService().GetTCBankRollList(ID, type, begin_time, end_time, true, offset, limit);

                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<Payment.TCBankRollList> list = APIUtil.ConvertTo<Payment.TCBankRollList>(infos.Tables[0]);
                APIUtil.Print<Payment.TCBankRollList>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetTCBankRollList").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetTCBankRollList").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 用户资金流水 0,QQID;1,ListID
        /// </summary>
        [WebMethod]
        public void GetBankRollList()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "ID", "type", "cur_type", "begin_time", "end_time", "offset", "limit", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string qqid = paramsHt.ContainsKey("ID") ? paramsHt["ID"].ToString() : "";
                int type = APIUtil.StringToInt(paramsHt["type"].ToString());
                int cur_type = APIUtil.StringToInt(paramsHt["cur_type"].ToString());
                DateTime begin_time = APIUtil.StrToDate(paramsHt["begin_time"].ToString());
                DateTime end_time = APIUtil.StrToDate(paramsHt["end_time"].ToString());
                //将查询时间跨度设置在3个月以内,最长3个月92天
                TimeSpan ts = end_time - begin_time;
                int days = ts.Days;
                if (days > 92)
                {
                    end_time = begin_time.AddDays(92);
                }
                end_time = end_time.AddDays(1);
                int offset = APIUtil.StringToInt(paramsHt["offset"].ToString());
                int limit = APIUtil.StringToInt(paramsHt["limit"].ToString());

                if (offset < 0)
                {
                    offset = 0;
                }
                if (limit < 0)
                {
                    limit = 20;
                }

                var infos = new CFT.CSOMS.BLL.TradeModule.TradeService().GetUserRollList(qqid, type, cur_type, begin_time, end_time, offset, limit);
                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<Trade.QQRollList> list = APIUtil.ConvertTo<Trade.QQRollList>(infos.Tables[0]);
                APIUtil.Print<Trade.QQRollList>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetBankRollList").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetBankRollList").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 提现记录 0,QQID;1,ListID
        /// </summary>
        [WebMethod]
        public void GetTCBankPAYList()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "ID", "type", "begin_time", "end_time", "offset", "limit", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string ID = paramsHt.ContainsKey("ID") ? paramsHt["ID"].ToString() : "";
                int type = APIUtil.StringToInt(paramsHt["type"].ToString());
                DateTime begin_time = APIUtil.StrToDate(paramsHt["begin_time"].ToString());
                DateTime end_time = APIUtil.StrToDate(paramsHt["end_time"].ToString());
                //将查询时间跨度设置在一个月之内,月初查询到月底
                end_time.AddDays(1);    //需要加一天,不然当天的记录查询不到
                int month = end_time.Month - begin_time.Month;
                if (month != 0)
                {
                    DateTime tmpDate = new DateTime(begin_time.Year, begin_time.Month, 1);
                    end_time = tmpDate.AddMonths(1).AddDays(-1);
                }
               
                int offset = APIUtil.StringToInt(paramsHt["offset"].ToString());
                int limit = APIUtil.StringToInt(paramsHt["limit"].ToString());

                if (offset < 0)
                {
                    offset = 0;
                }
                if (limit < 0 || limit > 100)
                {
                    limit = 20;
                }

                var infos = new CFT.CSOMS.BLL.TradeModule.PickService().GetTCBankPAYList(ID, type, begin_time, end_time, offset, limit);
                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<Trade.TXRollList> list = APIUtil.ConvertTo<Trade.TXRollList>(infos.Tables[0]);
                APIUtil.Print<Trade.TXRollList>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetTCBankPAYList").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetTCBankPAYList").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 退款单 0,买家退款单;1,卖家退款单
        /// </summary>
        [WebMethod]
        public void GetRefund()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "ID", "type", "begin_time", "end_time", "offset", "limit", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string ID = paramsHt.ContainsKey("ID") ? paramsHt["ID"].ToString() : "";
                int type = APIUtil.StringToInt(paramsHt["type"].ToString());
                DateTime begin_time = APIUtil.StrToDate(paramsHt["begin_time"].ToString());
                DateTime end_time = APIUtil.StrToDate(paramsHt["end_time"].ToString());
                //将查询时间跨度设置在半年以内,最长半年184天
                TimeSpan ts = end_time - begin_time;
                int days = ts.Days;
                if (days > 184)
                {
                    end_time = begin_time.AddDays(184);
                }
                end_time = end_time.AddDays(1);
                int offset = APIUtil.StringToInt(paramsHt["offset"].ToString());
                int limit = APIUtil.StringToInt(paramsHt["limit"].ToString());

                if (offset < 0)
                {
                    offset = 0;
                }
                if (limit < 0)
                {
                    limit = 20;
                }

                var infos = new CFT.CSOMS.BLL.TradeModule.TradeService().GetRefund(ID, type, begin_time, end_time, offset, limit);
                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }

                List<Trade.TKRollList> list = APIUtil.ConvertTo<Trade.TKRollList>(infos.Tables[0]);
                APIUtil.Print<Trade.TKRollList>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetRefund").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetRefund").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        #endregion

        #region 生活缴费

        [WebMethod]
        public void GetRemitDataList()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "spid", "offset", "limit", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string spid = paramsHt.ContainsKey("spid") ? paramsHt["spid"].ToString() : "";
                string remit_rec = paramsHt.ContainsKey("remit_rec") ? paramsHt["remit_rec"].ToString() : "";
                string list_id = paramsHt.ContainsKey("list_id") ? paramsHt["list_id"].ToString() : "";
                int offset = APIUtil.StringToInt(paramsHt["offset"].ToString());
                int limit = APIUtil.StringToInt(paramsHt["limit"].ToString());
                if (offset < 0)
                {
                    offset = 0;
                }
                if (limit < 0)
                {
                    limit = 20;
                }

                string trade_type = paramsHt.ContainsKey("trade_type") ? paramsHt["trade_type"].ToString() : "99";
                string trade_state = paramsHt.ContainsKey("trade_state") ? paramsHt["trade_state"].ToString() : "99";
                string remit_type = paramsHt.ContainsKey("remit_type") ? paramsHt["remit_type"].ToString() : "99";
                string data_type = paramsHt.ContainsKey("data_type") ? paramsHt["data_type"].ToString() : "99";

                var infos = new CFT.CSOMS.BLL.RemitModule.RemitService().GetRemitDataList("", trade_type, data_type, remit_type, trade_state, spid, remit_rec, list_id, offset, limit);
                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<Payment.RemitList> list = APIUtil.ConvertTo<Payment.RemitList>(infos.Tables[0]);
                APIUtil.Print<Payment.RemitList>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetRemitDataList").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetRemitDataList").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        [WebMethod]
        public void GetRemitSpid()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                var infos = new CFT.CSOMS.BLL.RemitModule.RemitService().GetRemitSpid();
                if (infos == null || infos.Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<Payment.RemitSpid> list = APIUtil.ConvertTo<Payment.RemitSpid>(infos);
                APIUtil.Print<Payment.RemitSpid>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetRemitSpid").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetRemitSpid").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 手机充值卡查询
        /// </summary>
        [WebMethod]
        public void GetFundCardListDetail()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "offset", "limit", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string listid = paramsHt.ContainsKey("listid") ? paramsHt["listid"].ToString() : "";        //充值单号
                string supply_list = paramsHt.ContainsKey("supply_list") ? paramsHt["supply_list"].ToString() : "";  //给供应商订单号
                string cardid = paramsHt.ContainsKey("cardid") ? paramsHt["cardid"].ToString() : "";        //充值卡序列号
                int offset = paramsHt.ContainsKey("offset") ? APIUtil.StringToInt(paramsHt["offset"].ToString()) : 0;
                int limit = paramsHt.ContainsKey("limit") ? APIUtil.StringToInt(paramsHt["limit"].ToString()) : 1;

                var infos = new CFT.CSOMS.BLL.TradeModule.TradeService().GetFundCardListDetail(listid, supply_list, cardid, offset, limit);
                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<Trade.FundCardListDetail> list = APIUtil.ConvertTo<Trade.FundCardListDetail>(infos.Tables[0]);
                APIUtil.Print<Trade.FundCardListDetail>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetFundCardListDetail").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetFundCardListDetail").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 信用卡还款查询-列表
        /// </summary>
        [WebMethod]
        public void GetCreditQueryListUin()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "uin", "begin_time", "end_time", "offset", "limit", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string uin = paramsHt.ContainsKey("uin") ? paramsHt["uin"].ToString() : "";
                DateTime begin_time = paramsHt.ContainsKey("begin_time") ? APIUtil.StrToDate(paramsHt["begin_time"].ToString()) : DateTime.Now;
                DateTime end_time = paramsHt.ContainsKey("end_time") ? APIUtil.StrToDate(paramsHt["end_time"].ToString()) : DateTime.Now;
                int offset = paramsHt.ContainsKey("offset") ? APIUtil.StringToInt(paramsHt["offset"].ToString()) : 0;
                int limit = paramsHt.ContainsKey("limit") ? APIUtil.StringToInt(paramsHt["limit"].ToString()) : 10;

                if (offset < 0)
                    offset = 0;
                if (limit < 0 || limit > 100)
                    limit = 100;

                var infos = new CFT.CSOMS.BLL.TradeModule.PickService().GetCreditQueryListForFaid(uin, begin_time, end_time, offset, limit);

                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<Trade.CreditQueryList> list = APIUtil.ConvertTo<Trade.CreditQueryList>(infos.Tables[0]);
                APIUtil.Print<Trade.CreditQueryList>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetCreditQueryListUin").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetCreditQueryListUin").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 信用卡还款查询-详情
        /// </summary>
        [WebMethod]
        public void GetCreditQueryList()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "listid", "offset", "limit", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string listid = paramsHt.ContainsKey("listid") ? paramsHt["listid"].ToString() : "";
                int offset = paramsHt.ContainsKey("offset") ? APIUtil.StringToInt(paramsHt["offset"].ToString()) : 0;
                int limit = paramsHt.ContainsKey("limit") ? APIUtil.StringToInt(paramsHt["limit"].ToString()) : 10;

                if (offset < 0)
                    offset = 0;
                if (limit < 0 || limit > 100)
                    limit = 100;

                var infos = new CFT.CSOMS.BLL.TradeModule.PickService().GetCreditQueryList(listid, offset, limit);
                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<Trade.CreditQueryList> list = APIUtil.ConvertTo<Trade.CreditQueryList>(infos.Tables[0]);
                APIUtil.Print<Trade.CreditQueryList>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetCreditQueryList").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetCreditQueryList").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 自动充值查询
        /// </summary>
        [WebMethod]
        public void QueryAutomaticRecharge()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "uin", "offset", "limit", "ip", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string uin = paramsHt.ContainsKey("uin") ? paramsHt["uin"].ToString() : "";
                int offset = paramsHt.ContainsKey("offset") ? APIUtil.StringToInt(paramsHt["offset"].ToString()) : 0;
                int limit = paramsHt.ContainsKey("limit") ? APIUtil.StringToInt(paramsHt["limit"].ToString()) : 10;
                string ip = paramsHt.ContainsKey("ip") ? paramsHt["ip"].ToString() : "";

                if (offset < 0)
                    offset = 0;
                if (limit < 0 || limit > 100)
                    limit = 100;

                var infos = new CFT.CSOMS.BLL.AutoRecharge.AutoRechargeService().QueryAutomaticRecharge(uin, offset, limit, ip);
                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<Trade.AutomaticRecharge> list = APIUtil.ConvertTo<Trade.AutomaticRecharge>(infos.Tables[0]);
                APIUtil.Print<Trade.AutomaticRecharge>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("QueryAutomaticRecharge").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("QueryAutomaticRecharge").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 自动充值查询
        /// </summary>
        [WebMethod]
        public void QueryAutomaticRechargeDetial()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "uin", "plan_id", "offset", "limit", "ip", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string uin = paramsHt.ContainsKey("uin") ? paramsHt["uin"].ToString() : "";
                string plan_id = paramsHt.ContainsKey("plan_id") ? paramsHt["plan_id"].ToString() : "";
                int offset = paramsHt.ContainsKey("offset") ? APIUtil.StringToInt(paramsHt["offset"].ToString()) : 0;
                int limit = paramsHt.ContainsKey("limit") ? APIUtil.StringToInt(paramsHt["limit"].ToString()) : 10;
                string ip = paramsHt.ContainsKey("ip") ? paramsHt["ip"].ToString() : "";

                if (offset < 0)
                    offset = 0;
                if (limit < 0 || limit > 100)
                    limit = 100;

                var infos = new CFT.CSOMS.BLL.AutoRecharge.AutoRechargeService().QueryAutomaticRechargeBillList(uin, plan_id, offset, limit, ip);
                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<Trade.AutomaticRechargeDetail> list = APIUtil.ConvertTo<Trade.AutomaticRechargeDetail>(infos.Tables[0]);
                APIUtil.Print<Trade.AutomaticRechargeDetail>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("QueryAutomaticRechargeDetial").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("QueryAutomaticRechargeDetial").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        #endregion

        #region 银行卡查询

        /// <summary>
        /// 查询银行名称及类型编码
        /// </summary>
        [WebMethod]
        public void RequestBankInfo()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                Hashtable ht = new CFT.CSOMS.BLL.WechatPay.FastPayService().RequestBankInfo();
                ArrayList array = new ArrayList(ht.Keys);
                array.Sort();
                List<RecordNew> list = new List<RecordNew>();
                foreach (string s in array)
                {
                    RecordNew record = new RecordNew();
                    record.RetValue = s;
                    record.RetMemo = ht[s].ToString();
                    list.Add(record);
                }
                APIUtil.Print<RecordNew>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("RequestBankInfo").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("RequestBankInfo").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        [WebMethod]
        public void QueryBankCardNewList()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "bank_card", "bank_type", "bit_type", "date", "offset", "limit", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string bank_card = paramsHt.ContainsKey("bank_card") ? paramsHt["bank_card"].ToString() : "";
                string bank_type = paramsHt.ContainsKey("bank_type") ? paramsHt["bank_type"].ToString() : "";
                int bit_type = paramsHt.ContainsKey("bit_type") ? APIUtil.StringToInt(paramsHt["bit_type"].ToString()) : 10100;
                DateTime date = paramsHt.ContainsKey("date") ? APIUtil.StrToDate(paramsHt["date"].ToString()) : DateTime.Now;
                string date_str = date.ToString("yyyyMMdd");
                int offset = paramsHt.ContainsKey("offset") ? APIUtil.StringToInt(paramsHt["offset"].ToString()) : 0;
                int limit = paramsHt.ContainsKey("limit") ? APIUtil.StringToInt(paramsHt["limit"].ToString()) : 10;

                if (offset < 0)
                    offset = 0;
                if (limit < 0 || limit > 1000)
                    limit = 100;

                var infos = new CFT.CSOMS.BLL.WechatPay.FastPayService().QueryBankCardNewList(bank_card, date_str, bank_type, bit_type, offset, limit);
                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<Trade.BankCardList> list = APIUtil.ConvertTo<Trade.BankCardList>(infos.Tables[0]);
                APIUtil.Print<Trade.BankCardList>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("QueryBankCardNewList").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("QueryBankCardNewList").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }
        #endregion

    }
}
