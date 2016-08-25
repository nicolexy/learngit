using System;
using System.Text;
using System.Net;
using System.Xml;
using System.IO;
using CFT.Apollo.Logging;

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
    }
}
