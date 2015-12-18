using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Configuration;
using SunLibrary;
using CFT.Apollo.CommunicationFramework;
using TENCENT.OSS.C2C.Finance.Common.CommLib;


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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requstType"></param>
        /// <param name="parameterString"></param>
        /// <param name="resultKeyWord"></param>
        /// <returns></returns>
        protected static string CallWechatAPI(string requstType, string parameterString, string resultKeyWord)
        {
            try
            {
                string result;

                var ip = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("WeChatAppId_Ip", "10.198.132.188");
                var port = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("WeChatAppId_Port", 22000);

                var req = "wechat_xml_text=" + parameterString;
                var relay_result = RelayInvoke(req, requstType, false, false, ip, port);
                var relay_dic = SunLibraryEX.StringEx.ToDictionary(relay_result);
                if (relay_dic["result"] != "0")
                {
                    throw new Exception("relay转发l5,异常 [" + relay_result + "]");
                }
                result = System.Web.HttpUtility.UrlDecode(relay_dic["res_info"]);

                #region 开发时不使用l5 功能,注释上面的代码, 直接访问接口

                //string msg;
                //string api = "";
                //switch (requstType)
                //{
                //    case "101338": api = "/usernametoopenid?f=xml&appname=cft_fahongbao"; break;
                //    case "101339": api = "/usernametoopenid?f=xml&appname=cft_xykhk"; break;
                //    case "101340": api = "/usernametoopenid?f=xml&appname=wx_cft-aa"; break;
                //    case "101341": api = "/openidtoacctid?f=xml&appname=cft_fahongbao"; break;
                //    case "101342": api = "/openidtoacctid?f=xml&appname=wx_cft-aa"; break;
                //    default:
                //        break;
                //}
                //var url = "http://172.27.31.236:8080/wx_tenpay_new/bizhttp";
                //result = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetFromCGIPost(url + api, "", parameterString, out msg); 

                #endregion

                var resultXml = new XmlDocument();
                resultXml.LoadXml(result);
                var responseNode = resultXml.SelectSingleNode("Response");
                var errorCode = responseNode.SelectSingleNode("error").SelectSingleNode("errcode").InnerText;
                var errorMessage = responseNode.SelectSingleNode("error").SelectSingleNode("errmsg").InnerText;

                if (errorCode != "0")
                    throw new Exception(string.Format("用微信号获取{0}异常:{1}", resultKeyWord, errorMessage));

                return responseNode.SelectSingleNode("result").SelectSingleNode(resultKeyWord).InnerText;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("调用微信URL接口异常：{0}", ex.Message));
            }
        }

        /// <summary>
        /// 调relay接口
        /// </summary>
        /// <param name="requestString">请求串，包含接口特性的参数，不包含ver、head等</param>
        /// <param name="serviceCode">request-type</param>
        /// <param name="encrypt">请求串是否加密</param>
        /// <param name="invisible"></param>
        /// <param name="relayIP">ip 不填默认</param>
        /// <param name="relayPort">端口 不填默认</param>
        /// <param name="relayDefaultSPId">sp_id</param>
        /// <returns></returns>
        protected static string RelayInvoke(string requestString, string serviceCode, bool encrypt = false, bool invisible = false, string relayIP = "", int relayPort = 0, string coding = "", string relayDefaultSPId = "")
        {
            LogHelper.LogInfo(relayIP + "  " + relayPort);
            try
            {
                if (encrypt)
                    LogHelper.LogInfo("加密前特性参数串：" + requestString);
                RelayRequest relayReq = new RelayRequest() { RequestString = requestString };
                var relayResponse = RelayHelper.CommunicateWithRelay(relayReq, serviceCode, encrypt, invisible, relayIP, relayPort, relayDefaultSPId);
                if (!string.IsNullOrEmpty(coding))
                {
                    Encoding encoding = Encoding.GetEncoding(coding);
                    return encoding.GetString(relayResponse.ResponseBuffer);
                }
                else
                {
                    return Encoding.Default.GetString(relayResponse.ResponseBuffer);
                }
            }
            catch (Exception err)
            {
                string error = "调用relay服务前失败" + err.Message;
                LogHelper.LogInfo(error);
                throw new Exception(error);
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
                var appId = ConfigurationManager.AppSettings["WeChatAppIdAA"];
                var parameterString = string.Format("<root><appid>{0}</appid><username>{1}</username></root>", appId, wechatName);

                return CallWechatAPI("101340", parameterString, "openid");
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
                var appId = ConfigurationManager.AppSettings["WeChatAppIdHB"];
                var parameterString = string.Format("<root><appid>{0}</appid><username>{1}</username></root>", appId, wechatName);

                return CallWechatAPI("101338", parameterString, "openid");
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
                var appId = ConfigurationManager.AppSettings["WeChatAppIdXYKHK"];
                var parameterString = string.Format("<root><appid>{0}</appid><username>{1}</username></root>", appId, wechatName);

                return CallWechatAPI("101339", parameterString, "openid");
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("通过微信api转换微信号到XYKHKopenid异常:{0}", ex.Message));
            }

        }

        /// <summary>
        /// 通过微信号获取香港钱包openid
        /// </summary>
        /// <param name="wechatName"></param>
        /// <param name="client_ip"></param>
        /// <returns></returns>
        public static string GetFCXGOpenIdFromWeChatName(string wechatName, string client_ip)
        {
            var ip = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("WeChatAppId_Ip", "10.198.132.188");
            var port = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("WeChatAppId_Port", 22000);
            System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();

            try
            {
                var obj = new
                {
                    appid = "wx29c4303a2ae3bf0f",
                    username = wechatName,
                    client_ip = client_ip
                };

                var req = "wechat_xml_text=" + jss.Serialize(obj);
                var relay_result = RelayInvoke(req, "101448", false, false, ip, port);

                var relay_dic = SunLibraryEX.StringEx.ToDictionary(relay_result);
                if (relay_dic["result"] != "0")
                {
                    throw new Exception("relay转发l5,异常 [" + relay_result + "]");
                }

                var result = System.Web.HttpUtility.UrlDecode(relay_dic["res_info"]); //"{\"openid\":\"o6_bmjrPTlm6_2sgVt7hMZOPfL2M\"}";
                var resultDic = jss.DeserializeObject(result) as Dictionary<string, object>;
                if (resultDic == null)
                {
                    throw new Exception("通过微信api转换微信号到香港钱包openid出错" + result);
                }
                return resultDic["openid"] as string;
            }
            catch (Exception ex)
            {
                throw new Exception("通过微信api转换微信号到香港钱包openid 接口返回信息:" + ex.Message);
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
                var appId = ConfigurationManager.AppSettings["WeChatAppIdHB"];
                var parameterString = string.Format("<root><appid>{0}</appid><openid>{1}</openid></root>", appId, openId);

                return CallWechatAPI("101341", parameterString, "outeracctid");
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
                var appId = ConfigurationManager.AppSettings["WeChatAppIdAA"];
                var parameterString = string.Format("<root><appid>{0}</appid><openid>{1}</openid></root>", appId, openId);

                return CallWechatAPI("101342", parameterString, "outeracctid");
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("通过微信api转换openid到财付通帐号acctId异常:{0}", ex.Message));
            }
        }

        /// <summary>
        /// 通过Openid,获取微信号
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="oaticket"></param>
        /// <returns></returns>
        public static string GetUserNameFromOpenid(string openId, string oaticket)
        {
            var msg = "";
            try
            {
                var url = Apollo.Common.Configuration.AppSettings.Get<string>("QueryWXUserNameCGI", "http://10.229.141.17:11903/cgi-bin/wxuser/queryuserinfo") + "?f=json";
                System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                var reqObj = new
                {
                    openid = openId,
                    oaticket = oaticket
                };
                var req = jss.Serialize(reqObj);
                var result = commRes.GetFromCGIPost(url, "", req, out msg); //@"{""result"":{""retcode"":0,""retmsg"":""ok"",""username"":""cat""}}";
                var obj = (jss.DeserializeObject(result) as Dictionary<string, object>)["result"];
                var dic = obj as Dictionary<string, object>;
                if ((int)dic["retcode"] == 0)
                {
                    return (string)dic["username"];
                }
                throw new Exception(result);
            }
            catch (Exception ex)
            {
                LogHelper.LogError("通过Openid,获取微信号 出现异常" + ex.ToString());
                throw new Exception(string.Format("通过Openid,获取微信号 出现异常 {0}", ex.Message));
            }
        }
        /// <summary>
        ///  通过acctid 获取微信号
        /// </summary>
        /// <param name="acctid"></param>
        /// <param name="oaticket"></param>
        /// <returns></returns>
        public static string GetUserNameFromAcctid(string acctid, string oaticket)
        {
            var msg = "";
            try
            {
                var url = Apollo.Common.Configuration.AppSettings.Get<string>("QueryWXUserNameCGI", "http://10.229.141.17:11903/cgi-bin/wxuser/queryuserinfo") + "?f=json";
                System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                var reqObj = new
                {
                    acctid = acctid,
                    oaticket = oaticket          
                };
   
                var req = jss.Serialize(reqObj);
                var result = commRes.GetFromCGIPost(url, "", req, out msg);
                var obj = (jss.DeserializeObject(result) as Dictionary<string, object>)["result"];
                var dic = obj as Dictionary<string, object>;
                if ((int)dic["retcode"] == 0)
                {
                    return (string)dic["username"];
                }
                throw new Exception(result);
            }
            catch (Exception ex)
            {
                LogHelper.LogError("通过acctid 获取微信号 出现异常" + ex.ToString());
                throw new Exception(string.Format("通过acctid 获取微信号 出现异常 {0}", ex.Message));
            }
        }
    }
}
