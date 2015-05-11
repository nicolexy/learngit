using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TENCENT.OSS.C2C.Finance.DataAccess;
using System.Configuration;
using CFT.Apollo.CommunicationFramework;
using System.Collections;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using System.Data;
using CFT.Apollo.CommunicationFramework;
using CFT.Apollo.Logging;

namespace CFT.CSOMS.DAL.Infrastructure
{
    public class RelayAccessFactory
    {
       /// <summary>
        /// 调relay接口，返回全部字符串 自己拼接全部源串
        /// </summary>
        /// <param name="inmsg"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public static string RelayInvoke(string relayRequestString, string relayIP = "", int relayPort = 0, string coding = "", bool invisible = false, string relayDefaultSPId = "")
        {
            LogHelper.LogInfo(relayRequestString);
            //关闭组件日志打印，因打印的可能是乱，编码后手动打印。
            var relayResponse = RelayHelper.CommunicateWithRelay(relayRequestString, true, relayIP, relayPort);
            if (!string.IsNullOrEmpty(coding))
            {
                Encoding encoding = Encoding.GetEncoding(coding);
                string result = encoding.GetString(relayResponse.ResponseBuffer);
                LogHelper.LogInfo(result);
                return result;
            }
            else
            {
                return Encoding.Default.GetString(relayResponse.ResponseBuffer);
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
        public static string RelayInvoke(string requestString, string serviceCode, bool encrypt = false, bool invisible = false, string relayIP = "", int relayPort = 0, string coding = "", string relayDefaultSPId = "")
        {
            try
            {
                
                var relayResponse = RelayHelper.CommunicateWithRelay( requestString,  serviceCode,  encrypt,  invisible ,  relayIP ,  relayPort,  relayDefaultSPId);
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
        /// 调relay接口字节转UTF8
        /// </summary>
        /// <param name="requestString">请求串，包含接口特性的参数，不包含ver、head等</param>
        /// <param name="serviceCode">request-type</param>
        /// <param name="encrypt">请求串是否加密</param>
        /// <param name="invisible"></param>
        /// <param name="relayIP">ip 不填默认</param>
        /// <param name="relayPort">端口 不填默认</param>
        /// <param name="relayDefaultSPId">sp_id</param>
        /// <returns></returns>
        public static string RelayInvokeUTF(string requestString, string serviceCode, bool encrypt = false, bool invisible = false, string relayIP = "", int relayPort = 0, string relayDefaultSPId = "")
        {
            try
            {

                var relayResponse = RelayHelper.CommunicateWithRelay(requestString, serviceCode, encrypt, invisible, relayIP, relayPort, relayDefaultSPId);
                return Encoding.UTF8.GetString(relayResponse.ResponseBuffer);
            }
            catch (Exception err)
            {
                string error = "调用relay服务前失败" + err.Message;
                LogHelper.LogInfo(error);
                throw new Exception(error);
            }
        }
        /// <summary>
        /// 
        /// 解析字符串到DataSet  接口返回字符串格式
        /// </summary>
        /// <param name="inmsg"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public static DataSet GetDSFromRelay(string requestString, string serviceCode, string relayIP = "", int relayPort = 0, bool encrypt = false, bool invisible = false, string relayDefaultSPId = "")
        {
            string Msg = "";
            string answer = RelayInvoke(requestString, serviceCode, encrypt, invisible, relayIP, relayPort, relayDefaultSPId);
            DataSet ds = null;
            if (answer == "")
            {
                return null;
            }

            //解析
            ds = CommQuery.ParseRelayStr(answer, out Msg);
            if (Msg != "")
            {
                throw new Exception(Msg);
            }
            return ds;
        }

        /// <summary>
        /// 
        /// 解析字符串到DataSet  接口返回字符串格式
        /// </summary>
        /// <param name="inmsg"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public static DataSet GetBankInfoFromRelay(string requestString, string serviceCode, string relayIP = "", int relayPort = 0, bool encrypt = false, bool invisible = false, string relayDefaultSPId = "")
        {
            string answer = "";
            try
            {
                var relayResponse = RelayHelper.CommunicateWithRelay(requestString, serviceCode, encrypt, invisible, relayIP, relayPort, relayDefaultSPId);
                answer = Encoding.GetEncoding("gb2312").GetString(relayResponse.ResponseBuffer);//Encoding.UTF8.GetString(relayResponse.ResponseBuffer);
                //记录下日志
                string strLog = string.Format("银行类型请求串:{0},通过gb2312转义结果:{1}", requestString, answer);
                LogHelper.LogInfo(strLog, "GetHBDetailFromRelay");
            }
            catch (Exception err)
            {
                string error = "调用relay服务前失败" + err.Message;
                LogHelper.LogInfo(error);
                throw new Exception(error);
            }
            if (answer == "")
            {
                return null;
            }
            string strMsg = null;
            int nAll = 0;
            return CommQuery.ParseRelayPageRowNum(answer, out strMsg,out nAll);
        }

        /// <summary>
        /// 
        /// 解析字符串到DataSet  接口返回字符串格式
        /// </summary>
        /// <param name="inmsg"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public static DataSet GetHBDetailFromRelay(string requestString, string serviceCode, string relayIP = "", int relayPort = 0, bool encrypt = false, bool invisible = false, string relayDefaultSPId = "")
        {
           string answer = "";
           try
            {
                var relayResponse = RelayHelper.CommunicateWithRelay(requestString, serviceCode, encrypt, invisible, relayIP, relayPort, relayDefaultSPId);
                answer = Encoding.UTF8.GetString(relayResponse.ResponseBuffer);
                //记录下日志
                string strLog = string.Format("红包详情请求串:{0},通过UTF8转义结果:{1}", requestString, answer);
                LogHelper.LogInfo(strLog, "GetHBDetailFromRelay");
            }
            catch (Exception err)
            {
                string error = "调用relay服务前失败" + err.Message;
                LogHelper.LogInfo(error);
                throw new Exception(error);
            }
            if (answer == "")
            {
                return null;
            }
            string strMsg = null;
            return CommQuery.ParseHBDetailStr(answer, out strMsg);
        }


        /// <summary>
        /// 
        /// 解析字符串到DataSet  接口返回字符串格式
        /// </summary>
        /// <param name="inmsg"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public static DataSet GetHQDataFromRelay(string requestString, string serviceCode, string relayIP = "", int relayPort = 0, bool encrypt = false, bool invisible = false, string relayDefaultSPId = "")
        {

            string answer = "";
            try
            {

                var relayResponse = RelayHelper.CommunicateWithRelay(requestString, serviceCode, encrypt, invisible, relayIP, relayPort, relayDefaultSPId);
                answer = Encoding.UTF8.GetString(relayResponse.ResponseBuffer);
                //记录下日志
                string strLog = string.Format("红包数据请求串:{0},通过UTF8转义结果:{1}",requestString,answer);
                LogHelper.LogInfo(strLog, "GetHQDataFromRelay");
            }
            catch (Exception err)
            {
                string error = "调用relay服务前失败" + err.Message;
                LogHelper.LogInfo(error);
                throw new Exception(error);
            }
            DataSet ds = null;
            if (answer == "")
            {
                return null;
            }
            string Msg = "";
            //解析
             ds = CommQuery.ParseRelayStr(answer, out Msg);
            if (Msg != "")
            {
                throw new Exception(Msg);
            }
            DataSet hbds = new DataSet();
            string strMsg = null;
            DataTable dt = CommQuery.ParseHQHBDataSet(ds, out strMsg);

            if (dt != null)
            {
                hbds.Tables.Add(dt);
            }
            else
            {
                LogHelper.LogInfo(strMsg, "GetHQDataFromRelay-dt= null");
            }
            return hbds;
                
        }
        /// <summary>
        /// 
        /// 调用relay接口，接口返回xml格式，requestString只需传接口特性参数，不传ver=1&head_u=&sp_id=等
        /// 
        /// 解析XML:result=0&res_info=ok&rec_info=<root><recode>0</recode><total>0</total><ret_num>0</ret_num></root>
        /// 到DataSet  
        /// </summary>
        /// <param name="inmsg"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public static DataSet GetDSFromRelayFromXML(string requestString, string serviceCode, string relayIP = "", int relayPort = 0, bool encrypt = false, bool invisible = false, string relayDefaultSPId = "")
        {
            string Msg = "";
            string answer = RelayInvoke(requestString, serviceCode, encrypt, invisible, relayIP, relayPort, relayDefaultSPId);
            DataSet ds = null;
            if (answer == "")
            {
                return null;
            }

            //解析
            ds = CommQuery.PaseRelayXml(answer, out Msg);
            if (Msg != "")
            {
                throw new Exception(Msg);
            }
            return ds;
        }



        /// <summary>
        /// 调relay接口，解析字符串到DataSet,多行记录，result=0&row1=&row2=...格式字符串解析
        /// </summary>
        /// <param name="inmsg"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        /// 
        public static DataSet GetDSFromRelayRowNumNew(out int totalNum, string requestString, string serviceCode, string relayIP = "", int relayPort = 0, bool encrypt = false, bool invisible = false, string relayDefaultSPId = "")
        {
            string Msg = "";
            string answer = RelayInvoke(requestString, serviceCode, encrypt, invisible, relayIP, relayPort,"",relayDefaultSPId);
            DataSet ds = null;
            if (answer == "")
            {
                totalNum = 0;
                return null;
            }

            totalNum = 0;
            //解析
            ds = CommQuery.ParseRelayPageRowNum(answer, out Msg, out totalNum);
            if (Msg != "")
            {
                throw new Exception(Msg);
            }
            return ds;
        }

        /// <summary>
        /// 调relay接口，解析字符串到DataSet,多行记录，result=0&row1=&row2=...格式字符串解析
        /// </summary>
        /// <param name="inmsg"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        /// 
        public static DataSet GetDSFromRelayRowNum(out int totalNum, string requestString, string serviceCode, string relayIP = "", int relayPort = 0, bool encrypt = false, bool invisible = false, string relayDefaultSPId = "")
        {
            string Msg = "";
            string answer = RelayInvoke(requestString, serviceCode, encrypt, invisible, relayIP, relayPort, relayDefaultSPId);
            DataSet ds = null;
            if (answer == "")
            {
                totalNum = 0;
                return null;
            }

             totalNum = 0;
            //解析
            ds = CommQuery.ParseRelayPageRowNum(answer, out Msg,out totalNum);
            if (Msg != "")
            {
                throw new Exception(Msg);
            }
            return ds;
        }

        /// <summary>
        /// 调用Relay接口 "row0=&row1=" 格式字符串解析
        /// </summary>
        /// <param name="requestString"></param>
        /// <param name="serviceCode"></param>
        /// <param name="relayIP"></param>
        /// <param name="relayPort"></param>
        /// <param name="encrypt"></param>
        /// <param name="invisible"></param>
        /// <param name="relayDefaultSPId"></param>
        /// <returns></returns>
        public static DataSet GetDSFromRelayRowNumStartWithZero( string requestString, string serviceCode, string relayIP = "", int relayPort = 0, bool encrypt = false, bool invisible = false, string relayDefaultSPId = "")
        {
            string Msg = "";
            string answer = RelayInvoke(requestString, serviceCode, encrypt, invisible, relayIP, relayPort, relayDefaultSPId);
            DataSet ds = null;
            if (answer == "")
            {
                return null;
            }

            //解析
            ds = CommQuery.ParseRelayPageRowNum0(answer, out Msg);
            if (Msg != "")
            {
                throw new Exception(Msg);
            }
            return ds;
        }

        /// <summary>
        /// 特殊
        /// 调relay接口，inmsg为请求源串，包含所有请求参数，解析xml格式到DataSet
        /// </summary>
        /// <param name="inmsg"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public static DataSet GetDSFromRelayFromXML(string inmsg, string ip, int port, string coding = "")
        {
            string Msg = "";
            string answer = RelayInvoke(inmsg, ip, port, coding);// Replace("?/", "</");//解决解析报错问题
            //   string answer = RelayInvoke(inmsg, ip, port);
            DataSet ds = null;
            if (answer == "")
            {
                return null;
            }

            //解析
            ds = CommQuery.PaseRelayXml(answer, out Msg);
            if (Msg != "")
            {
                throw new Exception(Msg);
            }
            return ds;
        }
    }
}
