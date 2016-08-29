﻿using System;
using System.Text;
using System.Net;
using System.Xml;
using System.IO;
using CFT.Apollo.Logging;
using CFT.CSOMS.DAL.Infrastructure;
using SunLibraryEX;
namespace CFT.CSOMS.DAL.AccountModule
{
   public class AccountData
    {
        /// <summary>
        /// 注销微信支付账户
        /// </summary>   
       /// <param name="oaticket">登录态的oaticket</param>    
       public bool LogOnWxAccount(string accid, string username, string oaticket, string clientip, string reason, out string msg)
       {
           msg = "";
           bool retVal = false;         
           try
           {
               LogHelper.LogInfo(string.Format("注销微信支付账户请求串-curl:http://10.135.7.164:12137/cgi-bin/customunregister?f=xml&appname=wx_tenpay-d<Request><accid>{0}</accid><username>{1}</username><oaticket>{2}</oaticket><clientip>{3}</clientip><reason>{4}</reason></Request>", accid, username, oaticket, clientip, reason));
               string parameterString = string.Format("<Request><accid>{0}</accid><username>{1}</username><oaticket>{2}</oaticket><clientip>{3}</clientip><reason>{4}</reason></Request>", accid, username, oaticket, clientip, reason);         
               var data = Encoding.Default.GetBytes(parameterString);
               var request = (HttpWebRequest)WebRequest.Create(string.Format("http://10.135.7.164:12137/cgi-bin/{0}?f=xml&appname=wx_tenpay", "customunregister"));
               request.Method = "POST";           
               request.ContentType = "text/xml;charset=UTF-8";
               var parameter = request.GetRequestStream();
               parameter.Write(data, 0, data.Length);
               var response = (HttpWebResponse)request.GetResponse();
               var myResponseStream = response.GetResponseStream();
               var myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
               var resultXml = new XmlDocument();
               resultXml.LoadXml(myStreamReader.ReadToEnd());
               myStreamReader.Close();
               myResponseStream.Close();
               LogHelper.LogInfo("注销微信支付账户请求结果："+resultXml.ToString());
               var responseNode = resultXml.SelectSingleNode("Response");              
               if (responseNode.SelectSingleNode("error").SelectSingleNode("code").InnerText.Trim() == "0")
               {
                   if (responseNode.SelectSingleNode("result").SelectSingleNode("retcode").InnerText.Trim() == "0")
                   {
                       retVal = true;
                   }
                   else
                   {
                       retVal = false;
                       msg = responseNode.SelectSingleNode("result").SelectSingleNode("retmsg").InnerText.Trim();
                   }
               }
               else
               {
                   msg=responseNode.SelectSingleNode("error").SelectSingleNode("message").InnerText;
                   retVal=false;
               } 
           }
           catch (Exception e)
           {
               LogHelper.LogInfo("注销微信支付账户,LogOnWxAccount:" + e.ToString());
               retVal = false;
               msg = "接口异常cgi:customunregister："+e.Message;
           }
           return retVal;
       }

       public bool LogOnWxAccountByRelay(string accid, string username, string oaticket, string clientip, string reason, out string msg)
       {
           bool retVal = false;
           var ip = System.Configuration.ConfigurationManager.AppSettings["WebchatHongBaoRelayL5_Ip"] ?? "10.198.132.188"; //10.12.23.14
           var port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["WebchatHongBaoRelayL5_Port"] ?? "22000");
           msg = "";
           #region 参数组合
           string parameterString = string.Format("<Request><accid>{0}</accid><username>{1}</username><oaticket>{2}</oaticket><clientip>{3}</clientip><reason>{4}</reason></Request>", accid, username, oaticket, clientip, reason);
           var req = "wechat_xml_text=" + parameterString;
           #endregion
            LogHelper.LogInfo("注销微信支付账户请求参数：LogOnWxAccountByRelay" + parameterString);
            var relay_result = RelayAccessFactory.RelayInvoke(req, "102585", false, false, ip, port);            
            LogHelper.LogInfo("注销微信支付账户请求Relay的结果" + relay_result);
           #region relay转发  响应结果处理        
           var relay_dic = relay_result.ToDictionary();
           if (relay_dic["result"] != "0")
           {
               throw new Exception("relay转发l5,异常 [" + relay_result + "]");
           }
           var result = System.Web.HttpUtility.UrlDecode(relay_dic["res_info"]);
           var resultXml = new XmlDocument();
           resultXml.LoadXml(result);        
           LogHelper.LogInfo("注销微信支付账户请求结果：" + resultXml.ToString());
           var responseNode = resultXml.SelectSingleNode("Response");
           if (responseNode.SelectSingleNode("error").SelectSingleNode("code").InnerText.Trim() == "0")
           {
               if (responseNode.SelectSingleNode("result").SelectSingleNode("retcode").InnerText.Trim() == "0")
               {
                   retVal = true;
               }
               else
               {
                   retVal = false;
                   msg = responseNode.SelectSingleNode("result").SelectSingleNode("retmsg").InnerText.Trim();
               }
           }
           else
           {
               msg = responseNode.SelectSingleNode("error").SelectSingleNode("message").InnerText;
               retVal = false;
           } 
           #endregion
           return retVal;
       }
    }
}
