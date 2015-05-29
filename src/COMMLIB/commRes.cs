using System;
using System.Configuration;
using TENCENT.OSS.CFT.KF.DataAccess;
using System.Data;
using System.Collections;
using System.Web;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using TENCENT.OSS.C2C.Finance.Common;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using System.Threading;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF;
using System.Web.Mail;


using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Xml;
using System.Text.RegularExpressions;
using SunLibrary;
using CommLib;

namespace TENCENT.OSS.C2C.Finance.Common.CommLib
{
    /// <summary>
    /// commMethod 用来处理一些通用的方法。比如发送日志，Web和service都需要使用到的一些公用的方法，函数。
    /// 主要减轻publicRes的压力，减少冗余。
    /// </summary>
    public class commRes
    {
        public static void GetTCPReply(string bufferInString, byte[] bufferIn, string IP, int port, out string Msg, string serviceName, out string iResult, out string token_seq)
        {
            token_seq = "";
            iResult = "9999";
            Msg = "";
            try
            {
                //记录发送包
                log4net.ILog log = log4net.LogManager.GetLogger("commRes.GetTCPReply");
                if (log.IsInfoEnabled)
                    log.Info("发送包如下:参数组合长度=" + bufferInString.Length + "+参数=" + bufferInString);

                TcpClient tcpClient = new TcpClient();
                IPAddress ipAddress = IPAddress.Parse(IP);
                IPEndPoint ipPort = new IPEndPoint(ipAddress, port);
                tcpClient.Connect(ipPort);
                NetworkStream stream = tcpClient.GetStream();
                stream.Write(bufferIn, 0, bufferIn.Length);
                byte[] bufferOut = new byte[1024];
                stream.Read(bufferOut, 0, 1024);
                string answer = Encoding.Default.GetString(bufferOut);
                Hashtable paramsHt = new Hashtable();
                paramsHt = tcpParameters(answer);
                if (paramsHt != null)
                {
                    Msg = (string)paramsHt["res_info"];
                    iResult = (string)paramsHt["result"];
                    token_seq = (string)paramsHt["token_seq"];
                    //记录接收包
                    if (log.IsInfoEnabled)
                        log.Info("返回包如下:result=" + paramsHt["result"] + ",res_info=" + paramsHt["res_info"] + ",token_seq=" + paramsHt["token_seq"]);
                }
            }
            catch (Exception e)
            {
                iResult = "9999";
                log4net.ILog log = log4net.LogManager.GetLogger("commRes.GetTCPReply");
                if (log.IsErrorEnabled)
                    log.Error("serviceName=" + serviceName + ";", e);
                Msg = "调用TCP失败:" + e.ToString();
            }
        }

        public static Hashtable tcpParameters(string answer)
        {
            string validAnswer = Regex.Match(answer, @"result[^\\\0]*").Value;
            Hashtable paramsHt = new Hashtable();

            if (validAnswer != null)
            {
                foreach (String item in Regex.Split(validAnswer, "&", RegexOptions.IgnoreCase))
                {
                    String[] keyValue = Regex.Split(item, "=", RegexOptions.IgnoreCase);
                    if (keyValue.Length == 2)
                    {
                        paramsHt.Add(keyValue[0], keyValue[1]);
                    }
                }
                return paramsHt;
            }
            return paramsHt;
        }

        public static string GetWatchWord(string methodName)
        {
            string keyvalue = "";
            if (methodName != null)
            {
                string wordkey = methodName.ToLower() + "_word";
                if (ConfigurationManager.AppSettings[wordkey] != null)
                {
                    keyvalue = ConfigurationManager.AppSettings[wordkey].Trim();
                }
                else
                {
                    if (ConfigurationManager.AppSettings["ICEPASSWORD"] != null)
                        keyvalue = ConfigurationManager.AppSettings["ICEPASSWORD"].Trim();
                    else
                        return "";
                }
            }
            else
            {
                if (ConfigurationManager.AppSettings["ICEPASSWORD"] != null)
                    keyvalue = ConfigurationManager.AppSettings["ICEPASSWORD"].Trim();
                else
                    return "";
            }

            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(keyvalue, "md5").ToLower();
        }

        public static string ICEEncode(string instr)
        {
            if (instr == null || instr.Trim() == "")
                return instr;
            else
            {
                return instr.Replace("%", "%25").Replace("=", "%3d").Replace("&", "%26");
            }
        }

        public commRes()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        private static string ICEServerIP;
        private static int ICEPort;
        private static string ICEServerIP_ia;
        private static int ICEPort_ia;
        private static string iceUsr;
        private static string icePwd;

        static commRes()
        {
            try
            {
                ICEServerIP = ConfigurationManager.AppSettings["ICEServerIP"].Trim();
                ICEPort = Int32.Parse(ConfigurationManager.AppSettings["ICEPort"].Trim());
                ICEServerIP_ia = ConfigurationManager.AppSettings["ICEServerIP_ia"].Trim();
                ICEPort_ia = Int32.Parse(ConfigurationManager.AppSettings["ICEPort_ia"].Trim());
                iceUsr = ConfigurationManager.AppSettings["iceUsr"].ToString();
                icePwd = ConfigurationManager.AppSettings["icePwd"].ToString();
                // furion 20141023在这里初始化ICE客户端 最小代价的话，就是各系统加一个配置项就可以使用，
                // 数据库连接配置化修改版本时，就变成在各项目代码里对ICE客户端进行初始化。
            }
            catch (Exception ex)
            {
                LogHelper.LogInfo("读取配置文件出现错误：" + ex.ToString());
            }

            try
            {
                string dicconnstr = DbConnectionString.Instance.GetConnectionString("RelayConnString");
                InitRelayInfo(dicconnstr);
            }
            catch (Exception ex)
            {
                LogHelper.LogInfo("读取Relay连接字符串发生错误：" + ex.ToString());
            }
        }

        public static void InitRelayInfo(string connstring)
        {
            if (connstring == "")
                return;

            //furion 20141023 这个连接串里包含了relay信息转发表。
            Middle2Relay.InitRelayInfo(connstring);
        }

        /// <summary>
        /// 发送log4日志,同时记录文件日志
        /// </summary>
        /// <param name="modeInfo">发送的模块名称</param>
        /// <param name="Msg">发送的消息</param>
        /// <returns></returns>
        public static bool sendLog4Log(string modeInfo, string Msg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(modeInfo);
            if (log.IsErrorEnabled) log.Error(Msg);

            //commRes.WriteFile("[" + modeInfo + "]" + Msg);
            //commRes.CloseFile();

            return true;
        }


        /// <summary>
        /// 写文件日志
        /// </summary>
        //		public static string filename  = ConfigurationManager.AppSettings["logPath"].ToString() + "帐务日志" +DateTime.Now.ToString("yyyyMMdd") + ".txt";  //每天一个日志   \\"c:\\333.txt";
        //		public static StreamWriter swFromFile = null;
        public static void WriteFile(string strmsg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("Finance_Service.Commlig.WriteFile");
            if (log.IsInfoEnabled)
                log.Info(strmsg);

            //			string newfilename  = ConfigurationManager.AppSettings["logPath"].ToString() + "帐务日志" +DateTime.Now.ToString("yyyyMMdd") + ".txt";  //每天一个日志   \\"c:\\333.txt";
            //			
            //			if(newfilename != filename)
            //			{
            //				if (swFromFile != null)
            //				{
            //					swFromFile.Flush();
            //					swFromFile.Close();
            //					swFromFile = null;
            //				}
            //				filename = newfilename;
            //			}
            //
            //			if (swFromFile == null)
            //			{
            //				FileStream fs1 = new FileStream(filename,FileMode.Append,FileAccess.Write,FileShare.ReadWrite);
            //				swFromFile = new StreamWriter(fs1,System.Text.Encoding.GetEncoding("GB2312"));
            //			}
            //
            //			swFromFile.WriteLine( DateTime.Now.ToString("yyyy-MM-dd HH:mm;ss") + ">>" + strmsg);
        }

        public static void CloseFile()
        {
            //			if (swFromFile != null)
            //			{
            //				swFromFile.Flush();
            //				swFromFile.Close();	
            //			}
            //			swFromFile = null;
        }


        /// <summary>
        /// 去除敏感字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        //public static string replaceMStr(string str)  //对插入数据库的字符串的敏感字符进行替换,放置非法sql注入访问
        public static string replaceSqlStr(string instr)
        {
            //			if(str == null) return null; //furion 20050819
            //
            //			str = str.Replace("\\","");
            //			str = str.Replace("\"","\\\"");
            //			str = str.Replace("'","\\'");  
            //			return str;
            if (instr == null || instr == "")
                return "";

            byte[] outbuff = Encoding.GetEncoding("gb2312").GetBytes(instr);

            for (int i = 0; i < outbuff.Length; i++)
            {
                if (outbuff[i] == 39)  //'
                    outbuff[i] = 34;  //"
                else if (outbuff[i] == 92) //\
                    outbuff[i] = 47; // /
            }

            return Encoding.GetEncoding("gb2312").GetString(outbuff);
        }

        //		public static string GetHexStr(string instr)
        //		{
        //			if(instr == null || instr == "")
        //				return "";
        //
        //			byte[] outbuff = Encoding.GetEncoding("gb2312").GetBytes(instr);
        //
        //			string outstr = "";
        //			for(int i=0 ; i< outbuff.Length ; i++)
        //			{
        //				outstr += Convert.ToString(outbuff[i],16);
        //			}
        //
        //			return outstr.ToUpper();
        //		}

        public static string GetHexStr(string s)
        {
            try
            {
                if (s == null || s == "")
                    return "";

                System.Text.Encoding chs = System.Text.Encoding.GetEncoding("gb2312");
                byte[] bytes = chs.GetBytes(s);
                string str = "";
                for (int i = 0; i < bytes.Length; i++)
                {
                    str += string.Format("{0:X2}", bytes[i]);
                }
                return str;
            }
            catch (Exception ex)
            {

                log4net.ILog log = log4net.LogManager.GetLogger("GetHexStr");
                if (log.IsErrorEnabled)
                    log.Error("转账字符的16进制失败:" + ex.ToString());
                return "";
            }
        }

        public static string GetStrByHexStr(string hexstring)
        {
            System.Text.Encoding encode = System.Text.Encoding.GetEncoding("gb2312");
            byte[] b = new byte[hexstring.Length / 2];
            for (int i = 0; i < hexstring.Length / 2; i++)
            {
                b[i] = Convert.ToByte("0x" + hexstring.Substring(i * 2, 2), 16);
            }
            return encode.GetString(b);

        }

        public static string convertBase64FromUFT8(string str)
        {
            return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(str.Replace("-", "+").Replace("_", "/").Replace("%3D", "=")));
        }

        public static string convertToUTF8Base64(string tmp)
        {
            byte[] by = System.Text.Encoding.UTF8.GetBytes(tmp.ToCharArray());
            return Convert.ToBase64String(by).Replace("=", "%3D").Replace("+", "-").Replace("/", "_");
        }

        public static string replaceHtmlStr(string str)
        {
            if (str == null) return null; //furion 20050819

            str = str.Replace("\"", "＂");
            str = str.Replace("'", "＇");

            str = str.Replace("script", "ｓｃｒｉｐｔ");
            str = str.Replace("<", "〈");
            str = str.Replace(">", "〉");
            return str;
        }

        public static string GetTimeFromTick(string tick)
        {
            if (tick == null || tick.Trim() == "0")
            {
                return "";
            }

            long ltick = 0;
            try
            {
                ltick = long.Parse(tick);
            }
            catch
            {
                return "";
            }

            return DateTime.Parse("1970-01-01 08:00:00").AddTicks(ltick * 10000000).ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string GetTickFromTime(string time)
        {
            if (time == null || time.Trim() == "")
            {
                return "";
            }

            long ltick = 0;
            try
            {
                ltick = DateTime.Parse(time).Ticks;
            }
            catch
            {
                //return "";
                ltick = DateTime.Now.Ticks;
            }

            ltick = ltick - DateTime.Parse("1970-01-01 08:00:00").Ticks;
            ltick = ltick / 10000000;
            return ltick.ToString();
        }

        /// <summary>
        /// 检查DataSet是否有数据
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static bool checkDataSet(DataSet ds, out string Msg)
        {
            Msg = "";
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                Msg = "对不起，数据为空.请检查!";
                return false;
            }

            return true;
        }



        /// <summary>
        /// 向middle发送请求包（分加密和非加密两种）
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="requestMsg"></param>
        /// <param name="secret"></param>
        /// <param name="strReplyInfo"></param>
        /// <param name="iResult">0表示成功；9999表示调用异常错误,其他为ICE接口返回错误</param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public static bool middleInvoke(string serviceName, string requestMsg, bool secret, out string strReplyInfo, out short iResult, out string Msg)
        {
            strReplyInfo = "";
            iResult = 9999;
            Msg = "";
            try
            {
                //furion 记录一下发送包和接收包
                /*
                log4net.ILog log = log4net.LogManager.GetLogger("CommLib.middleInvoke");
                if(log.IsInfoEnabled)
                    log.Info("发送包如下:" + requestMsg);
                */

                TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ICEServerIP.ToString(), serviceName, requestMsg, "KF");

                //furion 20141023 增加relay转发，如果配置值就转发，如果无配置，还走原来调用。
                string routevalue = "";
                if (Middle2Relay.NeedGoRelay(serviceName, requestMsg))
                {
                    if (!Middle2Relay.GetRouteParam(serviceName, requestMsg, out routevalue))
                    {
                        //这里必须返回true，如果false就应该是配置有误。可以报错。
                        Msg = "转换调用relay时出错：" + routevalue;
                        LogHelper.LogInfo(Msg);
                        return false;
                    }
                }
                //20060612 这个函数应该是解码. furion
                //requestMsg = cf.UnEscape(requestMsg);   //编码

                //ex_common_query_service不需要另前缀或加密 furion 20090527
                if (serviceName != "ex_common_query_service" && serviceName != "ui_common_query_service" && serviceName != "order_zwupdate_service"
                    && serviceName != "order_update_service" && serviceName != "order_prerefund_service" && serviceName != "fund_queryacc_service"
                    && serviceName != "deposit_sigleqrycardinfo_service" && serviceName != "card_query_userinfo_service" && serviceName != "card_query_merinfo_service"
                    && serviceName != "card_query_user_service" && serviceName != "ci_common_query_service" && serviceName != "cq_query_tcbanklist_service"
                    && serviceName != "query_order_service" && serviceName != "au_zwset_useratt_service" && serviceName != "au_zwset_prodatt_service"
                    && serviceName != "au_zwset_reqatt_service" && serviceName != "order_history2real_service" && serviceName != "bank_channel_bulletin_service"
                    && serviceName != "query_posbankinfo_service" && serviceName != "bank_pos_single_query_service" && serviceName != "bank_pos_batch_query_service"
                    && serviceName != "waika_bank_pos_single_query_service" && serviceName != "waika_bank_pos_batch_query_service"
                    && serviceName != "zw_prodatt_query_service" && serviceName != "common_query_service")
                {
                    string CFTAcc = ConfigurationManager.AppSettings["CFTAccount"].Trim();

                    if (secret == true)
                    {
                        CFTGWCLib.CFTCryptoClass cf = new CFTGWCLib.CFTCryptoClass();
                        //						string tmp = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(requestMsg,"SHA1");
                        //						requestMsg = requestMsg + "&abstract=" + tmp;
                        requestMsg = cf.EncapsRequest(CFTAcc, requestMsg);      //加密
                        requestMsg = "sp_id=" + CFTAcc + "&request_text=" + ICEEncode(requestMsg);  //组包
                    }
                    else
                    {
                        requestMsg = "sp_id=" + CFTAcc + "&" + requestMsg;
                    }

                }

                //				if (secret == true)
                //					requestMsg = cf.EncapsRequest("2000000000",requestMsg);      //加密
                //			
                //				requestMsg = "sp_id=2000000000&request_text=" + requestMsg;  //组包

                //				BackProcessClient.CIceClientProxyClass cc = new BackProcessClient.CIceClientProxyClass();
                //				iResult = cc.Init("192.168.3.180", 6644, out Msg); //初始化 IP 端口号
                //				iResult = cc.Init(ICEServerIP, ICEPort, out Msg); //初始化 IP 端口号
                //				cc.MiddleInvoke(serviceName,requestMsg,0,5000,iceUsr,icePwd,out strReplyInfo,out iResult,out Msg); 

                //furion 20141023 增加relay转发，如果配置值就转发，如果无配置，还走原来调用。
                if (Middle2Relay.NeedGoRelay(serviceName, requestMsg))
                {
                    if (!Middle2Relay.GoRelay(serviceName, requestMsg, routevalue, out strReplyInfo, out iResult, out Msg))
                    {
                        return false;
                    }

                    return true;

                }

                //ICEReceiveThread irt = new ICEReceiveThread(ICEServerIP,ICEPort,serviceName,requestMsg,0,5000,iceUsr,icePwd);
                ICEReceiveThread irt = new ICEReceiveThread(ICEServerIP, ICEPort, serviceName, requestMsg, 0, 20000, iceUsr, icePwd);

                irt.OpenThread();

                try
                {
                    //for(int i=0; i<50; i++)
                    for (int i = 0; i < 600; i++)
                    {
                        Thread.Sleep(50);
                        if (irt.flag > 0)
                        {
                            //irt.Close();
                            strReplyInfo = irt.strReplyInfo;
                            iResult = (short)irt.iResult;
                            Msg = irt.Msg;
                            /*
                            if(log.IsInfoEnabled)
                                log.Info("返回包如下:" + strReplyInfo);
                                */
                            TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ICEServerIP.ToString(), serviceName, "返回包如下1:" + "[flag=]" + irt.flag + "[msg=]" + irt.Msg + "[replyinfo=]" + strReplyInfo, "KF");
                            return true;
                        }

                        //if(i == 49)
                        if (i == 599)
                        {
                            //irt.Close();
                            throw new LogicException("ICE处理超时！");
                        }
                    }
                }
                finally
                {
                    irt.Close();
                }
                /*
                //furion 记录一下发送包和接收包
                //log4net.ILog log = log4net.LogManager.GetLogger("commRes.middleInvoke");
                if(log.IsInfoEnabled)
                    log.Info("返回包如下:" + strReplyInfo);
                    */
                TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ICEServerIP.ToString(), serviceName, "返回包如下2:" + "[flag=]" + iResult + "[msg=]" + Msg + "[replyinfo=]" + strReplyInfo, "KF");

                return true;
            }
            catch (Exception err)
            {
                /*
                log4net.ILog log = log4net.LogManager.GetLogger("CommLib.middleInvoke");
                if(log.IsErrorEnabled)
                    log.Error("serviceName=" + serviceName + ";",err);  //requestMsg=" + requestMsg + ";"
                    */
                TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ICEServerIP.ToString(), serviceName, err.Message, "KF");

                iResult = 9999;
                Msg = "调用ICE服务前失败" + err.ToString();
                return false;
            }
        }

        public bool NoStaticMiddleInvokeTest(string serviceName, string requestMsg, bool secret, out string strReplyInfo, out short iResult, out string Msg)
        {
            bool index = middleInvoke(serviceName, requestMsg, secret, out strReplyInfo, out iResult, out Msg);
            return index;
        }

        /// <summary>
        /// 向middle发送请求包（分加密和非加密两种）外币系统
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="requestMsg"></param>
        /// <param name="secret"></param>
        /// <param name="strReplyInfo"></param>
        /// <param name="iResult">0表示成功；9999表示调用异常错误,其他为ICE接口返回错误</param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public static bool middleInvokeIA(string serviceName, string requestMsg, bool secret, out string strReplyInfo, out short iResult, out string Msg)
        {
            strReplyInfo = "";
            iResult = 9999;
            Msg = "";
            try
            {
                //furion 记录一下发送包和接收包
                /*
                log4net.ILog log = log4net.LogManager.GetLogger("CommLib.middleInvoke");
                if(log.IsInfoEnabled)
                    log.Info("发送包如下:" + requestMsg);
                */

                TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ICEServerIP_ia.ToString(), serviceName, requestMsg, "KF");

                //20060612 这个函数应该是解码. furion
                //requestMsg = cf.UnEscape(requestMsg);   //编码

                //ex_common_query_service不需要另前缀或加密 furion 20090527
                if (serviceName != "ex_common_query_service" && serviceName != "ui_common_query_service" && serviceName != "order_zwupdate_service"
                    && serviceName != "order_update_service" && serviceName != "order_prerefund_service" && serviceName != "fund_queryacc_service"
                    && serviceName != "deposit_sigleqrycardinfo_service" && serviceName != "card_query_userinfo_service" && serviceName != "card_query_merinfo_service"
                    && serviceName != "card_query_user_service" && serviceName != "ci_common_query_service" && serviceName != "common_query_service")
                {
                    string CFTAcc = ConfigurationManager.AppSettings["CFTAccount"].Trim();

                    if (secret == true)
                    {
                        CFTGWCLib.CFTCryptoClass cf = new CFTGWCLib.CFTCryptoClass();
                        //						string tmp = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(requestMsg,"SHA1");
                        //						requestMsg = requestMsg + "&abstract=" + tmp;
                        requestMsg = cf.EncapsRequest(CFTAcc, requestMsg);      //加密
                        requestMsg = "sp_id=" + CFTAcc + "&request_text=" + ICEEncode(requestMsg);  //组包
                    }
                    else
                    {
                        requestMsg = "sp_id=" + CFTAcc + "&" + requestMsg;
                    }

                }

                //				if (secret == true)
                //					requestMsg = cf.EncapsRequest("2000000000",requestMsg);      //加密
                //			
                //				requestMsg = "sp_id=2000000000&request_text=" + requestMsg;  //组包

                //				BackProcessClient.CIceClientProxyClass cc = new BackProcessClient.CIceClientProxyClass();
                //				iResult = cc.Init("192.168.3.180", 6644, out Msg); //初始化 IP 端口号
                //				iResult = cc.Init(ICEServerIP, ICEPort, out Msg); //初始化 IP 端口号
                //				cc.MiddleInvoke(serviceName,requestMsg,0,5000,iceUsr,icePwd,out strReplyInfo,out iResult,out Msg); 

                //ICEReceiveThread irt = new ICEReceiveThread(ICEServerIP,ICEPort,serviceName,requestMsg,0,5000,iceUsr,icePwd);
                ICEReceiveThread irt = new ICEReceiveThread(ICEServerIP_ia, ICEPort_ia, serviceName, requestMsg, 0, 20000, iceUsr, icePwd);

                irt.OpenThread();

                try
                {
                    //for(int i=0; i<50; i++)
                    for (int i = 0; i < 600; i++)
                    {
                        Thread.Sleep(50);
                        if (irt.flag > 0)
                        {
                            //irt.Close();
                            strReplyInfo = irt.strReplyInfo;
                            iResult = (short)irt.iResult;
                            Msg = irt.Msg;
                            /*
                            if(log.IsInfoEnabled)
                                log.Info("返回包如下:" + strReplyInfo);
                                */
                            TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ICEServerIP_ia.ToString(), serviceName, "返回包如下1:" + "[flag=]" + irt.flag + "[msg=]" + irt.Msg + "[replyinfo=]" + strReplyInfo, "KF");
                            return true;
                        }

                        //if(i == 49)
                        if (i == 599)
                        {
                            //irt.Close();
                            throw new LogicException("ICE处理超时！");
                        }
                    }
                }
                finally
                {
                    irt.Close();
                }
                /*
                //furion 记录一下发送包和接收包
                //log4net.ILog log = log4net.LogManager.GetLogger("commRes.middleInvoke");
                if(log.IsInfoEnabled)
                    log.Info("返回包如下:" + strReplyInfo);
                    */
                TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ICEServerIP_ia.ToString(), serviceName, "返回包如下2:" + "[flag=]" + iResult + "[msg=]" + Msg + "[replyinfo=]" + strReplyInfo, "KF");

                return true;
            }
            catch (Exception err)
            {
                /*
                log4net.ILog log = log4net.LogManager.GetLogger("CommLib.middleInvoke");
                if(log.IsErrorEnabled)
                    log.Error("serviceName=" + serviceName + ";",err);  //requestMsg=" + requestMsg + ";"
                    */
                TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ICEServerIP_ia.ToString(), serviceName, err.Message, "KF");

                iResult = 9999;
                Msg = "调用ICE服务前失败" + err.ToString();
                return false;
            }
        }

        /// <summary>
        /// 向middle发送请求包（字符串不需要加密和转义的）
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="requestMsg"></param>
        /// <param name="secret"></param>
        /// <param name="strReplyInfo"></param>
        /// <param name="iResult">0表示成功；9999表示调用异常错误,其他为ICE接口返回错误</param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public static bool middleInvoke2(string serviceName, string requestMsg, out string strReplyInfo, out short iResult, out string Msg)
        {
            strReplyInfo = "";
            iResult = 9999;
            Msg = "";
            try
            {
                //furion 记录一下发送包和接收包
                log4net.ILog log = log4net.LogManager.GetLogger("commRes.middleInvoke");
                if (log.IsInfoEnabled)
                    log.Info("发送包如下:" + requestMsg);


                string CFTAcc = ConfigurationManager.AppSettings["CFTAccount"].Trim();

                requestMsg = "sp_id=" + CFTAcc + "&" + requestMsg;  //组包

                ICEReceiveThread irt = new ICEReceiveThread(ICEServerIP, ICEPort, serviceName, requestMsg, 0, 20000, iceUsr, icePwd);

                irt.OpenThread();

                try
                {
                    //for(int i=0; i<50; i++)
                    for (int i = 0; i < 600; i++)
                    {
                        Thread.Sleep(50);
                        if (irt.flag > 0)
                        {
                            //irt.Close();
                            strReplyInfo = irt.strReplyInfo;
                            iResult = (short)irt.iResult;
                            Msg = irt.Msg;
                            if (log.IsInfoEnabled)
                                log.Info("返回包如下1:" + "[flag=]" + irt.flag + "[msg=]" + irt.Msg + "[replyinfo=]" + strReplyInfo);
                            return true;
                        }

                        //if(i == 49)
                        if (i == 599)
                        {
                            //irt.Close();
                            throw new LogicException("ICE处理超时！");
                        }
                    }
                }
                finally
                {
                    irt.Close();
                }
                //furion 记录一下发送包和接收包
                //log4net.ILog log = log4net.LogManager.GetLogger("commRes.middleInvoke");
                if (log.IsInfoEnabled)
                    log.Info("返回包如下2:" + "[flag=]" + iResult + "[msg=]" + Msg + "[replyinfo=]" + strReplyInfo);

                return true;
            }
            catch (Exception err)
            {
                log4net.ILog log = log4net.LogManager.GetLogger("commRes.middleInvoke");
                if (log.IsErrorEnabled)
                    log.Error("serviceName=" + serviceName + ";", err);  //requestMsg=" + requestMsg + ";"
                iResult = 9999;
                Msg = "调用ICE服务前失败" + err.ToString();
                return false;
            }
        }
        /// <summary>
        /// 向middle发送请求包（分加密和非加密两种）用于3.0的request_type放在外面的情况
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="requestMsg"></param>
        /// <param name="secret"></param>
        /// <param name="strReplyInfo"></param>
        /// <param name="iResult">0表示成功；9999表示调用异常错误,其他为ICE接口返回错误</param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public static bool middleInvoke_V30(string serviceName, string requestMsg, bool secret, string outrangestr, out string strReplyInfo, out short iResult, out string Msg)
        {
            strReplyInfo = "";
            iResult = 9999;
            Msg = "";
            try
            {
                //furion 记录一下发送包和接收包
                /*
                log4net.ILog log = log4net.LogManager.GetLogger("CommLib.middleInvoke");
                if(log.IsInfoEnabled)
                    log.Info("发送包如下:" + requestMsg);
                    */
                TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ICEServerIP.ToString(), serviceName, requestMsg, "KF");

                CFTGWCLib.CFTCryptoClass cf = new CFTGWCLib.CFTCryptoClass();

                //furion 20141023 增加relay转发，如果配置值就转发，如果无配置，还走原来调用。
                string routevalue = "";
                if (Middle2Relay.NeedGoRelay(serviceName, requestMsg))
                {
                    if (!Middle2Relay.GetRouteParam(serviceName, requestMsg, out routevalue))
                    {
                        //这里必须返回true，如果false就应该是配置有误。可以报错。
                        Msg = "转换调用relay时出错：" + routevalue;
                        return false;
                    }
                }
                //20060612 这个函数应该是解码. furion
                //requestMsg = cf.UnEscape(requestMsg);   //编码

                //ex_common_query_service不需要另前缀或加密 furion 20090527
                if (serviceName != "ex_common_query_service" && serviceName != "ui_common_query_service" && serviceName != "ui_common_update_service"
                    && serviceName != "cq_query_tcbanklist_service" && serviceName != "query_order_service" && serviceName != "bank_pos_single_query_service"
                    && serviceName != "bank_pos_batch_query_service" && serviceName != "waika_bank_pos_single_query_service" && serviceName != "zw_prodatt_query_service")
                {
                    string CFTAcc = ConfigurationManager.AppSettings["CFTAccount"].Trim();

                    if (secret == true)
                        requestMsg = cf.EncapsRequest(CFTAcc, requestMsg);      //加密

                    //furion 20091121 blue的接口不能前接spid,eriken的接口需要前接spid
                    if (outrangestr.IndexOf("sp_id=") > -1)
                        requestMsg = outrangestr + "&request_text=" + ICEEncode(requestMsg);
                    else
                        requestMsg = "sp_id=" + CFTAcc + "&" + outrangestr + "&request_text=" + ICEEncode(requestMsg);  //组包
                }

                //furion 20141023 增加relay转发，如果配置值就转发，如果无配置，还走原来调用。
                if (Middle2Relay.NeedGoRelay(serviceName, requestMsg))
                {
                    if (!Middle2Relay.GoRelay(serviceName, requestMsg, routevalue, out strReplyInfo, out iResult, out Msg))
                    {
                        return false;
                    }
                    return true;
                }
                ICEReceiveThread irt = new ICEReceiveThread(ICEServerIP, ICEPort, serviceName, requestMsg, 0, 20000, iceUsr, icePwd);

                irt.OpenThread();

                try
                {
                    //for(int i=0; i<50; i++)
                    for (int i = 0; i < 600; i++)
                    {
                        Thread.Sleep(50);
                        if (irt.flag > 0)
                        {
                            //irt.Close();
                            strReplyInfo = irt.strReplyInfo;
                            iResult = (short)irt.iResult;
                            Msg = irt.Msg;
                            /*
                            if(log.IsInfoEnabled)
                                log.Info("返回包如下:" + strReplyInfo);
                                */
                            TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ICEServerIP.ToString(), serviceName, "返回包如下:" + strReplyInfo, "KF");
                            return true;
                        }

                        //if(i == 49)
                        if (i == 599)
                        {
                            //irt.Close();
                            throw new LogicException("ICE处理超时！");
                        }
                    }
                }
                finally
                {
                    irt.Close();
                }
                /*
                //furion 记录一下发送包和接收包
                //log4net.ILog log = log4net.LogManager.GetLogger("commRes.middleInvoke");
                if(log.IsInfoEnabled)
                    log.Info("返回包如下:" + strReplyInfo);
                    */
                TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ICEServerIP.ToString(), serviceName, "返回包如下:" + strReplyInfo, "KF");

                return true;
            }
            catch (Exception err)
            {
                /*
                log4net.ILog log = log4net.LogManager.GetLogger("CommLib.middleInvoke");
                if(log.IsErrorEnabled)
                    log.Error("serviceName=" + serviceName + ";",err);  //requestMsg=" + requestMsg + ";"
                iResult = 9999;
                Msg = "调用ICE服务前失败" + err.Message;
                */

                TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ICEServerIP.ToString(), serviceName, "调用ICE服务前失败" + err.ToString(), "KF");
                return false;
            }
        }

        /// <summary>
        /// 向middle发送请求包（分加密和非加密两种）
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="requestMsg"></param>
        /// <param name="secret"></param>
        /// <param name="strReplyInfo"></param>
        /// <param name="iResult">0表示成功；9999表示调用异常错误,其他为ICE接口返回错误</param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public static bool NTBmiddleInvoke(string serviceName, string requestMsg, bool secret, out string strReplyInfo, out short iResult, out string Msg)
        {
            strReplyInfo = "";
            iResult = 9999;
            Msg = "";
            try
            {
                //furion 记录一下发送包和接收包
                log4net.ILog log = log4net.LogManager.GetLogger("CommLib.middleInvoke");
                if (log.IsInfoEnabled)
                    log.Info("发送包如下:" + requestMsg);

                CFTGWCLib.CFTCryptoClass cf = new CFTGWCLib.CFTCryptoClass();

                string CFTAcc = ConfigurationManager.AppSettings["CFTAccount"].Trim();

                if (secret == true)
                    requestMsg = cf.EncapsRequest(CFTAcc, requestMsg);      //加密

                ICEReceiveThread irt = new ICEReceiveThread(ICEServerIP, ICEPort, serviceName, requestMsg, 0, 20000, iceUsr, icePwd);

                irt.OpenThread();

                try
                {
                    //for(int i=0; i<50; i++)
                    for (int i = 0; i < 600; i++)
                    {
                        Thread.Sleep(50);
                        if (irt.flag > 0)
                        {
                            //irt.Close();
                            strReplyInfo = irt.strReplyInfo;
                            iResult = (short)irt.iResult;
                            Msg = irt.Msg;
                            if (log.IsInfoEnabled)
                                log.Info("返回包如下:" + strReplyInfo);
                            return true;
                        }

                        //if(i == 49)
                        if (i == 599)
                        {
                            //irt.Close();
                            throw new LogicException("ICE处理超时！");
                        }
                    }
                }
                finally
                {
                    irt.Close();
                }
                //furion 记录一下发送包和接收包
                //log4net.ILog log = log4net.LogManager.GetLogger("commRes.middleInvoke");
                if (log.IsInfoEnabled)
                    log.Info("返回包如下:" + strReplyInfo);

                return true;
            }
            catch (Exception err)
            {
                log4net.ILog log = log4net.LogManager.GetLogger("commRes.middleInvoke");
                if (log.IsErrorEnabled)
                    log.Error("serviceName=" + serviceName + ";", err);  //requestMsg=" + requestMsg + ";"
                iResult = 9999;
                Msg = "调用ICE服务前失败" + err.ToString();
                return false;
            }
        }

        /// <summary>
        /// 通过ICE调用可执行程序
        /// </summary>
        /// <param name="strCommand"></param>
        /// <param name="iResult">0表示成功;9999表示调用异常错误,其他为ICE接口返回错误.</param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public static bool IceCommand(string strCommand, out short iResult, out string strReplyInfo, out string Msg)
        {
            strReplyInfo = "";
            iResult = 9999;  //9999表示调用异常错误
            Msg = "";
            BackProcessClient.CIceClientProxyClass cc = new BackProcessClient.CIceClientProxyClass();
            try
            {
                CFTGWCLib.CFTCryptoClass cf = new CFTGWCLib.CFTCryptoClass();

                iResult = cc.Init(ICEServerIP, ICEPort, out Msg); //初始化 IP 端口号    测试数据//iResult = cc.Init("192.168.3.180", 6644, out Msg); //初始化 IP 端口号

                cc.ProcessInvoke(strCommand, out iResult, out strReplyInfo);

                return true;
            }
            catch (Exception err)
            {
                iResult = 9999;
                Msg = "调用ICE服务前失败" + err.ToString();
                return false;
            }
            finally
            {
                try
                {
                    string tmpMsg = "";
                    bool send = sendMail("bruceliao@tencent.com", "bruceliao@tencent.com", "IceCommand", "调用IceCommand: " + strCommand + "\r\n strReplyInfo: " + strReplyInfo + "\r\n iResult:" + iResult + "\r\n" + Msg, "inner", out tmpMsg);
                    //send=sendMail("alexguan@tencent.com","alexguan@tencent.com","IceCommand","调用IceCommand: " + strCommand+"\r\n strReplyInfo: "+strReplyInfo+"\r\n iResult:"+iResult+"\r\n"+Msg,"inner",out tmpMsg);
                    if (!send)
                    {
                        log4net.ILog log = log4net.LogManager.GetLogger("commRes.IceCommand");
                        if (log.IsErrorEnabled)
                            log.Error("调用IceCommand发送邮件失败:" + tmpMsg);
                    }
                }
                catch
                {
                }

                cc.Finit();
            }
        }

        public static bool sendMail(string mailToStr, string mailFromStr, string subject, string content, string type, out string Msg)  //发送邮件
        {
            Msg = null;
            try
            {
                TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend newMail = new TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend();
                newMail.SendMail(mailToStr, "", subject, content, true, null);

                //				MailMessage mail = new MailMessage();
                //				mail.From = mailFromStr;        //发件人
                //				mail.To   = mailToStr;          //收件人
                //				//mail.BodyEncoding = System.Text.Encoding.Unicode;
                //				mail.BodyFormat = MailFormat.Html;
                //				mail.Body = content; //邮件内容
                //				mail.Priority = MailPriority.High; //优先级
                //				mail.Subject  = subject;           //邮件主题
                //
                //				//SmtpMail.SmtpServer = ConfigurationManager.AppSettings["smtpServer"].ToString(); //"192.168.1.27";  邮件服务器地址
                //			
                //				if (type == "inner")
                //				{
                //					SmtpMail.SmtpServer = null;
                //					//SmtpMail.SmtpServer.Insert(0,ConfigurationManager.AppSettings["smtpServer"].ToString().Trim());	
                //					SmtpMail.SmtpServer = ConfigurationManager.AppSettings["smtpServer"].ToString(); //"192.168.1.27";  邮件服务器地址
                //					SmtpMail.Send(mail);
                //					SmtpMail.SmtpServer = null;
                //					
                //				}
                //				else if(type == "out")  //外部邮箱
                //				{
                //					SmtpMail.SmtpServer = null;
                //					//SmtpMail.SmtpServer.Insert(0,ConfigurationManager.AppSettings["OutSmtpServer"].ToString().Trim());	
                //					SmtpMail.SmtpServer = ConfigurationManager.AppSettings["OutSmtpServer"].ToString();
                //					SmtpMail.Send(mail);
                //					SmtpMail.SmtpServer = null;
                //				}
                //				else
                //				{
                //					Msg  = "邮件类型错误！ 请检查！只能为inner或out。";
                //					return false;
                //				}

                return true;
            }
            catch (Exception er)
            {
                Msg = er.Message.ToString().Replace("'", "’");
                return false;
            }
        }


        public static void GetTCPReply(string inmsg, string p, int portNumber)
        {
            throw new NotImplementedException();
        }

        /// <summary> 
        /// 检测输入的邮件地址strEmail是否合法，非法则返回true。 
        /// </summary> 
        public static bool CheckEmail(string strEmail)
        {
            int i, j;
            string strTmp, strResult;
            string strWords = "abcdefghijklmnopqrstuvwxyz_-.0123456789"; //定义合法字符范围 
            bool blResult = true;
            strTmp = strEmail.Trim();
            //检测输入字符串是否为空，不为空时才执行代码。 
            if (!(strTmp == "" || strTmp.Length == 0))
            {
                //判断邮件地址中是否存在“@”号 
                if ((strTmp.IndexOf("@") < 0))
                {
                    return blResult;
                }
                //以“@”号为分割符，把地址切分成两部分，分别进行验证。 
                string[] strChars = strTmp.Split(new char[] { '@' });
                foreach (string strChar in strChars)
                {
                    i = strChar.Length;
                    //“@”号前部分或后部分为空时。 
                    if (i == 0)
                    {
                        return blResult;
                    }
                    //逐个字进行验证，如果超出所定义的字符范围strWords，则表示地址非法。 
                    for (j = 0; j < i; j++)
                    {
                        strResult = strChar.Substring(j, 1).ToLower();//逐个字符取出比较 
                        if (strWords.IndexOf(strResult) < 0)
                        {
                            return blResult;
                        }
                    }
                }
                return false;
            }
            return blResult;
        }



        public static string GetFromRelay(string req_params, string ip, string port, out string Msg)
        {
            Msg = "";

            if (req_params == null || req_params == "")
            {
                Msg = "参数为空";
                return "";
            }

            TcpClient tcpClient = null;

            try
            {
                TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ip, "GetFromRelay", "发送包如下:" + req_params, "KF");

                //将请求参数封装
                byte[] b_msg = System.Text.Encoding.Default.GetBytes(req_params);
                byte[] b_msg_len = BitConverter.GetBytes(b_msg.Length);
                byte[] contData = new byte[b_msg_len.Length + b_msg.Length];
                b_msg_len.CopyTo(contData, 0);
                b_msg.CopyTo(contData, b_msg_len.Length);

                //connect...
                tcpClient = new TcpClient();
                IPAddress ipAddress = IPAddress.Parse(ip);
                IPEndPoint ipPort = new IPEndPoint(ipAddress, Int32.Parse(port));
                tcpClient.Connect(ipPort);
                NetworkStream stream = tcpClient.GetStream();
                stream.Write(contData, 0, contData.Length);
                byte[] bufferOut = new byte[4];
                stream.Read(bufferOut, 0, 4);  //读取返回长度
                int len = BitConverter.ToInt32(bufferOut, 0);
                bufferOut = new byte[len];
                // stream.Read(bufferOut, 0, len); //读取返回内容

                int nowindex = 0;
                while (nowindex < len)
                {
                    nowindex += stream.Read(bufferOut, nowindex, len - nowindex); //读取返回内容
                }

                string answer = Encoding.Default.GetString(bufferOut);

                TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ip, "GetFromRelay", "返回包如下:" + answer, "KF");
                return answer;
            }
            catch (Exception e)
            {
                Msg = "调用前置机失败：" + e.ToString();
                throw new Exception(Msg);
            }
            finally
            {
                if (tcpClient != null)
                    tcpClient.Close();
            }
        }

        //协议：包头4字节为包长度(网络字节序，包括包头和包体总长) 包体为字符串
        //响应：字符串
        public static string GetFromRelayHead4(string req_params, string ip, string port, out string Msg)
        {
            Msg = "";

            if (req_params == null || req_params == "")
            {
                Msg = "参数为空";
                return "";
            }

            TcpClient tcpClient = null;

            try
            {
                TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ip, "GetFromRelayHead4", "发送包如下:" + req_params, "KF");
                var parameters = System.Text.Encoding.Default.GetBytes(req_params);
                var length = new byte[4];
                //  length = BitConverter.GetBytes(parameters.Length);
                length = UDP.GetByteFromInt(parameters.Length + 4);
                List<byte> bufferIn = new List<byte>();
                bufferIn.AddRange(length);
                bufferIn.AddRange(parameters);

                //connect...
                tcpClient = new TcpClient();
                IPAddress ipAddress = IPAddress.Parse(ip);
                IPEndPoint ipPort = new IPEndPoint(ipAddress, Int32.Parse(port));
                tcpClient.Connect(ipPort);
                NetworkStream stream = tcpClient.GetStream();
                stream.Write(bufferIn.ToArray(), 0, bufferIn.ToArray().Length);
                byte[] bufferOut = new byte[1024];
                stream.Read(bufferOut, 0, 1024);
                byte[] bufferLen = new byte[4];
                Array.Copy(bufferOut, 0, bufferLen, 0, 4);
                // int c = BitConverter.ToInt32(bufferLen, 0);
                int c = UDP.GetIntFromByte(bufferLen);
                c = c - 4;
                byte[] cont = new byte[c];
                Array.Copy(bufferOut, 4, cont, 0, c);
                string answer = Encoding.Default.GetString(cont);
                string[] strlist1 = answer.Split('&');
                if (strlist1.Length == 0)
                {
                    throw new LogicException("返回结果有误：" + answer);
                }

                TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ip, "GetFromRelayHead4", "返回包如下:" + answer, "KF");
                return answer;
            }
            catch (Exception e)
            {
                Msg = "调用前置机失败：" + e.ToString();
                throw new Exception(Msg);
            }
            finally
            {
                if (tcpClient != null)
                    tcpClient.Close();
            }
        }

        public static string GetFromCGI(string cgi, string code, out string msg)
        {
            msg = "";
            if (string.IsNullOrEmpty(code))
            {
                code = "GB2312";
            }
            string answer = "";
            try
            {
                LogHelper.LogInfo("CGIQuery : " + cgi);
                System.Text.Encoding encoding = System.Text.Encoding.GetEncoding(code);
                System.Net.HttpWebRequest webrequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(cgi);
                webrequest.ContentType = "text/xml;charset=GBK";

                System.Net.HttpWebResponse webresponse = (System.Net.HttpWebResponse)webrequest.GetResponse();
                using (Stream stream = webresponse.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(stream, encoding))
                    {
                        answer = streamReader.ReadToEnd();
                    }
                }
                LogHelper.LogInfo("CGIQuery return: " + answer);
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
                LogHelper.LogInfo("CGIQuery Error: " + ex.Message);
                return "";
            }
            return answer;
        }

        /// <summary>
        ///  通过http post方式调用接口
        ///  接口还需要优化
        /// </summary>
        /// <param name="cgi">接口url</param>
        /// <param name="resCode">输出的编码方式</param>
        /// <param name="req">请求类型参数</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string GetFromCGIPost(string cgi, string resCode, string req, out string msg)
        {
            msg = "";
            if (string.IsNullOrEmpty(resCode))
            {
                resCode = "GBK";
            }

            string answer = "";
            try
            {
                LogHelper.LogInfo(string.Format("CGIPostQuery : {0}   req[{1}]"), cgi, req);
                System.Text.Encoding encoding = System.Text.Encoding.GetEncoding(resCode);
                System.Net.HttpWebRequest webrequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(cgi);
                webrequest.ContentType = "text/xml;charset=GBK";
                webrequest.Method = "POST";

                var data = Encoding.Default.GetBytes(req);
                var parameter = webrequest.GetRequestStream();
                parameter.Write(data, 0, data.Length);

                System.Net.HttpWebResponse webresponse = (System.Net.HttpWebResponse)webrequest.GetResponse();
                using (Stream stream = webresponse.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(stream, encoding))
                    {
                        answer = streamReader.ReadToEnd();
                    }
                }
                LogHelper.LogInfo("CGIPostQuery return: " + answer);
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
                LogHelper.LogInfo("CGIPostQuery Error: " + ex.Message);
                return "";
            }
            return answer;
        }
    }


    /// <summary>
    /// ICE超时处理线程
    /// </summary>
    public class ICEReceiveThread
    {
        /// <summary>
        /// 接收到的信息
        /// </summary>
        public string strReplyInfo;
        public int iResult;
        public string Msg;

        /// <summary>
        /// 是否已得到结果
        /// </summary>
        public int flag = 0;

        private Thread tr = null;

        private string ficeserverip;
        private int ficeport;
        private string fservicename;
        private string frequestmsg;
        private short fserverflag;
        private short ftimeout;
        private string fuser;
        private string fpwd;

        /// <summary>
        /// 关闭线程
        /// </summary>
        public void Close()
        {
            if (tr != null)
            {
                tr.Abort();
            }

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ICEReceiveThread(string iceserverip, int iceport, string servicename, string requestmsg,
            short serverflag, short timeout, string user, string pwd)
        {
            ficeserverip = iceserverip;
            ficeport = iceport;
            fservicename = servicename;
            frequestmsg = requestmsg;
            fserverflag = serverflag;
            ftimeout = timeout;
            fuser = user;
            fpwd = pwd;
        }

        /// <summary>
        /// 开始线程
        /// </summary>
        public void OpenThread()
        {
            Thread tr = new Thread(new ThreadStart(RunThread));
            tr.Start();
        }

        //		/// <summary>
        //		/// 运行线程
        //		/// </summary>
        //		void RunThread()
        //		{
        //
        //			try
        //			{
        //				int temFlag=0;
        //				BackProcessClient.CIceClientProxyClass cc = new BackProcessClient.CIceClientProxyClass();
        //				iResult = -1;
        //				
        //				try
        //				{
        //					iResult = cc.Init(ficeserverip, ficeport, out Msg); //初始化 IP 端口号
        //					
        //
        //					if(iResult == 0)
        //					{
        //						cc.MiddleInvoke(fservicename,frequestmsg,fserverflag,ftimeout,fuser,fpwd,out strReplyInfo,out iResult,out Msg);
        //						temFlag = 1;
        //					}
        //					else
        //					{
        //						Msg = "初始化ICE报错 " + Msg;
        //						strReplyInfo = "";
        //						temFlag = 9;
        //					}
        //				}
        //				finally
        //				{
        //					
        //					try
        //					{
        //						if(cc!=null)
        //						{
        //							cc.Finit();	
        //						}
        //						else
        //						{
        //							Msg = "初始化ICE对象失败";
        //							temFlag=9;
        //						}
        //					}
        //					catch(Exception cerr)
        //					{
        //						string tmpMsg="";
        //						commRes.sendMail("bruceliao@tencent.com","bruceliao@tencent.com","调用ICE关闭异常","服务名："+fservicename+" 调用串："+frequestmsg+" 错误信息："+cerr.Message,"inner",out tmpMsg);
        //					}
        //					flag=temFlag;
        //					
        //				}
        //			}
        //			catch(Exception err)
        //			{
        //				try
        //				{
        //					string tmpMsg="";
        //					bool send=commRes.sendMail("bruceliao@tencent.com","bruceliao@tencent.com","调用ICE异常","调用ICE异常: " +Msg +" 服务名："+fservicename+" 调用串："+frequestmsg+" 错误信息："+err.Message,"inner",out tmpMsg);
        //					if(!send)
        //					{
        //						log4net.ILog log = log4net.LogManager.GetLogger("middleInvoke.RunThread");
        //						if(log.IsErrorEnabled)
        //							log.Error("调用ICE异常:" + tmpMsg);
        //					}
        //				}
        //				catch
        //				{
        //				}
        //
        //				Msg = err.Message;
        //				flag = 9;
        //			}            
        //		}


        /// <summary>
        /// 运行线程
        /// </summary>
        void RunThread()
        {

            try
            {
                int temFlag = 0;
                iResult = -1;
                BackProcessClientCS.ICEInvoke cc = new BackProcessClientCS.ICEInvoke();

                try
                {
                    bool initFlag = cc.Init(ficeserverip, ficeport); //初始化 IP 端口号
                    if (initFlag)
                    {
                        cc.middleInvoke(fservicename, frequestmsg, fserverflag, ftimeout, fuser, fpwd, out strReplyInfo, out iResult, out Msg);
                        temFlag = 1;

                    }
                    else
                    {
                        Msg = "初始化ICE报错 " + Msg;
                        strReplyInfo = "";
                        temFlag = 9;
                    }
                }
                finally
                {

                    try
                    {
                        if (cc != null)
                        {
                            cc.Finit();
                        }
                        else
                        {
                            Msg = "初始化ICE对象失败";
                            temFlag = 9;
                        }
                    }
                    catch
                    {
                    }
                    flag = temFlag;

                }
            }
            catch (Exception err)
            {
                Msg = err.ToString();
                flag = 9;
            }
        }

    }
}
