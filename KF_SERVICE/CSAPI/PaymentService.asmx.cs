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
        /// <param name="appid"></param>
        /// <param name="trade_id"></param>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="currency_type"></param>
        /// <param name="spid"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [WebMethod]
        public void GetUserProfitList() //string appid, string trade_id, string start_date, string end_date, int currency_type, string spid, int offset, int limit, string token
        {
            try
            {
                //SunLibrary.LoggerFactory.Get("GetUserProfitList").InfoFormat("appid:{0},trade_id:{1},start_date:{2},end_date:{3},currency_type:{4},spid:{5},offset:{6},limit:{7},token:{8}", appid, trade_id, start_date, end_date, currency_type, spid, offset, limit, token);
                //APIUtil.ValidateParams("appid|" + appid, "trade_id|" + trade_id, "currency_type|" + currency_type, "offset|" + offset, "limit|" + limit, "token|" + token);

                //APIUtil.ValidateToken(token, appid, trade_id, start_date, end_date, currency_type.ToString(), spid, offset.ToString(), limit.ToString());


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

                string start_date = "", end_date = "", spid="";

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
                var infos = new CFT.CSOMS.BLL.FundModule.FundService().BindProfitList(paramsHt["trade_id"].ToString(), start_date,end_date, currency_type, spid, offset, limit);
                
                if (infos == null || infos.Rows.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }

                List<Payment.UserProfit> list = APIUtil.ConvertTo<Payment.UserProfit>(infos);
                
                //var ret = new ResultParse<Payment.UserProfit>().ReturnToObject(list);
                
                //return APIUtil.ConverToXml<RetObject<Payment.UserProfit>>(ret);

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
        /// 通过账号查找交易ID
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="uin"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [WebMethod]
        public void GetTradeIdByUIN()//string appid, string uin, string token
        {
            try
            {
                //SunLibrary.LoggerFactory.Get("GetTradeIdByUIN").InfoFormat("appid:{0},uin:{1},token:{2}", appid , uin, token);

                ////必填参数验证格式: 参数名|参数值
                //APIUtil.ValidateParams("appid|"+appid,"uin|" + uin,"token|"+token);

                ////token验证,参数需按照加密顺序传递
                //APIUtil.ValidateToken(token, appid, uin);

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
                //var ret = new ResultParse<Record>().ReturnToObject(list);

                //return APIUtil.ConverToXml<RetObject<Record>>(ret);
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
        /// <param name="appid"></param>
        /// <param name="uin"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [WebMethod]
        public void GetPayCardInfo() //string appid, string uin, string token
        {
            try
            {
                //SunLibrary.LoggerFactory.Get("GetPayCardInfo").InfoFormat("appid:{0},uin:{1},token:{2}" , appid , uin , token);
                //APIUtil.ValidateParams("appid|" + appid, "uin|" + uin, "token|" + token);

                ////token验证,参数需按照加密顺序传递
                //APIUtil.ValidateToken(token, appid, uin);

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

                //var ret = new ResultParse<Payment.PayCardInfo>().ReturnToObject(list);

                //return APIUtil.ConverToXml<RetObject<Payment.PayCardInfo>>(ret);
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
        /// <param name="appid"></param>
        /// <param name="uin"></param>
        /// <param name="cur_type"></param>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="redirection_type"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [WebMethod]
        public void GetBankRollListNotChildren()// string appid, string uin, string cur_type, string start_date, string end_date, int redirection_type, int offset, int limit, string token
        {
            try
            {
                //SunLibrary.LoggerFactory.Get("GetBankRollListNotChildren").InfoFormat("appid:{0},uin:{1},cur_type:{2},start_date:{3},end_date:{4},redirection_type:{5},offset:{6},limit:{7},token:{8}" ,appid ,uin, cur_type, start_date, end_date, redirection_type, offset, limit, token);
                //APIUtil.ValidateParams("appid|" + appid, "uin|" + uin, "cur_type|" + cur_type, "start_date|" + start_date, "end_date|" + end_date, "redirection_type|" + redirection_type, "offset|" + offset, "limit|" + limit, "token|" + token);

                //APIUtil.ValidateToken(token, appid, uin,cur_type,start_date,end_date,redirection_type.ToString(),offset.ToString(),limit.ToString());

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

               // var infos = new CFT.CSOMS.BLL.FundModule.FundService().BindBankRollListNotChildren(uin, cur_type, sDate, eDate, offset, limit, redirection_type);
                var infos = new CFT.CSOMS.BLL.FundModule.FundService().BindBankRollListNotChildren(paramsHt["uin"].ToString(), paramsHt["cur_type"].ToString(), sDate, eDate, offset, limit, redirection_type);
                if (infos == null || infos.Rows.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }

                List<Payment.UserTradePosInfo> list = APIUtil.ConvertTo<Payment.UserTradePosInfo>(infos);

                //var ret = new ResultParse<Payment.UserTradePosInfo>().ReturnToObject(list);

                //return APIUtil.ConverToXml<RetObject<Payment.UserTradePosInfo>>(ret);
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
        /// <param name="appid"></param>
        /// <param name="uin"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [WebMethod]
        public void GetUserFundSummary()//string appid, string uin, string token
        {
            try
            {
                //SunLibrary.LoggerFactory.Get("GetUserFundSummary").InfoFormat("appid:{0},uin:{1},token:{2}" ,appid , uin , token);
                //APIUtil.ValidateParams("appid|" + appid, "uin|" + uin, "token|" + token);

                ////token验证,参数需按照加密顺序传递
                //APIUtil.ValidateToken(token, appid, uin);

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

                //var ret = new ResultParse<Payment.UserFundSummary>().ReturnToObject(list);

                //return APIUtil.ConverToXml<RetObject<Payment.UserFundSummary>>(ret);
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
        /// <param name="appid"></param>
        /// <param name="uin"></param>
        /// <param name="cur_type"></param>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="redirection_type"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [WebMethod]
        public void QueryCloseFundRollList()//string appid, string trade_id, string fund_code, string start_date, string end_date, int offset, int limit, string token
        {
            try
            {
                //SunLibrary.LoggerFactory.Get("QueryCloseFundRollList").InfoFormat("appid:{0},trade_id:{1},fund_code:{2},start_date:{3},end_date:{4},offset:{5},limit:{6},token:{7}",appid ,trade_id,fund_code ,start_date , end_date ,offset ,limit,token);
                //APIUtil.ValidateParams("appid|" + appid, "trade_id|" + trade_id, "fund_code|" + fund_code, "start_date|" + start_date, "end_date|" + end_date, "offset|" + offset, "limit|" + limit, "token|" + token);

                //APIUtil.ValidateToken(token, appid, trade_id, fund_code, start_date, end_date, offset.ToString(), limit.ToString());


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

                //var ret = new ResultParse<Payment.CloseFundRoll>().ReturnToObject(list);

                //return APIUtil.ConverToXml<RetObject<Payment.CloseFundRoll>>(ret);
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
        public void GetUserFundAccountInfo()//string appid, string uin, string token
        {
            try
            {
                //SunLibrary.LoggerFactory.Get("GetUserFundAccountInfo").InfoFormat("appid:{0},uin:{1},token:{2}" ,appid ,uin, token);
                //APIUtil.ValidateParams("appid|" + appid, "uin|" + uin, "token|" + token);

                ////token验证,参数需按照加密顺序传递
                //APIUtil.ValidateToken(token, appid, uin);

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

                //var ret = new ResultParse<Payment.UserFundAccountInfo>().ReturnToObject(list);

                //return APIUtil.ConverToXml<RetObject<Payment.UserFundAccountInfo>>(ret);
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
        public void GetChildrenBankRollListEx()//string appid, string uin, string spid, string curtype, string start_date, string end_date, int offset, int limit, int ftype, string token
        {
            try
            {
                //SunLibrary.LoggerFactory.Get("GetChildrenBankRollListEx").InfoFormat("appid:{0},uin:{1},spid:{2},curtype:{3},start_date:{4},end_date:{5},offset:{6},limit:{7},ftype:{8},token:{9}",appid ,uin
                //    , spid ,curtype ,start_date , end_date ,offset , limit ,ftype ,token);
                //APIUtil.ValidateParams("appid|" + appid, "uin|" + uin, "spid|" + spid, "curtype|" + curtype, "start_date|" + start_date, "end_date|" + end_date, "offset|" + offset, "limit|" + limit, "ftype|" + ftype, "token|" + token);

                //APIUtil.ValidateToken(token, appid, uin, spid, curtype, start_date, end_date, offset.ToString(), limit.ToString(), ftype.ToString());


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

                //var ret = new ResultParse<Payment.ChildrenBankRoll>().ReturnToObject(list);

                //return APIUtil.ConverToXml<RetObject<Payment.ChildrenBankRoll>>(ret);
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
         #endregion

        #region 快捷支付

        [WebMethod]
        public void GetBankDic()//string appid,string token
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
        public void SyncBankCardBind()//string bank_type, string bank_id, string card_tail
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
        public void UnbindBankCardBind()//String bankType, String qqid, String protocolNo, String userIP
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
        public void UnbingBankCardBindSpecial()//string bankType, string qqid, string card_tail, string bindSerialno, string protocol_no
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
        public void GetBankCardBindList()//string fuin, string Fbank_type, string bankID, string fuid, string creType, string creID, string protocolno, string phoneno, string strBeginDate, string strEndDate,int queryType, int bindStatue
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
        public void GetBankCardBindDetail()//string fuid, string findex, string fBDIndex
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
        public void GetBankCardBindRelationList()//string bank_type, string bank_id, string cre_type, string cre_id, string protocol_no, string phoneno, int bind_state, int limStart, int limCount
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();

                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "bind_state", "offset", "limit", "token");
                //token验证
                APIUtil.ValidateToken(paramsHt);

                String bank_type    =  paramsHt.ContainsKey("bank_type") ?  paramsHt["bank_type"].ToString() : "";
                String bank_id      =  paramsHt.ContainsKey("bank_id") ? paramsHt["bank_id"].ToString() : "";
                String cre_type     =  paramsHt.ContainsKey("cre_type") ? paramsHt["cre_type"].ToString() : "";
                String cre_id       =  paramsHt.ContainsKey("cre_id") ? paramsHt["cre_id"].ToString() : "";
                String protocol_no  =  paramsHt.ContainsKey("protocol_no") ? paramsHt["protocol_no"].ToString() : "";
                String phoneno      =  paramsHt.ContainsKey("phoneno") ? paramsHt["phoneno"].ToString() : "";

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

        [WebMethod]
        public void QueryBankCardList()//string appid, string bank_card, string date, int offset, int limit, string token
        {
            try
            {
                //SunLibrary.LoggerFactory.Get("QueryBankCardList").InfoFormat("appid:{0},bank_card:{1},date:{2},offset:{6},limit:{7},token:{8}", appid, bank_card, date, offset, limit, token);
                //APIUtil.ValidateParams("appid|" + appid, "bank_card|" + bank_card, "date|" + date, "offset|" + offset, "limit|" + limit, "token|" + token);

                //APIUtil.ValidateToken(token, appid, bank_card, date, offset.ToString(), limit.ToString());

                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();

                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "bank_card", "date",  "offset", "limit", "token");
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
                var infos = new CFT.CSOMS.BLL.WechatPay.FastPayService().QueryBankCardList(paramsHt["bank_card"].ToString(), paramsHt["date"].ToString(), 10100,offset, limit);
                if (infos == null || infos.Tables.Count==0|| infos.Tables[0].Rows.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }

                List<Payment.BankCard> list = APIUtil.ConvertTo<Payment.BankCard>(infos.Tables[0]);

                //var ret = new ResultParse<Payment.BankCard>().ReturnToObject(list);

                //return APIUtil.ConverToXml<RetObject<Payment.BankCard>>(ret);
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

        #region 微信帐号
        /// <summary>
        /// 微信帐号转换
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="query_type"></param>
        /// <param name="query_string"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [WebMethod]
        public void GetUINFromWechat()//string appid, string query_type, string query_string, string token
        {
            try
            {
                //SunLibrary.LoggerFactory.Get("GetUINFromWechat").InfoFormat("appid:{0},query_type:{1},query_string:{2},token:{3}", appid, query_type, query_string, token);

                ////必填参数验证格式: 参数名|参数值
                //APIUtil.ValidateParams("appid|" + appid, "query_type|" + query_type, "query_string|" + query_string, "token|" + token);

                ////token验证,参数需按照加密顺序传递
                //APIUtil.ValidateToken(token, appid, query_type, query_string);

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
                //var ret = new ResultParse<Record>().ReturnToObject(list);

                //return APIUtil.ConverToXml<RetObject<Record>>(ret);

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
    }
    
}
