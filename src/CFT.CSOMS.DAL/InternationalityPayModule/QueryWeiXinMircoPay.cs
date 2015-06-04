using CFT.CSOMS.DAL.Infrastructure;
using commLib.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using SunLibraryEX;

namespace CFT.CSOMS.DAL.InternationalityPayModule
{
    public class QueryWeiXinMircoPay
    {
        //static Dictionary<string, string> CurrencyDict = new Dictionary<string, string>();
        //static QueryWeiXinMircoPay()
        //{
        //    #region 初始化字典信息
        //    //货币字典
        //    CurrencyDict.Add("USD", "美元");
        //    CurrencyDict.Add("HKD", "港币");
        //    CurrencyDict.Add("JPY", "日元");
        //    CurrencyDict.Add("EUR", "欧元");
        //    CurrencyDict.Add("GBP", "英镑");
        //    CurrencyDict.Add("MYR", "马来西亚币");
        //    CurrencyDict.Add("RUB", "卢布");
        //    #endregion
        //}
        /// <summary>
        /// 通过条件查询境外微信小额支付信息
        /// </summary>
        /// <param name="spid">商户号</param>
        /// <param name="out_trade_no">商户订单号</param>
        /// <param name="listid">微信订单号</param>
        /// <param name="cftlistid">财付通订单号</param>
        /// <param name="mobile">用户手机号码</param>
        /// <param name="name">用户名称</param>
        /// <param name="fromtime">查询开始时间</param>
        /// <param name="totime">查询结束时间</param>
        /// <param name="pagenum">页码</param>
        /// <param name="limit">页大小</param>   
        /// <param name="trade_state">交易状态</param>
        /// <returns></returns>
        public List<Fmicro_Order_Result> FMicroOrderList(string spid, string out_trade_no, string listid, string cftlistid, string mobile, string name, DateTime fromtime, DateTime totime, int pagenum, int limit, string trade_state)
        {
            var ip = System.Configuration.ConfigurationManager.AppSettings["ExternalWeiXinMircoPayIP"] ?? "172.27.31.177";
            var port = System.Configuration.ConfigurationManager.AppSettings["ExternalWeiXinMircoPayPort"] ?? "22000";
            string msg = "";
            var req = "ver=1&sp_id=2000000000&head_u=" +
                "&spid=" + spid +
                "&request_type=101052" +    //开发环境:100970,线上环境:101052
                "&out_trade_no=" + out_trade_no +
                "&listid=" + listid +
                "&cftlistid=" + cftlistid +
                "&mobile=" + mobile +
                "&name=" + CFT.CSOMS.COMMLIB.CommUtil.URLEncode(name) +
                "&fromtime=" + fromtime.ToString("yyyy-MM-dd") +
                "&totime=" + totime.ToString("yyyy-MM-dd") +
                "&pagenum=" + pagenum.ToString() +
                "&limit=" + limit.ToString() +
                "&trade_state=" + trade_state
                ;
            //string answer = commRes.GetFromRelay(req, ip, port, out msg);
            //string answer = RelayAccessFactory.RelayInvoke(req, "100970", false, false, ip, port);
            string answer = RelayAccessFactory.RelayInvoke(req, ip, int.Parse(port));
            #region 如果请求异常-->抛出异常信息
            if (msg != "")
            {
                throw new Exception(msg);
            }
            if (string.IsNullOrEmpty(answer))
            {
                throw new Exception("调用Relay接口返回值等于空!");
            }
            var dic = answer.ToDictionary('&','='); //AnalyzeDictionary(answer);
            if (dic["result"] != "0")
            {
                throw new Exception("查询失败:" + dic["res_info"]);
            }
            #endregion
            if (dic["total_num"] != "0")
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                return js.Deserialize<List<Fmicro_Order_Result>>(dic["row_info"]);
            }
            return null;
        }
    }

}
