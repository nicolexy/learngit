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
            RelayRequest relayReq = new RelayRequest() { RequestString = relayRequestString };
            var relayResponse = RelayHelper.CommunicateWithRelay(relayReq, true, relayIP, relayPort);
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
            LogHelper.LogInfo("CFT.CSOMS.DAL.Infrastructure.RelayAccessFactory    public static string RelayInvoke(string requestString, string serviceCode, bool encrypt = false, bool invisible = false, string relayIP, int relayPort = 0, string coding, string relayDefaultSPId) ，relayIP="+relayIP +","+ relayPort);
            try
            {
                if(encrypt)
                    LogHelper.LogInfo("加密前特性参数串："+requestString);

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
                string error = "调用relay服务前失败:" + err;
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
                RelayRequest relayReq = new RelayRequest() { RequestString = requestString };
                var relayResponse = RelayHelper.CommunicateWithRelay(relayReq, serviceCode, encrypt, invisible, relayIP, relayPort, relayDefaultSPId);
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
#if DEBUG
            answer ="result=0&res_info=ok&name_auth_status=1&pass_gov_auth=1&pass_bank_auth=1&bind_count=0&authen_channel_state=12325&authen_account_type=3&authen_channel_sum=6&modify_ver=9&pass_ocr_auth=0&white_list_expire=0000-00-00 00:00:00&bind_bank=COMM|CEB&gov_authen_info=F9EC9CCC1EC35CA43484B8E828BAF70D2B6875ED4EFA221E474E1B0D1E66F09D5A023BA8585C927F7ADE3955765603944C61E51EDC291CF8CE8ACC92ECB998BFE7BFE9B78481D0F8E7701065ADAD431D5002C7FF6B7478BF&mobile_authen_info=CE8ACC92ECB998BFF3551E0C0ADD9D0C6A5390126513C9629276E9B01EBE14F5859F43F071F15ABD94338D2FA2798765D72722693B8F7578";
#endif
            DataSet ds = null;
            if (answer == "")
            {
                return null;
            }

            //解析
            ds = CommQuery.ParseRelayStr(answer, out Msg);
            if (Msg != "")
            {
                throw new Exception("relayIP:"+relayIP+" 请求串：" + requestString + " " + Msg);
            }
            return ds;
        }

        /// <summary>
        /// 特殊：请求串加密，返回串不加密的接口
        /// 解析字符串到DataSet  接口返回字符串格式
        /// </summary>
        /// <param name="inmsg"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public static DataSet GetDSFromRelayAnsNotEncr(string paramsStr, string serviceCode, string relayIP = "", int relayPort = 0,  string coding = "", bool invisible = false, string relayDefaultSPId = "")
        {
            string Msg = "";
            paramsStr = RequestStringHelperEx.MiddleRequestEncrypt(paramsStr, true, "");
            paramsStr = "request_type="+serviceCode+"&ver=1&head_u=&sp_id=20000000000&" + paramsStr;
            string answer = RelayAccessFactory.RelayInvoke(paramsStr, relayIP, relayPort, coding, invisible, relayDefaultSPId); 
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
        /// 特殊：请求串加密，返回串不加密的接口(并且返回串允许result!=0)
        /// 解析字符串到DataSet  接口返回字符串格式
        /// </summary>
        /// <param name="inmsg"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public static DataSet GetDSFromRelayAnsNotEncr2(string paramsStr, string serviceCode, string relayIP = "", int relayPort = 0, string coding = "", bool invisible = false, string relayDefaultSPId = "")
        {
            string Msg = "";
            paramsStr = RequestStringHelperEx.MiddleRequestEncrypt(paramsStr, true, "");
            paramsStr = "request_type=" + serviceCode + "&ver=1&head_u=&sp_id=20000000000&" + paramsStr;
            string answer = RelayAccessFactory.RelayInvoke(paramsStr, relayIP, relayPort, coding, invisible, relayDefaultSPId);           
            DataSet ds = null;
            if (answer == "")
            {
                return null;
            }

            //解析
            ds = CommQuery.ParseRelayStr(answer, out Msg, true);
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
                RelayRequest relayReq = new RelayRequest() { RequestString = requestString };
                var relayResponse = RelayHelper.CommunicateWithRelay(relayReq, serviceCode, encrypt, invisible, relayIP, relayPort, relayDefaultSPId);
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
                RelayRequest relayReq = new RelayRequest() { RequestString = requestString };
                var relayResponse = RelayHelper.CommunicateWithRelay(relayReq, serviceCode, encrypt, invisible, relayIP, relayPort, relayDefaultSPId);
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
                RelayRequest relayReq = new RelayRequest() { RequestString = requestString };
                var relayResponse = RelayHelper.CommunicateWithRelay(relayReq, serviceCode, encrypt, invisible, relayIP, relayPort, relayDefaultSPId);
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
                throw new Exception("请求串：" + requestString + " " + Msg);
            }
            return ds;
        }

        public static DataSet GetDSFromRelayFromXMLHasTotalCount(string requestString, string serviceCode,out int total_num, string relayIP = "", int relayPort = 0, bool encrypt = false, bool invisible = false, string relayDefaultSPId = "")
        {
            string Msg = "";
            string answer = RelayInvoke(requestString, serviceCode, encrypt, invisible, relayIP, relayPort, relayDefaultSPId);
            DataSet ds = null;
            if (answer == "")
            {
                total_num = 0;
                return null;
            }

            //解析
            ds = CommQuery.PaseRelayXml(answer, out Msg,out total_num);
            if (Msg != "")
            {
                throw new Exception("请求串：" + requestString + " " + Msg);
            }
            return ds;
        }

        /// <summary>
        /// 
        /// 调用relay接口，接口返回xml格式，requestString只需传接口特性参数，不传ver=1&head_u=&sp_id=等 兼容乱码
        /// 
        /// 解析XML:result=0&res_info=ok&rec_info=<root><recode>0</recode><total>0</total><ret_num>0</ret_num></root>
        /// 到DataSet  
        /// </summary>
        /// <param name="inmsg"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public static DataSet GetDSFromRelayFromXML1(string requestString, string serviceCode, string relayIP = "", int relayPort = 0, bool encrypt = false, bool invisible = false, string relayDefaultSPId = "")
        {
            string Msg = "";
            string answer = RelayInvoke(requestString, serviceCode, encrypt, invisible, relayIP, relayPort, relayDefaultSPId);
            //answer = "result=0&res_info=ok&rec_info=<root><recode>0</recode><total>24</total><ret_num>20</ret_num><record><FlistId>1000062801201601270072008723</FlistId><Fdraw_state>0</Fdraw_state><Fstate>3</Fstate><Ftotal_fee>1</Ftotal_fee><Fprice>1</Fprice><Fcharge_fee>0</Fcharge_fee><Fpayer_uin>3135976280</Fpayer_uin><Fpayer_name>3135976280</Fpayer_name><Fseller_uin>475965921</Fseller_uin><Fseller_name>江%3d湖|夜%26amp;雨</Fseller_name><Fcreate_time>2016-01-27 17:16:31</Fcreate_time><Fmodify_time>2016-01-27 17:16:31</Fmodify_time><Fmemo></Fmemo><Ftransfer_listid>1000062801101601270072008723</Ftransfer_listid><Fsave_listid>1000062801201601270482274857</Fsave_listid></record><record><FlistId>1000026901201511162077384296</FlistId><Fdraw_state>0</Fdraw_state><Fstate>3</Fstate><Ftotal_fee>1</Ftotal_fee><Fprice>1</Fprice><Fcharge_fee>0</Fcharge_fee><Fpayer_uin>443586985</Fpayer_uin><Fpayer_name>鏉ㄥ皬浼?/Fpayer_name><Fseller_uin>475965921</Fseller_uin><Fseller_name>dickyfyang鏉ㄤ箟...</Fseller_name><Fcreate_time>2015-11-16 12:36:07</Fcreate_time><Fmodify_time>2015-11-16 12:36:07</Fmodify_time><Fmemo></Fmemo><Ftransfer_listid></Ftransfer_listid><Fsave_listid>1000026901201511160396321448</Fsave_listid></record><record><FlistId>1000026901201511162077377904</FlistId><Fdraw_state>0</Fdraw_state><Fstate>3</Fstate><Ftotal_fee>1</Ftotal_fee><Fprice>1</Fprice><Fcharge_fee>0</Fcharge_fee><Fpayer_uin>280373895</Fpayer_uin><Fpayer_name>寮犺偛娣?/Fpayer_name><Fseller_uin>475965921</Fseller_uin><Fseller_name>涔夊嚒</Fseller_name><Fcreate_time>2015-11-16 12:33:04</Fcreate_time><Fmodify_time>2015-11-16 12:33:04</Fmodify_time><Fmemo></Fmemo><Ftransfer_listid></Ftransfer_listid><Fsave_listid>1000026901201511160396318529</Fsave_listid></record><record><FlistId>1000026901201511112065646955</FlistId><Fdraw_state>0</Fdraw_state><Fstate>3</Fstate><Ftotal_fee>1797</Ftotal_fee><Fprice>1797</Fprice><Fcharge_fee>0</Fcharge_fee><Fpayer_uin>280373895</Fpayer_uin><Fpayer_name>张育淼</Fpayer_name><Fseller_uin>475965921</Fseller_uin><Fseller_name>义凡</Fseller_name><Fcreate_time>2015-11-11 13:30:12</Fcreate_time><Fmodify_time>2015-11-11 13:30:12</Fmodify_time><Fmemo>全部家当给你</Fmemo><Ftransfer_listid></Ftransfer_listid><Fsave_listid>1000026901201511110390572956</Fsave_listid></record><record><FlistId>1000026901201511052052207058</FlistId><Fdraw_state>0</Fdraw_state><Fstate>3</Fstate><Ftotal_fee>1</Ftotal_fee><Fprice>1</Fprice><Fcharge_fee>0</Fcharge_fee><Fpayer_uin>374504528</Fpayer_uin><Fpayer_name>陈姗</Fpayer_name><Fseller_uin>475965921</Fseller_uin><Fseller_name>dickyfyang(杨...</Fseller_name><Fcreate_time>2015-11-05 20:11:45</Fcreate_time><Fmodify_time>2015-11-05 20:11:45</Fmodify_time><Fmemo></Fmemo><Ftransfer_listid></Ftransfer_listid><Fsave_listid>1000026901201511050383968892</Fsave_listid></record><record><FlistId>1000026901201511052052205992</FlistId><Fdraw_state>0</Fdraw_state><Fstate>3</Fstate><Ftotal_fee>1</Ftotal_fee><Fprice>1</Fprice><Fcharge_fee>0</Fcharge_fee><Fpayer_uin>374504528</Fpayer_uin><Fpayer_name>陈姗</Fpayer_name><Fseller_uin>475965921</Fseller_uin><Fseller_name>dickyfyang(杨...</Fseller_name><Fcreate_time>2015-11-05 20:11:15</Fcreate_time><Fmodify_time>2015-11-05 20:11:15</Fmodify_time><Fmemo></Fmemo><Ftransfer_listid></Ftransfer_listid><Fsave_listid>1000026901201511050383968325</Fsave_listid></record><record><FlistId>1000026901201511052052204030</FlistId><Fdraw_state>0</Fdraw_state><Fstate>3</Fstate><Ftotal_fee>1</Ftotal_fee><Fprice>1</Fprice><Fcharge_fee>0</Fcharge_fee><Fpayer_uin>374504528</Fpayer_uin><Fpayer_name>陈姗</Fpayer_name><Fseller_uin>475965921</Fseller_uin><Fseller_name>dickyfyang(杨...</Fseller_name><Fcreate_time>2015-11-05 20:10:27</Fcreate_time><Fmodify_time>2015-11-05 20:10:27</Fmodify_time><Fmemo></Fmemo><Ftransfer_listid></Ftransfer_listid><Fsave_listid>1000026901201511050383967255</Fsave_listid></record><record><FlistId>1000026901201511052052183278</FlistId><Fdraw_state>0</Fdraw_state><Fstate>3</Fstate><Ftotal_fee>1</Ftotal_fee><Fprice>1</Fprice><Fcharge_fee>0</Fcharge_fee><Fpayer_uin>443586985</Fpayer_uin><Fpayer_name>杨小伟</Fpayer_name><Fseller_uin>475965921</Fseller_uin><Fseller_name>dickyfyang杨...</Fseller_name><Fcreate_time>2015-11-05 20:01:53</Fcreate_time><Fmodify_time>2015-11-05 20:01:53</Fmodify_time><Fmemo></Fmemo><Ftransfer_listid></Ftransfer_listid><Fsave_listid>1000026901201511050383956002</Fsave_listid></record><record><FlistId>1000026901201511052052181353</FlistId><Fdraw_state>0</Fdraw_state><Fstate>3</Fstate><Ftotal_fee>1</Ftotal_fee><Fprice>1</Fprice><Fcharge_fee>0</Fcharge_fee><Fpayer_uin>443586985</Fpayer_uin><Fpayer_name>杨小伟</Fpayer_name><Fseller_uin>475965921</Fseller_uin><Fseller_name>dickyfyang杨...</Fseller_name><Fcreate_time>2015-11-05 20:01:04</Fcreate_time><Fmodify_time>2015-11-05 20:01:04</Fmodify_time><Fmemo></Fmemo><Ftransfer_listid></Ftransfer_listid><Fsave_listid>1000026901201511050383954995</Fsave_listid></record><record><FlistId>1000026901201511052052144490</FlistId><Fdraw_state>0</Fdraw_state><Fstate>3</Fstate><Ftotal_fee>1</Ftotal_fee><Fprice>1</Fprice><Fcharge_fee>0</Fcharge_fee><Fpayer_uin>374504528</Fpayer_uin><Fpayer_name>陈姗</Fpayer_name><Fseller_uin>475965921</Fseller_uin><Fseller_name>dickyfyang(杨...</Fseller_name><Fcreate_time>2015-11-05 19:45:17</Fcreate_time><Fmodify_time>2015-11-05 19:45:17</Fmodify_time><Fmemo></Fmemo><Ftransfer_listid></Ftransfer_listid><Fsave_listid>1000026901201511050383934809</Fsave_listid></record><record><FlistId>1000026901201511052052071330</FlistId><Fdraw_state>0</Fdraw_state><Fstate>3</Fstate><Ftotal_fee>1</Ftotal_fee><Fprice>1</Fprice><Fcharge_fee>0</Fcharge_fee><Fpayer_uin>80080124</Fpayer_uin><Fpayer_name>吴镇权</Fpayer_name><Fseller_uin>475965921</Fseller_uin><Fseller_name>dickyfyang杨...</Fseller_name><Fcreate_time>2015-11-05 19:12:51</Fcreate_time><Fmodify_time>2015-11-05 19:12:51</Fmodify_time><Fmemo></Fmemo><Ftransfer_listid></Ftransfer_listid><Fsave_listid>1000026901201511050383895693</Fsave_listid></record><record><FlistId>1000026901201508311917563831</FlistId><Fdraw_state>0</Fdraw_state><Fstate>3</Fstate><Ftotal_fee>100</Ftotal_fee><Fprice>100</Fprice><Fcharge_fee>0</Fcharge_fee><Fpayer_uin>280373895</Fpayer_uin><Fpayer_name>张育淼</Fpayer_name><Fseller_uin>475965921</Fseller_uin><Fseller_name>义凡</Fseller_name><Fcreate_time>2015-08-31 17:54:45</Fcreate_time><Fmodify_time>2015-08-31 17:54:45</Fmodify_time><Fmemo></Fmemo><Ftransfer_listid></Ftransfer_listid><Fsave_listid>1000026901201508310323169901</Fsave_listid></record><record><FlistId>1000026901201508221903998389</FlistId><Fdraw_state>0</Fdraw_state><Fstate>3</Fstate><Ftotal_fee>100</Ftotal_fee><Fprice>100</Fprice><Fcharge_fee>0</Fcharge_fee><Fpayer_uin>1453167654</Fpayer_uin><Fpayer_name>郑欢圆</Fpayer_name><Fseller_uin>475965921</Fseller_uin><Fseller_name>老杨</Fseller_name><Fcreate_time>2015-08-22 17:02:32</Fcreate_time><Fmodify_time>2015-08-22 17:02:32</Fmodify_time><Fmemo></Fmemo><Ftransfer_listid></Ftransfer_listid><Fsave_listid>1000026901201508220317168224</Fsave_listid></record><record><FlistId>1000026901201507271865076512</FlistId><Fdraw_state>0</Fdraw_state><Fstate>3</Fstate><Ftotal_fee>5700</Ftotal_fee><Fprice>5700</Fprice><Fcharge_fee>0</Fcharge_fee><Fpayer_uin>443586985</Fpayer_uin><Fpayer_name>杨小伟</Fpayer_name><Fseller_uin>475965921</Fseller_uin><Fseller_name>dickyfyang杨...</Fseller_name><Fcreate_time>2015-07-27 10:05:15</Fcreate_time><Fmodify_time>2015-07-27 10:05:15</Fmodify_time><Fmemo></Fmemo><Ftransfer_listid></Ftransfer_listid><Fsave_listid>1000026901201507270299505469</Fsave_listid></record><record><FlistId>1000026901201505211785901590</FlistId><Fdraw_state>0</Fdraw_state><Fstate>3</Fstate><Ftotal_fee>1</Ftotal_fee><Fprice>1</Fprice><Fcharge_fee>0</Fcharge_fee><Fpayer_uin>28329423</Fpayer_uin><Fpayer_name>柯文锋</Fpayer_name><Fseller_uin>475965921</Fseller_uin><Fseller_name>杨义凡</Fseller_name><Fcreate_time>2015-05-21 17:13:12</Fcreate_time><Fmodify_time>2015-05-21 17:13:13</Fmodify_time><Fmemo></Fmemo><Ftransfer_listid></Ftransfer_listid><Fsave_listid>1000026901201505210266426010</Fsave_listid></record><record><FlistId>1000026901201505181781119044</FlistId><Fdraw_state>0</Fdraw_state><Fstate>3</Fstate><Ftotal_fee>2000</Ftotal_fee><Fprice>2000</Fprice><Fcharge_fee>0</Fcharge_fee><Fpayer_uin>85461847</Fpayer_uin><Fpayer_name>柯资颖</Fpayer_name><Fseller_uin>475965921</Fseller_uin><Fseller_name>dickyfyang杨...</Fseller_name><Fcreate_time>2015-05-18 13:00:27</Fcreate_time><Fmodify_time>2015-05-18 13:00:27</Fmodify_time><Fmemo></Fmemo><Ftransfer_listid></Ftransfer_listid><Fsave_listid>1000026901201505180264182473</Fsave_listid></record><record><FlistId>1000026901201504281763972952</FlistId><Fdraw_state>0</Fdraw_state><Fstate>3</Fstate><Ftotal_fee>100000</Ftotal_fee><Fprice>100000</Fprice><Fcharge_fee>0</Fcharge_fee><Fpayer_uin>93346872</Fpayer_uin><Fpayer_name>宋智刚</Fpayer_name><Fseller_uin>475965921</Fseller_uin><Fseller_name>杨义凡</Fseller_name><Fcreate_time>2015-04-28 20:40:51</Fcreate_time><Fmodify_time>2015-04-28 20:40:51</Fmodify_time><Fmemo></Fmemo><Ftransfer_listid></Ftransfer_listid><Fsave_listid>1000026901201504280257164036</Fsave_listid></record><record><FlistId>1000026901201503031715504436</FlistId><Fdraw_state>0</Fdraw_state><Fstate>3</Fstate><Ftotal_fee>1300</Ftotal_fee><Fprice>1300</Fprice><Fcharge_fee>0</Fcharge_fee><Fpayer_uin>26906391</Fpayer_uin><Fpayer_name>谭金文</Fpayer_name><Fseller_uin>475965921</Fseller_uin><Fseller_name>dickyfyang杨...</Fseller_name><Fcreate_time>2015-03-03 18:42:56</Fcreate_time><Fmodify_time>2015-03-03 18:42:57</Fmodify_time><Fmemo>吃面</Fmemo><Ftransfer_listid></Ftransfer_listid><Fsave_listid>1000026901201503030240177632</Fsave_listid></record><record><FlistId>1000026901201503021714126527</FlistId><Fdraw_state>0</Fdraw_state><Fstate>3</Fstate><Ftotal_fee>1300</Ftotal_fee><Fprice>1300</Fprice><Fcharge_fee>0</Fcharge_fee><Fpayer_uin>26906391</Fpayer_uin><Fpayer_name>谭金文</Fpayer_name><Fseller_uin>475965921</Fseller_uin><Fseller_name>dickyfyang杨...</Fseller_name><Fcreate_time>2015-03-02 13:28:00</Fcreate_time><Fmodify_time>2015-03-02 13:28:01</Fmodify_time><Fmemo>吃面</Fmemo><Ftransfer_listid></Ftransfer_listid><Fsave_listid>1000026901201503020239835936</Fsave_listid></record><record><FlistId>1000026901201502081683201917</FlistId><Fdraw_state>0</Fdraw_state><Fstate>3</Fstate><Ftotal_fee>956</Ftotal_fee><Fprice>956</Fprice><Fcharge_fee>0</Fcharge_fee><Fpayer_uin>443586985</Fpayer_uin><Fpayer_name>杨小伟</Fpayer_name><Fseller_uin>475965921</Fseller_uin><Fseller_name>杨义凡</Fseller_name><Fcreate_time>2015-02-08 09:46:08</Fcreate_time><Fmodify_time>2015-02-08 09:46:09</Fmodify_time><Fmemo></Fmemo><Ftransfer_listid></Ftransfer_listid><Fsave_listid>1000026901201502080230885466</Fsave_listid></record></root>";
            DataSet ds = null;
            if (answer == "")
            {
                return null;
            }

            try
            {
                //解析
                ds = CommQuery.PaseRelayXml(answer, out Msg);
            }
            catch
            {
                answer = answer.Replace("?/", "</");
                ds = CommQuery.PaseRelayXml(answer, out Msg);
            }

            if (Msg != "")
            {
                throw new Exception("请求串：" + requestString + " " + Msg);
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
                throw new Exception("请求串：" + requestString + " " + Msg);
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
                throw new Exception("请求串：" + requestString + " " + Msg);
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
                throw new Exception("请求串：" + requestString + " " + Msg);
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
                throw new Exception("请求串：" + inmsg + " " + Msg);
            }
            return ds;
        }

        /// <summary>
        /// 调relay接口，解析字符串到DataSet,多行记录，
        /// 格式：result=0&res_info=ok&total_num=n&paramOne_0=&paramTwo_0=&paramOne_1=&paramTwo_1=...&paramOne_n=&paramTwo_n=
        /// </summary>
        /// <param name="inmsg"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        /// 
        public static DataSet GetDSFromRelayMethod1(string requestString, string serviceCode, string relayIP = "", int relayPort = 0, bool encrypt = false, bool invisible = false, string relayDefaultSPId = "")
       {
            string Msg = "";
            string answer = RelayInvoke(requestString, serviceCode, encrypt, invisible, relayIP, relayPort, relayDefaultSPId);
            DataSet ds = null;
            if (answer == "")
            {
                return null;
            }

            //解析
            ds = CommQuery.ParseRelayPageMethod1(answer, out Msg);
            if (Msg != "")
            {
                throw new Exception(Msg);
            }
            return ds;
        }
        /// <summary>
        /// 调relay接口，解析字符串到DataSet,多行记录，
        /// 格式：result=0&res_info=ok&total_num=n&paramOne_0=&paramTwo_0=&paramOne_1=&paramTwo_1=...&paramOne_n=&paramTwo_n=
        /// 并且允许resualt!=0
        /// </summary>
        public static DataSet GetDSFromRelayMethod2(string requestString, string serviceCode, string relayIP = "", int relayPort = 0, bool encrypt = false, bool invisible = false, string relayDefaultSPId = "")
        {
            string Msg = "";
            string answer = RelayInvoke(requestString, serviceCode, encrypt, invisible, relayIP, relayPort, relayDefaultSPId);
            DataSet ds = null;
            if (answer == "")
            {
                return null;
            }

            //解析
            ds = CommQuery.ParseRelayPageMethod1(answer, out Msg,true);
            if (Msg != "")
            {
                throw new Exception(Msg);
            }
            return ds;
        }
    }
}
