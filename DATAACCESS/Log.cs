/************************************************************
  FileName: Log.cs
  Author: Furion       Version :    1.0      Date:2005-06-01
  Description: 日志类，可以记录操作日志，异常日志，效率检测。    // 模块描述      
  Version:         // 版本信息
  Function List:   // 主要函数及其功能
    1. WriteLog      记录日常操作日志。
    2. WriteErrLog   记录异常日志。
    3. WriteStart    函数开始计时。
    4. WriteEnd      函数计时结束。
  History:         // 历史修改记录
      <author>  <time>   <version >   <desc>
      Furion  2005-06-01     1.0     build this moudle  
***********************************************************/

using System;
using TENCENT.OSS.CFT.KF.DataAccess;
using System.Text;

namespace TENCENT.OSS.CFT.KF.Common
{
	/// <summary>
	/// 日志操作静态类，参数很多，所以各系统在使用时要进行进一步封装。如在PublicRes.CS中。
	/// </summary>
    public class LogManage
    {
        private string fIP; //远程服务器IP
        private int fPort;  //远程服务器服务端口.

        public LogManage(string ServerIP, int ServerPort)
        {
            fIP = ServerIP;
            fPort = ServerPort;
        }

        private bool WriteLog(string FileType,string Suin,string SystemId, string Level, string ErrorId, string Content)
        {
            TWriteLogIn wli = new TWriteLogIn(FileType);
            wli.Suin = Suin;
            wli.SystemId = SystemId;
            wli.Level = Level;
            wli.ErrorId = ErrorId;
            wli.Content = Content;
            
            return  UDP.SendUDP(wli.ToBytes(),fIP,fPort);
        }


        /// <summary>
        /// 记录日志到文件
        /// </summary>
        /// <param name="Suin">操作的QQ号码或者操作人昵称</param>
        /// <param name="SystemId">记录系统的标识,如支付server记录pay_server</param>
        /// <param name="Level">记录日志级别,分为以下几种info,debug,warning,error</param>
        /// <param name="ErrorId">记录错误ID号,便于查询与统计</param>
        /// <param name="Content">错误内容,长度小于512byte</param>
        /// <returns></returns>
        public bool Log(string Suin,string SystemId, string Level, string ErrorId, string Content)
        {
            return WriteLog("file",Suin,SystemId,Level,ErrorId,Content);
        }

        /// <summary>
        /// 记录日志到数据库
        /// </summary>
        /// <param name="Suin">操作的QQ号码或者操作人昵称</param>
        /// <param name="SystemId">记录系统的标识,如支付server记录pay_server</param>
        /// <param name="Level">记录日志级别,分为以下几种info,debug,warning,error</param>
        /// <param name="ErrorId">记录错误ID号,便于查询与统计</param>
        /// <param name="Content">错误内容,长度小于512byte</param>
        /// <returns></returns>
        public bool DBLog(string Suin,string SystemId, string Level, string ErrorId, string Content)
        {
            return WriteLog("table",Suin,SystemId,Level,ErrorId,Content);
        }
    }


    public class TWriteLogIn
    {
        private string LogType; //11
        public string Suin; //长度13byte,记录操作的QQ号码或者操作人昵称
        public string SystemId; //长度11byte,记录系统的标识,如支付server记录pay_server.
        public string Level; //长度11byte,记录日志级别,分为以下几种info,debug,warning,error
        public string ErrorId; //长度11byte, 记录错误ID号,便于查询与统计
        public string Content; //记录错误内容,长度小于512byte

        public TWriteLogIn(string logType)
        {
            LogType = logType;
        }

        public byte[] ToBytes()
        {
            int ilen = 0;
            int bytelen = 569;

            byte[] result = new byte[bytelen];

            byte[] tmp = Encoding.GetEncoding("GB2312").GetBytes(LogType);
            tmp.CopyTo(result,0);
            ilen = 11;

            tmp = Encoding.GetEncoding("GB2312").GetBytes(Suin);
            tmp.CopyTo(result,ilen );
            ilen = ilen + 13;

            tmp = Encoding.GetEncoding("GB2312").GetBytes(SystemId);
            tmp.CopyTo(result,ilen);
            ilen = ilen + 11;

            tmp = Encoding.GetEncoding("GB2312").GetBytes(Level);
            tmp.CopyTo(result, ilen);
            ilen = ilen + 11;

            tmp = Encoding.GetEncoding("GB2312").GetBytes(ErrorId);
            tmp.CopyTo(result,ilen );
            ilen = ilen + 11;

            tmp = Encoding.GetEncoding("GB2312").GetBytes(Content);
            tmp.CopyTo(result,ilen);

            return result;
        }
    }

}
