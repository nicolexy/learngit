using CFT.CSOMS.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TENCENT.OSS.CFT.KF.DataAccess;
using CFT.Apollo.Logging;

namespace CFT.CSOMS.DAL.CreditModule
{
    public class CreditData
    {
        /// <summary>
        /// 腾讯信用查询
        /// </summary>
        /// <param name="uin"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public DataSet TencentCreditQuery(string uin, string username)
        {
            var ip = System.Configuration.ConfigurationManager.AppSettings["TencentCreditQueryIP"] ?? "172.27.31.177";
            var port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TencentCreditQueryPort"] ?? "22000");
            var key = System.Configuration.ConfigurationManager.AppSettings["TencentCreditQueryKey"] ?? "f58ac057fd7395ff4d372a05b9796d2b";
            var kokenValue = "uin=" + uin + "&username=" + username + "&key=" + key;
            var token = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(kokenValue, "md5");
            var req =
                "&uin=" + uin +
                "&username=" + username +
                "&token=" + token
                ;
            return RelayAccessFactory.GetDSFromRelay(req, "101025", ip, port);
        }

        /// <summary>
        /// 账户信息查询
        /// </summary>
        /// <param name="accountNo">用户账户</param>
        /// <param name="accountType">账户类型：0=手Q;1=微信；2=身份证(全)</param>
        /// <returns></returns>
        public string SearchAccountInfo(string accountNo, int accountType, string timeStamp, out string errorMessage)
        {
            errorMessage = string.Empty;
            string result = string.Empty;
            try
            {
                var ip = System.Configuration.ConfigurationManager.AppSettings["TencentCreditAccountSearchIP"] ?? "172.27.31.177";
                var port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TencentCreditAccountSearchPort"] ?? "22000");
                var key = System.Configuration.ConfigurationManager.AppSettings["TencentCreditAccountSearchKey"] ?? "8934e7d15453e97507ef794cf7b0519d";
                string requestType = System.Configuration.ConfigurationManager.AppSettings["TencentCreditAccountSearchRequestType"] ?? "101025";
                var kokenValue = "acct_no=" + accountNo + "&acct_type=" + accountType + "&key=" + key;
                var token = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(kokenValue, "md5").ToUpper();
               
                StringBuilder sb_RequestString = new StringBuilder();
                sb_RequestString.Append("acct_no=").Append(accountNo);
                sb_RequestString.Append("&acct_type=").Append(accountType);
                sb_RequestString.Append("&ts=").Append(timeStamp);
                sb_RequestString.Append("&sign=").Append(token);
                result = RelayAccessFactory.RelayInvoke(sb_RequestString.ToString(), requestType, false, false, ip, port);
            }
            catch (Exception ex)
            {
                result = string.Empty;
                errorMessage = "调用接口失败";
                LogHelper.LogInfo("CreditData.SearchAccountInfo,error:" + ex.ToString());
            }
            return result;
        }
    }
}
