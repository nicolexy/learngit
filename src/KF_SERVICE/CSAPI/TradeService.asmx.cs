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

                var infos = new CFT.CSOMS.BLL.TradeModule.TradeService().GetTradeList(tradeid, 4, DateTime.Now);
                if (infos == null || infos.Tables.Count == 0 || infos.Tables[0].Rows.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<Payment.TradePayList> list = APIUtil.ConvertTo<Payment.TradePayList>(infos.Tables[0]);
                APIUtil.Print<Payment.TradePayList>(list);
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
        /// 根据银行返回定单号查询交易记录
        /// </summary>
        [WebMethod]
        public void GetBankTradeList()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "tradeid", "date", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string tradeid = paramsHt.ContainsKey("tradeid") ? paramsHt["tradeid"].ToString() : "";
                DateTime time = APIUtil.StrToDate(paramsHt["date"].ToString());

                var infos = new CFT.CSOMS.BLL.TradeModule.TradeService().GetTradeList(tradeid, 1, time);
                if (infos == null || infos.Tables.Count == 0 || infos.Tables[0].Rows.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<Payment.TradePayList> list = APIUtil.ConvertTo<Payment.TradePayList>(infos.Tables[0]);
                APIUtil.Print<Payment.TradePayList>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetBankTradeList").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetBankTradeList").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
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
                APIUtil.ValidateParamsNew(paramsHt, "appid", "ID", "id_type", "begin_time", "end_time", "offset", "limit", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string ID = paramsHt.ContainsKey("ID") ? paramsHt["ID"].ToString() : "";
                int type = APIUtil.StringToInt(paramsHt["id_type"].ToString());
                DateTime begin_time = APIUtil.StrToDate(paramsHt["begin_time"].ToString());
                DateTime end_time = APIUtil.StrToDate(paramsHt["end_time"].ToString());
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
                APIUtil.ValidateParamsNew(paramsHt, "appid", "offset", "limit", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string qqid = paramsHt.ContainsKey("qqid") ? paramsHt["qqid"].ToString() : "";
                int type = APIUtil.StringToInt(paramsHt["id_type"].ToString());
                int cur_type = APIUtil.StringToInt(paramsHt["cur_type"].ToString());
                DateTime begin_time = APIUtil.StrToDate(paramsHt["begin_time"].ToString());
                DateTime end_time = APIUtil.StrToDate(paramsHt["end_time"].ToString());
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

                if (type == 0)
                {
                    List<Trade.QQRollList> list = APIUtil.ConvertTo<Trade.QQRollList>(infos.Tables[0]);
                    APIUtil.Print<Trade.QQRollList>(list);
                }
                if (type == 1)
                {
                    List<Trade.IDRollList> list = APIUtil.ConvertTo<Trade.IDRollList>(infos.Tables[0]);
                    APIUtil.Print<Trade.IDRollList>(list);
                }

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
                APIUtil.ValidateParamsNew(paramsHt, "appid", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string ID = paramsHt.ContainsKey("ID") ? paramsHt["ID"].ToString() : "";
                int type = APIUtil.StringToInt(paramsHt["type"].ToString());
                DateTime begin_time = APIUtil.StrToDate(paramsHt["begin_time"].ToString());
                DateTime end_time = APIUtil.StrToDate(paramsHt["end_time"].ToString());
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

                var infos = new CFT.CSOMS.BLL.TradeModule.TradeService().GetTCBankPAYList(ID, type, begin_time, end_time, offset, limit);
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
                APIUtil.ValidateParamsNew(paramsHt, "appid", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string ID = paramsHt.ContainsKey("ID") ? paramsHt["ID"].ToString() : "";
                int type = APIUtil.StringToInt(paramsHt["type"].ToString());
                DateTime begin_time = APIUtil.StrToDate(paramsHt["begin_time"].ToString());
                DateTime end_time = APIUtil.StrToDate(paramsHt["end_time"].ToString());
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

        #region 邮政汇款

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

        #endregion
    }
}
