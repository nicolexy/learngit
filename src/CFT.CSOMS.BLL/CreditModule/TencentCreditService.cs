using CFT.CSOMS.DAL.CreditModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace CFT.CSOMS.BLL.CreditModule
{
    public class TencentCreditService
    {
        /// <summary>
        /// 腾讯信用查询
        /// </summary>
        /// <param name="uin">QQ号</param>
        /// <param name="username">操作员</param>
        /// <returns></returns>
        public DataSet TencentCreditQuery(string uin, string username)
        {
            return new CreditData().TencentCreditQuery(uin, username);
        }

        /// <summary>
        /// 账户信息查询
        /// </summary>
        /// <param name="accountNo">用户账户</param>
        /// <param name="accountType">账户类型：0=手Q;1=微信；2=身份证(全)</param>
        /// <returns></returns>
        public string SearchAccountInfo(string accountNo, int accountType)
        {
            string result = string.Empty;
            string errorMessage = string.Empty;
            try
            {
                CreditData creditDataDAL = new CreditData();
                string timeStamp = CommQuery.GetTimeStamp();
                string interfaceRetrnResult = creditDataDAL.SearchAccountInfo(accountNo, accountType, timeStamp, out errorMessage);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    result = string.Empty;
                }
            }
            catch (Exception ex)
            {
                result = string.Empty;
            }
            return result;
        }

       /// <summary>
        /// 账户信息查询
       /// </summary>
        /// <param name="accountNo">用户账户</param>
        /// <param name="accountType">账户类型：0=手Q;1=微信；2=身份证(全)</param>
       /// <param name="searchResult">调用接口返回结果，true</param>
       /// <param name="errorMessage"></param>
       /// <returns></returns>
        public string SearchAccountInfo(string accountNo, int accountType, out bool searchResult, out string errorMessage)
        {
            string returnResult = string.Empty;
            errorMessage = string.Empty;
            searchResult = false;
            try
            {
                string interfaceRetrnResult = SearchAccountInfo(accountNo, accountType);
                if (!string.IsNullOrEmpty(interfaceRetrnResult))
                {
                    returnResult = interfaceRetrnResult;
                    //调用接口返回结果                
                    var interfaceRetrnResultJson = Newtonsoft.Json.JsonConvert.DeserializeObject(interfaceRetrnResult) as Newtonsoft.Json.Linq.JObject;
                    if (interfaceRetrnResultJson != null && interfaceRetrnResultJson.Count > 0)
                    {
                        string resultValue = interfaceRetrnResultJson["result"].ToString();
                        searchResult = resultValue.Equals("0") ? true : false;
                        if (resultValue.Equals("0"))
                        {
                            errorMessage = "请求执行成功";// interfaceRetrnResultJson["res_info"].ToString();
                        }
                        else
                        {
                            errorMessage = "请求执行失败,账户不存在";// interfaceRetrnResultJson["res_info"].ToString();
                        }
                    }
                }
                else
                {
                    searchResult = false;
                    errorMessage = "接口调用失败";
                }
            }
            catch (Exception ex)
            {
                returnResult = string.Empty;
            }
            return returnResult;
        }
    }
}
