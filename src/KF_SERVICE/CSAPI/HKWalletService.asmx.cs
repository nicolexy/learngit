using CFT.CSOMS.BLL.ForeignCurrencyModule;
using CFT.CSOMS.Service.CSAPI.Language;
using CFT.CSOMS.Service.CSAPI.PayMent;
using CFT.CSOMS.Service.CSAPI.Utility;
using commLib.Entity.HKWallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace CSAPI
{
    /// <summary>
    /// Summary description for WechatService
    /// </summary>
    [WebService(Namespace = "")]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class HKWalletService : System.Web.Services.WebService
    {

        /// <summary>
        /// 香港钱包实名认证查询信息
        /// </summary>
        [WebMethod]
        public void GetHKRealNameInfo()
        {
            try
            {
                //http://localhost:61131/CSAPI/HKWalletService.asmx/GetHKRealNameInfo?appid=10001&offset=0&limit=10&token=00801887064ba38d713a82c90ddcf008

                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "offset", "limit", "token");
                // 非必填参数 :"uin", "type", "state", "start_time", "end_time"
                //token验证
                APIUtil.ValidateToken(paramsHt);

                int offset = APIUtil.StringToInt(paramsHt["offset"].ToString());
                int limit = APIUtil.StringToInt(paramsHt["limit"].ToString());

                if (offset < 0)
                {
                    offset = 0;
                }

                if (limit > 50 || limit < 0)
                {
                    limit = 50;
                }

                DateTime? start_time = null, end_time = null;

                if (paramsHt.Keys.Contains("start_time"))
                { //校验时间格式
                    APIUtil.ValidateDate(paramsHt["start_time"].ToString(), "yyyy-MM-dd", true);
                    start_time = Convert.ToDateTime(paramsHt["start_time"].ToString());
                }
                if (paramsHt.Keys.Contains("end_time"))
                { //校验时间格式
                    APIUtil.ValidateDate(paramsHt["end_time"].ToString(), "yyyy-MM-dd", true);
                    end_time = Convert.ToDateTime(paramsHt["end_time"].ToString());
                }

                string uin = "", type = "", state = "", client_ip = "";


                if (paramsHt.Keys.Contains("uin"))
                {
                    uin = paramsHt["uin"].ToString();
                }
                if (paramsHt.Keys.Contains("type"))
                {
                    type = paramsHt["type"].ToString();
                }
                if (paramsHt.Keys.Contains("state"))
                {
                    state = paramsHt["state"].ToString();
                }

                client_ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                List<HKWalletRealNameCheckModel> list = new FCXGWallet().New_QueryRealNameInfo2(uin, type, state, start_time, end_time, client_ip, offset, limit);

                if (list == null || list.Count == 0)
                {
                    throw new ServiceException(APIUtil.ERR_NORECORD, ErroMessage.MESSAGE_NORECORD);
                }
                APIUtil.Print<HKWalletRealNameCheckModel>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetHKRealNameInfo").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetHKRealNameInfo").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

        [WebMethod]
        public void CheckHKRealName()
        {
            try
            {
                //http://localhost:61131/CSAPI/HKWalletService.asmx/CheckRealName?appid=10001&Operator=v_yqyqguo&uin=o5PXlsk4_hPj-PkB8v10sFq7YAJs@wx.hkg&approval_id=201606106000003681465539312&state=2&memo=01&client_ip=127.0.0.0.1&token=7e6bc347332a8fd127611ed11179153d
                Dictionary<string, string> paramsHt = APIUtil.GetQueryStrings();
                //必填参数验证
                APIUtil.ValidateParamsNew(paramsHt, "appid", "Operator", "uin", "approval_id", "state", "client_ip", "token");
                // 非必填参数 :"memo  
                //token验证
                APIUtil.ValidateToken(paramsHt);


                string Operator, uin, approval_id, state, client_ip, memo = "";

                Operator = paramsHt["Operator"].ToString();
                uin = paramsHt["uin"].ToString();
                approval_id = paramsHt["approval_id"].ToString();
                state = paramsHt["state"].ToString();
                client_ip = paramsHt["client_ip"].ToString();
                if (paramsHt.Keys.Contains("memo"))
                {
                    memo = paramsHt["memo"].ToString();
                }
                string stateMsg = "";
                //try
                //{
                if ("0" == new FCXGWallet().checkRealName(Operator, uin, approval_id, state, memo, client_ip))
                {
                    stateMsg = "0";
                }
                //}
                //catch (Exception ex)
                //{
                //    stateMsg = HttpUtility.JavaScriptStringEncode(ex.Message);
                //}

                Record record = new Record();
                record.RetValue = stateMsg;
                List<Record> list = new List<Record>();
                list.Add(record);

                APIUtil.Print<Record>(list);
            }
            catch (ServiceException se)
            {
                SunLibrary.LoggerFactory.Get("GetHKRealNameInfo").ErrorFormat("return_code:{0},msg:{1}", se.GetRetcode, se.GetRetmsg);
                APIUtil.PrintError(se.GetRetcode, se.GetRetmsg);
            }
            catch (Exception ex)
            {
                SunLibrary.LoggerFactory.Get("GetHKRealNameInfo").ErrorFormat("return_code:{0},msg:{1}", APIUtil.ERR_SYSTEM, ex.Message);
                APIUtil.PrintError(APIUtil.ERR_SYSTEM, ErroMessage.MESSAGE_ERROBUSINESS);
            }
        }

    }
}
