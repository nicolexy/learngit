using System;
using Ice;
using BackProcessInvoke;
//using TENCENT.OSS.C2C.Finance.DataAccess;

namespace BackProcessClientCS
{
	/// <summary>
	/// ICE���ÿͻ��˴�����
	/// </summary>
	public class ICEInvoke : IDisposable		
	{
		private string serverIP = "172.25.38.9";
		private int serverPort = 6644;  

		private Ice.Communicator  ic = null;
		private IProcessInvokePrx register = null;

		private string lastError = "";

		/// <summary>
		/// ������
		/// </summary>
		public ICEInvoke()
		{
			
		}

		/// <summary>
		/// �ͷ���Դ
		/// </summary>
		public void Dispose()
		{
			Finit();
		}

		/// <summary>
		/// ��ʼ��ICE
		/// </summary>
		/// <param name="ServerIP">ICE Server��IP</param>
		/// <param name="ServerPort">ICE Server�˶˿�</param>
		/// <returns>��ʼ���Ƿ�ɹ���������ɹ�������GetLastError����</returns>
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
					lastError = "����ICE����register == null";
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
		/// �ͷ���Դ
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
		/// ��ȡ���һ�δ�����Ϣ
		/// </summary>
		/// <returns>�������һ�δ�����Ϣ</returns>
		public string GetLastError()
		{
			return lastError;
		}

		/// <summary>
		/// ����middle���õķ�װ������
		/// </summary>
		/// <param name="serviceName">��������</param>
		/// <param name="requestMsg">����</param>
		/// <param name="serverflag">�����־��û������0</param>
		/// <param name="timeOut">��ʱֵ</param>
		/// <param name="strUser">�û���</param>
		/// <param name="strPwd">����</param>
		/// <param name="strReplyInfo">����ִ�н��</param>
		/// <param name="iResult">���ش������</param>
		/// <param name="Msg">���ش�����Ϣ</param>
		/// <returns></returns>
		public bool middleInvoke(string serviceName,string requestMsg,int serverflag, int timeOut, string strUser, string strPwd,
			out string strReplyInfo,out int iResult,out string Msg)
		{
			

			iResult = -1;
			strReplyInfo = "����ǰ";
			Msg = "";

			if(ic == null || register == null)
			{
				lastError = "���ȳ�ʼ��ICE���ٽ��е��á�";
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
		/// 3.0���õĴ�����
		/// </summary>
		/// <param name="transListNo">����</param>
		/// <param name="sequenceNo">���кţ����Ϊ""��ʾ��У���ǲ��Ǳ��η���</param>
		/// <param name="cmdNo">ҵ������</param>
		/// <param name="middleNo">ϵͳ��</param>
		/// <param name="request">������������ֶκ�ֵ�����,��ת��֮���</param>
		/// <param name="iResult"> ͨѶ��������0, �ӿ�Ӧ�𱣴���respond�У��ӿ�ҵ�񷵻����respond��ȡ�� ���򷵻�-1,������Ϣ������respond��</param>
		/// <param name="respond">Ӧ������"result=0&res_info=ok"</param>
		/// <returns>�����Ƿ�ɹ�</returns>
		public bool InvokeV30(string transListNo,string sequenceNo,int cmdNo,int middleNo,string request,out int iResult, out string respond)
		{
			iResult = -1;
			respond = "����ǰ";

			if(ic == null || register == null)
			{
				lastError = "���ȳ�ʼ��ICE���ٽ��е��á�";
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
			strResp = "����ǰ";

			if(ic == null || register == null)
			{
				lastError = "���ȳ�ʼ��ICE���ٽ��е��á�";
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
