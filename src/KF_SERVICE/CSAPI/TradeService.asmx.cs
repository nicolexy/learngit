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
using CFT.CSOMS.COMMLIB;
namespace CSAPI
{
  /// <summary>
    /// Summary description for WechatService
    /// </summary>
    [WebService(Namespace = "")]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class TradeService : System.Web.Services.WebService
    {
        #region 网银
        [WebMethod]
        public void  AddRefundInfo()
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
    }
}
