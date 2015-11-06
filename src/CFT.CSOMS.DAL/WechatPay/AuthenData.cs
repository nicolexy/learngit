using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CFT.Apollo.Logging;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using SunLibraryEX;

namespace CFT.CSOMS.DAL.WechatPay
{
    public class AuthenData
    {
        public DataSet QueryWechatRealNameAuthen(string uin, string serialno, string clientIp)
        {       
            string msg = string.Empty;
            string serviceName = "au_check_cre_service";
            StringBuilder reqBuilder = new StringBuilder()
            .AppendFormat("uin={0}", uin)
            .AppendFormat("&bind_serialno={0}", serialno)
            .AppendFormat("&client_ip={0}", clientIp);

            LogHelper.LogInfo(serviceName + " send req:" + reqBuilder.ToString());
            DataSet ds = TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.GetOneTableFromICE(reqBuilder.ToString(), "", serviceName, false, out msg);
            LogHelper.LogInfo(serviceName + " return:" + msg);

            return ds;

        }

        /// <summary>
        /// 微信CKV值获取和同步CKV
        /// </summary>
        /// <param name="flag">操作类型 1:查询 2:同步</param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public Dictionary<string, string> WXOperateCKVCGI(int flag, string uid)
        {
            var url = System.Configuration.ConfigurationManager.AppSettings["WXOperateCKVCGI"] ?? "http://op.cf.com/perltools/cgi-bin/wx_balance_kf_tool";
            var req = url +
                "?jsonstr={" +
                "\"flag\":\"" + flag + "\"," +
                "\"uid\":\"" + uid + "\"" +
                "}";
            string msg = "";
            var answer = commRes.GetFromCGI(req, null, out msg);
            if (msg != "")
            {
                throw new Exception("调用CGI出错:" + msg);
            }
            if (string.IsNullOrEmpty(answer))
            {
                throw new Exception("CGI返回值为空");
            }
            System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            var model = jss.DeserializeObject(answer) as Dictionary<string, object>;
            if (model["result"].ToString() != "0" || model["ret_data"].ToString() == "")
            {
                throw new Exception("调用CKV接口失败:[" + model["process_msg"] + model["result_msg"] + "]");
            }
            var retData = model["ret_data"].ToString();
            if (retData.IndexOf("err") != -1)
            {
                throw new Exception("调用CKV接口失败:[" + retData + "]");
            }
            retData = retData.Replace(" ", "").Replace("BALACNEVALUE:", "");
            var arr = retData.Split(new string[] { "<br>" }, StringSplitOptions.None);
            if (arr.Length == 3)
            {
                return arr[1].ToDictionary();
            }
            return null;
        }
    }
}
