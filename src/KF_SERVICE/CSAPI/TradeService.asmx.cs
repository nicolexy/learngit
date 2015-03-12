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
        [WebMethod(Description="交易记录查询")]
        public void GetTradeList()
        {
            try
            {
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //验证必填参数
                APIUtil.ValidateParamsNew(paramsHt, "appid", "tradeid", "type", "time", "token");
                //验证token
                APIUtil.ValidateToken(paramsHt);

                string tradeid = paramsHt.ContainsKey("tradeid") ? paramsHt["tradeid"].ToString() : "";
                int type = APIUtil.StringToInt(paramsHt["type"].ToString());
                DateTime time = APIUtil.StrToDate(paramsHt["time"].ToString());

                var infos = new CFT.CSOMS.BLL.TradeModule.TradeService().GetTradeList(tradeid, type, time);
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
    }
}
