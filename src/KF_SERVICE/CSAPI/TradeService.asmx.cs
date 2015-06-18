﻿using System;
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

        [WebMethod(Description = "根据交易单号查询交易记录")]
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

        [WebMethod(Description = "根据银行返回定单号查询交易记录")]
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


        #region 邮政汇款

        [WebMethod]
        public void GetRemitDataList()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "spid", "offset","limit","token");
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
