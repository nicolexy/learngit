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
using System.Configuration;
using System.Net;
using System.Data;
using CFT.CSOMS.COMMLIB;
using TENCENT.OSS.CFT.KF.Common;
using CFT.CSOMS.Service.CSAPI.BaseInfo;

namespace CFT.CSOMS.Service.CSAPI
{
    /// <summary>
    /// Summary description for WechatService
    /// </summary>
    [WebService(Namespace = "")]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class PaymentService : System.Web.Services.WebService
    {
        #region 理财通

        /// <summary>
        /// 获取用户余额收益列表
        /// </summary>
        [WebMethod]
        public void GetUserProfitList()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "trade_id", "currency_type", "offset", "limit", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                int offset = APIUtil.StringToInt(paramsHt["offset"].ToString());
                int limit = APIUtil.StringToInt(paramsHt["limit"].ToString());
                int currency_type = APIUtil.StringToInt(paramsHt["currency_type"].ToString());
                //校验时间格式
                APIUtil.ValidateDate(paramsHt["start_date"].ToString(), "yyyyMMdd", true);
                APIUtil.ValidateDate(paramsHt["end_date"].ToString(), "yyyyMMdd", true);

                if (offset < 0)
                {
                    offset = 0;
                }

                if (limit > 50 || limit < 0)
                {
                    limit = 50;
                }

                string start_date = "", end_date = "", spid = "";

                if (paramsHt.Keys.Contains("start_date"))
                {
                    start_date = paramsHt["start_date"].ToString();
                }
                if (paramsHt.Keys.Contains("end_date"))
                {
                    end_date = paramsHt["end_date"].ToString();
                }
                if (paramsHt.Keys.Contains("spid"))
                {
                    spid = paramsHt["spid"].ToString();
                }
                var infos = new CFT.CSOMS.BLL.FundModule.FundService().BindProfitList(paramsHt["trade_id"].ToString(), start_date, end_date, currency_type, spid, offset, limit);

                if (infos == null || infos.Rows.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }

                List<Payment.UserProfit> list = APIUtil.ConvertTo<Payment.UserProfit>(infos);
                APIUtil.Print<Payment.UserProfit>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetUserProfitList").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetUserProfitList").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        ///  通过账号查找交易ID
        /// </summary>
        [WebMethod]
        public void GetTradeIdByUIN()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "uin", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                var infos = new CFT.CSOMS.BLL.FundModule.FundService().GetTradeIdByUIN(paramsHt["uin"].ToString());

                if (string.IsNullOrEmpty(infos))
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }

                //多条记录使用此方式
                Record record = new Record();
                record.RetValue = infos;
                List<Record> list = new List<Record>();
                list.Add(record);
                APIUtil.Print<Record>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetTradeIdByUIN").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetTradeIdByUIN").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 查询安全卡信息
        /// </summary>
        [WebMethod]
        public void GetPayCardInfo()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "uin", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                var infos = new CFT.CSOMS.BLL.FundModule.FundService().GetPayCardInfo(paramsHt["uin"].ToString());
                if (infos == null || infos.Rows.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<Payment.PayCardInfo> list = APIUtil.ConvertTo<Payment.PayCardInfo>(infos);
                APIUtil.Print<Payment.PayCardInfo>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetPayCardInfo").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetPayCardInfo").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 查询用户交易流水
        /// </summary>
        [WebMethod]
        public void GetBankRollListNotChildren()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "uin", "cur_type", "start_date", "end_date", "redirection_type", "offset", "limit", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                int cur_type = APIUtil.StringToInt(paramsHt["cur_type"].ToString());
                int offset = APIUtil.StringToInt(paramsHt["offset"].ToString());
                int limit = APIUtil.StringToInt(paramsHt["limit"].ToString());
                int redirection_type = APIUtil.StringToInt(paramsHt["redirection_type"].ToString());
                string start_date = paramsHt["start_date"].ToString();
                string end_date = paramsHt["end_date"].ToString();


                if (offset < 0)
                {
                    offset = 0;
                }

                if (limit > 50 || limit < 0)
                {
                    limit = 50;
                }

                DateTime sDate = APIUtil.StrToDate(start_date);
                DateTime eDate = APIUtil.StrToDate(end_date);

                var infos = new CFT.CSOMS.BLL.FundModule.FundService().BindBankRollListNotChildren(paramsHt["uin"].ToString(), paramsHt["cur_type"].ToString(), sDate, eDate, offset, limit, redirection_type);
                if (infos == null || infos.Rows.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }

                List<Payment.UserTradePosInfo> list = APIUtil.ConvertTo<Payment.UserTradePosInfo>(infos);
                APIUtil.Print<Payment.UserTradePosInfo>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetBankRollListNotChildren").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetBankRollListNotChildren").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 用户各基金账户查询
        /// </summary>
        [WebMethod]
        public void GetUserFundSummary()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "uin", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                var infos = new CFT.CSOMS.BLL.FundModule.FundService().GetUserFundSummary(paramsHt["uin"].ToString());
                if (infos == null || infos.Rows.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<Payment.UserFundSummary> list = APIUtil.ConvertTo<Payment.UserFundSummary>(infos);
                APIUtil.Print<Payment.UserFundSummary>(list);

            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetUserFundSummary").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetUserFundSummary").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 查询定期基金交易明细
        /// </summary>
        [WebMethod]
        public void QueryCloseFundRollList()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "trade_id", "fund_code", "start_date", "end_date", "offset", "limit", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                int offset = APIUtil.StringToInt(paramsHt["offset"].ToString());
                int limit = APIUtil.StringToInt(paramsHt["limit"].ToString());
                //校验时间格式
                APIUtil.ValidateDate(paramsHt["start_date"].ToString(), "yyyyMMdd", false);
                APIUtil.ValidateDate(paramsHt["end_date"].ToString(), "yyyyMMdd", false);

                if (offset < 0)
                {
                    offset = 0;
                }

                if (limit > 50 || limit == 0)
                {
                    limit = 50;
                }

                var infos = new CFT.CSOMS.BLL.FundModule.FundService().QueryCloseFundRollList(paramsHt["trade_id"].ToString(), paramsHt["fund_code"].ToString(), paramsHt["start_date"].ToString(), paramsHt["end_date"].ToString(), offset, limit);
                if (infos == null || infos.Rows.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }

                List<Payment.CloseFundRoll> list = APIUtil.ConvertTo<Payment.CloseFundRoll>(infos);
                APIUtil.Print<Payment.CloseFundRoll>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("QueryCloseFundRollList").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("QueryCloseFundRollList").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        [WebMethod]
        public void GetUserFundAccountInfo()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "uin", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                var infos = new CFT.CSOMS.BLL.FundModule.FundService().GetUserFundAccountInfo(paramsHt["uin"].ToString());
                if (infos == null || infos.Rows.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<Payment.UserFundAccountInfo> list = APIUtil.ConvertTo<Payment.UserFundAccountInfo>(infos);
                APIUtil.Print<Payment.UserFundAccountInfo>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetUserFundAccountInfo").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetUserFundAccountInfo").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        [WebMethod]
        public void GetChildrenBankRollListEx()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "uin", "spid", "curtype", "start_date", "end_date", "ftype", "offset", "limit", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                int cur_type = APIUtil.StringToInt(paramsHt["curtype"].ToString());
                int offset = APIUtil.StringToInt(paramsHt["offset"].ToString());
                int limit = APIUtil.StringToInt(paramsHt["limit"].ToString());
                int ftype = APIUtil.StringToInt(paramsHt["ftype"].ToString());
                string start_date = paramsHt["start_date"].ToString();
                string end_date = paramsHt["end_date"].ToString();

                if (offset < 0)
                {
                    offset = 0;
                }

                if (limit > 50 || limit == 0)
                {
                    limit = 50;
                }
                DateTime sDate = APIUtil.StrToDate(start_date);
                DateTime eDate = APIUtil.StrToDate(end_date);

                var infos = new CFT.CSOMS.BLL.FundModule.FundService().GetChildrenBankRollListEx(paramsHt["uin"].ToString(), paramsHt["spid"].ToString(), paramsHt["curtype"].ToString(), sDate, eDate, offset, limit, ftype, "");
                if (infos == null || infos.Rows.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<Payment.ChildrenBankRoll> list = APIUtil.ConvertTo<Payment.ChildrenBankRoll>(infos);
                APIUtil.Print<Payment.ChildrenBankRoll>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetChildrenBankRollListEx").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetChildrenBankRollListEx").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        [WebMethod]
        public void GetLCTBalance()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "uin", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string uin = paramsHt.ContainsKey("uin") ? paramsHt["uin"].ToString() : "";

                var infos = new CFT.CSOMS.BLL.FundModule.LCTBalanceService().QuerySubAccountInfo(uin, 89);//查询理财通余额，币种89
                if (infos == null || infos.Rows.Count == 0)
                {
                    Payment.LCTBalance balance = new Payment.LCTBalance();
                    balance.Fbalance = "0";
                    List<Payment.LCTBalance> list = new List<Payment.LCTBalance>();
                    list.Add(balance);
                    APIUtil.Print<Payment.LCTBalance>(list);
                }
                else
                {
                    List<Payment.LCTBalance> list = APIUtil.ConvertTo<Payment.LCTBalance>(infos);
                    APIUtil.Print<Payment.LCTBalance>(list);
                }
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetLCTBalance").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetLCTBalance").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }
        #endregion

        #region 快捷支付

        [WebMethod]
        public void QueryBankCardList()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "bank_card", "date", "offset", "limit", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                int offset = APIUtil.StringToInt(paramsHt["offset"].ToString());
                int limit = APIUtil.StringToInt(paramsHt["limit"].ToString());

                APIUtil.ValidateDate(paramsHt["date"].ToString(), "yyyyMMdd", false);
                //offset limit可为-1
                if (offset != -1 && offset < 0)
                {
                    offset = 0;
                }

                if (limit > 50 || limit < 0)
                {
                    limit = 50;
                }

                //10100 目前接口只查询支付，后续接入其他业务类型，但是要增加接口参数
                var infos = new CFT.CSOMS.BLL.WechatPay.FastPayService().QueryBankCardList(paramsHt["bank_card"].ToString(), paramsHt["date"].ToString(), 10100, offset, limit);
                if (infos == null || infos.Tables.Count == 0 || infos.Tables[0].Rows.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<Payment.BankCard> list = APIUtil.ConvertTo<Payment.BankCard>(infos.Tables[0]);
                APIUtil.Print<Payment.BankCard>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("QueryBankCardList").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("QueryBankCardList").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        #endregion

        #region 一点通业务

        [WebMethod]
        public void GetBankDic()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                //查询银行字典参数
                var infos = new CFT.CSOMS.BLL.BankCardBindModule.BankCardBindService().GetBankDic();
                if (infos == null || infos.Tables.Count == 0 || infos.Tables[0].Rows.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }

                Dictionary<string, string> maps = new Dictionary<string, string>();
                maps.Add("fno", "no");
                maps.Add("ftype", "type");
                maps.Add("fvalue", "value");
                maps.Add("fmemo", "memo");
                maps.Add("fsymbol", "symbol");
                APIUtil.Print4DataTable(infos.Tables[0], null, maps);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetBankDic").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetBankDic").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        [WebMethod]
        public void SyncBankCardBind()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "bank_type", "bank_id", "card_tail", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                String bankType = paramsHt["bank_type"].ToString();
                String bank_id = paramsHt["bank_id"].ToString();
                String card_tail = paramsHt["card_tail"].ToString();

                //查询银行字典参数
                var infos = new CFT.CSOMS.BLL.BankCardBindModule.
                    BankCardBindService().SyncBankCardBind(bankType, card_tail, bank_id);

                if (infos == null || infos.Rows.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                APIUtil.Print4DataTable(infos, null, null);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("SyncBankCardBind").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("SyncBankCardBind").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        [WebMethod]
        public void UnbindBankCardBind()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "bank_type", "uin", "protocol_no", "userip", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                String bankType = paramsHt["bank_type"].ToString();
                String qqid = paramsHt["uin"].ToString();
                String protocolNo = paramsHt["protocol_no"].ToString();
                String userIP = paramsHt["userip"].ToString();

                //查询银行字典参数
                var infos = new CFT.CSOMS.BLL.BankCardBindModule.
                    BankCardBindService().UnbindBankCardBind(bankType, qqid, protocolNo, userIP);

                if (infos == null || infos.Rows.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                APIUtil.Print4DataTable(infos, null, null);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("UnbindBankCardBind").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("UnbindBankCardBind").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        [WebMethod]
        public void UnbingBankCardBindSpecial()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "bank_type", "uin", "card_tail", "bind_serialno", "protocol_no", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                String bankType = paramsHt["bank_type"].ToString();
                String qqid = paramsHt["uin"].ToString();
                String protocolNo = paramsHt["protocol_no"].ToString();
                String Fbind_serialno = paramsHt["bind_serialno"].ToString();
                String Fcard_tail = paramsHt["card_tail"].ToString();

                //解绑操作
                var infos = new CFT.CSOMS.BLL.BankCardBindModule.
                    BankCardBindService().UnBindBankCardBindSpecial(bankType, qqid, Fcard_tail, Fbind_serialno, protocolNo);

                if (infos == null || infos.Rows.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }

                APIUtil.Print4DataTable(infos, null, null);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("UnbingBankCardBindSpecial").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("UnbingBankCardBindSpecial").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        [WebMethod]
        public void GetBankCardBindList()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "query_type", "bind_state", "offset", "limit", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                String fuin = paramsHt.ContainsKey("uin") ? paramsHt["uin"].ToString() : "";

                String bankID = paramsHt.ContainsKey("bank_id") ? paramsHt["bank_id"].ToString() : "";
                String fuid = paramsHt.ContainsKey("uid") ? paramsHt["uid"].ToString() : "";
                String creType = paramsHt.ContainsKey("cre_type") ? paramsHt["cre_type"].ToString() : "";
                String creID = paramsHt.ContainsKey("cre_id") ? paramsHt["cre_id"].ToString() : "";
                String protocolno = paramsHt.ContainsKey("protocol_no") ? paramsHt["protocol_no"].ToString() : "";
                String phoneno = paramsHt.ContainsKey("phoneno") ? paramsHt["phoneno"].ToString() : "";
                String strBeginDate = paramsHt.ContainsKey("begin_date") ? paramsHt["begin_date"].ToString() : "";
                String strEndDate = paramsHt.ContainsKey("end_date") ? paramsHt["end_date"].ToString() : "";
                String Fbank_type = paramsHt.ContainsKey("bank_type") ? paramsHt["bank_type"].ToString() : "";

                int queryType = paramsHt.ContainsKey("query_type") ? APIUtil.StringToInt(paramsHt["query_type"]) : 0;
                int bindState = paramsHt.ContainsKey("bind_state") ? APIUtil.StringToInt(paramsHt["bind_state"]) : 99;

                APIUtil.ValidateDate(strBeginDate, "yyyy-MM-dd HH:mm:ss", true);
                APIUtil.ValidateDate(strBeginDate, "yyyy-MM-dd HH:mm:ss", true);

                int limStart = APIUtil.StringToInt(paramsHt["offset"].ToString());
                int limCount = APIUtil.StringToInt(paramsHt["limit"].ToString());
                if (limStart < 0)
                    limStart = 0;
                if (limCount < 0 || limCount > 50)
                    limCount = 50;

                //解绑操作
                var infos = new CFT.CSOMS.BLL.BankCardBindModule.BankCardBindService().GetBankCardBindList(
                    fuin, Fbank_type, bankID, fuid, creType, creID, protocolno, phoneno, strBeginDate, strEndDate, queryType, false, bindState, "", limStart, limCount);

                if (infos == null || infos.Tables.Count == 0 || infos.Tables[0].Rows.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }

                Dictionary<string, string> maps = new Dictionary<string, string>();

                maps.Add("fuin", "uin");
                maps.Add("fbank_id", "bank_id");
                maps.Add("fbank_type", "bank_type");
                maps.Add("fcard_tail", "card_tail");
                maps.Add("ftruename", "truename");

                maps.Add("fbank_status", "bank_status");
                maps.Add("fbind_type", "bind_type");
                maps.Add("fbind_status", "bind_status");
                maps.Add("fbind_flag", "bind_flag");
                maps.Add("fprotocol_no", "protocol_no");
                maps.Add("fbind_serialno", "bind_serialno");

                maps.Add("fuid", "uid");
                maps.Add("findex", "index");
                maps.Add("fbdindex", "bdindex");
                maps.Add("fmemo", "memo");
                maps.Add("fcre_id", "cre_id");
                maps.Add("ftelephone", "telephone");
                maps.Add("fmobilephone", "mobilephone");

                maps.Add("fi_character4", "fi_character4");
                maps.Add("funchain_time_local", "unchain_time_local");
                maps.Add("fmodify_time", "modify_time");
                maps.Add("fbind_time_bank", "bind_time_bank");
                maps.Add("fbind_time_local", "bind_time_local");

                APIUtil.Print4DataTable(infos.Tables[0], null, maps);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetBankCardBindList").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetBankCardBindList").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        [WebMethod]
        public void GetBankCardBindDetail()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "uid", "index", "bdindex", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                String fuid = paramsHt["uid"].ToString();
                String findex = paramsHt["index"].ToString();
                String fbdindex = paramsHt["bdindex"].ToString();

                //查询银行绑定详情
                var infos = new CFT.CSOMS.BLL.BankCardBindModule.
                    BankCardBindService().GetBankCardBindDetail(fuid, findex, fbdindex);

                if (infos == null || infos.Tables.Count == 0 || infos.Tables[0].Rows.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                APIUtil.Print4DataTable(infos.Tables[0], null, null);

            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("UnbindBankCardBind").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("UnbindBankCardBind").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        [WebMethod]
        public void GetBankCardBindRelationList()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "bind_state", "offset", "limit", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                String bank_type = paramsHt.ContainsKey("bank_type") ? paramsHt["bank_type"].ToString() : "";
                String bank_id = paramsHt.ContainsKey("bank_id") ? paramsHt["bank_id"].ToString() : "";
                String cre_type = paramsHt.ContainsKey("cre_type") ? paramsHt["cre_type"].ToString() : "";
                String cre_id = paramsHt.ContainsKey("cre_id") ? paramsHt["cre_id"].ToString() : "";
                String protocol_no = paramsHt.ContainsKey("protocol_no") ? paramsHt["protocol_no"].ToString() : "";
                String phoneno = paramsHt.ContainsKey("phoneno") ? paramsHt["phoneno"].ToString() : "";

                int bind_state = APIUtil.StringToInt(paramsHt["bind_state"].ToString());
                int limStart = APIUtil.StringToInt(paramsHt["offset"].ToString());
                int limCount = APIUtil.StringToInt(paramsHt["limit"].ToString());

                //查询银行绑定详情
                var infos = new CFT.CSOMS.BLL.BankCardBindModule.
                    BankCardBindService().GetBankCardBindRelationList(bank_type, bank_id, cre_type, cre_id, protocol_no, phoneno, bind_state, limStart, limCount);

                if (infos == null || infos.Tables.Count == 0 || infos.Tables[0].Rows.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                APIUtil.Print4DataTable(infos.Tables[0], null, null);

            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetBankCardBindRelationList").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetBankCardBindRelationList").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        #endregion

        #region 微信帐号

        /// <summary>
        /// 微信帐号转换
        /// </summary>
        [WebMethod]
        public void GetUINFromWechat()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();

                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "query_type", "query_string", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                var infos = CFT.CSOMS.BLL.CFTAccountModule.AccountService.GetQQID(paramsHt["query_type"].ToString(), paramsHt["query_string"].ToString());

                //多条记录使用此方式
                Record record = new Record();
                record.RetValue = infos;
                List<Record> list = new List<Record>();
                list.Add(record);

                APIUtil.Print<Record>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetUINFromWechat").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetUINFromWechat").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 通过微信号获取微信支付财付通帐号
        /// </summary>
        [WebMethod]
        public void GetUINFromWeChatName()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();

                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "wechatName", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                string wechatName = paramsHt.ContainsKey("wechatName") ? paramsHt["wechatName"].ToString() : "";

                var infos = CFT.CSOMS.COMMLIB.WeChatHelper.GetUINFromWeChatName(wechatName);

                Record record = new Record();
                record.RetValue = infos;
                List<Record> list = new List<Record>();
                list.Add(record);

                APIUtil.Print<Record>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetUINFromWeChatName").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetUINFromWeChatName").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 获取微信支付账号的账号信息
        /// </summary>
        [WebMethod]
        public void GetUserAccountFromWechat()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "qqid", "type", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string qqid = paramsHt.ContainsKey("qqid") ? paramsHt["qqid"].ToString() : "";
                //"WeChatId"-微信帐号,"WeChatQQ"-微信绑定QQ,"WeChatMobile"-微信绑定手机,"WeChatEmail"-微信绑定邮箱,"WeChatUid"-微信内部ID,"WeChatCft"-微信财付通帐号，"WeChatAA"-微信AA收款账户
                string type = paramsHt.ContainsKey("type") ? paramsHt["type"].ToString() : "";

                var infos = new CFT.CSOMS.BLL.CFTAccountModule.AccountService().GetUserAccountFromWechat(qqid, type);
                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<BaseInfoC.PersonalInfo> list = APIUtil.ConvertTo<BaseInfoC.PersonalInfo>(infos.Tables[0]);
                APIUtil.Print<BaseInfoC.PersonalInfo>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetUserAccountFromWechat").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetUserAccountFromWechat").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 查询微信账户CKV
        /// </summary>
        [WebMethod]
        public void GetWechatCKVInfo()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();

                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "uid", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string uid = paramsHt["uid"].ToString();

                var infos = new CFT.CSOMS.BLL.WechatPay.AuthenService().WXOperateCKVCGI(1, uid);

                if (infos == null)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }

                RecordNew record = new RecordNew();
                record.RetValue = MoneyTransfer.FenToYuan(infos["balance"].ToString());     //微信账号余额CKV
                record.RetMemo = MoneyTransfer.FenToYuan(infos["con"].ToString());          //冻结金额CKV
                List<RecordNew> list = new List<RecordNew>();
                list.Add(record);
                APIUtil.Print<RecordNew>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetWechatCKVInfo").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetWechatCKVInfo").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 同步CKV
        /// </summary>
        [WebMethod]
        public void SyncWechatCKV()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();

                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "uid", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string uid = paramsHt["uid"].ToString();

                var infos = new CFT.CSOMS.BLL.WechatPay.AuthenService().WXOperateCKVCGI(2, uid);
                bool result = false;

                Record record = new Record();
                List<Record> list = new List<Record>();
                if (infos == null)
                {
                    result = false;
                    record.RetValue = result.ToString().ToLower();
                }
                else
                {
                    result = infos["RESULT"] == "0" ? true : false;
                    record.RetValue = result.ToString().ToLower();
                }
                list.Add(record);
                APIUtil.Print<Record>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("SyncWechatCKV").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("SyncWechatCKV").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }


        /// <summary>
        /// 获取微信红包-收到的红包
        /// </summary>
        [WebMethod]
        public void GetWechatReceiveRedPacket()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();

                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "wechatName", "begin_time", "end_time", "offset", "limit", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                string wechatName = paramsHt.ContainsKey("wechatName") ? paramsHt["wechatName"].ToString() : "";
                string HBUin = WeChatHelper.GetHBUINFromWeChatName(wechatName);
                wechatName = HBUin.Replace("@hb.tenpay.com", "");
                DateTime begin_time = paramsHt.ContainsKey("begin_time") ? APIUtil.StrToDate(paramsHt["begin_time"].ToString()) : DateTime.Now.AddDays(-1);
                DateTime end_time = paramsHt.ContainsKey("end_time") ? APIUtil.StrToDate(paramsHt["end_time"].ToString()) : DateTime.Now.AddDays(1);
                int offset = paramsHt.ContainsKey("offset") ? APIUtil.StringToInt(paramsHt["offset"].ToString()) : 0;
                int limit = paramsHt.ContainsKey("limit") ? APIUtil.StringToInt(paramsHt["limit"].ToString()) : 10;

                if (offset < 0)
                    offset = 0;
                if (limit < 0 || limit > 50)
                    limit = 50;

                var infos = new CFT.CSOMS.BLL.WechatPay.WechatPayService().QueryUserReceiveList(wechatName, begin_time, end_time, offset, limit);

                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<WechatApi.WechatReceviceHB> list = APIUtil.ConvertTo<WechatApi.WechatReceviceHB>(infos.Tables[0]);
                APIUtil.Print<WechatApi.WechatReceviceHB>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetWechatReceiveRedPacket").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetWechatReceiveRedPacket").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 获取微信红包-发出的红包
        /// </summary>
        [WebMethod]
        public void GetWechatSendRedPacket()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();

                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "wechatName", "begin_time", "end_time", "offset", "limit", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                string wechatName = paramsHt.ContainsKey("wechatName") ? paramsHt["wechatName"].ToString() : "";
                string HBUin = WeChatHelper.GetHBUINFromWeChatName(wechatName);
                wechatName = HBUin.Replace("@hb.tenpay.com", "");
                DateTime begin_time = paramsHt.ContainsKey("begin_time") ? APIUtil.StrToDate(paramsHt["begin_time"].ToString()) : DateTime.Now.AddDays(-1);
                DateTime end_time = paramsHt.ContainsKey("end_time") ? APIUtil.StrToDate(paramsHt["end_time"].ToString()) : DateTime.Now.AddDays(1);
                int offset = paramsHt.ContainsKey("offset") ? APIUtil.StringToInt(paramsHt["offset"].ToString()) : 0;
                int limit = paramsHt.ContainsKey("limit") ? APIUtil.StringToInt(paramsHt["limit"].ToString()) : 10;

                if (offset < 0)
                    offset = 0;
                if (limit < 0 || limit > 50)
                    limit = 50;

                var infos = new CFT.CSOMS.BLL.WechatPay.WechatPayService().QueryUserSendList(wechatName, begin_time, end_time, offset, limit);

                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<WechatApi.WechatSendHB> list = APIUtil.ConvertTo<WechatApi.WechatSendHB>(infos.Tables[0]);
                APIUtil.Print<WechatApi.WechatSendHB>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetWechatSendRedPacket").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetWechatSendRedPacket").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 微信红包-红包详情
        /// </summary>
        [WebMethod]
        public void GetWechatRedPacketDetail()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();

                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "listid", "token");    //listid必填，查询收到的红包详情时还需要加入send_listid
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string listid = paramsHt["listid"].ToString();
                string send_listid = paramsHt.ContainsKey("send_listid") ? paramsHt["send_listid"].ToString() : null;

                DataSet ds = null;
                if (send_listid == null)
                {
                    ds = new CFT.CSOMS.BLL.WechatPay.WechatPayService().QueryDetail(listid, 0);     //发送的红包
                }
                else
                {
                    ds = new CFT.CSOMS.BLL.WechatPay.WechatPayService().QueryReceiveHBInfoById(listid, send_listid, 1);     //收到的红包
                }

                if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var detail_dt = ds.Tables[0];
                    detail_dt.Columns.Add("Amount_text", typeof(string));
                    detail_dt.Columns.Add("ReceiveOpenid_text", typeof(string));
                    detail_dt.Columns.Add("SendOpenid_text", typeof(string));

                    Func<string, string> GetWxNoByOpenId = openId =>
                    {
                        try
                        {
                            var accid = WeChatHelper.GetAcctIdFromOpenId(openId);
                            return accid + "@wx.tenpay.com";
                        }
                        catch (Exception ex)
                        {
                            return ex.Message + "  [OpenId=" + openId + "]";
                        }
                    };

                    var sendOpenid = detail_dt.Rows[0]["SendOpenid"].ToString();
                    string send_openid_tmp = GetWxNoByOpenId(sendOpenid);   // string.Format("{0}@wx.tenpay.com", WeChatHelper.GetAcctIdFromOpenId(sendOpenid));

                    foreach (DataRow item in detail_dt.Rows)
                    {
                        item["Amount_text"] = MoneyTransfer.FenToYuan(item["Amount"].ToString());
                        item["ReceiveOpenid_text"] = GetWxNoByOpenId(item["ReceiveOpenid"].ToString());
                        item["SendOpenid_text"] = send_openid_tmp;
                    };
                }

                List<WechatApi.WechatHBDetail> list = APIUtil.ConvertTo<WechatApi.WechatHBDetail>(ds.Tables[0]);
                APIUtil.Print<WechatApi.WechatHBDetail>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetWechatRedPacketDetail").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetWechatRedPacketDetail").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }


        #endregion

        #region 手Q支付

        /// <summary>
        ///手Q还款列表 
        /// </summary>
        [WebMethod]
        public void RefundHandQQuery()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "ID", "type", "begin_time", "end_time", "offset", "limit", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string ID = paramsHt.ContainsKey("ID") ? paramsHt["ID"].ToString() : "";
                int type = paramsHt.ContainsKey("type") ? APIUtil.StringToInt(paramsHt["type"].ToString()) : 1;
                DateTime begin_time = paramsHt.ContainsKey("begin_time") ? APIUtil.StrToDate(paramsHt["begin_time"].ToString()) : DateTime.Now;
                DateTime end_time = paramsHt.ContainsKey("end_time") ? APIUtil.StrToDate(paramsHt["end_time"].ToString()) : DateTime.Now;
                int offset = paramsHt.ContainsKey("offset") ? APIUtil.StringToInt(paramsHt["offset"].ToString()) : 0;
                int limit = paramsHt.ContainsKey("limit") ? APIUtil.StringToInt(paramsHt["limit"].ToString()) : 10;

                if (offset < 0)
                {
                    offset = 0;
                }
                if (limit < 0 || limit > 50)
                {
                    limit = 50;
                }
                string start_time = begin_time.ToString("yyyy-MM-dd 00:00:00");
                string end_time_str = end_time.ToString("yyyy-MM-dd 23:59:59");

                var infos = new CFT.CSOMS.BLL.HandQModule.HandQService().RefundHandQQuery(ID, type.ToString(), start_time, end_time_str, offset, limit);

                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<Payment.RefundHandQList> list = APIUtil.ConvertTo<Payment.RefundHandQList>(infos.Tables[0]);
                APIUtil.Print<Payment.RefundHandQList>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("RefundHandQQuery").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("RefundHandQQuery").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        /// <summary>
        /// 手Q还款详情
        /// </summary>
        [WebMethod]
        public void RefundHandQDetail()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "ID", "type", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string ID = paramsHt.ContainsKey("ID") ? paramsHt["ID"].ToString() : "";
                int type = paramsHt.ContainsKey("type") ? APIUtil.StringToInt(paramsHt["type"].ToString()) : 3;

                var infos = new CFT.CSOMS.BLL.HandQModule.HandQService().RefundHandQDetailQuery(ID, type.ToString());

                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<Payment.RefundHandQDetail> list = APIUtil.ConvertTo<Payment.RefundHandQDetail>(infos.Tables[0]);
                APIUtil.Print<Payment.RefundHandQDetail>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("RefundHandQDetail").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("RefundHandQDetail").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        [WebMethod]
        public void GetSendRedPacketInfo()
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

                string begin_time_str = begin_time.ToString("yyyy-MM-dd");
                string end_time_str = end_time.AddDays(1).ToString("yyyy-MM-dd");
                if (offset < 0)
                {
                    offset = 0;
                }
                if (limit < 0 || limit > 50)
                {
                    limit = 50;
                }

                string strOutMsg = "发红包信息反馈";
                var infos = new CFT.CSOMS.BLL.HandQModule.HandQService().SendRedPacketInfo(uin, "", begin_time_str, end_time_str, offset, limit, out strOutMsg);
                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<Payment.SendRedPacketInfo> list = APIUtil.ConvertTo<Payment.SendRedPacketInfo>(infos.Tables[0]);
                APIUtil.Print<Payment.SendRedPacketInfo>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetSendRedPacketInfo").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetSendRedPacketInfo").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        [WebMethod]
        public void GetRecvRedPacketInfo()
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

                string begin_time_str = begin_time.ToString("yyyy-MM-dd");
                string end_time_str = end_time.AddDays(1).ToString("yyyy-MM-dd");
                if (offset < 0)
                {
                    offset = 0;
                }
                if (limit < 0 || limit > 50)
                {
                    limit = 50;
                }

                string strOutMsg = "收红包信息反馈";
                var infos = new CFT.CSOMS.BLL.HandQModule.HandQService().RecvRedPacketInfo(uin, "", begin_time_str, end_time_str, offset, limit, out strOutMsg);
                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<Payment.RecvRedPacketInfo> list = APIUtil.ConvertTo<Payment.RecvRedPacketInfo>(infos.Tables[0]);
                APIUtil.Print<Payment.RecvRedPacketInfo>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetRecvRedPacketInfo").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetRecvRedPacketInfo").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        [WebMethod]
        public void GetRedPacketDetail()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "listid", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string listid = paramsHt.ContainsKey("listid") ? paramsHt["listid"].ToString() : "";
                int type = paramsHt.ContainsKey("type") ? APIUtil.StringToInt(paramsHt["type"].ToString()) : 1;

                string strOutMsg = "红包详情";
                var infos = new CFT.CSOMS.BLL.HandQModule.HandQService().RequestHandQDetail(listid, 1, 0, 1, out strOutMsg);
                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<Payment.RedPacketDetail> list = APIUtil.ConvertTo<Payment.RedPacketDetail>(infos.Tables[0]);
                APIUtil.Print<Payment.RedPacketDetail>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetRedPacketDetail").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetRedPacketDetail").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        #endregion

        #region 姓名生僻字

        [WebMethod]
        public void RareNameQuery()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "card_no", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string card_no = paramsHt.ContainsKey("card_no") ? paramsHt["card_no"].ToString() : "";

                var infos = new CFT.CSOMS.BLL.RareName.RareNameService().RareNameQuery(card_no);
                if (infos == null || infos.Tables.Count <= 0 || infos.Tables[0].Rows.Count <= 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                List<Payment.RareNameList> list = APIUtil.ConvertTo<Payment.RareNameList>(infos.Tables[0]);
                APIUtil.Print<Payment.RareNameList>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("RareNameQuery").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("RareNameQuery").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        [WebMethod]
        public void OperateRareName()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "op_type", "card_no", "account_name", "user_type", "modify_type", "card_state", "modify_username", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string op_type = paramsHt.ContainsKey("op_type") ? paramsHt["op_type"].ToString() : "";
                string card_no = paramsHt.ContainsKey("card_no") ? paramsHt["card_no"].ToString() : "";
                string account_name = paramsHt.ContainsKey("account_name") ? paramsHt["account_name"].ToString() : "";
                string user_type = paramsHt.ContainsKey("user_type") ? paramsHt["user_type"].ToString() : "";
                string modify_type = paramsHt.ContainsKey("modify_type") ? paramsHt["modify_type"].ToString() : "";
                string card_state = paramsHt.ContainsKey("card_state") ? paramsHt["card_state"].ToString() : "";
                string modify_username = paramsHt.ContainsKey("modify_username") ? paramsHt["modify_username"].ToString() : "";

                new CFT.CSOMS.BLL.RareName.RareNameService().OperateRareName(op_type, card_no, user_type, account_name, card_state, modify_username, modify_type);

                Record record = new Record();
                record.RetValue = "true";
                List<Record> list = new List<Record>();
                list.Add(record);
                APIUtil.Print<Record>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("OperateRareName").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("OperateRareName").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        #endregion

        #region 快捷支付(一点通业务新)
        [WebMethod]
        public void GetBankCardBindListFinalNew()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "query_type", "bind_state", "operator", "offset", "limit", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                String fuin = paramsHt.ContainsKey("uin") ? paramsHt["uin"].ToString() : "";
                String Operator = paramsHt["operator"].ToString();
                String bankID = paramsHt.ContainsKey("bank_id") ? paramsHt["bank_id"].ToString() : "";
                String fuid = paramsHt.ContainsKey("uid") ? paramsHt["uid"].ToString() : "";
                String creType = paramsHt.ContainsKey("cre_type") ? paramsHt["cre_type"].ToString() : "";
                String creID = paramsHt.ContainsKey("cre_id") ? paramsHt["cre_id"].ToString() : "";
                String protocolno = paramsHt.ContainsKey("protocol_no") ? paramsHt["protocol_no"].ToString() : "";
                String phoneno = paramsHt.ContainsKey("phoneno") ? paramsHt["phoneno"].ToString() : "";
                String strBeginDate = paramsHt.ContainsKey("begin_date") ? paramsHt["begin_date"].ToString() : "";
                String strEndDate = paramsHt.ContainsKey("end_date") ? paramsHt["end_date"].ToString() : "";
                String Fbank_type = paramsHt.ContainsKey("bank_type") ? paramsHt["bank_type"].ToString() : "";
                String bind_serialno = paramsHt.ContainsKey("bind_serialno") ? paramsHt["bind_serialno"].ToString() : "";
                int queryType = paramsHt.ContainsKey("query_type") ? APIUtil.StringToInt(paramsHt["query_type"]) : 0;
                int bindState = paramsHt.ContainsKey("bind_state") ? APIUtil.StringToInt(paramsHt["bind_state"]) : 99;

                APIUtil.ValidateDate(strBeginDate, "yyyy-MM-dd HH:mm:ss", true);
                APIUtil.ValidateDate(strBeginDate, "yyyy-MM-dd HH:mm:ss", true);

                int limStart = APIUtil.StringToInt(paramsHt["offset"].ToString());
                int limCount = APIUtil.StringToInt(paramsHt["limit"].ToString());
                if (limStart < 0)
                    limStart = 0;
                if (limCount < 0 || limCount > 50)
                    limCount = 50;
                int total_num = 0;
                var infos = new CFT.CSOMS.BLL.BankCardBindModule.BankCardBindService().GetBankCardBindList_FinalNew(
                    fuin, Fbank_type, bankID, fuid, creID, protocolno, phoneno, strBeginDate, strEndDate, bindState, bind_serialno, Operator, queryType, creType, limStart, limCount, out total_num);

                if (infos == null || infos.Tables.Count == 0 || infos.Tables[0].Rows.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }

                Dictionary<string, string> maps = new Dictionary<string, string>();

                maps.Add("Fbank_id", "bank_id");
                maps.Add("Fbank_status", "bank_status");
                maps.Add("Fbank_type", "bank_type");
                maps.Add("Fbank_type_txt", "bank_type_txt");
                maps.Add("Fbind_flag", "bind_flag");
                maps.Add("Fbind_serialno", "bind_serialno");
                maps.Add("Fbind_status", "bind_status");
                maps.Add("Fbind_time_bank", "bind_time_bank");
                maps.Add("Fbind_time_local", "bind_time_local");
                maps.Add("Fbind_type", "bind_type");
                maps.Add("Fcard_tail", "card_tail");
                maps.Add("Fcre_id", "cre_id");
                maps.Add("Fcre_type", "cre_type");
                maps.Add("Fcreate_time", "create_time");
                maps.Add("Fday_quota", "day_quota");//单次支付限额(元)
                maps.Add("Findex", "index");
                maps.Add("Fmemo", "memo");
                maps.Add("Fmobilephone", "mobilephone");
                maps.Add("Fmodify_time", "modify_time");
                maps.Add("Fonce_quota", "once_quota");//单天支付限额（元）
                maps.Add("Fprotocol_no", "protocol_no");
                maps.Add("Ftelephone", "telephone");
                maps.Add("Ftruename", "truename");
                maps.Add("Fuid", "uid");
                maps.Add("Fuin", "uin");
                maps.Add("Funchain_time_bank", "unchain_time_bank");//解绑时间（银行）
                maps.Add("Funchain_time_local", "unchain_time_local");//解绑时间（本地）
                maps.Add("Fxyzf_typeStr", "xyzf_type_str");//是否为微信信用卡
                maps.Add("bank_name", "bank_name");
                maps.Add("sms_flag", "sms_flag");//小额名短信通知,0已关闭，1已开启

                APIUtil.Print4DataTable(infos.Tables[0], null, maps, total_num);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetBankCardBindListFinalNew").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetBankCardBindListFinalNew").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }
        #endregion

        #region 增加测试代码
        [WebMethod]
        public void GetFinanceOdTcBankRollDay()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "auid", "query_day","sign","token");
                //token验证
                APIUtil.ValidateToken(paramsHt);
                string errMsg = string.Empty;
                string auid = paramsHt.ContainsKey("auid") ? paramsHt["auid"].ToString() : "";
                string sign = paramsHt["sign"].ToString();
                string query_day= paramsHt["query_day"].ToString();
                var infos = new CFT.CSOMS.BLL.HandQModule.HandQService().GetFinanceOdTcBankRollDay(auid, sign, query_day, out errMsg);
                if (infos == null || infos.Tables.Count == 0 || infos.Tables[0].Rows.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }

                Dictionary<string, string> maps = new Dictionary<string, string>();

                maps.Add("Flist_id", "list_id");
                maps.Add("Fnum", "num");
                maps.Add("Fstate", "state");
                maps.Add("Fbank_list", "bank_list");
                maps.Add("Fbank_acc", "bank_acc");
                maps.Add("Fbank_type", "bank_type");
                maps.Add("Faname", "aname");
                maps.Add("Fpay_front_time", "pay_front_time");
                maps.Add("Fbank_time", "bank_time");
                maps.Add("Fmodify_time", "modify_time");           

                APIUtil.Print4DataTable(infos.Tables[0], null, maps);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetFinanceOdTcBankRollDay").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetFinanceOdTcBankRollDay").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }
        #endregion
    }
}
