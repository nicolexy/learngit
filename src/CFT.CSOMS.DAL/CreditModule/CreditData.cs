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
            var tokenValue = "uin=" + uin + "&username=" + username + "&key=" + key;
            var token = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tokenValue, "md5");
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
                var ip = System.Configuration.ConfigurationManager.AppSettings["TencentCreditSearchAccountInfoIP"] ?? "10.231.45.238";// "10.123.9.162";
                var port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TencentCreditSearchAccountInfoPort"] ?? "22000");
                var key = System.Configuration.ConfigurationManager.AppSettings["TencentCreditSearchAccountInfoKey"] ?? "bs1hat86vpw6mkrqrxn92nywn1mr345k";// "7tzkfz7u18fbbibf7vb62662vqwnblaq";
                string requestType = System.Configuration.ConfigurationManager.AppSettings["TencentCreditSearchAccountInfoRequestType"] ?? "102804";// "111144";

                #region tokenValue
                Dictionary<string, string> dic = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(accountNo))
                {
                    dic.Add("acct_no", accountNo);
                }
                if (accountType>=0)
                {
                    dic.Add("acct_type", accountType.ToString());
                }
                dic.Add("ts", timeStamp);
                
                Dictionary<string, string> dicAsc= dic.OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value);
                dicAsc.Add("key", key);
                StringBuilder sb_tokenValue = new StringBuilder();
                if (dicAsc != null && dicAsc.Count > 0)
                {
                    int i= 0;
                    foreach (var item in dicAsc)
                    {
                        i++;
                        if (i < dicAsc.Count)
                        {
                            sb_tokenValue.Append(item.Key).Append("=").Append(item.Value).Append("&");
                        }
                        else
                        {
                            sb_tokenValue.Append(item.Key).Append("=").Append(item.Value);
                        }
                    }
                }
                #endregion

                //var tokenValue = "acct_no=" + accountNo + "&acct_type=" + accountType + "&key=" + key;
                //var token = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tokenValue, "md5");
                var token = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sb_tokenValue.ToString(), "md5");
                StringBuilder sb_RequestString = new StringBuilder();
                sb_RequestString.Append("acct_no=").Append(accountNo);
                sb_RequestString.Append("&acct_type=").Append(accountType);
                sb_RequestString.Append("&ts=").Append(timeStamp);
                sb_RequestString.Append("&sign=").Append(token);
                LogHelper.LogInfo("SearchAccountInfo_RequestString:" + sb_RequestString.ToString());
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

        /// <summary>
        /// 账单查询
        /// </summary>
        /// <param name="accountNo">用户账户</param>
        /// <param name="accountType">账户类型：0=手Q;1=微信；2=身份证(全)</param>
        /// <param name="billStatus">账单状态</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="searchResult">调用接口返回结果，true  false</param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public string LoadBillList(string accountNo, int accountType, int billStatus, string beginDate, string endDate, string timeStamp, int pageSize, int pageNumber, string order,  out string errorMessage)
        {
            errorMessage = string.Empty;
            string result = string.Empty;
            try
            {
                var ip = System.Configuration.ConfigurationManager.AppSettings["TencentCreditLoadBillListIP"] ?? "10.231.45.238";// "10.123.9.162";
                var port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TencentCreditLoadBillListPort"] ?? "22000");
                var key = System.Configuration.ConfigurationManager.AppSettings["TencentCreditLoadBillListKey"] ?? "bs1hat86vpw6mkrqrxn92nywn1mr345k";// "7tzkfz7u18fbbibf7vb62662vqwnblaq";
                string requestType = System.Configuration.ConfigurationManager.AppSettings["TencentCreditLoadBillListRequestType"] ?? "102805";// "111145";                
                             
                #region tokenValue
                Dictionary<string, string> dic = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(accountNo))
                {
                    dic.Add("acct_no", accountNo);
                }
                if (accountType >= 0)
                {
                    dic.Add("acct_type", accountType.ToString());
                }
                if (!string.IsNullOrEmpty(beginDate))
                {
                    dic.Add("start_date", DateTime.Parse(beginDate).ToString("yyyyMMdd"));                    
                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    dic.Add("end_date", DateTime.Parse(beginDate).ToString("yyyyMMdd"));                    
                }
                //if (!string.IsNullOrEmpty(next_row_key) && !next_row_key.Equals("0"))
                //{
                //    dic.Add("next_row_key", next_row_key);
                //}
                dic.Add("ts", timeStamp);
                dic.Add("status", billStatus.ToString());
                dic.Add("page_size", pageSize.ToString());
                dic.Add("page_offset", (pageNumber - 1).ToString());      
                          
                Dictionary<string, string> dicAsc = dic.OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value);
                dicAsc.Add("key", key);
                StringBuilder sb_tokenValue = new StringBuilder();
                if (dicAsc != null && dicAsc.Count > 0)
                {
                    int i = 0;
                    foreach (var item in dicAsc)
                    {
                        i++;
                        if (i < dicAsc.Count)
                        {
                            sb_tokenValue.Append(item.Key).Append("=").Append(item.Value).Append("&");
                        }
                        else
                        {
                            sb_tokenValue.Append(item.Key).Append("=").Append(item.Value);
                        }
                    }
                }
                #endregion
                var token = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sb_tokenValue.ToString(), "md5");

                StringBuilder sb_RequestString = new StringBuilder();
                if (!string.IsNullOrEmpty(accountNo))
                {
                    sb_RequestString.Append("acct_no=").Append(accountNo);
                    sb_RequestString.Append("&acct_type=").Append(accountType);
                }
                sb_RequestString.Append("&ts=").Append(timeStamp);
                if (!string.IsNullOrEmpty(beginDate))
                {
                    sb_RequestString.Append("&start_date=").Append(DateTime.Parse(beginDate).ToString("yyyyMMdd"));
                }
                if (!string.IsNullOrEmpty(beginDate))
                {
                    sb_RequestString.Append("&end_date=").Append(DateTime.Parse(endDate).ToString("yyyyMMdd"));
                }

                sb_RequestString.Append("&status=").Append(billStatus);
                sb_RequestString.Append("&page_size=").Append(pageSize);
                sb_RequestString.Append("&page_offset=").Append(pageNumber - 1);                
                sb_RequestString.Append("&sign=").Append(token);
                //string testRequest = "acct_no=2092833410&acct_type=0&page_offset=0&page_size=1&ts=1476843756&key=7tzkfz7u18fbbibf7vb62662vqwnblaq&sign=c542651e07938d961426bcd33fd64094";
                LogHelper.LogInfo("LoadBillList_RequestString:" + sb_RequestString.ToString());
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

        /// <summary>
        /// 账单查询
        /// </summary>
        /// <param name="accountNo">用户账户</param>
        /// <param name="accountType">账户类型：0=手Q;1=微信；2=身份证(全)</param>
        /// <param name="billId">账单ID</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="searchResult">调用接口返回结果，true  false</param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public string LoadBillDetailInfo(string accountNo, int accountType, string billId, string beginDate, string endDate, string timeStamp, int pageSize, int pageNumber, string order, out string errorMessage)
        {
            errorMessage = string.Empty;
            string result = string.Empty;
            try
            {
                var ip = System.Configuration.ConfigurationManager.AppSettings["TencentCreditLoadBillDetailInfoIP"] ?? "10.231.45.238";// "10.123.9.162";
                var port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TencentCreditLoadBillDetailInfoPort"] ?? "22000");
                var key = System.Configuration.ConfigurationManager.AppSettings["TencentCreditLoadBillDetailInfoKey"] ?? "bs1hat86vpw6mkrqrxn92nywn1mr345k";// "7tzkfz7u18fbbibf7vb62662vqwnblaq";
                string requestType = System.Configuration.ConfigurationManager.AppSettings["TencentCreditLoadBillDetailInfoRequestType"] ?? "102807";// "111146";

                #region tokenValue
                Dictionary<string, string> dic = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(accountNo))
                {
                    dic.Add("acct_no", accountNo);
                }
                if (accountType >= 0)
                {
                    dic.Add("acct_type", accountType.ToString());
                }
                if (!string.IsNullOrEmpty(billId))
                {
                    dic.Add("bill_id", billId);
                }
                if (!string.IsNullOrEmpty(beginDate))
                {
                    dic.Add("start_date", DateTime.Parse(beginDate).ToString("yyyyMMdd"));
                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    dic.Add("end_date", DateTime.Parse(endDate).ToString("yyyyMMdd"));
                }
                //if (!string.IsNullOrEmpty(next_row_key) && !next_row_key.Equals("0"))
                //{
                //    dic.Add("next_row_key", next_row_key);
                //}
                dic.Add("ts", timeStamp);                
                dic.Add("page_size", pageSize.ToString());
                dic.Add("page_offset", (pageNumber - 1).ToString());
                         
                Dictionary<string, string> dicAsc = dic.OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value);
                dicAsc.Add("key", key);
                StringBuilder sb_tokenValue = new StringBuilder();
                if (dicAsc != null && dicAsc.Count > 0)
                {
                    int i = 0;
                    foreach (var item in dicAsc)
                    {
                        i++;
                        if (i < dicAsc.Count)
                        {
                            sb_tokenValue.Append(item.Key).Append("=").Append(item.Value).Append("&");
                        }
                        else
                        {
                            sb_tokenValue.Append(item.Key).Append("=").Append(item.Value);
                        }
                    }
                }
                #endregion
                var token = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sb_tokenValue.ToString(), "md5");

                StringBuilder sb_RequestString = new StringBuilder();
                if (!string.IsNullOrEmpty(accountNo))
                {
                    sb_RequestString.Append("acct_no=").Append(accountNo);
                    sb_RequestString.Append("&acct_type=").Append(accountType);
                }
                sb_RequestString.Append("&ts=").Append(timeStamp);
                sb_RequestString.Append("&bill_id=").Append(billId);
                if (!string.IsNullOrEmpty(beginDate))
                {
                    sb_RequestString.Append("&start_date=").Append(DateTime.Parse(beginDate).ToString("yyyyMMdd"));
                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    sb_RequestString.Append("&end_date=").Append(DateTime.Parse(endDate).ToString("yyyyMMdd"));
                }               
                sb_RequestString.Append("&page_size=").Append(pageSize);
                sb_RequestString.Append("&page_offset=").Append(pageNumber-1);
                //var tokenValue = sb_RequestString.ToString() + "&key=" + key;
                //var token = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tokenValue, "md5");
                sb_RequestString.Append("&sign=").Append(token);
                //string testRequest = "acct_no=2092833410&acct_type=0&bill_id=B001706e8930b24b2919459990003275&page_offset=0&page_size=10&ts=1476848739&sign=1b08426d3007846a9ddcb07eacfaf906";
                LogHelper.LogInfo("LoadBillDetailInfo_RequestString:" + sb_RequestString.ToString());
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

        
        /// <summary>
        /// 支付明细
        /// </summary>
        /// <param name="accountNo">用户账户</param>
        /// <param name="accountType">账户类型：0=手Q;1=微信；2=身份证(全)</param>
        /// <param name="req_type">查询方式：0=按时间段；1=按交易订单号+时间段</param>
        /// <param name="trans_id">交易订单ID</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="searchResult">调用接口返回结果，true  false</param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public string LoadPayList(string accountNo, int accountType, int req_type, string trans_id, string beginDate, string endDate, string timeStamp, int pageSize, int pageNumber, string next_row_key, string order, out string errorMessage)
        {
            errorMessage = string.Empty;
            string result = string.Empty;
            try
            {
                var ip = System.Configuration.ConfigurationManager.AppSettings["TencentCreditLoadPayListIP"] ?? "10.231.45.238";// "10.123.9.162";
                var port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TencentCreditLoadLoadPayListPort"] ?? "22000");
                var key = System.Configuration.ConfigurationManager.AppSettings["TencentCreditLoadPayListKey"] ?? "bs1hat86vpw6mkrqrxn92nywn1mr345k";// "7tzkfz7u18fbbibf7vb62662vqwnblaq";
                string requestType = System.Configuration.ConfigurationManager.AppSettings["TencentCreditLoadPayListRequestType"] ?? "102816";// "111140";

                #region tokenValue
                Dictionary<string, string> dic = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(accountNo))
                {
                    dic.Add("acct_no", accountNo);
                }
                if (req_type >= 0)
                {
                    dic.Add("req_type", req_type.ToString());
                }
                if (!string.IsNullOrEmpty(trans_id))
                {
                    dic.Add("trans_id", trans_id);
                }
                if (!string.IsNullOrEmpty(beginDate))
                {
                    dic.Add("start_date", DateTime.Parse(beginDate).ToString("yyyyMMdd"));
                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    dic.Add("end_date", DateTime.Parse(endDate).ToString("yyyyMMdd"));
                }
                if (!string.IsNullOrEmpty(next_row_key) && !next_row_key.Equals("0"))
                {
                    dic.Add("next_row_key", next_row_key);
                }
                dic.Add("ts", timeStamp);
                dic.Add("page_size", pageSize.ToString());
                dic.Add("page_offset", (pageNumber - 1).ToString());
                
                Dictionary<string, string> dicAsc = dic.OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value);
                dicAsc.Add("key", key);
                StringBuilder sb_tokenValue = new StringBuilder();
                if (dicAsc != null && dicAsc.Count > 0)
                {
                    int i = 0;
                    foreach (var item in dicAsc)
                    {
                        i++;
                        if (i < dicAsc.Count)
                        {
                            sb_tokenValue.Append(item.Key).Append("=").Append(item.Value).Append("&");
                        }
                        else
                        {
                            sb_tokenValue.Append(item.Key).Append("=").Append(item.Value);
                        }
                    }
                }
                #endregion
                var token = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sb_tokenValue.ToString(), "md5");

                StringBuilder sb_RequestString = new StringBuilder();

                if (!string.IsNullOrEmpty(accountNo))
                {
                    sb_RequestString.Append("acct_no=").Append(accountNo);
                    sb_RequestString.Append("&acct_type=").Append(accountType);
                }
                sb_RequestString.Append("&ts=").Append(timeStamp);
                sb_RequestString.Append("&req_type=").Append(req_type);
                if (!string.IsNullOrEmpty(trans_id))
                {
                    sb_RequestString.Append("&trans_id=").Append(trans_id);
                }

                if (!string.IsNullOrEmpty(beginDate))
                {
                    sb_RequestString.Append("&start_date=").Append(DateTime.Parse(beginDate).ToString("yyyyMMdd"));
                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    sb_RequestString.Append("&end_date=").Append(DateTime.Parse(endDate).ToString("yyyyMMdd"));
                }  
                sb_RequestString.Append("&page_size=").Append(pageSize);
                if (!string.IsNullOrEmpty(next_row_key) && !next_row_key.Equals("0"))
                {
                    sb_RequestString.Append("&next_row_key=").Append(next_row_key);
                }

                //var tokenValue = sb_RequestString.ToString() + "&key=" + key;
                //var token = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tokenValue, "md5");
                sb_RequestString.Append("&sign=").Append(token);

                //string testRequest = "acct_no=1911162410&acct_type=0&end_date=20161014&page_size=10&req_type=0&start_date=20161010&ts=1476698617&key=7tzkfz7u18fbbibf7vb62662vqwnblaq&sign=c7c35a1e9aa03aa4f1cd1e25acf9af4f";

                LogHelper.LogInfo("LoadPayList_RequestString:" + sb_RequestString.ToString());
                result = RelayAccessFactory.RelayInvoke(sb_RequestString.ToString(), requestType, false, false, ip, port);//sb_RequestString.ToString()
            }
            catch (Exception ex)
            {
                result = string.Empty;
                errorMessage = "调用接口失败";
                LogHelper.LogInfo("CreditData.SearchAccountInfo,error:" + ex.ToString());
            }
            return result;
        }

        /// <summary>
        /// 还款明细查询
        /// </summary>
        /// <param name="accountNo">用户账户</param>
        /// <param name="accountType">账户类型：0=手Q;1=微信；2=身份证(全)</param>
        /// <param name="req_type">查询方式：0=按时间段；1=按交易订单号+时间段</param>
        /// <param name="trans_id">交易订单ID</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="searchResult">调用接口返回结果，true  false</param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public string LoadRepayList(string accountNo, int accountType, int req_type, string trans_id, string beginDate, string endDate, string timeStamp, int pageSize, int pageNumber, string next_row_key, string order, out string errorMessage)
        {
            errorMessage = string.Empty;
            string result = string.Empty;
            try
            {
                var ip = System.Configuration.ConfigurationManager.AppSettings["TencentCreditLoadRepayListIP"] ?? "10.231.45.238";// "10.123.9.162";
                var port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TencentCreditLoadRepayListPort"] ?? "22000");
                var key = System.Configuration.ConfigurationManager.AppSettings["TencentCreditLoadRepayListKey"] ?? "bs1hat86vpw6mkrqrxn92nywn1mr345k";// "7tzkfz7u18fbbibf7vb62662vqwnblaq";
                string requestType = System.Configuration.ConfigurationManager.AppSettings["TencentCreditLoadRepayListRequestType"] ?? "102822";// "111141";

                #region tokenValue
                Dictionary<string, string> dic = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(accountNo))
                {
                    dic.Add("acct_no", accountNo);
                }
                if (accountType >= 0)
                {
                    dic.Add("acct_type", accountType.ToString());
                }
                if (req_type >= 0)
                {
                    dic.Add("req_type", req_type.ToString());
                }
                if (!string.IsNullOrEmpty(trans_id))
                {
                    dic.Add("trans_id", trans_id);
                }
                if (!string.IsNullOrEmpty(beginDate))
                {
                    dic.Add("start_date", DateTime.Parse(beginDate).ToString("yyyyMMdd"));
                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    dic.Add("end_date", DateTime.Parse(endDate).ToString("yyyyMMdd"));
                }
                if (!string.IsNullOrEmpty(next_row_key) && !next_row_key.Equals("0"))
                {
                    dic.Add("next_row_key", next_row_key);                    
                }
                dic.Add("ts", timeStamp);
                dic.Add("page_size", pageSize.ToString());
                dic.Add("page_offset", (pageNumber - 1).ToString());
                
                Dictionary<string, string> dicAsc = dic.OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value);
                dicAsc.Add("key", key);
                StringBuilder sb_tokenValue = new StringBuilder();
                if (dicAsc != null && dicAsc.Count > 0)
                {
                    int i = 0;
                    foreach (var item in dicAsc)
                    {
                        i++;
                        if (i < dicAsc.Count)
                        {
                            sb_tokenValue.Append(item.Key).Append("=").Append(item.Value).Append("&");
                        }
                        else
                        {
                            sb_tokenValue.Append(item.Key).Append("=").Append(item.Value);
                        }
                    }
                }
                #endregion
                var token = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sb_tokenValue.ToString(), "md5");

                StringBuilder sb_RequestString = new StringBuilder();

                if (!string.IsNullOrEmpty(accountNo))
                {
                    sb_RequestString.Append("acct_no=").Append(accountNo);
                    sb_RequestString.Append("&acct_type=").Append(accountType);
                }
                sb_RequestString.Append("&ts=").Append(timeStamp);
                sb_RequestString.Append("&req_type=").Append(req_type);
                if (!string.IsNullOrEmpty(trans_id))
                {
                    sb_RequestString.Append("&trans_id=").Append(trans_id);
                }

                if (!string.IsNullOrEmpty(beginDate))
                {
                    sb_RequestString.Append("&start_date=").Append(DateTime.Parse(beginDate).ToString("yyyyMMdd"));
                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    sb_RequestString.Append("&end_date=").Append(DateTime.Parse(endDate).ToString("yyyyMMdd"));
                }  
                sb_RequestString.Append("&page_size=").Append(pageSize);
                if (!string.IsNullOrEmpty(next_row_key) && !next_row_key.Equals("0"))
                {
                    sb_RequestString.Append("&next_row_key=").Append(next_row_key);
                }

                //var tokenValue = sb_RequestString.ToString() + "&key=" + key;
                //var token = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tokenValue, "md5");
                sb_RequestString.Append("&sign=").Append(token);
                //string testRquest = "acct_no=2056226882&acct_type=0&end_date=20161017&page_size=1&req_type=0&start_date=20160901&ts=1476710359&key=7tzkfz7u18fbbibf7vb62662vqwnblaq&sign=0435fb1d4a15b14947d67aa37237945a";
                LogHelper.LogInfo("LoadRepayList_RequestString:" + sb_RequestString.ToString());
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

         /// <summary>
        /// 退款单查询
        /// </summary>
        /// <param name="accountNo">用户账户</param>
        /// <param name="accountType">账户类型：0=手Q;1=微信；2=身份证(全)</param>
        /// <param name="req_type">查询方式：0=按时间段；1=按交易订单号+时间段</param>
        /// <param name="trans_id">交易订单ID</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="searchResult">调用接口返回结果，true  false</param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public string LoadRefundList(string accountNo, int accountType, int req_type, string trans_id, string beginDate, string endDate, string timeStamp, int pageSize, int pageNumber,string next_row_key,string order,  out string errorMessage)
        {
            errorMessage = string.Empty;
            string result = string.Empty;
            try
            {
                var ip = System.Configuration.ConfigurationManager.AppSettings["TencentCreditLoadRefundListIP"] ?? "10.231.45.238";// "10.123.9.162";
                var port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TencentCreditLoadRefundListPort"] ?? "22000");
                var key = System.Configuration.ConfigurationManager.AppSettings["TencentCreditLoadRefundListKey"] ?? "bs1hat86vpw6mkrqrxn92nywn1mr345k";// "7tzkfz7u18fbbibf7vb62662vqwnblaq";
                string requestType = System.Configuration.ConfigurationManager.AppSettings["TencentCreditLoadRefundListRequestType"] ?? "102824";// "111142";

                #region tokenValue
                Dictionary<string, string> dic = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(accountNo))
                {
                    dic.Add("acct_no", accountNo);
                }
                if (accountType >= 0)
                {
                    dic.Add("acct_type", accountType.ToString());
                }
                if (req_type >= 0)
                {
                    dic.Add("req_type", req_type.ToString());
                }
                if (!string.IsNullOrEmpty(trans_id))
                {
                    dic.Add("trans_id", trans_id);
                }
                if (!string.IsNullOrEmpty(beginDate))
                {
                    dic.Add("start_date", DateTime.Parse(beginDate).ToString("yyyyMMdd"));
                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    dic.Add("end_date", DateTime.Parse(endDate).ToString("yyyyMMdd"));
                }
                if (!string.IsNullOrEmpty(next_row_key) && !next_row_key.Equals("0"))
                {
                    dic.Add("next_row_key", next_row_key);
                }
                dic.Add("ts", timeStamp);
                dic.Add("page_size", pageSize.ToString());
                dic.Add("page_offset", (pageNumber - 1).ToString());
                
                Dictionary<string, string> dicAsc = dic.OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value);
                dicAsc.Add("key", key);
                StringBuilder sb_tokenValue = new StringBuilder();
                if (dicAsc != null && dicAsc.Count > 0)
                {
                    int i = 0;
                    foreach (var item in dicAsc)
                    {
                        i++;
                        if (i < dicAsc.Count)
                        {
                            sb_tokenValue.Append(item.Key).Append("=").Append(item.Value).Append("&");
                        }
                        else
                        {
                            sb_tokenValue.Append(item.Key).Append("=").Append(item.Value);
                        }
                    }
                }
                #endregion
                var token = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sb_tokenValue.ToString(), "md5");

                StringBuilder sb_RequestString = new StringBuilder();
                if (!string.IsNullOrEmpty(accountNo))
                {
                    sb_RequestString.Append("acct_no=").Append(accountNo);
                    sb_RequestString.Append("&acct_type=").Append(accountType);
                }
                sb_RequestString.Append("&ts=").Append(timeStamp);
                sb_RequestString.Append("&req_type=").Append(req_type);
                if (!string.IsNullOrEmpty(trans_id))
                {
                    sb_RequestString.Append("&trans_id=").Append(trans_id);
                }

                if (!string.IsNullOrEmpty(beginDate))
                {
                    sb_RequestString.Append("&start_date=").Append(DateTime.Parse(beginDate).ToString("yyyyMMdd"));
                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    sb_RequestString.Append("&end_date=").Append(DateTime.Parse(endDate).ToString("yyyyMMdd"));
                }  
                sb_RequestString.Append("&page_size=").Append(pageSize);
                if (!string.IsNullOrEmpty(next_row_key) && !next_row_key.Equals("0"))
                {
                    sb_RequestString.Append("&next_row_key=").Append(next_row_key);
                }
                
                //var tokenValue = sb_RequestString.ToString() + "&key=" + key;
                //var token = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tokenValue, "md5");
                sb_RequestString.Append("&sign=").Append(token);
                //string testRquest = "acct_no=1911162410&acct_type=0&end_date=20161014&page_size=5&req_type=0&start_date=20161010&ts=1476706748&key=7tzkfz7u18fbbibf7vb62662vqwnblaq&sign=22f40333ae731d16999c56150f02c0d6";
                LogHelper.LogInfo("LoadRefundList_RequestString:" + sb_RequestString.ToString());
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

        /// <summary>
        /// 退款明细
        /// </summary>
        /// <param name="accountNo">用户账户</param>
        /// <param name="accountType">账户类型：0=手Q;1=微信；2=身份证(全)</param>        
        /// <param name="refund_flow_id">退款流水ID</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="searchResult">调用接口返回结果，true  false</param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public string LoadRefundDetail(string accountNo, int accountType, string refund_flow_id, string c_rchg_id, string beginDate, string endDate, string timeStamp, int pageSize, int pageNumber, string order, out string errorMessage)
        {
            errorMessage = string.Empty;
            string result = string.Empty;
            try
            {
                var ip = System.Configuration.ConfigurationManager.AppSettings["TencentCreditLoadRefundDetailIP"] ?? "10.231.45.238";// "10.123.9.162";
                var port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TencentCreditLoadRefundDetailPort"] ?? "22000");
                var key = System.Configuration.ConfigurationManager.AppSettings["TencentCreditLoadRefundDetailKey"] ?? "bs1hat86vpw6mkrqrxn92nywn1mr345k";// "7tzkfz7u18fbbibf7vb62662vqwnblaq";
                string requestType = System.Configuration.ConfigurationManager.AppSettings["TencentCreditLoadRefundDetailRequestType"] ?? "102825";// "111143";

                #region tokenValue
                Dictionary<string, string> dic = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(accountNo))
                {
                    dic.Add("acct_no", accountNo);
                }
                if (accountType >= 0)
                {
                    dic.Add("acct_type", accountType.ToString());
                }
                if (!string.IsNullOrEmpty(refund_flow_id))
                {
                    dic.Add("refund_flow_id", refund_flow_id);
                }
                if (!string.IsNullOrEmpty(c_rchg_id))
                {
                    dic.Add("c_rchg_id", c_rchg_id);
                }
                if (!string.IsNullOrEmpty(beginDate))
                {
                    dic.Add("start_date", DateTime.Parse(beginDate).ToString("yyyyMMdd"));
                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    dic.Add("end_date", DateTime.Parse(endDate).ToString("yyyyMMdd"));
                }              
                dic.Add("ts", timeStamp);
                dic.Add("page_size", pageSize.ToString());
                dic.Add("page_offset", (pageNumber - 1).ToString());
                
                Dictionary<string, string> dicAsc = dic.OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value);
                dicAsc.Add("key", key);
                StringBuilder sb_tokenValue = new StringBuilder();
                if (dicAsc != null && dicAsc.Count > 0)
                {
                    int i = 0;
                    foreach (var item in dicAsc)
                    {
                        i++;
                        if (i < dicAsc.Count)
                        {
                            sb_tokenValue.Append(item.Key).Append("=").Append(item.Value).Append("&");
                        }
                        else
                        {
                            sb_tokenValue.Append(item.Key).Append("=").Append(item.Value);
                        }
                    }
                }
                #endregion
                var token = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sb_tokenValue.ToString(), "md5");

                StringBuilder sb_RequestString = new StringBuilder();
                if (!string.IsNullOrEmpty(accountNo))
                {
                    sb_RequestString.Append("acct_no=").Append(accountNo);
                    sb_RequestString.Append("&acct_type=").Append(accountType);
                }
                sb_RequestString.Append("&ts=").Append(timeStamp);
                if (!string.IsNullOrEmpty(refund_flow_id))
                {
                    sb_RequestString.Append("&refund_flow_id=").Append(refund_flow_id);
                }                
                if (!string.IsNullOrEmpty(c_rchg_id))
                {
                    sb_RequestString.Append("&c_rchg_id=").Append(c_rchg_id);
                }
                if (!string.IsNullOrEmpty(beginDate))
                {
                    sb_RequestString.Append("&start_date=").Append(DateTime.Parse(beginDate).ToString("yyyyMMdd"));
                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    sb_RequestString.Append("&end_date=").Append(DateTime.Parse(endDate).ToString("yyyyMMdd"));
                }  
                //sb_RequestString.Append("&page_size=").Append(pageSize);
                //sb_RequestString.Append("&next_row_key=").Append(pageNumber);
                //var tokenValue = sb_RequestString.ToString() + "&key=" + key;
                //var token = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tokenValue, "md5");
                sb_RequestString.Append("&sign=").Append(token);
                string testRquest = "acct_no=2547962691&acct_type=0&end_date=20161020&page_size=1&refund_flow_id=30000000201610251530479990024610&start_date=20161026&ts=1476706748&sign=22f40333ae731d16999c56150f02c0d6";//&key=7tzkfz7u18fbbibf7vb62662vqwnblaq
                LogHelper.LogInfo("LoadRefundList_RequestString:" + sb_RequestString.ToString());
                result = RelayAccessFactory.RelayInvoke(sb_RequestString.ToString(), requestType, false, false, ip, port);
                //result = "refund_time=20161010%2017%3A24%3A13&refund_trans_id=1091301278501201610254931202&res_info=ok&result=0&sp_bill_no=&sp_name=%CA%D6Q%C9%CC%BB%A7&trans_id=10000000201610251054479990005553&trans_info=&trans_time=&row_0=balance_go%3D%25E4%25BD%2599%25E9%25A2%259D%26refund_amount%3D1%26rf_trans_id%3D%25E5%25A4%2584%25E7%2590%2586%25E4%25B8%25AD&row_1=balance_go%3D%25E4%25BD%2599%25E9%25A2%259D%26refund_amount%3D1%26rf_trans_id%3D%25E5%25A4%2584%25E7%2590%2586%25E4%25B8%25AD&row_2=balance_go%3D%25E4%25BD%2599%25E9%25A2%259D%26refund_amount%3D1%26rf_trans_id%3D%25E5%25A4%2584%25E7%2590%2586%25E4%25B8%25AD";
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
