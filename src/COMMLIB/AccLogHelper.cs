using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using log4net;
using commLib;
using System.Threading.Tasks;
using CFT.Apollo.Logging;
using CFT.Apollo.Common.Configuration;

namespace TENCENT.OSS.C2C.Finance.Common.CommLib
{

    /// <summary>
    /// 接口名称，数字代替
    /// 1、提现查询接口
    /// 2、 疑似延迟查询接口
    /// 3、 注销接口
    /// 4、冻结
    /// 5、解冻
    /// </summary>
    public enum AccService
    {
        /// <summary>
        /// 提现查询接口
        /// </summary>
        GETPAYLIST=1,
        /// <summary>
        /// 疑似延迟查询接口
        /// </summary>
        RELAYSEARCH=2,
        /// <summary>
        /// 注销接口
        /// </summary>
        LOGOUT = 3,
        /// <summary>
        /// 冻结
        /// </summary>
        FREEZE = 4,
        /// <summary>
        /// 解冻
        /// </summary>
        UNFREEZE = 5
    }

    /// <summary>
    /// 结果类型:数字0-3 （0成功，1超时，2系统错误，3应用错误）
    /// </summary>
    public enum AccLogResult
    {
        SUCCESS = 0,
        TIMEOUT = 1,
        SYSTEMERROR = 2,
        APPLICATIONERROR = 3
    }

    /// <summary>
    /// （错误码）:自定义的返回码（字符串32位内）
    /// </summary>
    public enum AccReturnCode
    {
        SUCCESS = 200,
        BIGMONEYSUCCESS=2001,
        NORESULT = 4004,
        CREATEFREEZEID = 4001,
        EXCEPTION = 5000,
        REQUESTSERVICEERROR = 5004

    }

    /// <summary>
    /// ACC日志处理类
    /// </summary>
    public class AccLogHelper
    {
        /// <summary>
        /// 接入系统的服务标识名称
        /// </summary>
        private readonly static string SystemIdentity = AppSettings.Get<string>("CftAgentSystemIdentity", "KFService"); 
        /// <summary>
        /// CftAgentLog的服务IP
        /// </summary>
        private readonly static string ServerAddress = AppSettings.Get<string>("CftAgentIP", "10.132.131.146");
        /// <summary>
        /// CftAgentLog的服务端口（udp）
        /// </summary>
        private readonly static int ServerPort = AppSettings.Get<int>("CftAgentPort",6164);
        /// <summary>
        /// CftAgentLog报文头日志类型
        /// </summary>
        private readonly static int AgentLogType =  AppSettings.Get<int>("CftAgentLogType",1);
        /// <summary>
        /// 请求报文头，约定格式
        /// </summary>
        private readonly static byte[] UdpHead = new byte[12] { 1, 0, 0, 0, Convert.ToByte(AgentLogType), 0, 0, 0, 1, 0, 0, 0 };//报文头，针对Agent的报文头，业务约定(Convert.ToByte在255及以下支持这种写法)

        /// <summary>
        /// 上报ACC监控日志到CftAgentLog（UDP）
        /// </summary>
        /// <param name="content"></param>
        /// <param name="outMsg"></param>
        /// <returns></returns>
        private static bool Send(string content, out string outMsg)
        {
            outMsg = "";
            bool result = false;
            try
            {
                //构建要发送的二进制数组
                byte[] msgBytes = Encoding.GetEncoding("gbk").GetBytes(content);//要发送的业务内容，必须gbk编码
                byte[] sendBytes = new byte[UdpHead.Length + msgBytes.Length];//最终发送的数据，格式为：报文头+内容
                UdpHead.CopyTo(sendBytes, 0);//把报文头填充到sendBytes
                msgBytes.CopyTo(sendBytes, UdpHead.Length);//把内容填充到sendBytes

                //发送UDP包
                UdpClient udpClient = new UdpClient(ServerAddress, ServerPort);
                udpClient.Send(sendBytes, sendBytes.Length);

                result = true;
            }
            catch (Exception ex)
            {
                WriteLog("上报ACC监控日志,异常：" + ex.ToString());
                outMsg = string.Format("AccLogHelper.Send异常.{0}", ex.ToString());
            }
            return result;
        }

        /// <summary>
        /// 异步发送支付日志到财付通监控系统(推荐用法)
        /// </summary>
        /// <param name="msgNo"></param>
        /// <param name="FlistId"></param>
        /// <param name="codeFile"></param>
        /// <param name="codeLine"></param>
        /// <param name="service"></param>
        /// <param name="accResult"></param>
        /// <param name="returnCode"></param>
        /// <param name="returnPackage"></param>
        /// <param name="timeSpent"></param>
        /// <param name="outMsg"></param>
        /// <param name="payBankType"></param>
        /// <param name="ncNo"></param>
        /// <param name="ncIp"></param>
        /// <param name="ncInfo"></param>
        /// <returns></returns>
        public static void SendPayLogAsync(string reqIp, string msgNo, string FlistId, string codeFile, int codeLine, AccService service, AccLogResult accResult, AccReturnCode returnCode, string returnPackage, long timeSpent, string payBankType, string ncNo, string ncIp, string ncInfo)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    string outMsg = "";
                    if (!SendPayLog(reqIp, msgNo, FlistId, codeFile, codeLine, service, accResult, returnCode, returnPackage, timeSpent, out outMsg, payBankType, ncNo, ncIp, ncInfo))
                    {
                        WriteLog(string.Format("记录Acc日志失败(msgNo={0}).返回信息：{1}", msgNo, outMsg),true);
                    }
                }
                catch (Exception ex)
                {
                    WriteLog(string.Format("记录Acc日志失败(msgNo={0}).{1}", msgNo, ex.ToString()),true);
                }
            });
        }

        /// <summary>
        /// 发送支付日志到财付通监控系统
        /// </summary>
        /// <param name="msgNo"></param>
        /// <param name="FlistId"></param>
        /// <param name="codeFile"></param>
        /// <param name="codeLine"></param>
        /// <param name="accResult"></param>
        /// <param name="returnCode"></param>
        /// <param name="returnPackage"></param>
        /// <param name="timeSpent"></param>
        /// <param name="outMsg"></param>
        /// <param name="ex1"></param>
        /// <param name="ex2"></param>
        /// <param name="ex3"></param>
        /// <param name="ex4"></param>
        /// <param name="ex5"></param>
        /// <param name="ex6"></param>
        /// <param name="ex7"></param>
        /// <param name="ex8"></param>
        /// <param name="msgNo"></param>
        /// <returns></returns>
        private static bool SendPayLog(string reqIp, string msgNo, string FlistId, string codeFile, int codeLine, AccService service, AccLogResult accResult, AccReturnCode returnCode, string returnPackage, long timeSpent, out string outMsg, string ex1 = "", string ex2 = "", string ex3 = "", string ex4 = "", string ex5 = "", string ex6 = "", string ex7 = "", string ex8 = "")
        {
            #region 字段说明
            //系统日志标识|FILE|LINE|时间|pid|srcip|dstip|dstport|server| service(cgi)|MSG_NO(染色ID )| QQ号|单号|商户号|扩展key1|扩展key2|扩展key3|扩展key4|扩展key5|扩展key6|扩展key7|扩展key8|处理时耗|结果类型|返回值（错误码）|返回包|请求包|保留字段1(text)| 保留字段2(text)|保留字段3(text)|

            //字段说明：
            //系统日志标识:ZWDirectPayDP
            //FILE:代码所在文件（如:abc.cs）
            //LINE:代码行
            //时间:yyyy-MM-dd HH:mm:ss
            //pid:空 线程ID
            //srcip:空（后台服务为空）
            //dstip:当前系统部署的服务器IP
            //dstport:空（后台服务为空）
            //server:ZWDirectPayDP
            //service(cgi):1-3（1付款2回导3付款并回导）
            //MSG_NO(染色ID ):唯一ID
            //QQ号:空
            //单号:提现单ID（FlistId）
            //商户号:空
            //扩展key1:PayBankType（银行类型）
            //扩展key2:NcNo（NC编号）
            //扩展key3:NcIp（NCIP）
            //扩展key4:NcInfo（NC名称）
            //扩展key5:空
            //扩展key6:空
            //扩展key7:空
            //扩展key8:空
            //处理时耗:从开始到结束时间（毫秒）
            //结果类型:数字0-3 （0成功，1超时，2系统错误，3应用错误）
            //返回值（错误码）:自定义的返回码（字符串32位内）
            //返回包:银行返回码+银行返回信息
            //请求包:空
            //保留字段1(text):空
            //保留字段2(text):空
            //保留字段3(text):空

            #endregion

            bool result = false;
            outMsg = "";

            StringBuilder logBuilder = new StringBuilder();

            try
            {
                logBuilder.Append(SystemIdentity).Append("|");//系统日志标识
                logBuilder.Append(codeFile).Append("|");//FILE
                logBuilder.Append(codeLine).Append("|");//LINE
                logBuilder.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).Append("|");//时间
                logBuilder.Append("|");//pid
                logBuilder.Append("|");//srcip
                logBuilder.Append(reqIp).Append("|");//dstip
                logBuilder.Append("|");//dstport
                logBuilder.Append(SystemIdentity).Append("|");//server
                logBuilder.Append(((int)service).ToString()).Append("|");// service(cgi)
                logBuilder.Append(msgNo).Append("|");//MSG_NO(染色ID )
                logBuilder.Append("|");//QQ号
                logBuilder.Append(FlistId).Append("|");//单号
                logBuilder.Append("|");//商户号
                logBuilder.Append(ex1.Replace("|", "!&")).Append("|");//扩展key1
                logBuilder.Append(ex2.Replace("|", "!&")).Append("|");//扩展key2
                logBuilder.Append(ex3.Replace("|", "!&")).Append("|");//扩展key3
                logBuilder.Append(ex4.Replace("|", "!&")).Append("|");//扩展key4
                logBuilder.Append(ex5.Replace("|", "!&")).Append("|");//扩展key5
                logBuilder.Append(ex6.Replace("|", "!&")).Append("|");//扩展key6
                logBuilder.Append(ex7.Replace("|", "!&")).Append("|");//扩展key7
                logBuilder.Append(ex8.Replace("|", "!&")).Append("|");//扩展key8
                logBuilder.Append(timeSpent).Append("|");//处理时耗
                logBuilder.Append(((int)accResult).ToString()).Append("|");//结果类型
                logBuilder.Append(((int)returnCode).ToString()).Append("|");//返回值（错误码）
                logBuilder.Append(returnPackage.Replace("|", "!&")).Append("|");//返回包
                logBuilder.Append("|");//请求包
                logBuilder.Append("|");//保留字段1(text)
                logBuilder.Append("|");//保留字段2(text)
                logBuilder.Append("|");//保留字段3(text)


                WriteLog("发送数据AccLog:" + logBuilder.ToString());//记录Acc日志

                result = Send(logBuilder.ToString(), out outMsg);
            }
            catch (Exception ex)
            {
                outMsg = string.Format("SendPayLog.{0}.报文:{1}", ex.ToString(), logBuilder.ToString());
            }
            return result;
        }


        private static void WriteLog(string msg, bool isError = false)
        {
            if (isError)
            {
                LogHelper.LogError(msg);
            }
            else
            {
                LogHelper.LogInfo(msg);
            }
        }

        /// <summary>
        /// 获取调用该方法所在行
        /// </summary>
        /// <returns></returns>
        public static int GetLineNum()
        {
            try
            {
                System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
                return st.GetFrame(0).GetFileLineNumber();
            }
            catch {
                return 0;
            }
        }
    }
}
