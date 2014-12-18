using System;
using Ice;
using BackProcessInvoke;
//using TENCENT.OSS.C2C.Finance.DataAccess;

namespace BackProcessClientCS
{
	/// <summary>
	/// ICE调用客户端代理类
	/// </summary>
	public class ICEInvoke : IDisposable		
	{
		private string serverIP = "172.25.38.9";
		private int serverPort = 6644;  

		private Ice.Communicator  ic = null;
		private IProcessInvokePrx register = null;

		private string lastError = "";

		/// <summary>
		/// 构造类
		/// </summary>
		public ICEInvoke()
		{
			
		}

		/// <summary>
		/// 释放资源
		/// </summary>
		public void Dispose()
		{
			Finit();
		}

		/// <summary>
		/// 初始化ICE
		/// </summary>
		/// <param name="ServerIP">ICE Server端IP</param>
		/// <param name="ServerPort">ICE Server端端口</param>
		/// <returns>初始化是否成功，如果不成功，请用GetLastError函数</returns>
		public bool Init(string ServerIP, int ServerPort)
		{
			try
			{
				if(ic != null)
				{
					ic.destroy();
					ic = null;				
				}

				serverIP = ServerIP;
				serverPort = ServerPort;

				string initStr = "ProcessInvoke:tcp -h " + serverIP + " -p " + serverPort;

				String [] args = {"BackProcessInvoke"};
				ic = Ice.Util.initialize(ref args);

				Ice.ObjectPrx obj = ic.stringToProxy(initStr);

				register = IProcessInvokePrxHelper.checkedCast(obj);
				if (register == null) 
				{
					lastError = "调用ICE出错，register == null";
					return false;
				}

				return true;
			}
			catch(System.Exception err)
			{
				lastError = err.Message;

				if(ic != null)
				{
					register = null;
					ic.destroy();
					ic = null;				
				}

				return false;
			}
		}

		/// <summary>
		/// 释放资源
		/// </summary>
		public void Finit()
		{
			if(ic != null)
			{
				register = null;
				ic.destroy();
				ic = null;					
			}
		}

		/// <summary>
		/// 获取最后一次错误信息
		/// </summary>
		/// <returns>返回最后一次错误信息</returns>
		public string GetLastError()
		{
			return lastError;
		}

		/// <summary>
		/// 核心middle调用的封装代理函数
		/// </summary>
		/// <param name="serviceName">服务名称</param>
		/// <param name="requestMsg">请求串</param>
		/// <param name="serverflag">服务标志，没有请用0</param>
		/// <param name="timeOut">超时值</param>
		/// <param name="strUser">用户名</param>
		/// <param name="strPwd">密码</param>
		/// <param name="strReplyInfo">返回执行结果</param>
		/// <param name="iResult">返回错误代码</param>
		/// <param name="Msg">返回错误信息</param>
		/// <returns></returns>
		public bool middleInvoke(string serviceName,string requestMsg,int serverflag, int timeOut, string strUser, string strPwd,
			out string strReplyInfo,out int iResult,out string Msg)
		{
			

			iResult = -1;
			strReplyInfo = "调用前";
			Msg = "";

			if(ic == null || register == null)
			{
				lastError = "请先初始化ICE，再进行调用。";
				return false;
			}

			try
			{
				bool result = register.MiddleInvoke(serviceName,requestMsg,serverflag,timeOut,strUser,strPwd,out strReplyInfo,out iResult,out Msg);
				return result;

			}
			catch(System.Exception err)
			{
				Msg = err.Message;

				return false;
			}
		}

		/// <summary>
		/// 3.0调用的代理函数
		/// </summary>
		/// <param name="transListNo">单号</param>
		/// <param name="sequenceNo">序列号（如果为""表示不校验是不是本次发起）</param>
		/// <param name="cmdNo">业务命令</param>
		/// <param name="middleNo">系统号</param>
		/// <param name="request">请求参数，是字段和值的组合,是转义之后的</param>
		/// <param name="iResult"> 通讯正常返回0, 接口应答保存在respond中，接口业务返回码从respond中取； 否则返回-1,错误信息保存在respond中</param>
		/// <param name="respond">应答，类似"result=0&res_info=ok"</param>
		/// <returns>调用是否成功</returns>
		public bool InvokeV30(string transListNo,string sequenceNo,int cmdNo,int middleNo,string request,out int iResult, out string respond)
		{
			iResult = -1;
			respond = "调用前";

			if(ic == null || register == null)
			{
				lastError = "请先初始化ICE，再进行调用。";
				return false;
			}

			try
			{
				iResult = register.InvokeV30(transListNo,sequenceNo,cmdNo,middleNo,request,out respond);

				return true;
			}
			catch(System.Exception err)
			{
				return false;
			}			
		}


		public bool InvokeQuery(int iSourceType,int iSourceCmd,string strKey,int iMiddleNo,int iParaLen,string strPara
			,out int iRespLen,out string strResp,out int iResult)
		{
			iResult = -1;
			iRespLen = 0;
			strResp = "调用前";

			if(ic == null || register == null)
			{
				lastError = "请先初始化ICE，再进行调用。";
				return false;
			}

			try
			{
				iResult = register.InvokeQuery(iSourceType,iSourceCmd,strKey,iMiddleNo,iParaLen,strPara,out iRespLen,out strResp);
				return true;
			}
			catch(System.Exception err)
			{
				return false;
			}			
		}
	}
}
