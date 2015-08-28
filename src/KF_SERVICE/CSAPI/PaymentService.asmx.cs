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

                if (limit > 20 || limit < 0)
                {
                    limit = 20;
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

                if (limit > 20 || limit < 0)
                {
                    limit = 20;
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

                if (limit > 20 || limit == 0)
                {
                    limit = 20;
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

                if (limit > 20 || limit == 0)
                {
                    limit = 20;
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

                if (limit > 20 || limit < 0)
                {
                    limit = 20;
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
                APIUtil.ValidateParamsNew(paramsHt, "appid", "uin", "query_type", "bind_state", "offset", "limit", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                String fuin = paramsHt["uin"].ToString();

                String bankID = paramsHt.ContainsKey("bank_id") ? paramsHt["bank_id"].ToString() : "";
                String fuid = paramsHt.ContainsKey("uid") ? paramsHt["uid"].ToString() : "";
                String creType = paramsHt.ContainsKey("cre_type") ? paramsHt["cre_type"].ToString() : "";
                String creID = paramsHt.ContainsKey("cre_id") ? paramsHt["cre_id"].ToString() : "";
                String protocolno = paramsHt.ContainsKey("protocol_no") ? paramsHt["protocol_no"].ToString() : "";
                String phoneno = paramsHt.ContainsKey("phoneno") ? paramsHt["phoneno"].ToString() : "";
                String strBeginDate = paramsHt.ContainsKey("begin_date") ? paramsHt["begin_date"].ToString() : "";
                String strEndDate = paramsHt.ContainsKey("end_date") ? paramsHt["end_date"].ToString() : "";
                String Fbank_type = paramsHt.ContainsKey("bank_type") ? paramsHt["bank_type"].ToString() : "";

                int queryType = APIUtil.StringToInt(paramsHt["query_type"]);
                int bindState = 99;
                if (paramsHt.ContainsKey("bind_state"))
                {
                    bindState = APIUtil.StringToInt(paramsHt["bind_state"]);
                }

                APIUtil.ValidateDate(strBeginDate, "yyyy-MM-dd HH:mm:ss", true);
                APIUtil.ValidateDate(strBeginDate, "yyyy-MM-dd HH:mm:ss", true);

                int limStart = APIUtil.StringToInt(paramsHt["offset"].ToString());
                int limCount = APIUtil.StringToInt(paramsHt["limit"].ToString());


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
                if (limit < 0 || limit > 1000)
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
                if (limit < 0 || limit > 1000)
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
                if (limit < 0 || limit > 1000)
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
    }
}
