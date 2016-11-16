using CFT.CSOMS.DAL.CreditModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using commLib;
using System.Web;
using CFT.Apollo.Logging;

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
        /// <param name="searchResult">调用接口返回结果，true  false</param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public string LoadAccountInfo(string accountNo, int accountType, out bool searchResult, out string errorMessage)
        {
            string returnResult = string.Empty;
            errorMessage = string.Empty;
            searchResult = false;
            try
            {
                CreditData creditDataDAL = new CreditData();
                string timeStamp = CommQuery.GetTimeStamp();
                string interfaceRetrunResult = string.Empty;
                interfaceRetrunResult = creditDataDAL.SearchAccountInfo(accountNo, accountType, timeStamp, out errorMessage);
                //interfaceRetrunResult = "acct_no=263520382&acct_type=0&cft_status=1&credit=20000&credit_freeze=0&credit_line=20000&credit_used=0&cur_type=1&id_card_no=431***********1111&id_card_type=1&is_never_pay=0&is_overdue=0&mobile=159****0824&name=%B7%B6%CE%AC%CE%AC&res_info=ok&result=0&status=10";
                if (!string.IsNullOrEmpty(interfaceRetrunResult))
                {
                    string result = "0";
                    returnResult = SearchAccountInfoStringToJson(interfaceRetrunResult, out result, out errorMessage);
                    searchResult = result.Equals("0") ? true : false;
                    errorMessage = CodeToErrorInfo(result);
                }
                else
                {
                    LogHelper.LogInfo("LoadAccountInfo:接口调用失败1");
                    searchResult = false;
                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = "接口调用失败";
                    }
                    StringBuilder sb = new StringBuilder();
                    //sb.Append("[");
                    sb.Append("{");
                    sb.Append("\"result\":");
                    sb.Append("\"" + searchResult + "\",");
                    sb.Append("\"res_info\":");
                    sb.Append("\"" + errorMessage + "\"");
                    sb.Append("}");
                    //sb.Append("]");
                    returnResult = sb.ToString();
                }
            }
            catch (Exception ex)
            {
                searchResult = false;
                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = "接口调用失败";
                }
                StringBuilder sb = new StringBuilder();
                //sb.Append("[");
                sb.Append("{");
                sb.Append("\"result\":");
                sb.Append("\"" + searchResult + "\",");
                sb.Append("\"res_info\":");
                sb.Append("\"" + errorMessage + "\"");
                sb.Append("}");
                //sb.Append("]");
                returnResult = sb.ToString();
                LogHelper.LogInfo("LoadAccountInfo:接口调用失败2");
            }
            return returnResult;
        }

        /// <summary>
        /// 转成Json格式的数据
        /// </summary>
        /// <param name="str"></param>
        /// <param name="result"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string SearchAccountInfoStringToJson(string str, out string result, out string info)
        {
            string returnResult = string.Empty;
            result = "0";
            info = string.Empty;
            StringBuilder jsonBuilder = new StringBuilder();
            try
            {
                if (!string.IsNullOrEmpty(str))
                {
                    string[] strlist1 = str.Split('&');

                    if (strlist1.Length > 0)
                    {
                        //jsonBuilder.Append("[");
                        jsonBuilder.Append("{");
                        for (int i = 0; i < strlist1.Length; i++)
                        {
                            string[] strlist2 = strlist1[i].Split('=');
                            if (strlist2.Length == 2)
                            {
                                jsonBuilder.Append("\"").Append("" + strlist2[0].ToString().Trim() + "").Append("\"").Append(":");
                                string value = strlist2[1].ToString().Trim();

                                if (strlist2[0].ToString().Trim().Equals("result"))
                                {
                                    result = strlist2[1].ToString().Trim();
                                }
                                if (strlist2[0].ToString().Trim().Equals("res_info"))
                                {
                                    info = strlist2[1].ToString().Trim();
                                }
                                if (strlist2[0].ToString().Trim().Equals("acct_type"))
                                {
                                    if (strlist2[1].ToString().Trim().Equals("0"))
                                    {
                                        value = "QQ";
                                    }
                                    else if (strlist2[1].ToString().Trim().Equals("1"))
                                    {
                                        value = "微信";
                                    }
                                    else if (strlist2[1].ToString().Trim().Equals("2"))
                                    {
                                        value = "身份证";
                                    }
                                }
                                else if (strlist2[0].ToString().Trim().Equals("cft_status"))
                                {
                                    if (strlist2[1].ToString().Trim().Equals("0"))
                                    {
                                        value = "关闭";
                                    }
                                    else if (strlist2[1].ToString().Trim().Equals("1"))
                                    {
                                        value = "开通";
                                    }
                                }
                                else if (strlist2[0].ToString().Trim().Equals("is_overdue"))
                                {
                                    if (strlist2[1].ToString().Trim().Equals("0"))
                                    {
                                        value = "未逾期";
                                    }
                                    else if (strlist2[1].ToString().Trim().Equals("1"))
                                    {
                                        value = "逾期";
                                    }
                                }
                                else if (strlist2[0].ToString().Trim().Equals("status"))
                                {
                                    if (strlist2[1].ToString().Trim().Equals("0"))
                                    {
                                        value = "未开通";
                                    }
                                    else if (strlist2[1].ToString().Trim().Equals("10"))
                                    {
                                        value = "开户成功，可正常使用";
                                    }
                                    else if (strlist2[1].ToString().Trim().Equals("20"))
                                    {
                                        value = "止付（禁止借款）";
                                    }
                                    else if (strlist2[1].ToString().Trim().Equals("30"))
                                    {
                                        value = "冻结（禁止还款和借款）";
                                    }
                                    else if (strlist2[1].ToString().Trim().Equals("31"))
                                    {
                                        value = "逾期";
                                    }
                                    else if (strlist2[1].ToString().Trim().Equals("40"))
                                    {
                                        value = "销户（用户注销信用付）";
                                    }
                                }
                                else if (strlist2[0].ToString().Trim().Equals("cur_type"))
                                {
                                    if (strlist2[1].ToString().Trim().Equals("0"))
                                    {
                                        value = "RMB";
                                    }
                                }
                                else if (strlist2[0].ToString().Trim().Equals("id_card_type"))
                                {
                                    if (int.Parse(strlist2[1].ToString().Trim()) == 1)
                                    {
                                        value = "身份证";
                                    }
                                }
                                else if (strlist2[0].ToString().Trim().Equals("name"))
                                {
                                    value = UrlDecode(strlist2[1].ToString().Trim());
                                }
                                else if (strlist2[0].ToString().Trim().Equals("credit_line"))
                                {
                                    //用户总授信额度，单位分
                                    value = COMMLIB.CommUtil.FenToYuan(Convert.ToDouble(UrlDecode(strlist2[1].ToString().Trim())));
                                }
                                else if (strlist2[0].ToString().Trim().Equals("credit"))
                                {
                                    //用户可用额度，单位分
                                    value = COMMLIB.CommUtil.FenToYuan(Convert.ToDouble(UrlDecode(strlist2[1].ToString().Trim())));
                                }
                                
                                jsonBuilder.Append("\"").Append("" + value + "").Append("\"").Append(",");
                            }
                        }
                        jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                        jsonBuilder.Append("}");
                        //jsonBuilder.Append("]");
                    }
                    else
                    {
                        returnResult = string.Empty;
                    }
                    returnResult = jsonBuilder.ToString();
                }
                else
                {
                    returnResult = string.Empty;
                }
            }
            catch (Exception ex)
            {
                returnResult = string.Empty;
            }
            return returnResult;
        }

        
        public static string UrlDecode(string dataStr)
        {
            string result = string.Empty;
            try
            {
                result = System.Web.HttpUtility.UrlDecode(dataStr, Encoding.GetEncoding("gbk"));
            }
            catch (Exception ex)
            {
                result = string.Empty;
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
        public string LoadBillList(string accountNo, int accountType, int billStatus, string beginDate, string endDate, string timeStamp, int pageSize, int pageNumber, string order, out bool searchResult, out string errorMessage, ref int total)
        {
            string returnResult = string.Empty;
            errorMessage = string.Empty;
            searchResult = false;
            try
            {
                CreditData creditDataDAL = new CreditData();
                //接口返回格式：list_num=1&nextpage_flg=1&res_info=ok&result=0&row_0=balance%3D0%26bill_amount%3D0%26bill_date%3D20170601%26bill_id%3DB001706e8930b24b2919459990003275%26bill_status%3D40%26end_time%3D20170531%26interest%3D0%26reapy_status%3D1%26repay_date%3D20170610%26start_time%3D20170501
                string interfaceRetrunResult = string.Empty;
                interfaceRetrunResult = creditDataDAL.LoadBillList(accountNo, accountType, billStatus, beginDate, endDate, timeStamp, pageSize, pageNumber, order, out  errorMessage);
                //interfaceRetrunResult = "list_num=6&nextpage_flg=0&res_info=ok&result=0&row_0=bill_id%3D10000000201611022135095050000403%26bill_status%3D%25CA%25D6%25BB%25FA%25B3%25E4%25D6%25B5%26start_time%3D2016-11-02%252021%253A35%253A09%26end_time%3D2016-11-02%252021%253A35%253A09%26bill_date%3D2016-11-02%252021%253A35%253A09%26repay_date%3D2016-11-02%252021%253A35%253A09%26reapy_status%3D%25CA%25D6%25BB%25FA%25B3%25E4%25D6%25B5%26total_amount%3D1000%26balance%3D1000%26interest%3D1000%26overdue_balance%3D1000&row_1=bill_id%3D10000000201611022135095050000403%26bill_status%3D%25CA%25D6%25BB%25FA%25B3%25E4%25D6%25B5%26start_time%3D2016-11-02%252021%253A35%253A09%26end_time%3D2016-11-02%252021%253A35%253A09%26bill_date%3D2016-11-02%252021%253A35%253A09%26repay_date%3D2016-11-02%252021%253A35%253A09%26reapy_status%3D%25CA%25D6%25BB%25FA%25B3%25E4%25D6%25B5%26total_amount%3D1000%26balance%3D1000%26interest%3D1000%26overdue_balance%3D1000&row_2=bill_id%3D10000000201611022135095050000403%26bill_status%3D%25CA%25D6%25BB%25FA%25B3%25E4%25D6%25B5%26start_time%3D2016-11-02%252021%253A35%253A09%26end_time%3D2016-11-02%252021%253A35%253A09%26bill_date%3D2016-11-02%252021%253A35%253A09%26repay_date%3D2016-11-02%252021%253A35%253A09%26reapy_status%3D%25CA%25D6%25BB%25FA%25B3%25E4%25D6%25B5%26total_amount%3D1000%26balance%3D1000%26interest%3D1000%26overdue_balance%3D1000&row_3=bill_id%3D10000000201611022135095050000403%26bill_status%3D%25CA%25D6%25BB%25FA%25B3%25E4%25D6%25B5%26start_time%3D2016-11-02%252021%253A35%253A09%26end_time%3D2016-11-02%252021%253A35%253A09%26bill_date%3D2016-11-02%252021%253A35%253A09%26repay_date%3D2016-11-02%252021%253A35%253A09%26reapy_status%3D%25CA%25D6%25BB%25FA%25B3%25E4%25D6%25B5%26total_amount%3D1000%26balance%3D1000%26interest%3D1000%26overdue_balance%3D1000&row_4=bill_id%3D10000000201611022135095050000403%26bill_status%3D%25CA%25D6%25BB%25FA%25B3%25E4%25D6%25B5%26start_time%3D2016-11-02%252021%253A35%253A09%26end_time%3D2016-11-02%252021%253A35%253A09%26bill_date%3D2016-11-02%252021%253A35%253A09%26repay_date%3D2016-11-02%252021%253A35%253A09%26reapy_status%3D%25CA%25D6%25BB%25FA%25B3%25E4%25D6%25B5%26total_amount%3D1000%26balance%3D1000%26interest%3D1000%26overdue_balance%3D1000";
                if (!string.IsNullOrEmpty(interfaceRetrunResult))
                {
                    returnResult = interfaceRetrunResult;
                    string aaa = string.Empty;
                    string result = "0";
                    returnResult = LoadBillListStringToJson(interfaceRetrunResult, true, 10000,  pageNumber,out result, out errorMessage);
                    searchResult = result.Equals("0") ? true : false;
                    errorMessage = CodeToErrorInfo(result);
                }
                else
                {
                    searchResult = false;
                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = "接口调用失败";
                    }
                    StringBuilder sb = new StringBuilder();
                    //sb.Append("[");
                    sb.Append("{");
                    sb.Append("\"result\":");
                    sb.Append("\"" + searchResult + "\",");
                    sb.Append("\"res_info\":");
                    sb.Append("\"" + errorMessage + "\"");
                    sb.Append("}");
                    //sb.Append("]");
                    returnResult = sb.ToString();
                    LogHelper.LogInfo("LoadBillList:接口调用失败1");
                }
            }
            catch (Exception ex)
            {
                searchResult = false;
                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = "接口调用失败";
                }
                StringBuilder sb = new StringBuilder();
                //sb.Append("[");
                sb.Append("{");
                sb.Append("\"result\":");
                sb.Append("\"" + searchResult + "\",");
                sb.Append("\"res_info\":");
                sb.Append("\"" + errorMessage + "\"");
                sb.Append("}");
                //sb.Append("]");
                returnResult = sb.ToString();
                LogHelper.LogInfo("LoadBillList:接口调用失败2");
            }
            return returnResult;
        }

        /// <summary>
        /// 转成Json格式的数据
        /// </summary>
        /// <param name="str">list_num=1&nextpage_flg=1&res_info=ok&result=0&row_0=balance%3D0%26bill_amount%3D0%26bill_date%3D20170601%26bill_id%3DB001706e8930b24b2919459990003275%26bill_status%3D40%26end_time%3D20170531%26interest%3D0%26reapy_status%3D1%26repay_date%3D20170610%26start_time%3D20170501</param>
        /// <param name="result"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string LoadBillListStringToJson(string str, bool isShowTotal, int total, int pageNumber, out string result, out string info)
        {
            string returnResult = string.Empty;
            result = "0";
            info = string.Empty;
            int list_num = 0;
            bool nextpage_flg = false;
            StringBuilder jsonBuilder = new StringBuilder();
            try
            {
                if (!string.IsNullOrEmpty(str))
                {
                    string[] strlist1 = str.Split('&');

                    if (strlist1.Length > 0)
                    {
                        for (int i = 0; i < strlist1.Length; i++)
                        {
                            string[] strlist2 = strlist1[i].Split('=');
                            if (strlist2.Length == 2)
                            {
                                if (strlist2[0].ToString().Trim().Equals("result"))
                                {
                                    result = strlist2[1].ToString().Trim();
                                }
                                if (strlist2[0].ToString().Trim().Equals("res_info"))
                                {
                                    info = strlist2[1].ToString().Trim();
                                }
                                if (strlist2[0].ToString().Trim().Equals("list_num"))
                                {
                                    list_num = int.Parse(strlist2[1].ToString().Trim());
                                }
                                if (strlist2[0].ToString().Trim().Equals("nextpage_flg"))
                                {
                                    nextpage_flg = strlist2[1].ToString().Trim().Equals("1") ? true : false;
                                }
                            }
                        }

                        jsonBuilder.Append("{ ");
                        jsonBuilder.Append("\"rows\":[ ");
                        
                        for (int i = 0; i < strlist1.Length; i++)
                        {
                            string[] strlist2 = strlist1[i].Split('=');
                            if (strlist2[0].ToString().Trim().Contains("row_"))
                            {
                                //for (int num = 0; num < list_num; num++)
                                //{
                                jsonBuilder.Append("{ ");
                                //jsonBuilder.Append("\"").Append("" + "result" + "").Append("\"").Append(":");
                                //jsonBuilder.Append("\"").Append("" + result + "").Append("\"").Append(",");
                                //jsonBuilder.Append("\"").Append("" + "res_info" + "").Append("\"").Append(":");
                                //jsonBuilder.Append("\"").Append("" + info + "").Append("\"").Append(",");
                                //jsonBuilder.Append("\"").Append("" + "list_num" + "").Append("\"").Append(":");
                                //jsonBuilder.Append("\"").Append("" + list_num + "").Append("\"").Append(",");
                                //jsonBuilder.Append("\"").Append("" + "nextpage_flg" + "").Append("\"").Append(":");
                                //jsonBuilder.Append("\"").Append("" + nextpage_flg + "").Append("\"").Append(",");
                                //string aaa = "row_" + num.ToString();
                                //得到结果:balance%3D0%26bill_amount%3D0%26bill_date%3D20170601%26bill_id%3DB001706e8930b24b2919459990003275%26bill_status%3D40%26end_time%3D20170531%26interest%3D0%26reapy_status%3D1%26repay_date%3D20170610%26start_time%3D20170501
                                //string rowStr = strlist2[1].ToString().Trim().Substring(strlist2[1].ToString().IndexOf(aaa)+1);
                                string rowStrReplace = MyUrlDeCode(MyUrlDeCode(strlist2[1].ToString().Trim(), Encoding.Default), Encoding.Default); // strlist2[1].ToString().Trim().Replace("%3D", "=").Replace("%26", "&").Replace("%20", " ").Replace("%3A", ":").Replace("%2520", " ").Replace("%253A", ":");
                                string[] rowStrReplaceArray = rowStrReplace.Split('&');
                                for (int j = 0; j < rowStrReplaceArray.Length; j++)
                                {
                                    string[] aaa = rowStrReplaceArray[j].Split('=');
                                    string name = aaa[0].ToString().Trim();
                                    string value = (string.IsNullOrEmpty(aaa[1]) ? string.Empty : aaa[1].ToString().Trim());
                                    if (name.Equals("total_amount") || name.Equals("balance") || name.Equals("interest") || name.Equals("overdue_balance"))
                                    {
                                        if (string.IsNullOrEmpty(value))
                                        {
                                            value = "元";
                                        }
                                        else
                                        {
                                            value = COMMLIB.CommUtil.FenToYuan(Convert.ToDouble(UrlDecode(value)));
                                        }
                                        
                                    }

                                    if (j < rowStrReplaceArray.Length - 1)
                                    {
                                        jsonBuilder.Append("\"").Append("" + name + "").Append("\"").Append(":");
                                        jsonBuilder.Append("\"").Append("" + value + "").Append("\"").Append(",");
                                    }
                                    else
                                    {
                                        jsonBuilder.Append("\"").Append("" + name + "").Append("\"").Append(":");
                                        jsonBuilder.Append("\"").Append("" + value + "").Append("\"").Append("");
                                    }
                                }
                                if (i < strlist1.Length - 1)
                                {
                                    jsonBuilder.Append("}, ");
                                }
                                else
                                {
                                    jsonBuilder.Append("} ");
                                }

                                //}
                            }
                        }
                        jsonBuilder.Append("]");
                        jsonBuilder.Append(",");
                        jsonBuilder.Append("\"").Append("" + "page" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + pageNumber + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "result" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + result + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "res_info" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + info + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "list_num" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + list_num + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "nextpage_flg" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + nextpage_flg + "").Append("\"");
                        if (isShowTotal)
                        {
                            jsonBuilder.Append(",");
                            jsonBuilder.Append("\"total\":");
                            jsonBuilder.Append(total);
                        }
                        jsonBuilder.Append("}");
                    }
                    else
                    {
                        returnResult = string.Empty;
                    }
                    returnResult = jsonBuilder.ToString();
                }
                else
                {
                    jsonBuilder.Append("{ ");
                    jsonBuilder.Append("\"rows\":[ ");
                    jsonBuilder.Append("]");
                    if (isShowTotal)
                    {
                        jsonBuilder.Append(",");
                        jsonBuilder.Append("\"total\":");
                        jsonBuilder.Append(total);
                    }
                    jsonBuilder.Append("}");
                    return jsonBuilder.ToString();
                }
            }
            catch (Exception ex)
            {
                returnResult = string.Empty;
            }
            return returnResult;
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
        public string LoadBillDetailInfo(string accountNo, int accountType, string billId, string beginDate, string endDate, string timeStamp, int pageSize, int pageNumber, string order, out bool searchResult, out string errorMessage, ref int total)
        {
            string returnResult = string.Empty;
            errorMessage = string.Empty;
            searchResult = false;
            try
            {
                CreditData creditDataDAL = new CreditData();
                string interfaceRetrunResult = string.Empty;
                interfaceRetrunResult = creditDataDAL.LoadBillDetailInfo(accountNo, accountType, billId, beginDate, endDate, timeStamp, pageSize, pageNumber, order, out  errorMessage);
                //interfaceRetrunResult = "list_num=6&nextpage_flg=0&res_info=ok&result=0&list_num=5&row_0=trans_time%3D2016-11-02%252021%253A35%253A09%26trans_type%3D1%26trans_amount%3D1000%26trans_info%3D1000%26trans_status%3D1%26sp_name%3D%25CA%25D6%25BB%25FA%25B3%25E4%25D6%25B5%26trans_flow_id%3D10000000201611022135095050000403%26trans_id%3D10000000201611022135095050000403%26sp_bill_no%3D10000000201611022135095050000403&row_1=trans_time%3D2016-11-02%252021%253A35%253A09%26trans_type%3D1%26trans_amount%3D1000%26trans_info%3D1000%26trans_status%3D1%26sp_name%3D%25CA%25D6%25BB%25FA%25B3%25E4%25D6%25B5%26trans_flow_id%3D10000000201611022135095050000403%26trans_id%3D10000000201611022135095050000403%26sp_bill_no%3D10000000201611022135095050000403&row_2=trans_time%3D2016-11-02%252021%253A35%253A09%26trans_type%3D1%26trans_amount%3D1000%26trans_info%3D1000%26trans_status%3D1%26sp_name%3D%25CA%25D6%25BB%25FA%25B3%25E4%25D6%25B5%26trans_flow_id%3D10000000201611022135095050000403%26trans_id%3D10000000201611022135095050000403%26sp_bill_no%3D10000000201611022135095050000403&row_3=trans_time%3D2016-11-02%252021%253A35%253A09%26trans_type%3D1%26trans_amount%3D1000%26trans_info%3D1000%26trans_status%3D1%26sp_name%3D%25CA%25D6%25BB%25FA%25B3%25E4%25D6%25B5%26trans_flow_id%3D10000000201611022135095050000403%26trans_id%3D10000000201611022135095050000403%26sp_bill_no%3D10000000201611022135095050000403&row_4=trans_time%3D2016-11-02%252021%253A35%253A09%26trans_type%3D1%26trans_amount%3D1000%26trans_info%3D1000%26trans_status%3D1%26sp_name%3D%25CA%25D6%25BB%25FA%25B3%25E4%25D6%25B5%26trans_flow_id%3D10000000201611022135095050000403%26trans_id%3D10000000201611022135095050000403%26sp_bill_no%3D10000000201611022135095050000403";                
                if (!string.IsNullOrEmpty(interfaceRetrunResult))
                {
                    returnResult = interfaceRetrunResult;
                    string aaa = string.Empty;
                    string result = "0";
                    returnResult = LoadBillDetailInfoStringToJson(interfaceRetrunResult, true, 10000,pageNumber, out result, out errorMessage);
                    searchResult = result.Equals("0") ? true : false;
                    errorMessage = CodeToErrorInfo(result);
                }
                else
                {
                    searchResult = false;
                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = "接口调用失败";
                    }
                    StringBuilder sb = new StringBuilder();
                    //sb.Append("[");
                    sb.Append("{");
                    sb.Append("\"result\":");
                    sb.Append("\"" + searchResult + "\",");
                    sb.Append("\"res_info\":");
                    sb.Append("\"" + errorMessage + "\"");
                    sb.Append("}");
                    //sb.Append("]");
                    returnResult = sb.ToString();
                    LogHelper.LogInfo("LoadBillDetailInfo:接口调用失败1");
                }
            }
            catch (Exception ex)
            {
                searchResult = false;
                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = "接口调用失败";
                }
                StringBuilder sb = new StringBuilder();
                //sb.Append("[");
                sb.Append("{");
                sb.Append("\"result\":");
                sb.Append("\"" + searchResult + "\",");
                sb.Append("\"res_info\":");
                sb.Append("\"" + errorMessage + "\"");
                sb.Append("}");
                //sb.Append("]");
                returnResult = sb.ToString();
                LogHelper.LogInfo("LoadBillDetailInfo:接口调用失败2");
            }
            return returnResult;
        }

        /// <summary>
        /// 转成Json格式的数据
        /// </summary>
        /// <param name="str">list_num=1&nextpage_flg=1&res_info=ok&result=0&row_0=balance%3D0%26bill_amount%3D0%26bill_date%3D20170601%26bill_id%3DB001706e8930b24b2919459990003275%26bill_status%3D40%26end_time%3D20170531%26interest%3D0%26reapy_status%3D1%26repay_date%3D20170610%26start_time%3D20170501</param>
        /// <param name="result"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string LoadBillDetailInfoStringToJson(string str, bool isShowTotal, int total,int pageNumber, out string result, out string info)
        {
            string returnResult = string.Empty;
            result = "0";
            info = string.Empty;
            int list_num = 0;
            bool nextpage_flg = false;
            StringBuilder jsonBuilder = new StringBuilder();
            try
            {
                if (!string.IsNullOrEmpty(str))
                {
                    string[] strlist1 = str.Split('&');

                    if (strlist1.Length > 0)
                    {
                        for (int i = 0; i < strlist1.Length; i++)
                        {
                            string[] strlist2 = strlist1[i].Split('=');
                            if (strlist2.Length == 2)
                            {
                                if (strlist2[0].ToString().Trim().Equals("result"))
                                {
                                    result = strlist2[1].ToString().Trim();
                                }
                                if (strlist2[0].ToString().Trim().Equals("res_info"))
                                {
                                    info = strlist2[1].ToString().Trim();
                                }
                                if (strlist2[0].ToString().Trim().Equals("list_num"))
                                {
                                    list_num = int.Parse(strlist2[1].ToString().Trim());
                                }
                                if (strlist2[0].ToString().Trim().Equals("nextpage_flg"))
                                {
                                    nextpage_flg = strlist2[1].ToString().Trim().Equals("1") ? true : false;
                                }
                            }
                        }

                        jsonBuilder.Append("{ ");
                        jsonBuilder.Append("\"rows\":[ ");
                       
                        for (int i = 0; i < strlist1.Length; i++)
                        {
                            string[] strlist2 = strlist1[i].Split('=');
                            if (strlist2[0].ToString().Trim().Contains("row_"))
                            {
                                //for (int num = 0; num < list_num; num++)
                                //{
                                jsonBuilder.Append("{ ");
                                //jsonBuilder.Append("\"").Append("" + "result" + "").Append("\"").Append(":");
                                //jsonBuilder.Append("\"").Append("" + result + "").Append("\"").Append(",");
                                //jsonBuilder.Append("\"").Append("" + "res_info" + "").Append("\"").Append(":");
                                //jsonBuilder.Append("\"").Append("" + info + "").Append("\"").Append(",");
                                //jsonBuilder.Append("\"").Append("" + "list_num" + "").Append("\"").Append(":");
                                //jsonBuilder.Append("\"").Append("" + list_num + "").Append("\"").Append(",");
                                //jsonBuilder.Append("\"").Append("" + "nextpage_flg" + "").Append("\"").Append(":");
                                //jsonBuilder.Append("\"").Append("" + nextpage_flg + "").Append("\"").Append(",");
                                //得到结果:balance%3D0%26bill_amount%3D0%26bill_date%3D20170601%26bill_id%3DB001706e8930b24b2919459990003275%26bill_status%3D40%26end_time%3D20170531%26interest%3D0%26reapy_status%3D1%26repay_date%3D20170610%26start_time%3D20170501
                                //string rowStr = strlist2[1].ToString().Trim().Substring(strlist2[1].ToString().IndexOf(aaa)+1);
                                string rowStrReplace = MyUrlDeCode(MyUrlDeCode(strlist2[1].ToString().Trim(), Encoding.Default), Encoding.Default); //strlist2[1].ToString().Trim().Replace("%3D", "=").Replace("%26", "&").Replace("%20", " ").Replace("%3A", ":").Replace("%2520", " ").Replace("%253A", ":");
                                string[] rowStrReplaceArray = rowStrReplace.Split('&');
                                for (int j = 0; j < rowStrReplaceArray.Length; j++)
                                {
                                    string[] aaa = rowStrReplaceArray[j].Split('=');
                                    string filedName = aaa[0].ToString().Trim();
                                    string filedValue = (string.IsNullOrEmpty(aaa[1]) ? string.Empty : aaa[1].ToString().Trim());
                                    if (filedName.Equals("trans_amount"))
                                    {
                                        if (string.IsNullOrEmpty(filedValue))
                                        {
                                            filedValue = "元";
                                        }
                                        else
                                        {
                                            filedValue = COMMLIB.CommUtil.FenToYuan(Convert.ToDouble(UrlDecode(filedValue)));
                                        }

                                    }
                                    if (j < rowStrReplaceArray.Length - 1)
                                    {
                                        jsonBuilder.Append("\"").Append("" + filedName + "").Append("\"").Append(":");
                                        jsonBuilder.Append("\"").Append("" + filedValue + "").Append("\"").Append(",");
                                    }
                                    else
                                    {
                                        jsonBuilder.Append("\"").Append("" + filedName + "").Append("\"").Append(":");
                                        jsonBuilder.Append("\"").Append("" + filedValue + "").Append("\"").Append("");
                                    }
                                }
                                if (i < strlist1.Length - 1)
                                {
                                    jsonBuilder.Append("}, ");
                                }
                                else
                                {
                                    jsonBuilder.Append("} ");
                                }
                                //}
                            }
                        }
                        jsonBuilder.Append("]");
                        jsonBuilder.Append(",");
                        jsonBuilder.Append("\"").Append("" + "page" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + pageNumber + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "result" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + result + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "res_info" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + info + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "list_num" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + list_num + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "nextpage_flg" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + nextpage_flg + "").Append("\"");
                        if (isShowTotal)
                        {
                            jsonBuilder.Append(",");
                            jsonBuilder.Append("\"total\":");
                            jsonBuilder.Append(total);
                        }
                        jsonBuilder.Append("}");
                    }
                    else
                    {
                        returnResult = string.Empty;
                    }
                    returnResult = jsonBuilder.ToString();
                }
                else
                {
                    jsonBuilder.Append("{ ");
                    jsonBuilder.Append("\"rows\":[ ");
                    jsonBuilder.Append("]");
                    if (isShowTotal)
                    {
                        jsonBuilder.Append(",");
                        jsonBuilder.Append("\"total\":");
                        jsonBuilder.Append(total);
                    }
                    jsonBuilder.Append("}");
                    return jsonBuilder.ToString();
                }
            }
            catch (Exception ex)
            {
                returnResult = string.Empty;
            }
            return returnResult;
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
        public string LoadPayList(string accountNo, int accountType, int req_type, string trans_id, string beginDate, string endDate, string timeStamp, int pageSize, int pageNumber, string next_row_key, string order, out bool searchResult, out string errorMessage, ref int total)
        {
            string returnResult = string.Empty;
            errorMessage = string.Empty;
            searchResult = false;
            try
            {
                CreditData creditDataDAL = new CreditData();
                string interfaceRetrunResult = creditDataDAL.LoadPayList(accountNo, accountType, req_type, trans_id, beginDate, endDate, timeStamp, pageSize, pageNumber, next_row_key, order, out errorMessage);
                //string interfaceRetrunResult = "list_num=6&next_row_key=&nextpage_flg=0&res_info=ok&result=0&row_0=sp_bill_no%3D%26sp_name%3D%25CA%25D6%25BB%25FA%25B3%25E4%25D6%25B5%26trans_amount%3D1000%26trans_flow_id%3D10000000201611022135095050000403%26trans_id%3D1800000821471611021107430689%26trans_info%3D18600837025%25B1%25B1%25BE%25A9%25B3%25E4%25D6%25B5%26trans_status%3D30%26trans_time%3D2016-11-02%252021%253A35%253A09&row_1=sp_bill_no%3D%26sp_name%3D%25CA%25D6%25BB%25FA%25B3%25E4%25D6%25B5%26trans_amount%3D9998%26trans_flow_id%3D10000000201611031926185050000407%26trans_id%3D1800000821471611031108007341%26trans_info%3D18600837025%25B1%25B1%25BE%25A9%25B3%25E4%25D6%25B5%26trans_status%3D30%26trans_time%3D2016-11-03%252019%253A26%253A18&row_2=sp_bill_no%3D%26sp_name%3D%25CA%25D6%25BB%25FA%25B3%25E4%25D6%25B5%26trans_amount%3D4998%26trans_flow_id%3D10000000201611081057355050000421%26trans_id%3D1800000821471611081270482136%26trans_info%3D18600837025%25B1%25B1%25BE%25A9%25B3%25E4%25D6%25B5%26trans_status%3D30%26trans_time%3D2016-11-08%252010%253A57%253A35&row_3=sp_bill_no%3D%26sp_name%3D%25BE%25A9%25B6%25AB%26trans_amount%3D2500%26trans_flow_id%3D10000000201611112037365050000448%26trans_id%3D1219098701201611112125238935%26trans_info%3D%25BE%25A9%25B6%25AB-44466876861%26trans_status%3D30%26trans_time%3D2016-11-11%252020%253A37%253A36&row_4=sp_bill_no%3D%26sp_name%3D%25CA%25D6%25BB%25FA%25B3%25E4%25D6%25B5%26trans_amount%3D1996%26trans_flow_id%3D10000000201611111733135050000437%26trans_id%3D1800000821471611111109962199%26trans_info%3D18600837025%25B1%25B1%25BE%25A9%25B3%25E4%25D6%25B5%26trans_status%3D30%26trans_time%3D2016-11-11%252017%253A33%253A13&row_5=sp_bill_no%3D%26sp_name%3D%25CA%25D6%25BB%25FA%25B3%25E4%25D6%25B5%26trans_amount%3D1996%26trans_flow_id%3D10000000201611111703505050000427%26trans_id%3D1800000821471611111404133727%26trans_info%3D18600837025%25B1%25B1%25BE%25A9%25B3%25E4%25D6%25B5%26trans_status%3D30%26trans_time%3D2016-11-11%252017%253A03%253A50";
                if (!string.IsNullOrEmpty(interfaceRetrunResult))
                {
                    returnResult = interfaceRetrunResult;
                    string aaa = string.Empty;
                    string result = "0";
                    returnResult = LoadPayListStringToJson(interfaceRetrunResult, true, 10000,pageNumber ,out result, out errorMessage);
                    searchResult = result.Equals("0") ? true : false;
                    errorMessage = CodeToErrorInfo(result);
                }
                else
                {
                    searchResult = false;
                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = "接口调用失败";
                    }
                    StringBuilder sb = new StringBuilder();
                    //sb.Append("[");
                    sb.Append("{");
                    sb.Append("\"result\":");
                    sb.Append("\"" + searchResult + "\",");
                    sb.Append("\"res_info\":");
                    sb.Append("\"" + errorMessage + "\"");
                    sb.Append("}");
                    //sb.Append("]");
                    returnResult = sb.ToString();
                    LogHelper.LogInfo("LoadPayList:接口调用失败1");
                }
            }
            catch (Exception ex)
            {
                searchResult = false;
                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = "接口调用失败";
                }
                StringBuilder sb = new StringBuilder();
                //sb.Append("[");
                sb.Append("{");
                sb.Append("\"result\":");
                sb.Append("\"" + searchResult + "\",");
                sb.Append("\"res_info\":");
                sb.Append("\"" + errorMessage + "\"");
                sb.Append("}");
                //sb.Append("]");
                returnResult = sb.ToString();
                LogHelper.LogInfo("LoadPayList:接口调用失败2");
            }
            return returnResult;
        }

        /// <summary>
        /// 转成Json格式的数据
        /// </summary>
        /// <param name="str">list_num=1&nextpage_flg=1&res_info=ok&result=0&row_0=balance%3D0%26bill_amount%3D0%26bill_date%3D20170601%26bill_id%3DB001706e8930b24b2919459990003275%26bill_status%3D40%26end_time%3D20170531%26interest%3D0%26reapy_status%3D1%26repay_date%3D20170610%26start_time%3D20170501</param>
        /// <param name="result"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string LoadPayListStringToJson(string str, bool isShowTotal, int total,int pageNumber, out string result, out string info)
        {
            string returnResult = string.Empty;
            result = "0";
            info = string.Empty;
            int list_num = 0;
            bool nextpage_flg = false;
            string next_row_key = string.Empty;
            StringBuilder jsonBuilder = new StringBuilder();
            try
            {
                if (!string.IsNullOrEmpty(str))
                {
                    string[] strlist1 = str.Split('&');

                    if (strlist1.Length > 0)
                    {
                        for (int i = 0; i < strlist1.Length; i++)
                        {
                            string[] strlist2 = strlist1[i].Split('=');
                            if (strlist2.Length == 2)
                            {
                                if (strlist2[0].ToString().Trim().Equals("result"))
                                {
                                    result = strlist2[1].ToString().Trim();
                                }
                                else if (strlist2[0].ToString().Trim().Equals("res_info"))
                                {
                                    info = strlist2[1].ToString().Trim();
                                }
                                else if (strlist2[0].ToString().Trim().Equals("list_num"))
                                {
                                    list_num = int.Parse(strlist2[1].ToString().Trim());
                                }
                                else if (strlist2[0].ToString().Trim().Equals("next_row_key"))
                                {
                                    next_row_key = strlist2[1].ToString().Trim();
                                }
                                else if (strlist2[0].ToString().Trim().Equals("nextpage_flg"))
                                {
                                    nextpage_flg = strlist2[1].ToString().Trim().Equals("1") ? true : false;
                                }
                            }
                        }

                        jsonBuilder.Append("{ ");
                        jsonBuilder.Append("\"rows\":[ ");
                        
                        for (int i = 0; i < strlist1.Length; i++)
                        {
                            string[] strlist2 = strlist1[i].Split('=');
                            if (strlist2[0].ToString().Trim().Contains("row_") && !strlist2[0].ToString().Trim().Equals("next_row_key"))
                            {
                                //for (int num = 0; num < list_num; num++)
                                //{
                                jsonBuilder.Append("{ ");
                                //jsonBuilder.Append("\"").Append("" + "result" + "").Append("\"").Append(":");
                                //jsonBuilder.Append("\"").Append("" + result + "").Append("\"").Append(",");
                                //jsonBuilder.Append("\"").Append("" + "res_info" + "").Append("\"").Append(":");
                                //jsonBuilder.Append("\"").Append("" + info + "").Append("\"").Append(",");
                                //jsonBuilder.Append("\"").Append("" + "list_num" + "").Append("\"").Append(":");
                                //jsonBuilder.Append("\"").Append("" + list_num + "").Append("\"").Append(",");
                                //jsonBuilder.Append("\"").Append("" + "nextpage_flg" + "").Append("\"").Append(":");
                                //jsonBuilder.Append("\"").Append("" + nextpage_flg + "").Append("\"").Append(",");
                                //jsonBuilder.Append("\"").Append("" + "next_row_key" + "").Append("\"").Append(":");
                                //jsonBuilder.Append("\"").Append("" + next_row_key + "").Append("\"").Append(",");
                                //得到结果:balance%3D0%26bill_amount%3D0%26bill_date%3D20170601%26bill_id%3DB001706e8930b24b2919459990003275%26bill_status%3D40%26end_time%3D20170531%26interest%3D0%26reapy_status%3D1%26repay_date%3D20170610%26start_time%3D20170501
                                //string rowStr = strlist2[1].ToString().Trim().Substring(strlist2[1].ToString().IndexOf(aaa)+1);
                                //string rowStrReplace = strlist2[1].ToString().Trim().Replace("%253D", "=").Replace("%2526", "&").Replace("%20", " ").Replace("%3A", ":").Replace("%252520", " ").Replace("%25253A",":");
                                string rowStrReplace = MyUrlDeCode(MyUrlDeCode(strlist2[1].ToString().Trim(), Encoding.Default), Encoding.Default); //strlist2[1].ToString().Trim().Replace("%3D", "=").Replace("%26", "&").Replace("%20", " ").Replace("%3A", ":").Replace("%2520", " ").Replace("%253A", ":");
                                string[] rowStrReplaceArray = rowStrReplace.Split('&');
                                for (int j = 0; j < rowStrReplaceArray.Length; j++)
                                {
                                    string[] aaa = rowStrReplaceArray[j].Split('=');
                                    string filedName = aaa[0].ToString().Trim();
                                    string filedValue = (string.IsNullOrEmpty(aaa[1]) ? string.Empty : aaa[1].ToString().Trim());
                                    if (filedName.Equals("trans_amount"))
                                    {
                                        if (string.IsNullOrEmpty(filedValue))
                                        {
                                            filedValue = "元";
                                        }
                                        else
                                        {
                                            filedValue = COMMLIB.CommUtil.FenToYuan(Convert.ToDouble(UrlDecode(filedValue)));
                                        }

                                    }
                                    if (j < rowStrReplaceArray.Length - 1)
                                    {
                                        jsonBuilder.Append("\"").Append("" + filedName + "").Append("\"").Append(":");
                                        jsonBuilder.Append("\"").Append("" + filedValue + "").Append("\"").Append(",");
                                    }
                                    else
                                    {
                                        jsonBuilder.Append("\"").Append("" + filedName + "").Append("\"").Append(":");
                                        jsonBuilder.Append("\"").Append("" + filedValue + "").Append("\"").Append("");
                                    }
                                }
                                if (i < strlist1.Length - 1)
                                {
                                    jsonBuilder.Append("}, ");
                                }
                                else
                                {
                                    jsonBuilder.Append("} ");
                                }
                                //}
                            }
                        }
                        jsonBuilder.Append("]");
                        jsonBuilder.Append(",");
                        jsonBuilder.Append("\"page\":");
                        jsonBuilder.Append(pageNumber).Append(",");
                        jsonBuilder.Append("\"").Append("" + "result" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + result + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "res_info" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + info + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "list_num" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + list_num + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "nextpage_flg" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + nextpage_flg + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "next_row_key" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + next_row_key + "").Append("\"");
                        if (isShowTotal)
                        {
                            jsonBuilder.Append(",");
                            jsonBuilder.Append("\"total\":");
                            jsonBuilder.Append(total);
                        }
                        jsonBuilder.Append("}");
                    }
                    else
                    {
                        returnResult = string.Empty;
                    }
                    returnResult = jsonBuilder.ToString();
                }
                else
                {
                    jsonBuilder.Append("{ ");
                    jsonBuilder.Append("\"rows\":[ ");
                    jsonBuilder.Append("]");
                    if (isShowTotal)
                    {
                        jsonBuilder.Append(",");
                        jsonBuilder.Append("\"total\":");
                        jsonBuilder.Append(total);
                    }
                    jsonBuilder.Append("}");
                    return jsonBuilder.ToString();
                }
            }
            catch (Exception ex)
            {
                returnResult = string.Empty;
            }
            return returnResult;
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
        public string LoadRepayList(string accountNo, int accountType, int req_type, string trans_id, string beginDate, string endDate, string timeStamp, int pageSize, int pageNumber, string next_row_key, string order, out bool searchResult, out string errorMessage, ref int total)
        {
            string returnResult = string.Empty;
            errorMessage = string.Empty;
            searchResult = false;
            try
            {
                CreditData creditDataDAL = new CreditData();
                string interfaceRetrunResult = string.Empty;
                interfaceRetrunResult = creditDataDAL.LoadRepayList(accountNo, accountType, req_type, trans_id, beginDate, endDate, timeStamp, pageSize, pageNumber, next_row_key, order, out errorMessage);
                //interfaceRetrunResult = "list_num=5&next_row_key=&nextpage_flg=0&res_info=ok&result=0&row_0=balance_orig%3D2011%26pay_status%3D4%26rapay_info%3D1%26repay_amount%3D19332%26repay_channel%3D1%26repay_status%3D30%26repay_time%3D2016-11-02%252021%253A33%253A21%26trans_flow_id%3D20000000201611022132045000000371%26trans_id%3D1800001871471611021498281080&row_1=balance_orig%3D%26pay_status%3D3%26rapay_info%3D1%26repay_amount%3D19332%26repay_channel%3D1%26repay_status%3D50%26repay_time%3D0000-00-00%252000%253A00%253A00%26trans_flow_id%3D20000000201611021842305040000357%26trans_id%3D&row_2=balance_orig%3D2011%26pay_status%3D4%26rapay_info%3D3%26repay_amount%3D1000%26repay_channel%3D1%26repay_status%3D30%26repay_time%3D2016-11-03%252006%253A19%253A00%26trans_flow_id%3D20000000201611030618555040000361%26trans_id%3D1800001871471611031268204862&row_3=balance_orig%3D2011%26pay_status%3D4%26rapay_info%3D3%26repay_amount%3D14996%26repay_channel%3D1%26repay_status%3D30%26repay_time%3D2016-11-08%252010%253A58%253A06%26trans_flow_id%3D20000000201611081058025000000372%26trans_id%3D1800001871471611081401487133&row_4=balance_orig%3D2011%26pay_status%3D4%26rapay_info%3D3%26repay_amount%3D1996%26repay_channel%3D1%26repay_status%3D30%26repay_time%3D2016-11-11%252017%253A29%253A01%26trans_flow_id%3D20000000201611111728585020000377%26trans_id%3D1800001871471611111321770006";
                if (!string.IsNullOrEmpty(interfaceRetrunResult))
                {
                    returnResult = interfaceRetrunResult;
                    string aaa = string.Empty;
                    string result = "0";
                    returnResult = LoadRepayDetailStringToJson(interfaceRetrunResult, true, 10000,pageNumber, out result, out errorMessage);
                    searchResult = result.Equals("0") ? true : false;
                    errorMessage = CodeToErrorInfo(result);
                }
                else
                {
                    searchResult = false;
                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = "接口调用失败";
                    }
                    StringBuilder sb = new StringBuilder();
                    //sb.Append("[");
                    sb.Append("{");
                    sb.Append("\"result\":");
                    sb.Append("\"" + searchResult + "\",");
                    sb.Append("\"res_info\":");
                    sb.Append("\"" + errorMessage + "\"");
                    sb.Append("}");
                    //sb.Append("]");
                    returnResult = sb.ToString();
                    LogHelper.LogInfo("LoadRepayList:接口调用失败1");
                }
            }
            catch (Exception ex)
            {
                searchResult = false;
                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = "接口调用失败";
                }
                StringBuilder sb = new StringBuilder();
                //sb.Append("[");
                sb.Append("{");
                sb.Append("\"result\":");
                sb.Append("\"" + searchResult + "\",");
                sb.Append("\"res_info\":");
                sb.Append("\"" + errorMessage + "\"");
                sb.Append("}");
                //sb.Append("]");
                returnResult = sb.ToString();
                LogHelper.LogInfo("LoadRepayList:接口调用失败2");
            }
            return returnResult;
        }

        /// <summary>
        /// 转成Json格式的数据
        /// </summary>
        /// <param name="str">list_num=1&nextpage_flg=1&res_info=ok&result=0&row_0=balance%3D0%26bill_amount%3D0%26bill_date%3D20170601%26bill_id%3DB001706e8930b24b2919459990003275%26bill_status%3D40%26end_time%3D20170531%26interest%3D0%26reapy_status%3D1%26repay_date%3D20170610%26start_time%3D20170501</param>
        /// <param name="result"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string LoadRepayDetailStringToJson(string str, bool isShowTotal, int total, int pageNumber,out string result, out string info)
        {
            string returnResult = string.Empty;
            result = "0";
            info = string.Empty;
            int list_num = 0;
            bool nextpage_flg = false;
            string next_row_key = string.Empty;
            StringBuilder jsonBuilder = new StringBuilder();
            try
            {
                if (!string.IsNullOrEmpty(str))
                {
                    string[] strlist1 = str.Split('&');

                    if (strlist1.Length > 0)
                    {
                        for (int i = 0; i < strlist1.Length; i++)
                        {
                            string[] strlist2 = strlist1[i].Split('=');
                            if (strlist2.Length == 2)
                            {
                                if (strlist2[0].ToString().Trim().Equals("result"))
                                {
                                    result = strlist2[1].ToString().Trim();
                                }
                                if (strlist2[0].ToString().Trim().Equals("res_info"))
                                {
                                    info = strlist2[1].ToString().Trim();
                                }
                                if (strlist2[0].ToString().Trim().Equals("list_num"))
                                {
                                    list_num = int.Parse(strlist2[1].ToString().Trim());
                                }
                                if (strlist2[0].ToString().Trim().Equals("next_row_key"))
                                {
                                    next_row_key = strlist2[1].ToString().Trim();
                                }
                                if (strlist2[0].ToString().Trim().Equals("nextpage_flg"))
                                {
                                    nextpage_flg = strlist2[1].ToString().Trim().Equals("1") ? true : false;
                                }
                            }
                        }

                        jsonBuilder.Append("{ ");
                        jsonBuilder.Append("\"rows\":[ ");
                        
                        for (int i = 0; i < strlist1.Length; i++)
                        {
                            string[] strlist2 = strlist1[i].Split('=');
                            if (strlist2[0].ToString().Trim().Contains("row_") && !strlist2[0].ToString().Trim().Equals("next_row_key"))
                            {
                                //for (int num = 0; num < list_num; num++)
                                //{
                                jsonBuilder.Append("{ ");
                               
                                //得到结果:balance%3D0%26bill_amount%3D0%26bill_date%3D20170601%26bill_id%3DB001706e8930b24b2919459990003275%26bill_status%3D40%26end_time%3D20170531%26interest%3D0%26reapy_status%3D1%26repay_date%3D20170610%26start_time%3D20170501
                                //string rowStr = strlist2[1].ToString().Trim().Substring(strlist2[1].ToString().IndexOf(aaa)+1);
                                string rowStrReplace = MyUrlDeCode(MyUrlDeCode(strlist2[1].ToString().Trim(), Encoding.Default), Encoding.Default); // strlist2[1].ToString().Trim().Replace("%3D", "=").Replace("%26", "&").Replace("%20", " ").Replace("%3A", ":").Replace("%2520", " ").Replace("%253A", ":");
                                string[] rowStrReplaceArray = rowStrReplace.Split('&');
                                for (int j = 0; j < rowStrReplaceArray.Length; j++)
                                {
                                    string[] aaa = rowStrReplaceArray[j].Split('=');
                                    string filedName = aaa[0].ToString().Trim();
                                    string filedValue = (string.IsNullOrEmpty(aaa[1]) ? string.Empty : aaa[1].ToString().Trim());
                                    if (filedName.Equals("balance_orig"))
                                    {
                                        filedValue= TENCENT.OSS.C2C.Finance.BankLib.BankIO.QueryBankName(filedValue);
                                    }
                                    if (filedName.Equals("repay_amount"))
                                    {
                                        if (string.IsNullOrEmpty(filedValue))
                                        {
                                            filedValue = "元";
                                        }
                                        else
                                        {
                                            filedValue = COMMLIB.CommUtil.FenToYuan(Convert.ToDouble(UrlDecode(filedValue)));
                                        }

                                    }
                                    if (j < rowStrReplaceArray.Length - 1)
                                    {
                                        jsonBuilder.Append("\"").Append("" + filedName + "").Append("\"").Append(":");
                                        jsonBuilder.Append("\"").Append("" + filedValue + "").Append("\"").Append(",");
                                    }
                                    else
                                    {
                                        jsonBuilder.Append("\"").Append("" + filedName + "").Append("\"").Append(":");
                                        jsonBuilder.Append("\"").Append("" + filedValue + "").Append("\"").Append("");
                                    }
                                }
                                if (i < strlist1.Length - 1)
                                {
                                    jsonBuilder.Append("}, ");
                                }
                                else
                                {
                                    jsonBuilder.Append("} ");
                                }
                                //}
                            }
                        }
                        jsonBuilder.Append("]");
                        jsonBuilder.Append(",");
                        jsonBuilder.Append("\"page\":");
                        jsonBuilder.Append(pageNumber).Append(",");
                        jsonBuilder.Append("\"").Append("" + "result" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + result + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "res_info" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + info + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "list_num" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + list_num + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "nextpage_flg" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + nextpage_flg + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "next_row_key" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + next_row_key + "").Append("\"");
                        if (isShowTotal)
                        {
                            jsonBuilder.Append(",");
                            jsonBuilder.Append("\"total\":");
                            jsonBuilder.Append(total);
                        }
                        jsonBuilder.Append("}");
                    }
                    else
                    {
                        returnResult = string.Empty;
                    }
                    returnResult = jsonBuilder.ToString();
                }
                else
                {
                    jsonBuilder.Append("{ ");
                    jsonBuilder.Append("\"rows\":[ ");
                    jsonBuilder.Append("]");
                    if (isShowTotal)
                    {
                        jsonBuilder.Append(",");
                        jsonBuilder.Append("\"total\":");
                        jsonBuilder.Append(total);
                    }
                    jsonBuilder.Append("}");
                    return jsonBuilder.ToString();
                }
            }
            catch (Exception ex)
            {
                returnResult = string.Empty;
            }
            return returnResult;
        }
        /// <summary>
        /// 退款明细查询
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
        public string LoadRefundList(string accountNo, int accountType, int req_type, string trans_id, string beginDate, string endDate, string timeStamp, int pageSize, int pageNumber, string next_row_key, string order, out bool searchResult, out string errorMessage, ref int total)
        {
            string returnResult = string.Empty;
            errorMessage = string.Empty;
            searchResult = false;
            try
            {
                CreditData creditDataDAL = new CreditData();
                string interfaceRetrunResult = creditDataDAL.LoadRefundList(accountNo, accountType, req_type, trans_id, beginDate, endDate, timeStamp, pageSize, pageNumber, next_row_key, order, out errorMessage);
                //string interfaceRetrunResult = "list_num=20&next_row_key=20161115_0&nextpage_flg=1&res_info=ok&result=0&row_0=refund_amount%3D3%26refund_flow_id%3D30000000201611041739305040000364%26refund_time%3D20161104%252017%253A39%253A30%26refund_trans_id%3D1900005911471610311497634769%26sp_bill_no%3D%26sp_name%3D%26trans_id%3D20161031335520161031471600000005%26trans_info%3D&row_1=refund_amount%3D1%26refund_flow_id%3D30000000201611011009485010000132%26refund_time%3D20161101%252010%253A09%253A48%26refund_trans_id%3D1900005911471610311497634769%26sp_bill_no%3D%26sp_name%3D%26trans_id%3D107201610310880353236%26trans_info%3D&row_10=refund_amount%3D3%26refund_flow_id%3D30000000201611041739305040000364%26refund_time%3D20161104%252017%253A39%253A30%26refund_trans_id%3D1900005911471610311497634769%26sp_bill_no%3D%26sp_name%3D%26trans_id%3D20161031335520161031471600000005%26trans_info%3D&row_11=refund_amount%3D1%26refund_flow_id%3D30000000201611011009485010000132%26refund_time%3D20161101%252010%253A09%253A48%26refund_trans_id%3D1900005911471610311497634769%26sp_bill_no%3D%26sp_name%3D%26trans_id%3D107201610310880353236%26trans_info%3D&row_12=refund_amount%3D3%26refund_flow_id%3D30000000201611041739305040000364%26refund_time%3D20161104%252017%253A39%253A30%26refund_trans_id%3D1900005911471610311497634769%26sp_bill_no%3D%26sp_name%3D%26trans_id%3D20161031335520161031471600000005%26trans_info%3D&row_13=refund_amount%3D1%26refund_flow_id%3D30000000201611011009485010000132%26refund_time%3D20161101%252010%253A09%253A48%26refund_trans_id%3D1900005911471610311497634769%26sp_bill_no%3D%26sp_name%3D%26trans_id%3D107201610310880353236%26trans_info%3D&row_14=refund_amount%3D3%26refund_flow_id%3D30000000201611041739305040000364%26refund_time%3D20161104%252017%253A39%253A30%26refund_trans_id%3D1900005911471610311497634769%26sp_bill_no%3D%26sp_name%3D%26trans_id%3D20161031335520161031471600000005%26trans_info%3D&row_15=refund_amount%3D1%26refund_flow_id%3D30000000201611011009485010000132%26refund_time%3D20161101%252010%253A09%253A48%26refund_trans_id%3D1900005911471610311497634769%26sp_bill_no%3D%26sp_name%3D%26trans_id%3D107201610310880353236%26trans_info%3D&row_16=refund_amount%3D3%26refund_flow_id%3D30000000201611041739305040000364%26refund_time%3D20161104%252017%253A39%253A30%26refund_trans_id%3D1900005911471610311497634769%26sp_bill_no%3D%26sp_name%3D%26trans_id%3D20161031335520161031471600000005%26trans_info%3D&row_17=refund_amount%3D1%26refund_flow_id%3D30000000201611011009485010000132%26refund_time%3D20161101%252010%253A09%253A48%26refund_trans_id%3D1900005911471610311497634769%26sp_bill_no%3D%26sp_name%3D%26trans_id%3D107201610310880353236%26trans_info%3D&row_18=refund_amount%3D3%26refund_flow_id%3D30000000201611041739305040000364%26refund_time%3D20161104%252017%253A39%253A30%26refund_trans_id%3D1900005911471610311497634769%26sp_bill_no%3D%26sp_name%3D%26trans_id%3D20161031335520161031471600000005%26trans_info%3D&row_19=refund_amount%3D1%26refund_flow_id%3D30000000201611011009485010000132%26refund_time%3D20161101%252010%253A09%253A48%26refund_trans_id%3D1900005911471610311497634769%26sp_bill_no%3D%26sp_name%3D%26trans_id%3D107201610310880353236%26trans_info%3D&row_2=refund_amount%3D3%26refund_flow_id%3D30000000201611041739305040000364%26refund_time%3D20161104%252017%253A39%253A30%26refund_trans_id%3D1900005911471610311497634769%26sp_bill_no%3D%26sp_name%3D%26trans_id%3D20161031335520161031471600000005%26trans_info%3D&row_3=refund_amount%3D1%26refund_flow_id%3D30000000201611011009485010000132%26refund_time%3D20161101%252010%253A09%253A48%26refund_trans_id%3D1900005911471610311497634769%26sp_bill_no%3D%26sp_name%3D%26trans_id%3D107201610310880353236%26trans_info%3D&row_4=refund_amount%3D3%26refund_flow_id%3D30000000201611041739305040000364%26refund_time%3D20161104%252017%253A39%253A30%26refund_trans_id%3D1900005911471610311497634769%26sp_bill_no%3D%26sp_name%3D%26trans_id%3D20161031335520161031471600000005%26trans_info%3D&row_5=refund_amount%3D1%26refund_flow_id%3D30000000201611011009485010000132%26refund_time%3D20161101%252010%253A09%253A48%26refund_trans_id%3D1900005911471610311497634769%26sp_bill_no%3D%26sp_name%3D%26trans_id%3D107201610310880353236%26trans_info%3D&row_6=refund_amount%3D3%26refund_flow_id%3D30000000201611041739305040000364%26refund_time%3D20161104%252017%253A39%253A30%26refund_trans_id%3D1900005911471610311497634769%26sp_bill_no%3D%26sp_name%3D%26trans_id%3D20161031335520161031471600000005%26trans_info%3D&row_7=refund_amount%3D1%26refund_flow_id%3D30000000201611011009485010000132%26refund_time%3D20161101%252010%253A09%253A48%26refund_trans_id%3D1900005911471610311497634769%26sp_bill_no%3D%26sp_name%3D%26trans_id%3D107201610310880353236%26trans_info%3D&row_8=refund_amount%3D3%26refund_flow_id%3D30000000201611041739305040000364%26refund_time%3D20161104%252017%253A39%253A30%26refund_trans_id%3D1900005911471610311497634769%26sp_bill_no%3D%26sp_name%3D%26trans_id%3D20161031335520161031471600000005%26trans_info%3D&row_9=refund_amount%3D1%26refund_flow_id%3D30000000201611011009485010000132%26refund_time%3D20161101%252010%253A09%253A48%26refund_trans_id%3D1900005911471610311497634769%26sp_bill_no%3D%26sp_name%3D%26trans_id%3D107201610310880353236%26trans_info%3D";
                if (!string.IsNullOrEmpty(interfaceRetrunResult))
                {
                    returnResult = interfaceRetrunResult;
                    string aaa = string.Empty;
                    string result = "0";
                    returnResult = LoadRefundListStringToJson(interfaceRetrunResult, true, 10000, pageNumber, out result, out errorMessage);
                    searchResult = result.Equals("0") ? true : false;
                    errorMessage = CodeToErrorInfo(result);
                }
                else
                {
                    searchResult = false;
                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = "接口调用失败";
                    }
                    StringBuilder sb = new StringBuilder();
                    //sb.Append("[");
                    sb.Append("{");
                    sb.Append("\"result\":");
                    sb.Append("\"" + searchResult + "\",");
                    sb.Append("\"res_info\":");
                    sb.Append("\"" + errorMessage + "\"");
                    sb.Append("}");
                    //sb.Append("]");
                    returnResult = sb.ToString();
                    LogHelper.LogInfo("LoadRefundList:接口调用失败1");
                }
            }
            catch (Exception ex)
            {
                searchResult = false;
                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = "接口调用失败";
                }
                StringBuilder sb = new StringBuilder();
                //sb.Append("[");
                sb.Append("{");
                sb.Append("\"result\":");
                sb.Append("\"" + searchResult + "\",");
                sb.Append("\"res_info\":");
                sb.Append("\"" + errorMessage + "\"");
                sb.Append("}");
                //sb.Append("]");
                returnResult = sb.ToString();
                LogHelper.LogInfo("LoadRefundList:接口调用失败2");
            }
            return returnResult;
        }

        /// <summary>
        /// 转成Json格式的数据
        /// </summary>
        /// <param name="str">list_num=1&nextpage_flg=1&res_info=ok&result=0&row_0=balance%3D0%26bill_amount%3D0%26bill_date%3D20170601%26bill_id%3DB001706e8930b24b2919459990003275%26bill_status%3D40%26end_time%3D20170531%26interest%3D0%26reapy_status%3D1%26repay_date%3D20170610%26start_time%3D20170501</param>
        /// <param name="result"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string LoadRefundListStringToJson(string str, bool isShowTotal, int total, int pageNumber, out string result, out string info)
        {
            string returnResult = string.Empty;
            result = "0";
            info = string.Empty;
            int list_num = 0;
            bool nextpage_flg = false;
            string next_row_key = string.Empty;
            StringBuilder jsonBuilder = new StringBuilder();
            try
            {
                if (!string.IsNullOrEmpty(str))
                {
                    string[] strlist1 = str.Split('&');

                    if (strlist1.Length > 0)
                    {
                        for (int i = 0; i < strlist1.Length; i++)
                        {
                            string[] strlist2 = strlist1[i].Split('=');
                            if (strlist2.Length == 2)
                            {
                                if (strlist2[0].ToString().Trim().Equals("result"))
                                {
                                    result = strlist2[1].ToString().Trim();
                                }
                                if (strlist2[0].ToString().Trim().Equals("res_info"))
                                {
                                    info = strlist2[1].ToString().Trim();
                                }
                                if (strlist2[0].ToString().Trim().Equals("list_num"))
                                {
                                    list_num = int.Parse(strlist2[1].ToString().Trim());
                                }
                                if (strlist2[0].ToString().Trim().Equals("next_row_key"))
                                {
                                    next_row_key = strlist2[1].ToString().Trim();
                                }
                                if (strlist2[0].ToString().Trim().Equals("nextpage_flg"))
                                {
                                    nextpage_flg = strlist2[1].ToString().Trim().Equals("1") ? true : false;
                                }
                            }
                        }

                        jsonBuilder.Append("{ ");
                        jsonBuilder.Append("\"rows\":[ ");
                       
                        for (int i = 0; i < strlist1.Length; i++)
                        {
                            string[] strlist2 = strlist1[i].Split('=');
                            if (strlist2[0].ToString().Trim().Contains("row_") && !strlist2[0].ToString().Trim().Equals("next_row_key"))
                            {
                                //for (int num = 0; num < list_num; num++)
                                //{
                                jsonBuilder.Append("{ ");
                                //jsonBuilder.Append("\"").Append("" + "result" + "").Append("\"").Append(":");
                                //jsonBuilder.Append("\"").Append("" + result + "").Append("\"").Append(",");
                                //jsonBuilder.Append("\"").Append("" + "res_info" + "").Append("\"").Append(":");
                                //jsonBuilder.Append("\"").Append("" + info + "").Append("\"").Append(",");
                                //jsonBuilder.Append("\"").Append("" + "list_num" + "").Append("\"").Append(":");
                                //jsonBuilder.Append("\"").Append("" + list_num + "").Append("\"").Append(",");
                                //jsonBuilder.Append("\"").Append("" + "nextpage_flg" + "").Append("\"").Append(":");
                                //jsonBuilder.Append("\"").Append("" + nextpage_flg + "").Append("\"").Append(",");
                                //jsonBuilder.Append("\"").Append("" + "next_row_key" + "").Append("\"").Append(":");
                                //jsonBuilder.Append("\"").Append("" + next_row_key + "").Append("\"").Append(",");
                                //得到结果:balance%3D0%26bill_amount%3D0%26bill_date%3D20170601%26bill_id%3DB001706e8930b24b2919459990003275%26bill_status%3D40%26end_time%3D20170531%26interest%3D0%26reapy_status%3D1%26repay_date%3D20170610%26start_time%3D20170501
                                //string rowStr = strlist2[1].ToString().Trim().Substring(strlist2[1].ToString().IndexOf(aaa)+1);
                                //string rowStrReplace = strlist2[1].ToString().Trim().Replace("%253D", "=").Replace("%2526", "&").Replace("%20", " ").Replace("%3A", ":").Replace("%252520", " ").Replace("%25253A", ":");
                                string rowStrReplace = MyUrlDeCode(MyUrlDeCode(strlist2[1].ToString().Trim(), Encoding.Default), Encoding.Default); // strlist2[1].ToString().Trim().Replace("%3D", "=").Replace("%26", "&").Replace("%20", " ").Replace("%3A", ":").Replace("%2520", " ").Replace("%253A", ":");
                                string[] rowStrReplaceArray = rowStrReplace.Split('&');
                                for (int j = 0; j < rowStrReplaceArray.Length; j++)
                                {
                                    string[] aaa = rowStrReplaceArray[j].Split('=');
                                    string filedName = aaa[0].ToString().Trim();
                                    string filedValue = (string.IsNullOrEmpty(aaa[1]) ? string.Empty : aaa[1].ToString().Trim());
                                    if (filedName.Equals("refund_amount"))
                                    {
                                        if (string.IsNullOrEmpty(filedValue))
                                        {
                                            filedValue = "元";
                                        }
                                        else
                                        {
                                            filedValue = COMMLIB.CommUtil.FenToYuan(Convert.ToDouble(UrlDecode(filedValue)));
                                        }

                                    }
                                    if (j < rowStrReplaceArray.Length - 1)
                                    {
                                        jsonBuilder.Append("\"").Append("" + filedName + "").Append("\"").Append(":");
                                        jsonBuilder.Append("\"").Append("" + filedValue + "").Append("\"").Append(",");
                                    }
                                    else
                                    {
                                        jsonBuilder.Append("\"").Append("" + filedName + "").Append("\"").Append(":");
                                        jsonBuilder.Append("\"").Append("" + filedValue + "").Append("\"").Append("");
                                    }
                                }
                                if (i < strlist1.Length - 1)
                                {
                                    jsonBuilder.Append("}, ");
                                }
                                else
                                {
                                    jsonBuilder.Append("} ");
                                }
                                //}
                            }
                        }
                        jsonBuilder.Append("]");
                        jsonBuilder.Append(",");
                        jsonBuilder.Append("\"page\":");
                        jsonBuilder.Append(pageNumber).Append(",");
                        jsonBuilder.Append("\"").Append("" + "result" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + result + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "res_info" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + info + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "list_num" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + list_num + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "nextpage_flg" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + nextpage_flg + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "next_row_key" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + next_row_key + "").Append("\"");
                        if (isShowTotal)
                        {
                            jsonBuilder.Append(",");
                            jsonBuilder.Append("\"total\":");
                            jsonBuilder.Append(total);
                        }

                        jsonBuilder.Append("}");
                    }
                    else
                    {
                        returnResult = string.Empty;
                    }
                    returnResult = jsonBuilder.ToString();
                }
                else
                {
                    jsonBuilder.Append("{ ");
                    jsonBuilder.Append("\"rows\":[ ");
                    jsonBuilder.Append("]");
                    if (isShowTotal)
                    {
                        jsonBuilder.Append(",");
                        jsonBuilder.Append("\"total\":");
                        jsonBuilder.Append(total);
                    }
                    jsonBuilder.Append("}");
                    return jsonBuilder.ToString();
                }
            }
            catch (Exception ex)
            {
                returnResult = string.Empty;
            }
            return returnResult;
        }


        //public string CreateDataGridColumnModel(string str, bool isShowTotal, int total, int pageindex, decimal tableWidth, out string result, out string info)
        //{
        //    //string returnResult = string.Empty;
        //    //result = "0";
        //    //info = string.Empty;
        //    //int list_num = 0;
        //    //bool nextpage_flg = false;
        //    //string next_row_key = string.Empty;
        //    //if (!string.IsNullOrEmpty(str))
        //    //{
        //    //    string[] strlist1 = str.Split('&');

        //    //    if (strlist1.Length > 0)
        //    //    {                  
        //    //        for (int i = 0; i < strlist1.Length; i++)
        //    //        {
        //    //            string[] strlist2 = strlist1[i].Split('=');
        //    //            if (strlist2.Length == 2)
        //    //            {
        //    //                if (strlist2[0].ToString().Trim().Equals("result"))
        //    //                {
        //    //                    result = strlist2[1].ToString().Trim();
        //    //                }
        //    //                if (strlist2[0].ToString().Trim().Equals("res_info"))
        //    //                {
        //    //                    info = strlist2[1].ToString().Trim();
        //    //                }
        //    //                if (strlist2[0].ToString().Trim().Equals("list_num"))
        //    //                {
        //    //                    list_num = int.Parse(strlist2[1].ToString().Trim());
        //    //                }
        //    //                if (strlist2[0].ToString().Trim().Equals("next_row_key"))
        //    //                {
        //    //                    next_row_key = strlist2[1].ToString().Trim();
        //    //                }
        //    //                if (strlist2[0].ToString().Trim().Equals("nextpage_flg"))
        //    //                {
        //    //                    nextpage_flg = strlist2[1].ToString().Trim().Equals("1") ? true : false;
        //    //                }
        //    //            }
        //    //        }
        //    //    }
        //    //}

        //    //StringBuilder stringBuilder = new StringBuilder("{ ");          
        //    //stringBuilder.Append("'columns':[[");
        //    //stringBuilder.AppendFormat("{{field:'{0}',title:'{1}', halign: 'center', align: 'center' ,width:{2}}},", "refund_time", "退款时间", (tableWidth - 250m) / (dt.Columns.Count - 2));
        //    //stringBuilder.Append("]],data:[{");
        //    //stringBuilder.Append("\"rows\":[ ");
        //    //if (dt != null)
        //    //{
        //    //    for (int i = 0; i < dt.Rows.Count; i++)
        //    //    {
        //    //        stringBuilder.Append("{ ");
        //    //        for (int j = 0; j < dt.Columns.Count; j++)
        //    //        {
        //    //            if (j < dt.Columns.Count - 1)
        //    //            {
        //    //                if (j > 1)
        //    //                {
        //    //                    string text = string.Concat(new string[]
        //    //            {
        //    //                "ShowDetails('",
        //    //                JsonHelper.JsonCharFilter(dt.Rows[i]["ID"].ToString()),
        //    //                "','",
        //    //                dt.Columns[j].ColumnName.ToString().ToLower(),
        //    //                "')"
        //    //            });
        //    //                    stringBuilder.Append(string.Concat(new string[]
        //    //            {
        //    //                "\"",
        //    //                dt.Columns[j].ColumnName.ToString().ToLower(),
        //    //                "\":\"<span onclick=",
        //    //                text,
        //    //                ">",
        //    //                JsonHelper.JsonCharFilter(dt.Rows[i][j].ToString()),
        //    //                "</span>\","
        //    //            }));
        //    //                }
        //    //                else if (dt.Columns[j].ColumnName.Equals("ID"))
        //    //                {
        //    //                    stringBuilder.Append(string.Concat(new string[]
        //    //            {
        //    //                "\"",
        //    //                dt.Columns[j].ColumnName.ToString().ToLower(),
        //    //                "\":\"",
        //    //                JsonHelper.JsonCharFilter(dt.Rows[i][j].ToString()),
        //    //                "\","
        //    //            }));
        //    //                }
        //    //                else if (dt.Columns[j].ColumnName.Equals("Name"))
        //    //                {
        //    //                    string text2 = string.Concat(new string[]
        //    //            {
        //    //                "ShowPageDetails('",
        //    //                JsonHelper.JsonCharFilter(dt.Rows[i]["ID"].ToString()),
        //    //                "','",
        //    //                JsonHelper.JsonCharFilter(dt.Rows[i][j].ToString()),
        //    //                "')"
        //    //            });
        //    //                    stringBuilder.Append(string.Concat(new string[]
        //    //            {
        //    //                "\"",
        //    //                dt.Columns[j].ColumnName.ToString().ToLower(),
        //    //                "\":\"<span onclick=",
        //    //                text2,
        //    //                ">",
        //    //                JsonHelper.JsonCharFilter(dt.Rows[i][j].ToString()),
        //    //                "</span>\","
        //    //            }));
        //    //                }
        //    //            }
        //    //            else if (j == dt.Columns.Count - 1)
        //    //            {
        //    //                string text3 = string.Concat(new string[]
        //    //        {
        //    //            "ShowDetails('",
        //    //            JsonHelper.JsonCharFilter(dt.Rows[i]["ID"].ToString()),
        //    //            "','",
        //    //            dt.Columns[j].ColumnName.ToString().ToLower(),
        //    //            "')"
        //    //        });
        //    //                stringBuilder.Append(string.Concat(new string[]
        //    //        {
        //    //            "\"",
        //    //            dt.Columns[j].ColumnName.ToString().ToLower(),
        //    //            "\":\"<span onclick=",
        //    //            text3,
        //    //            ">",
        //    //            JsonHelper.JsonCharFilter(dt.Rows[i][j].ToString()),
        //    //            "</span>\","
        //    //        }));
        //    //            }
        //    //        }
        //    //        if (i == dt.Rows.Count - 1)
        //    //        {
        //    //            stringBuilder.Append("} ");
        //    //        }
        //    //        else
        //    //        {
        //    //            stringBuilder.Append("}, ");
        //    //        }
        //    //    }
        //    //}
        //    //stringBuilder.Append("]");
        //    //stringBuilder.Append(",");
        //    //stringBuilder.Append("\"total\":");
        //    //stringBuilder.Append(total);
        //    //stringBuilder.Append(",");
        //    //stringBuilder.Append("\"page\":");
        //    //stringBuilder.Append(pageindex);
        //    //stringBuilder.Append("}");
        //    //stringBuilder.Append("]");
        //    //stringBuilder.Append("}");
        //    //return stringBuilder.ToString();
        //}
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
        public string LoadRefundDetail(string accountNo, int accountType, string refund_flow_id, string c_rchg_id, string beginDate, string endDate, string timeStamp, int pageSize, int pageNumber, string order, out bool searchResult, out string errorMessage, ref int total)
        {
            string returnResult = string.Empty;
            errorMessage = string.Empty;
            searchResult = false;
            try
            {
                CreditData creditDataDAL = new CreditData();
                string interfaceRetrunResult = string.Empty;
                interfaceRetrunResult = creditDataDAL.LoadRefundDetail(accountNo, accountType, refund_flow_id, c_rchg_id, beginDate, endDate, timeStamp, pageSize, pageNumber, order, out  errorMessage);
                //interfaceRetrunResult = "refund_time=20161101%2010%3A09%3A48&refund_trans_id=1900005911471610311497634769&res_info=ok&result=0&row_0=balance_go%3D2016%25C4%25EA11%25D4%25C2%25D5%25CB%25B5%25A5%26refund_amount%3D1%26rf_trans_id%3D&sp_bill_no=&sp_name=www.tenpay.com&trans_id=107201610310880353236&trans_info=QPay-Test-Goods&trans_time=2016-10-31%2019%3A48%3A00";
                if (!string.IsNullOrEmpty(interfaceRetrunResult))
                {
                    returnResult = interfaceRetrunResult;
                    string aaa = string.Empty;
                    string result = "0";
                    returnResult = LoadRefundDetailStringToJson(interfaceRetrunResult, true, 1, out result, out errorMessage);
                    searchResult = result.Equals("0") ? true : false;
                    errorMessage = CodeToErrorInfo(result);
                }
                else
                {
                    searchResult = false;
                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = "接口调用失败";
                    }
                    StringBuilder sb = new StringBuilder();
                    //sb.Append("[");
                    sb.Append("{");
                    sb.Append("\"result\":");
                    sb.Append("\"" + searchResult + "\",");
                    sb.Append("\"res_info\":");
                    sb.Append("\"" + errorMessage + "\"");
                    sb.Append("}");
                    //sb.Append("]");
                    returnResult = sb.ToString();
                    LogHelper.LogInfo("LoadRefundDetail:接口调用失败1");
                }
            }
            catch (Exception ex)
            {
                searchResult = false;
                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = "接口调用失败";
                }
                StringBuilder sb = new StringBuilder();
                //sb.Append("[");
                sb.Append("{");
                sb.Append("\"result\":");
                sb.Append("\"" + searchResult + "\",");
                sb.Append("\"res_info\":");
                sb.Append("\"" + errorMessage + "\"");
                sb.Append("}");
                //sb.Append("]");
                returnResult = sb.ToString();
                LogHelper.LogInfo("LoadRefundDetail:接口调用失败2");
            }
            return returnResult;
        }

        /// <summary>
        /// 转成Json格式的数据
        /// </summary>
        /// <param name="str">list_num=1&nextpage_flg=1&res_info=ok&result=0&row_0=balance%3D0%26bill_amount%3D0%26bill_date%3D20170601%26bill_id%3DB001706e8930b24b2919459990003275%26bill_status%3D40%26end_time%3D20170531%26interest%3D0%26reapy_status%3D1%26repay_date%3D20170610%26start_time%3D20170501</param>
        /// <param name="result"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string LoadRefundDetailStringToJson(string str, bool isShowTotal, int total, out string result, out string info)
        {
            string returnResult = string.Empty;
            result = "0";
            info = string.Empty;
            int list_num = 0;
            string trans_time = string.Empty;
            string trans_info = string.Empty;
            string refund_time = string.Empty;
            string sp_bill_no = string.Empty;
            string refund_trans_id = string.Empty;
            string refund_flow_id = string.Empty;
            string trans_id = string.Empty;
            string sp_name = string.Empty;
            StringBuilder jsonBuilder = new StringBuilder();
            try
            {
                if (!string.IsNullOrEmpty(str))
                {
                    string[] strlist1 = str.Split('&');

                    if (strlist1.Length > 0)
                    {
                        jsonBuilder.Append("{ ");
                        jsonBuilder.Append("\"rows\":[ ");

                        for (int i = 0; i < strlist1.Length; i++)
                        {
                            string[] strlist2 = strlist1[i].Split('=');
                            if (strlist2.Length == 2)
                            {
                                var value = MyUrlDeCode(strlist2[1].ToString().Trim(), Encoding.UTF8);
                                if (strlist2[0].ToString().Trim().Equals("result"))
                                {
                                    result = value;
                                }
                                if (strlist2[0].ToString().Trim().Equals("res_info"))
                                {
                                    info = value;
                                }
                                if (strlist2[0].ToString().Trim().Equals("list_num"))
                                {
                                    list_num = int.Parse(value);
                                }
                                if (strlist2[0].ToString().Trim().Equals("trans_time"))
                                {
                                    trans_time = value;
                                }
                                if (strlist2[0].ToString().Trim().Equals("trans_info"))
                                {
                                    trans_info = value;
                                }
                                if (strlist2[0].ToString().Trim().Equals("refund_time"))
                                {
                                    refund_time = value;
                                }
                                if (strlist2[0].ToString().Trim().Equals("sp_bill_no"))
                                {
                                    sp_bill_no = value;
                                }
                                if (strlist2[0].ToString().Trim().Equals("refund_trans_id"))
                                {
                                    refund_trans_id = value;
                                }
                                if (strlist2[0].ToString().Trim().Equals("refund_flow_id"))
                                {
                                    refund_flow_id = value;
                                }
                                if (strlist2[0].ToString().Trim().Equals("trans_id"))
                                {
                                    trans_id = value;
                                }
                                if (strlist2[0].ToString().Trim().Equals("sp_name"))
                                {
                                    sp_name = value;
                                }

                            }
                        }

                        jsonBuilder.Append("{ ");
                        jsonBuilder.Append("\"").Append("" + "result" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + result + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "res_info" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + info + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "list_num" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + list_num + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "trans_time" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + trans_time + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "trans_info" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + trans_info + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "refund_time" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + refund_time + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "sp_bill_no" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + sp_bill_no + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "refund_trans_id" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + refund_trans_id + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "refund_flow_id" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + refund_flow_id + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "trans_id" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + trans_id + "").Append("\"").Append(",");
                        jsonBuilder.Append("\"").Append("" + "sp_name" + "").Append("\"").Append(":");
                        jsonBuilder.Append("\"").Append("" + sp_name + "").Append("\"");
                        jsonBuilder.Append("} ");
                        jsonBuilder.Append("]");

                        if (isShowTotal)
                        {
                            jsonBuilder.Append(",");
                            jsonBuilder.Append("\"total\":");
                            jsonBuilder.Append(total);
                        }
                        jsonBuilder.Append("}");
                    }
                    else
                    {
                        returnResult = string.Empty;
                    }
                    returnResult = jsonBuilder.ToString();
                }
                else
                {
                    jsonBuilder.Append("{ ");
                    jsonBuilder.Append("\"rows\":[ ");
                    jsonBuilder.Append("]");
                    if (isShowTotal)
                    {
                        jsonBuilder.Append(",");
                        jsonBuilder.Append("\"total\":");
                        jsonBuilder.Append(total);
                    }
                    jsonBuilder.Append("}");
                    return jsonBuilder.ToString();
                }
            }
            catch (Exception ex)
            {
                returnResult = string.Empty;
            }
            return returnResult;
        }
        /// <summary>
        /// 退款去向
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
        public string LoadRefundQuXiangInfo(string accountNo, int accountType, string refund_flow_id, string c_rchg_id, string beginDate, string endDate, string timeStamp, int pageSize, int pageNumber, string order, out bool searchResult, out string errorMessage, ref int total)
        {
            string returnResult = string.Empty;
            errorMessage = string.Empty;
            searchResult = false;
            try
            {
                CreditData creditDataDAL = new CreditData();
                string interfaceRetrunResult = string.Empty;
                 interfaceRetrunResult = creditDataDAL.LoadRefundDetail(accountNo, accountType, refund_flow_id, c_rchg_id, beginDate, endDate, timeStamp, pageSize, pageNumber, order, out  errorMessage);
                //interfaceRetrunResult = "refund_time=20161101%2010%3A09%3A48&refund_trans_id=1900005911471610311497634769&res_info=ok&result=0&row_0=balance_go%3D2016%25C4%25EA11%25D4%25C2%25D5%25CB%25B5%25A5%26refund_amount%3D1%26rf_trans_id%3D&sp_bill_no=&sp_name=www.tenpay.com&trans_id=107201610310880353236&trans_info=QPay-Test-Goods&trans_time=2016-10-31%2019%3A48%3A00";
                if (!string.IsNullOrEmpty(interfaceRetrunResult))
                {
                    returnResult = interfaceRetrunResult;
                    string aaa = string.Empty;
                    string result = "0";
                    returnResult = LoadRefundQuXiangInfoStringToJson(interfaceRetrunResult, true, 0, out result, out errorMessage);
                    searchResult = result.Equals("0") ? true : false;
                    errorMessage = CodeToErrorInfo(result);
                }
                else
                {
                    searchResult = false;
                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = "接口调用失败";
                    }

                    StringBuilder sb = new StringBuilder();
                    //sb.Append("[");
                    sb.Append("{");
                    sb.Append("\"result\":");
                    sb.Append("\"" + searchResult + "\",");
                    sb.Append("\"res_info\":");
                    sb.Append("\"" + errorMessage + "\"");
                    sb.Append("}");
                    //sb.Append("]");
                    returnResult = sb.ToString();
                    LogHelper.LogInfo("LoadRefundQuXiangInfo:接口调用失败1");
                }
            }
            catch (Exception ex)
            {
                searchResult = false;
                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = "接口调用失败";
                }
                StringBuilder sb = new StringBuilder();
                //sb.Append("[");
                sb.Append("{");
                sb.Append("\"result\":");
                sb.Append("\"" + searchResult + "\",");
                sb.Append("\"res_info\":");
                sb.Append("\"" + errorMessage + "\"");
                sb.Append("}");
                //sb.Append("]");
                returnResult = sb.ToString();
                LogHelper.LogInfo("LoadRefundQuXiangInfo:接口调用失败2");
            }
            return returnResult;
        }

        /// <summary>
        /// 转成Json格式的数据
        /// </summary>
        /// <param name="str">list_num=1&nextpage_flg=1&res_info=ok&result=0&row_0=balance%3D0%26bill_amount%3D0%26bill_date%3D20170601%26bill_id%3DB001706e8930b24b2919459990003275%26bill_status%3D40%26end_time%3D20170531%26interest%3D0%26reapy_status%3D1%26repay_date%3D20170610%26start_time%3D20170501</param>
        /// <param name="result"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string LoadRefundQuXiangInfoStringToJson(string str, bool isShowTotal, int total, out string result, out string info)
        {
            string returnResult = string.Empty;
            result = "0";
            info = string.Empty;
            int list_num = 0;
            string trans_time = string.Empty;
            string trans_info = string.Empty;
            string refund_time = string.Empty;
            string sp_bill_no = string.Empty;
            string refund_trans_id = string.Empty;
            string refund_flow_id = string.Empty;
            string trans_id = string.Empty;
            string sp_name = string.Empty;
            StringBuilder jsonBuilder = new StringBuilder();
            try
            {
                if (!string.IsNullOrEmpty(str))
                {
                    string[] strlist1 = str.Split('&');

                    if (strlist1.Length > 0)
                    {
                        jsonBuilder.Append("{ ");
                        jsonBuilder.Append("\"rows\":[ ");
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        for (int i = 0; i < strlist1.Length; i++)
                        {
                            string[] strlist2 = strlist1[i].Split('=');
                            if (strlist2.Length == 2)
                            {
                                var name = strlist2[0].ToString().Trim();
                                var value = MyUrlDeCode(strlist2[1].ToString().Trim(), Encoding.UTF8);
                                dic.Add(name, value);
                                if (strlist2[0].ToString().Trim().Equals("result"))
                                {
                                    result = value;
                                }
                                if (strlist2[0].ToString().Trim().Equals("res_info"))
                                {
                                    info = value;
                                }
                                if (strlist2[0].ToString().Trim().Equals("list_num"))
                                {
                                    list_num = int.Parse(value);
                                    //total = list_num;
                                }
                                if (strlist2[0].ToString().Trim().Equals("trans_time"))
                                {
                                    trans_time = value;
                                }
                                if (strlist2[0].ToString().Trim().Equals("trans_info"))
                                {
                                    trans_info = value;
                                }
                                if (strlist2[0].ToString().Trim().Equals("refund_time"))
                                {
                                    refund_time = value;
                                }
                                if (strlist2[0].ToString().Trim().Equals("sp_bill_no"))
                                {
                                    sp_bill_no = value;
                                }
                                if (strlist2[0].ToString().Trim().Equals("refund_trans_id"))
                                {
                                    refund_trans_id = value;
                                }
                                if (strlist2[0].ToString().Trim().Equals("refund_flow_id"))
                                {
                                    refund_flow_id = value;
                                }
                                if (strlist2[0].ToString().Trim().Equals("trans_id"))
                                {
                                    trans_id = value;
                                }
                                if (strlist2[0].ToString().Trim().Equals("sp_name"))
                                {
                                    sp_name = value;
                                }

                            }
                        }
                        //foreach (var item in dic)
                        //{
                        //    if (item.Key.Contains("row_"))
                        //    {
                        //        //{[row_0, balance_go=2016%C4%EA11%D4%C2%D5%CB%B5%A5&refund_amount=1&rf_trans_id=]
                        //        string rowStrReplace = MyUrlDeCode(MyUrlDeCode(item.Value.ToString().Trim(), Encoding.Default), Encoding.Default);
                        //        string[] aaa = rowStrReplace.Split('&');
                        //        for()
                        //        jsonBuilder.Append("{ ");
                        //        jsonBuilder.Append("\"").Append("" + item.Key + "").Append("\"").Append(":");
                        //        jsonBuilder.Append("\"").Append("" + MyUrlDeCode(MyUrlDeCode(item.Value, Encoding.Default), Encoding.Default) + "").Append("\"").Append(",");
                        //        jsonBuilder.Append("} ");
                        //    }
                            
                        //}
                        for (int i = 0; i < strlist1.Length; i++)
                        {
                            string[] strlist2 = strlist1[i].Split('=');
                            if (strlist2[0].ToString().Trim().Contains("row_"))
                            {
                                total += 1;
                                //for (int num = 0; num < list_num; num++)
                                //{
                                jsonBuilder.Append("{ ");
                                jsonBuilder.Append("\"").Append("" + "result" + "").Append("\"").Append(":");
                                jsonBuilder.Append("\"").Append("" + result + "").Append("\"").Append(",");
                                jsonBuilder.Append("\"").Append("" + "res_info" + "").Append("\"").Append(":");
                                jsonBuilder.Append("\"").Append("" + info + "").Append("\"").Append(",");
                                jsonBuilder.Append("\"").Append("" + "list_num" + "").Append("\"").Append(":");
                                jsonBuilder.Append("\"").Append("" + list_num + "").Append("\"").Append(",");
                                jsonBuilder.Append("\"").Append("" + "trans_time" + "").Append("\"").Append(":");
                                jsonBuilder.Append("\"").Append("" + trans_time + "").Append("\"").Append(",");
                                jsonBuilder.Append("\"").Append("" + "trans_info" + "").Append("\"").Append(":");
                                jsonBuilder.Append("\"").Append("" + trans_info + "").Append("\"").Append(",");
                                jsonBuilder.Append("\"").Append("" + "refund_time" + "").Append("\"").Append(":");
                                jsonBuilder.Append("\"").Append("" + refund_time + "").Append("\"").Append(",");
                                jsonBuilder.Append("\"").Append("" + "sp_bill_no" + "").Append("\"").Append(":");
                                jsonBuilder.Append("\"").Append("" + sp_bill_no + "").Append("\"").Append(",");
                                jsonBuilder.Append("\"").Append("" + "refund_trans_id" + "").Append("\"").Append(":");
                                jsonBuilder.Append("\"").Append("" + refund_trans_id + "").Append("\"").Append(",");
                                jsonBuilder.Append("\"").Append("" + "refund_flow_id" + "").Append("\"").Append(":");
                                jsonBuilder.Append("\"").Append("" + refund_flow_id + "").Append("\"").Append(",");
                                jsonBuilder.Append("\"").Append("" + "trans_id" + "").Append("\"").Append(":");
                                jsonBuilder.Append("\"").Append("" + trans_id + "").Append("\"").Append(",");
                                jsonBuilder.Append("\"").Append("" + "sp_name" + "").Append("\"").Append(":");
                                jsonBuilder.Append("\"").Append("" + sp_name + "").Append("\"").Append(",");
                                //得到结果:balance%3D0%26bill_amount%3D0%26bill_date%3D20170601%26bill_id%3DB001706e8930b24b2919459990003275%26bill_status%3D40%26end_time%3D20170531%26interest%3D0%26reapy_status%3D1%26repay_date%3D20170610%26start_time%3D20170501
                                //string rowStr = strlist2[1].ToString().Trim().Substring(strlist2[1].ToString().IndexOf(aaa)+1);
                                //string rowStrReplace = strlist2[1].ToString().Trim().Replace("%253D", "=").Replace("%2526", "&").Replace("%20", " ").Replace("%3A", ":").Replace("%252520", " ").Replace("%25253A", ":");
                                //string rowStrReplace = strlist2[1].ToString().Trim().Replace("%3D", "=").Replace("%26", "&").Replace("%20", " ").Replace("%3A", ":").Replace("%2520", " ").Replace("%253A", ":");
                                string rowStrReplace = MyUrlDeCode(MyUrlDeCode(strlist2[1].ToString().Trim(), Encoding.Default), Encoding.Default);

                                string[] rowStrReplaceArray = rowStrReplace.Split('&');
                                for (int j = 0; j < rowStrReplaceArray.Length; j++)
                                {
                                    string[] aaa = rowStrReplaceArray[j].Split('=');
                                    string filedName = aaa[0].ToString().Trim();
                                    string filedValue = (string.IsNullOrEmpty(aaa[1]) ? string.Empty : aaa[1].ToString().Trim());
                                    if (filedName.Equals("refund_amount"))
                                    {
                                        if (string.IsNullOrEmpty(filedValue))
                                        {
                                            filedValue = "元";
                                        }
                                        else
                                        {
                                            filedValue = COMMLIB.CommUtil.FenToYuan(Convert.ToDouble(UrlDecode(filedValue)));
                                        }

                                    }
                                    if (j < rowStrReplaceArray.Length - 1)
                                    {
                                        jsonBuilder.Append("\"").Append("" + filedName + "").Append("\"").Append(":");
                                        jsonBuilder.Append("\"").Append("" + filedValue + "").Append("\"").Append(",");
                                    }
                                    else
                                    {
                                        jsonBuilder.Append("\"").Append("" + filedName + "").Append("\"").Append(":");
                                        jsonBuilder.Append("\"").Append("" + filedValue + "").Append("\"").Append("");
                                    }
                                }
                                if (i < strlist1.Where(a => a.Contains("row_")).Count() - 1)
                                {
                                    jsonBuilder.Append("}, ");
                                }
                                else
                                {
                                    jsonBuilder.Append("} ");
                                }
                                //}
                            }
                        }
                        jsonBuilder.Append("]");
                        if (isShowTotal)
                        {
                            jsonBuilder.Append(",");
                            jsonBuilder.Append("\"total\":");
                            jsonBuilder.Append(total);
                        }
                        jsonBuilder.Append("}");
                    }
                    else
                    {
                        returnResult = string.Empty;
                    }
                    returnResult = jsonBuilder.ToString();
                }
                else
                {
                    jsonBuilder.Append("{ ");
                    jsonBuilder.Append("\"rows\":[ ");
                    jsonBuilder.Append("]");
                    if (isShowTotal)
                    {
                        jsonBuilder.Append(",");
                        jsonBuilder.Append("\"total\":");
                        jsonBuilder.Append(total);
                    }
                    jsonBuilder.Append("}");
                    return jsonBuilder.ToString();
                }
            }
            catch (Exception ex)
            {
                returnResult = string.Empty;
            }
            return returnResult;
        }

        public string DataTableToJsonForFailReasonReport(string inputStr, int total, bool isShowTotal = true)
        {
            //StringBuilder builder = new StringBuilder();
            //try
            //{
            //    if (string.IsNullOrEmpty(inputStr))
            //    {
            //        builder.Append("{ ");
            //        builder.Append("\"rows\":[ ");
            //        builder.Append("]");
            //        if (isShowTotal)
            //        {
            //            builder.Append(",");
            //            builder.Append("\"total\":");
            //            builder.Append(total);
            //        }
            //        builder.Append("}");
            //    }
            //    else
            //    {
            //        string[] inputStrArray = inputStr.Split('&');

            //        if (inputStrArray.Length > 0)
            //        {
            //            builder.Append("{ ");
            //            builder.Append("\"rows\":[ ");
            //            for (int i = 0; i < inputStrArray.Length;i++ )
            //            {
            //                builder.Append("{ ");
            //                string[] valueSplit = inputStrArray[i].Split('=');
            //                if (valueSplit[0].Equals("list_num") || valueSplit[0].Equals("next_row_key") || valueSplit[0].Equals("nextpage_flg") || valueSplit[0].Equals("res_info") || valueSplit[0].Equals("result"))
            //                {
            //                    builder.Append("\"" + valueSplit[0] + "\":\"" + JsonHelper.JsonCharFilter(valueSplit[1].ToString()) + "\",");
            //                }
            //                else
            //                { 

            //                }
            //                for (int j = 0; j < dt.Columns.Count; j++)
            //                {
            //                    if (j < (dt.Columns.Count - 1))
            //                    {
            //                        builder.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":\"" + JsonHelper.JsonCharFilter(dt.Rows[i][j].ToString()) + "\",");
            //                    }
            //                    else if (j == (dt.Columns.Count - 1))
            //                    {
            //                        builder.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":\"" + JsonHelper.JsonCharFilter(dt.Rows[i][j].ToString()) + "\"");
            //                    }
            //                }
            //                if (i == (inputStrArray.Length - 1))
            //                {
            //                    builder.Append("} ");
            //                }
            //                else
            //                {
            //                    builder.Append("}, ");
            //                }
            //            }                       
            //            builder.Append("]");

            //            if (isShowTotal)
            //            {
            //                builder.Append(",");
            //                builder.Append("\"total\":");
            //                builder.Append(total);
            //            }
            //            builder.Append("}");
            //        }
            //        else
            //        {
            //            builder.Append("{ ");
            //            builder.Append("\"rows\":[ ");
            //            builder.Append("]");
            //            if (isShowTotal)
            //            {
            //                builder.Append(",");
            //                builder.Append("\"total\":");
            //                builder.Append(total);
            //            }
            //            builder.Append("}");
            //        }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    builder.Append("{ ");
            //    builder.Append("\"rows\":[ ");
            //    builder.Append("]");
            //    if (isShowTotal)
            //    {
            //        builder.Append(",");
            //        builder.Append("\"total\":");
            //        builder.Append(total);
            //    }
            //    builder.Append("}");
            //}


            //return builder.ToString();
            return "1";
        }

        public static string MyUrlDeCode(string str, Encoding encoding)
        {
            if (encoding == null)
            {
                Encoding utf8 = Encoding.UTF8;
                //首先用utf-8进行解码                    
                string code = HttpUtility.UrlDecode(str.ToUpper(), utf8);
                //将已经解码的字符再次进行编码.
                string encode = HttpUtility.UrlEncode(code, utf8).ToUpper();
                if (str == encode)
                    encoding = Encoding.UTF8;
                else
                    encoding = Encoding.GetEncoding("gb2312");
            }
            return HttpUtility.UrlDecode(str, encoding);
        }

        /// <summary>
        /// 将接口返回的错误码转为对应的中文
        /// </summary>
        /// <param name="inputCode">接口错误码</param>
        /// <returns></returns>
        public static string CodeToErrorInfo(string inputCode)
        {
            string result = string.Empty;
            try
            {
                if (inputCode.Equals("0"))
                {
                    result = "OK";
                }

                else if (inputCode.Equals("237920001"))
                {
                    result = "输入参数错误";
                }
                else if (inputCode.Equals("237920002"))
                {
                    result = "参数不合法";
                }
                else if (inputCode.Equals("237920010"))
                {
                    result = "用户不在白名单内";
                }
                else if (inputCode.Equals("237920011"))
                {
                    result = "根据渠道无法找到用户";
                }
                else if (inputCode.Equals("237920101"))
                {
                    result = "签名校验失败";
                }
                else if (inputCode.Equals("237920102"))
                {
                    result = "协议版本不支持";
                }
                else if (inputCode.Equals("237920103"))
                {
                    result = "配置错误";
                }
                else if (inputCode.Equals("237920104"))
                {
                    result = "支付单查询错误";
                }
                else if (inputCode.Equals("237920105"))
                {
                    result = "还款单查询错误";
                }
                else if (inputCode.Equals("237920106"))
                {
                    result = "退款单查询错误";
                }
                else if (inputCode.Equals("237920107"))
                {
                    result = "退款单详情查询错误";
                }
                else if (inputCode.Equals("237920108"))
                {
                    result = "查询次数受限";
                }
                else if (inputCode.Equals("10003"))
                {
                    result = "没有权限";
                }
            }
            catch (Exception ex)
            {
                result = string.Empty;
            }
            return result;
        }
    }
}
