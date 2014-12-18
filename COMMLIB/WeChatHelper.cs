using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Configuration;

namespace CFT.CSOMS.COMMLIB
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    ///目前三类帐号：
    ///
    /// 微信支付财付通帐号               微信红包财付通帐号                微信AA收款财付通帐号
    ///acctid@wx.tenpay.com              hbopenid@hb.tenpay.com            aaopenid@aa.tenpay.com
    ///
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //微信接口辅助类
    public class WeChatHelper
    {
        public static string CallWechatAPI(string apiUrl, string parameterString, string resultKeyWord)
        {
            try
            {
                var data = Encoding.Default.GetBytes(parameterString);
                var request = (HttpWebRequest)WebRequest.Create(apiUrl);
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
                var responseNode = resultXml.SelectSingleNode("Response");
                var errorCode = responseNode.SelectSingleNode("error").SelectSingleNode("errcode").InnerText;
                var errorMessage = responseNode.SelectSingleNode("error").SelectSingleNode("errmsg").InnerText;

                if (errorCode != "0")
                    throw new Exception(string.Format("用微信号获取{0}异常:{1}", resultKeyWord, errorMessage));

                return responseNode.SelectSingleNode("result").SelectSingleNode(resultKeyWord).InnerText;
            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("调用微信URL接口异常：{0}", ex.Message));
            }
        }

        /// <summary>
        /// 通过微信号获取微信支付财付通帐号
        /// </summary>
        /// <param name="wechatName"></param>
        /// <returns></returns>
        public static string GetUINFromWeChatName(string wechatName)
        {
            try
            {
                var hbOpenId = GetHBOpenIdFromWeChatName(wechatName);

                var uin = string.Format("{0}@wx.tenpay.com", GetAcctIdFromOpenId(hbOpenId));

                return uin;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("通过微信api转换微信号到财付通帐号异常:{0}", ex.Message));
            }

        }

        /// <summary>
        /// 通过微信号获取微信红包财付通帐号
        /// </summary>
        /// <param name="wechatName"></param>
        /// <returns></returns>
        public static string GetHBUINFromWeChatName(string wechatName)
        {
            try
            {
                var hbOpenId = GetHBOpenIdFromWeChatName(wechatName);

                var uin = string.Format("{0}@hb.tenpay.com", hbOpenId);

                return uin;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("通过微信api转换微信号到微信红包财付通帐号异常:{0}", ex.Message));
            }
        }


        /// <summary>
        /// 通过微信号获取微信AA财付通帐号
        /// </summary>
        /// <param name="wechatName"></param>
        /// <returns></returns>
        public static string GetAAUINFromWeChatName(string wechatName)
        {
            try
            {
                var aaOpenId = GetAAOpenIdFromWeChatName(wechatName);

                var uin = string.Format("{0}@aa.tenpay.com", aaOpenId);

                return uin;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("通过微信api转换微信号到微信AA财付通帐号异常:{0}", ex.Message));
            }
        }

        /// <summary>
        /// 通过微信号获取AAopenid
        /// </summary>
        /// <param name="wechatName"></param>
        /// <returns></returns>
        public static string GetAAOpenIdFromWeChatName(string wechatName)
        {
            try
            {
                string ip, appId, api, appName, apiUrl, parameterString;

                try
                {
                    ip = ConfigurationManager.AppSettings["WeChatApiIP"];
                    appName = ConfigurationManager.AppSettings["WeChatAppNameAA"];
                    appId = ConfigurationManager.AppSettings["WeChatAppIdAA"];
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("从配置文件读取微信接口参数异常：{0}", ex.Message));
                }

                api = "usernametoopenid";
                apiUrl = string.Format("{0}/{1}?f=xml&appname={2}", ip, api, appName);
                parameterString = string.Format("<root><appid>{0}</appid><username>{1}</username></root>", appId, wechatName);

                return CallWechatAPI(apiUrl, parameterString, "openid");
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("通过微信api转换微信号到AAopenid异常:{0}", ex.Message));
            }

        }

        /// <summary>
        /// 通过微信号获取红包openid
        /// </summary>
        /// <param name="wechatName"></param>
        /// <returns></returns>
        public static string GetHBOpenIdFromWeChatName(string wechatName)
        {
            try
            {
                string ip, appId, api, appName, apiUrl, parameterString;

                try
                {
                    ip = ConfigurationManager.AppSettings["WeChatApiIP"];
                    appName = ConfigurationManager.AppSettings["WeChatAppNameHB"];
                    appId = ConfigurationManager.AppSettings["WeChatAppIdHB"];
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("从配置文件读取微信接口参数异常：{0}", ex.Message));
                }

                api = "usernametoopenid";
                apiUrl = string.Format("{0}/{1}?f=xml&appname={2}", ip, api, appName);
                parameterString = string.Format("<root><appid>{0}</appid><username>{1}</username></root>", appId, wechatName);

                return CallWechatAPI(apiUrl, parameterString, "openid");
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("通过微信api转换微信号到红包openid异常:{0}", ex.Message));
            }

        }

        /// <summary>
        /// 通过微信号获取微信信用卡还款openid
        /// </summary>
        /// <param name="wechatName"></param>
        /// <returns></returns>
        public static string GetXYKHKOpenIdFromWeChatName(string wechatName)
        {
            try
            {
                string ip, appId, api, appName, apiUrl, parameterString;

                try
                {
                    ip = ConfigurationManager.AppSettings["WeChatApiIP"];
                    appName = ConfigurationManager.AppSettings["WeChatAppNameXYKHK"];
                    appId = ConfigurationManager.AppSettings["WeChatAppIdXYKHK"];
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("从配置文件读取微信接口参数异常：{0}", ex.Message));
                }

                api = "usernametoopenid";
                apiUrl = string.Format("{0}/{1}?f=xml&appname={2}", ip, api, appName);
                parameterString = string.Format("<root><appid>{0}</appid><username>{1}</username></root>", appId, wechatName);

                return CallWechatAPI(apiUrl, parameterString, "openid");
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("通过微信api转换微信号到XYKHKopenid异常:{0}", ex.Message));
            }

        }

        /// <summary>
        /// 通过openid获取财付通帐号acctId
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static string GetAcctIdFromOpenId(string openId)
        {
            try
            {
                string ip, appId, api, appName, apiUrl, parameterString;

                try
                {
                    ip = ConfigurationManager.AppSettings["WeChatApiIP"];
                    appName = ConfigurationManager.AppSettings["WeChatAppNameHB"];
                    appId = ConfigurationManager.AppSettings["WeChatAppIdHB"];
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("从配置文件读取微信接口参数异常：{0}", ex.Message));
                }

                api = "openidtoacctid";
                apiUrl = string.Format("{0}/{1}?f=xml&appname={2}", ip, api, appName);
                parameterString = string.Format("<root><appid>{0}</appid><openid>{1}</openid></root>", appId, openId);

                return CallWechatAPI(apiUrl, parameterString, "outeracctid");
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("通过微信api转换openid到财付通帐号acctId异常:{0}", ex.Message));
            }

        }

        /// <summary>
        /// 通过AA的openid获取财付通帐号acctId
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static string GetAcctIdFromAAOpenId(string openId)
        {
            try
            {
                string ip, appId, api, appName, apiUrl, parameterString;

                try
                {
                    ip = ConfigurationManager.AppSettings["WeChatApiIP"];
                    appName = ConfigurationManager.AppSettings["WeChatAppNameAA"];
                    appId = ConfigurationManager.AppSettings["WeChatAppIdAA"];
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("从配置文件读取微信接口参数异常：{0}", ex.Message));
                }

                api = "openidtoacctid";
                apiUrl = string.Format("{0}/{1}?f=xml&appname={2}", ip, api, appName);
                parameterString = string.Format("<root><appid>{0}</appid><openid>{1}</openid></root>", appId, openId);

                return CallWechatAPI(apiUrl, parameterString, "outeracctid");
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("通过微信api转换openid到财付通帐号acctId异常:{0}", ex.Message));
            }

        }

    }
}
