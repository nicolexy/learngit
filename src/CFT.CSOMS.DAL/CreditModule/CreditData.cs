using CFT.CSOMS.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TENCENT.OSS.CFT.KF.DataAccess;

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
    }
}
