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
        [WebMethod(Description="根据交易单号查询交易记录")]
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
                    throw new ServiceException(APIUtil.ERR_NORECORD,ErroMessage.MESSAGE_NORECORD);
                }
                List<TradeInfo.TradePayList> list = APIUtil.ConvertTo<TradeInfo.TradePayList>(infos.Tables[0]);
                APIUtil.Print<TradeInfo.TradePayList>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetTradeList").ErrorFormat("return_code:{0},msg:{1}",se.GetRetcode,se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            { 
                SunLibrary.LoggerFactory.Get("GetTradeList").ErrorFormat("return_code:{0},msg:{1}",APIUtil.ERR_SYSTEM,ex.Message);
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
                List<TradeInfo.TradePayList> list = APIUtil.ConvertTo<TradeInfo.TradePayList>(infos.Tables[0]);
                APIUtil.Print<TradeInfo.TradePayList>(list);
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
    }
}
