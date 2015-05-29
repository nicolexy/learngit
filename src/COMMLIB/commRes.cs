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
    /// commMethod ��������һЩͨ�õķ��������緢����־��Web��service����Ҫʹ�õ���һЩ���õķ�����������
    /// ��Ҫ����publicRes��ѹ�����������ࡣ
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
                //��¼���Ͱ�
                log4net.ILog log = log4net.LogManager.GetLogger("commRes.GetTCPReply");
                if (log.IsInfoEnabled)
                    log.Info("���Ͱ�����:������ϳ���=" + bufferInString.Length + "+����=" + bufferInString);

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
                    //��¼���հ�
                    if (log.IsInfoEnabled)
                        log.Info("���ذ�����:result=" + paramsHt["result"] + ",res_info=" + paramsHt["res_info"] + ",token_seq=" + paramsHt["token_seq"]);
                }
            }
            catch (Exception e)
            {
                iResult = "9999";
                log4net.ILog log = log4net.LogManager.GetLogger("commRes.GetTCPReply");
                if (log.IsErrorEnabled)
                    log.Error("serviceName=" + serviceName + ";", e);
                Msg = "����TCPʧ��:" + e.ToString();
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
            // TODO: �ڴ˴���ӹ��캯���߼�
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
                // furion 20141023�������ʼ��ICE�ͻ��� ��С���۵Ļ������Ǹ�ϵͳ��һ��������Ϳ���ʹ�ã�
                // ���ݿ��������û��޸İ汾ʱ���ͱ���ڸ���Ŀ�������ICE�ͻ��˽��г�ʼ����
            }
            catch (Exception ex)
            {
                LogHelper.LogInfo("��ȡ�����ļ����ִ���" + ex.ToString());
            }

            try
            {
                string dicconnstr = DbConnectionString.Instance.GetConnectionString("RelayConnString");
                InitRelayInfo(dicconnstr);
            }
            catch (Exception ex)
            {
                LogHelper.LogInfo("��ȡRelay�����ַ�����������" + ex.ToString());
            }
        }

        public static void InitRelayInfo(string connstring)
        {
            if (connstring == "")
                return;

            //furion 20141023 ������Ӵ��������relay��Ϣת����
            Middle2Relay.InitRelayInfo(connstring);
        }

        /// <summary>
        /// ����log4��־,ͬʱ��¼�ļ���־
        /// </summary>
        /// <param name="modeInfo">���͵�ģ������</param>
        /// <param name="Msg">���͵���Ϣ</param>
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
        /// д�ļ���־
        /// </summary>
        //		public static string filename  = ConfigurationManager.AppSettings["logPath"].ToString() + "������־" +DateTime.Now.ToString("yyyyMMdd") + ".txt";  //ÿ��һ����־   \\"c:\\333.txt";
        //		public static StreamWriter swFromFile = null;
        public static void WriteFile(string strmsg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("Finance_Service.Commlig.WriteFile");
            if (log.IsInfoEnabled)
                log.Info(strmsg);

            //			string newfilename  = ConfigurationManager.AppSettings["logPath"].ToString() + "������־" +DateTime.Now.ToString("yyyyMMdd") + ".txt";  //ÿ��һ����־   \\"c:\\333.txt";
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
        /// ȥ�������ַ�
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        //public static string replaceMStr(string str)  //�Բ������ݿ���ַ����������ַ������滻,���÷Ƿ�sqlע�����
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
                    log.Error("ת���ַ���16����ʧ��:" + ex.ToString());
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

            str = str.Replace("\"", "��");
            str = str.Replace("'", "��");

            str = str.Replace("script", "�������");
            str = str.Replace("<", "��");
            str = str.Replace(">", "��");
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
        /// ���DataSet�Ƿ�������
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static bool checkDataSet(DataSet ds, out string Msg)
        {
            Msg = "";
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                Msg = "�Բ�������Ϊ��.����!";
                return false;
            }

            return true;
        }



        /// <summary>
        /// ��middle������������ּ��ܺͷǼ������֣�
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="requestMsg"></param>
        /// <param name="secret"></param>
        /// <param name="strReplyInfo"></param>
        /// <param name="iResult">0��ʾ�ɹ���9999��ʾ�����쳣����,����ΪICE�ӿڷ��ش���</param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public static bool middleInvoke(string serviceName, string requestMsg, bool secret, out string strReplyInfo, out short iResult, out string Msg)
        {
            strReplyInfo = "";
            iResult = 9999;
            Msg = "";
            try
            {
                //furion ��¼һ�·��Ͱ��ͽ��հ�
                /*
                log4net.ILog log = log4net.LogManager.GetLogger("CommLib.middleInvoke");
                if(log.IsInfoEnabled)
                    log.Info("���Ͱ�����:" + requestMsg);
                */

                TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ICEServerIP.ToString(), serviceName, requestMsg, "KF");

                //furion 20141023 ����relayת�����������ֵ��ת������������ã�����ԭ�����á�
                string routevalue = "";
                if (Middle2Relay.NeedGoRelay(serviceName, requestMsg))
                {
                    if (!Middle2Relay.GetRouteParam(serviceName, requestMsg, out routevalue))
                    {
                        //������뷵��true�����false��Ӧ�����������󡣿��Ա���
                        Msg = "ת������relayʱ����" + routevalue;
                        LogHelper.LogInfo(Msg);
                        return false;
                    }
                }
                //20060612 �������Ӧ���ǽ���. furion
                //requestMsg = cf.UnEscape(requestMsg);   //����

                //ex_common_query_service����Ҫ��ǰ׺����� furion 20090527
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
                        requestMsg = cf.EncapsRequest(CFTAcc, requestMsg);      //����
                        requestMsg = "sp_id=" + CFTAcc + "&request_text=" + ICEEncode(requestMsg);  //���
                    }
                    else
                    {
                        requestMsg = "sp_id=" + CFTAcc + "&" + requestMsg;
                    }

                }

                //				if (secret == true)
                //					requestMsg = cf.EncapsRequest("2000000000",requestMsg);      //����
                //			
                //				requestMsg = "sp_id=2000000000&request_text=" + requestMsg;  //���

                //				BackProcessClient.CIceClientProxyClass cc = new BackProcessClient.CIceClientProxyClass();
                //				iResult = cc.Init("192.168.3.180", 6644, out Msg); //��ʼ�� IP �˿ں�
                //				iResult = cc.Init(ICEServerIP, ICEPort, out Msg); //��ʼ�� IP �˿ں�
                //				cc.MiddleInvoke(serviceName,requestMsg,0,5000,iceUsr,icePwd,out strReplyInfo,out iResult,out Msg); 

                //furion 20141023 ����relayת�����������ֵ��ת������������ã�����ԭ�����á�
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
                                log.Info("���ذ�����:" + strReplyInfo);
                                */
                            TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ICEServerIP.ToString(), serviceName, "���ذ�����1:" + "[flag=]" + irt.flag + "[msg=]" + irt.Msg + "[replyinfo=]" + strReplyInfo, "KF");
                            return true;
                        }

                        //if(i == 49)
                        if (i == 599)
                        {
                            //irt.Close();
                            throw new LogicException("ICE����ʱ��");
                        }
                    }
                }
                finally
                {
                    irt.Close();
                }
                /*
                //furion ��¼һ�·��Ͱ��ͽ��հ�
                //log4net.ILog log = log4net.LogManager.GetLogger("commRes.middleInvoke");
                if(log.IsInfoEnabled)
                    log.Info("���ذ�����:" + strReplyInfo);
                    */
                TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ICEServerIP.ToString(), serviceName, "���ذ�����2:" + "[flag=]" + iResult + "[msg=]" + Msg + "[replyinfo=]" + strReplyInfo, "KF");

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
                Msg = "����ICE����ǰʧ��" + err.ToString();
                return false;
            }
        }

        public bool NoStaticMiddleInvokeTest(string serviceName, string requestMsg, bool secret, out string strReplyInfo, out short iResult, out string Msg)
        {
            bool index = middleInvoke(serviceName, requestMsg, secret, out strReplyInfo, out iResult, out Msg);
            return index;
        }

        /// <summary>
        /// ��middle������������ּ��ܺͷǼ������֣����ϵͳ
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="requestMsg"></param>
        /// <param name="secret"></param>
        /// <param name="strReplyInfo"></param>
        /// <param name="iResult">0��ʾ�ɹ���9999��ʾ�����쳣����,����ΪICE�ӿڷ��ش���</param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public static bool middleInvokeIA(string serviceName, string requestMsg, bool secret, out string strReplyInfo, out short iResult, out string Msg)
        {
            strReplyInfo = "";
            iResult = 9999;
            Msg = "";
            try
            {
                //furion ��¼һ�·��Ͱ��ͽ��հ�
                /*
                log4net.ILog log = log4net.LogManager.GetLogger("CommLib.middleInvoke");
                if(log.IsInfoEnabled)
                    log.Info("���Ͱ�����:" + requestMsg);
                */

                TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ICEServerIP_ia.ToString(), serviceName, requestMsg, "KF");

                //20060612 �������Ӧ���ǽ���. furion
                //requestMsg = cf.UnEscape(requestMsg);   //����

                //ex_common_query_service����Ҫ��ǰ׺����� furion 20090527
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
                        requestMsg = cf.EncapsRequest(CFTAcc, requestMsg);      //����
                        requestMsg = "sp_id=" + CFTAcc + "&request_text=" + ICEEncode(requestMsg);  //���
                    }
                    else
                    {
                        requestMsg = "sp_id=" + CFTAcc + "&" + requestMsg;
                    }

                }

                //				if (secret == true)
                //					requestMsg = cf.EncapsRequest("2000000000",requestMsg);      //����
                //			
                //				requestMsg = "sp_id=2000000000&request_text=" + requestMsg;  //���

                //				BackProcessClient.CIceClientProxyClass cc = new BackProcessClient.CIceClientProxyClass();
                //				iResult = cc.Init("192.168.3.180", 6644, out Msg); //��ʼ�� IP �˿ں�
                //				iResult = cc.Init(ICEServerIP, ICEPort, out Msg); //��ʼ�� IP �˿ں�
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
                                log.Info("���ذ�����:" + strReplyInfo);
                                */
                            TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ICEServerIP_ia.ToString(), serviceName, "���ذ�����1:" + "[flag=]" + irt.flag + "[msg=]" + irt.Msg + "[replyinfo=]" + strReplyInfo, "KF");
                            return true;
                        }

                        //if(i == 49)
                        if (i == 599)
                        {
                            //irt.Close();
                            throw new LogicException("ICE����ʱ��");
                        }
                    }
                }
                finally
                {
                    irt.Close();
                }
                /*
                //furion ��¼һ�·��Ͱ��ͽ��հ�
                //log4net.ILog log = log4net.LogManager.GetLogger("commRes.middleInvoke");
                if(log.IsInfoEnabled)
                    log.Info("���ذ�����:" + strReplyInfo);
                    */
                TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ICEServerIP_ia.ToString(), serviceName, "���ذ�����2:" + "[flag=]" + iResult + "[msg=]" + Msg + "[replyinfo=]" + strReplyInfo, "KF");

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
                Msg = "����ICE����ǰʧ��" + err.ToString();
                return false;
            }
        }

        /// <summary>
        /// ��middle������������ַ�������Ҫ���ܺ�ת��ģ�
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="requestMsg"></param>
        /// <param name="secret"></param>
        /// <param name="strReplyInfo"></param>
        /// <param name="iResult">0��ʾ�ɹ���9999��ʾ�����쳣����,����ΪICE�ӿڷ��ش���</param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public static bool middleInvoke2(string serviceName, string requestMsg, out string strReplyInfo, out short iResult, out string Msg)
        {
            strReplyInfo = "";
            iResult = 9999;
            Msg = "";
            try
            {
                //furion ��¼һ�·��Ͱ��ͽ��հ�
                log4net.ILog log = log4net.LogManager.GetLogger("commRes.middleInvoke");
                if (log.IsInfoEnabled)
                    log.Info("���Ͱ�����:" + requestMsg);


                string CFTAcc = ConfigurationManager.AppSettings["CFTAccount"].Trim();

                requestMsg = "sp_id=" + CFTAcc + "&" + requestMsg;  //���

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
                                log.Info("���ذ�����1:" + "[flag=]" + irt.flag + "[msg=]" + irt.Msg + "[replyinfo=]" + strReplyInfo);
                            return true;
                        }

                        //if(i == 49)
                        if (i == 599)
                        {
                            //irt.Close();
                            throw new LogicException("ICE����ʱ��");
                        }
                    }
                }
                finally
                {
                    irt.Close();
                }
                //furion ��¼һ�·��Ͱ��ͽ��հ�
                //log4net.ILog log = log4net.LogManager.GetLogger("commRes.middleInvoke");
                if (log.IsInfoEnabled)
                    log.Info("���ذ�����2:" + "[flag=]" + iResult + "[msg=]" + Msg + "[replyinfo=]" + strReplyInfo);

                return true;
            }
            catch (Exception err)
            {
                log4net.ILog log = log4net.LogManager.GetLogger("commRes.middleInvoke");
                if (log.IsErrorEnabled)
                    log.Error("serviceName=" + serviceName + ";", err);  //requestMsg=" + requestMsg + ";"
                iResult = 9999;
                Msg = "����ICE����ǰʧ��" + err.ToString();
                return false;
            }
        }
        /// <summary>
        /// ��middle������������ּ��ܺͷǼ������֣�����3.0��request_type������������
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="requestMsg"></param>
        /// <param name="secret"></param>
        /// <param name="strReplyInfo"></param>
        /// <param name="iResult">0��ʾ�ɹ���9999��ʾ�����쳣����,����ΪICE�ӿڷ��ش���</param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public static bool middleInvoke_V30(string serviceName, string requestMsg, bool secret, string outrangestr, out string strReplyInfo, out short iResult, out string Msg)
        {
            strReplyInfo = "";
            iResult = 9999;
            Msg = "";
            try
            {
                //furion ��¼һ�·��Ͱ��ͽ��հ�
                /*
                log4net.ILog log = log4net.LogManager.GetLogger("CommLib.middleInvoke");
                if(log.IsInfoEnabled)
                    log.Info("���Ͱ�����:" + requestMsg);
                    */
                TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ICEServerIP.ToString(), serviceName, requestMsg, "KF");

                CFTGWCLib.CFTCryptoClass cf = new CFTGWCLib.CFTCryptoClass();

                //furion 20141023 ����relayת�����������ֵ��ת������������ã�����ԭ�����á�
                string routevalue = "";
                if (Middle2Relay.NeedGoRelay(serviceName, requestMsg))
                {
                    if (!Middle2Relay.GetRouteParam(serviceName, requestMsg, out routevalue))
                    {
                        //������뷵��true�����false��Ӧ�����������󡣿��Ա���
                        Msg = "ת������relayʱ����" + routevalue;
                        return false;
                    }
                }
                //20060612 �������Ӧ���ǽ���. furion
                //requestMsg = cf.UnEscape(requestMsg);   //����

                //ex_common_query_service����Ҫ��ǰ׺����� furion 20090527
                if (serviceName != "ex_common_query_service" && serviceName != "ui_common_query_service" && serviceName != "ui_common_update_service"
                    && serviceName != "cq_query_tcbanklist_service" && serviceName != "query_order_service" && serviceName != "bank_pos_single_query_service"
                    && serviceName != "bank_pos_batch_query_service" && serviceName != "waika_bank_pos_single_query_service" && serviceName != "zw_prodatt_query_service")
                {
                    string CFTAcc = ConfigurationManager.AppSettings["CFTAccount"].Trim();

                    if (secret == true)
                        requestMsg = cf.EncapsRequest(CFTAcc, requestMsg);      //����

                    //furion 20091121 blue�Ľӿڲ���ǰ��spid,eriken�Ľӿ���Ҫǰ��spid
                    if (outrangestr.IndexOf("sp_id=") > -1)
                        requestMsg = outrangestr + "&request_text=" + ICEEncode(requestMsg);
                    else
                        requestMsg = "sp_id=" + CFTAcc + "&" + outrangestr + "&request_text=" + ICEEncode(requestMsg);  //���
                }

                //furion 20141023 ����relayת�����������ֵ��ת������������ã�����ԭ�����á�
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
                                log.Info("���ذ�����:" + strReplyInfo);
                                */
                            TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ICEServerIP.ToString(), serviceName, "���ذ�����:" + strReplyInfo, "KF");
                            return true;
                        }

                        //if(i == 49)
                        if (i == 599)
                        {
                            //irt.Close();
                            throw new LogicException("ICE����ʱ��");
                        }
                    }
                }
                finally
                {
                    irt.Close();
                }
                /*
                //furion ��¼һ�·��Ͱ��ͽ��հ�
                //log4net.ILog log = log4net.LogManager.GetLogger("commRes.middleInvoke");
                if(log.IsInfoEnabled)
                    log.Info("���ذ�����:" + strReplyInfo);
                    */
                TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ICEServerIP.ToString(), serviceName, "���ذ�����:" + strReplyInfo, "KF");

                return true;
            }
            catch (Exception err)
            {
                /*
                log4net.ILog log = log4net.LogManager.GetLogger("CommLib.middleInvoke");
                if(log.IsErrorEnabled)
                    log.Error("serviceName=" + serviceName + ";",err);  //requestMsg=" + requestMsg + ";"
                iResult = 9999;
                Msg = "����ICE����ǰʧ��" + err.Message;
                */

                TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ICEServerIP.ToString(), serviceName, "����ICE����ǰʧ��" + err.ToString(), "KF");
                return false;
            }
        }

        /// <summary>
        /// ��middle������������ּ��ܺͷǼ������֣�
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="requestMsg"></param>
        /// <param name="secret"></param>
        /// <param name="strReplyInfo"></param>
        /// <param name="iResult">0��ʾ�ɹ���9999��ʾ�����쳣����,����ΪICE�ӿڷ��ش���</param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public static bool NTBmiddleInvoke(string serviceName, string requestMsg, bool secret, out string strReplyInfo, out short iResult, out string Msg)
        {
            strReplyInfo = "";
            iResult = 9999;
            Msg = "";
            try
            {
                //furion ��¼һ�·��Ͱ��ͽ��հ�
                log4net.ILog log = log4net.LogManager.GetLogger("CommLib.middleInvoke");
                if (log.IsInfoEnabled)
                    log.Info("���Ͱ�����:" + requestMsg);

                CFTGWCLib.CFTCryptoClass cf = new CFTGWCLib.CFTCryptoClass();

                string CFTAcc = ConfigurationManager.AppSettings["CFTAccount"].Trim();

                if (secret == true)
                    requestMsg = cf.EncapsRequest(CFTAcc, requestMsg);      //����

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
                                log.Info("���ذ�����:" + strReplyInfo);
                            return true;
                        }

                        //if(i == 49)
                        if (i == 599)
                        {
                            //irt.Close();
                            throw new LogicException("ICE����ʱ��");
                        }
                    }
                }
                finally
                {
                    irt.Close();
                }
                //furion ��¼һ�·��Ͱ��ͽ��հ�
                //log4net.ILog log = log4net.LogManager.GetLogger("commRes.middleInvoke");
                if (log.IsInfoEnabled)
                    log.Info("���ذ�����:" + strReplyInfo);

                return true;
            }
            catch (Exception err)
            {
                log4net.ILog log = log4net.LogManager.GetLogger("commRes.middleInvoke");
                if (log.IsErrorEnabled)
                    log.Error("serviceName=" + serviceName + ";", err);  //requestMsg=" + requestMsg + ";"
                iResult = 9999;
                Msg = "����ICE����ǰʧ��" + err.ToString();
                return false;
            }
        }

        /// <summary>
        /// ͨ��ICE���ÿ�ִ�г���
        /// </summary>
        /// <param name="strCommand"></param>
        /// <param name="iResult">0��ʾ�ɹ�;9999��ʾ�����쳣����,����ΪICE�ӿڷ��ش���.</param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public static bool IceCommand(string strCommand, out short iResult, out string strReplyInfo, out string Msg)
        {
            strReplyInfo = "";
            iResult = 9999;  //9999��ʾ�����쳣����
            Msg = "";
            BackProcessClient.CIceClientProxyClass cc = new BackProcessClient.CIceClientProxyClass();
            try
            {
                CFTGWCLib.CFTCryptoClass cf = new CFTGWCLib.CFTCryptoClass();

                iResult = cc.Init(ICEServerIP, ICEPort, out Msg); //��ʼ�� IP �˿ں�    ��������//iResult = cc.Init("192.168.3.180", 6644, out Msg); //��ʼ�� IP �˿ں�

                cc.ProcessInvoke(strCommand, out iResult, out strReplyInfo);

                return true;
            }
            catch (Exception err)
            {
                iResult = 9999;
                Msg = "����ICE����ǰʧ��" + err.ToString();
                return false;
            }
            finally
            {
                try
                {
                    string tmpMsg = "";
                    bool send = sendMail("bruceliao@tencent.com", "bruceliao@tencent.com", "IceCommand", "����IceCommand: " + strCommand + "\r\n strReplyInfo: " + strReplyInfo + "\r\n iResult:" + iResult + "\r\n" + Msg, "inner", out tmpMsg);
                    //send=sendMail("alexguan@tencent.com","alexguan@tencent.com","IceCommand","����IceCommand: " + strCommand+"\r\n strReplyInfo: "+strReplyInfo+"\r\n iResult:"+iResult+"\r\n"+Msg,"inner",out tmpMsg);
                    if (!send)
                    {
                        log4net.ILog log = log4net.LogManager.GetLogger("commRes.IceCommand");
                        if (log.IsErrorEnabled)
                            log.Error("����IceCommand�����ʼ�ʧ��:" + tmpMsg);
                    }
                }
                catch
                {
                }

                cc.Finit();
            }
        }

        public static bool sendMail(string mailToStr, string mailFromStr, string subject, string content, string type, out string Msg)  //�����ʼ�
        {
            Msg = null;
            try
            {
                TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend newMail = new TENCENT.OSS.C2C.Finance.Common.CommLib.NewMailSend();
                newMail.SendMail(mailToStr, "", subject, content, true, null);

                //				MailMessage mail = new MailMessage();
                //				mail.From = mailFromStr;        //������
                //				mail.To   = mailToStr;          //�ռ���
                //				//mail.BodyEncoding = System.Text.Encoding.Unicode;
                //				mail.BodyFormat = MailFormat.Html;
                //				mail.Body = content; //�ʼ�����
                //				mail.Priority = MailPriority.High; //���ȼ�
                //				mail.Subject  = subject;           //�ʼ�����
                //
                //				//SmtpMail.SmtpServer = ConfigurationManager.AppSettings["smtpServer"].ToString(); //"192.168.1.27";  �ʼ���������ַ
                //			
                //				if (type == "inner")
                //				{
                //					SmtpMail.SmtpServer = null;
                //					//SmtpMail.SmtpServer.Insert(0,ConfigurationManager.AppSettings["smtpServer"].ToString().Trim());	
                //					SmtpMail.SmtpServer = ConfigurationManager.AppSettings["smtpServer"].ToString(); //"192.168.1.27";  �ʼ���������ַ
                //					SmtpMail.Send(mail);
                //					SmtpMail.SmtpServer = null;
                //					
                //				}
                //				else if(type == "out")  //�ⲿ����
                //				{
                //					SmtpMail.SmtpServer = null;
                //					//SmtpMail.SmtpServer.Insert(0,ConfigurationManager.AppSettings["OutSmtpServer"].ToString().Trim());	
                //					SmtpMail.SmtpServer = ConfigurationManager.AppSettings["OutSmtpServer"].ToString();
                //					SmtpMail.Send(mail);
                //					SmtpMail.SmtpServer = null;
                //				}
                //				else
                //				{
                //					Msg  = "�ʼ����ʹ��� ���飡ֻ��Ϊinner��out��";
                //					return false;
                //				}

                return true;
            }
            catch (Exception er)
            {
                Msg = er.Message.ToString().Replace("'", "��");
                return false;
            }
        }


        public static void GetTCPReply(string inmsg, string p, int portNumber)
        {
            throw new NotImplementedException();
        }

        /// <summary> 
        /// ���������ʼ���ַstrEmail�Ƿ�Ϸ����Ƿ��򷵻�true�� 
        /// </summary> 
        public static bool CheckEmail(string strEmail)
        {
            int i, j;
            string strTmp, strResult;
            string strWords = "abcdefghijklmnopqrstuvwxyz_-.0123456789"; //����Ϸ��ַ���Χ 
            bool blResult = true;
            strTmp = strEmail.Trim();
            //��������ַ����Ƿ�Ϊ�գ���Ϊ��ʱ��ִ�д��롣 
            if (!(strTmp == "" || strTmp.Length == 0))
            {
                //�ж��ʼ���ַ���Ƿ���ڡ�@���� 
                if ((strTmp.IndexOf("@") < 0))
                {
                    return blResult;
                }
                //�ԡ�@����Ϊ�ָ�����ѵ�ַ�зֳ������֣��ֱ������֤�� 
                string[] strChars = strTmp.Split(new char[] { '@' });
                foreach (string strChar in strChars)
                {
                    i = strChar.Length;
                    //��@����ǰ���ֻ�󲿷�Ϊ��ʱ�� 
                    if (i == 0)
                    {
                        return blResult;
                    }
                    //����ֽ�����֤�����������������ַ���ΧstrWords�����ʾ��ַ�Ƿ��� 
                    for (j = 0; j < i; j++)
                    {
                        strResult = strChar.Substring(j, 1).ToLower();//����ַ�ȡ���Ƚ� 
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
                Msg = "����Ϊ��";
                return "";
            }

            TcpClient tcpClient = null;

            try
            {
                TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ip, "GetFromRelay", "���Ͱ�����:" + req_params, "KF");

                //�����������װ
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
                stream.Read(bufferOut, 0, 4);  //��ȡ���س���
                int len = BitConverter.ToInt32(bufferOut, 0);
                bufferOut = new byte[len];
                // stream.Read(bufferOut, 0, len); //��ȡ��������

                int nowindex = 0;
                while (nowindex < len)
                {
                    nowindex += stream.Read(bufferOut, nowindex, len - nowindex); //��ȡ��������
                }

                string answer = Encoding.Default.GetString(bufferOut);

                TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ip, "GetFromRelay", "���ذ�����:" + answer, "KF");
                return answer;
            }
            catch (Exception e)
            {
                Msg = "����ǰ�û�ʧ�ܣ�" + e.ToString();
                throw new Exception(Msg);
            }
            finally
            {
                if (tcpClient != null)
                    tcpClient.Close();
            }
        }

        //Э�飺��ͷ4�ֽ�Ϊ������(�����ֽ��򣬰�����ͷ�Ͱ����ܳ�) ����Ϊ�ַ���
        //��Ӧ���ַ���
        public static string GetFromRelayHead4(string req_params, string ip, string port, out string Msg)
        {
            Msg = "";

            if (req_params == null || req_params == "")
            {
                Msg = "����Ϊ��";
                return "";
            }

            TcpClient tcpClient = null;

            try
            {
                TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ip, "GetFromRelayHead4", "���Ͱ�����:" + req_params, "KF");
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
                    throw new LogicException("���ؽ������" + answer);
                }

                TENCENT.OSS.CFT.KF.DataAccess.MySqlAccess.WriteSqlLog(ip, "GetFromRelayHead4", "���ذ�����:" + answer, "KF");
                return answer;
            }
            catch (Exception e)
            {
                Msg = "����ǰ�û�ʧ�ܣ�" + e.ToString();
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
        ///  ͨ��http post��ʽ���ýӿ�
        ///  �ӿڻ���Ҫ�Ż�
        /// </summary>
        /// <param name="cgi">�ӿ�url</param>
        /// <param name="resCode">����ı��뷽ʽ</param>
        /// <param name="req">�������Ͳ���</param>
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
    /// ICE��ʱ�����߳�
    /// </summary>
    public class ICEReceiveThread
    {
        /// <summary>
        /// ���յ�����Ϣ
        /// </summary>
        public string strReplyInfo;
        public int iResult;
        public string Msg;

        /// <summary>
        /// �Ƿ��ѵõ����
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
        /// �ر��߳�
        /// </summary>
        public void Close()
        {
            if (tr != null)
            {
                tr.Abort();
            }

        }

        /// <summary>
        /// ���캯��
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
        /// ��ʼ�߳�
        /// </summary>
        public void OpenThread()
        {
            Thread tr = new Thread(new ThreadStart(RunThread));
            tr.Start();
        }

        //		/// <summary>
        //		/// �����߳�
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
        //					iResult = cc.Init(ficeserverip, ficeport, out Msg); //��ʼ�� IP �˿ں�
        //					
        //
        //					if(iResult == 0)
        //					{
        //						cc.MiddleInvoke(fservicename,frequestmsg,fserverflag,ftimeout,fuser,fpwd,out strReplyInfo,out iResult,out Msg);
        //						temFlag = 1;
        //					}
        //					else
        //					{
        //						Msg = "��ʼ��ICE���� " + Msg;
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
        //							Msg = "��ʼ��ICE����ʧ��";
        //							temFlag=9;
        //						}
        //					}
        //					catch(Exception cerr)
        //					{
        //						string tmpMsg="";
        //						commRes.sendMail("bruceliao@tencent.com","bruceliao@tencent.com","����ICE�ر��쳣","��������"+fservicename+" ���ô���"+frequestmsg+" ������Ϣ��"+cerr.Message,"inner",out tmpMsg);
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
        //					bool send=commRes.sendMail("bruceliao@tencent.com","bruceliao@tencent.com","����ICE�쳣","����ICE�쳣: " +Msg +" ��������"+fservicename+" ���ô���"+frequestmsg+" ������Ϣ��"+err.Message,"inner",out tmpMsg);
        //					if(!send)
        //					{
        //						log4net.ILog log = log4net.LogManager.GetLogger("middleInvoke.RunThread");
        //						if(log.IsErrorEnabled)
        //							log.Error("����ICE�쳣:" + tmpMsg);
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
        /// �����߳�
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
                    bool initFlag = cc.Init(ficeserverip, ficeport); //��ʼ�� IP �˿ں�
                    if (initFlag)
                    {
                        cc.middleInvoke(fservicename, frequestmsg, fserverflag, ftimeout, fuser, fpwd, out strReplyInfo, out iResult, out Msg);
                        temFlag = 1;

                    }
                    else
                    {
                        Msg = "��ʼ��ICE���� " + Msg;
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
                            Msg = "��ʼ��ICE����ʧ��";
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
